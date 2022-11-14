namespace Qowaiv;

/// <summary>Represents a date, so opposed to a date time without time precision.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable, SingleValueObject(SingleValueStaticOptions.All ^ SingleValueStaticOptions.HasEmptyValue ^ SingleValueStaticOptions.HasUnknownValue, typeof(DateTime))]
[OpenApiDataType(description: "Full-date notation as defined by RFC 3339, section 5.6.", example: "2017-06-10", type: "string", format: "date")]
[OpenApi.OpenApiDataType(description: "Full-date notation as defined by RFC 3339, section 5.6.", example: "2017-06-10", type: "string", format: "date")]
[TypeConverter(typeof(DateTypeConverter))]
#if NET5_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.DateJsonConverter))]
#endif
public readonly partial struct Date : ISerializable, IXmlSerializable, IFormattable, IEquatable<Date>, IComparable, IComparable<Date>
#if NET7_0_OR_GREATER
    , IIncrementOperators<Date>, IDecrementOperators<Date>
    , IAdditionOperators<Date, TimeSpan, Date>, ISubtractionOperators<Date, TimeSpan, Date>
    , IAdditionOperators<Date, MonthSpan, Date>, ISubtractionOperators<Date, MonthSpan, Date>
    , ISubtractionOperators<Date, Date, TimeSpan>
#endif
{
    private const string SerializableFormat = "yyyy-MM-dd";

    /// <summary>Represents the largest possible value date. This field is read-only.</summary>
    public static readonly Date MaxValue = new(DateTime.MaxValue);

    /// <summary>Represents the smallest possible value of date. This field is read-only.</summary>
    public static readonly Date MinValue = new(DateTime.MinValue);

    /// <summary>Gets the day before today.</summary>
    [Obsolete("Use Clock.Yesterday() instead. Will be dropped when the next major version is released.")]
    public static Date Yesterday => Clock.Yesterday();

    /// <summary>Gets the current date.</summary>
    [Obsolete("Use Clock.Today() instead. Will be dropped when the next major version is released.")]
    public static Date Today => Clock.Today();

    /// <summary>Gets the day after today.</summary>
    [Obsolete("Use Clock.Tomorrow() instead. Will be dropped when the next major version is released.")]
    public static Date Tomorrow => Clock.Tomorrow();

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
    private Date(DateTime dt) => m_Value = dt.Date;

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

    /// <summary>The inner value of the date.</summary>
    private readonly DateTime m_Value;

    #region Methods

    /// <summary>Adds one day to the date.</summary>
    [Pure]
    internal Date Increment() => AddDays(+1);

    /// <summary>Subtracts one day from the date.</summary>
    [Pure]
    internal Date Decrement() => AddDays(-1);

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
    [Pure]
    public Date Add(TimeSpan value) => new(Ticks + value.Ticks);

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
    [Pure]
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
    /// A new date whose value is the sum of the date represented
    /// by this instance and the time interval represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue"/> or greater
    /// than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public Date Add(DateSpan value, bool daysFirst)
    {
        return daysFirst
            ? AddDays(value.Days).AddMonths(value.TotalMonths)
            : AddMonths(value.TotalMonths).AddDays(value.Days)
        ;
    }

    /// <summary>Returns a new date that adds the value of the specified <see cref="MonthSpan"/>
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
    public Date Add(MonthSpan value) => AddMonths(value.TotalMonths);

    /// <summary>Subtracts the specified date from this instance.</summary>
    /// <param name="value">
    /// An instance of date.
    /// </param>
    /// <returns>
    /// A System.TimeSpan interval equal to the date represented by this
    /// instance minus the date represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The result is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public TimeSpan Subtract(Date value) => new(Ticks - value.Ticks);

    /// <summary>Subtracts the specified duration from this instance.</summary>
    /// <param name="value">
    /// An instance of <see cref="TimeSpan"/>.
    /// </param>
    /// <returns>
    /// A date equal to the date represented by this instance
    /// minus the time interval represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The result is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public Date Subtract(TimeSpan value) => new(Ticks - value.Ticks);

    /// <summary>Subtracts the specified duration from this instance.</summary>
    /// <param name="value">
    /// An instance of <see cref="DateSpan"/>.
    /// </param>
    /// <returns>
    /// A date equal to the date represented by this instance
    /// minus the time interval represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The result is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public Date Subtract(DateSpan value) => Add(-value);

    /// <summary>Returns a new date that adds the specified number of years to
    /// the value of this instance.
    /// </summary>
    /// Parameters:
    /// <param name="value">
    /// A number of years. The value parameter can be negative or positive.
    /// </param>
    /// <returns>
    /// A date whose value is the sum of the date represented
    /// by this instance and the number of years represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// value or the resulting date is less than <see cref="MinValue"/>
    /// or greater than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public Date AddYears(int value) => new(m_Value.AddYears(value));

    /// <summary>Returns a new date that adds the specified number of months to
    /// the value of this instance.
    /// </summary>
    /// <param name="months">
    /// A number of months. The months parameter can be negative or positive.
    /// </param>
    /// <returns>
    /// A date whose value is the sum of the date represented
    /// by this instance and months.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue"/> or greater
    /// than <see cref="MaxValue"/>.-or- months is less than -120,000 or greater
    /// than 120,000.
    /// </exception>
    [Pure]
    public Date AddMonths(int months) => new(m_Value.AddMonths(months));

    /// <summary>Returns a new date that adds the specified number of days to the
    /// value of this instance.
    /// </summary>
    /// <param name="value">
    /// A number of whole and fractional days. The value parameter can be negative
    /// or positive.
    /// </param>
    /// <returns>
    /// A date whose value is the sum of the date represented
    /// by this instance and the number of days represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue"/> or greater
    /// than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public Date AddDays(double value) => new(m_Value.AddDays(value));

    /// <summary>Returns a new date that adds the specified number of ticks to
    /// the value of this instance.
    /// </summary>
    /// <param name="value">
    /// A number of 100-nanosecond ticks. The value parameter can be positive or
    /// negative.
    /// </param>
    /// <returns>
    /// A date whose value is the sum of the date represented
    /// by this instance and the time represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue"/> or greater
    /// than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public Date AddTicks(long value) => new(Ticks + value);

    /// <summary>Returns a new date that adds the specified number of hours to
    /// the value of this instance.
    /// </summary>
    /// <param name="value">
    /// A number of whole and fractional hours. The value parameter can be negative
    /// or positive.
    /// </param>
    /// <returns>
    /// A date whose value is the sum of the date represented
    /// by this instance and the number of hours represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue"/> or greater
    /// than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public Date AddHours(double value) => new(m_Value.AddHours(value));

    /// <summary>Returns a new date that adds the specified number of minutes to
    /// the value of this instance.
    /// </summary>
    /// <param name="value">
    /// A number of whole and fractional minutes. The value parameter can be negative
    /// or positive.
    /// </param>
    /// <returns>
    /// A date whose value is the sum of the date represented
    /// by this instance and the number of minutes represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue"/> or greater
    /// than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public Date AddMinutes(double value) => new(m_Value.AddMinutes(value));

    /// <summary>Returns a new date that adds the specified number of seconds to
    /// the value of this instance.
    /// </summary>
    /// <param name="value">
    /// A number of whole and fractional seconds. The value parameter can be negative
    /// or positive.
    /// </param>
    /// <returns>
    /// A date whose value is the sum of the date represented
    /// by this instance and the number of seconds represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue"/> or greater
    /// than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public Date AddSeconds(double value) => new(m_Value.AddSeconds(value));

    /// <summary>Returns a new date that adds the specified number of milliseconds
    /// to the value of this instance.
    /// </summary>
    /// <param name="value">
    /// A number of whole and fractional milliseconds. The value parameter can be
    /// negative or positive. Note that this value is rounded to the nearest integer.
    /// </param>
    /// <returns>
    /// A date whose value is the sum of the date represented
    /// by this instance and the number of milliseconds represented by value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue"/> or greater
    /// than <see cref="MaxValue"/>.
    /// </exception>
    [Pure]
    public Date AddMilliseconds(double value) => new(m_Value.AddMilliseconds(value));

    #endregion

    /// <summary>Deserializes the date from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized date.
    /// </returns>
    [Pure]
    public static Date FromJson(long json) => new(json);

    /// <summary>Serializes the date to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string ToJson() => ToString(SerializableFormat, CultureInfo.InvariantCulture);

    #region IFormattable / ToString

    /// <summary>Returns a <see cref="string"/> that represents the current Date for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay($"{{0:{SerializableFormat}}}");

    /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        // We don't want to see hh:mm pop up.
        format = format.WithDefault("d");
        return StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
            ? formatted 
            : m_Value.ToString(format, formatProvider);
    }

    /// <summary>Gets an XML string representation of the @FullName.</summary>
    [Pure]
    private string ToXmlString() => ToString(SerializableFormat, CultureInfo.InvariantCulture);

    #endregion

    #region (Explicit) casting

    /// <summary>Casts a date to a date time.</summary>
    public static implicit operator DateTime(Date val) => val.m_Value;

    /// <summary>Casts a date time to a date.</summary>
    public static explicit operator Date(DateTime val) => new(val);

    /// <summary>Casts a local date time to a date.</summary>
    public static explicit operator Date(LocalDateTime val) => val.Date;

    /// <summary>Casts a week date to a date.</summary>
    public static implicit operator Date(WeekDate val) => val.Date;

    #endregion

    #region Operators

    /// <summary>Adds the time span to the date.</summary>
    public static Date operator +(Date d, TimeSpan t) => d.Add(t);

    /// <summary>Subtracts the Time Span from the date.</summary>
    public static Date operator -(Date d, TimeSpan t) => d.Subtract(t);

    /// <summary>Adds the month span to the date.</summary>
    public static Date operator +(Date date, MonthSpan span) => date.Add(span);

    /// <summary>Subtracts the month span from the date.</summary>
    public static Date operator -(Date date, MonthSpan span) => date.Add(-span);

    /// <summary>Adds one day to the date.</summary>
    public static Date operator ++(Date d) => d.Increment();

    /// <summary>Subtracts one day from the date.</summary>
    public static Date operator --(Date d) => d.Decrement();

    /// <summary>Subtracts the right Date from the left date.</summary>
    public static TimeSpan operator -(Date l, Date r) => l.Subtract(r);

    #endregion

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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out Date result)
        => TryParse(s, formatProvider, DateTimeStyles.None, out result);

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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, DateTimeStyles styles, out Date result)
    {
        if (DateTime.TryParse(s, formatProvider, styles, out DateTime dt))
        {
            result = new Date(dt);
            return true;
        }
        else
        {
            result = MinValue;
            return false;
        }
    }
}
