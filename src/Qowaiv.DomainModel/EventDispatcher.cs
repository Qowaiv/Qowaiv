namespace Qowaiv.DomainModel;

/// <summary>Factory for creating <see cref="ExpressionCompilingEventDispatcher{TDispatcher}"/>'s.</summary>
public interface EventDispatcher
{
    /// <summary>Gets the supported event types.</summary>
    ReadOnlySet<Type> SupportedEventTypes { get; }

    /// <summary>Invokes the When(@event) method.</summary>
    void When(object? @event);
}
