#if !NETSTANDARD2_0

namespace Qowaiv.Globalization;

public readonly partial struct Country
{
    /// <inheritdoc cref="ExistsOnDate(Date)" />
    [Pure]
    public bool ExistsOnDate(DateOnly measurement) => ExistsOnDate((Date)measurement);

    /// <inheritdoc cref="GetCurrency(Date)" />
    [Pure]
    public Financial.Currency GetCurrency(DateOnly measurement) => GetCurrency((Date)measurement);

    /// <inheritdoc cref="GetExisting(Date)" />
    [Pure]
    public static IEnumerable<Country> GetExisting(DateOnly measurement) => GetExisting((Date)measurement);
}

#endif
