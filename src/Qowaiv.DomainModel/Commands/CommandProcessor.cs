using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Qowaiv.DomainModel.Commands;

/// <summary>Command processor that resolves the registered command handler
/// base on the type of the command.
/// </summary>
/// <typeparam name="TReturnType">
/// The return type of the command handler methods.
/// </typeparam>
/// <remarks>
/// It compiles (one time per command type) the expression to handle the
/// command on the command handler.
/// </remarks>
public abstract class CommandProcessor<TReturnType>
{
    /// <summary>Gets the command types sent at least once by the command processor.</summary>
    public IReadOnlyCollection<object> CommandTypes => [..handlers.Keys];

    /// <summary>The generic type definition of the command handlers to support.</summary>
    /// <remarks>
    /// Something like `typeof(CommandHandler&lt;&gt;)`.
    /// </remarks>
    protected abstract Type GenericHandlerType { get; }

    /// <summary>The name of the method defined on the generic interface.</summary>
    /// <remarks>
    /// Something like "Handle", or "HandleAsync".
    /// </remarks>
    protected abstract string HandlerMethod { get; }

    /// <summary>Gets the command handler to send the command to.</summary>
    /// <param name="handlerType">
    /// The resolved command handler type to get the instance of.
    /// </param>
    /// <remarks>
    /// This is the method that could/should hook on to your DI of choice.
    /// </remarks>
    [Pure]
    protected abstract object GetHandler(Type handlerType);

    /// <summary>Sends the command to the registered command handler and
    /// communicates its response.
    /// </summary>
    /// <param name="command">
    /// The command that should be handled by some command handler.
    /// </param>
    /// <returns>
    /// The response of the registered command handler.
    /// </returns>
    [Impure]
    public TReturnType Send(object command) => Send(command, token: default);

    /// <summary>Sends the command to the registered command handler and
    /// communicates its response.
    /// </summary>
    /// <param name="command">
    /// The command that should be handled by some command handler.
    /// </param>
    /// <param name="token">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// The response of the registered command handler.
    /// </returns>
    [Impure]
    public TReturnType Send(object command, CancellationToken token)
    {
        var commandType = Guard.NotNull(command).GetType();
        var handlerType = HandlerTypeFor(commandType);
        var handler = GetHandler(handlerType) ?? throw UnresolvedCommandHandler.Type(handlerType);
        return Handle(handlerType, commandType)(handler, command, token);
    }

    /// <summary>Gets function to invoke.</summary>
    [Impure]
    private Func<object, object, CancellationToken, TReturnType> Handle(Type handlerType, Type commandType)
    {
        if (!handlers.TryGetValue(commandType, out var handle))
        {
            var method = GetMethod(handlerType, commandType)
                ?? throw UnresolvedCommandHandler.Method(typeof(TReturnType), handlerType, HandlerMethod, commandType);
            handle = GetExpression(method).Compile();
            handlers[commandType] = handle;
        }
        return handle;
    }

    /// <summary>Gets the generic interface for the specific command handler.</summary>
    /// <param name="commandType">
    /// The type of the command to process.
    /// </param>
    [Pure]
    private Type HandlerTypeFor(Type commandType) => GenericHandlerType.MakeGenericType(commandType);

    /// <summary>Gets the <see cref="MethodInfo"/> for the handler method to call.</summary>
    [Pure]
    private MethodInfo? GetMethod(Type handlerType, Type commandType)
        => handlerType.GetMethods().TryFind(m => WithCancelationToken(m, commandType))
        ?? handlerType.GetMethods().TryFind(m => WithoutCancelationToken(m, commandType));

    [Pure]
    private bool WithCancelationToken(MethodInfo method, Type commandType)
        => method.GetParameters().Length == 2
        && method.GetParameters()[1].ParameterType == typeof(CancellationToken)
        && Matches(method, commandType);

    [Pure]
    private bool WithoutCancelationToken(MethodInfo method, Type commandType)
      => method.GetParameters().Length == 1
      && Matches(method, commandType);

    [Pure]
    private bool Matches(MethodInfo method, Type commandType)
        => method.Name == HandlerMethod
        && method.ReturnType == typeof(TReturnType)
        && method.GetParameters()[0].ParameterType == commandType;

    /// <summary>Gets an expression that calls the Handle method.</summary>
    /// <remarks>
    /// (handler, cmd, token) => ((HandlerType)handler).{HandlerMethod}((CommandType)cmd, token);
    ///
    /// or if the token can not be consumed by the handler.
    ///
    /// <code>
    /// (handler, cmd, token) => ((HandlerType)handler).{HandlerMethod}((CommandType)cmd);
    /// </code>
    /// </remarks>
    [Pure]
    private static Expression<Func<object, object, CancellationToken, TReturnType>> GetExpression(MethodInfo method)
    {
        var handler = Expression.Parameter(typeof(object), "handler");
        var cmd = Expression.Parameter(typeof(object), "cmd");
        var token = Expression.Parameter(typeof(CancellationToken), "token");
        var typedCommand = Expression.Convert(cmd, method.GetParameters()[0].ParameterType);
        var typedHandler = Expression.Convert(handler, method.DeclaringType!);
        var body = method.GetParameters().Length == 1
            ? Expression.Call(typedHandler, method, typedCommand)
            : Expression.Call(typedHandler, method, typedCommand, token);

        return Expression.Lambda<Func<object, object, CancellationToken, TReturnType>>(body, handler, cmd, token);
    }

    private readonly ConcurrentDictionary<Type, Func<object, object, CancellationToken, TReturnType>> handlers = new();
}
