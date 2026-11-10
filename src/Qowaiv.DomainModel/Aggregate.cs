using System.Reflection;

namespace Qowaiv.DomainModel;

/// <summary>Factory method for creating <see cref="Aggregate{TAggregate, TId}"/> from stored events.</summary>
public static class Aggregate
{
    /// <summary>Creates an <see cref="Aggregate{TAggregate, TId}"/> from committed events.</summary>
    /// <typeparam name="TAggregate">
    /// The type of aggregate to create.
    /// </typeparam>
    /// <typeparam name="TId">
    /// The type of identifier of the aggregate.
    /// </typeparam>
    /// <param name="buffer">
    /// The buffer to replay.
    /// </param>
    [Pure]
    public static TAggregate FromStorage<TAggregate, TId>(EventBuffer<TId> buffer)
        where TAggregate : Aggregate<TAggregate, TId>, new()
    {
        Guard.HasAny(buffer);

        var aggregate = new TAggregate();
        aggregate.Replay(buffer);
        return aggregate;
    }

    /// <inheritdoc cref="EventDispatcher.SupportedEventTypes" />
    [Pure]
    public static ReadOnlySet<Type> SupportedEventTypes<TAggregate>()
        where TAggregate : Aggregate<TAggregate>, new()
    {
        return Dispatcher(new TAggregate()).SupportedEventTypes;

        static EventDispatcher Dispatcher(TAggregate aggregate)
            => (EventDispatcher)typeof(TAggregate)
                .GetProperty(nameof(Dispatcher), NonPublic)!
                .GetValue(aggregate)!;
    }

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    // We own the code.
    private const BindingFlags NonPublic = BindingFlags.Instance | BindingFlags.NonPublic;
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
}
