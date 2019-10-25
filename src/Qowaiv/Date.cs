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
    /// <summary>Represents a date, so opposed to a date time without time precision.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All ^ SingleValueStaticOptions.HasEmptyValue ^ SingleValueStaticOptions.HasUnknownValue, typeof(DateTime))]
    [OpenApiDataType(description: "Full-date notation as defined by RFC 3339, section 5.6, for example, 2017-06-10.", type: "string", format: "date")]
    [TypeConverter(typeof(DateTypeConverter))]
    public struct Date : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<Date>, IComparable, IComparable<Date>
    {
        private const string SerializableFormat = "yyyy-MM-dd";

        /// <summary>Represents the largest possible value date. This field is read-only.</summary>
        public static readonly Date MaxValue = new Date(DateTime.MaxValue);

        /// <summary>Represents the smallest possible value of date. This field is read-only.</summary>
        public static readonly Date MinValue = new Date(DateTime.MinValue);

        /// <summary>Gets the day before today.</summary>
        public static Date Yesterday => Clock.Yesterday();

        /// <summary>Gets the current date.</summary>
        public static Date Today => Clock.Today();

        /// <summary>Gets the day after today.</summary>
        public static Date Tomorrow => Clock.Tomorrow();

        #region Constructors
           
        /// <summary>Initializes a new instance of the date structure to a specified number of ticks.</summary>
        /// <param name="ticks">
        /// A date expressed in 100-nanosecond units.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// ticks is less than System.DateTime.MinValue or greater than System.DateTime.MaxValue.
        /// </exception>
        public Date(long ticks) : this(new DateTime(ticks)) { }

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
        /// The specified parameters evaluate to less than <see cref="MinValue"/> or
        /// more than <see cref="MaxValue"/>.
        /// </exception>
        public Date(int year, int month, int day) : this(new DateTime(year, month, day)) { }

        /// <summary>Initializes a new instance of the date structure based on a System.DateTime.
        /// </summary>
        /// <param name="dt">
        /// A date and time.
        /// </param>
        /// <remarks>
        /// The date of the date time is taken.
        /// </remarks>
        private Date(DateTime dt)
        {
            m_Value = dt.Date;
        }

        #endregion

        #region Properties

        /// <summary>Gets the year component of the date represented by this instance.</summary>
        public int Year => m_Value.Year;

        /// <summary>Gets the month component of the date represented by this instance.</summary>
        public int Month => m_Value.Month;

        /// <summary>Gets the day of the month represented by this instance.</summary>
        public int Day => m_Value.Day;

        /// <summary>Gets the number of ticks that represent the date of this instance..</summary>
        public long Ticks => m_Value.Ticks;

        /// <summary>Gets the day of the week represented by this instance.</summary>
        public DayOfWeek DayOfWeek => m_Value.DayOfWeek;

        /// <summary>Gets the day of the year represented by this instance.</summary>
        public int DayOfYear => m_Value.DayOfYear;

        /// <summary>The inner value of the </summary>
        private DateTime m_Value;

        #endregion

        #region Methods

        /// <summary>Returns a new date that adds the value of the specified <see cref="TimeSpan"/>
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
        /// The resulting date is less than <see cref="MinValue"/> or greater
        /// than <see cref="MaxValue"/>.
        /// </exception>
        public Date Add(TimeSpan value) => new Date(Ticks + value.Ticks);

        /// <summary>Returns a new date that adds the value of the specified <see cref="DateSpan"/>
        /// to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A <see cref="DateSpan"/> object that represents a positive or negative time interval.
        /// </param>
        /// <returns>
        /// A new date whose value is the sum of the date and time represented
        /// by this instance and the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than <see cref="MinValue"/> or greater
        /// than <see cref="MaxValue"/>.
        /// </exception>
        public Date Add(DateSpan value) => Add(value, false);

        /// <summary>Returns a new date that adds the value of the specified <see cref="DateSpan"/>
        /// to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A <see cref="DateSpan"/> object that represents a positive or negative time interval.
        /// </param>
        /// <param name="daysFirst">
        /// If true, days are added first, otherwise months are added first.
        /// </param>
        /// <returns>
        /// A new date whose value is the sum of the date and time represented
        /// by this instance and the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting date is less than <see cref="MinValue"/> or greater
        /// than <see cref="MaxValue"/>.
        /// </exception>
        public Date Add(DateSpan value, bool daysFirst)
        {
            return daysFirst
                ? AddDays(value.Days).AddMonths(value.TotalMonths)
                : AddMonths(value.TotalMonths).AddDays(value.Days)
            ;
        }

        /// <summary>Subtracts the specified date and time from this instance.</summary>
        /// <param name="value">
        /// An instance of date.
        /// </param>
        /// <returns>
        /// A System.TimeSpan interval equal to the date and time represented by this
        /// instance minus the date and time represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The result is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        /// </exception>
        public TimeSpan Subtract(Date value) => new TimeSpan(Ticks - value.Ticks);

        /// <summary>Subtracts the specified duration from this instance.</summary>
        /// <param name="value">
        /// An instance of <see cref="TimeSpan"/>.
        /// </param>
        /// <returns>
        /// A date equal to the date and time represented by this instance
        /// minus the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The result is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        /// </exception>
        public Date Subtract(TimeSpan value) => new Date(Ticks - value.Ticks);

        /// <summary>Subtracts the specified duration from this instance.</summary>
        /// <param name="value">
        /// An instance of <see cref="DateSpan"/>.
        /// </param>
        /// <returns>
        /// A date equal to the date and time represented by this instance
        /// minus the time interval represented by value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The result is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        /// </exception>
        public Date Subtract(DateSpan value) => Add(-value);

        /// <summary>Returns a new date that adds the specified number of years to
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
        /// value or the resulting date is less than <see cref="MinValue"/>
        /// or greater than <see cref="MaxValue"/>.
        /// </exception>
        public Date AddYears(int value) => new Date(m_Value.AddYears(value));

        /// <summary>Returns a new date that adds the specified number of months to
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
        /// The resulting date is less than <see cref="MinValue"/> or greater
        /// than <see cref="MaxValue"/>.-or- months is less than -120,000 or greater
        /// than 120,000.
        /// </exception>
        public Date AddMonths(int months) => new Date(m_Value.AddMonths(months));

        /// <summary>Returns a new date that adds the specified number of days to the
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
        /// The resulting date is less than <see cref="MinValue"/> or greater
        /// than <see cref="MaxValue"/>.
        /// </exception>
        public Date AddDays(double value) => new Date(m_Value.AddDays(value));

        /// <summary>Returns a new date that adds the specified number of ticks to
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
        /// The resulting date is less than <see cref="MinValue"/> or greater
        /// than <see cref="MaxValue"/>.
        /// </exception>
        public Date AddTicks(long value) => new Date(Ticks + value);

        /// <summary>Returns a new date that adds the specified number of hours to
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
        /// The resulting date is less than <see cref="MinValue"/> or greater
        /// than <see cref="MaxValue"/>.
        /// </exception>
        public Date AddHours(double value) => new Date(m_Value.AddHours(value));

        /// <summary>Returns a new date that adds the specified number of minutes to
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
        /// The resulting date is less than <see cref="MinValue"/> or greater
        /// than <see cref="MaxValue"/>.
        /// </exception>
        public Date AddMinutes(double value) => new Date(m_Value.AddMinutes(value));

        /// <summary>Returns a new date that adds the specified number of seconds to
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
        /// The resulting date is less than <see cref="MinValue"/> or greater
        /// than <see cref="MaxValue"/>.
        /// </exception>
        public Date AddSeconds(double value) => new Date(m_Value.AddSeconds(value));

        /// <summary>Returns a new date that adds the specified number of milliseconds
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
        /// The resulting date is less than <see cref="MinValue"/> or greater
        /// than <see cref="MaxValue"/>.
        /// </exception>
        public Date AddMilliseconds(double value) => new Date(m_Value.AddMilliseconds(value));

        #endregion

        #region (XML) (De)serialization

        /// <summary>Initializes a new instance of Date based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Date(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            m_Value = info.GetDateTime("Value");
        }

        /// <summary>Adds the underlying property of Date to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue("Value", m_Value);
        }

        /// <summary>Gets the <see href="XmlSchema"/> to (de) XML serialize a </summary>
        /// <remarks>
        /// Returns null as no schema is required.
        /// </remarks>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>Reads the Date from an <see href="XmlReader"/>.</summary>
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

        /// <summary>Writes the Date to an <see href="XmlWriter"/>.</summary>
        /// <remarks>
        /// Uses the string representation of 
        /// </remarks>
        /// <param name="writer">An XML writer.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));
            writer.WriteString(ToString(SerializableFormat, CultureInfo.InvariantCulture));
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Generates a Date from a JSON null object representation.</summary>
        void IJsonSerializable.FromJson() => throw new NotSupportedException(QowaivMessages.JsonSerialization_NullNotSupported);

        /// <summary>Generates a Date from a JSON string representation.</summary>
        /// <param name="jsonString">
        /// The JSON string that represents the 
        /// </param>
        void IJsonSerializable.FromJson(string jsonString) => m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;

        /// <summary>Generates a Date from a JSON integer representation.</summary>
        /// <param name="jsonInteger">
        /// The JSON integer that represents the 
        /// </param>
        void IJsonSerializable.FromJson(Int64 jsonInteger) => m_Value = new Date(jsonInteger).m_Value;

        /// <summary>Generates a Date from a JSON number representation.</summary>
        /// <param name="jsonNumber">
        /// The JSON number that represents the 
        /// </param>
        void IJsonSerializable.FromJson(Double jsonNumber) => throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported);

        /// <summary>Generates a Date from a JSON date representation.</summary>
        /// <param name="jsonDate">
        /// The JSON Date that represents the 
        /// </param>
        void IJsonSerializable.FromJson(DateTime jsonDate) => m_Value = jsonDate.Date;

        /// <summary>Converts a Date into its JSON object representation.</summary>
        object IJsonSerializable.ToJson() => ToString(SerializableFormat, CultureInfo.InvariantCulture);

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current Date for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Debugger.")]
        private string DebuggerDisplay
        {
            get => m_Value.ToString(SerializableFormat, CultureInfo.InvariantCulture);
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
        public string ToString(IFormatProvider formatProvider) => ToString("d", formatProvider);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            // We don't want to see hh:mm pop up.
            if (string.IsNullOrEmpty(format)) { format = "d"; }

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
        public override bool Equals(object obj) => obj is Date && Equals((Date)obj);

        /// <summary>Returns true if this instance and the other <see cref="Date"/> are equal, otherwise false.</summary>
        /// <param name="other">The <see cref="Date"/> to compare with.</param>
        public bool Equals(Date other) => m_Value == other.m_Value;

        /// <summary>Returns the hash code for this </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => m_Value.GetHashCode();

        /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator ==(Date left, Date right) => left.Equals(right);

        /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand</param>
        public static bool operator !=(Date left, Date right) => !(left == right);

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
            if (obj is Date)
            {
                return CompareTo((Date)obj);
            }
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.ArgumentException_Must, "a Date"), "obj");
        }

        /// <summary>Compares this instance with a specified Date and indicates
        /// whether this instance precedes, follows, or appears in the same position
        /// in the sort order as the specified 
        /// </summary>
        /// <param name="other">
        /// The Date to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows,
        /// or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(Date other) => m_Value.CompareTo(other.m_Value);

        /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
        public static bool operator <(Date l, Date r) => l.CompareTo(r) < 0;

        /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
        public static bool operator >(Date l, Date r) => l.CompareTo(r) > 0;

        /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
        public static bool operator <=(Date l, Date r) => l.CompareTo(r) <= 0;

        /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
        public static bool operator >=(Date l, Date r) => l.CompareTo(r) >= 0;

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a date to a <see cref="string"/>.</summary>
        public static explicit operator string(Date val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a date to a date time.</summary>
        public static implicit operator DateTime(Date val) => val.m_Value;

        /// <summary>Casts a <see cref="string"/> to a date.</summary>
        public static explicit operator Date(string str) => Parse(str, CultureInfo.CurrentCulture);
        /// <summary>Casts a date time to a date.</summary>
        public static explicit operator Date(DateTime val) { return new Date(val); }

        /// <summary>Casts a local date time to a date.</summary>
        public static explicit operator Date(LocalDateTime val) { return val.Date; }
        /// <summary>Casts a week date to a date.</summary>
        public static implicit operator Date(WeekDate val) { return val.Date; }

        #endregion

        #region Operators

        /// <summary>Adds the time span to the date.</summary>
        public static Date operator +(Date d, TimeSpan t) => d.Add(t);

        /// <summary>Subtracts the Time Span from the date.</summary>
        public static Date operator -(Date d, TimeSpan t) => d.Subtract(t);

        /// <summary>Adds one day to the date.</summary>
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "AddDays is the logical named alternate.")]
        public static Date operator ++(Date d) => d.AddDays(+1);

        /// <summary>Subtracts one day from the date.</summary>
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "AddDays is the logical named alternate.")]
        public static Date operator --(Date d) => d.AddDays(-1);

        /// <summary>Subtracts the right Date from the left date.</summary>
        public static TimeSpan operator -(Date l, Date r) => l.Subtract(r);

        #endregion

        #region Factory methods

        /// <summary>Converts the string to a </summary>
        /// <param name="s">
        /// A string containing a Date to convert.
        /// </param>
        /// <returns>
        /// A 
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Date Parse(string s) => Parse(s, CultureInfo.CurrentCulture);

        /// <summary>Converts the string to a </summary>
        /// <param name="s">
        /// A string containing a Date to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The specified format provider.
        /// </param>
        /// <returns>
        /// A 
        /// </returns>
        /// <exception cref="FormatException">
        /// s is not in the correct format.
        /// </exception>
        public static Date Parse(string s, IFormatProvider formatProvider)
        {
            if (TryParse(s, formatProvider, out Date val))
            {
                return val;
            }
            throw new FormatException(QowaivMessages.FormatExceptionDate);
        }

        /// <summary>Converts the string to a 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Date to convert.
        /// </param>
        /// <returns>
        /// The Date if the string was converted successfully, otherwise MinValue.
        /// </returns>
        public static Date TryParse(string s)
        {
            if (TryParse(s, out Date val))
            {
                return val;
            }
            return MinValue;
        }

        /// <summary>Converts the string to a 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Date to convert.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, out Date result)
        {
            return TryParse(s, CultureInfo.CurrentCulture, out result);
        }

        /// <summary>Converts the string to a 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Date to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out Date result)
        {
            return TryParse(s, formatProvider, DateTimeStyles.None, out result);
        }

        /// <summary>Converts the string to a 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Date to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, DateTimeStyles styles, out Date result)
        {
            if (DateTime.TryParse(s, formatProvider, styles, out DateTime dt))
            {
                result = new Date(dt);
                return true;
            }
            result = MinValue;
            return false;
        }

        #endregion

        #region Validation

        /// <summary>Returns true if the val represents a valid Date, otherwise false.</summary>
        public static bool IsValid(string val) => IsValid(val, CultureInfo.CurrentCulture);

        /// <summary>Returns true if the val represents a valid Date, otherwise false.</summary>
        public static bool IsValid(string val, IFormatProvider formatProvider) => TryParse(val, formatProvider, out _);

        #endregion
    }
}
