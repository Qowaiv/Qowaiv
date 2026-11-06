namespace Qowaiv.Mathematics;

/// <summary>Math similar to <see cref="Math" /> but with <see cref="decimal" /> precision.</summary>
public static partial class DecimalMath
{
    /// <summary>
    /// Represents smallest non-zero decimal value, specified by the constant,
    /// ϵ = 1 × 10⁻²⁸.
    /// </summary>
    public const decimal Epsilon = 0.0000000000000000000000000001m;

    /// <summary>
    /// Represents the natural logarithmic base, specified by the constant,
    /// e ≈ 2.7182818284590452353602874714.
    /// </summary>
    /// <remarks>See: https://en.wikipedia.org/wiki/E_(mathematical_constant).</remarks>
    public const decimal E = 2.7182818284590452353602874714m;

    /// <summary>
    /// Represents the ratio of the circumference of a circle to its diameter, specified
    /// by the constant, π ≈ 3.1415926535897932384626433833.
    /// </summary>
    /// <remarks>See: https://en.wikipedia.org/wiki/Pi.</remarks>
    public const decimal Pi = 3.1415926535897932384626433833m;

    extension(decimal value)
    {
        /// <inheritdoc cref="Math.Abs(decimal)" />
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal Abs() => Math.Abs(value);

        /// <summary>Rounds a decimal value to the nearest integer.</summary>
        /// <returns>
        /// The integer that is nearest to the <paramref name="value" /> parameter. If the <paramref name="value" /> is halfway between two integers,
        /// it is rounded toward the nearest even integer.
        /// </returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal Round() => value.Round(0);

        /// <summary>Rounds a decimal value to a specified number of decimal places.</summary>
        /// <param name="decimals">
        /// A value from -28 to 28 that specifies the number of decimal places to round to.
        /// </param>
        /// <returns>
        /// The decimal number equivalent to <paramref name="value" /> rounded to <paramref name="decimals" /> number of decimal places.
        /// </returns>
        /// <remarks>
        /// A negative value for <paramref name="decimals" /> lowers precision to tenfold, hundredfold, and bigger.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal Round(int decimals) => value.Round(decimals, DecimalRounding.ToEven);

        /// <summary>Rounds a decimal value to a specified number of decimal places.</summary>
        /// <param name="decimals">
        /// A value from -28 to 28 that specifies the number of decimal places to round to.
        /// </param>
        /// <param name="mode">
        /// The mode of rounding applied.
        /// </param>
        /// <returns>
        /// The decimal number equivalent to <paramref name="value" /> rounded to <paramref name="decimals" /> number of decimal places.
        /// </returns>
        /// <remarks>
        /// A negative value for <paramref name="decimals" /> lowers precision to tenfold, hundredfold, and bigger.
        /// </remarks>
        [Pure]
        public decimal Round(int decimals, DecimalRounding mode)
        {
            Guard.DefinedEnum(mode);

            if (decimals is < -28 or > 28)
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

        /// <summary>Rounds a value to the closed number that is a multiple of the specified factor.</summary>
        /// <param name="multipleOf">
        /// The factor of which the number should be multiple of.
        /// </param>
        /// <returns>
        /// A rounded number that is multiple to the specified factor.
        /// </returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal RoundToMultiple(decimal multipleOf) => value.RoundToMultiple(multipleOf, DecimalRounding.ToEven);

        /// <summary>Rounds a value to the closed number that is a multiple of the specified factor.</summary>
        /// <param name="multipleOf">
        /// The factor of which the number should be multiple of.
        /// </param>
        /// <param name="mode">
        /// The rounding method used to determine the closed by number.
        /// </param>
        /// <returns>
        /// A rounded number that is multiple to the specified factor.
        /// </returns>
        [Pure]
        public decimal RoundToMultiple(decimal multipleOf, DecimalRounding mode)
        {
            Guard.Positive(multipleOf);
            return (value / multipleOf).Round(0, mode) * multipleOf;
        }

        /// <inheritdoc cref="Math.Sign(decimal)" />
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sign() => Math.Sign(value);
    }
}
