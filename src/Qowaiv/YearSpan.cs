namespace Qowaiv;

/// <summary>Represents a year span.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.Continuous, typeof(int))]
[OpenApiDataType(description: "Year span", type: "int", format: "year-span", example: 17)]
[TypeConverter(typeof(Conversion.YearSpanTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.YearSpanJsonConverter))]
#endif
public readonly partial struct YearSpan : IXmlSerializable, IFormattable, IEquatable<YearSpan>, IComparable, IComparable<YearSpan>
{
    /// <summary>Represents a year span with a zero duration.</summary>
    public static YearSpan Zero => default;

    /// <summary>Gets the minimum year span (-9999 years).</summary>
    public static YearSpan MinValue => new(-9999);

    /// <summary>Gets the maximum year span (+9999 years).</summary>
    public static YearSpan MaxValue => new(+9999);

    /// <summary>Returns a <see cref="string" /> that represents the year span for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0} years");

    /// <summary>Returns the absolute value of the year span.</summary>
    [Pure]
    public YearSpan Abs() => new(Math.Abs(m_Value));

    /// <summary>
    /// Returns an integer that indicates the sign of the year span.
    /// </summary>
    /// <returns>
    /// A number that indicates the sign of value, as shown in the following table.
    ///
    /// Return value – Meaning
    /// -1 –value is less than zero.
    /// 0 –value is equal to zero.
    /// 1 –value is greater than zero.
    /// </returns>
    [Pure]
    public int Sign() => Math.Sign(m_Value);

    /// <summary>Unary plus the year span.</summary>
    public static YearSpan operator +(YearSpan span) => span;

    /// <summary>Negates the year span.</summary>
    public static YearSpan operator -(YearSpan span) => new(-span.m_Value);

    /// <summary>Increases the year span with one year.</summary>
    public static YearSpan operator ++(YearSpan span) => new(span.m_Value + 1);

    /// <summary>Decreases the year span with one year.</summary>
    public static YearSpan operator --(YearSpan span) => new(span.m_Value - 1);

    /// <summary>Adds two year spans.</summary>
    public static YearSpan operator +(YearSpan l, YearSpan r) => new(l.m_Value + r.m_Value);

    /// <summary>Subtracts two year spans.</summary>
    public static YearSpan operator -(YearSpan l, YearSpan r) => new(l.m_Value - r.m_Value);

    /// <summary>Multiplies the year span with a factor.</summary>
    public static YearSpan operator *(YearSpan span, int factor) => new(span.m_Value * factor);

    /// <summary>Multiplies the year span with a factor.</summary>
    public static YearSpan operator *(YearSpan span, short factor) => new(span.m_Value * factor);

    /// <summary>Multiplies the year span with a factor.</summary>
    public static YearSpan operator *(YearSpan span, decimal factor) => new(Cast.ToInt<YearSpan>(span.m_Value * factor));

    /// <summary>Multiplies the year span with a factor.</summary>
    public static YearSpan operator *(YearSpan span, double factor) => new(Cast.ToInt<YearSpan>(span.m_Value * factor));

    /// <summary>Divides the year span by a factor.</summary>
    public static YearSpan operator /(YearSpan span, int factor) => new(span.m_Value / factor);

    /// <summary>Divides the year span by a factor.</summary>
    public static YearSpan operator /(YearSpan span, short factor) => new(span.m_Value / factor);

    /// <summary>Divides the year span by a factor.</summary>
    public static YearSpan operator /(YearSpan span, decimal factor) => new(Cast.ToInt<YearSpan>(span.m_Value / factor));

    /// <summary>Divides the year span by a factor.</summary>
    public static YearSpan operator /(YearSpan span, double factor) => new(Cast.ToInt<YearSpan>(span.m_Value / factor));

    /// <summary>Casts the year span to a <see cref="int"/>.</summary>
    public static explicit operator int(YearSpan val) => val.m_Value;

    /// <summary>Casts a <see cref="int"/> to a year span.</summary>
    public static explicit operator YearSpan(int val) => Create(val);

    /// <summary>Casts a month span to a year span.</summary>
    public static implicit operator YearSpan(MonthSpan months) => new(months.Years);

    /// <summary>Returns a formatted <see cref="string" /> that represents the year span.</summary>
    /// <param name="format">
    /// The string that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : m_Value.ToString(format, formatProvider);

    /// <summary>Gets an XML string representation of the year span.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the year span to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON number.
    /// </returns>
    [Pure]
    public int ToJson() => m_Value;

    /// <summary>Deserializes the year span from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized year span.
    /// </returns>
    [Pure]
    public static YearSpan FromJson(double json) => new(Cast.ToInt<YearSpan>(json));

    /// <summary>Deserializes the year span from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized year span.
    /// </returns>
    [Pure]
    public static YearSpan FromJson(long json) => new(Cast.ToInt<YearSpan>(json));

    /// <summary>Converts the <see cref="string"/> to <see cref="YearSpan"/>.
    /// The return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the year span to convert.
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
    [Pure]
    public static bool TryParse(string? s, IFormatProvider? provider, out YearSpan result)
    {
        result = default;
        if (int.TryParse(s, NumberStyles.Integer, provider, out var span))
        {
            result = new(span);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>Creates a year span.</summary>
    [Pure]
    public static YearSpan Create(int years)
        => TryCreate(years, out var yearSpan)
        ? yearSpan
        : throw new ArgumentOutOfRangeException(nameof(years), QowaivMessages.FormatExceptionYearSpan);

    /// <summary>Tries to Create a year span from.</summary>
    public static bool TryCreate(long? years, out YearSpan yearSpan)
    {
        yearSpan = default;
        if (years is { } y && y >= MinValue.m_Value && y <= MaxValue.m_Value)
        {
            yearSpan = new(y);
            return true;
        }
        return false;
    }
}
