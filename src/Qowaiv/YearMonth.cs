namespace Qowaiv;

/// <summary>Represents a year-month.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All ^ SingleValueStaticOptions.HasEmptyValue ^ SingleValueStaticOptions.HasUnknownValue, typeof(int))]
[OpenApiDataType(description: "Date notation with month precision.", example: "2017-06", type: "string", format: "year-month", pattern: "[0-9]{4}-(0?[1-9]|1[0-2])")]
[TypeConverter(typeof(YearMonthTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.YearMonthJsonConverter))]
#endif
public readonly partial struct YearMonth : IXmlSerializable, IFormattable, IEquatable<YearMonth>, IComparable, IComparable<YearMonth>
{
    /// <summary>Represents the smallest possible year-month (0001-01).</summary>
    public static readonly YearMonth MinValue = new(0001, 01);

    /// <summary>Represents the largest possible year-month (9999-12).</summary>
    public static readonly YearMonth MaxValue = new(9999, 12);

    /// <summary>12 months per year.</summary>
    private const int MonthsPerYear = 12;

    /// <summary>Initializes a new instance of the <see cref="YearMonth" /> struct to the specified year, and month.</summary>
    /// <param name="year">
    /// The year of the year-month.
    /// </param>
    /// <param name="month">
    /// The month of the year-month.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// When year is not in range [1,9999] or month is not in range [1,12].
    /// </exception>
    public YearMonth(int year, int month) : this(Create(year, month)) { }

    /// <summary>Gets the year component of the date represented by this instance.</summary>
    public int Year => 1 + (m_Value / MonthsPerYear);

    /// <summary>Gets the month component of the date represented by this instance.</summary>
    public int Month => 1 + (m_Value % MonthsPerYear);

    /// <summary>Returns a <see cref="string" /> that represents the year-month for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:yyyy-MM}");

    /// <summary>Deconstructs the date month in a year and month.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out int year, out int month)
    {
        year = Year;
        month = Month;
    }

    /// <summary>Returns a new year-month that adds the value of the specified <see cref="MonthSpan" />
    /// to the value of this instance.
    /// </summary>
    /// <param name="months">
    /// A <see cref="MonthSpan" /> that represents a positive or negative time interval.
    /// </param>
    /// <returns>
    /// A new year-month whose value is the sum of the year-month represented
    /// by this instance and the time interval represented by <paramref name="months" />.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue" /> or greater
    /// than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public YearMonth Add(MonthSpan months)
    {
        var value = (long)months.TotalMonths + m_Value;

        return value.IsInRange(MinValue.m_Value, MaxValue.m_Value)
            ? new((int)value)
            : throw new ArgumentOutOfRangeException(nameof(months), QowaivMessages.ArgumentOutOfRange_YearMonth);
    }

    /// <summary>Returns a new year-month that adds the specified number of months to the
    /// value of this instance.
    /// </summary>
    /// <param name="months">
    /// A number of months. The <paramref name="months" /> parameter can be negative
    /// or positive.
    /// </param>
    /// <returns>
    /// A date whose value is the sum of the year-month represented
    /// by this instance and the number of days represented by <paramref name="months" />.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue" /> or greater
    /// than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public YearMonth AddMonths(int months)
    {
        var value = (long)months + m_Value;

        return value.IsInRange(MinValue.m_Value, MaxValue.m_Value)
            ? new((int)value)
            : throw new ArgumentOutOfRangeException(nameof(months), QowaivMessages.ArgumentOutOfRange_YearMonth);
    }

    /// <summary>Returns a new year-month that adds the specified number of years to the
    /// value of this instance.
    /// </summary>
    /// <param name="years">
    /// A number of years. The <paramref name="years" /> parameter can be negative
    /// or positive.
    /// </param>
    /// <returns>
    /// A date whose value is the sum of the year-month represented
    /// by this instance and the number of days represented by <paramref name="years" />.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The resulting date is less than <see cref="MinValue" /> or greater
    /// than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public YearMonth AddYears(int years)
    {
        var value = (((long)years) * 12) + m_Value;

        return value.IsInRange(MinValue.m_Value, MaxValue.m_Value)
            ? new((int)value)
            : throw new ArgumentOutOfRangeException(nameof(years), QowaivMessages.ArgumentOutOfRange_YearMonth);
    }

    /// <summary>
    /// Returns a date that is set to a date with the year and month of this
    /// year-month and the specified day.
    /// </summary>
    /// <param name="day">
    /// The day of the date.
    /// </param>
    [Pure]
    public Date ToDate(int day) => new(Year, Month, day);

    /// <summary>Returns true if the year-month is in the specified month, otherwise false.</summary>
    /// <param name="month">
    /// The <see cref="Qowaiv.Month" /> the date should be in.
    /// </param>
    [Pure]
    public bool IsIn(Month month) => month is { HasValue: true } && Month == (int)month;

    /// <summary>Returns true if the  year-month  is in the specified year, otherwise false.</summary>
    /// <param name="year">
    /// The <see cref="Qowaiv.Year" /> the date should be in.
    /// </param>
    [Pure]
    public bool IsIn(Year year) => year is { HasValue: true } && Year == (int)year;

    /// <summary>Returns a formatted <see cref="string" /> that represents the year-month.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        // We don't want to see hh:mm pop up.
        format = format.WithDefault("yyyy-MM");
        return StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
            ? formatted
            : new Date(Year, Month, 01).ToString(format, formatProvider);
    }

    /// <summary>Gets an XML string representation of the year-month.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the year-month to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string ToJson() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Adds a number of months to the year-month.</summary>
    public static YearMonth operator +(YearMonth date, MonthSpan months) => date.Add(months);

    /// <summary>Subtracts a number of months to the year-month.</summary>
    public static YearMonth operator -(YearMonth date, MonthSpan months) => date.Add(-months);

    /// <summary>Determines the duretion between the left and the right year-month.</summary>
    public static MonthSpan operator -(YearMonth l, YearMonth r) => MonthSpan.FromMonths(l.m_Value - r.m_Value);

    /// <summary>Casts a year-month to a date.</summary>
    public static explicit operator Date(YearMonth date) => date.ToDate(01);

    /// <summary>Casts a year-month to a date time.</summary>
    public static explicit operator DateTime(YearMonth date) => date.ToDate(01);

    /// <summary>Casts a year-month to a local date time.</summary>
    public static explicit operator LocalDateTime(YearMonth date) => (LocalDateTime)date.ToDate(01);

    /// <summary>Casts a week date to a year-month.</summary>
    public static explicit operator YearMonth(Date date) => new(date.Year, date.Month);

    /// <summary>Casts a date time to a year-month.</summary>
    public static explicit operator YearMonth(DateTime date) => new(date.Year, date.Month);

    /// <summary>Casts a local date time to a year-month.</summary>
    public static explicit operator YearMonth(LocalDateTime date) => new(date.Year, date.Month);

    /// <summary>Adds one month to the year-month.</summary>
    public static YearMonth operator ++(YearMonth date) => date.AddMonths(+1);

    /// <summary>Subtracts one month to the year-month.</summary>
    public static YearMonth operator --(YearMonth date) => date.AddMonths(-1);

    /// <summary>Converts the string to a year-month.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing a Date to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out YearMonth result)
        => TryParse(s, provider, DateTimeStyles.None, out result);

    /// <summary>Converts the string to a year-month.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing a Date to convert.
    /// </param>
    /// <param name="provider">
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
    public static bool TryParse(string? s, IFormatProvider? provider, DateTimeStyles styles, out YearMonth result)
    {
        if (DateTime.TryParse(s, provider, styles, out var dt))
        {
            result = new(dt.Year, dt.Month);
            return true;
        }
        else
        {
            result = MinValue;
            return false;
        }
    }

    [Pure]
    private static int Create(int year, int month)
    {
        if (year < 1 || year > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year), QowaivMessages.ArgumentOutOfRange_YearMonth);
        }
        if (month < 1 || month > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), QowaivMessages.ArgumentOutOfRange_YearMonth);
        }
        return ((year - 1) * MonthsPerYear) + month - 1;
    }
}
