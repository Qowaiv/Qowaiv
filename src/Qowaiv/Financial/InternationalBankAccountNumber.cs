#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
using Qowaiv.Conversion.Financial;
using Qowaiv.Globalization;

namespace Qowaiv.Financial;

/// <summary>The International Bank Account Number (IBAN) is an international standard
/// for identifying bank accounts across national borders with a minimal risk
/// of propagating transcription errors. It was originally adopted by the European
/// Committee for Banking Standards (ECBS), and was later adopted as an international
/// standard under ISO 13616:1997 and now as ISO 13616-1:2007.
/// </summary>
/// <remarks>
/// The official IBAN registrar under ISO 13616-2:2007 is SWIFT.
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
[OpenApiDataType(description: "International Bank Account Number notation as defined by ISO 13616:2007.", example: "BE71096123456769", type: "string", format: "iban", nullable: true, pattern: "[A-Z]{2}[0-9]{2}[A-Z0-9]{8,32}")]
[OpenApi.OpenApiDataType(description: "International Bank Account Number notation as defined by ISO 13616:2007.", example: "BE71096123456769", type: "string", format: "iban", nullable: true, pattern: "[A-Z]{2}[0-9]{2}[A-Z0-9]{8,32}")]
[TypeConverter(typeof(InternationalBankAccountNumberTypeConverter))]
#if NET5_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Financial.InternationalBankAccountNumberJsonConverter))]
#endif
public readonly partial struct InternationalBankAccountNumber : IXmlSerializable, IFormattable, IEquatable<InternationalBankAccountNumber>, IComparable, IComparable<InternationalBankAccountNumber>
{
    /// <summary>Represents the pattern of a (potential) valid IBAN.</summary>
    /// <remarks>
    /// Pairs of IBAN characters can be divided by maximum 2 spacing characters.
    /// </remarks>
    private static readonly Regex Pattern = new(@"^[A-Z]\s{0,2}[A-Z]\s{0,2}[0-9]\s{0,2}[0-9](\s{0,2}[0-9A-Z]){8,36}$", RegOptions.IgnoreCase, RegOptions.Timeout);

    /// <summary>Represents an empty/not set IBAN.</summary>
    public static readonly InternationalBankAccountNumber Empty;

    /// <summary>Represents an unknown (but set) IBAN.</summary>
    public static readonly InternationalBankAccountNumber Unknown = new("ZZ");

    /// <summary>Gets the number of characters of IBAN.</summary>
    public int Length => m_Value is { Length: > 2 } ? m_Value.Length : 0;

    /// <summary>Gets the country of IBAN.</summary>
    public Country Country
    {
        get
        {
            if (m_Value == default)
            {
                return Country.Empty;
            }
            else if (m_Value == Unknown.m_Value)
            {
                return Country.Unknown;
            }
            else
            {
                return Country.Parse(m_Value[..2], CultureInfo.InvariantCulture);
            }
        }
    }

    /// <summary>Serializes the IBAN to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToUnformattedString();

    /// <summary>Returns a <see cref="string"/> that represents the current IBAN for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Formats the IBAN without spaces.</summary>
    [Pure]
    private string ToUnformattedString()
    {
        if (m_Value == default)
        {
            return string.Empty;
        }
        else if (m_Value == Unknown.m_Value)
        {
            return "?";
        }
        else
        {
            return m_Value;
        }
    }

    /// <summary>Formats the IBAN without spaces as lowercase.</summary>
    [Pure]
    private string ToUnformattedLowercaseString() => ToUnformattedString().ToLowerInvariant();

    /// <summary>Formats the IBAN with spaces.</summary>
    [Pure]
    private string ToFormattedString()
    {
        if (m_Value == default)
        {
            return string.Empty;
        }
        if (m_Value == Unknown.m_Value)
        {
            return "?";
        }
        return string.Join(" ", Chunk(m_Value));

        static IEnumerable<string> Chunk(string str)
        {
            for (var i = 0; i < str.Length; i += 4)
            {
                yield return str.Length - i > 4
                    ? str.Substring(i, 4)
                    : str[i..];
            }
        }
    }

    /// <summary>Formats the IBAN with spaces as lowercase.</summary>
    [Pure]
    private string ToFormattedLowercaseString() => ToFormattedString().ToLowerInvariant();

    /// <summary>Returns a formatted <see cref="string"/> that represents the current IBAN.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// The formats:
    ///
    /// u: as unformatted lowercase.
    /// U: as unformatted uppercase.
    /// f: as formatted lowercase.
    /// F: as formatted uppercase.
    /// </remarks>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : StringFormatter.Apply(this, format.WithDefault("U"), formatProvider, FormatTokens);

    /// <summary>The format token instructions.</summary>
    private static readonly Dictionary<char, Func<InternationalBankAccountNumber, IFormatProvider, string>> FormatTokens = new()
    {
        { 'u', (svo, _) => svo.ToUnformattedLowercaseString() },
        { 'U', (svo, _) => svo.ToUnformattedString() },
        { 'f', (svo, _) => svo.ToFormattedLowercaseString() },
        { 'F', (svo, _) => svo.ToFormattedString() },
    };

    /// <summary>Gets an XML string representation of the IBAN.</summary>
    [Pure]
    private string ToXmlString() => ToUnformattedString();

    /// <summary>Converts the string to an IBAN.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing an IBAN to convert.
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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out InternationalBankAccountNumber result)
    {
        result = default;
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
        else if (str.Length.IsInRange(12, 36)
            && str.Matches(Pattern)
            && ValidForCountry(str)
            && Mod97(str))
        {
            result = new InternationalBankAccountNumber(str);
            return true;
        }
        return false;
    }

    [Pure]
    private static bool ValidForCountry(string iban)
        => Country.TryParse(iban[..2], out var country)
        && !country.IsEmptyOrUnknown()
        && (!LocalizedPatterns.TryGetValue(country, out var localizedPattern)
        || iban.Matches(localizedPattern));

    [Pure]
    private static bool Mod97(string iban)
    {
        var mod = 0;
        for (var i = 0; i < iban.Length; i++)
        {
            var digit = iban[(i + 4) % iban.Length]; // Calculate the first 4 characters (country and checksum) last
            var index = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(digit);
            mod *= index > 9 ? 100 : 10;
            mod += index;
            mod = mod.Mod(97);
        }
        return mod == 1;
    }
}
