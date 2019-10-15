using System;

namespace Qowaiv
{
    /// <summary>Extensions on <see cref="decimal"/> rounding.</summary>
    public static class DecimalRound
    {
        private const int ScaleMask = 0x00FF0000;
        private const int SignMask = unchecked((int)0x80000000);

        public static decimal Round(this decimal value) => value.Round(0);
        public static decimal Round(this decimal value, int decimals) => value.Round(decimals, DecimalRounding.AwayFromZero);
        public static decimal Round(this decimal value, int decimals, int multiplyOf) => value.Round(decimals, DecimalRounding.AwayFromZero, multiplyOf);
        public static decimal Round(this decimal value, int decimals, DecimalRounding rounding) => value.Round(decimals, rounding, 1);
        public static decimal Round(this decimal value, int decimals, DecimalRounding rounding, int multiplyOf)
        {
            Guard.DefinedEnum(rounding, nameof(rounding));
            Guard.Positive(multiplyOf, nameof(multiplyOf));

            if ((decimals < -28) || (decimals > 28))
            {
                throw new ArgumentOutOfRangeException(nameof(decimals), QowaivMessages.ArgumentOutOfRange_DecimalRound);
            }

            var withMultiply = multiplyOf != 1;

            var bits = decimal.GetBits(withMultiply ? value / multiplyOf : value);

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

            if (AdditionForRounding(b0, remainder, divisor, rounding, !negative) &&
                InternalAdd(ref b0, ref b1, ref b2, 1) != 0)
            {
                throw new OverflowException();
            }

            // For negative decimals, this can happen.
            while (scale < 0)
            {
                var diffChunk = (-scale > MaxInt64Scale) ? MaxInt64Scale : -scale;
                var factor = Powers10[diffChunk];

                if (InternalMultiply(ref b0, ref b1, ref b2, (uint)factor) != 0)
                {
                    throw new OverflowException();
                }
                scale += diffChunk;
            }

            var lo = (int)b0;
            var mi = (int)b1;
            var hi = (int)b2;

            var rounded = new decimal(lo, mi, hi, negative, (byte)scale);

            return withMultiply ?  rounded * multiplyOf: rounded;
        }

        private static bool AdditionForRounding(ulong b0, ulong remainder, ulong divisor, DecimalRounding rounding, bool isPositive)
        {
            if (remainder == 0 || rounding == DecimalRounding.Truncate)
            {
                return false;
            }
            if (rounding == DecimalRounding.Ceiling)
            {
                return isPositive;
            }
            if (rounding == DecimalRounding.Floor)
            {
                return !isPositive;
            }
            // if to even, and the divisor is twice the remainder only add
            // when the number is odd.
            if (rounding == DecimalRounding.ToEven && remainder == (divisor >> 1))
            {
                return (b0 & 1) == 1;
            }

            return remainder >= (divisor >> 1);
        }

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

        // The maximum power of 20 that a 64 bit integer can store
        private const int MaxInt64Scale = 20;

        // Fast access for 10^n where n is 0-9        
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
