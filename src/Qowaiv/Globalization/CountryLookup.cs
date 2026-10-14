namespace Qowaiv.Globalization;

[DebuggerDisplay("Count = {DebuggerDisplay.Length}")]
internal readonly struct CountryLookup<T>(params T[] lookup) : IEnumerable<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly T[] Lookup = lookup;

    public T this[CountryId id] => Lookup[(int)id];

    public T this[Country country] => Lookup[(int)CountryId.New(country)];

    /// <inheritdoc cref="IDictionary{TKey, TValue}" />
    [Pure]
    public bool TryGetValue(Country country, [NotNullWhen(true)] out T? instance)
        => TryGetValue(CountryId.New(country), out instance);

    /// <inheritdoc cref="IDictionary{TKey, TValue}" />
    [Pure]
    public bool TryGetValue(CountryId id, [NotNullWhen(true)] out T? instance)
    {
        if ((int)id < Lookup.Length && this[id] is T found)
        {
            instance = found;
            return true;
        }
        else
        {
            instance = default;
            return false;
        }
    }

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
