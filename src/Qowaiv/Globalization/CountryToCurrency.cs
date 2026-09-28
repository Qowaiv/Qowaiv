using Qowaiv.Financial;
using System.Collections.ObjectModel;
using static Qowaiv.Financial.Currency;
using static Qowaiv.Globalization.Country;

namespace Qowaiv.Globalization;

internal readonly partial record struct CountryToCurrency(Country Country, Currency Currency, Date StartDate = default)
{
    [Pure]
    private static IEnumerable<CountryToCurrency> New(Country country, Currency currency, int year = 0, int month = 0, int day = 0)
        => [new(country, currency, year is 0 ? default : new Date(year, month, day))];
}

file static class Extensions
{
    [Pure]
    public static IEnumerable<CountryToCurrency> And(this IEnumerable<CountryToCurrency> mappings, Currency currency, int year = 0, int month = 0, int day = 0) =>
    [
        .. mappings,
        new(mappings.FirstOrDefault().Country, currency, year is 0 ? default : new Date(year, month, day)),
    ];
}
