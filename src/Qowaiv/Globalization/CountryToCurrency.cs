using Qowaiv.Financial;

namespace Qowaiv.Globalization;

internal readonly partial struct CountryToCurrency(Country country, Currency currency, Date startDate) : IEquatable<CountryToCurrency>
{
    public CountryToCurrency(Country country, Currency currency)
        : this(country, currency, Date.MinValue) { }

    public Country Country { get; } = country;

    public Currency Currency { get; } = currency;

    public Date StartDate { get; } = startDate;

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
