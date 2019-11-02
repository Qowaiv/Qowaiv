#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Financial;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Financial
{
    /// <summary>Represents an </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(decimal))]
    [OpenApiDataType(description: "Decimal representation of a currency amount.", type: "number", format: "amount")]
    [TypeConverter(typeof(AmountTypeConverter))]
    public struct Amount : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<Amount>, IComparable, IComparable<Amount>
    {
        /// <summary>Represents an Amount of zero.</summary>
        public static readonly Amount Zero;
        /// <summary>Represents the smallest possible value of an </summary>
        public static readonly Amount MinValue = decimal.MinValue;
        /// <summary>Represents the biggest possible value of an </summary>
        public static readonly Amount MaxValue = decimal.MaxValue;

        #region Properties

        /// <summary>The inner value of the </summary>
        private decimal m_Value;

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of Amount based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Amount(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetDecimal("Value");
        }

        /// <summary>Adds the underlying property of Amount to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the Amount from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of 
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the Amount to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of 
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates an Amount from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => throw new NotSupportedException(QowaivMessages.JsonSerialization_NullNotSupported);

        /// <summary>Generates an Amount from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the 
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates an Amount from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the 
        /// </param>
        void IJsonSerializable.FromJson(long jsonInteger)
        {
            m_Value = jsonInteger;
        }

        /// <summary>Generates an Amount from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the 
        /// </param>
        void IJsonSerializable.FromJson(double jsonNumber)
        {
            m_Value = (decimal)jsonNumber;
        }

        /// <summary>Generates an Amount from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the 
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);


        /// <summary>Converts an Amount into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return m_Value;
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current Amount for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "¤{0:0.00########}", m_Value);
            }
        }

        /// <summary>Returns a <see cref="string"/> that represents the current </summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("", formatProvider);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
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
            var info = Money.GetNumberFormatInfo(formatProvider);
            return m_Value.ToString(format, info);
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) { return obj is Amount && Equals((Amount)obj); }

        /// <summary>Returns true if this instance and the other <see cref="Amount"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="Amount"/> to compare with.</param>
        public bool Equals(Amount other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(Amount left, Amount right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(Amount left, Amount right)
        {
            return !(left == right);
        }

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a 
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a 
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is Amount)
            {
                return CompareTo((Amount)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "an Amount"), "obj");
        }

        /// <summary>Compares this instance with a specified Amount and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified 
        /// </summary>
        /// <param name="other">
        /// The Amount to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(Amount other) => m_Value.CompareTo(other.m_Value);


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Amount l, Amount r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(Amount l, Amount r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Amount l, Amount r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Amount l, Amount r) => l.CompareTo(r) >= 0;

        #endregion

        #region (Explicit) casting

        /// <summary>Casts an Amount to a <see cref="string"/>.</summary>
        public static explicit operator string(Amount val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a </summary>
        public static explicit operator Amount(string str) => Parse(str, CultureInfo.CurrentCulture);


        /// <summary>Casts a decimal an </summary>
        public static implicit operator Amount(decimal val) => Create(val);
        /// <summary>Casts a decimal an </summary>
        public static implicit operator Amount(double val) => Create(val);
        /// <summary>Casts a long an </summary>
        public static implicit operator Amount(long val) => Create((decimal)val);
        /// <summary>Casts a int an </summary>
        public static implicit operator Amount(int val) => Create((decimal)val);

        /// <summary>Casts an Amount to a decimal.</summary>
        public static explicit operator decimal(Amount val) => val.m_Value;
        /// <summary>Casts an Amount to a double.</summary>
        public static explicit operator double(Amount val) => (double)val.m_Value;
        /// <summary>Casts an Amount to a long.</summary>
        public static explicit operator long(Amount val) => (long)val.m_Value;
        /// <summary>Casts an Amount to a int.</summary>
        public static explicit operator int(Amount val) => (int)val.m_Value;

        #endregion

        #region Factory methods

        /// <summary>Converts the string to an </summary>
        /// <param name="s">
        /// A string containing an Amount to convert.
        /// </param>
        /// <returns>
        /// An 
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Amount Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to an </summary>
        /// <param name="s">
        /// A string containing an Amount to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// An 
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Amount Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out Amount val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionFinancialAmount);
        }

        /// <summary>Converts the string to an 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an Amount to convert.
        /// </param>
        /// <returns>
        /// The Amount if the string was converted successfully, otherwise Empty.
        /// </returns>
        public static Amount TryParse(string s)
        {
            if (TryParse(s, out Amount val))
            {
                return val;
            }
            return Zero;
        }

        /// <summary>Converts the string to an 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an Amount to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Amount result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to an 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an Amount to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out Amount result)
        {
            result = Zero;
            if (Money.TryParse(s, formatProvider, out Money money))
            {
                result = (decimal)money;
                return true;
            }
            return false;
        }

        /// <summary>Creates an Amount from a Decimal.</summary >
        /// <param name="val" >
        /// A decimal describing an Amount.
        /// </param >
        public static Amount Create(decimal val) => new Amount { m_Value = val };

        /// <summary>Creates an Amount from a Double.</summary >
        /// <param name="val" >
        /// A decimal describing an Amount.
        /// </param >
        public static Amount Create(double val) => Create((decimal)val);

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid Amount, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);

        /// <summary>Returns true if the val represents a valid Amount, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            return TryParse(val, formatProvider, out _);
        }

        #endregion
    }
}
