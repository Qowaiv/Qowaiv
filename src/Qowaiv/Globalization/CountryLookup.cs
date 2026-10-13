namespace Qowaiv.Globalization;

[DebuggerDisplay("Count = {DebuggerDisplay.Length}")]
internal readonly struct CountryLookup<T>(params T[] lookup) : IEnumerable<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly T[] Lookup = lookup;

    public T this[CountryId id] => Lookup[(int)id];

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private KeyValuePair<CountryId, T>[] DebuggerDisplay =>
    [
        .. Lookup
        .Select((item, id) => new KeyValuePair<CountryId, T>(new CountryId(id), item))
        .Where(kvp => !Equals(kvp.Value, default(T)))
    ];

    /// <inheritdoc />
    [Pure]
    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Lookup).GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => Lookup.GetEnumerator();
}
