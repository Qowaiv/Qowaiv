#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
using Qowaiv.Conversion.Financial;
using Qowaiv.Globalization;

namespace Qowaiv.Financial;

/// <summary>The Business Identifier Code (BIC) is a standard format of Business Identifier Codes
/// approved by the International Organization for Standardization (ISO) as ISO 9362.
/// It is a unique identification code for both financial and non-financial institutions.
/// </summary>
/// <remarks>
/// When assigned to a non-financial institution, a code may also be known
/// as a Business Entity Identifier or BEI.
///
/// These codes are used when transferring money between banks, particularly
/// for international wire transfers, and also for the exchange of other
/// messages between banks. The codes can sometimes be found on account
/// statements.
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
[OpenApiDataType(description: "Business Identifier Code, as defined by ISO 9362.", example: "DEUTDEFF", type: "string", format: "bic", nullable: true)]
[TypeConverter(typeof(BusinessIdentifierCodeTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Financial.BusinessIdentifierCodeJsonConverter))]
#endif
public readonly partial struct BusinessIdentifierCode : IXmlSerializable, IFormattable, IEquatable<BusinessIdentifierCode>, IComparable, IComparable<BusinessIdentifierCode>
{
    /// <remarks>
    /// http://www.codeproject.com/KB/recipes/bicRegexValidator.aspx.
    /// </remarks>
    private static readonly Regex Pattern = GetPattern();

#if NET8_0_OR_GREATER
    [GeneratedRegex(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$", RegOptions.IgnoreCase, RegOptions.TimeoutMilliseconds)]
    private static partial Regex GetPattern();
#else
    [Pure]
    private static Regex GetPattern() => new(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$", RegOptions.IgnoreCase, RegOptions.Timeout);
#endif

    /// <summary>Represents an empty/not set BIC.</summary>
    public static readonly BusinessIdentifierCode Empty;

    /// <summary>Represents an unknown (but set) BIC.</summary>
    public static readonly BusinessIdentifierCode Unknown = new("ZZZZZZZZZZZ");

    /// <summary>Gets the number of characters of BIC.</summary>
    public int Length => IsUnknown() ? 0 : m_Value?.Length ?? 0;

    /// <summary>Gets the institution code or business code.</summary>
    public string Business
        => m_Value is { Length: >= 4 } && !IsUnknown()
        ? m_Value[..4]
        : string.Empty;

    /// <summary>Gets the country info of the country code.</summary>
    public Country Country
        => m_Value is { Length: >= 6 }
        ? Country.Parse(m_Value.Substring(4, 2), CultureInfo.InvariantCulture)
        : Country.Empty;

    /// <summary>Gets the location code.</summary>
    public string Location
        => m_Value is { Length: >= 8 } && !IsUnknown()
        ? m_Value.Substring(6, 2)
        : string.Empty;

    /// <summary>Gets the branch code.</summary>
    /// <remarks>
    /// Is optional, XXX for primary office.
    /// </remarks>
    public string Branch => m_Value is { Length: 11 } && !IsUnknown() ? m_Value[8..] : string.Empty;

    /// <summary>Serializes the BIC to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value;

    /// <summary>Returns a <see cref="string"/> that represents the current BIC for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0}");

    /// <summary>Returns a formatted <see cref="string"/> that represents the current BIC.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
        {
            return formatted;
        }
        else if (IsUnknown()) return "?";
        else return m_Value ?? string.Empty;
    }

    /// <summary>Gets an XML string representation of the BIC.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Converts the string to a BIC.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing a BIC to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out BusinessIdentifierCode result)
    {
        result = default;
        var str = s.Unify();
        if (str.IsEmpty())
        {
            return true;
        }
        else if (str.IsUnknown(provider))
        {
            result = Unknown;
            return true;
        }
        else if (Pattern.IsMatch(str)
            && Country.TryParse(str.Substring(4, 2), out var country)
            && !country.IsEmptyOrUnknown())
        {
            result = new BusinessIdentifierCode(str);
            return true;
        }
        else return false;
    }
}
