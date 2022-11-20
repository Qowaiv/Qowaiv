using System.Numerics;

namespace Qowaiv;

/// <summary>Represents a local date time.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(DateTime))]
[OpenApiDataType(description: "Date-time notation as defined by RFC 3339, without time zone information.", example: "2017-06-10 15:00", type: "string", format: "local-date-time")]
[OpenApi.OpenApiDataType(description: "Date-time notation as defined by RFC 3339, without time zone information.", example: "2017-06-10 15:00", type: "string", format: "local-date-time")]
[TypeConverter(typeof(LocalDateTimeTypeConverter))]
#if NET5_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.LocalDateTimeJsonConverter))]
#endif
public readonly partial struct LocalDateTime : ISerializable, IXmlSerializable, IFormattable, IEquatable<LocalDateTime>, IComparable, IComparable<LocalDateTime>
#if NET7_0_OR_GREATER
    , IIncrementOperators<LocalDateTime>, IDecrementOperators<LocalDateTime>
    , IAdditionOperators<LocalDateTime, TimeSpan, LocalDateTime>, ISubtractionOperators<LocalDateTime, TimeSpan, LocalDateTime>
    , IAdditionOperators<LocalDateTime, MonthSpan, LocalDateTime>, ISubtractionOperators<LocalDateTime, MonthSpan, LocalDateTime>
    , ISubtractionOperators<LocalDateTime, LocalDateTime, TimeSpan>
#endif
{
    private const string SerializableFormat = @"yyyy-MM-dd HH:mm:ss.FFFFFFF";

    /// <summary>Represents the smallest possible value of date. This field is read-only.</summary>
    public static readonly LocalDateTime MinValue = new(DateTime.MinValue);

    /// <summary>Represents the largest possible value date. This field is read-only.</summary>
    public static readonly LocalDateTime MaxValue = new(DateTime.MaxValue);

    #region Constructors

    /// <summary>Initializes a new instance of the local date time structure to a specified number of ticks.</summary>
    /// <param name="ticks">
    /// A date expressed in 100-nanosecond units.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// ticks is less than System.DateTime.MinValue or greater than System.DateTime.MaxValue.
    /// </exception>
    public LocalDateTime(long ticks)
    {
        m_Value = new DateTime(ticks, DateTimeKind.Local);
    }

    /// <summary>Initializes a new instance of the local date time structure based on a System.DateTime.
    /// </summary>
    /// <param name="dt">
    /// A date and time.
    /// </param>
    /// <remarks>
    /// The date of the date time is taken.
    /// </remarks>
    private LocalDateTime(DateTime dt) : this(dt.Ticks) { }

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
        : this(new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Local)) { }

    #endregion

    #region Properties

    /// <summary>Gets the year component of the date represented by this instance.</summary>
    public int Year => m_Value.Year;

    /// <summary>Gets the month component of the date represented by this instance.</summary>
    public int Month => m_Value.Month;

    /// <summary>Gets the day of the month represented by this instance.</summary>
    public int Day => m_Value.Day;

    /// <summary>Gets the hour component of the date represented by this instance.</summary>
    public int Hour => m_Value.Hour;

    /// <summary>Gets the minute component of the date represented by this instance.</summary>
    public int Minute => m_Value.Minute;

    /// <summary>Gets the seconds component of the date represented by this instance.</summary>
    public int Second => m_Value.Second;

    /// <summary>Gets the milliseconds component of the date represented by this instance.</summary>
    public int Millisecond => m_Value.Millisecond;

    /// <summary>Gets the number of ticks that represent the date of this instance..</summary>
    public long Ticks => m_Value.Ticks;

    /// <summary>Gets the day of the week represented by this instance.</summary>
    public DayOfWeek DayOfWeek => m_Value.DayOfWeek;

    /// <summary>Gets the day of the year represented by this instance.</summary>
    public int DayOfYear => m_Value.DayOfYear;

    /// <summary>Gets the date component of this instance.</summary>
    public Date Date => (Date)m_Value;

    /// <summary>The inner value of the local date time.</summary>
    private readonly DateTime m_Value;

    #endregion

    #region Methods

    /// <summary>Adds one day to the local date time.</summary>
    [Pure]
    internal LocalDateTime Increment() => AddDays(+1);

    /// <summary>Subtracts one day from the local date time.</summary>
    [Pure]
    internal LocalDateTime Decrement() => AddDays(-1);

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
    [Pure]
    public LocalDateTime Add(TimeSpan value)
    {
        return new LocalDateTime(m_Value.Ticks + value.Ticks);
    }

    /// <summary>Returns a new local date time that adds the value of the specified <see cref="DateSpan"/>
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
    [Pure]
    public LocalDateTime Add(DateSpan value) => Add(value, false);

    /// <summary>Returns a new local date time that adds the value of the specified <see cref="MonthSpan"/>
    /// to the value of this instance.
    /// </summary>
    /// <param name="value">
    /// A <see cref="DateSpan"/> object that represents a positive or negative time interval.
    /// </param>
    /// <returns>
    /// A new date whose value is the sum of the date represented
    /// by this instance and the time interval represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue"/> or greater
    /// than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public LocalDateTime Add(MonthSpan value) => AddMonths(value.TotalMonths);

    /// <summary>Returns a new local date time that adds the value of the specified <see cref="DateSpan"/>
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
    [Pure]
    public LocalDateTime Add(DateSpan value, bool daysFirst)
    {
        return daysFirst
            ? AddDays(value.Days).AddMonths(value.TotalMonths)
            : AddMonths(value.TotalMonths).AddDays(value.Days);
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
    [Pure]
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
    [Pure]
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
    [Pure]
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
    [Pure]
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
    [Pure]
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
    [Pure]
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
    [Pure]
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
    [Pure]
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
    [Pure]
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
    [Pure]
    public LocalDateTime AddMilliseconds(double value)
    {
        return new LocalDateTime(m_Value.AddMilliseconds(value));
    }

    #endregion

    /// <summary>Deserializes the local date time from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to local date time.
    /// </param>
    /// <returns>
    /// The deserialized local date time.
    /// </returns>
    [Pure]
    public static LocalDateTime FromJson(long json) => new(json);

    /// <summary>Serializes the local date time to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string ToJson() => ToString(SerializableFormat, CultureInfo.InvariantCulture);

    /// <summary>Returns a <see cref="string"/> that represents the current local date time for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:yyyy-MM-dd hh:mm:ss.FFF}");

    /// <summary>Returns a formatted <see cref="string"/> that represents the current local date time.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : m_Value.ToString(format, formatProvider);

    /// <summary>Gets an XML string representation of the local date time.</summary>
    [Pure]
    private string ToXmlString() => ToString(SerializableFormat, CultureInfo.InvariantCulture);

    #region (Explicit) casting

    /// <summary>Casts a local date time to a date time.</summary>
    public static implicit operator DateTime(LocalDateTime val) => val.m_Value;

    /// <summary>Casts a date time to a local date time.</summary>
    public static implicit operator LocalDateTime(DateTime val) => new(val);

    /// <summary>Casts a date to a local date time.</summary>
    public static explicit operator LocalDateTime(Date val) => new(val);

    /// <summary>Casts a week date to a week date.</summary>
    public static implicit operator LocalDateTime(WeekDate val) => (LocalDateTime)val.Date;

    #endregion

    #region Operators

    /// <summary>Adds the time span to the local date time.</summary>
    public static LocalDateTime operator +(LocalDateTime d, TimeSpan t) => d.Add(t);

    /// <summary>Subtracts the Time Span from the local date time.</summary>
    public static LocalDateTime operator -(LocalDateTime d, TimeSpan t) => d.Subtract(t);

    /// <summary>Adds the month span to the date.</summary>
    public static LocalDateTime operator +(LocalDateTime date, MonthSpan span) => date.Add(span);

    /// <summary>Subtracts the month span from the date.</summary>
    public static LocalDateTime operator -(LocalDateTime date, MonthSpan span) => date.Add(-span);

    /// <summary>Adds one day to the local date time.</summary>
    public static LocalDateTime operator ++(LocalDateTime d) => d.Increment();

    /// <summary>Subtracts one day from the local date time.</summary>
    public static LocalDateTime operator --(LocalDateTime d) => d.Decrement();

    /// <summary>Subtracts the right local date time from the left date.</summary>
    public static TimeSpan operator -(LocalDateTime l, LocalDateTime r) => l.Subtract(r);

    #endregion

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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out LocalDateTime result)
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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, DateTimeStyles styles, out LocalDateTime result)
    {
        if (DateTime.TryParse(s, formatProvider, styles, out DateTime dt))
        {
            result = new LocalDateTime(dt);
            return true;
        }
        else
        {
            result = MinValue;
            return false;
        }
    }
}
