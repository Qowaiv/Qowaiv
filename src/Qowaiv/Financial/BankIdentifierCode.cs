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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Financial
{
    /// <summary>The Bank Identifier Code (BIC) is a standard format of Business Identifier Codes
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
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
    [SwaggerDataType(format: "bic", nullable: true)]
    [TypeConverter(typeof(BankIdentifierCodeTypeConverter))]
    public struct BankIdentifierCode : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<BankIdentifierCode>, IComparable, IComparable<BankIdentifierCode>
    {
        /// <remarks>
        /// http://www.codeproject.com/KB/recipes/bicRegexValidator.aspx
        /// </remarks>
        public static readonly Regex Pattern = new Regex(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Represents an empty/not set BIC.</summary>
        public static readonly BankIdentifierCode Empty;

        /// <summary>Represents an unknown (but set) BIC.</summary>
        public static readonly BankIdentifierCode Unknown = new BankIdentifierCode { m_Value = "ZZZZZZZZZZZ" };

        #region Properties

        /// <summary>The inner value of the BIC.</summary>
        private string m_Value;

        /// <summary>Gets the number of characters of BIC.</summary>
        public int Length => IsEmptyOrUnknown() ? 0 : m_Value.Length;

        /// <summary>Gets the institution code or bank code.</summary>
        public string BankCode => IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(0, 4);

        /// <summary>Gets the country code.</summary>
        public string CountryCode => IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(4, 2);

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
                return Country.Parse(CountryCode, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>Gets the location code.</summary>
        public string LocationCode => IsEmptyOrUnknown() ? string.Empty : m_Value.Substring(6, 2);

        /// <summary>Gets the branch code.</summary>
        /// <remarks>
        /// Is optional, XXX for primary office.
        /// </remarks>
        public string BranchCode => Length != 11 ? string.Empty : m_Value.Substring(8);

        #endregion

        #region Methods

        /// <summary>Returns true if the BIC is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default;

        /// <summary>Returns true if the BIC is unknown, otherwise false.</summary>
        public bool IsUnknown() => m_Value == Unknown.m_Value;

        /// <summary>Returns true if the BIC is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of BIC based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private BankIdentifierCode(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetString("Value");
        }

        /// <summary>Adds the underlying property of BIC to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a BIC.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the BIC from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of BIC.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the BIC to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of BIC.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a BIC from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => m_Value = default;


        /// <summary>Generates a BIC from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the BIC.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates a BIC from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the BIC.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger) => throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);

        /// <summary>Generates a BIC from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the BIC.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates a BIC from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the BIC.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a BIC into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return m_Value == default ? null : ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current BIC for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get
            {
                if (IsEmpty()) { return "BankIdentifierCode: (empty)"; }
                if (IsUnknown()) { return "BankIdentifierCode: (unknown)"; }
                return string.Format(CultureInfo.InvariantCulture, "BankIdentifierCode: {0}", m_Value);
            }
        }

        /// <summary>Returns a <see cref="string"/> that represents the current BIC.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current BIC.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current BIC.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("", formatProvider);
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

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) => obj is BankIdentifierCode && Equals((BankIdentifierCode)obj);

        /// <summary>Returns true if this instance and the other <see cref="BankIdentifierCode"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="BankIdentifierCode"/> to compare with.</param>
        public bool Equals(BankIdentifierCode other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this BIC.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value == null ? 0 : m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(BankIdentifierCode left, BankIdentifierCode right) => left.Equals(right);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(BankIdentifierCode left, BankIdentifierCode right) => !(left == right);

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a BIC.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a BIC.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is BankIdentifierCode)
            {
                return CompareTo((BankIdentifierCode)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a BIC"), "obj");
        }

        /// <summary>Compares this instance with a specified BIC and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified BIC.
        /// </summary>
        /// <param name="other">
        /// The BIC to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(BankIdentifierCode other) { return String.Compare(m_Value, other.m_Value, StringComparison.Ordinal); }

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a BIC to a <see cref="string"/>.</summary>
        public static explicit operator string(BankIdentifierCode val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a BIC.</summary>
        public static explicit operator BankIdentifierCode(string str) => Parse(str, CultureInfo.CurrentCulture);


        #endregion

        #region Factory methods

        /// <summary>Converts the string to a BIC.</summary>
        /// <param name="s">
        /// A string containing a BIC to convert.
        /// </param>
        /// <returns>
        /// A BIC.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static BankIdentifierCode Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to a BIC.</summary>
        /// <param name="s">
        /// A string containing a BIC to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A BIC.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static BankIdentifierCode Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out BankIdentifierCode val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionBankIdentifierCode);
        }

        /// <summary>Converts the string to a BIC.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a BIC to convert.
        /// </param>
        /// <returns>
        /// The BIC if the string was converted successfully, otherwise BankIdentifierCode.Empty.
        /// </returns>
        public static BankIdentifierCode TryParse(string s)
        {
            if (TryParse(s, out BankIdentifierCode val))
            {
                return val;
            }
            return Empty;
        }

        /// <summary>Converts the string to a BIC.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a BIC to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out BankIdentifierCode result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

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
        public static bool TryParse(string s, IFormatProvider formatProvider, out BankIdentifierCode result)
        {
            result = Empty;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            if (Qowaiv.Unknown.IsUnknown(s, formatProvider as CultureInfo))
            {
                result = Unknown;
                return true;
            }
            if (IsValid(s, formatProvider))
            {
                result = new BankIdentifierCode { m_Value = Parsing.ClearSpacingAndMarkupToUpper(s) };
                return true;
            }
            return false;
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid BIC, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);

        /// <summary>Returns true if the val represents a valid BIC, otherwise false.</summary>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "formatProvider is validated by Country.IsValid().")]
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            var value = val ?? string.Empty;
            return Pattern.IsMatch(value) && Country.IsValid(value.Substring(4, 2), formatProvider);
        }
        #endregion
    }
}
