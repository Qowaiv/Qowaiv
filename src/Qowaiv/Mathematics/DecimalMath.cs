namespace Qowaiv.Mathematics;

internal static class DecimalMath
{
    /// <summary>The maximum power of 19 that a 32 bit integer can store.</summary>
    internal const int MaxInt32Scale = 9;

    /// <summary>Fast access for 10^n where n is [0..9].</summary>
    internal static readonly uint[] Powers10 =
    [
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
    ];

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

        var calc = DecCalc.New(value);

        var scaleDifference = calc.scale - decimals;

        if (scaleDifference <= 0)
        {
            return value;
        }

        ulong remainder;
        uint divisor;

        do
        {
            var diffCunck = (scaleDifference > MaxInt32Scale) ? MaxInt32Scale : scaleDifference;
            divisor = Powers10[diffCunck];
            remainder = calc.Divide(divisor);
            scaleDifference -= diffCunck;
            calc.scale -= diffCunck;
        }
        while (scaleDifference > 0);

        if (remainder != 0 && RoundUp(calc, remainder, divisor, mode))
        {
            calc.Add(1);
        }

        // For negative decimals, this can happen.
        while (calc.scale < 0)
        {
            var diffChunk = (-calc.scale > MaxInt32Scale) ? MaxInt32Scale : -calc.scale;
            var factor = Powers10[diffChunk];
            calc.Multiply(factor);
            calc.scale += diffChunk;
        }

        return calc.Value();

        static bool RoundUp(DecCalc calc, ulong remainder, ulong divisor, DecimalRounding mode) => mode switch
        {
            DecimalRounding.Truncate or
            DecimalRounding.DirectTowardsZero => false,
            DecimalRounding.DirectAwayFromZero => true,
            DecimalRounding.Ceiling => !calc.negative,
            DecimalRounding.Floor => calc.negative,
            DecimalRounding.StochasticRounding => StochasticRounding(remainder, divisor),
            _ when remainder == (divisor >> 1) => NearestRoundingUp(calc, mode),
            _ => remainder >= (divisor >> 1),
        };

        static bool NearestRoundingUp(DecCalc calc, DecimalRounding mode) => mode switch
        {
            DecimalRounding.ToEven => (calc.lo & 1) != 0,
            DecimalRounding.ToOdd => (calc.lo & 1) == 0,
            DecimalRounding.AwayFromZero => true,
            DecimalRounding.TowardsZero => false,
            DecimalRounding.Up => !calc.negative,
            DecimalRounding.Down => calc.negative,

            // Pick a 50-50 random.
            // DecimalRounding.RandomTieBreaking
            _ => (Random().Next() & 1) == 0,
        };

        static bool StochasticRounding(ulong remainder, ulong divisor)
        {
            var ratio = remainder / (double)divisor;
            return Random().NextDouble() <= ratio;
        }
    }

    /// <summary>Changes the scale part of the <see cref="decimal"/>.</summary>
    /// <remarks>
    /// This is equivalent to multiplying (or dividing) by a power of 10.
    /// </remarks>
    [Pure]
    public static decimal ChangeScale(decimal d, int delta)
    {
        var calc = DecCalc.New(d);
        calc.scale -= delta;

        calc.RemoveTrailingZeros();

        while (calc.scale < 0)
        {
            var diffChunk = (-calc.scale > MaxInt32Scale) ? MaxInt32Scale : -calc.scale;
            var factor = Powers10[diffChunk];
            calc.Multiply(factor);
            calc.scale += diffChunk;
        }
        while (calc.scale > 28)
        {
            var diffChunk = Math.Min(MaxInt32Scale, calc.scale - 28);
            var factor = Powers10[diffChunk];
            calc.Divide(factor);
            calc.scale -= diffChunk;
        }
        return calc.Value();
    }

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
