using Qowaiv.Conversion.Sql;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Qowaiv.Sql
{
    /// <summary>Represents a time stamp.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(UInt64))]
    [TypeConverter(typeof(TimestampTypeConverter))]
    public struct Timestamp : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<Timestamp>, IComparable, IComparable<Timestamp>
    {
        /// <summary>Gets the minimum value of a time stamp.</summary>
        public static readonly Timestamp MinValue = UInt64.MinValue;

        /// <summary>Gets the maximum value of a time stamp.</summary>
        public static readonly Timestamp MaxValue = UInt64.MaxValue;

        #region Properties

        /// <summary>The inner value of the time stamp.</summary>
        private UInt64 m_Value;

        #endregion

        #region Methods

        /// <summary>Represents the time stamp .</summary>
        public byte[] ToByteArray() { return BitConverter.GetBytes(m_Value); }

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of time stamp based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Timestamp(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetUInt64("Value");
        }

        /// <summary>Adds the underlying property of time stamp to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a time stamp.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the time stamp from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of time stamp.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(writer, nameof(writer));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the time stamp to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of time stamp.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a time stamp from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => throw new NotSupportedException(QowaivMessages.JsonSerialization_NullNotSupported);

        /// <summary>Generates a time stamp from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the time stamp.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates a time stamp from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the time stamp.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger)
        {
            m_Value = Create(jsonInteger).m_Value;
        }

        /// <summary>Generates a time stamp from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the time stamp.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber)
        {
            m_Value = Create((Int64)jsonNumber).m_Value;
        }

        /// <summary>Generates a time stamp from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the time stamp.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);

        /// <summary>Converts a time stamp into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current time stamp for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay { get { return ToString(CultureInfo.InvariantCulture); } }

        /// <summary>Returns a <see cref="string"/> that represents the current time stamp.</summary>
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current time stamp.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current time stamp.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(string.Empty, formatProvider);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current time stamp.</summary>
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
            if (string.IsNullOrEmpty(format))
            {
                return string.Format(formatProvider, "0x{0:X16}", m_Value);
            }
            return m_Value.ToString(format, formatProvider);
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) { return obj is Timestamp && Equals((Timestamp)obj); }

        /// <summary>Returns true if this instance and the other <see cref="Timestamp"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="Timestamp"/> to compare with.</param>
        public bool Equals(Timestamp other) => m_Value == other.m_Value;


        /// <summary>Returns the hash code for this time stamp.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override int GetHashCode() => m_Value.GetHashCode();
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(Timestamp left, Timestamp right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(Timestamp left, Timestamp right)
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
        /// An object that evaluates to a time stamp.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a time stamp.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is Timestamp)
            {
                return CompareTo((Timestamp)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a time stamp"), "obj");
        }

        /// <summary>Compares this instance with a specified time stamp and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified time stamp.
        /// </summary>
        /// <param name="other">
        /// The time stamp to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(Timestamp other) => m_Value.CompareTo(other.m_Value);

        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Timestamp l, Timestamp r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(Timestamp l, Timestamp r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Timestamp l, Timestamp r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Timestamp l, Timestamp r) => l.CompareTo(r) >= 0;

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a time stamp to a <see cref="string"/>.</summary>
        public static explicit operator string(Timestamp val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a time stamp.</summary>
        public static explicit operator Timestamp(string str) => Parse(str, CultureInfo.CurrentCulture);



        /// <summary>Casts a time stamp to a System.Int32.</summary>
        public static explicit operator byte[] (Timestamp val) { return val.ToByteArray(); }
        /// <summary>Casts an System.Int32 to a time stamp.</summary>
        public static explicit operator Timestamp(byte[] val) => Create(val);

        /// <summary>Casts a time stamp to a System.Int64.</summary>
        public static explicit operator Int64(Timestamp val) { return BitConverter.ToInt64(val.ToByteArray(), 0); }
        /// <summary>Casts a System.Int64 to a time stamp.</summary>
        public static explicit operator Timestamp(Int64 val) => Create(val);

        /// <summary>Casts a time stamp to a System.UInt64.</summary>
        [CLSCompliant(false)]
        public static explicit operator UInt64(Timestamp val) => val.m_Value;
        /// <summary>Casts a System.UInt64 to a time stamp.</summary>
        [CLSCompliant(false)]
        public static implicit operator Timestamp(UInt64 val) => Create(val);
        #endregion

        #region Factory methods

        /// <summary>Converts the string to a time stamp.</summary>
        /// <param name="s">
        /// A string containing a time stamp to convert.
        /// </param>
        /// <returns>
        /// A time stamp.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Timestamp Parse(string s)
        {
            return Parse(s, CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the string to a time stamp.</summary>
        /// <param name="s">
        /// A string containing a time stamp to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A time stamp.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Timestamp Parse(string s, IFormatProvider formatProvider)
        {
            Timestamp val;
            if (TryParse(s, formatProvider, out val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionTimestamp);
        }

        /// <summary>Converts the string to a time stamp.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a time stamp to convert.
        /// </param>
        /// <returns>
        /// The time stamp if the string was converted successfully, otherwise MinValue.
        /// </returns>
        public static Timestamp TryParse(string s)
        {
            Timestamp val;
            if (TryParse(s, out val))
            {
                return val;
            }
            return MinValue;
        }

        /// <summary>Converts the string to a time stamp.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a time stamp to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Timestamp result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to a time stamp.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a time stamp to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out Timestamp result)
        {
            result = MinValue;
            UInt64 val;

            if (string.IsNullOrEmpty(s)) { return false; }
            if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (UInt64.TryParse(s.Substring(2), NumberStyles.HexNumber, formatProvider, out val))
                {
                    result = Create(val);
                    return true;
                }
            }
            else if (UInt64.TryParse(s, NumberStyles.Number, formatProvider, out val))
            {
                result = Create(val);
                return true;
            }
            return false;
        }

        /// <summary>Creates a time stamp from a Int64. </summary >
        /// <param name="val" >
        /// A decimal describing a time stamp.
        /// </param >
        [CLSCompliant(false)]
        public static Timestamp Create(UInt64 val)
        {
            return new Timestamp { m_Value = val };
        }

        /// <summary>Creates a time stamp from a Int64. </summary >
        /// <param name="val" >
        /// A decimal describing a time stamp.
        /// </param >
        public static Timestamp Create(Int64 val)
        {
            return Create(BitConverter.GetBytes(val));
        }

        /// <summary>Creates a time stamp from a Int64. </summary >
        /// <param name="bytes" >
        /// A byte array describing a time stamp.
        /// </param >
        public static Timestamp Create(byte[] bytes)
        {
            Guard.HasAny(bytes, nameof(bytes));
            if (bytes.Length != 8) { throw new ArgumentException(QowaivMessages.ArgumentException_TimestampArrayShouldHaveSize8, "bytes"); }

            return Create(BitConverter.ToUInt64(bytes, 0));
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid time stamp, otherwise false.</summary>
        public static bool IsValid(string val)
        {
            return IsValid(val, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns true if the val represents a valid time stamp, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider) => TryParse(val, formatProvider, out Timestamp timestamp);

        #endregion
    }
}
