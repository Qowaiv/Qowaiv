using Qowaiv.Financial;

namespace Qowaiv.Globalization;

internal partial struct CountryToCurrency : IEquatable<CountryToCurrency>
{
    public CountryToCurrency(Country country, Currency currency, Date startdate)
    {
        Country = country;
        Currency = currency;
        StartDate = startdate;
    }

    public CountryToCurrency(Country country, Currency currency)
        : this(country, currency, Date.MinValue) { }

    public Country Country { get; }
    public Currency Currency { get; }
    public Date StartDate { get; }

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode()
        => Country.GetHashCode()
        ^ Currency.GetHashCode()
        ^ StartDate.GetHashCode();

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is CountryToCurrency other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(CountryToCurrency other)
        => Country.Equals(other.Country)
        && Currency.Equals(other.Currency)
        && StartDate.Equals(other.StartDate);
}
