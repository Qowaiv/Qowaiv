#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion;
using Qowaiv.Formatting;
using Qowaiv.Json;
using Qowaiv.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv
{
    /// <summary>Represents a Percentage.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, SingleValueObject(SingleValueStaticOptions.All ^ SingleValueStaticOptions.HasEmptyValue ^ SingleValueStaticOptions.HasUnknownValue, typeof(decimal))]
    [OpenApiDataType(description: "Ratio expressed as a fraction of 100 denoted using the percent sign '%', for example 13.76%.", type: "string", format: "percentage", pattern: @"-?[0-9]+(\.[0-9])?%")]
    [TypeConverter(typeof(PercentageTypeConverter))]
    public partial struct Percentage : ISerializable, IXmlSerializable, IFormattable, IEquatable<Percentage>, IComparable, IComparable<Percentage>
    {
        /// <summary>The percentage mark (%).</summary>
        public static readonly string PercentageMark = "%";
        /// <summary>The per mille mark (‰).</summary>
        public static readonly string PerMilleMark = "‰";
        /// <summary>The per ten thousand mark (0/000).</summary>
        public static readonly string PerTenThousandMark = "‱";

        /// <summary>Represents 0 percent.</summary>
        public static readonly Percentage Zero;
        /// <summary>Represents 1 percent.</summary>
        public static readonly Percentage One = 0.01m;
        /// <summary>Represents 100 percent.</summary>
        public static readonly Percentage Hundred = 1m;

        /// <summary>Gets the minimum value of a percentage.</summary>
        public static readonly Percentage MinValue = decimal.MinValue;

        /// <summary>Gets the maximum value of a percentage.</summary>
        public static readonly Percentage MaxValue = decimal.MaxValue;

        #region Percentage manipulation

        /// <summary>Gets the sign of the percentage.</summary>
        public int Sign() => m_Value.Sign();

        /// <summary>Returns the absolute value of the percentage.</summary>
        public Percentage Abs() => Math.Abs(m_Value);

        /// <summary>Increases the percentage with one percent.</summary>
        internal Percentage Increment() => Add(One);

        /// <summary>Decreases the percentage with one percent.</summary>
        internal Percentage Decrement() => Subtract(One);

        /// <summary>Pluses the percentage.</summary>
        internal Percentage Plus() => +m_Value;

        /// <summary>Negates the percentage.</summary>
        internal Percentage Negate() => -m_Value;

        /// <summary>Gets a percentage of the current percentage.</summary>
        /// <param name="p">
        /// The percentage to get.
        /// </param>
        public Percentage Multiply(Percentage p) => m_Value * p.m_Value;

        /// <summary>Divides the current percentage by a specified percentage.</summary>
        /// <param name="p">
        /// The percentage to devides to..
        /// </param>
        public Percentage Divide(Percentage p) => m_Value / p.m_Value;

        /// <summary>Adds a percentage to the current percentage.
        /// </summary>
        /// <param name="p">
        /// The percentage to add.
        /// </param>
        public Percentage Add(Percentage p) => m_Value + p.m_Value;

        /// <summary>Subtracts a percentage from the current percentage.
        /// </summary>
        /// <param name="p">
        /// The percentage to Subtract.
        /// </param>
        public Percentage Subtract(Percentage p) => m_Value - p.m_Value;

        #region Multiply

        /// <summary>Multiplies the percentage with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Multiply(decimal factor) => m_Value * factor;

        /// <summary>Multiplies the percentage with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Multiply(double factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the percentage with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Multiply(float factor) => Multiply((decimal)factor);


        /// <summary>Multiplies the percentage with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Multiply(long factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the percentage with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Multiply(int factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the percentage with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Multiply(short factor) => Multiply((decimal)factor);


        /// <summary>Multiplies the percentage with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Percentage Multiply(ulong factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the percentage with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Percentage Multiply(uint factor) => Multiply((decimal)factor);

        /// <summary>Multiplies the percentage with a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Percentage Multiply(ushort factor) => Multiply((decimal)factor);

        #endregion

        #region Divide

        /// <summary>Divide the percentage by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Divide(decimal factor) => m_Value / factor;

        /// <summary>Divide the percentage by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Divide(double factor) => Divide((decimal)factor);

        /// <summary>Divide the percentage by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Divide(float factor) => Divide((decimal)factor);


        /// <summary>Divide the percentage by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Divide(long factor) => Divide((decimal)factor);

        /// <summary>Divide the percentage by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Divide(int factor) => Divide((decimal)factor);

        /// <summary>Divide the percentage by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Percentage Divide(short factor) => Divide((decimal)factor);


        /// <summary>Divide the percentage by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Percentage Divide(ulong factor) => Divide((decimal)factor);

        /// <summary>Divide the percentage by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Percentage Divide(uint factor) => Divide((decimal)factor);

        /// <summary>Divide the percentage by a specified factor.
        /// </summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        [CLSCompliant(false)]
        public Percentage Divide(ushort factor) => Divide((decimal)factor);

        #endregion

        /// <summary>Increases the percentage with one percent.</summary>
        public static Percentage operator ++(Percentage p) => p.Increment();
        /// <summary>Decreases the percentage with one percent.</summary>
        public static Percentage operator --(Percentage p) => p.Decrement();

        /// <summary>Unitary plusses the percentage.</summary>
        public static Percentage operator +(Percentage p) => p.Plus();
        /// <summary>Negates the percentage.</summary>
        public static Percentage operator -(Percentage p) => p.Negate();

        /// <summary>Multiplies the left and the right percentage.</summary>
        public static Percentage operator *(Percentage l, Percentage r) => l.Multiply(r);
        /// <summary>Divides the left by the right percentage.</summary>
        public static Percentage operator /(Percentage l, Percentage r) => l.Divide(r);
        /// <summary>Adds the left and the right percentage.</summary>
        public static Percentage operator +(Percentage l, Percentage r) => l.Add(r);
        /// <summary>Subtracts the right from the left percentage.</summary>
        public static Percentage operator -(Percentage l, Percentage r) => l.Subtract(r);

        /// <summary>Multiplies the percentage with the factor.</summary>
        public static Percentage operator *(Percentage p, decimal factor) => p.Multiply(factor);
        /// <summary>Multiplies the percentage with the factor.</summary>
        public static Percentage operator *(Percentage p, double factor) => p.Multiply(factor);
        /// <summary>Multiplies the percentage with the factor.</summary>
        public static Percentage operator *(Percentage p, float factor) => p.Multiply(factor);

        /// <summary>Multiplies the percentage with the factor.</summary>
        public static Percentage operator *(Percentage p, long factor) => p.Multiply(factor);
        /// <summary>Multiplies the percentage with the factor.</summary>
        public static Percentage operator *(Percentage p, int factor) => p.Multiply(factor);
        /// <summary>Multiplies the percentage with the factor.</summary>
        public static Percentage operator *(Percentage p, short factor) => p.Multiply(factor);

        /// <summary>Multiplies the percentage with the factor.</summary>
        [CLSCompliant(false)]
        public static Percentage operator *(Percentage p, ulong factor) => p.Multiply(factor);
        /// <summary>Multiplies the percentage with the factor.</summary>
        [CLSCompliant(false)]
        public static Percentage operator *(Percentage p, uint factor) => p.Multiply(factor);
        /// <summary>Multiplies the percentage with the factor.</summary>
        [CLSCompliant(false)]
        public static Percentage operator *(Percentage p, ushort factor) => p.Multiply(factor);

        /// <summary>Divides the percentage by the factor.</summary>
        public static Percentage operator /(Percentage p, decimal factor) => p.Divide(factor);
        /// <summary>Divides the percentage by the factor.</summary>
        public static Percentage operator /(Percentage p, double factor) => p.Divide(factor);
        /// <summary>Divides the percentage by the factor.</summary>
        public static Percentage operator /(Percentage p, float factor) => p.Divide(factor);

        /// <summary>Divides the percentage by the factor.</summary>
        public static Percentage operator /(Percentage p, long factor) => p.Divide(factor);
        /// <summary>Divides the percentage by the factor.</summary>
        public static Percentage operator /(Percentage p, int factor) => p.Divide(factor);
        /// <summary>Divides the percentage by the factor.</summary>
        public static Percentage operator /(Percentage p, short factor) => p.Divide(factor);

        /// <summary>Divides the percentage by the factor.</summary>
        [CLSCompliant(false)]
        public static Percentage operator /(Percentage p, ulong factor) => p.Divide(factor);
        /// <summary>Divides the percentage by the factor.</summary>
        [CLSCompliant(false)]
        public static Percentage operator /(Percentage p, uint factor) => p.Divide(factor);
        /// <summary>Divides the percentage by the factor.</summary>
        [CLSCompliant(false)]
        public static Percentage operator /(Percentage p, ushort factor) => p.Divide(factor);

        #endregion

        #region Number manipulation

        /// <summary>Gets the percentage of the Decimal.</summary>
        public static decimal operator *(decimal d, Percentage p) => d.Multiply(p);
        /// <summary>Gets the percentage of the Double.</summary>
        public static double operator *(double d, Percentage p) => d.Multiply(p);
        /// <summary>Gets the percentage of the Single.</summary>
        public static float operator *(float d, Percentage p) => d.Multiply(p);

        /// <summary>Gets the percentage of the Int64.</summary>
        public static long operator *(long d, Percentage p) => d.Multiply(p);
        /// <summary>Gets the percentage of the Int32.</summary>
        public static int operator *(int d, Percentage p) => d.Multiply(p);
        /// <summary>Gets the percentage of the Int16.</summary>
        public static short operator *(short d, Percentage p) => d.Multiply(p);

        /// <summary>Gets the percentage of the UInt64.</summary>
        [CLSCompliant(false)]
        public static ulong operator *(ulong d, Percentage p) => d.Multiply(p);
        /// <summary>Gets the percentage of the UInt32.</summary>
        [CLSCompliant(false)]
        public static uint operator *(uint d, Percentage p) => d.Multiply(p);
        /// <summary>Gets the percentage of the UInt16.</summary>
        [CLSCompliant(false)]
        public static ushort operator *(ushort d, Percentage p) => d.Multiply(p);

        /// <summary>Divides the Decimal by the percentage.</summary>
        public static decimal operator /(decimal d, Percentage p) => d.Divide(p);
        /// <summary>Divides the Double by the percentage.</summary>
        public static double operator /(double d, Percentage p) => d.Divide(p);
        /// <summary>Divides the Single by the percentage.</summary>
        public static float operator /(float d, Percentage p) => d.Divide(p);

        /// <summary>Divides the Int64 by the percentage.</summary>
        public static long operator /(long d, Percentage p) => d.Divide(p);
        /// <summary>Divides the Int32 by the percentage.</summary>
        public static int operator /(int d, Percentage p) => d.Divide(p);
        /// <summary>Divides the Int16 by the percentage.</summary>
        public static short operator /(short d, Percentage p) => d.Divide(p);

        /// <summary>Divides the UInt64 by the percentage.</summary>
        [CLSCompliant(false)]
        public static ulong operator /(ulong d, Percentage p) => d.Divide(p);
        /// <summary>Divides the UInt32 by the percentage.</summary>
        [CLSCompliant(false)]
        public static uint operator /(uint d, Percentage p) => d.Divide(p);
        /// <summary>Divides the UInt16 by the percentage.</summary>
        [CLSCompliant(false)]
        public static ushort operator /(ushort d, Percentage p) => d.Divide(p);

        /// <summary>Adds the percentage to the Decimal.</summary>
        public static decimal operator +(decimal d, Percentage p) => d.Add(p);
        /// <summary>Adds the percentage to the Double.</summary>
        public static double operator +(double d, Percentage p) => d.Add(p);
        /// <summary>Adds the percentage to the Single.</summary>
        public static float operator +(float d, Percentage p) => d.Add(p);

        /// <summary>Adds the percentage to the Int64.</summary>
        public static long operator +(long d, Percentage p) => d.Add(p);
        /// <summary>Adds the percentage to the Int32.</summary>
        public static int operator +(int d, Percentage p) => d.Add(p);
        /// <summary>Adds the percentage to the Int16.</summary>
        public static short operator +(short d, Percentage p) => d.Add(p);

        /// <summary>Adds the percentage to the UInt64.</summary>
        [CLSCompliant(false)]
        public static ulong operator +(ulong d, Percentage p) => d.Add(p);
        /// <summary>Adds the percentage to the UInt32.</summary>
        [CLSCompliant(false)]
        public static uint operator +(uint d, Percentage p) => d.Add(p);
        /// <summary>Adds the percentage to the UInt16.</summary>
        [CLSCompliant(false)]
        public static ushort operator +(ushort d, Percentage p) => d.Add(p);

        /// <summary>Subtracts the percentage to the Decimal.</summary>
        public static decimal operator -(decimal d, Percentage p) => d.Subtract(p);
        /// <summary>Subtracts the percentage to the Double.</summary>
        public static double operator -(double d, Percentage p) => d.Subtract(p);
        /// <summary>Subtracts the percentage to the Single.</summary>
        public static float operator -(float d, Percentage p) => d.Subtract(p);

        /// <summary>Subtracts the percentage to the Int64.</summary>
        public static long operator -(long d, Percentage p) => d.Subtract(p);
        /// <summary>Subtracts the percentage to the Int32.</summary>
        public static int operator -(int d, Percentage p) => d.Subtract(p);
        /// <summary>Subtracts the percentage to the Int16.</summary>
        public static short operator -(short d, Percentage p) => d.Subtract(p);

        /// <summary>Subtracts the percentage to the UInt64.</summary>
        [CLSCompliant(false)]
        public static ulong operator -(ulong d, Percentage p) => d.Subtract(p);
        /// <summary>Subtracts the percentage to the UInt32.</summary>
        [CLSCompliant(false)]
        public static uint operator -(uint d, Percentage p) => d.Subtract(p);
        /// <summary>Subtracts the percentage to the UInt16.</summary>
        [CLSCompliant(false)]
        public static ushort operator -(ushort d, Percentage p) => d.Subtract(p);

        #endregion

        #region Math-like methods

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
        public static Percentage Max(Percentage val1, Percentage val2) => val1 > val2 ? val1 : val2;

        /// <summary>Returns the largest of the percentages.</summary>
        /// <param name="values">
        /// The percentages to compare.
        /// </param>
        /// <returns>
        /// The percentage with the largest value.
        /// </returns>
        public static Percentage Max(params Percentage[] values) => Guard.NotNull(values, nameof(values)).Max();

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
        public static Percentage Min(Percentage val1, Percentage val2) => val1 < val2 ? val1 : val2;

        /// <summary>Returns the smallest of the percentages.</summary>
        /// <param name="values">
        /// The percentages to compare.
        /// </param>
        /// <returns>
        /// The percentage with the smallest value.
        /// </returns>
        public static Percentage Min(params Percentage[] values) => Guard.NotNull(values, nameof(values)).Min();

        /// <summary>Rounds the percentage.</summary>
        /// <returns>
        /// The percentage nearest to the percentage that contains zero
        /// fractional digits. If the percentage has no fractional digits,
        /// the percentage is returned unchanged.
        /// </returns>
        public Percentage Round() => Round(0);

        /// <summary>Rounds the percentage to a specified number of fractional digits.</summary>
        /// <param name="decimals">
        /// The number of decimal places in the return value.
        /// </param>
        /// <returns>
        /// The percentage nearest to the percentage that contains a number of
        /// fractional digits equal to <paramref name="decimals"/>. If the
        /// percentage has fewer fractional digits than <paramref name="decimals"/>,
        /// the percentage is returned unchanged.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="decimals"/> is less than 0 or greater than 26.
        /// </exception>
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
        /// fractional digits equal to <paramref name="decimals"/>. If the
        /// percentage has fewer fractional digits than <paramref name="decimals"/>,
        /// the percentage is returned unchanged.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="decimals"/> is less than 0 or greater than 26.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="mode"/> is not a valid value of <see cref="MidpointRounding"/>.
        /// </exception>
        [Obsolete("Use  Round(decimals, DecimalRounding) instead")]
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
        /// fractional digits equal to <paramref name="decimals"/>. If the
        /// percentage has fewer fractional digits than <paramref name="decimals"/>,
        /// the percentage is returned unchanged.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="decimals"/> is less than 0 or greater than 26.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="mode"/> is not a valid value of <see cref="DecimalRounding"/>.
        /// </exception>
        public Percentage Round(int decimals, DecimalRounding mode)
        {
            if ((decimals < -26) || (decimals > 26))
            {
                throw new ArgumentOutOfRangeException(nameof(decimals), QowaivMessages.ArgumentOutOfRange_PercentageRound);
            }
            return m_Value.Round(decimals + 2, mode);
        }

        /// <summary>Rounds the percentage to a specified multiple of the specified percentage.</summary>
        /// <param name="multipleOf">
        /// The percentage of which the number should be multiple of.
        /// </param>
        public Percentage RoundToMultiple(Percentage multipleOf) => RoundToMultiple(multipleOf, DecimalRounding.AwayFromZero);

        /// <summary>Rounds the percentage to a specified multiple of the specified percentage.</summary>
        /// <param name="multipleOf">
        /// The percentage of which the number should be multiple of.
        /// </param>
        /// <param name="mode">
        /// Specification for how to round if it is midway between two other numbers.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="mode"/> is not a valid value of <see cref="DecimalRounding"/>.
        /// </exception>
        public Percentage RoundToMultiple(Percentage multipleOf, DecimalRounding mode)
        {
            return m_Value.RoundToMultiple((decimal)multipleOf, mode);
        }

        #endregion

        #region (JSON) (De)serialization

        /// <summary>Deserializes the percentage from a JSON number.</summary>
        /// <param name="json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized percentage.
        /// </returns>
        public static Percentage FromJson(double json) => new Percentage((decimal)json);

        /// <summary>Serializes the percentage to a JSON node.</summary>
        /// <returns>
        /// The serialized JSON string.
        /// </returns>
        public string ToJson() => ToString("0.############################%", CultureInfo.InvariantCulture);

        #endregion

        #region IFormattable / ToString

        /// <summary>Returns a <see cref="string"/> that represents the current Percentage for debug purposes.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get => ToString("0.00##########################%", CultureInfo.InvariantCulture);
        }

        /// <summary>Returns a <see cref="string"/> that represents the current Percentage formatted with a per ten Thousand mark.</summary>
        public string ToPerTenThousandMarkString()
        {
            return ToString("0.############################‱", CultureInfo.InvariantCulture);
        }
        /// <summary>Returns a <see cref="string"/> that represents the current Percentage formatted with a per mille mark.</summary>
        public string ToPerMilleString()
        {
            return ToString("0.############################‰", CultureInfo.InvariantCulture);
        }

        /// <summary>Returns a <see cref="string"/> that represents the current Percentage.</summary>
        public override string ToString() => ToString(CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Percentage.</summary>
        /// <param name="format">
        /// The format that this describes the formatting.
        /// </param>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a formatted <see cref="string"/> that represents the current Percentage.</summary>
        /// <param name="provider">
        /// The format provider.
        /// </param>
        public string ToString(IFormatProvider provider)
        {
            DefaultFormats.TryGetValue(provider ?? CultureInfo.CurrentCulture, out string format);
            format ??= "0.############################%";

            return ToString(format, provider);
        }

        /// <summary>Gets the default format for different countries.</summary>
        private static readonly Dictionary<IFormatProvider, string> DefaultFormats = new Dictionary<IFormatProvider, string>
        {
            { new CultureInfo("fr-FR"), "%0.############################" },
            { new CultureInfo("fa-IR"), "%0.############################" },
        };


        /// <summary>Returns a formatted <see cref="string"/> that represents the current Percentage.</summary>
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

            var marker = GetMarkerType(format ?? string.Empty, null);
            if (marker == PercentageMarkerType.Invalid)
            {
                throw new FormatException(QowaivMessages.FormatException_InvalidFormat);
            }
            var decimalVal = m_Value / Dividers[marker];
            var info = GetNumberFormatInfo(formatProvider);
            var str = decimalVal.ToString(RemoveMarks(format ?? string.Empty, null), info);

            switch (marker)
            {
                case PercentageMarkerType.PercentageBefore: str = info.PercentSymbol + str; break;
                case PercentageMarkerType.PercentageAfter: str += info.PercentSymbol; break;
                case PercentageMarkerType.PerMilleBefore: str = info.PerMilleSymbol + str; break;
                case PercentageMarkerType.PerMilleAfter: str += info.PerMilleSymbol; break;
                case PercentageMarkerType.PerTenThousandBefore: str = PerTenThousandMark + str; break;
                case PercentageMarkerType.PerTenThousandAfter: str += PerTenThousandMark; break;
            }
            return str;
        }

        /// <summary>Gets an XML string representation of the @FullName.</summary>
        private string ToXmlString() => ToString("0.############################%", CultureInfo.InvariantCulture);

        #endregion

        #region (Explicit) casting

        /// <summary>Casts a Percentage to a <see cref="string"/>.</summary>
        public static explicit operator string(Percentage val) => val.ToString(CultureInfo.CurrentCulture);
        /// <summary>Casts a <see cref="string"/> to a Percentage.</summary>
        public static explicit operator Percentage(string str) => Parse(str, CultureInfo.CurrentCulture);


        /// <summary>Casts a decimal a Percentage.</summary>
        public static implicit operator Percentage(decimal val) => Create(val);
        /// <summary>Casts a decimal a Percentage.</summary>
        public static implicit operator Percentage(double val) => Create(val);

        /// <summary>Casts a Percentage to a decimal.</summary>
        public static explicit operator decimal(Percentage val) => val.m_Value;
        /// <summary>Casts a Percentage to a double.</summary>
        public static explicit operator double(Percentage val) => (double)val.m_Value;

        #endregion

        /// <summary>Represents the underlying value as <see cref="IConvertible"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IConvertible Convertable => m_Value;

        /// <inheritdoc/>
        TypeCode IConvertible.GetTypeCode() => TypeCode.Decimal;

        /// <summary>Converts the string to a Percentage.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">
        /// A string containing a Percentage to convert.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider.
        /// </param>
        /// <param name="result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, IFormatProvider formatProvider, out Percentage result)
        {
            result = Zero;

            if (!string.IsNullOrEmpty(s))
            {
                var info = GetNumberFormatInfo(formatProvider);
                var marker = GetMarkerType(s, info);
                s = RemoveMarks(s, info);
                if (marker != PercentageMarkerType.Invalid && decimal.TryParse(s, NumberStyles.Number, info, out decimal dec))
                {
                    dec *= Dividers[marker];
                    result = Create(dec);
                    return true;
                }
            }
            return false;
        }

        /// <summary>Creates a Percentage from a Decimal.</summary >
        /// <param name="val" >
        /// A decimal describing a Percentage.
        /// </param >
        public static Percentage Create(decimal val) => new Percentage { m_Value = val };

        /// <summary>Creates a Percentage from a Double.</summary >
        /// <param name="val" >
        /// A decimal describing a Percentage.
        /// </param >
        public static Percentage Create(double val) => Create((decimal)val);

        #region Parsing Helpers

        internal enum PercentageMarkerType
        {
            None = 0,
            PercentageBefore,
            PercentageAfter,
            PerMilleBefore,
            PerMilleAfter,
            PerTenThousandBefore,
            PerTenThousandAfter,
            Invalid,
        }

        internal static readonly Dictionary<PercentageMarkerType, decimal> Dividers = new Dictionary<PercentageMarkerType, decimal>
        {
            { PercentageMarkerType.None, 0.01m },
            { PercentageMarkerType.PercentageBefore, 0.01m },
            { PercentageMarkerType.PercentageAfter, 0.01m },
            { PercentageMarkerType.PerMilleBefore, 0.001m },
            { PercentageMarkerType.PerMilleAfter, 0.001m },
            { PercentageMarkerType.PerTenThousandBefore, 0.0001m },
            { PercentageMarkerType.PerTenThousandAfter, 0.0001m },
        };

        private static PercentageMarkerType GetMarkerType(string str, NumberFormatInfo info)
        {
            var cent = info == null ? PercentageMark : info.PercentSymbol;
            var mille = info == null ? PerMilleMark : info.PerMilleSymbol;

            var count = Count(str, PercentageMark) + Count(str, PerMilleMark) + Count(str, PerTenThousandMark);
            if (cent != PercentageMark) { count += Count(str, cent); }
            if (mille != PerMilleMark) { count += Count(str, mille); }
            if (count > 1) { return PercentageMarkerType.Invalid; }

            if (str.EndsWith(PercentageMark, StringComparison.Ordinal)) { return PercentageMarkerType.PercentageAfter; }
            if (str.StartsWith(PercentageMark, StringComparison.Ordinal)) { return PercentageMarkerType.PercentageBefore; }
            if (str.Contains(PercentageMark)) { return PercentageMarkerType.Invalid; }

            if (cent != PercentageMark)
            {
                if (str.EndsWith(cent, StringComparison.Ordinal)) { return PercentageMarkerType.PercentageAfter; }
                if (str.StartsWith(cent, StringComparison.Ordinal)) { return PercentageMarkerType.PercentageBefore; }
                if (str.Contains(cent)) { return PercentageMarkerType.Invalid; }
            }

            if (str.EndsWith(PerMilleMark, StringComparison.Ordinal)) { return PercentageMarkerType.PerMilleAfter; }
            if (str.StartsWith(PerMilleMark, StringComparison.Ordinal)) { return PercentageMarkerType.PerMilleBefore; }
            if (str.Contains(PerMilleMark)) { return PercentageMarkerType.Invalid; }

            if (mille != PerMilleMark)
            {
                if (str.EndsWith(mille, StringComparison.Ordinal)) { return PercentageMarkerType.PerMilleAfter; }
                if (str.StartsWith(mille, StringComparison.Ordinal)) { return PercentageMarkerType.PerMilleBefore; }
                if (str.Contains(mille)) { return PercentageMarkerType.Invalid; }
            }

            if (str.EndsWith(PerTenThousandMark, StringComparison.Ordinal)) { return PercentageMarkerType.PerTenThousandAfter; }
            if (str.StartsWith(PerTenThousandMark, StringComparison.Ordinal)) { return PercentageMarkerType.PerTenThousandBefore; }
            if (str.Contains(PerTenThousandMark)) { return PercentageMarkerType.Invalid; }
            return PercentageMarkerType.None;
        }

        /// <summary>Removers all percentage/per mille/per ten thousand marks from the string.</summary>
        private static string RemoveMarks(string str, IFormatProvider formatprovider)
        {
            var info = NumberFormatInfo.GetInstance(formatprovider);
            return str
                .Replace(PercentageMark, "")
                .Replace(PerMilleMark, "")
                .Replace(PerTenThousandMark, "")
                .Replace(info.PercentSymbol, "")
                .Replace(info.PerMilleSymbol, "");
        }

        private static int Count(string s, string sub)
        {
            var rep = s.Replace(sub, string.Empty);
            return (s.Length - rep.Length) / sub.Length;
        }

        /// <summary>Gets a <see cref="NumberFormatInfo"/> based on the <see cref="IFormatProvider"/>.</summary>
        /// <remarks>
        /// Because the options for formatting and parsing percentages as provided 
        /// by the .NET framework are not sufficient, internally we use number
        /// settings. For parsing and formatting however we like to use the
        /// percentage properties of the <see cref="NumberFormatInfo"/> instead of
        /// the number properties, so we copy them for desired behavior.
        /// </remarks>
        private static NumberFormatInfo GetNumberFormatInfo(IFormatProvider formatProvider)
        {
            var info = NumberFormatInfo.GetInstance(formatProvider);
            info = (NumberFormatInfo)info.Clone();
            info.NumberDecimalDigits = info.PercentDecimalDigits;
            info.NumberDecimalSeparator = info.PercentDecimalSeparator;
            info.NumberGroupSeparator = info.PercentGroupSeparator;
            info.NumberGroupSizes = info.PercentGroupSizes;
            return info;
        }

        #endregion
    }
}
