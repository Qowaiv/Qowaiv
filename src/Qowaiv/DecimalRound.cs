using System;

namespace Qowaiv
{
    /// <summary>Extensions on <see cref="decimal"/> rounding.</summary>
    public static class DecimalRound
    {
        private const int ScaleMask = 0x00FF0000;
        private const int SignMask = unchecked((int)0x80000000);

        /// <summary>Rounds a value to the closed number that is a multiple of the specified factor.</summary>
        /// <param name="value">
        /// The value to round to.
        /// </param>
        /// <param name="factor">
        /// The factor of which the number should be multiple of.
        /// </param>
        /// <returns>
        /// A rounded number that is multiple to the specified factor.
        /// </returns>
        public static decimal MultipleOf(this decimal value, decimal factor)
        {
            Guard.Positive(factor, nameof(factor));
            return (value / factor).Round() * factor;
        }

        /// <summary>Rounds a value to the closed number that is a multiple of the specified factor.</summary>
        /// <param name="value">
        /// The value to round to.
        /// </param>
        /// <param name="factor">
        /// The factor of which the number should be multiple of.
        /// </param>
        /// <param name="mode">
        /// The rounding method used to determine the closed by number.
        /// </param>
        /// <returns>
        /// A rounded number that is multiple to the specified factor.
        /// </returns>
        public static decimal MultipleOf(this decimal value, decimal factor, DecimalRounding mode)
        {
            Guard.Positive(factor, nameof(factor));
            return (value / factor).Round(0, mode) * factor;
        }

        /// <summary>Rounds a decimal value to the nearest integer.</summary>
        /// <param name="value">
        /// A decimal number to round.
        /// </param>
        /// <returns>
        /// The integer that is nearest to the <paramref name="value"/> parameter. If the <paramref name="value"/> is halfway between two integers,
        /// it is rounded away from zero.
        /// </returns>
        public static decimal Round(this decimal value) => value.Round(0);

        /// <summary>Rounds a decimal value to a specified number of decimal places.</summary>
        /// <param name="value">
        /// A decimal number to round.
        /// </param>
        /// <param name="decimals">
        /// A value from -28 to 28 that specifies the number of decimal places to round to.
        /// </param>
        /// <returns>
        /// The decimal number equivalent to <paramref name="value"/> rounded to <paramref name="decimals"/> number of decimal places.
        /// </returns>
        /// <remarks>
        /// A negative value for <paramref name="decimals"/> lowers precision to tenfold, hundredfold, and bigger.
        /// </remarks>
        public static decimal Round(this decimal value, int decimals) => value.Round(decimals, DecimalRounding.AwayFromZero);

        /// <summary>Rounds a decimal value to a specified number of decimal places.</summary>
        /// <param name="value">
        /// A decimal number to round.
        /// </param>
        /// <param name="decimals">
        /// A value from -28 to 28 that specifies the number of decimal places to round to.
        /// </param>
        /// <param name="mode">
        /// The mode of rounding applied.
        /// </param>
        /// <returns>
        /// The decimal number equivalent to <paramref name="value"/> rounded to <paramref name="decimals"/> number of decimal places.
        /// </returns>
        /// <remarks>
        /// A negative value for <paramref name="decimals"/> lowers precision to tenfold, hundredfold, and bigger.
        /// </remarks>
        public static decimal Round(this decimal value, int decimals, DecimalRounding mode)
        {
            Guard.DefinedEnum(mode, nameof(mode));

            if ((decimals < -28) || (decimals > 28))
            {
                throw new ArgumentOutOfRangeException(nameof(decimals), QowaivMessages.ArgumentOutOfRange_DecimalRound);
            }

            var bits = decimal.GetBits(value);

            int scale = (bits[3] & ScaleMask) >> 16;
            var scaleDifference = scale - decimals;

            if (scaleDifference <= 0)
            {
                return value;
            }

            ulong b0 = (ulong)bits[0];
            ulong b1 = (ulong)bits[1];
            ulong b2 = (ulong)bits[2];
            var negative = (bits[3] & SignMask) != 0;

            ulong remainder;
            ulong divisor;

            do
            {
                var diffCunck = (scaleDifference > MaxInt64Scale) ? MaxInt64Scale : scaleDifference;
                divisor = Powers10[diffCunck];
                remainder = InternalDivide(ref b0, ref b1, ref b2, divisor);
                scaleDifference -= diffCunck;
                scale -= diffCunck;
            }
            while (scaleDifference > 0);

            var shouldRoundup = ShouldRoundUp(b0, remainder, divisor, mode, !negative);

            if (shouldRoundup && InternalAdd(ref b0, ref b1, ref b2, 1) != 0)
            {
                throw new OverflowException(QowaivMessages.OverflowException_DecimalRound);
            }

            // For negative decimals, this can happen.
            while (scale < 0)
            {
                var diffChunk = (-scale > MaxInt64Scale) ? MaxInt64Scale : -scale;
                var factor = Powers10[diffChunk];

                if (InternalMultiply(ref b0, ref b1, ref b2, (uint)factor) != 0)
                {
                    throw new OverflowException(QowaivMessages.OverflowException_DecimalRound);
                }
                scale += diffChunk;
            }

            var lo = (int)b0;
            var mi = (int)b1;
            var hi = (int)b2;

            var rounded = new decimal(lo, mi, hi, negative, (byte)scale);

            return rounded;
        }

        /// <summary>Returns true if the rounding should be up, otherwise false.</summary>
        private static bool ShouldRoundUp(ulong b0, ulong remainder, ulong divisor, DecimalRounding mode, bool isPositive)
        {
            if (remainder == 0 || mode == DecimalRounding.Truncate)
            {
                return false;
            }
            if (mode == DecimalRounding.Ceiling)
            {
                return isPositive;
            }
            if (mode == DecimalRounding.Floor)
            {
                return !isPositive;
            }
            // if to even, and the divisor is twice the remainder only add
            // when the number is odd.
            if (mode == DecimalRounding.ToEven && remainder == (divisor >> 1))
            {
                return (b0 & 1) == 1;
            }

            return remainder >= (divisor >> 1);
        }

        /// <summary>Divides the decimal with an <see cref="uint"/> divisor.</summary>
        private static ulong InternalDivide(ref ulong b0, ref ulong b1, ref ulong b2, ulong divisor)
        {
            ulong remainder = 0;

            if (b2 != 0)
            {
                remainder = b2 % divisor;
                b2 /= divisor;
            }
            if (b1 != 0 || remainder != 0)
            {
                b1 |= remainder << 32;
                remainder = b1 % divisor;
                b1 /= divisor;
            }
            if (b0 != 0 || remainder != 0)
            {
                b0 |= remainder << 32;
                remainder = b0 % divisor;
                b0 /= divisor;
            }
            return remainder;
        }

        /// <summary>Multiplies the decimal with an <see cref="uint"/> factor.</summary>
        private static ulong InternalMultiply(ref ulong b0, ref ulong b1, ref ulong b2, uint factor)
        {
            ulong overflow = 0;

            if (b0 != 0)
            {
                b0 *= factor;
                overflow = b0 >> 32;
            }
            if (b1 != 0 || overflow != 0)
            {
                b1 += overflow;
                b1 *= factor;
                overflow = b1 >> 32;
            }
            if (b2 != 0 || overflow != 0)
            {
                b2 += overflow;
                b2 *= factor;
                overflow = b2 >> 32;
            }
            return overflow;
        }

        /// <summary>Adds an <see cref="uint"/> to the decimal.</summary>
        private static ulong InternalAdd(ref ulong b0, ref ulong b1, ref ulong b2, uint addition)
        {
            ulong overflow = 0;

            if (b0 != 0)
            {
                b0 += addition;
                overflow = b0 >> 32;
            }
            if (overflow != 0)
            {
                b1 += overflow;
                overflow = b1 >> 32;

                if (overflow != 0)
                {
                    b2 += overflow;
                    overflow = b2 >> 32;
                }
            }

            return overflow;
        }

        /// <summary>The maximum power of 20 that a 64 bit integer can store.</summary>
        private const int MaxInt64Scale = 20;

        /// <summary>Fast access for 10^n where n is 0-19.</summary>
        private static readonly ulong[] Powers10 = new ulong[] {
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000,
            10000000000,
            100000000000,
            1000000000000,
            10000000000000,
            100000000000000,
            1000000000000000,
            10000000000000000,
            100000000000000000,
            1000000000000000000,
            10000000000000000000,
        };
    }
}
