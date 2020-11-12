#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Financial;
using Qowaiv.Diagnostics;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Json;
using Qowaiv.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv.Financial
{
    ///<summary>The International Bank Account Number (IBAN) is an international standard
    /// for identifying bank accounts across national borders with a minimal risk
    /// of propagating transcription errors. It was originally adopted by the European
    /// Committee for Banking Standards (ECBS), and was later adopted as an international
    /// standard under ISO 13616:1997 and now as ISO 13616-1:2007.
    /// </summary>
    /// <remarks>
    /// The official IBAN registrar under ISO 13616-2:2007 is SWIFT.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
    [OpenApiDataType(description: "International Bank Account Number notation as defined by ISO 13616:2007, for example, BE71096123456769.", type: "string", format: "iban", nullable: true)]
    [TypeConverter(typeof(InternationalBankAccountNumberTypeConverter))]
    public partial struct InternationalBankAccountNumber : ISerializable, IXmlSerializable, IFormattable, IEquatable<InternationalBankAccountNumber>, IComparable, IComparable<InternationalBankAccountNumber>
    {
        /// <summary>Represents the pattern of a (potential) valid IBAN.</summary>
        /// <remarks>
        /// Pairs of IBAN characters can be divided by maximum 2 spacing characters.
        /// </remarks>
        public static readonly Regex Pattern = new Regex(@"^[A-Z]\s{0,2}[A-Z]\s{0,2}[0-9]\s{0,2}[0-9](\s{0,2}[0-9A-Z]){8,32}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Represents an empty/not set IBAN.</summary>
        public static readonly InternationalBankAccountNumber Empty;

        /// <summary>Represents an unknown (but set) IBAN.</summary>
        public static readonly InternationalBankAccountNumber Unknown = new InternationalBankAccountNumber("ZZ");

        /// <summary>Gets the number of characters of IBAN.</summary>
        public int Length => IsEmptyOrUnknown() ? 0 : m_Value.Length;

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
                    return Country.Parse(m_Value.Substring(0, 2), CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>Serializes the IBAN to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => m_Value == default ? null : ToUnformattedString();

        /// <summary>Returns a <see cref="string"/> that represents the current IBAN for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

        /// <summary>Formats the IBAN without spaces.</summary>
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
        private string ToUnformattedLowercaseString() => ToUnformattedString().ToLowerInvariant();

        /// <summary>Formats the IBAN with spaces.</summary>
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
            return FormattedPattern.Replace(m_Value, "$0 ");
        }
        
        /// <summary>Formats the IBAN with spaces as lowercase.</summary>
        private string ToFormattedLowercaseString() => ToFormattedString().ToLowerInvariant();

        private static readonly Regex FormattedPattern = new Regex(@"\w{4}(?!$)", RegexOptions.Compiled);

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
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }
            // If no format specified, use the default format.
            if (string.IsNullOrEmpty(format))
            {
                return ToUnformattedString();
            }
            // Apply the format.
            return StringFormatter.Apply(this, format, formatProvider, FormatTokens);
        }

        /// <summary>The format token instructions.</summary>
        private static readonly Dictionary<char, Func<InternationalBankAccountNumber, IFormatProvider, string>> FormatTokens = new Dictionary<char, Func<InternationalBankAccountNumber, IFormatProvider, string>>
        {
            { 'u', (svo, provider) => svo.ToUnformattedLowercaseString() },
            { 'U', (svo, provider) => svo.ToUnformattedString() },
            { 'f', (svo, provider) => svo.ToFormattedLowercaseString() },
            { 'F', (svo, provider) => svo.ToFormattedString() },
        };

        /// <summary>Gets an XML string representation of the IBAN.</summary>
        private string ToXmlString() => ToUnformattedString();

        /// <summary>Casts an IBAN to a <see cref="string"/>.</summary>
        public static explicit operator string(InternationalBankAccountNumber val) => val.ToString(CultureInfo.InvariantCulture);
        
        /// <summary>Casts a <see cref="string"/> to a IBAN.</summary>
        public static explicit operator InternationalBankAccountNumber(string str) => Cast.String<InternationalBankAccountNumber>(TryParse, str);

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
        public static bool TryParse(string s, IFormatProvider formatProvider, out InternationalBankAccountNumber result)
        {
            result = default;
            var buffer = s.Buffer().Unify();
            if (buffer.IsEmpty())
            {
                return true;
            }
            else if (buffer.IsUnknown(formatProvider))
            {
                result = Unknown;
                return true;
            }
            else if (buffer.Length >= 12 
                && buffer.Length <= 32
                && buffer.Matches(Pattern)
                && ValidForCountry(buffer) 
                && (Mod97(buffer)))
            {
                result = new InternationalBankAccountNumber(buffer);
                return true;
            }
            return false;
        }

        private static bool ValidForCountry(CharBuffer buffer)
        {
            var country = Country.TryParse(buffer.Substring(0, 2));
            return !country.IsEmptyOrUnknown()
                && (!LocalizedPatterns.TryGetValue(country, out Regex localizedPattern)
                || buffer.Matches(localizedPattern));
        }

        private static bool Mod97(CharBuffer buffer)
        {
            var digits = Digits(buffer);
            int sum = 0;
            int exp = 1;

            for (int pos = digits.Length - 1; pos >= 0; pos--)
            {
                sum += exp * AlphanumericAndNumericLookup.IndexOf(digits[pos]);
                exp = (exp * 10) % 97;
            }
            return sum % 97 == 1;
        }

        private static CharBuffer Digits(CharBuffer input)
        {
            var digits = CharBuffer.Empty(input.Length * 2);
            foreach(var ch in input.Skip(4).Concat(input.Take(4)))
            {
                digits.Add(AlphanumericAndNumericLookup.IndexOf(ch).ToString());
            }
            return digits;
        }

        private const string AlphanumericAndNumericLookup = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
}
