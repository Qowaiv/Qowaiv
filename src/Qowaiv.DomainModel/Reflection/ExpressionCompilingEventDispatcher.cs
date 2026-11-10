using System.Linq.Expressions;
using System.Reflection;

namespace Qowaiv.DomainModel.Reflection;

/// <summary>A dynamic event dispatcher, is a extremely limited dynamic object
/// that is capable of invoking instance methods with the signature When(@event).
/// </summary>
/// <typeparam name="TDispatcher">
/// The type of the non dynamic event dispatcher.
/// </typeparam>
/// <remarks>
/// The constraints on the method:
/// * Name of the method is 'When'
/// * Binding is on instance (both public and non-public)
/// * One parameter with a type that is/could be an event.
/// * Return type is ignored.
///
/// It caches the available methods per type.
/// </remarks>
[Inheritable]
public class ExpressionCompilingEventDispatcher<TDispatcher>(TDispatcher dispatcher) : EventDispatcher
    where TDispatcher : class
{
    private readonly TDispatcher dispatcher = Guard.NotNull(dispatcher, nameof(dispatcher));

    /// <inheritdoc />
    public void When(object? @event)
    {
        if (@event is { } && Lookup.TryGetValue(@event.GetType(), out var when))
        {
            when(dispatcher, @event);
        }
    }

    /// <summary>Gets the supported event types.</summary>
    public ReadOnlySet<Type> SupportedEventTypes => Supported!;

    /// <summary>Initializes all When(@event) methods.</summary>
    [Pure]
    private static Dictionary<Type, Action<TDispatcher, object>> Init()
    {
        var lookup = new Dictionary<Type, Action<TDispatcher, object>>();

        foreach (var method in WhenMethods())
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 1)
            {
                var parameterType = parameters[0].ParameterType;

                // Leave out object itself, primitives and enumerations.
                if (parameterType != typeof(object) &&
                    !parameterType.IsPrimitive &&
                    !parameterType.IsEnum)
                {
                    lookup[parameterType] = Compile(method, parameterType);
                }
            }
        }
        Supported = new ReadOnlySet<Type>(lookup.Keys);
        return lookup;
    }

    /// <summary>Uses reflection to resolve `When` methods to use,
    /// both non-public (mostly internal) and public.
    /// </summary>
    [Pure]
    private static IEnumerable<MethodInfo> WhenMethods()
        => typeof(TDispatcher)
        .GetMethods(MethodSignature)
        .Where(method => method.Name == nameof(When));

    [Pure]
    private static Action<TDispatcher, object> Compile(MethodInfo method, Type eventType)
    {
        var dispatcherParam = Expression.Parameter(typeof(TDispatcher), "dispatcher");
        var @event = Expression.Parameter(typeof(object), "e");

        var typedEvent = Expression.Convert(@event, eventType);
        var body = Expression.Call(dispatcherParam, method, typedEvent);

        var expression = Expression.Lambda<Action<TDispatcher, object>>(body, dispatcherParam, @event);

        return expression.Compile();
    }

    private static readonly Dictionary<Type, Action<TDispatcher, object>> Lookup = Init();
#pragma warning disable S2743 // Static fields should not be used in generic types
    // Intended behavior, should only be shared between the same dispatcher types.
    private static ReadOnlySet<Type>? Supported;
#pragma warning restore S2743 // Static fields should not be used in generic types
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    /// <remarks>The whole point is to find non-public When methods.</remarks>
    private const BindingFlags MethodSignature = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
}
