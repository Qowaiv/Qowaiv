#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

using Qowaiv.Conversion.Financial;
using Qowaiv.Diagnostics;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Json;
using Qowaiv.Text;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv.Financial
{
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
    public partial struct BusinessIdentifierCode : ISerializable, IXmlSerializable, IFormattable, IEquatable<BusinessIdentifierCode>, IComparable, IComparable<BusinessIdentifierCode>
    {
        /// <remarks>
        /// http://www.codeproject.com/KB/recipes/bicRegexValidator.aspx
        /// </remarks>
        private static readonly Regex Pattern = new(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Represents an empty/not set BIC.</summary>
        public static readonly BusinessIdentifierCode Empty;

        /// <summary>Represents an unknown (but set) BIC.</summary>
        public static readonly BusinessIdentifierCode Unknown = new BusinessIdentifierCode("ZZZZZZZZZZZ");

        /// <summary>Gets the number of characters of BIC.</summary>
        public int Length => IsEmptyOrUnknown() ? 0 : m_Value.Length;

        /// <summary>Gets the institution code or business code.</summary>
        public string Business => IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(0, 4);

        /// <summary>Gets the country info of the country code.</summary>
        public Country Country
        {
            get
            {
                if (IsEmpty())
                {
                    return Country.Empty;
                }
                if (IsUnknown())
                {
                    return Country.Unknown;
                }
                return Country.Parse(m_Value.Substring(4, 2), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>Gets the location code.</summary>
        public string Location => IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(6, 2);

        /// <summary>Gets the branch code.</summary>
        /// <remarks>
        /// Is optional, XXX for primary office.
        /// </remarks>
        public string Branch => Length != 11 ? string.Empty : m_Value.Substring(8);

        /// <summary>Serializes the BIC to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        [Pure]
        public string ToJson() => m_Value;

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
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }
            if (IsEmpty()) { return string.Empty; }
            if (IsUnknown()) { return "?"; }
            return m_Value;
        }

        /// <summary>Gets an XML string representation of the BIC.</summary>
        [Pure]
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts a BIC to a <see cref="string"/>.</summary>
        public static explicit operator string(BusinessIdentifierCode val) => val.ToString(CultureInfo.CurrentCulture);
        
        /// <summary>Casts a <see cref="string"/> to a BIC.</summary>
        public static explicit operator BusinessIdentifierCode(string str) => Cast.String<BusinessIdentifierCode>(TryParse, str);

        /// <summary>Converts the string to a BIC.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a BIC to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out BusinessIdentifierCode result)
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
            else if (buffer.Matches(Pattern) && 
                !Country.TryParse(buffer.Substring(4, 2)).IsEmptyOrUnknown())
            {
                result = new BusinessIdentifierCode(buffer);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
