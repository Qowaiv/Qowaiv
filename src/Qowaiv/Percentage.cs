using Qowaiv.Mathematics;

namespace Qowaiv;

/// <summary>Represents a Percentage.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All ^ SingleValueStaticOptions.HasEmptyValue ^ SingleValueStaticOptions.HasUnknownValue, typeof(decimal))]
[OpenApiDataType(description: "Ratio expressed as a fraction of 100 denoted using the percent sign '%'.", example: "13.76%", type: "string", format: "percentage", pattern: @"-?[0-9]+(\.[0-9]+)?%")]
[TypeConverter(typeof(PercentageTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.PercentageJsonConverter))]
#endif
public readonly partial struct Percentage : IXmlSerializable, IFormattable, IEquatable<Percentage>, IComparable, IComparable<Percentage>
#if NET8_0_OR_GREATER
    , IMinMaxValue<Percentage>
#endif
{
    /// <summary>The percentage symbol (%).</summary>
    public static readonly string PercentSymbol = "%";

    /// <summary>The per mille symbol (‰).</summary>
    public static readonly string PerMilleSymbol = "‰";

    /// <summary>The per ten thousand symbol (0/000).</summary>
    public static readonly string PerTenThousandSymbol = "‱";

    private const string DefaultFormat = "0.############################";

    /// <summary>Represents 0 percent.</summary>
    public static Percentage Zero => default;

    /// <summary>Represents 1 percent.</summary>
    public static Percentage One => new(0.01m);

    /// <summary>Represents 100 percent.</summary>
    public static Percentage Hundred => new(1);

    /// <summary>Gets the minimum value of a percentage.</summary>
    public static Percentage MinValue => new(-7_922_816_251_426_433_759_354_395.0335m);

    /// <summary>Gets the maximum value of a percentage.</summary>
    public static Percentage MaxValue => new(+7_922_816_251_426_433_759_354_395.0335m);

    /// <summary>Gets the sign of the percentage.</summary>
    [Pure]
    public int Sign() => m_Value.Sign();

    /// <summary>Returns the absolute value of the percentage.</summary>
    [Pure]
    public Percentage Abs() => new(Math.Abs(m_Value));

    /// <summary>Returns the larger of two percentages.</summary>
    /// <param name="val1">
    /// The second of the two percentages to compare.
    /// </param>
    /// <param name="val2">
    /// The first of the two percentages to compare.
    /// </param>
    /// <returns>
    /// Parameter val1 or val2, whichever is larger.
    /// </returns>
    [Pure]
    public static Percentage Max(Percentage val1, Percentage val2) => val1 > val2 ? val1 : val2;

    /// <summary>Returns the largest of the percentages.</summary>
    /// <param name="values">
    /// The percentages to compare.
    /// </param>
    /// <returns>
    /// The percentage with the largest value.
    /// </returns>
    [Pure]
    public static Percentage Max(params Percentage[] values) => Guard.NotNull(values).Max();

    /// <summary>Returns the smaller of two percentages.</summary>
    /// <param name="val1">
    /// The second of the two percentages to compare.
    /// </param>
    /// <param name="val2">
    /// The first of the two percentages to compare.
    /// </param>
    /// <returns>
    /// Parameter val1 or val2, whichever is smaller.
    /// </returns>
    [Pure]
    public static Percentage Min(Percentage val1, Percentage val2) => val1 < val2 ? val1 : val2;

    /// <summary>Returns the smallest of the percentages.</summary>
    /// <param name="values">
    /// The percentages to compare.
    /// </param>
    /// <returns>
    /// The percentage with the smallest value.
    /// </returns>
    [Pure]
    public static Percentage Min(params Percentage[] values) => Guard.NotNull(values).Min();

    /// <summary>Rounds the percentage.</summary>
    /// <returns>
    /// The percentage nearest to the percentage that contains zero
    /// fractional digits. If the percentage has no fractional digits,
    /// the percentage is returned unchanged.
    /// </returns>
    [Pure]
    public Percentage Round() => Round(0);

    /// <summary>Rounds the percentage to a specified number of fractional digits.</summary>
    /// <param name="decimals">
    /// The number of decimal places in the return value.
    /// </param>
    /// <returns>
    /// The percentage nearest to the percentage that contains a number of
    /// fractional digits equal to <paramref name="decimals" />. If the
    /// percentage has fewer fractional digits than <paramref name="decimals" />,
    /// the percentage is returned unchanged.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="decimals" /> is less than 0 or greater than 26.
    /// </exception>
    [Pure]
    public Percentage Round(int decimals) => Round(decimals, DecimalRounding.AwayFromZero);

    /// <summary>Rounds the percentage to a specified number of fractional
    /// digits. A parameter specifies how to round the value if it is midway
    /// between two numbers.
    /// </summary>
    /// <param name="decimals">
    /// The number of decimal places in the return value.
    /// </param>
    /// <param name="mode">
    /// Specification for how to round if it is midway between two other numbers.
    /// </param>
    /// <returns>
    /// The percentage nearest to the percentage that contains a number of
    /// fractional digits equal to <paramref name="decimals" />. If the
    /// percentage has fewer fractional digits than <paramref name="decimals" />,
    /// the percentage is returned unchanged.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="decimals" /> is less than 0 or greater than 26.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="mode" /> is not a valid value of <see cref="MidpointRounding" />.
    /// </exception>
    [Obsolete("Use  Round(decimals, DecimalRounding) instead.")]
    public Percentage Round(int decimals, MidpointRounding mode) => Round(decimals, (DecimalRounding)mode);

    /// <summary>Rounds the percentage to a specified number of fractional
    /// digits. A parameter specifies how to round the value if it is midway
    /// between two numbers.
    /// </summary>
    /// <param name="decimals">
    /// The number of decimal places in the return value.
    /// </param>
    /// <param name="mode">
    /// Specification for how to round if it is midway between two other numbers.
    /// </param>
    /// <returns>
    /// The percentage nearest to the percentage that contains a number of
    /// fractional digits equal to <paramref name="decimals" />. If the
    /// percentage has fewer fractional digits than <paramref name="decimals" />,
    /// the percentage is returned unchanged.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="decimals" /> is less than 0 or greater than 26.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="mode" /> is not a valid value of <see cref="DecimalRounding" />.
    /// </exception>
    [Pure]
    public Percentage Round(int decimals, DecimalRounding mode)
        => decimals >= -26 && decimals <= 26
        ? new(m_Value.Round(decimals + 2, mode))
        : throw new ArgumentOutOfRangeException(nameof(decimals), QowaivMessages.ArgumentOutOfRange_PercentageRound);

    /// <summary>Rounds the percentage to a specified multiple of the specified percentage.</summary>
    /// <param name="multipleOf">
    /// The percentage of which the number should be multiple of.
    /// </param>
    [Pure]
    public Percentage RoundToMultiple(Percentage multipleOf)
        => RoundToMultiple(multipleOf, DecimalRounding.AwayFromZero);

    /// <summary>Rounds the percentage to a specified multiple of the specified percentage.</summary>
    /// <param name="multipleOf">
    /// The percentage of which the number should be multiple of.
    /// </param>
    /// <param name="mode">
    /// Specification for how to round if it is midway between two other numbers.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="mode" /> is not a valid value of <see cref="DecimalRounding" />.
    /// </exception>
    [Pure]
    public Percentage RoundToMultiple(Percentage multipleOf, DecimalRounding mode)
        => new(m_Value.RoundToMultiple((decimal)multipleOf, mode));

    /// <summary>Deserializes the percentage from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized percentage.
    /// </returns>
    [Pure]
    public static Percentage FromJson(double json) => new(Cast.ToDecimal<Percentage>(json));

    /// <summary>Serializes the percentage to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string ToJson() => ToString(DefaultFormat + PercentSymbol, CultureInfo.InvariantCulture);

    /// <summary>Returns a <see cref="string" /> that represents the current Percentage for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => ToString("0.00##########################%", CultureInfo.InvariantCulture);

    /// <summary>Returns a formatted <see cref="string" /> that represents the current Percentage.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        _ when StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted) => formatted,
        _ when FormatInfo.TryParse(format, formatProvider, out var info) => info.ToString(m_Value),
        _ => throw new FormatException(QowaivMessages.FormatException_InvalidFormat),
    };

    /// <summary>Gets an XML string representation of the percentage.</summary>
    [Pure]
    private string ToXmlString() => ToString(DefaultFormat + PercentSymbol, CultureInfo.InvariantCulture);

    /// <summary>Casts a decimal to a Percentage.
    /// <example>E.g:
    /// (Percentage)1.23m => 1.23%
    /// (Percentage)0.1m => 0.1%
    /// </example>
    /// </summary>
    public static explicit operator Percentage(decimal val) => new(val);

    /// <summary>Casts a decimal to a Percentage.
    /// <example>E.g:
    /// (Percentage)1.23 => 1.23%
    /// (Percentage)0.1 => 0.1%
    /// </example>
    /// </summary>
    public static explicit operator Percentage(double val) => Create(val);

    /// <summary>Casts a Percentage to a decimal.
    /// <example>E.g:
    /// 1.23% => 1.23m
    /// 0.1% => 0.1m
    /// </example>
    /// </summary>
    public static explicit operator decimal(Percentage val) => val.m_Value;

    /// <summary>Casts a Percentage to a double.
    /// <example>E.g:
    /// 1.23% => 1.23
    /// 0.1% => 0.1
    /// </example>
    /// </summary>
    public static explicit operator double(Percentage val) => (double)val.m_Value;

    /// <summary>Converts the string to a Percentage.
    /// A return value indicates whether the conversion succeeded.
    /// <example>E.g:
    /// "1.23%" => 1.23%
    /// "175.1‰" => 17.51%
    /// "0.1" => 0.1%
    /// </example>
    /// </summary>
    /// <param name="s">
    /// A string containing a Percentage to convert.
    /// </param>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out Percentage result)
        => TryParse(s, NumberStyles.Number, provider, out result);

    /// <summary>Converts the string to a Percentage.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing a Percentage to convert.
    /// </param>
    /// <param name="style">
    /// The preferred number style.
    /// </param>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out Percentage result)
    {
        Guard(style);

        result = Zero;

        if (s is { Length: > 0 }
            && FormatInfo.TryParse(s, provider, out var info)
            && TryDecimal(info, style, out var dec)
            && dec.IsInRange(MinValue.m_Value, MaxValue.m_Value))
        {
            result = new(dec);
            return true;
        }
        return false;

        static void Guard(NumberStyles style)
        {
            var extra = style & ~NumberStyles.Number;
            if (extra != NumberStyles.None)
            {
                throw new ArgumentOutOfRangeException(nameof(style), string.Format(QowaivMessages.ArgumentOutOfRange_NumberStyleNotSupported, extra));
            }
        }

        static bool TryDecimal(FormatInfo info, NumberStyles style, out decimal dec)
        {
            if (decimal.TryParse(info.Format, style, info.Provider, out dec))
            {
                dec = DecimalMath.ChangeScale(dec, info.ScaleShift);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>Creates a Percentage from a Decimal.
    /// <example>E.g:
    /// Percentage.Create(1.23m) => 123%
    /// Percentage.Create(0.1m) => 10%
    /// </example>
    /// </summary >
    /// <param name="val" >
    /// A decimal describing a Percentage.
    /// </param >
    [Pure]
    public static Percentage Create(decimal val)
    => val.IsInRange(MinValue.m_Value, MaxValue.m_Value)
        ? new(val)
        : throw new ArgumentOutOfRangeException(QowaivMessages.ArgumentOutOfRange_Percentage, (Exception?)null);

    /// <summary>Creates a Percentage from a Double.
    /// <example>E.g:
    /// Percentage.Create(1.23) => 123%
    /// Percentage.Create(0.1) => 10%
    /// </example>
    /// </summary >
    /// <param name="val" >
    /// A decimal describing a Percentage.
    /// </param >
    [Pure]
    public static Percentage Create(double val) => Create(Cast.ToDecimal<Percentage>(val));
}
