#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
using Qowaiv.Conversion.Financial;
using Qowaiv.Globalization;
using System.Collections.ObjectModel;

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
    public string? ToJson() => m_Value == default ? null : MachineReadable();

    /// <summary>Returns a <see cref="string"/> that represents the current IBAN for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Represents the IBAN a string without formatting.</summary>
    [Pure]
    public string MachineReadable()
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

    /// <summary>In order to facilitate reading by humans, an IBAN can be
    /// expressed in groups of four characters separated by spaces, the last
    /// group being of variable length.
    /// </summary>
    /// <remarks>
    /// Uses non-breaking spaces to prevent unintended line-breaks.
    /// </remarks>
    [Pure]
    public string HumanReadable() => HumanReadable((char)0160);

    /// <summary>In order to facilitate reading by humans, an IBAN can be
    /// expressed in groups of four characters separated by spaces, the last
    /// group being of variable length.
    /// </summary>
    /// <param name="space">
    /// The spacing character to apply.
    /// </param>
    [Pure]
    public string HumanReadable(char space)
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
            var index = 0;
            var pointer = 0;
            var spacing = (m_Value.Length - 1) / 4;
            var buffer = new char[m_Value.Length + spacing];

            while (index < m_Value.Length)
            {
                buffer[pointer++] = m_Value[index++];
                if ((index % 4) == 0 && pointer < buffer.Length)
                {
                    buffer[pointer++] = space;
                }
            }
            return new(buffer);
        }
    }

    /// <summary>Returns a formatted <see cref="string"/> that represents the current IBAN.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// The formats:
    /// m: as machine readable lowercase.
    /// M: as machine readable.
    /// u: as unformatted lowercase (equal to machine readable lowercase).
    /// U: as unformatted uppercase  (equal to machine readable).
    /// h: as human readable lowercase (with non-breaking spaces).
    /// H: as human readable (equal to machine readable).
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
        { 'u', (svo, _) => svo.MachineReadable().ToLowerInvariant() },
        { 'U', (svo, _) => svo.MachineReadable() },
        { 'm', (svo, _) => svo.MachineReadable().ToLowerInvariant() },
        { 'M', (svo, _) => svo.MachineReadable() },
        { 'h', (svo, _) => svo.HumanReadable(' ').ToLowerInvariant() },
        { 'H', (svo, _) => svo.HumanReadable(' ') },
        { 'f', (svo, _) => svo.HumanReadable(' ').ToLowerInvariant() },
        { 'F', (svo, _) => svo.HumanReadable(' ') },
    };

    /// <summary>Gets an XML string representation of the IBAN.</summary>
    [Pure]
    private string ToXmlString() => MachineReadable();

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

        if (s is { Length: >= 12 } && IbanParser.Parse(s) is { } iban)
        {
            result = new(iban);
            return true;
        }
        else
        {
            var str = s.Unify();
            if (str.IsEmpty())
            {
                return true;
            }
            if (str.IsUnknown(formatProvider))
            {
                result = Unknown;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>A list with countries supporting IBAN.</summary>
    public static readonly IReadOnlyCollection<Country> Supported = new ReadOnlyCollection<Country>(
        IbanParser.Parsers
            .OfType<BbanParser>()
            .Select(p => p.Country)
            .Where(c => c.IsKnown)
            .ToList());
}
