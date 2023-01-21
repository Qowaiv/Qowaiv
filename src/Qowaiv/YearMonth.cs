namespace Qowaiv;

/// <summary>Represents a year-month.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All ^ SingleValueStaticOptions.HasEmptyValue ^ SingleValueStaticOptions.HasUnknownValue, typeof(int))]
[OpenApiDataType(description: "Date notation with month precision.", example: "2017-06", type: "string", format: "year-month", pattern: "[0-9]{4}-[01][0-9]")]
[OpenApi.OpenApiDataType(description: "Date notation with month precision.", example: "2017-06", type: "string", format: "year-month", pattern: "[0-9]{4}-[01][0-9]")]
[TypeConverter(typeof(Conversion.YearMonthTypeConverter))]
#if NET5_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(YearMonthJsonConverter))]
#endif
public readonly partial struct YearMonth : ISerializable, IXmlSerializable, IFormattable, IEquatable<YearMonth>, IComparable, IComparable<YearMonth>
{
    /// <summary>Represents the smallest possible year-month (0001-01).</summary>
    public static readonly YearMonth MinValue = new(0001, 01);

    /// <summary>Represents the largest possible year-month (9999-12).</summary>
    public static readonly YearMonth MaxValue = new(9999, 12);

    /// <summary>12 months per year.</summary>
    private const int MonthsPerYear = 12;

    /// <summary>Creates a new instance of the <see cref="YearMonth"/> struct.</summary>
    /// <param name="year">
    /// The year of the year-month.
    /// </param>
    /// <param name="month">
    /// The month of the year-month.
    /// </param>
    public YearMonth(int year, int month) : this(Create(year, month)) { }

    /// <summary>Gets the year component of the date represented by this instance.</summary>
    public int Year => 1 + m_Value / MonthsPerYear;

    /// <summary>Gets the month component of the date represented by this instance.</summary>
    public int Month => 1 + m_Value % MonthsPerYear;

    /// <summary>Returns a <see cref="string" /> that represents the year-month for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:yyyy-MM}");

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

    /// <summary>Converts the string to a 
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

    /// <summary>Converts the string to a 
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
        return (year - 1) * MonthsPerYear + month - 1;
    }
}
