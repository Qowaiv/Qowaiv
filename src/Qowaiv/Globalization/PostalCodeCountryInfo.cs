namespace Qowaiv.Globalization;

/// <summary>Represents country specific postal code information.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
public sealed partial class PostalCodeCountryInfo
{
    /// <summary>Constructor.</summary>
    private PostalCodeCountryInfo(
        Country country,
        Regex? validationPattern,
        Regex? formattingSearchPattern,
        string? formattingReplacePattern,
        bool isSingleValue)
    {
        Country = country;
        ValidationPattern = validationPattern;
        FormattingSearchPattern = formattingSearchPattern;
        FormattingReplacePattern = formattingReplacePattern;
        IsSingleValue = isSingleValue;
    }

    /// <summary>Gets the country.</summary>
    public Country Country { get; }

    /// <summary>Returns true if the country has a postal code system, otherwise false.</summary>
    public bool HasPostalCode => ValidationPattern != null;

    /// <summary>Returns true if the country has supports formatting, otherwise false.</summary>
    public bool HasFormatting => FormattingSearchPattern != null;

    /// <summary>Returns true if the country has only one postal code, otherwise false.</summary>
    public bool IsSingleValue { get; }

    /// <summary>Gets the postal code validation pattern for the country.</summary>
    private Regex? ValidationPattern { get; }

    /// <summary>Gets the postal code formatting search pattern for the country.</summary>
    private Regex? FormattingSearchPattern { get; }

    /// <summary>Gets the postal code formatting replace pattern for the country.</summary>
    private string? FormattingReplacePattern { get; }

    /// <summary>Returns true if the postal code is valid for the specified country, otherwise false.</summary>
    /// <param name="postalcode">
    /// The postal code to test.
    /// </param>
    /// <remarks>
    /// Returns false if the country does not have postal codes.
    /// </remarks>
    [Pure]
    public bool IsValid(string? postalcode)
        => !string.IsNullOrEmpty(postalcode)
        && ValidationPattern is { }
        && postalcode.Unify().Matches(ValidationPattern);

    /// <summary>Formats the postal code.</summary>
    /// <param name="postalcode">
    /// The postal code.
    /// </param>
    /// <returns>
    /// A formatted string representing the postal code.
    /// </returns>
    /// <remarks>
    /// If the country supports formatting and if the postal code is valid
    /// for the country.
    /// </remarks>
    [Pure]
    public string Format(string postalcode)
    {
        if (FormattingSearchPattern is { } && IsValid(postalcode))
        {
            return FormattingSearchPattern.Replace(postalcode, FormattingReplacePattern ?? string.Empty);
        }
        else if (Unknown.IsUnknown(postalcode, CultureInfo.InvariantCulture))
        {
            return "?";
        }
        else return postalcode ?? string.Empty;
    }

    /// <summary>Gets the single value if supported, otherwise string.Empty.</summary>
    [Pure]
    public string GetSingleValue()
        => IsSingleValue && FormattingReplacePattern is { }
        ? FormattingReplacePattern
        : string.Empty;

    /// <summary>Returns a <see cref="string"/> that represents the current postal code country info for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Postal code[{0}], ", Country.IsoAlpha2Code);
            if (!HasPostalCode)
            {
                sb.Append("none");
            }
            else if (IsSingleValue)
            {
                sb.Append("Value: ").Append(FormattingReplacePattern);
            }
            else
            {
                sb.Append("Pattern: ").Append(ValidationPattern);

                if (HasFormatting)
                {
                    sb.Append(", Format: ").Append(FormattingReplacePattern);
                }
            }
            return sb.ToString();
        }
    }

    /// <summary>Gets countries without a postal code system.</summary>
    [Pure]
    public static IEnumerable<Country> GetCountriesWithoutPostalCode()
        => Country.GetExisting().Where(country => !GetInstance(country).HasPostalCode);

    /// <summary>Gets countries with postal codes with formatting.</summary>
    [Pure]
    public static IEnumerable<Country> GetCountriesWithFormatting()
        => Country.GetExisting().Where(country => GetInstance(country).HasFormatting);

    /// <summary>Gets countries with a single postal code value.</summary>
    [Pure]
    public static IEnumerable<Country> GetCountriesWithSingleValue()
        => Country.GetExisting().Where(country => GetInstance(country).IsSingleValue);

    /// <summary>Gets the postal code country info associated with the specified country.</summary>
    /// <param name="country">
    /// The specified country.
    /// </param>
    [Pure]
    public static PostalCodeCountryInfo GetInstance(Country country)
        => Instances.TryGetValue(country, out var instance)
        ? instance
        : new(country, null, null, null, false);

    /// <summary>Creates a new instance.</summary>
    /// <remarks>
    /// Used for initializing the Instances dictionary.
    /// </remarks>
    [Pure]
    private static PostalCodeCountryInfo New(Country country, string validation, string? search = null, string? replace = null, bool isSingle = false)
        => new(
            country: country,
            validationPattern: new(validation, RegOptions.WithBackTracking, RegOptions.Timeout),
            formattingSearchPattern: string.IsNullOrEmpty(search) ? null : new(search, RegOptions.Default, RegOptions.Replacement),
            formattingReplacePattern: replace,
            isSingleValue: isSingle);
}
