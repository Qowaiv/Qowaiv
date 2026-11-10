namespace Qowaiv.DomainModel.Collections;

/// <summary>Represents a read-only set of items.</summary>
/// <typeparam name="T">
/// The type of items.
/// </typeparam>
[Inheritable]
[DebuggerDisplay("Count: {Count}")]
[DebuggerTypeProxy(typeof(CollectionDebugView))]
public class ReadOnlySet<T> : IEnumerable<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly HashSet<T> collection = [];

    /// <summary>Initializes a new instance of the <see cref="ReadOnlySet{T}"/> class.</summary>
    /// <param name="items">
    /// The items it contains.
    /// </param>
    public ReadOnlySet(IEnumerable<T> items)
    {
        Guard.NotNull(items);
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }

    /// <summary>Gets the number of elements that are contained in a set.</summary>
    public int Count => collection.Count;

    /// <summary>Determines whether the set contains the item.</summary>
    [Pure]
    public bool Contains(T item) => collection.Contains(item);

    /// <inheritdoc />
    [Pure]
    public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
