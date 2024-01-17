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
    , IIncrementOperators<Amount>, IDecrementOperators<Amount>
    , IUnaryPlusOperators<Amount, Amount>, IUnaryNegationOperators<Amount, Amount>
    , IAdditionOperators<Amount, Amount, Amount>, ISubtractionOperators<Amount, Amount, Amount>
    , IAdditionOperators<Amount, Percentage, Amount>, ISubtractionOperators<Amount, Percentage, Amount>
    , IMultiplyOperators<Amount, Percentage, Amount>, IDivisionOperators<Amount, Percentage, Amount>
    , IMultiplyOperators<Amount, decimal, Amount>, IDivisionOperators<Amount, decimal, Amount>
    , IMultiplyOperators<Amount, double, Amount>, IDivisionOperators<Amount, double, Amount>
    , IMultiplyOperators<Amount, long, Amount>, IDivisionOperators<Amount, long, Amount>
    , IMultiplyOperators<Amount, int, Amount>, IDivisionOperators<Amount, int, Amount>
    , IMultiplyOperators<Amount, short, Amount>, IDivisionOperators<Amount, short, Amount>
    , IMultiplyOperators<Amount, ulong, Amount>, IDivisionOperators<Amount, ulong, Amount>
    , IMultiplyOperators<Amount, uint, Amount>, IDivisionOperators<Amount, uint, Amount>
    , IMultiplyOperators<Amount, ushort, Amount>, IDivisionOperators<Amount, ushort, Amount>
#endif
{
    /// <summary>Represents an Amount of zero.</summary>
    public static readonly Amount Zero;

    /// <summary>Represents the smallest possible value of the amount.</summary>
    public static readonly Amount MinValue = new(decimal.MinValue);

    /// <summary>Represents the biggest possible value of the amount.</summary>
    public static readonly Amount MaxValue = new(decimal.MaxValue);

    /// <summary>Gets the sign of the value of the amount.</summary>
    [Pure]
    public int Sign() => m_Value.Sign();

    /// <summary>Returns the absolute value of the amount.</summary>
    [Pure]
    public Amount Abs() => (Amount)m_Value.Abs();

    /// <summary>Pluses the amount.</summary>
    [Pure]
    internal Amount Plus() => (Amount)(+m_Value);

    /// <summary>Negates the amount.</summary>
    [Pure]
    internal Amount Negate() => (Amount)(-m_Value);

    /// <summary>Increases the amount with one.</summary>
    [Pure]
    internal Amount Increment() => (Amount)(m_Value + 1);

    /// <summary>Decreases the amount with one.</summary>
    [Pure]
    internal Amount Decrement() => (Amount)(m_Value - 1);

    /// <summary>Decreases the amount with one.</summary>
    /// <summary>Adds a amount to the current amount.</summary>
    /// <param name="amount">
    /// The amount to add.
    /// </param>
    [Pure]
    public Amount Add(Amount amount) => (Amount)(m_Value + amount.m_Value);

    /// <summary>Adds the specified percentage to the amount.</summary>
    /// <param name="p">
    /// The percentage to add.
    /// </param>
    [Pure]
    public Amount Add(Percentage p) => (Amount)m_Value.Add(p);

    /// <summary>Subtracts a amount from the current amount.</summary>
    /// <param name="amount">
    /// The amount to Subtract.
    /// </param>
    [Pure]
    public Amount Subtract(Amount amount) => (Amount)(m_Value - amount.m_Value);

    /// <summary>AddsSubtract the specified percentage from the amount.</summary>
    /// <param name="p">
    /// The percentage to add.
    /// </param>
    [Pure]
    public Amount Subtract(Percentage p) => (Amount)m_Value.Subtract(p);

    /// <summary>Gets a percentage of the current amount.</summary>
    /// <param name="p">
    /// The percentage to get.
    /// </param>
    [Pure]
    public Amount Multiply(Percentage p) => (Amount)(m_Value * p);

    /// <summary>Multiplies the amount with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Multiply(decimal factor) => (Amount)(m_Value * factor);

    /// <summary>Multiplies the amount with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Multiply(double factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the amount with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Multiply(float factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the amount with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Multiply(long factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the amount with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Multiply(int factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the amount with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Multiply(short factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the amount with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Amount Multiply(ulong factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the amount with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Amount Multiply(uint factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the amount with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Amount Multiply(ushort factor) => Multiply((decimal)factor);

    /// <summary>Divides the amount by a specified amount.</summary>
    /// <param name="p">
    /// The amount to divides to..
    /// </param>
    [Pure]
    public Amount Divide(Percentage p) => (Amount)(m_Value / p);

    /// <summary>Divides the amount by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Divide(decimal factor) => (Amount)(m_Value / factor);

    /// <summary>Divides the amount by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Divide(double factor) => Divide((decimal)factor);

    /// <summary>Divides the amount by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Divide(float factor) => Divide((decimal)factor);

    /// <summary>Divides the amount by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Divide(long factor) => Divide((decimal)factor);

    /// <summary>Divides the amount by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Divide(int factor) => Divide((decimal)factor);

    /// <summary>Divides the amount by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Amount Divide(short factor) => Divide((decimal)factor);

    /// <summary>Divides the amount by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Amount Divide(ulong factor) => Divide((decimal)factor);

    /// <summary>Divides the amount by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Amount Divide(uint factor) => Divide((decimal)factor);

    /// <summary>Divides the amount by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Amount Divide(ushort factor) => Divide((decimal)factor);

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

    /// <summary>Unitary plusses the amount.</summary>
    public static Amount operator +(Amount amount) => amount.Plus();

    /// <summary>Negates the amount.</summary>
    public static Amount operator -(Amount amount) => amount.Negate();

    /// <summary>Increases the amount with one.</summary>
    public static Amount operator ++(Amount amount) => amount.Increment();

    /// <summary>Decreases the amount with one.</summary>
    public static Amount operator --(Amount amount) => amount.Decrement();

    /// <summary>Adds the left and the right amount.</summary>
    public static Amount operator +(Amount l, Amount r) => l.Add(r);

    /// <summary>Adds the percentage to the amount.</summary>
    public static Amount operator +(Amount amount, Percentage p) => amount.Add(p);

    /// <summary>Subtracts the right from the left amount.</summary>
    public static Amount operator -(Amount l, Amount r) => l.Subtract(r);

    /// <summary>Subtracts the percentage from the amount.</summary>
    public static Amount operator -(Amount amount, Percentage p) => amount.Subtract(p);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, Percentage factor) => amount.Multiply(factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, decimal factor) => amount.Multiply(factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, double factor) => amount.Multiply(factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, float factor) => amount.Multiply(factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, long factor) => amount.Multiply(factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, int factor) => amount.Multiply(factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, short factor) => amount.Multiply(factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator *(Amount amount, ulong factor) => amount.Multiply(factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator *(Amount amount, uint factor) => amount.Multiply(factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator *(Amount amount, ushort factor) => amount.Multiply(factor);

    /// <summary>Divides the amount by an other amount.</summary>
    public static decimal operator /(Amount numerator, Amount denominator) => numerator.m_Value / denominator.m_Value;

    /// <summary>Divides the amount by the percentage.</summary>
    public static Amount operator /(Amount amount, Percentage p) => amount.Divide(p);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, decimal factor) => amount.Divide(factor);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, double factor) => amount.Divide(factor);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, float factor) => amount.Divide(factor);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, long factor) => amount.Divide(factor);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, int factor) => amount.Divide(factor);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, short factor) => amount.Divide(factor);

    /// <summary>Divides the amount by the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator /(Amount amount, ulong factor) => amount.Divide(factor);

    /// <summary>Divides the amount by the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator /(Amount amount, uint factor) => amount.Divide(factor);

    /// <summary>Divides the amount by the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator /(Amount amount, ushort factor) => amount.Divide(factor);

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
    public double ToJson() => m_Value == decimal.Zero ? 0 : (double)m_Value;

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
    public static explicit operator Amount(decimal val) => Create(val);

    /// <summary>Casts a decimal to an amount.</summary>
    public static explicit operator Amount(double val) => Create(val);

    /// <summary>Casts a long to an amount.</summary>
    public static explicit operator Amount(long val) => Create((decimal)val);

    /// <summary>Casts a int to an amount.</summary>
    public static explicit operator Amount(int val) => Create((decimal)val);

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
    {
        result = default;
        if (Money.TryParse(s, provider, out Money money))
        {
            result = (Amount)(decimal)money;
            return true;
        }
        return false;
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
