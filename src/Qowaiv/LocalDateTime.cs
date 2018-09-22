#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
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

namespace Qowaiv
{
    /// <summary>Represents a local date time.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(DateTime))]
    [TypeConverter(typeof(LocalDateTimeTypeConverter))]
    public struct LocalDateTime : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<LocalDateTime>, IComparable, IComparable<LocalDateTime>
    {
        private const string SerializableFormat = @"yyyy-MM-dd HH:mm:ss.FFFFFFF";

        /// <summary>Represents the smallest possible value of date. This field is read-only.</summary>
        public static readonly LocalDateTime MinValue = new LocalDateTime(DateTime.MinValue);

        /// <summary>Represents the largest possible value date. This field is read-only.</summary>
        public static readonly LocalDateTime MaxValue = new LocalDateTime(DateTime.MaxValue);

        #region Constructors

        /// <summary>Initializes a new instance of the local date time structure to a specified number of ticks.</summary>
        /// <param name="ticks">
        /// A date expressed in 100-nanosecond units.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// ticks is less than System.DateTime.MinValue or greater than System.DateTime.MaxValue.
        /// </exception>
        public LocalDateTime(long ticks)
            : this(new DateTime(ticks)) { }

        /// <summary>Initializes a new instance of the local date time structure based on a System.DateTime.
        /// </summary>
        /// <param name="dt">
        /// A date and time.
        /// </param>
        /// <remarks>
        /// The date of the date time is taken.
        /// </remarks>
        private LocalDateTime(DateTime dt) : this(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond) { }

        /// <summary>Initializes a new instance of the date structure to the specified year, month, and day.</summary>
        /// <param name="year">
        /// The year (1 through 9999).
        /// </param>
        /// <param name="month">
        /// The month (1 through 12).
        /// </param>
        /// <param name="day">
        /// The day (1 through the number of days in month).
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// year is less than 1 or greater than 9999.-or- month is less than 1 or greater
        /// than 12.-or- day is less than 1 or greater than the number of days in month.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The specified parameters evaluate to less than date.MinValue or
        /// more than date.MaxValue.
        /// </exception>
        public LocalDateTime(int year, int month, int day) : this(year, month, day, 0, 0) { }

        /// <summary>Initializes a new instance of the date structure to the specified year, month, and day.</summary>
        /// <param name="year">
        /// The year (1 through 9999).
        /// </param>
        /// <param name="month">
        /// The month (1 through 12).
        /// </param>
        /// <param name="day">
        /// The day (1 through the number of days in month).
        /// </param>
        /// <param name="hour">
        /// The hours (0 through 23).
        /// </param>
        /// <param name="minute">
        /// The minutes (0 through 59).
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// year is less than 1 or greater than 9999.-or- month is less than 1 or greater
        /// than 12.-or- day is less than 1 or greater than the number of days in month.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The specified parameters evaluate to less than date.MinValue or
        /// more than date.MaxValue.
        /// </exception>
        public LocalDateTime(int year, int month, int day, int hour, int minute)
            : this(year, month, day, hour, minute, 0) { }

        /// <summary>Initializes a new instance of the date structure to the specified year, month, and day.</summary>
        /// <param name="year">
        /// The year (1 through 9999).
        /// </param>
        /// <param name="month">
        /// The month (1 through 12).
        /// </param>
        /// <param name="day">
        /// The day (1 through the number of days in month).
        /// </param>
        /// <param name="hour">
        /// The hours (0 through 23).
        /// </param>
        /// <param name="minute">
        /// The minutes (0 through 59).
        /// </param>
        /// <param name="second">
        /// The seconds (0 through 59).
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// year is less than 1 or greater than 9999.-or- month is less than 1 or greater
        /// than 12.-or- day is less than 1 or greater than the number of days in month.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The specified parameters evaluate to less than date.MinValue or
        /// more than date.MaxValue.
        /// </exception>
        public LocalDateTime(int year, int month, int day, int hour, int minute, int second)
            : this(year, month, day, hour, minute, second, 0) { }

        /// <summary>Initializes a new instance of the date structure to the specified year, month, and day.</summary>
        /// <param name="year">
        /// The year (1 through 9999).
        /// </param>
        /// <param name="month">
        /// The month (1 through 12).
        /// </param>
        /// <param name="day">
        /// The day (1 through the number of days in month).
        /// </param>
        /// <param name="hour">
        /// The hours (0 through 23).
        /// </param>
        /// <param name="minute">
        /// The minutes (0 through 59).
        /// </param>
        /// <param name="second">
        /// The seconds (0 through 59).
        /// </param>
        /// <param name="millisecond">
        /// The milliseconds (0 through 999).
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// year is less than 1 or greater than 9999.-or- month is less than 1 or greater
        /// than 12.-or- day is less than 1 or greater than the number of days in month.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The specified parameters evaluate to less than date.MinValue or
        /// more than date.MaxValue.
        /// </exception>
        public LocalDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            m_Value = new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Local);
        }

        #endregion

        #region Properties

        /// <summary>Gets the year component of the date represented by this instance.</summary>
        public int Year { get { return m_Value.Year; } }

        /// <summary>Gets the month component of the date represented by this instance.</summary>
        public int Month { get { return m_Value.Month; } }

        /// <summary>Gets the day of the month represented by this instance.</summary>
        public int Day { get { return m_Value.Day; } }

        /// <summary>Gets the hour component of the date represented by this instance.</summary>
        public int Hour { get { return m_Value.Hour; } }

        /// <summary>Gets the minute component of the date represented by this instance.</summary>
        public int Minute { get { return m_Value.Minute; } }

        /// <summary>Gets the seconds component of the date represented by this instance.</summary>
        public int Second { get { return m_Value.Second; } }

        /// <summary>Gets the milliseconds component of the date represented by this instance.</summary>
        public int Millisecond { get { return m_Value.Millisecond; } }

        /// <summary>Gets the number of ticks that represent the date of this instance..</summary>
        public long Ticks { get { return m_Value.Ticks; } }

        /// <summary>Gets the day of the week represented by this instance.</summary>
        public DayOfWeek DayOfWeek { get { return m_Value.DayOfWeek; } }

        /// <summary>Gets the day of the year represented by this instance.</summary>
        public int DayOfYear { get { return m_Value.DayOfYear; } }

        /// <summary>Gets the date component of this instance.</summary>
        public Date Date { get { return (Date)m_Value; } }

        /// <summary>The inner value of the locat date time.</summary>
        private DateTime m_Value;

        #endregion

        #region Methods

        /// <summary>Returns a new local date time that adds the value of the specified System.TimeSpan
        /// to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A System.TimeSpan object that represents a positive or negative time interval.
        /// </param>
        /// <returns>
        /// A new date whose value is the sum of the date and time represented
        /// by this instance and the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than date.MinValue or greater
        /// than date.MaxValue.
        /// </exception>
        public LocalDateTime Add(TimeSpan value)
        {
            return new LocalDateTime(this.Ticks + value.Ticks);
        }

        /// <summary>Subtracts the specified local date time and time from this instance.</summary>
        /// <param name="value">
        /// An instance of date.
        /// </param>
        /// <returns>
        /// A System.TimeSpan interval equal to the date and time represented by this
        /// instance minus the date and time represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The result is less than date.MinValue or greater than date.MaxValue.
        /// </exception>
        public TimeSpan Subtract(LocalDateTime value)
        {
            return new TimeSpan(this.Ticks - value.Ticks);
        }

        /// <summary>Subtracts the specified duration from this instance.</summary>
        /// <param name="value">
        /// An instance of System.TimeSpan.
        /// </param>
        /// <returns>
        /// A date equal to the date and time represented by this instance
        /// minus the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The result is less than date.MinValue or greater than date.MaxValue.
        /// </exception>
        public LocalDateTime Subtract(TimeSpan value)
        {
            return new LocalDateTime(this.Ticks - value.Ticks);
        }

        /// <summary>Returns a new local date time that adds the specified number of years to
        /// the value of this instance.
        /// </summary>
        /// Parameters:
        /// <param name="value">
        /// A number of years. The value parameter can be negative or positive.
        /// </param>
        /// <returns>
        /// A date whose value is the sum of the date and time represented
        /// by this instance and the number of years represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// value or the resulting date is less than date.MinValue
        /// or greater than date.MaxValue.
        /// </exception>
        public LocalDateTime AddYears(int value)
        {
            return new LocalDateTime(m_Value.AddYears(value));
        }

        /// <summary>Returns a new local date time that adds the specified number of months to
        /// the value of this instance.
        /// </summary>
        /// <param name="months">
        /// A number of months. The months parameter can be negative or positive.
        /// </param>
        /// <returns>
        /// A date whose value is the sum of the date and time represented
        /// by this instance and months.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than date.MinValue or greater
        /// than date.MaxValue.-or- months is less than -120,000 or greater
        /// than 120,000.
        /// </exception>
        public LocalDateTime AddMonths(int months)
        {
            return new LocalDateTime(m_Value.AddMonths(months));
        }

        /// <summary>Returns a new local date time that adds the specified number of days to the
        /// value of this instance.
        /// </summary>
        /// <param name="value">
        /// A number of whole and fractional days. The value parameter can be negative
        /// or positive.
        /// </param>
        /// <returns>
        /// A date whose value is the sum of the date and time represented
        /// by this instance and the number of days represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than date.MinValue or greater
        /// than date.MaxValue.
        /// </exception>
        public LocalDateTime AddDays(double value)
        {
            return new LocalDateTime(m_Value.AddDays(value));
        }

        /// <summary>Returns a new local date time that adds the specified number of ticks to
        /// the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A number of 100-nanosecond ticks. The value parameter can be positive or
        /// negative.
        /// </param>
        /// <returns>
        /// A date whose value is the sum of the date and time represented
        /// by this instance and the time represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than date.MinValue or greater
        /// than date.MaxValue.
        /// </exception>
        public LocalDateTime AddTicks(long value)
        {
            return new LocalDateTime(Ticks + value);
        }

        /// <summary>Returns a new local date time that adds the specified number of hours to
        /// the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A number of whole and fractional hours. The value parameter can be negative
        /// or positive.
        /// </param>
        /// <returns>
        /// A date whose value is the sum of the date and time represented
        /// by this instance and the number of hours represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than date.MinValue or greater
        /// than date.MaxValue.
        /// </exception>
        public LocalDateTime AddHours(double value)
        {
            return new LocalDateTime(m_Value.AddHours(value));
        }

        /// <summary>Returns a new local date time that adds the specified number of minutes to
        /// the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A number of whole and fractional minutes. The value parameter can be negative
        /// or positive.
        /// </param>
        /// <returns>
        /// A date whose value is the sum of the date and time represented
        /// by this instance and the number of minutes represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than date.MinValue or greater
        /// than date.MaxValue.
        /// </exception>
        public LocalDateTime AddMinutes(double value)
        {
            return new LocalDateTime(m_Value.AddMinutes(value));
        }

        /// <summary>Returns a new local date time that adds the specified number of seconds to
        /// the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A number of whole and fractional seconds. The value parameter can be negative
        /// or positive.
        /// </param>
        /// <returns>
        /// A date whose value is the sum of the date and time represented
        /// by this instance and the number of seconds represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than date.MinValue or greater
        /// than date.MaxValue.
        /// </exception>
        public LocalDateTime AddSeconds(double value)
        {
            return new LocalDateTime(m_Value.AddSeconds(value));
        }

        /// <summary>Returns a new local date time that adds the specified number of milliseconds
        /// to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A number of whole and fractional milliseconds. The value parameter can be
        /// negative or positive. Note that this value is rounded to the nearest integer.
        /// </param>
        /// <returns>
        /// A date whose value is the sum of the date and time represented
        /// by this instance and the number of milliseconds represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than date.MinValue or greater
        /// than date.MaxValue.
        /// </exception>
        public LocalDateTime AddMilliseconds(double value)
        {
            return new LocalDateTime(m_Value.AddMilliseconds(value));
        }

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of local date time based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private LocalDateTime(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetDateTime("Value");
        }

        /// <summary>Adds the underlying property of local date time to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a local date time.</summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the local date time from an <see href="XmlReader"/>.</summary>
        /// <remarks>
        /// Uses the string parse function of local date time.
        /// </remarks>
        /// <param name="reader">An XML reader.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(writer, nameof(writer));
            var s = reader.ReadElementString();
            var val = Parse(s, CultureInfo.InvariantCulture);
            m_Value = val.m_Value;
        }

        /// <summary>Writes the local date time to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of local date time.
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(SerializableFormat, CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a local date time from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson()  => throw new NotSupportedException(QowaivMessages.JsonSerialization_NullNotSupported);

        /// <summary>Generates a local date time from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the local date time.
        /// </param>
        void IJsonSerializable.FromJson(string jsonString)
        {
            m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
        }

        /// <summary>Generates a local date time from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the local date time.
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger)
        {
            m_Value = new LocalDateTime(jsonInteger).m_Value;
        }

        /// <summary>Generates a local date time from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the local date time.
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber) => new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates a local date time from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the local date time.
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate)
        {
            m_Value = new LocalDateTime(jsonDate).m_Value;
        }

        /// <summary>Converts a local date time into its JSON object representation.</summary>
        object IJsonSerializable.ToJson()
        {
            return ToString(SerializableFormat, CultureInfo.InvariantCulture);
        }

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current local date time for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get { return m_Value.ToString("yyyy-MM-dd hh:mm:ss.FFF", CultureInfo.InvariantCulture); }
        }

        /// <summary>Returns a <see cref="string"/> that represents the current local date time.</summary>
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current local date time.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current local date time.</summary>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString("", formatProvider);
        }

        /// <summary>Returns a formatted <see cref="string"/> that represents the current local date time.</summary>
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
            return m_Value.ToString(format, formatProvider);
        }

        #endregion

        #region IEquatable

        /// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
        /// <param name="obj">An object to compare with.</param>
        public override bool Equals(object obj)  { return obj is LocalDateTime && Equals((LocalDateTime)obj); }

        /// <summary>Returns true if this instance and the other <see cref="LocalDateTime"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="LocalDateTime"/> to compare with.</param>
        public bool Equals(LocalDateTime other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this local date time.</summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(LocalDateTime left, LocalDateTime right)
        {
            return left.Equals(right);
        }

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(LocalDateTime left, LocalDateTime right)
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
        /// An object that evaluates to a local date time.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.Value
        /// Condition Less than zero This instance precedes value. Zero This instance
        /// has the same position in the sort order as value. Greater than zero This
        /// instance follows value.-or- value is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// value is not a local date time.
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is LocalDateTime)
            {
                return CompareTo((LocalDateTime)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a local date time"), "obj");
        }

        /// <summary>Compares this instance with a specified local date time and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified local date time.
        /// </summary>
        /// <param name="other">
        /// The local date time to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(LocalDateTime other) => m_Value.CompareTo(other.m_Value);


        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(LocalDateTime l, LocalDateTime r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(LocalDateTime l, LocalDateTime r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(LocalDateTime l, LocalDateTime r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(LocalDateTime l, LocalDateTime r) => l.CompareTo(r) >= 0;

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a local date time to a <see cref="string"/>.</summary>
        public static explicit operator string(LocalDateTime val)=> val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a local date time to a date time.</summary>
        public static implicit operator DateTime(LocalDateTime val) => val.m_Value;


        /// <summary>Casts a <see cref="string"/> to a local date time.</summary>
        public static explicit operator LocalDateTime(string str) => Parse(str, CultureInfo.CurrentCulture);
        /// <summary>Casts a date time to a local date time.</summary>
        public static implicit operator LocalDateTime(DateTime val) { return new LocalDateTime(val); }

        /// <summary>Casts a date to a local date time.</summary>
        public static explicit operator LocalDateTime(Date val) { return new LocalDateTime(val); }
        /// <summary>Casts a week date to a week date.</summary>
        public static implicit operator LocalDateTime(WeekDate val) { return (LocalDateTime)val.Date; }

        #endregion

        #region Operators

        /// <summary>Adds the time span to the local date time.</summary>
        public static LocalDateTime operator +(LocalDateTime d, TimeSpan t) { return d.Add(t); }

        /// <summary>Subtracts the Time Span from the local date time.</summary>
        public static LocalDateTime operator -(LocalDateTime d, TimeSpan t) { return d.Subtract(t); }

        /// <summary>Adds one day to the local date time.</summary>
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "AddDays is the logical named alternate.")]
        public static LocalDateTime operator ++(LocalDateTime d) { return d.AddDays(+1); }

        /// <summary>Subtracts one day from the local date time.</summary>
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "AddDays is the logical named alternate.")]
        public static LocalDateTime operator --(LocalDateTime d) { return d.AddDays(-1); }

        /// <summary>Subtracts the right local date time from the left date.</summary>
        public static TimeSpan operator -(LocalDateTime l, LocalDateTime r) { return l.Subtract(r); }

        #endregion

        #region Factory methods

        /// <summary>Converts the string to a local date time.</summary>
        /// <param name="s">
        /// A string containing a local date time to convert.
        /// </param>
        /// <returns>
        /// A local date time.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static LocalDateTime Parse(string s)
        {
            return Parse(s, CultureInfo.CurrentCulture);
        }

        /// <summary>Converts the string to a local date time.</summary>
        /// <param name="s">
        /// A string containing a local date time to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A local date time.
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static LocalDateTime Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out LocalDateTime val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionDate);
        }

        /// <summary>Converts the string to a local date time.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a local date time to convert.
        /// </param>
        /// <returns>
        /// The local date time if the string was converted successfully, otherwiseMinValue.
        /// </returns>
        public static LocalDateTime TryParse(string s)
        {
            if (TryParse(s, out LocalDateTime val))
            {
                return val;
            }
            return MinValue;
        }

        /// <summary>Converts the string to a local date time.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a local date time to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out LocalDateTime result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to a local date time.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a local date time to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out LocalDateTime result)
        {
            return TryParse(s, formatProvider, DateTimeStyles.None, out result);
        }

        /// <summary>Converts the string to a local date time.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a local date time to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <param name="styles">
        /// A bitwise combination of enumeration values that defines how to interpret
        /// the parsed date in relation to the current time zone or the current date.
        /// A typical value to specify is System.Globalization.DateStyles.None.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, IFormatProvider formatProvider, DateTimeStyles styles, out LocalDateTime result)
        {
            if (DateTime.TryParse(s, formatProvider, styles, out DateTime dt))
            {
                result = new LocalDateTime(dt);
                return true;
            }
            result = MinValue;
            return false;
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid local date time, otherwise false.</summary>
        public static bool IsValid(string val)
        {
            return IsValid(val, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns true if the val represents a valid local date time, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider)
        {
            return TryParse(val, formatProvider, out LocalDateTime d);
        }

        #endregion
    }
}
