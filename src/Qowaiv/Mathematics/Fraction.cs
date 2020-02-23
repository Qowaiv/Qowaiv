#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Qowaiv.Conversion.Mathematics;
using Qowaiv.Formatting;
using Qowaiv.Json;
using Qowaiv.Text;

namespace Qowaiv.Mathematics
{
    /// <summary>Represents a fraction.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable]
    [OpenApiDataType(description: "Faction", type: "string", format: "faction", pattern: "-?[0-9]+(/[0-9]+)?")]
    [TypeConverter(typeof(FractionTypeConverter))]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Fraction : ISerializable
    {
        public static readonly Fraction Zero;
        public static readonly Fraction Epsilon = New(1, long.MaxValue);

        public static readonly Fraction MaxValue = New(long.MaxValue, 1);
        public static readonly Fraction MinValue = New(long.MaxValue, 1);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long numerator;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long denominator;

        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
            {
                throw new DivideByZeroException();
            }

            if (denominator < 0)
            {
                denominator = -denominator;
                numerator = -numerator;
            }

            this.numerator = numerator;
            this.denominator = denominator;

            Reduce(ref this.numerator, ref this.denominator);
        }

        /// <summary>Initializes a new instance of the fraction based on the serialization info.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private Fraction(SerializationInfo info, StreamingContext context)
        {
            Guard.NotNull(info, nameof(info));
            numerator = info.GetInt64(nameof(numerator));
            denominator = info.GetInt64(nameof(denominator));
        }

        private static Fraction New(long n, long d) => new Fraction { numerator = n, denominator = d, };

        /// <summary>Gets the numerator of the fraction.</summary>
        public long Numerator => numerator;

        /// <summary>Gets the denominator of the fraction.</summary>
        public long Denominator => denominator;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString("F", CultureInfo.InvariantCulture);


        /// <summary>Returns true if the faction is zero.</summary>
        public bool IsZero() => numerator == 0;


        public Fraction Multiply(long factor)
        {
            if (factor == 0)
            {
                return Zero;
            }

            // TODO: support negative.
            long n = Numerator;
            long d = Denominator;
            long f = factor;
            Reduce(ref d, ref f);

            return New(checked(n * f), d);
        }

        public Fraction Multiply(Fraction factor)
        {
            if (factor.IsZero())
            {
                return Zero;
            }

            // TODO: support negative.
            long n0 = Numerator;
            long d0 = Denominator;
            long n1 = factor.Numerator;
            long d1 = factor.Denominator;

            Reduce(ref n0, ref d1);
            Reduce(ref n1, ref d0);

            return New(checked(n0 * n1), checked(d0 * d1));
        }

        public Fraction Divide(long factor)
        {
            if (factor == 0)
            {
                throw new DivideByZeroException();
            }
            // TODO: support negative.
            long n = Numerator;
            long d = Denominator;
            long f = factor;
            Reduce(ref n, ref f);

            return New(n, checked(d * f));
        }

        public Fraction Divide(Fraction factor)
        {
            if (factor.IsZero())
            {
                throw new DivideByZeroException();
            }

            // TODO: support negative.
            long n0 = Numerator;
            long d0 = Denominator;
            long n1 = factor.Denominator;
            long d1 = factor.Numerator;

            Reduce(ref n0, ref d1);
            Reduce(ref n1, ref d0);

            return New(checked(n0 * n1), checked(d0 * d1));
        }

        public Fraction Add(Fraction other)
        {
            if (other.IsZero())
            {
                return this;
            }
            long n0 = Numerator;
            long d0 = Denominator;
            long n1 = other.Numerator;
            long d1 = other.Denominator;

            Reduce(ref n0, ref d1);
            Reduce(ref n1, ref d0);

            checked
            {
                var n = n0 * d1 + n1 * d0;
                var d = d0 * d1;
                return New(n, d);
            }
        }

        /// <summary>Reduce the numbers based on the greatest common divisor.</summary>
        private static void Reduce(ref long a, ref long b)
        {
            // while both are even.
            while ((a & 1) == 0 && (b & 1) == 0)
            {
                a >>= 1;
                b >>= 1;
            }
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
            long remainder;
            while (b != 0)
            {
                remainder = a % b;
                a = b;
                b = remainder;
            }
            return a;
        }


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

            return $"{Numerator}/{Denominator}";
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
            // TODO: implement.
            return 0;
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
        
        /// <summary>Deserializes the @FullNumber from a JSON number.</summary>
        /// <param name = "json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized fraction.
        /// </returns>
        public static Fraction FromJson(double json) => Create(json);

        /// <summary>Deserializes the @FullNumber from a JSON number.</summary>
        /// <param name = "json">
        /// The JSON number to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized fraction.
        /// </returns>
        public static Fraction FromJson(long json) => Create(json);

        #region (Explicit) casting

        public static explicit operator Fraction(long n) => n == 0 ? Zero : New(n, 1);
        public static explicit operator decimal(Fraction fraction) => fraction.IsZero() ? decimal.Zero : fraction.numerator / (decimal)fraction.denominator;
        public static explicit operator double(Fraction fraction) => fraction.IsZero() ? 0d : fraction.numerator / (double)fraction.denominator;

        ///// <summary>Casts the fraction to a <see cref = "long "/>.</summary>
        //public static explicit operator long(Fraction val) => val.m_Value;
        ///// <summary>Casts a <see cref = "long "/> to a fraction.</summary>
        //public static explicit operator Fraction(long val) => Create(val);

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

            if(fraction.HasValue)
            {
                result = fraction.Value;
                return true;
            }
            return false;
        }


        public static Fraction Create(decimal number)
        {
            throw new NotImplementedException();
        }

        public static Fraction Create(long number)
        {
            throw new NotImplementedException();
        }


        private static Fraction Create(double json)
        {
            throw new NotImplementedException();
        }


    }
}
