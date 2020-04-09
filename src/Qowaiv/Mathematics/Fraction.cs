﻿#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using Qowaiv.Conversion.Mathematics;
using Qowaiv.Formatting;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Qowaiv.Mathematics
{
    /// <summary>Represents a fraction.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable]
    [SingleValueObject(SingleValueStaticOptions.Continuous, typeof(Tuple<long, long>))]
    [OpenApiDataType(description: "Faction", type: "string", format: "faction", pattern: "-?[0-9]+(/[0-9]+)?")]
    [TypeConverter(typeof(FractionTypeConverter))]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Fraction : ISerializable, IXmlSerializable, IFormattable, IEquatable<Fraction>, IComparable, IComparable<Fraction>
    {
        /// <summary>Represents the zero (0) <see cref="Fraction"/> value.</summary>
        /// <remarks>
        /// Is the default value of <see cref="Fraction"/>.
        /// </remarks>
        public static readonly Fraction Zero;

        /// <summary>Represents the one (1) <see cref="Fraction"/> value.</summary>
        public static readonly Fraction One = New(1, 1);

        /// <summary>Represents the smallest positive <see cref="Fraction"/> value that is greater than zero.</summary>
        public static readonly Fraction Epsilon = New(1, long.MaxValue);

        /// <summary>Represents the largest possible value of a <see cref="Fraction"/>.</summary>
        public static readonly Fraction MaxValue = New(+long.MaxValue, 1);

        /// <summary>Represents the smallest possible value of a <see cref="Fraction"/>.</summary>
        public static readonly Fraction MinValue = New(-long.MaxValue, 1);

        internal static class Formatting
        {
            public const string SuperScript = "⁰¹²³⁴⁵⁶⁷⁸⁹";
            public const string SubScript = "₀₁₂₃₄₅₆₇₈₉";

            public const char Slash = '/';
            public const char Colon = ':';
            public const char DivisionSign = '÷';
            public const char FractionSlash = (char)0x2044;
            public const char DivisionSlash = (char)0x2215;
            public const char ShortSlash = (char)0x337;
            public const char LongSlash = (char)0x338;

            /// <summary>The different supported fraction bar characters.</summary>
            /// <remarks>
            /// name           | c | code 
            /// ---------------|---|------
            /// slash          | / |   5C
            /// colon          | : |   3A
            /// division sign  | ÷ |   F7
            /// fraction slash | ⁄ | 2044
            /// division slash | ∕ | 2215
            /// short slash    | ̷  |  337
            /// long slash     | ̸  |  338
            /// </remarks>
            public static readonly string FractionBars = new string(new[]
            {
                Slash,
                Colon,
                DivisionSign,
                FractionSlash,
                DivisionSlash,
                ShortSlash,
                LongSlash
            });

            public static readonly Regex Pattern = new Regex
            (
                @"^(\[(?<Whole>.+)\] ?)?(?<Numerator>.+?)(?<FractionBars>[/:÷⁄∕̷̸])(?<Denominator>.+)$", RegexOptions.Compiled
            );

            /// <summary>Returns true if the <see cref="char"/> is a supported fraction bar.</summary>
            public static bool IsFractionBar(char ch) => FractionBars.IndexOf(ch) != Parsing.NotFound;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long numerator;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long denominator;

        /// <summary>Creates a new instance of the <see cref="Fraction"/> class.</summary>
        /// <param name="numerator">
        /// The numerator part of the fraction.
        /// </param>
        /// <param name="denominator">
        /// The denominator part of the fraction.
        /// </param>
        /// <exception cref="DivideByZeroException">
        /// if the <paramref name="denominator"/> is zero.
        /// </exception>
        /// <remarks>
        /// If the fraction is negative, the numerator 'carries' the sign.
        /// Furthermore, the numerator and denominator are reduced (so 2/4
        /// becomes 1/2).
        /// </remarks>
        public Fraction(long numerator, long denominator)
        {
            if (numerator == long.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(numerator), QowaivMessages.OverflowException_Fraction);
            }
            if (denominator == 0 || denominator == long.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(denominator), QowaivMessages.OverflowException_Fraction);
            }

            // In case of zero, is should represent the default case.
            if (numerator == 0)
            {
                this.numerator = default;
                this.denominator = default;
            }
            else
            {
                var negative = numerator < 0 ^ denominator < 0;
                this.numerator = Math.Abs(numerator);
                this.denominator = Math.Abs(denominator);
                Reduce(ref this.numerator, ref this.denominator);

                if (negative)
                {
                    this.numerator = -this.numerator;
                }
            }
        }

        /// <summary>Gets the numerator of the fraction.</summary>
        public long Numerator => numerator;

        /// <summary>Gets the denominator of the fraction.</summary>
        public long Denominator => IsZero() ? 1 : denominator;

        /// <summary>Get whole of the fraction.</summary>
        public long Whole => IsZero() ? 0 : numerator / denominator;

        /// <summary>Gets the remainder of the fraction.</summary>
        /// <remarks>
        /// The remainder is expressed as an absolute value.
        /// </remarks>
        public long Remainder => IsZero() ? 0 : (numerator % denominator).Abs();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => string.Format(CultureInfo.InvariantCulture, "{0:super⁄sub} = {0:0.########}", this);

        /// <summary>Returns true if the faction is zero.</summary>
        public bool IsZero() => numerator == 0;

        /// <summary>Gets the sign of the fraction.</summary>
        public int Sign() => Math.Sign(numerator);

        /// <summary>Returns the absolute value of the fraction.</summary>
        public Fraction Abs() => New(numerator.Abs(), denominator);

        /// <summary>Pluses the fraction.</summary>
        internal Fraction Plus() => New(+numerator, denominator);

        /// <summary>Negates the fraction.</summary>
        internal Fraction Negate() => New(-numerator, denominator);

        /// <summary>Gets the inverse of a faction.</summary>
        /// <exception cref="DivideByZeroException">
        /// When the fraction is <see cref="Zero"/>.
        /// </exception>
        public Fraction Inverse()
        {
            if (IsZero())
            {
                throw new DivideByZeroException();
            }
            return New(Sign() * denominator, numerator.Abs());
        }

        /// <summary>Multiplies the fraction with the factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Fraction Multiply(Fraction factor)
        {
            if (factor.IsZero())
            {
                return Zero;
            }

            var sign = Sign() * factor.Sign();
            long n0 = numerator.Abs();
            long d0 = denominator;
            long n1 = factor.numerator.Abs();
            long d1 = factor.denominator;

            Reduce(ref n0, ref d1);
            Reduce(ref n1, ref d0);

            return checked(New(sign * n0 * n1, d0 * d1));
        }

        /// <summary>Multiplies the fraction with the factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Fraction Multiply(long factor) => Multiply(Create(factor));

        /// <summary>Multiplies the fraction with the factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Fraction Multiply(int factor) => Multiply((long)factor);

        /// <summary>Divide the fraction by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Fraction Divide(Fraction factor) => Multiply(factor.Inverse());

        /// <summary>Divide the fraction by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Fraction Divide(long factor)
        {
            if (factor == 0)
            {
                throw new DivideByZeroException();
            }
            return Multiply(New(1, factor));
        }

        /// <summary>Divide the fraction by a specified factor.</summary>
        /// <param name="factor">
        /// The factor to multiply with.
        /// </param>
        public Fraction Divide(int factor) => Divide((long)factor);

        /// <summary>Adds a fraction to the current fraction.</summary>
        /// <param name="fraction">
        /// The fraction to add.
        /// </param>
        public Fraction Add(Fraction fraction)
        {
            if (IsZero())
            {
                return fraction;
            }
            if (fraction.IsZero())
            {
                return this;
            }

            long n0 = numerator.Abs();
            long d0 = denominator;
            long n1 = fraction.numerator.Abs();
            long d1 = fraction.denominator;

            Reduce(ref n0, ref d1);
            Reduce(ref n1, ref d0);

            checked
            {
                long n;
                long d;

                // Same denominator.
                if (d0 == d1)
                {
                    d = d0;
                }
                // d0 is a multiple of d1
                else if (d0 > d1 && d0 % d1 == 0)
                {
                    d = d0;
                    n1 *= d0 / d1;
                }
                // d1 is a multiple of d0
                else if (d1 % d0 == 0)
                {
                    d = d1;
                    n0 *= d1 / d0;
                }
                else
                {
                    d = d0 * d1;
                    n0 *= d1;
                    n1 *= d0;
                }

                n = n0 * Sign() + n1 * fraction.Sign();

                var sign = n.Sign();
                n = n.Abs();

                Reduce(ref n, ref d);

                return New(n * sign, d);
            }
        }

        /// <summary>Adds a number to the current fraction.</summary>
        /// <param name="number">
        /// The number to add.
        /// </param>
        public Fraction Add(long number) => Add(Create(number));

        /// <summary>Adds a number to the current fraction.</summary>
        /// <param name="number">
        /// The number to add.
        /// </param>
        public Fraction Add(int number) => Add((long)number);

        /// <summary>Subtracts a fraction from the current fraction.</summary>
        /// <param name="fraction">
        /// The fraction to subtract.
        /// </param>
        public Fraction Subtract(Fraction fraction) => Add(fraction.Negate());

        /// <summary>Subtracts a number from the current fraction.</summary>
        /// <param name="number">
        /// The number to subtract.
        /// </param>
        public Fraction Subtract(long number) => Subtract(Create(number));

        /// <summary>Subtracts a number from the current fraction.</summary>
        /// <param name="number">
        /// The number to subtract.
        /// </param>
        public Fraction Subtract(int number) => Subtract((long)number);

        /// <summary>Pluses the fraction.</summary>
        public static Fraction operator +(Fraction fraction) => fraction.Plus();
        /// <summary>Negates the fraction.</summary>
        public static Fraction operator -(Fraction fraction) => fraction.Negate();

        /// <summary>Multiplies the left and the right fractions.</summary>
        public static Fraction operator *(Fraction left, Fraction right) => left.Multiply(right);
        /// <summary>Multiplies the left and the right fractions.</summary>
        public static Fraction operator *(Fraction left, long right) => left.Multiply(right);
        /// <summary>Multiplies the left and the right fractions.</summary>
        public static Fraction operator *(Fraction left, int right) => left.Multiply(right);
        /// <summary>Multiplies the left and the right fraction.</summary>
        public static Fraction operator *(long left, Fraction right) => right.Multiply(left);
        /// <summary>Multiplies the left and the right fraction.</summary>
        public static Fraction operator *(int left, Fraction right) => right.Multiply(left);

        /// <summary>Divide the fraction by a specified factor.</summary>
        public static Fraction operator /(Fraction fraction, Fraction factor) => fraction.Divide(factor);
        /// <summary>Divide the fraction by a specified factor.</summary>
        public static Fraction operator /(Fraction fraction, long factor) => fraction.Divide(factor);
        /// <summary>Divide the fraction by a specified factor.</summary>
        public static Fraction operator /(Fraction fraction, int factor) => fraction.Divide(factor);
        /// <summary>Divide the fraction by a specified factor.</summary>
        public static Fraction operator /(long number, Fraction factor) => Create(number).Divide(factor);
        /// <summary>Divide the fraction by a specified factor.</summary>
        public static Fraction operator /(int number, Fraction factor) => Create(number).Divide(factor);

        /// <summary>Adds the left and the right fraction.</summary>
        public static Fraction operator +(Fraction left, Fraction right) => left.Add(right);
        /// <summary>Adds the left and the right fraction.</summary>
        public static Fraction operator +(Fraction left, long right) => left.Add(right);
        /// <summary>Adds the left and the right fraction.</summary>
        public static Fraction operator +(Fraction left, int right) => left.Add(right);
        /// <summary>Adds the left and the right fraction.</summary>
        public static Fraction operator +(long left, Fraction right) => right.Add(left);
        /// <summary>Adds the left and the right fraction.</summary>
        public static Fraction operator +(int left, Fraction right) => right.Add(left);

        /// <summary>Subtracts the left from the right fraction.</summary>
        public static Fraction operator -(Fraction left, Fraction right) => left.Subtract(right);
        /// <summary>Subtracts the left from the right fraction.</summary>
        public static Fraction operator -(Fraction left, long right) => left.Subtract(right);
        /// <summary>Subtracts the left from the right fraction.</summary>
        public static Fraction operator -(Fraction left, int right) => left.Subtract(right);
        /// <summary>Subtracts the left from the right fraction.</summary>
        public static Fraction operator -(long left, Fraction right) => Create(left).Subtract(right);
        /// <summary>Subtracts the left from the right fraction.</summary>
        public static Fraction operator -(int left, Fraction right) => Create(left).Subtract(right);

        #region Equality & comparability

        /// <summary>Returns a formatted <see cref = "string "/> that represents the fraction.</summary>
        /// <param name = "format">
        /// The format that this describes the formatting.
        /// </param>
        /// <param name = "formatProvider">
        /// The format provider.
        /// </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
            {
                return formatted;
            }

            var match = Formatting.Pattern.Match(string.IsNullOrEmpty(format) ? @"0/0" : format);

            if (!match.Success)
            {
                // if no fraction bar character has been provided, format as a decimal.
                if (!format.Any(ch => Formatting.IsFractionBar(ch)))
                {
                    return ToDecimal().ToString(format, formatProvider);
                }
                throw new FormatException(QowaivMessages.FormatException_InvalidFormat);
            }

            var iFormat = match.Groups[nameof(Whole)].Value;
            var nFormat = match.Groups[nameof(Numerator)].Value;
            var dFormat = match.Groups[nameof(Denominator)].Value;
            var bar = match.Groups[nameof(Formatting.FractionBars)].Value;

            var sb = new StringBuilder();

            var remainder = numerator;
            var negative = formatProvider?.GetFormat<NumberFormatInfo>()?.NegativeSign ?? "-";

            if (!string.IsNullOrEmpty(iFormat))
            {
                remainder = Remainder;

                sb.Append(Whole.ToString(iFormat, formatProvider));

                // For -0 n/d
                if (Whole == 0 && Sign() == -1 && sb.Length != 0 && !sb.ToString().Contains(negative))
                {
                    sb.Insert(0, negative);
                }
            }
            if (nFormat == "super")
            {
                if (sb.Length == 0 && Sign() == -1)
                {
                    sb.Append(negative);
                }
                // use invariant as we want to convert to superscript.
                var super = remainder.Abs().ToString(CultureInfo.InvariantCulture).Select(ch => Formatting.SuperScript[ch - '0']).ToArray();
                sb.Append(super);
            }
            else
            {
                if (sb.Length != 0 && sb[sb.Length - 1] != ' ')
                {
                    sb.Append(' ');
                }
                sb.Append(remainder.ToString(nFormat, formatProvider));
            }

            sb.Append(bar);

            if (dFormat == "sub")
            {
                // use invariant as we want to convert to superscript.
                var super = Denominator.ToString(CultureInfo.InvariantCulture).Select(ch => Formatting.SubScript[ch - '0']).ToArray();
                sb.Append(super);
            }
            else
            {
                sb.Append(Denominator.ToString(dFormat, formatProvider));
            }
            return sb.ToString();
        }

        /// <inheritdoc/>
        public bool Equals(Fraction other)
        {
            // to deal with zero (default should be zero).
            if (IsZero() && other.IsZero())
            {
                return true;
            }
            return denominator == other.denominator
                && numerator == other.numerator;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (denominator * 113 * numerator).GetHashCode();
        }

        /// <inheritdoc/>
        public int CompareTo(Fraction other)
        {
            if (denominator == other.denominator || IsZero() || other.IsZero())
            {
                return numerator.CompareTo(other.numerator);
            }
            // To prevent overflows, normalize the numerators as double only.
            var self = (double)numerator * other.denominator;
            var othr = (double)other.numerator * denominator;

            return self.CompareTo(othr);
        }

        #endregion

        #region Serialization

        /// <summary>Initializes a new instance of the fraction based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Fraction(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            numerator = info.GetInt64(nameof(numerator));
            denominator = info.GetInt64(nameof(denominator));
        }

        /// <summary>Adds the underlying property of the fraction to the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            info.AddValue(nameof(numerator), numerator);
            info.AddValue(nameof(denominator), denominator);
        }

        /// <remarks>Sets the currency.</remarks>
        partial void OnReadXml(Fraction other)
        {
            numerator = other.numerator;
            denominator = other.denominator;
        }

        /// <summary>Gets an XML string representation of the fraction.</summary>
        private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Gets an JSON representation of the fraction.</summary>
        public string ToJson() => ToString(CultureInfo.InvariantCulture);

        /// <summary>Deserializes the fraction from a JSON number.</summary>
        /// <param name = "json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized fraction.
        /// </returns>
        public static Fraction FromJson(double json) => Cast(json);

        /// <summary>Deserializes the fraction from a JSON number.</summary>
        /// <param name = "json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized fraction.
        /// </returns>
        public static Fraction FromJson(long json) => New(json, 1);

        #endregion

        #region (Explicit) casting

        /// <summary>Casts the fraction to a <see cref="decimal"/>.</summary>
        private decimal ToDecimal() => IsZero() ? decimal.Zero : numerator / (decimal)denominator;
        /// <summary>Casts the fraction to a <see cref="double"/>.</summary>
        private double ToDouble() => IsZero() ? 0d : numerator / (double)denominator;

        /// <summary>Casts a <see cref="decimal"/> to a fraction.</summary>
        private static Fraction Cast(decimal number)
        {
            if (number < MinValue.numerator || number > MaxValue.numerator)
            {
                throw new OverflowException(QowaivMessages.OverflowException_Fraction);
            }
            return Create(number, MinimumError);
        }
        /// <summary>Casts a <see cref="double"/> to a fraction.</summary>
        private static Fraction Cast(double number)
        {
            if (number < MinValue.numerator || number > MaxValue.numerator)
            {
                throw new OverflowException(QowaivMessages.OverflowException_Fraction);
            }
            return Create((decimal)number, MinimumError);
        }

        /// <summary>Casts a <see cref="decimal"/> to a <see cref="Fraction"/>.</summary>
        public static explicit operator Fraction(decimal number) => Cast(number);
        /// <summary>Casts a <see cref="decimal"/> to a <see cref="Fraction"/>.</summary>
        public static explicit operator decimal(Fraction fraction) => fraction.ToDecimal();

        /// <summary>Casts a <see cref="Percentage"/> to a <see cref="Fraction"/>.</summary>
        public static explicit operator Fraction(Percentage number) => Cast((decimal)number);
        /// <summary>Casts a <see cref="Percentage"/> to a <see cref="Fraction"/>.</summary>
        public static explicit operator Percentage(Fraction fraction) => fraction.ToDecimal();

        /// <summary>Casts a <see cref="double"/> to a <see cref="Fraction"/>.</summary>
        public static explicit operator Fraction(double number) => Cast(number);
        /// <summary>Casts a <see cref="double"/> to a <see cref="Fraction"/>.</summary>
        public static explicit operator double(Fraction fraction) => fraction.ToDouble();

        /// <summary>Casts a <see cref="long"/> to a <see cref="Fraction"/>.</summary>
        public static explicit operator Fraction(long number) => Create(number);
        /// <summary>Casts a <see cref="long"/> to a <see cref="Fraction"/>.</summary>
        public static explicit operator long(Fraction fraction) => fraction.Whole;

        #endregion

        /// <summary>Converts the <see cref = "string "/> to <see cref = "Fraction"/>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name = "s">
        /// A string containing the fraction to convert.
        /// </param>
        /// <param name = "formatProvider">
        /// The specified format provider.
        /// </param>
        /// <param name = "result">
        /// The result of the parsing.
        /// </param>
        /// <returns>
        /// True if the string was converted successfully, otherwise false.
        /// </returns>
        public static bool TryParse(string s, IFormatProvider formatProvider, out Fraction result)
        {
            result = Zero;
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }

            var fraction = FractionParser.Parse(s, formatProvider?.GetFormat<NumberFormatInfo>() ?? CultureInfo.InvariantCulture.NumberFormat);

            if (fraction.HasValue)
            {
                result = fraction.Value;
                return true;
            }
            return false;
        }

        /// <summary>Creates a fraction based on decimal number.</summary>
        /// <param name="number">
        /// The decimal value to represent as a fraction.
        /// </param>
        /// <param name="error">
        /// The allowed error.
        /// </param>
        /// <remarks>
        /// Inspired by "Sjaak", see: https://stackoverflow.com/a/45314258/2266405
        /// </remarks>
        public static Fraction Create(decimal number, decimal error)
        {
            if (number < long.MinValue || number > long.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(number), QowaivMessages.OverflowException_Fraction);
            }
            if (error < MinimumError || error > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(error), QowaivMessages.ArgumentOutOfRange_FractionError);
            }
            if (number == decimal.Zero)
            {
                return Zero;
            }

            // Deal with negative values.
            var sign = number < 0 ? -1 : 1;
            var value = sign == 1 ? number : -number;
            var integer = (long)value;
            value -= integer;

            // The boundaries.
            var minValue = value - error;
            var maxValue = value + error;

            // Already within the error margin.
            if (minValue < 0)
            {
                return Create(sign * integer);
            }

            if (maxValue > 1)
            {
                return Create(sign * ++integer);
            }

            // The two parts of the denominator to find.
            long d_lo = 1;
            long d_hi = (long)(1 / maxValue);

            var f_lo = new DecimalFraction { n = minValue, d = 1 - d_hi * minValue };
            var f_hi = new DecimalFraction { n = 1 - d_hi * maxValue, d = maxValue };

            while (f_lo.OneOrMore)
            {
                // Improve the lower part.
                var step = f_lo.Value;
                f_lo.n -= step * f_lo.d;
                f_hi.d -= step * f_hi.n;
                d_lo += step * d_hi;

                if (!f_hi.OneOrMore) { break; }

                // improve the higher part.
                step = f_hi.Value;
                f_lo.d -= step * f_lo.n;
                f_hi.n -= step * f_hi.d;
                d_hi += step * d_lo;
            }

            long d = d_lo + d_hi;
            long n = (long)(value * d + 0.5m);
            return New(sign * (integer * d + n), d);
        }

        /// <summary>Creates a fraction based on a <see cref="decimal"/>.</summary>
        /// <param name="number">
        /// The decimal value to represent as a fraction.
        /// </param>
        public static Fraction Create(decimal number) => Create(number, MinimumError);

        /// <summary>Creates a fraction based on a <see cref="double"/>.</summary>
        /// <param name="number">
        /// The double value to represent as a fraction.
        /// </param>
        public static Fraction Create(double number)
        {
            if (number < (double)decimal.MinValue || number > (double)decimal.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(number), QowaivMessages.OverflowException_Fraction);
            }
            return Create((decimal)number, MinimumError);
        }

        /// <summary>Creates a fraction based on a <see cref="long"/>.</summary>
        /// <param name="number">
        /// The long value to represent as a fraction.
        /// </param>
        public static Fraction Create(long number) => number == 0 ? Zero : New(number, 1);

        /// <summary>Creates a new instance of the <see cref="Fraction"/> class.</summary>
        /// <exception cref="OverflowException">
        /// If the numerator is <see cref="long.MinValue"/>
        /// </exception>
        /// <remarks>
        /// This pseudo constructor differs from the public constructor that it
        /// does not reduce any value, nor checks the denominator.
        /// </remarks>
        private static Fraction New(long n, long d)
        {
            if (n == long.MinValue)
            {
                throw new OverflowException(QowaivMessages.OverflowException_Fraction);
            }
            return new Fraction { numerator = n, denominator = d, };
        }

        #region internal tooling

        /// <summary>The minimum error that can be provided to the Create from floating-point.</summary>
        private const decimal MinimumError = 1m / long.MaxValue;

        /// <remarks>
        /// An in-memory helper class to store a decimal numerator and decimal denominator.
        /// </remarks>
        private ref struct DecimalFraction
        {
            public decimal n;
            public decimal d;
            public long Value => (long)(n / d);
            public bool OneOrMore => n >= d;
        }

        /// <summary>Reduce the numbers based on the greatest common divisor.</summary>
        private static void Reduce(ref long a, ref long b)
        {
            var gcd = Gcd(a, b);
            a /= gcd;
            b /= gcd;
        }

        /// <summary>Gets the Greatest Common Divisor.</summary>
        /// <remarks>
        /// See: https://en.wikipedia.org/wiki/Greatest_common_divisor
        /// </remarks>
        private static long Gcd(long a, long b)
        {
            var even = 1;
            long remainder;
            // while both are even.
            while ((a & 1) == 0 && (b & 1) == 0)
            {
                a >>= 1;
                b >>= 1;
                even <<= 1;
            }
            while (b != 0)
            {
                remainder = a % b;
                a = b;
                b = remainder;
            }
            return a * even;
        }

        #endregion
    }
}
