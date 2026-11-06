#if !NETSTANDARD2_0

using Qowaiv.Globalization;

namespace Qowaiv.Financial;

public readonly partial struct Currency
{
    /// <inheritdoc cref="ExistsOnDate(Date)" />
    [Pure]
    public bool ExistsOnDate(DateOnly measurement) => ExistsOnDate((Date)measurement);

    /// <inheritdoc cref="GetCountries(Date)" />
    [Pure]
    public IEnumerable<Country> GetCountries(DateOnly measurement) => GetCountries((Date)measurement);

    /// <inheritdoc cref="GetExisting(Date)" />
    [Pure]
    public static IEnumerable<Currency> GetExisting(DateOnly measurement) => GetExisting((Date)measurement);
}
#endif
