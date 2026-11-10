namespace Qowaiv.DomainModel.Collections;

/// <summary>Represents the conditional addition of an item/items, after the if-statement.</summary>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(CollectionDebugView))]
public sealed class Then : IReadOnlyCollection<object>
{
    /// <summary>Initializes a new instance of the <see cref="Then"/> class.</summary>
    internal Then(bool done, AppendOnlyCollection item)
    {
        Items = item;
        Done = done;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly AppendOnlyCollection Items;

    /// <summary>The predecessor <see cref="ImmutableCollection"/>.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool Done { get; }

    /// <inheritdoc />
    public int Count => Items.Count;

    /// <summary>Creates a new <see cref="ImmutableCollection"/> with the added item(s).</summary>
    /// <param name="item">
    /// The item(s) to add.
    /// </param>
    /// <remarks>
    /// Null, and null items are ignored.
    /// </remarks>
    [Pure]
    public ImmutableCollection Add(object? item) => new(Items.Add(item));

    /// <summary>Creates a new <see cref="ImmutableCollection"/> with the added items.</summary>
    /// <remarks>
    /// Null, and null items are ignored.
    /// </remarks>
    [Pure]
    public ImmutableCollection AddRange(params object?[]? items) => Add(items);

    /// <summary>Starts a conditional addition.</summary>
    [Pure]
    public If If(bool? condition) => If(condition == true);

    /// <summary>Starts a conditional addition.</summary>
    [Pure]
    public If If(bool condition) => new(condition, Items);

    /// <summary>Adds else-if condition.</summary>
    [Pure]
    public If ElseIf(bool condition)
        => Done
        ? new If(IfState.Done, Items)
        : new If(condition, Items);

    /// <summary>Creates a new <see cref="ImmutableCollection"/> with the item(s) to add
    /// if the condition was not met.</summary>
    /// <param name="item">
    /// The item(s) to add.
    /// </param>
    /// <typeparam name="TElseItem">
    /// The type of the event to add.
    /// </typeparam>
    /// <remarks>
    /// Null, and null items are ignored.
    /// </remarks>
    [Pure]
    public ImmutableCollection Else<TElseItem>(Func<TElseItem> item) where TElseItem : class
        => Done || item is null
        ? new(Items)
        : new(Items.Add(item()));

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()" />
    [Pure]
    public Enumerator GetEnumerator() => Items.GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator<object> IEnumerable<object>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
