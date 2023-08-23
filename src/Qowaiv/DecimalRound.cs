namespace Qowaiv;

/// <summary>Extensions on <see cref="decimal"/> rounding.</summary>
internal static class DecimalRound
{
    private const int ScaleMask = 0x00FF0000;
    private const int SignMask = unchecked((int)0x80000000);

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
    [Pure]
    public static decimal Round(decimal value, int decimals, DecimalRounding mode)
    {
        Guard.DefinedEnum(mode);

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

        var b0 = (uint)bits[0];
        var b1 = (uint)bits[1];
        var b2 = (uint)bits[2];
        var negative = (bits[3] & SignMask) != 0;

        ulong remainder;
        uint divisor;

        do
        {
            var diffCunck = (scaleDifference > MaxInt32Scale) ? MaxInt32Scale : scaleDifference;
            divisor = Powers10[diffCunck];
            remainder = InternalDivide(ref b0, ref b1, ref b2, divisor);
            scaleDifference -= diffCunck;
            scale -= diffCunck;
        }
        while (scaleDifference > 0);

        if (ShouldRoundUp(b0, remainder, divisor, mode, !negative))
        {
            InternalAdd(ref b0, ref b1, ref b2, 1);
        }

        // For negative decimals, this can happen.
        while (scale < 0)
        {
            var diffChunk = (-scale > MaxInt32Scale) ? MaxInt32Scale : -scale;
            var factor = Powers10[diffChunk];
            InternalMultiply(ref b0, ref b1, ref b2, factor);
            scale += diffChunk;
        }

        var lo = (int)b0;
        var mi = (int)b1;
        var hi = (int)b2;

        var rounded = new decimal(lo, mi, hi, negative, (byte)scale);

        return rounded;
    }

    /// <summary>Returns true if the rounding should be up, otherwise false.</summary>
    [Pure]
    private static bool ShouldRoundUp(ulong b0, ulong remainder, ulong divisor, DecimalRounding mode, bool isPositive)
    {
        var halfway = divisor >> 1;
        if (remainder == 0 || mode == DecimalRounding.Truncate) return false;
        else return DirectRoundingUp(mode, isPositive)
            ?? NearestRoundingUp(b0, remainder, halfway, mode, isPositive)
            ?? StochasticRoundingUp(remainder, divisor, mode)
            ?? remainder >= halfway;
    }

    [Pure]
    private static bool? DirectRoundingUp(DecimalRounding mode, bool isPositive)
        => mode switch
        {
            DecimalRounding.DirectAwayFromZero => true,
            DecimalRounding.DirectTowardsZero => false,
            DecimalRounding.Ceiling => isPositive,
            DecimalRounding.Floor => !isPositive,
            _ => null,
        };

    [Pure]
    private static bool? NearestRoundingUp(ulong b0, ulong remainder, ulong halfway, DecimalRounding mode, bool isPositive)
        => remainder == halfway
        ? mode switch
        {
            DecimalRounding.ToEven => (b0 & 1) == 1,
            DecimalRounding.ToOdd => (b0 & 1) == 0,
            DecimalRounding.AwayFromZero => true,
            DecimalRounding.TowardsZero => false,
            DecimalRounding.Up => isPositive,
            DecimalRounding.Down => !isPositive,

            // Pick a 50-50 random.
            DecimalRounding.RandomTieBreaking => (Random().Next() & 1) == 0,
            _ => null,
        }
        : null;

    [Pure]
    private static bool? StochasticRoundingUp(ulong remainder, ulong divisor, DecimalRounding mode)
    {
        if (mode == DecimalRounding.StochasticRounding)
        {
            var ratio = remainder / (double)divisor;
            return Random().NextDouble() <= ratio;
        }
        else return null;
    }

    /// <summary>Multiplies the decimal with an <see cref="uint"/> factor.</summary>
    [Pure]
    private static void InternalMultiply(ref uint b0, ref uint b1, ref uint b2, uint factor)
    {
        ulong overflow = 0;
        ulong n;
        ulong f = factor;

        if (b0 != 0)
        {
            n = overflow + b0 * f;
            overflow = n >> 32;
            b0 = (uint)n;
        }
        if (b1 != 0 || overflow != 0)
        {
            n = overflow + b1 * f;
            overflow = n >> 32;
            b1 = (uint)n;
        }
        if (b2 != 0 || overflow != 0)
        {
            n = overflow + b2 * f;
            overflow = n >> 32;
            b2 = (uint)n;
        }
        if (overflow != 0)
        {
            throw new OverflowException(QowaivMessages.OverflowException_DecimalRound);
        }
    }

    /// <summary>Divides the decimal with an <see cref="uint"/> divisor.</summary>
    [Pure]
    private static ulong InternalDivide(ref uint b0, ref uint b1, ref uint b2, uint divisor)
    {
        ulong remainder = 0;
        ulong n;

        if (b2 != 0)
        {
            remainder = b2 % divisor;
            b2 /= divisor;
        }
        if (b1 != 0 || remainder != 0)
        {
            n = b1 | (remainder << 32);
            remainder = n % divisor;
            b1 = (uint)(n / divisor);
        }
        if (b0 != 0 || remainder != 0)
        {
            n = b0 | (remainder << 32);
            remainder = n % divisor;
            b0 = (uint)(n / divisor);
        }
        return remainder;
    }

    /// <summary>Adds an <see cref="uint"/> to the decimal.</summary>
    [Pure]
    private static void InternalAdd(ref uint b0, ref uint b1, ref uint b2, uint addition)
    {
        ulong overflow;
        ulong n;

        n = b0 + (ulong)addition;
        overflow = n >> 32;
        b0 = (uint)n;

        if (overflow != 0)
        {
            n = b1 + overflow;
            overflow = n >> 32;
            b1 = (uint)n;

            if (overflow != 0)
            {
                n = b2 + overflow;
                overflow = n >> 32;
                b2 = (uint)n;
            }
        }

        if (overflow != 0)
        {
            throw new OverflowException(QowaivMessages.OverflowException_DecimalRound);
        }
    }

    /// <summary>The maximum power of 19 that a 32 bit integer can store.</summary>
    private const int MaxInt32Scale = 9;

    /// <summary>Fast access for 10^n where n is 0-19.</summary>
    private static readonly uint[] Powers10 = new uint[]
    {
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
    };

    /// <summary>Gets a (thread static) instance of <see cref="Random"/>.</summary>
    /// <remarks>
    /// creates a new instance if required.
    /// </remarks>
    [Pure]
    private static Random Random()
    {
        _rnd ??= new Random();
        return _rnd;
    }

    [ThreadStatic]
    private static Random? _rnd;
}
