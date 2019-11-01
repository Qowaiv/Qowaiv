#pragma warning disable S3898
// Value types should implement "IEquatable<T>"
// Equals of this type is never called.

using Qowaiv.Financial;

namespace Qowaiv.Globalization
{
    internal partial struct CountryToCurrency
    {
        private Country m_Country;
        private Currency m_Currency;
        private Date m_StartDate;

        public CountryToCurrency(Country country, Currency currency, Date startdate)
        {
            m_Country = country;
            m_Currency = currency;
            m_StartDate = startdate;
        }

        public CountryToCurrency(Country country, Currency currency)
            : this(country, currency, Date.MinValue) { }

        public Country Country => m_Country;
        public Currency Currency => m_Currency;
        public Date StartDate => m_StartDate;
    }
}
