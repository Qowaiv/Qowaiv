namespace Qowaiv.Mathematics;

public readonly partial struct Fraction
{
    /// <summary>Creates a fraction based on decimal number.</summary>
    /// <param name="number">
    /// The decimal value to represent as a fraction.
    /// </param>
    /// <param name="error">
    /// The allowed error.
    /// </param>
    /// <remarks>
    /// Inspired by "Sjaak", see: https://stackoverflow.com/a/45314258/2266405.
    /// </remarks>
    [Pure]
    public static Fraction Create(decimal number, decimal error)
    {
        if (!number.IsInRange(long.MinValue, long.MaxValue))
        {
            throw new ArgumentOutOfRangeException(nameof(number), QowaivMessages.OverflowException_Fraction);
        }
        else if (!error.IsInRange(MinimumError, 1))
        {
            throw new ArgumentOutOfRangeException(nameof(error), QowaivMessages.ArgumentOutOfRange_FractionError);
        }
        else if (number == decimal.Zero)
        {
            return Zero;
        }
        else
        {
            var sign = number.Sign();
            var value = number.Abs();
            var integer = (long)value;
            value -= integer;

            // The boundaries.
            var min = value - error;
            var max = value + error;

            // Already within the error margin.
            if (min < 0)
            {
                return Create(sign * integer);
            }
            else if (max > 1)
            {
                return Create(sign * (integer + 1));
            }
            else
            {
                long d = FindDenominator(min, max);
                long n = (long)((value * d) + 0.5m);
                return New(sign * ((integer * d) + n), d);
            }
        }
    }

    [Pure]
    private static long FindDenominator(decimal minValue, decimal maxValue)
    {
        // The two parts of the denominator to find.
        long lo = 1;
        long hi = (long)(1 / maxValue);
        var min = new DecimalFraction { Numerator = minValue, Denominator = 1 - (hi * minValue) };
        var max = new DecimalFraction { Numerator = 1 - (hi * maxValue), Denominator = maxValue };

        while (min.OneOrMore)
        {
            // Improve the lower part.
            var step = min.Value;
            min.Numerator -= step * min.Denominator;
            max.Denominator -= step * max.Numerator;
            lo += step * hi;

            if (max.OneOrMore)
            {
                // improve the higher part.
                step = max.Value;
                min.Denominator -= step * min.Numerator;
                max.Numerator -= step * max.Denominator;
                hi += step * lo;
            }
        }
        return lo + hi;
    }

    /// <summary>The minimum error that can be provided to the Create from floating-point.</summary>
    private const decimal MinimumError = 1m / long.MaxValue;

    /// <remarks>
    /// An in-memory helper class to store a decimal numerator and decimal denominator.
    /// </remarks>
    private ref struct DecimalFraction
    {
        public decimal Numerator;

        public decimal Denominator;

        public readonly long Value => (long)(Numerator / Denominator);

        public readonly bool OneOrMore => Numerator >= Denominator;
    }
}
