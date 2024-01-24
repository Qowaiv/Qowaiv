using Qowaiv.Conversion.Financial;

namespace Qowaiv.Financial;

/// <summary>Represents an amount.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.Continuous, typeof(decimal))]
[OpenApiDataType(description: "Decimal representation of a currency amount.", example: 15.95, type: "number", format: "amount")]
[TypeConverter(typeof(AmountTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Financial.AmountJsonConverter))]
#endif
public readonly partial struct Amount : IXmlSerializable, IFormattable, IEquatable<Amount>, IComparable, IComparable<Amount>
#if NET8_0_OR_GREATER
    , IMinMaxValue<Amount>
#endif
{
    /// <summary>Represents the smallest possible value of the amount.</summary>
    public static Amount MinValue => new(decimal.MinValue);

    /// <summary>Represents the biggest possible value of the amount.</summary>
    public static Amount MaxValue => new(decimal.MaxValue);

    /// <summary>Gets the sign of the value of the amount.</summary>
    [Pure]
    public int Sign() => m_Value.Sign();

    /// <summary>Returns the absolute value of the amount.</summary>
    [Pure]
    public Amount Abs() => new(m_Value.Abs());

    /// <summary>Rounds the amount value to zero decimal places.</summary>
    [Pure]
    public Amount Round() => Round(0);

    /// <summary>Rounds the amount value to a specified number of decimal places.</summary>
    /// <param name="decimals">
    /// A value from -28 to 28 that specifies the number of decimal places to round to.
    /// </param>
    /// <remarks>
    /// A negative value for <paramref name="decimals"/> lowers precision to tenfold, hundredfold, and bigger.
    /// </remarks>
    [Pure]
    public Amount Round(int decimals) => Round(decimals, DecimalRounding.BankersRound);

    /// <summary>Rounds the amount value to a specified number of decimal places.</summary>
    /// <param name="decimals">
    /// A value from -28 to 28 that specifies the number of decimal places to round to.
    /// </param>
    /// <param name="mode">
    /// The mode of rounding applied.
    /// </param>
    /// <remarks>
    /// A negative value for <paramref name="decimals"/> lowers precision to tenfold, hundredfold, and bigger.
    /// </remarks>
    [Pure]
    public Amount Round(int decimals, DecimalRounding mode) => (Amount)m_Value.Round(decimals, mode);

    /// <summary>Rounds the amount value to the closed number that is a multiple of the specified factor.</summary>
    /// <param name="multipleOf">
    /// The factor of which the number should be multiple of.
    /// </param>
    [Pure]
    public Amount RoundToMultiple(decimal multipleOf) => RoundToMultiple(multipleOf, DecimalRounding.BankersRound);

    /// <summary>Rounds the amount value to the closed number that is a multiple of the specified factor.</summary>
    /// <param name="multipleOf">
    /// The factor of which the number should be multiple of.
    /// </param>
    /// <param name="mode">
    /// The rounding method used to determine the closed by number.
    /// </param>
    [Pure]
    public Amount RoundToMultiple(decimal multipleOf, DecimalRounding mode) => (Amount)m_Value.RoundToMultiple(multipleOf, mode);

    /// <summary>Returns the larger of two amounts.</summary>
    /// <param name="val1">
    /// The second of the two amounts to compare.
    /// </param>
    /// <param name="val2">
    /// The first of the two amounts to compare.
    /// </param>
    /// <returns>
    /// Parameter val1 or val2, whichever is larger.
    /// </returns>
    [Pure]
    public static Amount Max(Amount val1, Amount val2) => val1 > val2 ? val1 : val2;

    /// <summary>Returns the largest of the amounts.</summary>
    /// <param name="values">
    /// The amounts to compare.
    /// </param>
    /// <returns>
    /// The amount with the largest value.
    /// </returns>
    [Pure]
    public static Amount Max(params Amount[] values) => Guard.NotNull(values).Max();

    /// <summary>Returns the smaller of two amounts.</summary>
    /// <param name="val1">
    /// The second of the two amounts to compare.
    /// </param>
    /// <param name="val2">
    /// The first of the two amounts to compare.
    /// </param>
    /// <returns>
    /// Parameter val1 or val2, whichever is smaller.
    /// </returns>
    [Pure]
    public static Amount Min(Amount val1, Amount val2) => val1 < val2 ? val1 : val2;

    /// <summary>Returns the smallest of the amounts.</summary>
    /// <param name="values">
    /// The amounts to compare.
    /// </param>
    /// <returns>
    /// The amount with the smallest value.
    /// </returns>
    [Pure]
    public static Amount Min(params Amount[] values) => Guard.NotNull(values).Min();

    /// <summary>Serializes the amount to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON number.
    /// </returns>
    /// <remarks>
    /// Some <see cref="decimal.Zero"/> representations will be cast to a
    /// <see cref="double"/> value that slightly smaller than 0, at least
    /// enough to have a <see cref="string"/> representation of -0.
    /// </remarks>
    [Pure]
    public double ToJson() => (double)m_Value;

    /// <summary>Returns a <see cref="string"/> that represents the current Amount for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("¤{0:0.00########}");

    /// <inheritdoc />
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : m_Value.ToString(format, Money.GetNumberFormatInfo(formatProvider));

    /// <summary>Gets an XML string representation of the amount.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Deserializes the amount from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized amount.
    /// </returns>
    [Pure]
    public static Amount FromJson(double json) => new(Cast.ToDecimal<Amount>(json));

    /// <summary>Deserializes the amountfrom a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized amount.
    /// </returns>
    [Pure]
    public static Amount FromJson(long json) => new(json);

    /// <summary>Casts a decimal to an amount.</summary>
    public static explicit operator Amount(decimal val) => new(val);

    /// <summary>Casts a decimal to an amount.</summary>
    public static explicit operator Amount(double val) => Create(val);

    /// <summary>Casts a long to an amount.</summary>
    public static explicit operator Amount(long val) => new(val);

    /// <summary>Casts a int to an amount.</summary>
    public static explicit operator Amount(int val) => new(val);

    /// <summary>Casts an Amount to a decimal.</summary>
    public static explicit operator decimal(Amount val) => val.m_Value;

    /// <summary>Casts an Amount to a double.</summary>
    public static explicit operator double(Amount val) => (double)val.m_Value;

    /// <summary>Casts an Amount to a long.</summary>
    public static explicit operator long(Amount val) => (long)val.m_Value;

    /// <summary>Casts an Amount to an int.</summary>
    public static explicit operator int(Amount val) => (int)val.m_Value;

    /// <summary>Converts the string to an amount.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing an Amount to convert.
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
    public static bool TryParse(string? s, IFormatProvider? provider, out Amount result)
        => TryParse(s, NumberStyles.Currency, provider, out result);

    /// <summary>Converts the string to an amount.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing an Amount to convert.
    /// </param>
    /// <param name="style">
    /// The preferred number style.
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
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out Amount result)
    {
        Guard(style);

        return style.HasFlag(NumberStyles.AllowCurrencySymbol)
            ? ParseMoney(s, provider, out result)
            : ParseAmount(s, style, provider, out result);

        static bool ParseMoney(string? s, IFormatProvider? provider, out Amount result)
        {
            if (Money.TryParse(s, provider, out Money money))
            {
                result = money.Amount;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
        static bool ParseAmount(string? s, NumberStyles style, IFormatProvider? provider, out Amount result)
        {
            if (decimal.TryParse(s, style, provider, out decimal amount))
            {
                result = new(amount);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static void Guard(NumberStyles style)
        {
            var extra = style & ~NumberStyles.Currency;
            if (extra != NumberStyles.None)
            {
                throw new ArgumentOutOfRangeException(nameof(style), string.Format(QowaivMessages.ArgumentOutOfRange_NumberStyleNotSupported, extra));
            }
        }
    }

    /// <summary>Creates an Amount from a Decimal.</summary >
    /// <param name="val" >
    /// A decimal describing an Amount.
    /// </param >
    [Pure]
    public static Amount Create(decimal val) => new(val);

    /// <summary>Creates an Amount from a Double.</summary >
    /// <param name="val" >
    /// A decimal describing an Amount.
    /// </param >
    [Pure]
    public static Amount Create(double val) => Create(Cast.ToDecimal<Amount>(val));
}
