#pragma warning disable S3898
// Value types should implement "IEquatable<T>"
// Equals of this type is never called.

using Qowaiv.Financial;
using System;

namespace Qowaiv.Globalization
{
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
        public override int GetHashCode()
        {
            return Country.GetHashCode()
                ^ Currency.GetHashCode()
                ^ StartDate.GetHashCode();
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is CountryToCurrency other && Equals(other);

        /// <inheritdoc />
        public bool Equals(CountryToCurrency other)
        {
            return Country.Equals(other.Country)
                && Currency.Equals(other.Currency)
                && StartDate.Equals(other.StartDate);
        }
    }
}
