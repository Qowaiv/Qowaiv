using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Qowaiv.Mathematics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Fraction : IEquatable<Fraction>
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
        private static Fraction New(long n, long d) => new Fraction { numerator = n, denominator = d, };

        public long Numerator => numerator;
        public long Denominator => denominator;

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

        public bool IsZero() => numerator == 0;

        public override bool Equals(object obj) => obj is Fraction other && Equals(other);

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

        public override int GetHashCode()
        {
            return (denominator * 113 * numerator).GetHashCode();
        }

        public override string ToString() => $"{Numerator}/{Denominator}";

        public static explicit operator Fraction(long n) => n == 0 ? Zero : New(n, 1);

        public static bool operator ==(Fraction l, Fraction r) => l.Equals(r);
        public static bool operator !=(Fraction l, Fraction r) => !(l == r);

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
    }
}
