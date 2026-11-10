namespace Qowaiv.DomainModel;

/// <summary>Represents an (domain-driven design) aggregate that is based on event sourcing.</summary>
/// <typeparam name="TAggregate">
/// The type of the aggregate itself.
/// </typeparam>
/// <typeparam name="TId">
/// The type of the identifier.
/// </typeparam>
[Mutable]
public class Aggregate<TAggregate, TId> : Aggregate<TAggregate>
    where TAggregate : Aggregate<TAggregate, TId>, new()
{
    /// <summary>Initializes a new instance of the <see cref="Aggregate{TAggregate, TId}"/> class.</summary>
    /// <param name="validator">
    /// A custom <paramref name="validator"/> to validate the aggregate.
    /// </param>
    /// <param name="aggregateId">
    /// The identifier of the aggregate.
    /// </param>
    protected Aggregate(TId aggregateId, IValidator<TAggregate> validator) : base(validator)
        => Buffer = EventBuffer.Empty(aggregateId);

    /// <summary>Gets the identifier.</summary>
    public TId Id => Buffer.AggregateId;

    /// <summary>Gets the version of aggregate.</summary>
    public long Version => Buffer.Version;

    /// <summary>Gets the buffer with the recently added events.</summary>
    public EventBuffer<TId> Buffer { get; protected set; }

    /// <inheritdoc/>
    protected override void AddEventsToBuffer(IReadOnlyCollection<object> events)
        => Buffer = Buffer.Add(events);

    /// <inheritdoc/>
    [Pure]
    protected override TAggregate Clone()
    {
        var cloned = new TAggregate();
        cloned.Replay(Buffer);
        return cloned;
    }

    /// <summary>Loads the state of the aggregate by replaying events.</summary>
    internal void Replay(EventBuffer<TId> buffer)
    {
        Buffer = buffer;
        base.Replay(buffer);
    }
}
