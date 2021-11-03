#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

using Qowaiv.Conversion;
using Qowaiv.Diagnostics;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents an email address.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
    [OpenApiDataType(description: "Email notation as defined by RFC 5322.", example: "svo@qowaiv.org", type: "string", format: "email", nullable: true)]
    [TypeConverter(typeof(EmailAddressTypeConverter))]
    public partial struct EmailAddress : ISerializable, IXmlSerializable, IFormattable, IEquatable<EmailAddress>, IComparable, IComparable<EmailAddress>
    {
        /// <summary>An email address must not exceed 254 characters.</summary>
        /// <remarks>
        /// https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
        /// </remarks>
        public const int MaxLength = 254;

        /// <summary>Represents an empty/not set email address.</summary>
        public static readonly EmailAddress Empty;

        /// <summary>Represents an unknown (but set) email address.</summary>
        public static readonly EmailAddress Unknown = new EmailAddress("?");

        /// <summary>Gets the number of characters of email address.</summary>
        public int Length => IsEmptyOrUnknown() ? 0 : m_Value.Length;

        /// <summary>Gets the local part of the Email Address.</summary>
        public string Local => IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(0, m_Value.IndexOf('@'));

        /// <summary>Gets the domain part of the Email Address.</summary>
        public string Domain => IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(m_Value.IndexOf('@') + 1);

        /// <summary>True if the domain part of the Email Address is an IP-address.</summary>
        /// <remarks>
        /// As IP-Addresses are normalized by the <see cref="EmailParser"/> it
        /// can simply be checked by checking the last character of the string
        /// value.
        /// </remarks>
        public bool IsIPBased => !IsEmptyOrUnknown() && m_Value[m_Value.Length - 1] == ']';

        /// <summary>Gets the IP-Address if the Email Address is IP-based, otherwise <see cref="IPAddress.None"/>.</summary>
        public IPAddress IPDomain
        {
            get
            {
                if (!IsIPBased) { return IPAddress.None; }

                var ip = Domain.StartsWith("[IPv6:", StringComparison.InvariantCulture)
                    ? Domain.Substring(6, Domain.Length - 7)
                    : Domain.Substring(1, Domain.Length - 2);

                return IPAddress.Parse(ip);
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
        public string ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

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
        public string ToString(string format, IFormatProvider formatProvider)
            => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
            ? formatted
            : StringFormatter.Apply(this, format.WithDefault("f"), formatProvider, FormatTokens);

        /// <summary>Gets an XML string representation of the email address.</summary>
        [Pure]
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>The format token instructions.</summary>
        private static readonly Dictionary<char, Func<EmailAddress, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<EmailAddress, IFormatProvider, string>>
        {
            { 'U', (svo, provider) => svo.m_Value.ToUpper(provider) },
            { 'l', (svo, provider) => svo.Local },
            { 'L', (svo, provider) => svo.Local.ToUpper(provider) },
            { 'd', (svo, provider) => svo.Domain },
            { 'D', (svo, provider) => svo.Domain.ToUpper(provider) },
            { 'f', (svo, provider) => svo.m_Value ?? string.Empty },
        };

        /// <summary>Casts an email address to a <see cref="string"/>.</summary>
        public static explicit operator string(EmailAddress val) => val.ToString(CultureInfo.CurrentCulture);
        
        /// <summary>Casts a <see cref="string"/> to a email address.</summary>
        public static explicit operator EmailAddress(string str) => Cast.String<EmailAddress>(TryParse, str);

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
        public static bool TryParse(string s, IFormatProvider formatProvider, out EmailAddress result)
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
            else { return false; }
        }
    }
}
