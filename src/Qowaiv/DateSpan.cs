#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;

namespace Qowaiv
{
    /// <summary>Represents a date span.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All, typeof(ulong))]
    [TypeConverter(typeof(DateSpanTypeConverter))]
    public struct DateSpan : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<DateSpan>, IComparable, IComparable<DateSpan>
    {
        /// <summary>Represents an empty/not set date span.</summary>
        public static readonly DateSpan Empty;

        public DateSpan(int years, int months, int days)
        {
            var total = (uint)(months + years * 12);

            m_Value = (uint)days | ((ulong)total << 32);
        }

        #region Properties

        /// <summary>The inner value of the date span.</summary>
        private ulong m_Value;

        public int TotalMonths => (int)(m_Value >> 32);

        public int Years => TotalMonths / 12;

        public int Months => TotalMonths % 12;

        public int Days => (int)m_Value;

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of date span based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private DateSpan(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetUInt64("Value");
        }

        /// <summary>Adds the underlying property of date span to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema" /> to (de) XML serialize a date span.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the date span from an <see href="XmlReader" />.</summary>
        /// <remarks>
        /// Uses the string parse function of date span.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the date span to an <see href="XmlWriter" />.</summary>
        /// <remarks>
        /// Uses the string representation of date span.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a date span from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => m_Value = default;

        /// <summary>Generates a date span from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the date span.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString) => m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;

        /// <summary>Generates a date span from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the date span.
        /// </param>
        void IJsonSerializable.FromJson(long jsonInteger) => throw new NotSupportedException(QowaivMessages.JsonSerialization_Int64NotSupported);
        // m_Value = Create(jsonInteger).m_Value;

        /// <summary>Generates a date span from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the date span.
        /// </param>
        void IJsonSerializable.FromJson(double jsonNumber) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);
        // m_Value = Create(jsonNumber).m_Value;

        /// <summary>Generates a date span from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the date span.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DateTimeNotSupported);
        // m_Value = Create(jsonDate).m_Value;

        /// <summary>Converts a date span into its JSON object representation.</summary>
        object IJsonSerializable.ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string" /> that represents the current date span for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => string.Format(CultureInfo.InvariantCulture, "{0:#,###0} Months, {1:#,###0} Days", TotalMonths, Days);

        /// <summary>Returns a <see cref="string" /> that represents the current date span.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string" /> that represents the current date span.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string" /> that represents the current date span.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider) => ToString("", formatProvider);

        /// <summary>Returns a formatted <see cref="string" /> that represents the current date span.</summary>
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
            throw new NotImplementedException();
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj) => obj is DateSpan && Equals((DateSpan)obj);

        /// <summary>Returns true if this instance and the other <see cref="DateSpan" /> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="DateSpan" /> to compare with.</param>
        public bool Equals(DateSpan other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this date span.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(DateSpan left, DateSpan right) => left.Equals(right);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(DateSpan left, DateSpan right) => !(left == right);

        #endregion

        #region IComparable

        /// <summary>Compares this instance with a specified System.Object and indicates whether
        /// this instance precedes, follows, or appears in the same position in the sort
        /// order as the specified System.Object.
        /// </summary>
        /// <param name="obj">
        /// An object that evaluates to a date span.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a date span.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is DateSpan)
            {
                return CompareTo((DateSpan)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a date span"), "obj");
        }

        /// <summary>Compares this instance with a specified date span and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified date span.
        /// </summary>
        /// <param name="other">
        /// The date span to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(DateSpan other) => m_Value.CompareTo(other.m_Value);


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(DateSpan l, DateSpan r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(DateSpan l, DateSpan r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(DateSpan l, DateSpan r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(DateSpan l, DateSpan r) => l.CompareTo(r) >= 0;

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a date span to a <see cref="string" />.</summary>
        public static explicit operator string(DateSpan val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string" /> to a date span.</summary>
        public static explicit operator DateSpan(string str) => DateSpan.Parse(str, CultureInfo.CurrentCulture);


        #endregion

        #region Factory methods

        public static DateSpan Age(Date reference) => Delta(reference, Clock.Today());

        public static DateSpan Delta(Date start, Date end)
        {
            var years = end.Year - start.Year;
            var months = end.Month - start.Month;
            var days = end.Day - start.Day;

            return new DateSpan(years, months, days);
        }

        /// <summary>Converts the string to a date span.</summary>
        /// <param name="s">
        /// A string containing a date span to convert.
        /// </param>
        /// <returns>
        /// A date span.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static DateSpan Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to a date span.</summary>
        /// <param name="s">
        /// A string containing a date span to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A date span.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static DateSpan Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out DateSpan val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionDateSpan);
        }

        /// <summary>Converts the string to a date span.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a date span to convert.
        /// </param>
        /// <returns>
        /// The date span if the string was converted successfully, otherwise Empty.
        /// </returns>
        public static DateSpan TryParse(string s)
        {
            if (TryParse(s, out DateSpan val))
            {
                return val;
            }
            return Empty;
        }

        /// <summary>Converts the string to a date span.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a date span to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out DateSpan result) => TryParse(s, CultureInfo.CurrentCulture, out result);

        /// <summary>Converts the string to a date span.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a date span to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out DateSpan result)
        {
            result = new DateSpan { m_Value = ulong.Parse(s, formatProvider) };
            return true;
        }

        #endregion
    }
}
