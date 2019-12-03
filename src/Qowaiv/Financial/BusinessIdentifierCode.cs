#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Financial;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
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
    [OpenApiDataType(description: "Business Identifier Code, as defined by ISO 9362, for example, DEUTDEFF.", type: "string", format: "bic", nullable: true)]
    [TypeConverter(typeof(BusinessIdentifierCodeTypeConverter))]
    public partial struct BusinessIdentifierCode : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<BusinessIdentifierCode>, IComparable, IComparable<BusinessIdentifierCode>
    {
        /// <remarks>
        /// http://www.codeproject.com/KB/recipes/bicRegexValidator.aspx
        /// </remarks>
        public static readonly Regex Pattern = new Regex(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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

        /// <summary>Generates a BIC from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => m_Value = default;

        /// <summary>Generates a BIC from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the BIC.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString) => m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;

        /// <summary>Generates a BIC from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the BIC.
        /// </param>
        void IJsonSerializable.FromJson(long jsonInteger) => throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);

        /// <summary>Generates a BIC from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the BIC.
        /// </param>
        void IJsonSerializable.FromJson(double jsonNumber) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates a BIC from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the BIC.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a BIC into its JSON object representation.</summary>
        object IJsonSerializable.ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a <see cref="string"/> that represents the current BIC for debug purposes.</summary>
        /// <summary>Returns a <see cref="string"/> that represents the current BIC for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => IsEmpty() ? "{empty}" : ToString(CultureInfo.InvariantCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current BIC.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
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
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts a BIC to a <see cref="string"/>.</summary>
        public static explicit operator string(BusinessIdentifierCode val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a BIC.</summary>
        public static explicit operator BusinessIdentifierCode(string str) => Parse(str, CultureInfo.CurrentCulture);

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
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            if (Qowaiv.Unknown.IsUnknown(s, formatProvider as CultureInfo))
            {
                result = Unknown;
                return true;
            }
            if (Pattern.IsMatch(s))
            {
                result = new BusinessIdentifierCode { m_Value = Parsing.ClearSpacingAndMarkupToUpper(s) };
                return true;
            }
            return false;
        }
    }
}
