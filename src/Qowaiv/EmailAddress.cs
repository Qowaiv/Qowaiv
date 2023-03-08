#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

using System.Net;

namespace Qowaiv;

/// <summary>Represents an email address.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
[OpenApiDataType(description: "Email notation as defined by RFC 5322.", example: "svo@qowaiv.org", type: "string", format: "email", nullable: true)]
[OpenApi.OpenApiDataType(description: "Email notation as defined by RFC 5322.", example: "svo@qowaiv.org", type: "string", format: "email", nullable: true)]
[TypeConverter(typeof(EmailAddressTypeConverter))]
#if NET5_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.EmailAddressJsonConverter))]
#endif
public readonly partial struct EmailAddress : ISerializable, IXmlSerializable, IFormattable, IEquatable<EmailAddress>, IComparable, IComparable<EmailAddress>
{
    /// <summary>An email address must not exceed 254 characters.</summary>
    /// <remarks>
    /// https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address.
    /// </remarks>
    public const int MaxLength = 254;

    /// <summary>Represents an empty/not set email address.</summary>
    public static readonly EmailAddress Empty;

    /// <summary>Represents an unknown (but set) email address.</summary>
    public static readonly EmailAddress Unknown = new("?");

    /// <summary>Gets the number of characters of email address.</summary>
    public int Length => m_Value is { Length: > 1 } ? m_Value.Length : 0;

    /// <summary>Gets the local part of the Email Address.</summary>
    public string Local => m_Value is { Length: > 1 } ? m_Value.Substring(0, m_Value.IndexOf('@')) : string.Empty;

    /// <summary>Gets the domain part of the Email Address.</summary>
    public string Domain => m_Value is { Length: > 1 } ? m_Value.Substring(m_Value.IndexOf('@') + 1) : string.Empty;

    /// <summary>True if the domain part of the Email Address is an IP-address.</summary>
    /// <remarks>
    /// As IP-Addresses are normalized by the <see cref="EmailParser"/> it
    /// can simply be checked by checking the last character of the string
    /// value.
    /// </remarks>
    public bool IsIPBased => m_Value is { Length: > 1 } && m_Value[m_Value.Length - 1] == ']';

    /// <summary>Gets the IP-Address if the Email Address is IP-based, otherwise <see cref="IPAddress.None"/>.</summary>
    public IPAddress IPDomain
    {
        get
        {
            if (IsIPBased)
            {
                var ip = Domain.StartsWith("[IPv6:", StringComparison.InvariantCulture)
                    ? Domain.Substring(6, Domain.Length - 7)
                    : Domain.Substring(1, Domain.Length - 2);
                return IPAddress.Parse(ip);
            }
            else return IPAddress.None;
        }
    }

    /// <summary>Gets the email address with a (prefix) display name.</summary>
    /// <param name="displayName">
    /// The display name.
    /// </param>
    /// <returns>
    /// An email address with a display name.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// When the email address is empty or unknown.
    /// </exception>
    [Pure]
    public string WithDisplayName(string displayName)
    {
        if (IsEmptyOrUnknown())
        {
            throw new InvalidOperationException(QowaivMessages.InvalidOperationException_WithDisplayName);
        }
        if (string.IsNullOrWhiteSpace(displayName))
        {
            return ToString(CultureInfo.InvariantCulture);
        }
        return string.Format(CultureInfo.CurrentCulture, "{0} <{1}>", displayName.Trim(), this);
    }

    /// <summary>Serializes the email address to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Returns a <see cref="string"/> that represents the current email address for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0}");

    /// <summary>Returns a formatted <see cref="string"/> that represents the current email address.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// The formats:
    ///
    /// f: as formatted email address.
    /// U: as full email address uppercased.
    /// l: as local name.
    /// L: as local name uppercased.
    /// d: as domain.
    /// D: as domain uppercased.
    /// </remarks>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : StringFormatter.Apply(this, format.WithDefault("f"), formatProvider, FormatTokens);

    /// <summary>Gets an XML string representation of the email address.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>The format token instructions.</summary>
    private static readonly Dictionary<char, Func<EmailAddress, IFormatProvider, string>> FormatTokens = new()
    {
        { 'U', (svo, provider) => svo.m_Value?.ToUpper(provider) ?? string.Empty },
        { 'L', (svo, provider) => svo.Local.ToUpper(provider) },
        { 'D', (svo, provider) => svo.Domain.ToUpper(provider) },
        { 'l', (svo, _) => svo.Local },
        { 'd', (svo, _) => svo.Domain },
        { 'f', (svo, _) => svo.m_Value ?? string.Empty },
    };

    /// <summary>Converts the string to an email address.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing an email address to convert.
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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out EmailAddress result)
    {
        result = default;
        if (string.IsNullOrEmpty(s))
        {
            return true;
        }
        else if (Qowaiv.Unknown.IsUnknown(s, formatProvider as CultureInfo))
        {
            result = Unknown;
            return true;
        }
        else if (EmailParser.Parse(s) is string email)
        {
            result = new EmailAddress(email);
            return true;
        }
        else return false;
    }
}
