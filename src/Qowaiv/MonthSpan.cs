namespace Qowaiv;

/// <summary>Represents a month span.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[SingleValueObject(SingleValueStaticOptions.Continuous, underlyingType: typeof(int))]
[OpenApiDataType(description: "Month span, specified in years and months.", example: "1Y+10M", type: "string", format: "month-span", pattern: @"[+-]?[0-9]+Y[+-][0-9]+M")]
[TypeConverter(typeof(MonthSpanTypeConverter))]
#if NET8_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.MonthSpanJsonConverter))]
#endif
public readonly partial struct MonthSpan : IXmlSerializable, IFormattable, IEquatable<MonthSpan>, IComparable, IComparable<MonthSpan>
#if NET8_0_OR_GREATER
    , IIncrementOperators<MonthSpan>, IDecrementOperators<MonthSpan>
    , IUnaryPlusOperators<MonthSpan, MonthSpan>, IUnaryNegationOperators<MonthSpan, MonthSpan>
    , IAdditionOperators<MonthSpan, MonthSpan, MonthSpan>, ISubtractionOperators<MonthSpan, MonthSpan, MonthSpan>
    , IMultiplyOperators<MonthSpan, decimal, MonthSpan>, IDivisionOperators<MonthSpan, decimal, MonthSpan>
    , IMultiplyOperators<MonthSpan, double, MonthSpan>, IDivisionOperators<MonthSpan, double, MonthSpan>
    , IMultiplyOperators<MonthSpan, int, MonthSpan>, IDivisionOperators<MonthSpan, int, MonthSpan>
    , IMultiplyOperators<MonthSpan, short, MonthSpan>, IDivisionOperators<MonthSpan, short, MonthSpan>
    , IMinMaxValue<MonthSpan>
#endif
{
    /// <summary>Represents a month span with a zero duration.</summary>
    public static MonthSpan Zero => default;

    /// <summary>Gets the minimum month span (-9998 years).</summary>
    public static MonthSpan MinValue => new(-9998 * 12);

    /// <summary>Gets the maximum month span (+9998 years).</summary>
    public static MonthSpan MaxValue => new(+9998 * 12);

    /// <summary>Initializes a new instance of the <see cref="MonthSpan" /> struct.</summary>
    /// <param name="years">
    /// The total of years of the month span.</param>
    /// <param name="months">
    /// The additional months on top of the years.
    /// </param>
    public MonthSpan(int years, int months) : this(Create(years, months)) { }

    /// <summary>Gets the total of months.</summary>
    public int TotalMonths => m_Value;

    /// <summary>Gets the years component of the month span.</summary>
    public int Years => TotalMonths / DateSpan.MonthsPerYear;

    /// <summary>Gets the months component of the month span.</summary>
    public int Months => TotalMonths % DateSpan.MonthsPerYear;

    /// <summary>Deconstructs the month span in a years, and months.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out int years, out int months)
    {
        years = Years;
        months = Months;
    }

    /// <summary>Unary plus the month span.</summary>
    [Pure]
    internal MonthSpan Plus() => this;

    /// <summary>Negates the month span.</summary>
    [Pure]
    public MonthSpan Negate() => new(-m_Value);

    /// <summary>Increases the month span with one month.</summary>
    [Pure]
    private MonthSpan Increment() => new(m_Value + 1);

    /// <summary>Decreases the month span with one month.</summary>
    [Pure]
    private MonthSpan Decrement() => new(m_Value - 1);

    /// <summary>Returns a new month span whose value is the sum of the specified month span and this instance.</summary>
    /// <param name="other">
    /// The month span to add.
    /// </param>
    /// <exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue" /> or greater than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public MonthSpan Add(MonthSpan other) => FromMonths(m_Value + other.m_Value);

    /// <summary>Returns a new month span whose value is the subtraction of the specified month span and this instance.</summary>
    /// <param name="other">
    /// The month span to subtract.
    /// </param>
    /// <exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue" /> or greater than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public MonthSpan Subtract(MonthSpan other) => FromMonths(m_Value - other.m_Value);

    /// <summary>Returns a new month span whose value is the multiplication of the specified factor and this instance.</summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    /// <exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue" /> or greater than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public MonthSpan Multiply(int factor) => FromMonths(m_Value * factor);

    /// <summary>Returns a new month span whose value is the multiplication of the specified factor and this instance.</summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    /// <exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue" /> or greater than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public MonthSpan Multiply(decimal factor) => FromMonths(Cast.ToInt<MonthSpan>((long)(m_Value * factor)));

    /// <summary>Returns a new month span whose value is the multiplication of the specified factor and this instance.</summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    /// <exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue" /> or greater than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public MonthSpan Multiply(double factor) => Multiply(Cast.ToDecimal<MonthSpan>(factor));

    /// <summary>Returns a new month span whose value is the division of the specified factor and this instance.</summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    /// <exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue" /> or greater than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public MonthSpan Divide(int factor) => FromMonths(m_Value / factor);

    /// <summary>Returns a new month span whose value is the division of the specified factor and this instance.</summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    /// <exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue" /> or greater than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public MonthSpan Divide(decimal factor) => FromMonths(Cast.ToInt<MonthSpan>((long)(m_Value / factor)));

    /// <summary>Returns a new month span whose value is the division of the specified factor and this instance.</summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    /// <exception cref="OverflowException">
    /// The resulting time span is less than <see cref="MinValue" /> or greater than <see cref="MaxValue" />.
    /// </exception>
    [Pure]
    public MonthSpan Divide(double factor) => Divide(Cast.ToDecimal<MonthSpan>(factor));

    /// <summary>Unary plus the month span.</summary>
    public static MonthSpan operator +(MonthSpan span) => span.Plus();

    /// <summary>Negates the month span.</summary>
    public static MonthSpan operator -(MonthSpan span) => span.Negate();

    /// <summary>Increases the month span with one month.</summary>
    public static MonthSpan operator ++(MonthSpan span) => span.Increment();

    /// <summary>Decreases the monthspan with one month.</summary>
    public static MonthSpan operator --(MonthSpan span) => span.Decrement();

    /// <summary>Adds two month spans.</summary>
    public static MonthSpan operator +(MonthSpan l, MonthSpan r) => l.Add(r);

    /// <summary>Subtracts two month spans.</summary>
    public static MonthSpan operator -(MonthSpan l, MonthSpan r) => l.Subtract(r);

    /// <summary>Multiplies the month span with a factor.</summary>
    public static MonthSpan operator *(MonthSpan span, int factor) => span.Multiply(factor);

    /// <summary>Multiplies the month span with a factor.</summary>
    public static MonthSpan operator *(MonthSpan span, short factor) => span.Multiply(factor);

    /// <summary>Multiplies the month span with a factor.</summary>
    public static MonthSpan operator *(MonthSpan span, decimal factor) => span.Multiply(factor);

    /// <summary>Multiplies the month span with a factor.</summary>
    public static MonthSpan operator *(MonthSpan span, double factor) => span.Multiply(factor);

    /// <summary>Divides the month span by a factor.</summary>
    public static MonthSpan operator /(MonthSpan span, int factor) => span.Divide(factor);

    /// <summary>Divides the month span by a factor.</summary>
    public static MonthSpan operator /(MonthSpan span, short factor) => span.Divide(factor);

    /// <summary>Divides the month span by a factor.</summary>
    public static MonthSpan operator /(MonthSpan span, decimal factor) => span.Divide(factor);

    /// <summary>Divides the month span by a factor.</summary>
    public static MonthSpan operator /(MonthSpan span, double factor) => span.Divide(factor);

    /// <summary>Adds a month span to the date time.</summary>
    public static DateTime operator +(DateTime dt, MonthSpan span) => dt.Add(span);

    /// <summary>Subtracts a month span from the date time.</summary>
    public static DateTime operator -(DateTime dt, MonthSpan span) => dt.Add(-span);

#if NET8_0_OR_GREATER
    /// <summary>Adds a month span to the date only.</summary>
    public static DateOnly operator +(DateOnly dt, MonthSpan span) => dt.Add(span);

    /// <summary>Subtracts a month span from the date only.</summary>
    public static DateOnly operator -(DateOnly dt, MonthSpan span) => dt.Add(-span);
#endif

    /// <summary>Returns a <see cref = "string " /> that represents the month span for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Returns a formatted <see cref = "string " /> that represents the month span.</summary>
    /// <param name = "format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name = "formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// The formats:
    ///
    /// F: as 0Y+0M.
    /// All others format the total months.
    /// </remarks>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        _ when StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted) => formatted,
        null or "" or "F" => $"{Years}Y{Months:+0;-0;+0}M",
        _ => m_Value.ToString(format, formatProvider),
    };

    /// <summary>Gets an XML string representation of the month span.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the month span to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string ToJson() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Deserializes the month span from a JSON number.</summary>
    /// <param name = "json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized month span.
    /// </returns>
    [Pure]
    public static MonthSpan FromJson(long json) => Cast.Primitive<long, MonthSpan>(TryCreate, json);

    /// <summary>Casts the month span to a <see cref = "int" />.</summary>
    public static explicit operator int(MonthSpan val) => val.m_Value;

    /// <summary>Casts a <see cref = "int " /> to a month span.</summary>
    public static explicit operator MonthSpan(int val) => FromMonths(val);

    /// <summary>Casts the month span to a <see cref="DateSpan" />.</summary>
    public static implicit operator DateSpan(MonthSpan val) => DateSpan.FromMonths(val.m_Value);

    /// <summary>Casts a <see cref = "DateSpan" /> to a month span.</summary>
    public static explicit operator MonthSpan(DateSpan val) => FromMonths(val.TotalMonths);

    /// <summary>Converts the <see cref = "string " /> to <see cref = "MonthSpan" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name = "s">
    /// A string containing the month span to convert.
    /// </param>
    /// <param name = "provider">
    /// The specified format provider.
    /// </param>
    /// <param name = "result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out MonthSpan result)
    {
        result = default;
        if (string.IsNullOrEmpty(s))
        {
            return true;
        }
        else if (int.TryParse(s, NumberStyles.Integer, provider, out var months)
            && TryCreate(months, out result))
        {
            return true;
        }
        else if (DateSpan.TryParse(s, provider, out var dateSpan))
        {
            result = FromMonths(dateSpan.TotalMonths);
            return true;
        }
        else return false;
    }

    /// <summary>Creates a date span from years.</summary>
    [Pure]
    public static MonthSpan FromYears(int years)
    {
        if (TryCreate(years * DateSpan.MonthsPerYear, out var monthSpan))
        {
            return monthSpan;
        }
        throw new ArgumentOutOfRangeException(nameof(years), QowaivMessages.FormatExceptionMonthSpan);
    }

    /// <summary>Creates a date span from months.</summary>
    [Pure]
    public static MonthSpan FromMonths(int months)
    {
        if (TryCreate(months, out var monthSpan))
        {
            return monthSpan;
        }
        throw new ArgumentOutOfRangeException(nameof(months), QowaivMessages.FormatExceptionMonthSpan);
    }

    /// <summary>Creates a month span by subtracting <paramref name="d2" /> from <paramref name="d1" />.</summary>
    /// <param name="d1">
    /// The date to subtract from.
    /// </param>
    /// <param name="d2">
    /// The date to subtract.
    /// </param>
    /// <returns>
    /// Returns a month span describing the duration between <paramref name="d1" /> and <paramref name="d2" />.
    /// </returns>
    [Pure]
    public static MonthSpan Subtract(Date d1, Date d2)
    {
        var max = d1;
        var min = d2;

        var negative = d1 < d2;

        if (negative)
        {
            (min, max) = (max, min);
        }

        var years = Math.Max(0, max.Year - min.Year - 1);
        var months = years * 12;

        var test = min.AddYears(years);

        while (test < max)
        {
            test = test.AddMonths(1);
            months++;
        }
        if (test > max)
        {
            months--;
        }

        return new MonthSpan(negative ? -months : +months);
    }

    /// <summary>Tries to Create a date span from months only.</summary>
    public static bool TryCreate(long? months, out MonthSpan monthSpan)
    {
        monthSpan = default;
        if (months.HasValue && months >= MinValue.TotalMonths && months <= MaxValue.TotalMonths)
        {
            monthSpan = new MonthSpan((int)months);
            return true;
        }
        return false;
    }

    [Pure]
    private static int Create(int years, int months)
        => TryCreate((years * DateSpan.MonthsPerYear) + months, out var span)
        ? span.TotalMonths
        : throw new ArgumentOutOfRangeException(QowaivMessages.FormatExceptionMonthSpan);
}
