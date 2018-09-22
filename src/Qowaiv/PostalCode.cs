#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a postal code.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "The < and > operators have no meaning for a postal code.")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
    [TypeConverter(typeof(PostalCodeTypeConverter))]
    public struct PostalCode : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<PostalCode>, IComparable, IComparable<PostalCode>
    {
        /// <summary>Represents the pattern of a (potential) valid postal code.</summary>
        public static readonly Regex Pattern = new Regex(@"^.{2,10}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Represents an empty/not set postal code.</summary>
        public static readonly PostalCode Empty;

        /// <summary>Represents an unknown (but set) postal code.</summary>
        public static readonly PostalCode Unknown = new PostalCode { m_Value = "ZZZZZZZZZ" };

        #region Properties

        /// <summary>The inner value of the postal code.</summary>
        private string m_Value;

        /// <summary>Gets the number of characters of postal code.</summary>
        public int Length { get { return m_Value == null ? 0 : m_Value.Length; } }

        #endregion

        #region Methods

        /// <summary>Returns true if the postal code is empty, otherwise false.</summary>
        public bool IsEmpty() => m_Value == default(string);

        /// <summary>Returns true if the postal code is unknown, otherwise false.</summary>
        public bool IsUnknown() { return m_Value == PostalCode.Unknown.m_Value; }

        /// <summary>Returns true if the postal code is empty or unknown, otherwise false.</summary>
        public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();

        /// <summary>Returns true if the postal code is valid for the specified country, otherwise false.</summary>
        /// <param name="country">
        /// The country to valid for.
        /// </param>
        /// <remarks>
        /// Returns false if the country does not have postal codes.
        /// </remarks>
        public bool IsValid(Country country)
        {
            return IsValid(m_Value, country);
        }

        /// <summary>Returns a collection countries where the postal code is valid for.</summary>
        public IEnumerable<Country> IsValidFor()
        {
            var postalcode = m_Value;
            return Country.All.Where(country => IsValid(postalcode, country));
        }

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of postal code based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private PostalCode(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetString("Value");
        }

        /// <summary>Adds the underlying property of postal code to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a postal code.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the postal code from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of postal code.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the postal code to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of postal code.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString("xml", CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a postal code from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson()
        {
            m_Value = default(string);
        }

        /// <summary>Generates a postal code from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the postal code.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates a postal code from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the postal code.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger) => new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);

        /// <summary>Generates a postal code from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the postal code.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber) => new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates a postal code from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the postal code.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a postal code into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return m_Value;
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current postal code for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "PostalCode: {0}{1}",
                    ToString(),
                    this == Empty ? "(empty)" : "");
            }
        }

        /// <summary>Returns a <see cref="string"/> that represents the current postal code.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current postal code.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);


        /// <summary>Returns a formatted <see cref="string"/> that represents the current postal code.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("", formatProvider);


        /// <summary>Returns a formatted <see cref="string"/> that represents the current postal code.</summary>
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
            return ToString(Country.TryParse(format));
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current postal code.</summary>
        /// <param name="country">
        /// The country to format for.
        /// </param>
        /// <remarks>
        /// If the postal code is not valid for the country,
        /// or the country does not have special formattings, 
        /// the unformatted value is returned.
        /// </remarks>
        public string ToString(Country country)
        {
            // send a question mark in case of Unknown.
            var normalized = Unknown.m_Value == m_Value ? "?" : m_Value;
            return PostalCodeCountryInfo.GetInstance(country).Format(normalized);
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) { return obj is PostalCode && Equals((PostalCode)obj); }

        /// <summary>Returns true if this instance and the other <see cref="PostalCode"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="PostalCode"/> to compare with.</param>
        public bool Equals(PostalCode other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this postal code.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value == null ? 0 : m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(PostalCode left, PostalCode right) => left.Equals(right);


        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(PostalCode left, PostalCode right) => !(left == right);

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a postal code.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a postal code.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is PostalCode)
            {
                return CompareTo((PostalCode)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a postal code"), "obj");
        }

        /// <summary>Compares this instance with a specified postal code and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified postal code.
        /// </summary>
        /// <param name="other">
        /// The postal code to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(PostalCode other) => string.Compare(m_Value, other.m_Value, StringComparison.Ordinal);

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a postal code to a <see cref="string"/>.</summary>
        public static explicit operator string(PostalCode val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a postal code.</summary>
        public static explicit operator PostalCode(string str) => Parse(str, CultureInfo.CurrentCulture);


        #endregion

        #region Factory methods

        /// <summary>Converts the string to a postal code.</summary>
        /// <param name="s">
        /// A string containing a postal code to convert.
        /// </param>
        /// <returns>
        /// A postal code.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static PostalCode Parse(string s)
        {
            return Parse(s, CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the string to a postal code.</summary>
        /// <param name="s">
        /// A string containing a postal code to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A postal code.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static PostalCode Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out PostalCode val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionPostalCode);
        }

        /// <summary>Converts the string to a postal code.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a postal code to convert.
        /// </param>
        /// <returns>
        /// The postal code if the string was converted successfully, otherwise PostalCode.Empty.
        /// </returns>
        public static PostalCode TryParse(string s)
        {
            if (TryParse(s, out PostalCode val))
            {
                return val;
            }
            return Empty;
        }

        /// <summary>Converts the string to a postal code.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a postal code to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out PostalCode result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to a postal code.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a postal code to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out PostalCode result)
        {
            result = Empty;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            var culture = formatProvider as CultureInfo ?? CultureInfo.InvariantCulture;
            if (Qowaiv.Unknown.IsUnknown(s, culture))
            {
                result = Unknown;
                return true;
            }
            if (IsValid(s, formatProvider))
            {
                result = new PostalCode { m_Value = Parsing.ClearSpacingAndMarkupToUpper(s) };
                return true;
            }
            return false;
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid postal code, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);


        /// <summary>Returns true if the val represents a valid postal code, otherwise false.</summary>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "formatProvider",
            Justification = "Satisfies the static Qowaiv SVO contract.")]
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            return Pattern.IsMatch(val ?? string.Empty);
        }

        /// <summary>Returns true if the postal code is valid for the specified country, otherwise false.</summary>
        /// <param name="postalcode">
        /// The postal code to test.
        /// </param>
        /// <param name="country">
        /// The country to valid for.
        /// </param>
        /// <remarks>
        /// Returns false if the country does not have postal codes.
        /// </remarks>
        public static bool IsValid(string postalcode, Country country)
        {
            return PostalCodeCountryInfo.GetInstance(country).IsValid(postalcode);
        }

        #endregion
    }
}
