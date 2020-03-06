#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Globalization;
using Qowaiv.Financial;
using Qowaiv.Formatting;
using Qowaiv.Json;
using Qowaiv.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Qowaiv.Globalization
{
    /// <summary>Represents a </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable]
    [SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
    [OpenApiDataType(description: "Country notation as defined by ISO 3166-1 alpha-2, for example, NL.", type: "string", format: "country", nullable: true)]
    [TypeConverter(typeof(CountryTypeConverter))]
    public partial struct Country : ISerializable, IXmlSerializable, IFormattable, IEquatable<Country>, IComparable, IComparable<Country>
    {
        /// <summary>Represents an empty/not set </summary>
        public static readonly Country Empty;

        /// <summary>Represents an unknown (but set) </summary>
        public static readonly Country Unknown = new Country("ZZ");

        /// <summary>Gets a country based on the current thread.</summary>
        public static Country Current=>Thread.CurrentThread.GetValue<Country>();

        /// <summary>Gets the name of the country.</summary>
        /// <remarks>
        /// For countries that no longer exit, this returns the ISO 3166-3 code.
        /// For unknown a '?' is returned.
        /// For existing countries this returns the ISO 3166-1 code.
        /// </remarks>
        public string Name => IsUnknown() ? "?" : m_Value ?? string.Empty;

        /// <summary>Gets the display name.</summary>
        public string DisplayName => GetDisplayName(CultureInfo.CurrentCulture);

        /// <summary>Gets the full name of the country/region in English.</summary>
        /// <returns>
        /// The full name of the country in English.
        /// </returns>
        public string EnglishName => GetDisplayName(CultureInfo.InvariantCulture);

        ///<summary>Gets the two-letter code defined in ISO 3166-1 for the country.</summary>
        /// <returns>
        /// The two-letter code defined in ISO 3166-1 for the country.
        /// </returns>
        public string IsoAlpha2Code=> GetResourceString("ISO2", CultureInfo.InvariantCulture); 

        ///<summary>Gets the three-letter code defined in ISO 3166-1 for the country.</summary>
        /// <returns>
        /// The three-letter code defined in ISO 3166-1 for the country.
        /// </returns>
        public string IsoAlpha3Code=>GetResourceString("ISO3", CultureInfo.InvariantCulture); 

        ///<summary>Gets the numeric code defined in ISO 3166-1 for the country/region.</summary>
        /// <returns>
        /// The numeric code defined in ISO 3166-1 for the country/region.
        /// </returns>
        public int IsoNumericCode => m_Value == default ? 0 : XmlConvert.ToInt32(GetResourceString("ISO", CultureInfo.InvariantCulture)); 

        /// <summary>Gets the country calling code as defined by ITU-T.</summary>
        /// <remarks>
        /// Recommendations E.123 and E.164, also called IDD (International Direct Dialing) or ISD (International Subscriber Dialling) codes.
        /// </remarks>
        public string CallingCode => GetResourceString("CallingCode", CultureInfo.InvariantCulture); 

        ///<summary>Gets true if the RegionInfo equivalent of this country exists, otherwise false.</summary>
        public bool RegionInfoExists => !string.IsNullOrEmpty(GetResourceString("RegionInfoExists", CultureInfo.InvariantCulture));

        /// <summary>Gets the start date from witch the country exists.</summary>
        public Date StartDate=> m_Value == default ? Date.MinValue : (Date)XmlConvert.ToDateTime(GetResourceString("StartDate", CultureInfo.InvariantCulture), "yyyy-MM-dd");

        /// <summary>If the country does not exist anymore, the end date is given, otherwise null.</summary>
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
        /// <returns>
        /// Returns a localized display name.
        /// </returns>
        public string GetDisplayName(CultureInfo culture) => GetResourceString("DisplayName", culture);

        /// <summary>Returns true if the country exists at the given date, otherwise false.</summary>
        /// <param name="measurement">
        /// The date of existence.
        /// </param>
        public bool ExistsOnDate(Date measurement)
        {
            return StartDate <= measurement && (!EndDate.HasValue || EndDate.Value >= measurement);
        }

        /// <summary>Gets the active currency at the given date.</summary>
        /// <param name="measurement">
        /// The date of measurement.
        /// </param>
        public Currency GetCurrency(Date measurement)
        {
            if (!ExistsOnDate(measurement))
            {
                return Currency.Empty;
            }
            var country = this;
            return CountryToCurrency.All.Where(map => map.Country == country && map.StartDate <= measurement)
                .Select(map => map.Currency)
                .LastOrDefault();
        }

        /// <summary>Converts the CountryInfo to a RegionInfo.</summary>
        public RegionInfo ToRegionInfo()
        {
            try
            {
                return new RegionInfo(IsoAlpha2Code);
            }
            catch
            {
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture,
                    QowaivMessages.NotSupportedExceptionCountryToRegionInfo, EnglishName, IsoAlpha2Code));
            }
        }

        /// <summary>Deserializes the country from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized country.
        /// </returns>
        public static Country FromJson(long json) => FromJson(json.ToString("000", CultureInfo.InvariantCulture));

        /// <summary>Serializes the country to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => m_Value;

        /// <summary>Returns a <see cref="string"/> that represents the current Country for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                if (IsEmpty())
                {
                    return "{empty}";
                }
                if (IsUnknown())
                {
                    return "?";
                }
                return string.Format(CultureInfo.InvariantCulture, "{0} ({1}/{2})", EnglishName, IsoAlpha2Code, IsoAlpha3Code);
            }
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <remarks>
        /// The formats:
        /// 
        /// n: as Name.
        /// 0: as ISO Numeric.
        /// 2: as ISO Alpha-2.
        /// 3: as ISO Alpha-3.
        /// e: as English name.
        /// f: as formatted/display name.
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }

            // If no format specified, use the default format.
            if (string.IsNullOrEmpty(format)) { return Name; }

            // Apply the format.
            return StringFormatter.Apply(this, format, formatProvider, FormatTokens);
        }

        /// <summary>Gets an XML string representation of the country.</summary>
        private string ToXmlString() => m_Value;

        /// <summary>The format token instructions.</summary>
        private static readonly Dictionary<char, Func<Country, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<Country, IFormatProvider, string>>
        {
            { 'n', (svo, provider) => svo.Name },
            { '2', (svo, provider) => svo.GetResourceString("ISO2", provider) },
            { '3', (svo, provider) => svo.GetResourceString("ISO3", provider) },
            { '0', (svo, provider) => svo.GetResourceString("ISO", provider) },
            { 'e', (svo, provider) => svo.EnglishName },
            { 'f', (svo, provider) => svo.GetResourceString("DisplayName", provider) },
        };

        /// <summary>Casts a Country to a <see cref="string"/>.</summary>
        public static explicit operator string(Country val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a </summary>
        public static explicit operator Country(string str) => Parse(str, CultureInfo.CurrentCulture);

        /// <summary>Casts a System.Globalization.RegionInfo to a </summary>
        public static implicit operator Country(RegionInfo region) => Create(region);

        /// <summary>Casts a Country to a System.Globalization.RegionInf.</summary>
        public static explicit operator RegionInfo(Country val) =>val.ToRegionInfo();

        /// <summary>Represents the underlying value as <see cref="IConvertible"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IConvertible Convertable => m_Value ?? string.Empty;

        /// <inheritdoc/>
        TypeCode IConvertible.GetTypeCode() => TypeCode.String;

        /// <summary>Converts the string to a 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Country to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, IFormatProvider formatProvider, out Country result)
        {
            result = Empty;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }

            var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;

            if (Qowaiv.Unknown.IsUnknown(s, culture))
            {
                result = Unknown;
                return true;
            }

            AddCulture(culture);

            var str = Parsing.ToUnified(s);

            if (Parsings[culture].TryGetValue(str, out string val) || Parsings[CultureInfo.InvariantCulture].TryGetValue(str, out val))
            {
                result = new Country( val );
                return true;
            }
            return false;
        }

        /// <summary>Creates a country based on a region info.</summary>
        /// <param name="region">
        /// The corresponding region info.
        /// </param>
        /// <returns>
        /// Returns a country that represents the same region as region info.
        /// </returns>
        public static Country Create(RegionInfo region)
        {
            if (region == null) { return default; }
            // In .NET, Serbia and Montenegro (CS) is still active.
            if (region.TwoLetterISORegionName == "CS") { return CSXX; }

            return All.FirstOrDefault(c => c.Name == region.TwoLetterISORegionName);
        }

        /// <summary>Creates a country based on a culture info.</summary>
        /// <param name="culture">
        /// A culture info.
        /// </param>
        /// <returns>
        /// Returns a country that represents the country specified at the culture if
        /// any, otherwise Empty.
        /// </returns>
        public static Country Create(CultureInfo culture)
        {
            if (culture == null || culture == CultureInfo.InvariantCulture || culture.IsNeutralCulture)
            {
                return default;
            }

            var name = culture.Name.Substring(culture.Name.IndexOf('-') + 1);

            return All.FirstOrDefault(c => c.Name == name);
        }

        #region Get countries

        /// <summary>Gets all existing countries.</summary>
        /// <returns>
        /// A list of existing countries.
        /// </returns>
        public static IEnumerable<Country> GetExisting()
        {
            return GetExisting(Date.Today);
        }

        /// <summary>Gets all countries existing on the specified measurement date.</summary>
        /// <param name="measurement">
        /// The measurement date.
        /// </param>
        /// <returns>
        /// A list of existing countries.
        /// </returns>
        public static IEnumerable<Country> GetExisting(Date measurement)
        {
            return All.Where(country => country.ExistsOnDate(measurement));
        }

        /// <summary>Gets a collection of all country info's.</summary>
        public static readonly ReadOnlyCollection<Country> All = new ReadOnlyCollection<Country>(
            ResourceManager
                .GetString("All")
                .Split(';')
                .Select(str => new Country { m_Value = str })
                .ToList());

        #endregion

        #region Resources

        internal static ResourceManager ResourceManager
        {
            get
            {
                if (s_ResourceManager == null)
                {
                    ResourceManager temp = new ResourceManager("Qowaiv.Globalization.CountryLabels", typeof(Country).Assembly);
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
        internal string GetResourceString(string postfix, IFormatProvider formatProvider)
        {
            return GetResourceString(postfix, formatProvider as CultureInfo);
        }

        /// <summary>Get resource string.</summary>
        /// <param name="postfix">
        /// The prefix of the resource key.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        internal string GetResourceString(string postfix, CultureInfo culture)
        {
            if (m_Value == default) { return string.Empty; }
            return ResourceManager.GetString(m_Value + '_' + postfix, culture ?? CultureInfo.CurrentCulture) ?? string.Empty;
        }

        #endregion

        #region Lookup

        /// <summary>Initializes the country lookup.</summary>
        static Country()
        {
            foreach (var country in All)
            {
                Parsings[CultureInfo.InvariantCulture][country.IsoAlpha2Code.ToUpperInvariant()] = country.m_Value;
                Parsings[CultureInfo.InvariantCulture][country.IsoAlpha3Code.ToUpperInvariant()] = country.m_Value;
                Parsings[CultureInfo.InvariantCulture][country.IsoNumericCode.ToString("000", CultureInfo.InvariantCulture)] = country.m_Value;
                Parsings[CultureInfo.InvariantCulture][Parsing.ToUnified(country.GetDisplayName(CultureInfo.InvariantCulture))] = country.m_Value;
            }
        }

        /// <summary>Adds a culture to the parsing collections.</summary>
        /// <param name="culture">
        /// The culture to add.
        /// </param>
        private static void AddCulture(CultureInfo culture)
        {
            lock (locker)
            {
                if (Parsings.ContainsKey(culture)) { return; }

                Parsings[culture] = new Dictionary<string, string>();

                Parsings[culture][Unknown.GetDisplayName(culture)] = Unknown.m_Value;

                foreach (var country in All)
                {
                    Parsings[culture][Parsing.ToUnified(country.GetDisplayName(culture))] = country.m_Value;
                }
            }
        }

        /// <summary>Represents the parsing keys.</summary>
        private static readonly Dictionary<CultureInfo, Dictionary<string, string>> Parsings = new Dictionary<CultureInfo, Dictionary<string, string>>
        {
            {
                CultureInfo.InvariantCulture, new Dictionary<string, string>
                {
                    { "ZZ", "ZZ" },
                    { "ZZZ", "ZZ" },
                    { "999", "ZZ" },
                    { "unknown", "ZZ" },
                }
            },
        };

        /// <summary>The locker for adding a culture.</summary>
        private static volatile object locker = new object();

        #endregion
    }
}
