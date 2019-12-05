#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Financial;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.Financial
{
    /// <summary>Represents an </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(decimal))]
    [OpenApiDataType(description: "Decimal representation of a currency amount.", type: "number", format: "amount")]
    [TypeConverter(typeof(AmountTypeConverter))]
    public partial struct Amount : ISerializable, IXmlSerializable, IJsonSerializable, IFormattable, IEquatable<Amount>, IComparable, IComparable<Amount>
    {
        /// <summary>Represents an Amount of zero.</summary>
        public static readonly Amount Zero;
        /// <summary>Represents the smallest possible value of an </summary>
        public static readonly Amount MinValue = new Amount(decimal.MinValue);
        /// <summary>Represents the biggest possible value of an </summary>
        public static readonly Amount MaxValue = new Amount(decimal.MaxValue);

        /// <summary>Returns the absolute value of the amount.</summary>
        public Amount Abs() => (Amount)Math.Abs(m_Value);

        /// <summary>Pluses the amount.</summary>
        internal Amount Plus() => (Amount)(+m_Value);

        /// <summary>Negates the amount.</summary>

        internal Amount Negate() => (Amount)(-m_Value);

        /// <summary>Increases the amount with one.</summary>
        internal Amount Increment() => (Amount)(m_Value + 1);

        /// <summary>Decreases the amount with one.</summary>
        internal Amount Decrement() => (Amount)(m_Value - 1);

        /// <summary>Decreases the amount with one.</summary>
        /// <summary>Adds a amount to the current amount.</summary>
        /// <param name="amount">
        /// The amount to add.
        /// </param>
        public Amount Add(Amount amount) => (Amount)(m_Value + amount.m_Value);

        /// <summary>Adds the specified percentage to the amount.</summary>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public Amount Add(Percentage p) => (Amount)(m_Value.Add(p));

        /// <summary>Subtracts a amount from the current amount.</summary>
        /// <param name="amount">
        /// The amount to Subtract.
        /// </param>
        public Amount Subtract(Amount amount) => (Amount)(m_Value - amount.m_Value);

        /// <summary>AddsSubtract the specified percentage from the amount.</summary>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public Amount Subtract(Percentage p) => (Amount)(m_Value.Subtract(p));

        /// <summary>Gets a percentage of the current amount.</summary>
        /// <param name="p">
        /// The percentage to get.
        /// </param>
        public Amount Multiply(Percentage p) => (Amount)(m_Value * p);

        /// <summary>Multiplies the amount with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Multiply(decimal factor) => (Amount)(m_Value * factor);

        /// <summary>Multiplies the amount with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Multiply(double factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the amount with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Multiply(float factor) => Multiply((decimal)factor);


        /// <summary>Multiplies the amount with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Multiply(long factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the amount with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Multiply(int factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the amount with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Multiply(short factor) => Multiply((decimal)factor);


        /// <summary>Multiplies the amount with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Amount Multiply(ulong factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the amount with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Amount Multiply(uint factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the amount with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Amount Multiply(ushort factor) => Multiply((decimal)factor);


        /// <summary>Divides the amount by a specified amount.</summary>
        /// <param name="p">
        /// The amount to divides to..
        /// </param>
        public Amount Divide(Percentage p) => (Amount)(m_Value / p);

        /// <summary>Divides the amount by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Divide(decimal factor) => (Amount)(m_Value / factor);

        /// <summary>Divides the amount by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Divide(double factor) => Divide((decimal)factor);

        /// <summary>Divides the amount by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Divide(float factor) => Divide((decimal)factor);


        /// <summary>Divides the amount by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Divide(long factor) => Divide((decimal)factor);

        /// <summary>Divides the amount by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Divide(int factor) => Divide((decimal)factor);

        /// <summary>Divides the amount by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Amount Divide(short factor) => Divide((decimal)factor);


        /// <summary>Divides the amount by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Amount Divide(ulong factor) => Divide((decimal)factor);

        /// <summary>Divides the amount by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Amount Divide(uint factor) => Divide((decimal)factor);

        /// <summary>Divides the amount by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Amount Divide(ushort factor) => Divide((decimal)factor);


        /// <summary>Rounds the amount value to the 0 decimal places</summary>
        public Amount Round() => Round(0);

        /// <summary>Rounds the amount value to a specified number of decimal places.</summary>
        /// <param name="decimals">
        /// A value from -28 to 28 that specifies the number of decimal places to round to.
        /// </param>
        /// <remarks>
        /// A negative value for <paramref name="decimals"/> lowers precision to tenfold, hundredfold, and bigger.
        /// </remarks>
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
        public Amount Round(int decimals, DecimalRounding mode) => (Amount)m_Value.Round(decimals, mode);

        /// <summary>Rounds the amount value to the closed number that is a multiple of the specified factor.</summary>
        /// <param name="multipleOf">
        /// The factor of which the number should be multiple of.
        /// </param>
        public Amount RoundToMultiple(decimal multipleOf) => RoundToMultiple(multipleOf, DecimalRounding.BankersRound);

        /// <summary>Rounds the amount value to the closed number that is a multiple of the specified factor.</summary>
        /// <param name="multipleOf">
        /// The factor of which the number should be multiple of.
        /// </param>
        /// <param name="mode">
        /// The rounding method used to determine the closed by number.
        /// </param>
        public Amount RoundToMultiple(decimal multipleOf, DecimalRounding mode) => (Amount)m_Value.RoundToMultiple(multipleOf, mode);


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

        private void FromJson(object json) => m_Value = Parse(Parsing.ToInvariant(json), CultureInfo.InvariantCulture).m_Value;

        /// <inheritdoc />
        object IJsonSerializable.ToJson() => m_Value;

        /// <summary>Returns a <see cref="string"/> that represents the current Amount for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => string.Format(CultureInfo.InvariantCulture, "¤{0:0.00########}", m_Value);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current </summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }
            var info = Money.GetNumberFormatInfo(formatProvider);
            return m_Value.ToString(format, info);
        }

        /// <summary>Gets an XML string representation of the amount.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Casts an Amount to a <see cref="string"/>.</summary>
        public static explicit operator string(Amount val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a </summary>
        public static explicit operator Amount(string str) => Parse(str, CultureInfo.CurrentCulture);

        /// <summary>Casts a decimal an </summary>
        public static explicit operator Amount(decimal val) => Create(val);
        /// <summary>Casts a decimal an </summary>
        public static explicit operator Amount(double val) => Create(val);
        /// <summary>Casts a long an </summary>
        public static explicit operator Amount(long val) => Create((decimal)val);
        /// <summary>Casts a int an </summary>
        public static explicit operator Amount(int val) => Create((decimal)val);

        /// <summary>Casts an Amount to a decimal.</summary>
        public static explicit operator decimal(Amount val) => val.m_Value;
        /// <summary>Casts an Amount to a double.</summary>
        public static explicit operator double(Amount val) => (double)val.m_Value;
        /// <summary>Casts an Amount to a long.</summary>
        public static explicit operator long(Amount val) => (long)val.m_Value;
        /// <summary>Casts an Amount to a int.</summary>
        public static explicit operator int(Amount val) => (int)val.m_Value;

        /// <summary>Converts the string to an 
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing an Amount to convert.
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
        public static bool TryParse(string s, IFormatProvider formatProvider, out Amount result)
        {
            result = default;
            if (Money.TryParse(s, formatProvider, out Money money))
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
        public static Amount Create(decimal val) => new Amount(val);

        /// <summary>Creates an Amount from a Double.</summary >
        /// <param name="val" >
        /// A decimal describing an Amount.
        /// </param >
        public static Amount Create(double val) => Create((decimal)val);
    }
}
