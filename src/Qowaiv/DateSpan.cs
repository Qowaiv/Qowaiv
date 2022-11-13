namespace Qowaiv;

/// <summary>Represents a date span.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(ulong))]
[OpenApiDataType(description: "Date span, specified in years, months and days.", example: "1Y+10M+16D", type: "string", format: "date-span", pattern: @"[+-]?[0-9]+Y[+-][0-9]+M[+-][0-9]+D")]
[OpenApi.OpenApiDataType(description: "Date span, specified in years, months and days.", example: "1Y+10M+16D", type: "string", format: "date-span", pattern: @"[+-]?[0-9]+Y[+-][0-9]+M[+-][0-9]+D")]
[TypeConverter(typeof(DateSpanTypeConverter))]
#if NET5_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.DateSpanJsonConverter))]
#endif
public readonly partial struct DateSpan : ISerializable, IXmlSerializable, IFormattable, IEquatable<DateSpan>, IComparable, IComparable<DateSpan>
#if NET7_0_OR_GREATER
    , IAdditionOperators<DateSpan, DateSpan, DateSpan>, ISubtractionOperators<DateSpan, DateSpan, DateSpan>
#endif
{
    /// <summary>Represents the pattern of a (potential) valid year.</summary>
    private static readonly Regex Pattern = new(@"^(?<Years>([+-]?[0-9]{1,4}))Y(?<Months>([+-][0-9]{1,6}))M((?<Days>([+-][0-9]{1,7}))D)?$", RegOptions.IgnoreCase, RegOptions.Timeout);

    /// <summary>Represents the zero date span.</summary>
    public static readonly DateSpan Zero;

    /// <summary>Represents the maximum value of the date span.</summary>
    public static readonly DateSpan MaxValue = new(AsUInt64(MonthsPerYear * +9998 + 11, +30));

    /// <summary>Represents the minimum value of the date span.</summary>
    public static readonly DateSpan MinValue = new(AsUInt64(MonthsPerYear * -9998 - 11, -30));

    /// <summary>The average amount of days per month, taken leap years into account.</summary>
    internal const double DaysPerMonth = 30.421625;

    /// <summary>The total of days, that can not be applied on a <see cref="Date"/> or <see cref="DateTime"/>.</summary>
    internal const int MaxDays = (int)(DaysPerMonth * 120000);

    /// <summary>12 months per year.</summary>
    internal const int MonthsPerYear = 12;

    /// <summary>365 days per year.</summary>
    internal const int DaysPerYear = 365;

    /// <summary>366 days per leap year.</summary>
    internal const int DaysPerLeapYear = 366;

    /// <summary>The shift position of the total months in the value.</summary>
    internal const int MonthShift = 32;

    /// <summary>Creates a new instance of a <see cref="DateSpan"/>.</summary>
    /// <param name="months">
    /// Number of months.
    /// </param>
    /// <param name="days">
    /// Number of days.
    /// </param>
    public DateSpan(int months, int days) : this(AsUInt64(months, days))
    {
        if (IsOutOfRange(months, days, TotalDays))
        {
            throw new ArgumentOutOfRangeException(QowaivMessages.ArgumentOutOfRangeException_DateSpan, (Exception?)null);
        }
    }

    /// <summary>Creates a new instance of a <see cref="DateSpan"/>.</summary>
    /// <param name="years">
    /// Number of years.
    /// </param>
    /// <param name="months">
    /// Number of months.
    /// </param>
    /// <param name="days">
    /// Number of days.
    /// </param>
    public DateSpan(int years, int months, int days)
        : this(years * MonthsPerYear + months, days) { }

    /// <summary>Converts the combination of months and days to a <see cref="ulong"/>.</summary>
    [Pure]
    private static ulong AsUInt64(long months, long days) => (uint)days | ((ulong)months << MonthShift);

    /// <summary>Gets the total of months.</summary>
    public int TotalMonths => (int)(m_Value >> MonthShift);

    /// <summary>Gets the years component of the date span.</summary>
    public int Years => TotalMonths / MonthsPerYear;

    /// <summary>Gets the months component of the date span.</summary>
    public int Months => TotalMonths % MonthsPerYear;

    /// <summary>Gets the days component of the date span.</summary>
    public int Days => (int)m_Value;

    /// <summary>Gets a (approximate) value to sort the date spans by.</summary>
    internal double TotalDays => Days + TotalMonths * DaysPerMonth;

    #region Operations

    /// <summary>Unary plus the date span.</summary>
    /// <returns></returns>
    [Pure]
    internal DateSpan Plus() => this;

    /// <summary>Negates the date span.</summary>
    [Pure]
    public DateSpan Negate() => new(AsUInt64(-TotalMonths, -Days));

    /// <summary>Returns a new date span whose value is the sum of the specified date span and this instance.</summary>
    ///<param name="other">
    /// The date span to add.
    ///</param>
    ///<exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    ///</exception>
    [Pure]
    public DateSpan Add(DateSpan other)
    {
        long days = (long)Days + other.Days;
        long months = (long)TotalMonths + other.TotalMonths;
        return Mutate(months, days);
    }

    /// <summary>Returns a new date span whose value is the subtraction of the specified date span and this instance.</summary>
    ///<param name="other">
    /// The date span to subtract.
    ///</param>
    ///<exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    ///</exception>
    [Pure]
    public DateSpan Subtract(DateSpan other)
    {
        long days = (long)Days - other.Days;
        long months = (long)TotalMonths - other.TotalMonths;
        return Mutate(months, days);
    }

    /// <summary>Returns a new date span whose value is the sum of the days to add this instance.</summary>
    ///<param name="days">
    /// The days to add.
    ///</param>
    ///<exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    ///</exception>
    [Pure]
    public DateSpan AddDays(int days) => Mutate(TotalMonths, Days + (long)days);

    /// <summary>Returns a new date span whose value is the sum of the months to add this instance.</summary>
    ///<param name="months">
    /// The months to add.
    ///</param>
    ///<exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    ///</exception>
    [Pure]
    public DateSpan AddMonths(int months) => Mutate(TotalMonths + (long)months, Days);

    /// <summary>Returns a new date span whose value is the sum of the years to add this instance.</summary>
    ///<param name="years">
    /// The years to add.
    ///</param>
    ///<exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    ///</exception>
    [Pure]
    public DateSpan AddYears(int years) => Mutate(TotalMonths + years * (long)MonthsPerYear, Days);

    /// <summary>Mutates the months and days.</summary>
    ///<exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    ///</exception>
    [Pure]
    private static DateSpan Mutate(long months, long days)
    {
        var totalDays = months * DaysPerMonth + days;

        if (IsOutOfRange(months, days, totalDays))
        {
            throw new OverflowException(QowaivMessages.OverflowException_DateSpan);
        }
        return new DateSpan(AsUInt64(months, days));
    }

    #endregion

    /// <summary>Deserializes the date span from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized date span.
    /// </returns>
    [Pure]
    public static DateSpan FromJson(long json) => FromDays((int)json);

    /// <summary>Serializes the date span to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string ToJson() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Returns a <see cref="string" /> that represents the current date span for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the current date span.</summary>
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
        : string.Format(formatProvider, "{0}Y{1:+0;-0;+0}M{2:+0;-0;+0}D", Years, Months, Days);

    /// <summary>Gets an XML string representation of the date span.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    [Pure]
    public bool Equals(DateSpan other) => m_Value == other.m_Value;

    /// <inheritdoc/>
    [Pure]
    public int CompareTo(DateSpan other) => TotalDays.CompareTo(other.TotalDays);

    /// <inheritdoc/>
    [Pure]
    public override int GetHashCode() => m_Value.GetHashCode();

    /// <summary>Unary plus the date span.</summary>
    public static DateSpan operator +(DateSpan span) => span.Plus();

    /// <summary>Negates the date span.</summary>
    public static DateSpan operator -(DateSpan span) => span.Negate();

    /// <summary>Adds two date spans.</summary>
    public static DateSpan operator +(DateSpan l, DateSpan r) => l.Add(r);

    /// <summary>Subtracts two date spans.</summary>
    public static DateSpan operator -(DateSpan l, DateSpan r) => l.Subtract(r);

    /// <summary>Creates a date span from days only.</summary>
    [Pure]
    public static DateSpan FromDays(int days) => new(0, 0, days);

    /// <summary>Creates a date span from months only.</summary>
    [Pure]
    public static DateSpan FromMonths(int months) => new(0, months, 0);

    /// <summary>Creates a date span from months only.</summary>
    [Pure]
    public static DateSpan FromYears(int years) => new(years, 0, 0);

    /// <summary>Calculates the age (in years and days) for a given date for today.</summary>
    /// <param name="date">
    /// The date to get the age for.
    /// </param>
    /// <returns>
    /// The age defined in years and days.
    /// </returns>
    [Pure]
    public static DateSpan Age(Date date) => Age(date, Clock.Today());

    /// <summary>Calculates the age (in years and days) for a given date for the reference date.</summary>
    /// <param name="date">
    /// The date to get the age for.
    /// </param>
    /// <param name="reference">
    /// The reference date.
    /// </param>
    /// <returns>
    /// The age defined in years and days.
    /// </returns>
    [Pure]
    public static DateSpan Age(Date date, Date reference) => Subtract(reference, date, DateSpanSettings.WithoutMonths);

    /// <summary>Creates a date span on by subtracting <paramref name="d1"/> from <paramref name="d2"/>.</summary>
    /// <param name="d1">
    /// The date to subtract from.
    /// </param>
    /// <param name="d2">
    /// The date to subtract.
    /// </param>
    /// <returns>
    /// Returns a date span describing the duration between <paramref name="d1"/> and <paramref name="d2"/>.
    /// </returns>
    [Pure]
    public static DateSpan Subtract(Date d1, Date d2) => Subtract(d1, d2, DateSpanSettings.Default);

    /// <summary>Creates a date span on by subtracting <paramref name="d1"/> from <paramref name="d2"/>.</summary>
    /// <param name="d1">
    /// The date to subtract from.
    /// </param>
    /// <param name="d2">
    /// The date to subtract.
    /// </param>
    /// <param name="settings">
    /// The settings to apply.
    /// </param>
    /// <returns>
    /// Returns a date span describing the duration between <paramref name="d1"/> and <paramref name="d2"/>.
    /// </returns>
    [Pure]
    public static DateSpan Subtract(Date d1, Date d2, DateSpanSettings settings)
    {
        if ((settings & (DateSpanSettings.DaysOnly)) == DateSpanSettings.DaysOnly)
        {
            return FromDays((int)(d1 - d2).TotalDays);
        }
        else return d1 < d2 
            ? Subtraction(d2, d1, settings).Negate() 
            : Subtraction(d1, d2, settings);
    }

    [Pure]
    private static DateSpan Subtraction(Date max, Date min, DateSpanSettings settings)
    {
        var noMixedSings = (settings & DateSpanSettings.MixedSigns) == default;
        var daysFirst = (settings & DateSpanSettings.DaysFirst) != default;

        return (settings & DateSpanSettings.WithoutMonths) == default
            ? SubtractWithMonths(max, min, noMixedSings, daysFirst)
            : SubtractWithoutMonths(max, min, noMixedSings, daysFirst);
    }

    [Pure]
    private static DateSpan SubtractWithMonths(Date max, Date min, bool noMixedSings, bool daysFirst)
    {
        var months = max.Month - min.Month;
        var days = max.Day - min.Day;

        if (days < 0 && noMixedSings)
        {
            months--;
            var sub = daysFirst ? min : max.AddMonths(-1);
            days += DateTime.DaysInMonth(sub.Year, sub.Month);
        }
        return new DateSpan(max.Year - min.Year, months, days);
    }

    [Pure]
    private static DateSpan SubtractWithoutMonths(Date max, Date min, bool noMixedSings, bool daysFirst)
    {
        var years = max.Year - min.Year;
        var days = max.DayOfYear - min.DayOfYear;

        if (days < 0 && noMixedSings)
        {
            years--;
            var sub = daysFirst ? min : max.AddYears(-1);
            days += DateTime.IsLeapYear(sub.Year) ? DaysPerLeapYear : DaysPerYear;
        }
        return new DateSpan(years, 0, days);
    }

    

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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out DateSpan result)
    {
        result = default;

        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        var match = Pattern.Match(s);

        if (match.Success)
        {
            var y = IntFromGroup(match, nameof(Years), formatProvider);
            var m = IntFromGroup(match, nameof(Months), formatProvider);
            var d = IntFromGroup(match, nameof(Days), formatProvider);

            var months = y * 12 + m;
            var totalDays = d + months * DaysPerMonth;

            if (!IsOutOfRange(months, d, totalDays))
            {
                result = new DateSpan(AsUInt64(months, d));
                return true;
            }
        }
        return false;
    }

    [Pure]
    private static int IntFromGroup(Match match, string group, IFormatProvider? formatProvider)
    {
        var str = match.Groups[group].Value;
        return string.IsNullOrEmpty(str) ? 0 : int.Parse(str, formatProvider);
    }

    /// <summary>Returns true if the combination of months and days can not be processed.</summary>
    [Pure]
    private static bool IsOutOfRange(long months, long days, double totalDays)
        => !months.IsInRange(MinValue.TotalMonths, MaxValue.TotalMonths)
        || !days.IsInRange(-MaxDays, +MaxDays)
        || !totalDays.IsInRange(MinValue.TotalDays, MaxValue.TotalDays);
}
