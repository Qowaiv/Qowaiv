#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

using Qowaiv.Conversion.Financial;
using Qowaiv.Diagnostics;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Json;
using Qowaiv.Text;
using Qowaiv.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Qowaiv.Financial
{
    /// <summary>Represents a currency.</summary>
    /// <remarks>
    /// A currency in the most specific use of the word refers to money in any
    /// form when in actual use or circulation as a medium of exchange,
    /// especially circulating banknotes and coins.
    ///
    /// A much more general use of the word currency is anything that is used
    /// in any circumstances, as a medium of exchange. In this use, "currency"
    /// is a synonym for the concept of money.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable]
    [SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
    [OpenApiDataType(description: "Currency notation as defined by ISO 4217.", example: "EUR", type: "string", format: "currency", nullable: true)]
    [TypeConverter(typeof(CurrencyTypeConverter))]
    public partial struct Currency : ISerializable, IXmlSerializable, IFormattable, IFormatProvider, IEquatable<Currency>, IComparable, IComparable<Currency>
    {
        /// <summary>Represents an empty/not set currency.</summary>
        public static readonly Currency Empty;

        /// <summary>Represents an unknown (but set) currency.</summary>
        public static readonly Currency Unknown = new Currency("ZZZ");

        /// <summary>Gets a currency based on the current thread.</summary>
        public static Currency Current => Thread.CurrentThread.GetValue<Currency>();

        /// <summary>Gets the name of the currency.</summary>
        public string Name => IsUnknown() ? "?" : m_Value ?? string.Empty;

        /// <summary>Gets the display name.</summary>
        public string DisplayName => GetDisplayName(CultureInfo.CurrentCulture);

        /// <summary>Gets the full name of the currency in English.</summary>
        /// <returns>
        /// The full name of the currency in English.
        /// </returns>
        public string EnglishName => GetDisplayName(CultureInfo.InvariantCulture);

        ///<summary>Gets the code defined in ISO 4217 for the currency.</summary>
        public string IsoCode => GetResourceString("ISO", CultureInfo.InvariantCulture);

        ///<summary>Gets the numeric code defined in ISO 4217 for the currency.</summary>
        public int IsoNumericCode => m_Value == default ? 0 : XmlConvert.ToInt32(GetResourceString("Num", CultureInfo.InvariantCulture));

        ///<summary>Gets the symbol for a currency.</summary>
        public string Symbol => m_Value == default ? "" : GetResourceString("Symbol", CultureInfo.InvariantCulture);

        ///<summary>Gets the number of after the decimal separator.</summary>
        public int Digits => m_Value == default ? 0 : XmlConvert.ToInt32(GetResourceString("Digits", CultureInfo.InvariantCulture));

        /// <summary>Gets the start date from witch the currency exists.</summary>
        public Date StartDate => m_Value == default ? Date.MinValue : (Date)XmlConvert.ToDateTime(GetResourceString("StartDate", CultureInfo.InvariantCulture), "yyyy-MM-dd");

        /// <summary>If the currency does not exist anymore, the end date is given, otherwise null.</summary>
        public Date? EndDate
        {
            get
            {
                var val = GetResourceString("EndDate", CultureInfo.InvariantCulture);
                return string.IsNullOrEmpty(val) ? null : (Date?)XmlConvert.ToDateTime(val, "yyyy-MM-dd");
            }
        }

        /// <summary>Gets the display name for a specified culture.</summary>
        /// <param name="culture">
        /// The culture of the display name.
        /// </param>
        /// <returns></returns>
        [Pure]
        public string GetDisplayName(CultureInfo culture) => GetResourceString("DisplayName", culture);

        /// <summary>Returns true if the currency exists at the given date, otherwise false.</summary>
        /// <param name="measurement">
        /// The date of existence.
        /// </param>
        [Pure]
        public bool ExistsOnDate(Date measurement)
            => StartDate <= measurement && (!EndDate.HasValue || EndDate.Value >= measurement);

        /// <summary>Gets the countries using this currency at the given date.</summary>
        /// <param name="measurement">
        /// The date of measurement.
        /// </param>
        [Pure]
        public IEnumerable<Country> GetCountries(Date measurement)
        {
            var currency = this;
            return Country.GetExisting(measurement)
                .Where(country => country.GetCurrency(measurement) == currency);
        }

        /// <summary>Deserializes the currency from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized currency.
        /// </returns>
        [Pure]
        public static Currency FromJson(long json) => FromJson(json.ToString("000", CultureInfo.InvariantCulture));

        /// <summary>Serializes the currency to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        [Pure]
        public string ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string"/> that represents the current currency for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => this.DebuggerDisplay("{0:e (i/0)}");

        /// <summary>Returns a formatted <see cref="string"/> that represents the current currency.</summary>
        /// <param name="format">
        /// The format that describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// The formats:
        /// 
        /// n: as Name.
        /// i: as ISO Code.
        /// 0: as ISO Numeric.
        /// s/$: as symbol.
        /// e: as English name.
        /// f: as formatted/display name.
        /// </remarks>
        [Pure]
        public string ToString(string format, IFormatProvider formatProvider)
            => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
            ? formatted
            : StringFormatter.Apply(this, format.WithDefault("n"), formatProvider, FormatTokens);

        /// <summary>The format token instructions.</summary>
        private static readonly Dictionary<char, Func<Currency, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<Currency, IFormatProvider, string>>
        {
            { 'n', (svo, provider) => svo.Name },
            { 'i', (svo, provider) => svo.IsoCode },
            { '0', (svo, provider) => svo.IsoNumericCode.ToString("000", provider) },
            { 's', (svo, provider) => svo.Symbol },
            { '$', (svo, provider) => svo.Symbol },
            { 'e', (svo, provider) => svo.EnglishName },
            { 'f', (svo, provider) => svo.GetResourceString("DisplayName", provider) },
        };


        /// <summary>Gets an XML string representation of the @FullName.</summary>
        [Pure]
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        [Pure]
        object IFormatProvider.GetFormat(Type formatType)
            => formatType == typeof(NumberFormatInfo)
            ? GetNumberFormatInfo(CultureInfo.CurrentCulture)
            : null;

        /// <summary>Gets a <see cref="NumberFormatInfo"/> based on the <see cref="IFormatProvider"/>.</summary>
        /// <remarks>
        /// Because the options for formatting and parsing currencies as provided 
        /// by the .NET framework are not sufficient, internally we use number
        /// settings. For parsing and formatting however we like to use the
        /// currency properties of the <see cref="NumberFormatInfo"/> instead of
        /// the number properties, so we copy them for desired behavior.
        /// </remarks>
        [Pure]
        internal NumberFormatInfo GetNumberFormatInfo(IFormatProvider formatProvider)
        {
            var info = NumberFormatInfo.GetInstance(formatProvider);
            info = (NumberFormatInfo)info.Clone();
            info.CurrencySymbol = string.IsNullOrEmpty(Symbol) ? IsoCode : Symbol;
            info.CurrencyDecimalDigits = Digits;
            return info;
        }

        /// <summary>Casts a currency to a <see cref="string"/>.</summary>
        public static explicit operator string(Currency val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a currency.</summary>
        public static explicit operator Currency(string str) => Cast.String<Currency>(TryParse, str);

        /// <summary>Casts a currency to a <see cref="string"/>.</summary>
        public static explicit operator int(Currency val) => val.IsoNumericCode;

        /// <summary>Casts a <see cref="string"/> to a currency.</summary>
        public static explicit operator Currency(int val) => AllCurrencies.FirstOrDefault(c => c.IsoNumericCode == val);

        /// <summary>Converts the string to a currency.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a currency to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, IFormatProvider formatProvider, out Currency result)
        {
            result = Empty;
            var buffer = s.Buffer().Unify();
            if (buffer.IsEmpty())
            {
                return true;
            }
            else if (buffer.IsUnknown(formatProvider) || buffer.Equals(Unknown.Symbol))
            {
                result = Unknown;
                return true;
            }
            else if (ParseValues.TryGetValue(formatProvider, buffer.ToString(), out var val))
            {
                result = new Currency(val);
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Get currencies

        /// <summary>Gets all existing currencies.</summary>
        /// <returns>
        /// A list of existing currencies.
        /// </returns>
        [Pure]
        public static IEnumerable<Currency> GetExisting() => GetExisting(Date.Today);

        /// <summary>Gets all countries existing on the specified measurement date.</summary>
        /// <param name="measurement">
        /// The measurement date.
        /// </param>
        /// <returns>
        /// A list of existing countries.
        /// </returns>
        [Pure]
        public static IEnumerable<Currency> GetExisting(Date measurement)
            => AllCurrencies.Where(currency => currency.ExistsOnDate(measurement));

        /// <summary>Gets a collection of all country info's.</summary>
        /// <remarks>
        /// We'd like to call this All, but because of CLS-compliance, we can not,
        /// because ALL exists.
        /// </remarks>
        public static readonly ReadOnlyCollection<Currency> AllCurrencies = new(
            ResourceManager
                .GetString("All")
                .Split(';')
                .Select(str => new Currency(str))
                .ToList());

        #endregion

        private static readonly CurrencyValues ParseValues = new();

        private sealed class CurrencyValues : LocalizedValues<string>
        {
            public CurrencyValues() : base(new Dictionary<string, string>
            {
                { "ZZZ", "ZZZ" },
                { "999", "ZZZ" },
                { "unknown", "ZZZ" },
            })
            {
                foreach (var currency in AllCurrencies)
                {
                    var unified = currency.GetDisplayName(CultureInfo.InvariantCulture).Buffer().Unify();
                    this[CultureInfo.InvariantCulture][currency.IsoCode.ToUpperInvariant()] = currency.m_Value;
                    this[CultureInfo.InvariantCulture][currency.IsoNumericCode.ToString("000", CultureInfo.InvariantCulture)] = currency.m_Value;
                    this[CultureInfo.InvariantCulture][unified] = currency.m_Value;
                    if (!string.IsNullOrEmpty(currency.Symbol))
                    {
                        this[CultureInfo.InvariantCulture][currency.Symbol] = currency.m_Value;
                    }
                }
            }

            protected override void AddCulture(CultureInfo culture)
            {
                this[culture][Unknown.GetDisplayName(culture)] = Unknown.m_Value;
                foreach (var country in AllCurrencies)
                {
                    var unified = country.GetDisplayName(culture).Buffer().Unify();
                    this[culture][unified] = country.m_Value;
                }
            }
        }

        /// <summary>This is done so that it will be available when called by another initialization.</summary>
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (s_ResourceManager == null)
                {
                    ResourceManager temp = new ResourceManager("Qowaiv.Financial.CurrencyLabels", typeof(Currency).Assembly);
                    s_ResourceManager = temp;
                }
                return s_ResourceManager;
            }
        }
        private static ResourceManager s_ResourceManager;

        /// <summary>Get resource string.</summary>
        /// <param name="postfix">
        /// The prefix of the resource key.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        [Pure]
        internal string GetResourceString(string postfix, IFormatProvider formatProvider)
            => GetResourceString(postfix, formatProvider as CultureInfo);

        /// <summary>Get resource string.</summary>
        /// <param name="postfix">
        /// The prefix of the resource key.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        [Pure]
        internal string GetResourceString(string postfix, CultureInfo culture)
            => IsEmpty()
            ? string.Empty
            : ResourceManager.GetString(m_Value + '_' + postfix, culture ?? CultureInfo.CurrentCulture) ?? string.Empty;

        #region Money creation operators

#pragma warning disable S4069
        // Operator overloads should have named alternatives
        // In this case, Money.Create is the best way to achieve this. The name Add would be confusing.

        /// <summary>Creates money based on the amount and the currency.</summary>
        public static Money operator +(Amount val, Currency currency) => Money.Create((decimal)val, currency);
        /// <summary>Creates money based on the amount and the currency.</summary>
        public static Money operator +(decimal val, Currency currency) => Money.Create(val, currency);
        /// <summary>Creates money based on the amount and the currency.</summary>
        public static Money operator +(double val, Currency currency) => Money.Create(Cast.ToDecimal<Money>(val), currency);
        /// <summary>Creates money based on the amount and the currency.</summary>
        public static Money operator +(int val, Currency currency) => Money.Create(val, currency);

#pragma warning restore S4069 // Operator overloads should have named alternatives

        #endregion
    }
}
