using System.Runtime.CompilerServices;

namespace Qowaiv.DomainModel;

/// <summary>A function to convert the event (payload) from the stored event.</summary>
/// <typeparam name="TId">
/// The type of the identifier of the aggregate.
/// </typeparam>
/// <typeparam name="TStoredEvent">
/// The Type of the stored event.
/// </typeparam>
/// <param name="aggregateId">
/// The identifier of the aggregate.
/// </param>
/// <param name="version">
/// The version of the event.
/// </param>
/// <param name="event">
/// The event (payload).
/// </param>
/// <returns>
/// The converted event.
/// </returns>
[Pure]
public delegate TStoredEvent ConvertToStoredEvent<in TId, out TStoredEvent>(TId aggregateId, long version, object @event);

/// <summary>A buffer of events that should be added to an event stream.</summary>
/// <typeparam name="TId">
/// The type of the identifier of the aggregate.
/// </typeparam>
[Immutable]
[DebuggerDisplay("{DebuggerDisplay}")]
[DebuggerTypeProxy(typeof(CollectionDebugView))]
public readonly struct EventBuffer<TId> : IReadOnlyCollection<object>, ICollection<object>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly AppendOnlyCollection Buffer;
    private readonly long Offset;

    /// <summary>Initializes a new instance of the <see cref="EventBuffer{TId}"/> struct.</summary>
    internal EventBuffer(TId aggregateId, long offset, long committed, AppendOnlyCollection buffer)
    {
        AggregateId = aggregateId;
        CommittedVersion = committed;
        Offset = offset;
        Buffer = buffer;
    }

    /// <summary>Gets the identifier of the aggregate.</summary>
    public TId AggregateId { get; }

    /// <summary>The version of the event buffer.</summary>
    public long Version => Buffer.Count + Offset;

    /// <summary>Gets the committed version of the event buffer.</summary>
    public long CommittedVersion { get; }

    /// <summary>Gets the number of events in the event buffer.</summary>
    /// <remarks>
    /// This number is not the version. It can be lower when the event buffer has
    /// been created using an initial version.
    /// </remarks>
    public int Count => Buffer.Count;

    /// <summary>Get all committed events in the event buffer.</summary>
    public IEnumerable<object> Committed => Buffer.Take(CommittedOffset());

    /// <summary>Get all uncommitted events in the event buffer.</summary>
    public IEnumerable<object> Uncommitted => Buffer.Skip(CommittedOffset());

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CommittedOffset() => checked((int)(CommittedVersion - Offset));

    /// <summary>Returns true if the event buffer contains at least one uncommitted event.</summary>
    public bool HasUncommitted => Version != CommittedVersion;

    /// <summary>Returns true if the buffer contains no events.</summary>
    public bool IsEmpty => Buffer.Count == 0;

    /// <summary>Adds an event/events to the event buffer.</summary>
    /// <param name="event">
    /// The event(s) to add.
    /// </param>
    /// <remarks>
    /// Null, and null items are ignored.
    /// </remarks>
    [Pure]
    public EventBuffer<TId> Add(object @event)
        => new(AggregateId, Offset, CommittedVersion, (IsEmpty ? AppendOnlyCollection.Empty : Buffer).Add(@event));

    /// <summary>Marks all events as being committed.</summary>
    [Pure]
    public EventBuffer<TId> MarkAllAsCommitted()
        => new(AggregateId, Offset, Version, Buffer);

    /// <summary>Selects the uncommitted events.</summary>
    /// <typeparam name="TStoredEvent">
    /// The type to convert it to.
    /// </typeparam>
    /// <param name="convert">
    /// The function to convert a <typeparamref name="TStoredEvent"/> from
    /// the aggregate identifier and the version of the event.
    /// </param>
    /// <returns>
    /// The uncommitted events as <typeparamref name="TStoredEvent"/>.
    /// </returns>
    [Pure]
    public IEnumerable<TStoredEvent> SelectUncommitted<TStoredEvent>(ConvertToStoredEvent<TId, TStoredEvent> convert)
    {
        Guard.NotNull(convert);
        var self = this;
        return Uncommitted.Select((@event, index) => convert(self.AggregateId, self.CommittedVersion + index + 1, @event));
    }

    /// <inheritdoc />
    public void CopyTo(object[] array, int arrayIndex) => Buffer.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    [Pure]
    public bool Contains(object item) => Buffer.Contains(item);

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()" />
    [Pure]
    public Enumerator GetEnumerator() => Buffer.GetEnumerator();

    /// <inheritdoc/>
    [Pure]
    IEnumerator<object> IEnumerable<object>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>Returns a <see cref="string"/> that represents the event buffer for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal string DebuggerDisplay
        => Version == CommittedVersion
        ? $"Version: {Version}, Aggregate: {AggregateId}"
        : $"Version: {Version} (Committed: {CommittedVersion}), Aggregate: {AggregateId}";

    /// <inheritdoc />
    bool ICollection<object>.IsReadOnly => true;

    /// <inheritdoc />
    void ICollection<object>.Add(object item) => throw new NotSupportedException();

    /// <inheritdoc />
    void ICollection<object>.Clear() => throw new NotSupportedException();

    /// <inheritdoc />
    [Pure]
    bool ICollection<object>.Remove(object item) => throw new NotSupportedException();
}
