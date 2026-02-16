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

    /// <summary>Returns a formatted <see cref="string" /> that represents the year span.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
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

    /// <summary>Casts the year span to a <see cref="int"/>.</summary>
    public static explicit operator int(YearSpan val) => val.m_Value;

    /// <summary>Casts a <see cref="int"/> to a year span.</summary>
    public static explicit operator YearSpan(int val) => Create(val);

    /// <summary>Converts the <see cref="string"/> to <see cref="YearSpan"/>.
    /// A return value indicates whether the conversion succeeded.
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

    /// <summary>Creates a date span.</summary>
    [Pure]
    public static YearSpan Create(int months)
        => TryCreate(months, out var yearSpan)
        ? yearSpan
        : throw new ArgumentOutOfRangeException(nameof(months), QowaivMessages.FormatExceptionYearSpan);

    /// <summary>Tries to Create a year span from.</summary>
    public static bool TryCreate(long? months, out YearSpan yearSpan)
    {
        yearSpan = default;
        if (months.HasValue && months >= MinValue.m_Value && months <= MaxValue.m_Value)
        {
            yearSpan = new((int)months);
            return true;
        }
        return false;
    }
}
