using Qowaiv.Conversion.Financial;

namespace Qowaiv.Financial;

/// <summary>Represents money (amount and currency).</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.Continuous, typeof(decimal))]
[OpenApiDataType(description: "Combined currency and amount notation as defined by ISO 4217.", example: "EUR12.47", type: "string", format: "money", pattern: @"[A-Z]{3} -?[0-9]+(\.[0-9]+)?")]
[TypeConverter(typeof(MoneyTypeConverter))]
public partial struct Money : ISerializable, IXmlSerializable, IFormattable, IEquatable<Money>, IComparable, IComparable<Money>
{
    /// <summary>Represents an Amount of zero.</summary>
    public static readonly Money Zero;
    /// <summary>Represents the smallest possible value of an </summary>
    public static readonly Money MinValue = decimal.MinValue + Currency.Empty;
    /// <summary>Represents the biggest possible value of an </summary>
    public static readonly Money MaxValue = decimal.MaxValue + Currency.Empty;

    /// <summary>The inner value of the </summary>
    private decimal m_Value;
    private Currency m_Currency;

    /// <summary>Gets the amount of the money.</summary>
    public Amount Amount => (Amount)m_Value;

    /// <summary>Gets the currency of the money.</summary>
    public Currency Currency => m_Currency;


    /// <summary>Gets the sign of the value of the money.</summary>
    [Pure]
    public int Sign() => m_Value.Sign();

    /// <summary>Returns the absolute value of the money.</summary>
    [Pure]
    public Money Abs() => m_Value.Abs() + Currency;

    /// <summary>Pluses the money.</summary>
    [Pure]
    internal Money Plus() => +m_Value + Currency;

    /// <summary>Negates the money.</summary>
    [Pure]
    internal Money Negate() => -m_Value + Currency;

    /// <summary>Increases the money with one (of the current currency).</summary>
    [Pure]
    internal Money Increment() => (m_Value + 1) + Currency;

    /// <summary>Decreases the money with one (of the current currency).</summary>
    [Pure]
    internal Money Decrement() => (m_Value - 1) + Currency;

    /// <summary>Decreases the amount with one.</summary>
    /// <summary>Adds a amount to the current amount.</summary>
    /// <param name="money">
    /// The money to add.
    /// </param>
    [Pure]
    public Money Add(Money money) => (m_Value + money.m_Value) + HaveSameCurrency(this, money, "addition");

    /// <summary>Adds the specified percentage to the amount.</summary>
    /// <param name="p">
    /// The percentage to add.
    /// </param>
    [Pure]
    public Money Add(Percentage p) => m_Value.Add(p) + Currency;

    /// <summary>Subtracts a amount from the current amount.</summary>
    /// <param name="money">
    /// The money to Subtract.
    /// </param>
    [Pure]
    public Money Subtract(Money money) => (m_Value - money.m_Value) + HaveSameCurrency(this, money, "subtraction");

    /// <summary>AddsSubtract the specified percentage from the amount.</summary>
    /// <param name="p">
    /// The percentage to add.
    /// </param>
    [Pure]
    public Money Subtract(Percentage p) => m_Value.Subtract(p) + Currency;

    /// <summary>Gets a percentage of the money.</summary>
    /// <param name="p">
    /// The percentage to get.
    /// </param>
    [Pure]
    public Money Multiply(Percentage p) => (m_Value * p) + Currency;

    /// <summary>Multiplies the money with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Multiply(decimal factor) => (m_Value * factor) + Currency;

    /// <summary>Multiplies the money with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Multiply(double factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the money with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Multiply(float factor) => Multiply((decimal)factor);


    /// <summary>Multiplies the money with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Multiply(long factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the money with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Multiply(int factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the money with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Multiply(short factor) => Multiply((decimal)factor);


    /// <summary>Multiplies the money with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Money Multiply(ulong factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the money with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Money Multiply(uint factor) => Multiply((decimal)factor);

    /// <summary>Multiplies the money with a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Money Multiply(ushort factor) => Multiply((decimal)factor);

    /// <summary>Divides the money by a specified money.</summary>
    /// <param name="p">
    /// The money to devides to..
    /// </param>
    [Pure]
    public Money Divide(Percentage p) => (m_Value / p) + Currency;

    /// <summary>Divides the money by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Divide(decimal factor) => (m_Value / factor) + Currency;

    /// <summary>Divides the money by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Divide(double factor) => Divide((decimal)factor);

    /// <summary>Divides the money by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Divide(float factor) => Divide((decimal)factor);

    /// <summary>Divides the money by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Divide(long factor) => Divide((decimal)factor);

    /// <summary>Divides the money by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Divide(int factor) => Divide((decimal)factor);

    /// <summary>Divides the money by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    public Money Divide(short factor) => Divide((decimal)factor);


    /// <summary>Divides the money by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Money Divide(ulong factor) => Divide((decimal)factor);

    /// <summary>Divides the money by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Money Divide(uint factor) => Divide((decimal)factor);

    /// <summary>Divides the money by a specified factor.
    /// </summary>
    /// <param name="factor">
    /// The factor to multiply with.
    /// </param>
    [CLSCompliant(false)]
    [Pure]
    public Money Divide(ushort factor) => Divide((decimal)factor);

    /// <summary>Divides the money by a specified factor.
    /// </summary>
    /// <param name="denominator">
    /// The factor to multiply with.
    /// </param>
    [Pure]
    private decimal Divide(Money denominator)
    {
        _ = HaveSameCurrency(this, denominator, "division");
        return m_Value / denominator.m_Value;
    }

    /// <summary>Rounds the money value to the preferred number decimal places, based on its currency.</summary>
    [Pure]
    public Money Round() => Round(Currency.Digits);

    /// <summary>Rounds the money value to a specified number of decimal places.</summary>
    /// <param name="decimals">
    /// A value from -28 to 28 that specifies the number of decimal places to round to.
    /// </param>
    /// <remarks>
    /// A negative value for <paramref name="decimals"/> lowers precision to tenfold, hundredfold, and bigger.
    /// </remarks>
    [Pure]
    public Money Round(int decimals) => Round(decimals, DecimalRounding.BankersRound);

    /// <summary>Rounds the money value to a specified number of decimal places.</summary>
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
    public Money Round(int decimals, DecimalRounding mode) => m_Value.Round(decimals, mode) + Currency;

    /// <summary>Rounds the money value to the closed number that is a multiple of the specified factor.</summary>
    /// <param name="multipleOf">
    /// The factor of which the number should be multiple of.
    /// </param>
    [Pure]
    public Money RoundToMultiple(decimal multipleOf) => RoundToMultiple(multipleOf, DecimalRounding.BankersRound);

    /// <summary>Rounds the money value to the closed number that is a multiple of the specified factor.</summary>
    /// <param name="multipleOf">
    /// The factor of which the number should be multiple of.
    /// </param>
    /// <param name="mode">
    /// The rounding method used to determine the closed by number.
    /// </param>
    [Pure]
    public Money RoundToMultiple(decimal multipleOf, DecimalRounding mode) => m_Value.RoundToMultiple(multipleOf, mode) + Currency;

    /// <summary>Increases the money with one (of the current currency).</summary>
    public static Money operator ++(Money money) => money.Increment();
    /// <summary>Decreases the money with one (of the current currency).</summary>
    public static Money operator --(Money money) => money.Decrement();

    /// <summary>Unitary plusses the money.</summary>
    public static Money operator +(Money money) => money.Plus();
    /// <summary>Negates the money.</summary>
    public static Money operator -(Money money) => money.Negate();

    /// <summary>Adds money.</summary>
    /// <param name="l">The left operand.</param>
    /// <param name="r">The right operand</param>
    public static Money operator +(Money l, Money r) => l.Add(r);

    /// <summary>Adds the percentage to the money.</summary>
    public static Money operator +(Money money, Percentage p) => (money.m_Value + p) + money.Currency;

    /// <summary>Adds money.</summary>
    /// <param name="l">The left operand.</param>
    /// <param name="r">The right operand</param>
    public static Money operator -(Money l, Money r) => l.Subtract(r);

    /// <summary>Subtracts the percentage from the money.</summary>
    public static Money operator -(Money money, Percentage p) => (money.m_Value - p) + money.Currency;

    /// <summary>Multiplies the money with the factor.</summary>
    public static Money operator *(Money money, Percentage factor) => money.Multiply(factor);

    /// <summary>Multiplies the money with the factor.</summary>
    public static Money operator *(Money money, decimal factor) => money.Multiply(factor);
    /// <summary>Multiplies the money with the factor.</summary>
    public static Money operator *(Money money, double factor) => money.Multiply(factor);
    /// <summary>Multiplies the money with the factor.</summary>
    public static Money operator *(Money money, float factor) => money.Multiply(factor);

    /// <summary>Multiplies the money with the factor.</summary>
    public static Money operator *(Money money, long factor) => money.Multiply(factor);
    /// <summary>Multiplies the money with the factor.</summary>
    public static Money operator *(Money money, int factor) => money.Multiply(factor);
    /// <summary>Multiplies the money with the factor.</summary>
    public static Money operator *(Money money, short factor) => money.Multiply(factor);

    /// <summary>Multiplies the money with the factor.</summary>
    [CLSCompliant(false)]
    public static Money operator *(Money money, ulong factor) => money.Multiply(factor);
    /// <summary>Multiplies the money with the factor.</summary>
    [CLSCompliant(false)]
    public static Money operator *(Money money, uint factor) => money.Multiply(factor);
    /// <summary>Multiplies the money with the factor.</summary>
    [CLSCompliant(false)]
    public static Money operator *(Money money, ushort factor) => money.Multiply(factor);


    /// <summary>Divides the money by the percentage.</summary>
    public static Money operator /(Money money, Percentage p) => money.Divide(p);
    /// <summary>Divides the money by the factor.</summary>
    public static Money operator /(Money money, decimal factor) => money.Divide(factor);
    /// <summary>Divides the money by the factor.</summary>
    public static Money operator /(Money money, double factor) => money.Divide(factor);
    /// <summary>Divides the money by the factor.</summary>
    public static Money operator /(Money money, float factor) => money.Divide(factor);

    /// <summary>Divides the money by the factor.</summary>
    public static Money operator /(Money money, long factor) => money.Divide(factor);
    /// <summary>Divides the money by the factor.</summary>
    public static Money operator /(Money money, int factor) => money.Divide(factor);
    /// <summary>Divides the money by the factor.</summary>
    public static Money operator /(Money money, short factor) => money.Divide(factor);

    /// <summary>Divides the money by the factor.</summary>
    [CLSCompliant(false)]
    public static Money operator /(Money money, ulong factor) => money.Divide(factor);
    /// <summary>Divides the money by the factor.</summary>
    [CLSCompliant(false)]
    public static Money operator /(Money money, uint factor) => money.Divide(factor);
    /// <summary>Divides the money by the factor.</summary>
    [CLSCompliant(false)]
    public static Money operator /(Money money, ushort factor) => money.Divide(factor);

    /// <summary>Divides  money by money to get there ratio.</summary>
    public static decimal operator /(Money numerator, Money denominator) => numerator.Divide(denominator);

    [DebuggerStepThrough]
    [Pure]
    private static Currency HaveSameCurrency(Money l, Money r, string operation)
        => l.Currency == r.Currency
        ? l.Currency
        : throw new CurrencyMismatchException(l.Currency, r.Currency, operation);

    /// <summary>Initializes a new instance of Money based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private Money(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info, nameof(info));
        m_Value = info.GetDecimal("Value");
        m_Currency = Currency.Parse(info.GetString(nameof(Currency)), CultureInfo.InvariantCulture);
    }

    /// <summary>Adds the underlying property of Money to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info, nameof(info));
        info.AddValue("Value", m_Value);
        info.AddValue(nameof(Currency), m_Currency.Name);
    }

    /// <remarks>Sets the currency.</remarks>
    private void OnReadXml(Money value)
    {
        m_Value = value.m_Value;
        m_Currency = value.m_Currency;
    }

    /// <summary>Deserializes the money from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized money.
    /// </returns>
    [Pure]
    public static Money FromJson(long json) => (decimal)json + Currency.Empty;

    /// <summary>Deserializes the money from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized money.
    /// </returns>
    [Pure]
    public static Money FromJson(double json) => json + Currency.Empty;

    /// <summary>Serializes the money to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string ToJson() => Currency.Name + m_Value.ToString("", CultureInfo.InvariantCulture);

    /// <summary>Returns a <see cref="string"/> that represents the current Money for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0}");

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
        if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
        {
            return formatted;
        }
        else
        {
            var numberFormatInfo = Currency.GetNumberFormatInfo(formatProvider);
            return m_Value.ToString(string.IsNullOrEmpty(format) ? "C" : format, numberFormatInfo);
        }
    }

    /// <summary>Gets an XML string representation of the money.</summary>
    [Pure]
    private string ToXmlString() => Currency.Name + m_Value.ToString("", CultureInfo.InvariantCulture);

    /// <summary>Returns true if this instance and the other <see cref="Money"/> are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="Money"/> to compare with.</param>
    [Pure]
    public bool Equals(Money other) => m_Value == other.m_Value && m_Currency == other.m_Currency;

    /// <summary>Returns the hash code for this </summary>
    /// <returns>
    /// A 32-bit signed integer hash code.
    /// </returns>
    [Pure]
    public override int GetHashCode() => m_Value.GetHashCode() ^ m_Currency.GetHashCode();

    /// <summary>Compares this instance with a specified Money and indicates
    /// whether this instance precedes, follows, or appears in the same position
    /// in the sort order as the specified 
    /// </summary>
    /// <param name="other">
    /// The Money to compare with this instance.
    /// </param>
    /// <returns>
    /// A 32-bit signed integer that indicates whether this instance precedes, follows,
    /// or appears in the same position in the sort order as the value parameter.
    /// </returns>
    [Pure]
    public int CompareTo(Money other)
        => m_Currency.CompareTo(other.m_Currency) is var compare && compare == 0
        ? m_Value.CompareTo(other.m_Value)
        : compare;

    #region (Explicit) casting

    /// <summary>Casts Money to a decimal.</summary>
    public static explicit operator Amount(Money val) => (Amount)val.m_Value;

    /// <summary>Casts Money to a decimal.</summary>
    public static explicit operator decimal(Money val) => val.m_Value;

    /// <summary>Casts Money to a double.</summary>
    public static explicit operator double(Money val) => (double)val.m_Value;

    #endregion

    /// <summary>Converts the string to 
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing Money to convert.
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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out Money result)
    {
        result = default;

        var currency = Currency.Empty;
        var signs = formatProvider.NegativeSign() + formatProvider.PositiveSign();
        var span = s.CharSpan().TrimLeft(ch => CandidateCurrency(ch, signs), out var candidate);
        
        if (candidate.IsEmpty())
        {
            span = span.TrimRight(ch => CandidateCurrency(ch, signs), out candidate);
        }

        if ((candidate.IsEmpty() || Currency.TryParse(candidate.ToString(), out currency))
            && decimal.TryParse(span.ToString(), NumberStyles.Currency, formatProvider, out var amount))
        {
            result = amount + currency;
            return true;
        }
        else return false;
    }

    [Pure]
    private static bool CandidateCurrency(char ch, string forbidden)
        => !char.IsDigit(ch)
        && !char.IsWhiteSpace(ch)
        && !forbidden.Contains(ch);

    /// <summary >Creates Money from a Decimal. </summary >
    /// <param name="val" >
    /// The amount.
    /// </param>
    [Pure]
    public static Money Create(decimal val) => Create(val, Currency.Current);

    /// <summary >Creates Money from a Decimal. </summary >
    /// <param name="val" >
    /// A decimal describing the amount.
    /// </param >
    /// <param name="currency">
    /// The currency of the amount.
    /// </param>
    [Pure]
    public static Money Create(decimal val, Currency currency) => new() { m_Value = val, m_Currency = currency };

    /// <summary>Gets a <see cref="NumberFormatInfo"/> based on the <see cref="IFormatProvider"/>.</summary>
    /// <remarks>
    /// Because the options for formatting and parsing currencies as provided 
    /// by the .NET framework are not sufficient, internally we use number
    /// settings. For parsing and formatting however we like to use the
    /// currency properties of the <see cref="NumberFormatInfo"/> instead of
    /// the number properties, so we copy them for desired behavior.
    /// </remarks>
    [Pure]
    internal static NumberFormatInfo GetNumberFormatInfo(IFormatProvider? formatProvider)
    {
        var info = NumberFormatInfo.GetInstance(formatProvider);
        info = (NumberFormatInfo)info.Clone();
        info.NumberDecimalDigits = info.CurrencyDecimalDigits;
        info.NumberDecimalSeparator = info.CurrencyDecimalSeparator;
        info.NumberGroupSeparator = info.CurrencyGroupSeparator;
        info.NumberGroupSizes = info.CurrencyGroupSizes;
        return info;
    }
}
