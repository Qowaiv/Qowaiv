#if NETSTANDARD2_0
namespace System.Collections.Frozen;

[ExcludeFromCodeCoverage/*(Justification = "Fallback for lacking frozen set, but is just a wrapper arround a hashset.")*/]
[DebuggerDisplay("Count = {Count}")]
internal sealed class FrozenSet<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer) : ICollection<T> where T: notnull
{
    /// <summary>Gets an empty <see cref="FrozenSet{T}"/>.</summary>
    public static FrozenSet<T> Empty { get; } = new FrozenSet<T>([], EqualityComparer<T>.Default);

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly HashSet<T> Set = new(collection, comparer);

    /// <inheritdoc />
    public int Count => Set.Count;

    /// <inheritdoc />
    public bool IsReadOnly => true;

    /// <inheritdoc />
    [Pure]
    public bool Contains(T item) => Set.Contains(item);

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex) => Set.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    [Pure]
    public IEnumerator<T> GetEnumerator() => Set.GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [DoesNotReturn]
    public void Add(T item) => throw new NotSupportedException();

    /// <inheritdoc />
    [DoesNotReturn]
    public void Clear() => throw new NotSupportedException();

    /// <inheritdoc />
    [DoesNotReturn]
    public bool Remove(T item) => throw new NotSupportedException();
}
#endif
