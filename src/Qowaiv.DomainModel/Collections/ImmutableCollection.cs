namespace Qowaiv.DomainModel.Collections;

/// <summary>Represents an immutable collection.</summary>
/// <remarks>
/// As a design choice, adding null is ignored. Also <see cref="IEnumerable"/>s
/// are added as collections, expect for <see cref="string"/>.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(CollectionDebugView))]
public sealed class ImmutableCollection : IReadOnlyCollection<object>
{
    /// <summary>Gets an empty immutable collection.</summary>
    public static readonly ImmutableCollection Empty = new(AppendOnlyCollection.Empty);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly AppendOnlyCollection Items;

    /// <summary>Initializes a new instance of the <see cref="ImmutableCollection"/> class.</summary>
    internal ImmutableCollection(AppendOnlyCollection items) => Items = items;

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
    public ImmutableCollection Add(object? item)
        => new(Items.Add(item));

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

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()" />
    [Pure]
    public Enumerator GetEnumerator() => Items.GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator<object> IEnumerable<object>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>Adds an item to the collection.</summary>
    /// <remarks>
    /// Syntactic sugar.
    /// </remarks>
    public static ImmutableCollection operator +(ImmutableCollection collection, object item)
        => Guard.NotNull(collection).Add(item);
}
