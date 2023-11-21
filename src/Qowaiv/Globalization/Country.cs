#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
using Qowaiv.Conversion.Globalization;
using Qowaiv.Financial;
using System.Collections.ObjectModel;
using System.Threading;

namespace Qowaiv.Globalization;

/// <summary>Represents a country.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
[OpenApiDataType(description: "Country notation as defined by ISO 3166-1 alpha-2.", example: "NL", type: "string", format: "country", nullable: true)]
[OpenApi.OpenApiDataType(description: "Country notation as defined by ISO 3166-1 alpha-2.", example: "NL", type: "string", format: "country", nullable: true)]
[TypeConverter(typeof(CountryTypeConverter))]
#if NET5_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Globalization.CountryJsonConverter))]
#endif
public readonly partial struct Country : IXmlSerializable, IFormattable, IEquatable<Country>, IComparable, IComparable<Country>
{
    /// <summary>Represents an empty/not set country.</summary>
    public static readonly Country Empty;

    /// <summary>Represents an unknown (but set) country.</summary>
    public static readonly Country Unknown = new("ZZ");

    /// <summary>Gets a country based on the current thread.</summary>
    public static Country Current => Thread.CurrentThread.GetValue<Country>();

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

    /// <summary>Gets the two-letter code defined in ISO 3166-1 for the country.</summary>
    /// <returns>
    /// The two-letter code defined in ISO 3166-1 for the country.
    /// </returns>
    public string IsoAlpha2Code => GetResourceString("ISO2", CultureInfo.InvariantCulture);

    /// <summary>Gets the three-letter code defined in ISO 3166-1 for the country.</summary>
    /// <returns>
    /// The three-letter code defined in ISO 3166-1 for the country.
    /// </returns>
    public string IsoAlpha3Code => GetResourceString("ISO3", CultureInfo.InvariantCulture);

    /// <summary>Gets the numeric code defined in ISO 3166-1 for the country/region.</summary>
    /// <returns>
    /// The numeric code defined in ISO 3166-1 for the country/region.
    /// </returns>
    public int IsoNumericCode => m_Value == default ? 0 : XmlConvert.ToInt32(GetResourceString("ISO", CultureInfo.InvariantCulture));

    /// <summary>Gets the country calling code as defined by ITU-T.</summary>
    /// <remarks>
    /// Recommendations E.123 and E.164, also called IDD (International Direct Dialing) or ISD (International Subscriber Dialling) codes.
    /// </remarks>
    public string CallingCode => GetResourceString("CallingCode", CultureInfo.InvariantCulture);

    /// <summary>Gets true if the RegionInfo equivalent of this country exists, otherwise false.</summary>
    public bool RegionInfoExists
    {
        get
        {
            try
            {
                return ToRegionInfo() != null;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }
    }

    /// <summary>Gets the start date from witch the country exists.</summary>
    public Date StartDate => m_Value == default ? Date.MinValue : (Date)XmlConvert.ToDateTime(GetResourceString("StartDate", CultureInfo.InvariantCulture), "yyyy-MM-dd");

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
    [Pure]
    public string GetDisplayName(CultureInfo? culture) => GetResourceString("DisplayName", culture);

    /// <summary>Returns true if the country exists at the given date, otherwise false.</summary>
    /// <param name="measurement">
    /// The date of existence.
    /// </param>
    [Pure]
    public bool ExistsOnDate(Date measurement)
    {
        return StartDate <= measurement && (!EndDate.HasValue || EndDate.Value >= measurement);
    }

    /// <summary>Gets the active currency at the given date.</summary>
    /// <param name="measurement">
    /// The date of measurement.
    /// </param>
    [Pure]
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
    [Pure]
    public RegionInfo ToRegionInfo()
    {
        try
        {
            return new RegionInfo(IsoAlpha2Code);
        }
        catch
        {
            throw new NotSupportedException(string.Format(
                CultureInfo.InvariantCulture,
                QowaivMessages.NotSupportedExceptionCountryToRegionInfo,
                EnglishName,
                IsoAlpha2Code));
        }
    }

    /// <summary>Deserializes the country from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized country.
    /// </returns>
    [Pure]
    public static Country FromJson(long json) => FromJson(json.ToString("000", CultureInfo.InvariantCulture));

    /// <summary>Serializes the country to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value;

    /// <summary>Returns a <see cref="string"/> that represents the current Country for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:e (2/3)}");

    /// <summary>Returns a formatted <see cref="string"/> that represents the current country.</summary>
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
    /// 0: as ISO Numeric.
    /// 2: as ISO Alpha-2.
    /// 3: as ISO Alpha-3.
    /// e: as English name.
    /// f: as formatted/display name.
    /// </remarks>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : StringFormatter.Apply(this, format.WithDefault("n"), formatProvider, FormatTokens);

    /// <summary>Gets an XML string representation of the country.</summary>
    [Pure]
    private string ToXmlString() => m_Value ?? string.Empty;

    /// <summary>The format token instructions.</summary>
    private static readonly Dictionary<char, Func<Country, IFormatProvider, string>> FormatTokens = new()
    {
        { 'n', (svo, _) => svo.Name },
        { 'e', (svo, _) => svo.EnglishName },
        { '2', (svo, provider) => svo.GetResourceString("ISO2", provider) },
        { '3', (svo, provider) => svo.GetResourceString("ISO3", provider) },
        { '0', (svo, provider) => svo.GetResourceString("ISO", provider) },
        { 'f', (svo, provider) => svo.GetResourceString("DisplayName", provider) },
    };

    /// <summary>Casts a <see cref="RegionInfo"/> to a country.</summary>
    public static implicit operator Country(RegionInfo region) => Create(region);

    /// <summary>Casts a Country to a System.Globalization.RegionInf.</summary>
    public static explicit operator RegionInfo(Country val) => val.ToRegionInfo();

    /// <summary>Converts the string to a country.
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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out Country result)
    {
        result = Empty;
        var str = s.Unify();
        if (str.IsEmpty())
        {
            return true;
        }
        else if (str.IsUnknown(formatProvider))
        {
            result = Unknown;
            return true;
        }
        else if (ParseValues.TryGetValue(formatProvider, str, out var val))
        {
            result = new Country(val);
            return true;
        }
        else return false;
    }

    /// <summary>Creates a country based on a region info.</summary>
    /// <param name="region">
    /// The corresponding region info.
    /// </param>
    /// <returns>
    /// Returns a country that represents the same region as region info.
    /// </returns>
    [Pure]
    public static Country Create(RegionInfo? region)
    {
        if (region == null) { return default; }

        // In .NET, Serbia and Montenegro (CS) is still active.
        if (region.TwoLetterISORegionName == "CS")
        {
            return CSXX;
        }

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
    [Pure]
    public static Country Create(CultureInfo? culture)
    {
        if (culture == null || culture == CultureInfo.InvariantCulture || culture.IsNeutralCulture)
        {
            return default;
        }

        var name = culture.Name[(culture.Name.IndexOf('-') + 1)..];

        return All.FirstOrDefault(c => c.Name == name);
    }

    /// <summary>Gets all existing countries.</summary>
    /// <returns>
    /// A list of existing countries.
    /// </returns>
    [Pure]
    public static IEnumerable<Country> GetExisting() => GetExisting(Clock.Today());

    /// <summary>Gets all countries existing on the specified measurement date.</summary>
    /// <param name="measurement">
    /// The measurement date.
    /// </param>
    /// <returns>
    /// A list of existing countries.
    /// </returns>
    [Pure]
    public static IEnumerable<Country> GetExisting(Date measurement)
    {
        return All.Where(country => country.ExistsOnDate(measurement));
    }

    /// <summary>Gets a collection of all country info's.</summary>
    public static readonly ReadOnlyCollection<Country> All = new(ResourceManager
        .GetString("All")!
        .Split(';')
        .Select(str => new Country(str))
        .ToList());

    private static readonly CountryValues ParseValues = new();

    private sealed class CountryValues : LocalizedValues<string?>
    {
        public CountryValues() : base(new Dictionary<string, string?>
            {
                { "ZZ", "ZZ" },
                { "ZZZ", "ZZ" },
                { "999", "ZZ" },
                { "unknown", "ZZ" },
            })
        {
            foreach (var country in All)
            {
                var unified = country.GetDisplayName(CultureInfo.InvariantCulture).Unify();
                this[CultureInfo.InvariantCulture][country.IsoAlpha2Code.ToUpperInvariant()] = country.m_Value;
                this[CultureInfo.InvariantCulture][country.IsoAlpha3Code.ToUpperInvariant()] = country.m_Value;
                this[CultureInfo.InvariantCulture][country.IsoNumericCode.ToString("000", CultureInfo.InvariantCulture)] = country.m_Value;
                this[CultureInfo.InvariantCulture][unified] = country.m_Value;
            }
        }

        protected override void AddCulture(CultureInfo culture)
        {
            this[culture][Unknown.GetDisplayName(culture)] = Unknown.m_Value;
            foreach (var country in All)
            {
                var unified = country.GetDisplayName(culture).Unify();
                this[culture][unified] = country.m_Value;
            }
        }
    }

    private static ResourceManager ResourceManager
    {
        get
        {
            rm ??= new("Qowaiv.Globalization.CountryLabels", typeof(Country).Assembly);
            return rm;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static ResourceManager? rm;

    /// <summary>Get resource string.</summary>
    /// <param name="postfix">
    /// The prefix of the resource key.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    private string GetResourceString(string postfix, IFormatProvider? formatProvider)
        => ResourceManager.Localized(formatProvider, $"{m_Value}_{postfix}");
}
