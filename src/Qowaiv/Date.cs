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
	/// <summary>Represents a Date.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	[Serializable, SingleValueObject(SingleValueStaticOptions.All ^ SingleValueStaticOptions.HasEmptyValue ^ SingleValueStaticOptions.HasUnknownValue, typeof(DateTime))]
	[TypeConverter(typeof(DateTypeConverter))]
	public struct Date : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IComparable, IComparable<Date>
	{
		private const string SerializableFormat = "yyyy-MM-dd";

		/// <summary>Represents the largest possible value date. This field is read-only.</summary>
		public static readonly Date MaxValue = new Date(DateTime.MaxValue);

		/// <summary>Represents the smallest possible value of date. This field is read-only.</summary>
		public static readonly Date MinValue = new Date(DateTime.MinValue);

		/// <summary>Gets the current date.</summary>
		public static Date Today { get { return (Date)DateTime.Today; } }

		#region Constructors

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

		/// <summary>Initializes a new instance of the date structure to a specified number of ticks.</summary>
		/// <param name="ticks">
		///  A date expressed in 100-nanosecond units.
		/// </param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///  ticks is less than System.DateTime.MinValue or greater than System.DateTime.MaxValue.
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
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// year is less than 1 or greater than 9999.-or- month is less than 1 or greater
		/// than 12.-or- day is less than 1 or greater than the number of days in month.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// The specified parameters evaluate to less than date.MinValue or
		/// more than date.MaxValue.
		/// </exception>
		public Date(int year, int month, int day) : this(new DateTime(year, month, day)) { }

		#endregion

		#region Properties

		/// <summary>Gets the year component of the date represented by this instance.</summary>
		public int Year { get { return m_Value.Year; } }

		/// <summary>Gets the month component of the date represented by this instance.</summary>
		public int Month { get { return m_Value.Month; } }

		/// <summary>Gets the day of the month represented by this instance.</summary>
		public int Day { get { return m_Value.Day; } }

		/// <summary>Gets the number of ticks that represent the date of this instance..</summary>
		public long Ticks { get { return m_Value.Ticks; } }

		/// <summary>Gets the day of the week represented by this instance.</summary>
		public DayOfWeek DayOfWeek { get { return m_Value.DayOfWeek; } }

		/// <summary>Gets the day of the year represented by this instance.</summary>
		public int DayOfYear { get { return m_Value.DayOfYear; } }

		/// <summary>The inner value of the Date.</summary>
		private DateTime m_Value;

		#endregion

		#region Methods

		/// <summary>Returns a new date that adds the value of the specified System.TimeSpan
		/// to the value of this instance.
		/// </summary>
		/// <param name="value">
		/// A System.TimeSpan object that represents a positive or negative time interval.
		/// </param>
		/// <returns>
		///  A new date whose value is the sum of the date and time represented
		///  by this instance and the time interval represented by value.
		///  </returns>
		///  <exception cref="System.ArgumentOutOfRangeException">
		///  The resulting date is less than date.MinValue or greater
		///  than date.MaxValue.
		///  </exception>
		public Date Add(TimeSpan value)
		{
			return new Date(this.Ticks + value.Ticks);
		}

		/// <summary>Subtracts the specified date and time from this instance.</summary>
		/// <param name="value">
		/// An instance of date.
		/// </param>
		/// <returns>
		/// A System.TimeSpan interval equal to the date and time represented by this
		/// instance minus the date and time represented by value.
		/// </returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The result is less than date.MinValue or greater than date.MaxValue.
		/// </exception>
		public TimeSpan Subtract(Date value)
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
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The result is less than date.MinValue or greater than date.MaxValue.
		/// </exception>
		public Date Subtract(TimeSpan value)
		{
			return new Date(this.Ticks - value.Ticks);
		}

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
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// value or the resulting date is less than date.MinValue
		/// or greater than date.MaxValue.
		/// </exception>
		public Date AddYears(int value)
		{
			return new Date(m_Value.AddYears(value));
		}

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
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The resulting date is less than date.MinValue or greater
		/// than date.MaxValue.-or- months is less than -120,000 or greater
		/// than 120,000.
		/// </exception>
		public Date AddMonths(int months)
		{
			return new Date(m_Value.AddMonths(months));
		}

		/// <summary>Returns a new date that adds the specified number of days to the
		/// value of this instance.
		/// </summary>
		/// <param name="value">
		///  A number of whole and fractional days. The value parameter can be negative
		///  or positive.
		///  </param>
		/// <returns>
		/// A date whose value is the sum of the date and time represented
		/// by this instance and the number of days represented by value.
		/// </returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The resulting date is less than date.MinValue or greater
		/// than date.MaxValue.
		/// </exception>
		public Date AddDays(double value)
		{
			return new Date(m_Value.AddDays(value));
		}

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
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The resulting date is less than date.MinValue or greater
		/// than date.MaxValue.
		/// </exception>
		public Date AddTicks(long value)
		{
			return new Date(Ticks + value);
		}

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
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The resulting date is less than date.MinValue or greater
		/// than date.MaxValue.
		/// </exception>
		public Date AddHours(double value)
		{
			return new Date(m_Value.AddHours(value));
		}

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
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The resulting date is less than date.MinValue or greater
		/// than date.MaxValue.
		/// </exception>
		public Date AddMinutes(double value)
		{
			return new Date(m_Value.AddMinutes(value));
		}

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
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The resulting date is less than date.MinValue or greater
		/// than date.MaxValue.
		/// </exception>
		public Date AddSeconds(double value)
		{
			return new Date(m_Value.AddSeconds(value));
		}

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
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The resulting date is less than date.MinValue or greater
		/// than date.MaxValue.
		/// </exception>
		public Date AddMilliseconds(double value)
		{
			return new Date(m_Value.AddMilliseconds(value));
		}

		#endregion

		#region (XML) (De)serialization

		/// <summary>Initializes a new instance of Date based on the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		private Date(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			m_Value = info.GetDateTime("Value");
		}

		/// <summary>Adds the underlying property of Date to the serialization info.</summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Guard.NotNull(info, "info");
			info.AddValue("Value", m_Value);
		}

		/// <summary>Gets the xml schema to (de) xml serialize a Date.</summary>
		/// <remarks>
		/// Returns null as no schema is required.
		/// </remarks>
		XmlSchema IXmlSerializable.GetSchema() { return null; }

		/// <summary>Reads the Date from an xml writer.</summary>
		/// <remarks>
		/// Uses the string parse function of Date.
		/// </remarks>
		/// <param name="reader">An xml reader.</param>
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			var s = reader.ReadElementString();
			var val = Parse(s, CultureInfo.InvariantCulture);
			m_Value = val.m_Value;
		}

		/// <summary>Writes the Date to an xml writer.</summary>
		/// <remarks>
		/// Uses the string representation of Date.
		/// </remarks>
		/// <param name="writer">An xml writer.</param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteString(ToString(SerializableFormat, CultureInfo.InvariantCulture));
		}

		#endregion

		#region (JSON) (De)serialization

		/// <summary>Generates a Date from a JSON null object representation.</summary>
		void IJsonSerializable.FromJson() { throw new NotSupportedException(QowaivMessages.JsonSerialization_NullNotSupported); }

		/// <summary>Generates a Date from a JSON string representation.</summary>
		/// <param name="jsonString">
		/// The JSON string that represents the Date.
		/// </param>
		void IJsonSerializable.FromJson(String jsonString)
		{
			m_Value = Parse(jsonString, CultureInfo.InvariantCulture).m_Value;
		}

		/// <summary>Generates a Date from a JSON integer representation.</summary>
		/// <param name="jsonInteger">
		/// The JSON integer that represents the Date.
		/// </param>
		void IJsonSerializable.FromJson(Int64 jsonInteger)
		{
			m_Value = new Date(jsonInteger).m_Value;
		}

		/// <summary>Generates a Date from a JSON number representation.</summary>
		/// <param name="jsonNumber">
		/// The JSON number that represents the Date.
		/// </param>
		void IJsonSerializable.FromJson(Double jsonNumber) { throw new NotSupportedException(QowaivMessages.JsonSerialization_DoubleNotSupported); }

		/// <summary>Generates a Date from a JSON date representation.</summary>
		/// <param name="jsonDate">
		/// The JSON Date that represents the Date.
		/// </param>
		void IJsonSerializable.FromJson(DateTime jsonDate)
		{
			m_Value = jsonDate.Date;
		}

		/// <summary>Converts a Date into its JSON object representation.</summary>
		object IJsonSerializable.ToJson()
		{
			return ToString(SerializableFormat, CultureInfo.InvariantCulture);
		}

		#endregion

		#region IFormattable / ToString

		/// <summary>Returns a System.String that represents the current Date for debug purposes.</summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
		{
			get { return m_Value.ToString(SerializableFormat, CultureInfo.InvariantCulture); }
		}

		/// <summary>Returns a System.String that represents the current Date.</summary>
		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Date.</summary>
		/// <param name="format">
		/// The format that this describes the formatting.
		/// </param>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a formatted System.String that represents the current Date.</summary>
		/// <param name="formatProvider">
		/// The format provider.
		/// </param>
		public string ToString(IFormatProvider formatProvider)
		{
			return ToString("d", formatProvider);
		}

		/// <summary>Returns a formatted System.String that represents the current Date.</summary>
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

			string formatted;
			if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out formatted))
			{
				return formatted;
			}
			return m_Value.ToString(format, formatProvider);
		}

		#endregion

		#region IEquatable

		/// <summary>Returns true if this instance and the other object are equal, otherwise false.</summary>
		/// <param name="obj">An object to compare with.</param>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Returns the hash code for this Date.</summary>
		/// <returns>
		/// A 32-bit signed integer hash code.
		/// </returns>
		public override int GetHashCode() { return m_Value.GetHashCode(); }

		/// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator ==(Date left, Date right)
		{
			return left.Equals(right);
		}

		/// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
		/// <param name="left">The left operand.</param>
		/// <param name="right">The right operand</param>
		public static bool operator !=(Date left, Date right)
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
		/// An object that evaluates to a Date.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.Value
		/// Condition Less than zero This instance precedes value. Zero This instance
		/// has the same position in the sort order as value. Greater than zero This
		/// instance follows value.-or- value is null.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// value is not a Date.
		/// </exception>
		public int CompareTo(object obj)
		{
			if (obj is Date)
			{
				return CompareTo((Date)obj);
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.AgrumentException_Must, "a Date"), "obj");
		}

		/// <summary>Compares this instance with a specified Date and indicates
		/// whether this instance precedes, follows, or appears in the same position
		/// in the sort order as the specified Date.
		/// </summary>
		/// <param name="other">
		/// The Date to compare with this instance.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer that indicates whether this instance precedes, follows,
		/// or appears in the same position in the sort order as the value parameter.
		/// </returns>
		public int CompareTo(Date other) { return m_Value.CompareTo(other.m_Value); }

		/// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
		public static bool operator <(Date l, Date r) { return l.CompareTo(r) < 0; }

		/// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
		public static bool operator >(Date l, Date r) { return l.CompareTo(r) > 0; }

		/// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
		public static bool operator <=(Date l, Date r) { return l.CompareTo(r) <= 0; }

		/// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
		public static bool operator >=(Date l, Date r) { return l.CompareTo(r) >= 0; }

		#endregion

		#region (Explicit) casting

		/// <summary>Casts a date to a System.String.</summary>
		public static explicit operator string(Date val) { return val.ToString(CultureInfo.CurrentCulture); }
		/// <summary>Casts a date to a date time.</summary>
		public static implicit operator DateTime(Date val) { return val.m_Value; }

		/// <summary>Casts a System.String to a date.</summary>
		public static explicit operator Date(string str) { return Date.Parse(str, CultureInfo.CurrentCulture); }
		/// <summary>Casts a date time to a date.</summary>
		public static explicit operator Date(DateTime val) { return new Date(val); }
		
		/// <summary>Casts a local date time to a date.</summary>
		public static explicit operator Date(LocalDateTime val) { return val.Date; }
		/// <summary>Casts a week date to a date.</summary>
		public static implicit operator Date(WeekDate val) { return val.Date; }

		#endregion

		#region Operators

		/// <summary>Adds the time span to the date.</summary>
		public static Date operator +(Date d, TimeSpan t) { return d.Add(t); }

		/// <summary>Subtracts the Time Span from the date.</summary>
		public static Date operator -(Date d, TimeSpan t) { return d.Subtract(t); }

		/// <summary>Adds one day to the date.</summary>
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "AddDays is the logical named alternate.")]
		public static Date operator ++(Date d) { return d.AddDays(+1); }

		/// <summary>Subtracts one day from the date.</summary>
		[SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "AddDays is the logical named alternate.")]
		public static Date operator --(Date d) { return d.AddDays(-1); }

		/// <summary>Subtracts the right Date from the left date.</summary>
		public static TimeSpan operator -(Date l, Date r) { return l.Subtract(r); }

		#endregion

		#region Factory methods

		/// <summary>Converts the string to a Date.</summary>
		/// <param name="s">
		/// A string containing a Date to convert.
		/// </param>
		/// <returns>
		/// A Date.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Date Parse(string s)
		{
			return Parse(s, CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the string to a Date.</summary>
		/// <param name="s">
		/// A string containing a Date to convert.
		/// </param>
		/// <param name="formatProvider">
		/// The specified format provider.
		/// </param>
		/// <returns>
		/// A Date.
		/// </returns>
		/// <exception cref="System.FormatException">
		/// s is not in the correct format.
		/// </exception>
		public static Date Parse(string s, IFormatProvider formatProvider)
		{
			Date val;
			if (Date.TryParse(s, formatProvider, out val))
			{
				return val;
			}
			throw new FormatException(QowaivMessages.FormatExceptionDate);
		}

		/// <summary>Converts the string to a Date.
		/// A return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">
		/// A string containing a Date to convert.
		/// </param>
		/// <returns>
		/// The Date if the string was converted successfully, otherwise Date.MinValue.
		/// </returns>
		public static Date TryParse(string s)
		{
			Date val;
			if (Date.TryParse(s, out val))
			{
				return val;
			}
			return Date.MinValue;
		}

		/// <summary>Converts the string to a Date.
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

		/// <summary>Converts the string to a Date.
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

		/// <summary>Converts the string to a Date.
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
			DateTime dt;

			if (DateTime.TryParse(s, formatProvider, styles, out dt))
			{
				result = new Date(dt);
				return true;
			}
			result = Date.MinValue;
			return false;
		}

		#endregion

		#region Validation

		/// <summary>Returns true if the val represents a valid Date, otherwise false.</summary>
		public static bool IsValid(string val)
		{
			return IsValid(val, CultureInfo.CurrentCulture);
		}

		/// <summary>Returns true if the val represents a valid Date, otherwise false.</summary>
		public static bool IsValid(string val, IFormatProvider formatProvider)
		{
			Date d;
			return TryParse(val, formatProvider, out d);
		}

		#endregion
	}
}