namespace Qowaiv.Mathematics;

public readonly partial struct Fraction
{
    /// <summary>Pluses the fraction.</summary>
    public static Fraction operator +(Fraction fraction) => fraction;

    /// <summary>Negates the fraction.</summary>
    public static Fraction operator -(Fraction fraction) => New(-fraction.numerator, fraction.denominator);

    /// <summary>Increases the fraction with one.</summary>
    public static Fraction operator ++(Fraction value) => value + One;

    /// <summary>Decreases the fraction with one.</summary>
    public static Fraction operator --(Fraction value) => value - One;

    /// <summary>Multiplies the left and the right fractions.</summary>
    public static Fraction operator *(Fraction left, Fraction right) => left.Multiply(right);

    /// <summary>Multiplies the left and the right fractions.</summary>
    public static Fraction operator *(Fraction left, long right) => left * Create(right);

    /// <summary>Multiplies the left and the right fractions.</summary>
    public static Fraction operator *(Fraction left, int right) => left * Create(right);

    /// <summary>Multiplies the left and the right fraction.</summary>
    public static Fraction operator *(long left, Fraction right) => Create(left) * right;

    /// <summary>Multiplies the left and the right fraction.</summary>
    public static Fraction operator *(int left, Fraction right) => Create(left) * right;

    /// <summary>Divide the fraction by a specified factor.</summary>
    public static Fraction operator /(Fraction fraction, Fraction factor) => fraction * factor.Inverse();

    /// <summary>Divide the fraction by a specified factor.</summary>
    public static Fraction operator /(Fraction fraction, long factor) => fraction * New(1, factor);

    /// <summary>Divide the fraction by a specified factor.</summary>
    public static Fraction operator /(Fraction fraction, int factor) => fraction * New(1, factor);

    /// <summary>Divide the fraction by a specified factor.</summary>
    public static Fraction operator /(long number, Fraction factor) => Create(number) * factor.Inverse();

    /// <summary>Divide the fraction by a specified factor.</summary>
    public static Fraction operator /(int number, Fraction factor) => Create(number) * factor.Inverse();

    /// <summary>Adds the left and the right fraction.</summary>
    public static Fraction operator +(Fraction left, Fraction right) => left.Add(right);

    /// <summary>Adds the left and the right fraction.</summary>
    public static Fraction operator +(Fraction left, long right) => left + Create(right);

    /// <summary>Adds the left and the right fraction.</summary>
    public static Fraction operator +(Fraction left, int right) => left + Create(right);

    /// <summary>Adds the left and the right fraction.</summary>
    public static Fraction operator +(long left, Fraction right) => Create(left) + right;

    /// <summary>Adds the left and the right fraction.</summary>
    public static Fraction operator +(int left, Fraction right) => Create(left) + right;

    /// <summary>Subtracts the left from the right fraction.</summary>
    public static Fraction operator -(Fraction left, Fraction right) => left + New(-right.numerator, right.denominator);

    /// <summary>Subtracts the left from the right fraction.</summary>
    public static Fraction operator -(Fraction left, long right) => left - Create(right);

    /// <summary>Subtracts the left from the right fraction.</summary>
    public static Fraction operator -(Fraction left, int right) => left - Create(right);

    /// <summary>Subtracts the left from the right fraction.</summary>
    public static Fraction operator -(long left, Fraction right) => Create(left) - right;

    /// <summary>Subtracts the left from the right fraction.</summary>
    public static Fraction operator -(int left, Fraction right) => Create(left) - right;

    /// <summary>Gets the remainder of the fraction.</summary>
    public static Fraction operator %(Fraction fraction, Fraction divider) => fraction.Modulo(divider);

    /// <summary>Gets the remainder of the fraction.</summary>
    public static Fraction operator %(Fraction fraction, int divider) => fraction % Create(divider);

    /// <summary>Gets the remainder of the fraction.</summary>
    public static Fraction operator %(Fraction fraction, long divider) => fraction % Create(divider);

    /// <summary>Gets the remainder of the fraction.</summary>
    public static Fraction operator %(int number, Fraction divider) => Create(number) % divider;

    /// <summary>Gets the remainder of the fraction.</summary>
    public static Fraction operator %(long number, Fraction divider) => Create(number) % divider;

    /// <summary>Multiplies the fraction with the factor.</summary>
    [Pure]
    private Fraction Multiply(Fraction factor)
    {
        if (IsZero() || factor.IsZero()) return Zero;
        else
        {
            var sign = Sign() * factor.Sign();
            long n0 = numerator.Abs();
            long d0 = denominator;
            long n1 = factor.numerator.Abs();
            long d1 = factor.denominator;

            Reduce(ref n0, ref d1);
            Reduce(ref n1, ref d0);

            return checked(New(sign * n0 * n1, d0 * d1));
        }
    }

    /// <summary>Adds a fraction to the current fraction.</summary>
    [Pure]
    private Fraction Add(Fraction other)
    {
        if (IsZero()) return other;
        else if (other.IsZero()) return this;
        else
        {
            long d0 = denominator;
            long d1 = other.denominator;

            Reduce(ref d0, ref d1);

            checked
            {
                // The new denominator is reduced product of d0 en d1.
                var d2 = denominator * d1;
                var n2 = (numerator * d1) + (other.numerator * d0);

                return new(new Data(n2, d2).Simplify());
            }
        }
    }

    /// <summary>Multiplies the fraction with the factor.</summary>
    [Pure]
    private Fraction Modulo(Fraction divider)
    {
        if (divider.IsZero()) throw new DivideByZeroException();

        if (IsZero()) return Zero;
        else
        {
            long d0 = denominator;
            long d1 = divider.denominator;

            Reduce(ref d0, ref d1);

            checked
            {
                // The new denominator is reduced product of d0 en d1.
                var d2 = denominator * d1;
                var n2 = (numerator * d1) % (divider.numerator * d0);

                return new(new Data(n2, d2).Simplify());
            }
        }
    }
}

#if NET8_0_OR_GREATER

public readonly partial struct Fraction : INumber<Fraction>
    , IAdditionOperators<Fraction, long, Fraction>, ISubtractionOperators<Fraction, long, Fraction>
    , IAdditionOperators<Fraction, int, Fraction>, ISubtractionOperators<Fraction, int, Fraction>
    , IMultiplyOperators<Fraction, long, Fraction>, IDivisionOperators<Fraction, long, Fraction>
    , IMultiplyOperators<Fraction, int, Fraction>, IDivisionOperators<Fraction, int, Fraction>
    , IModulusOperators<Fraction, long, Fraction>, IModulusOperators<Fraction, int, Fraction>
    , IMinMaxValue<Fraction>
{
    /// <inheritdoc />
    static int INumberBase<Fraction>.Radix => 10;

    /// <inheritdoc />
    static Fraction IAdditiveIdentity<Fraction, Fraction>.AdditiveIdentity => One;

    /// <inheritdoc />
    static Fraction IMultiplicativeIdentity<Fraction, Fraction>.MultiplicativeIdentity => One;

    /// <inheritdoc />
    [Pure]
    static Fraction INumberBase<Fraction>.Abs(Fraction value) => value.Abs();

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsFinite(Fraction value) => true;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsImaginaryNumber(Fraction value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsNaN(Fraction value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsInfinity(Fraction value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsPositiveInfinity(Fraction value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsNegativeInfinity(Fraction value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsRealNumber(Fraction value) => true;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsComplexNumber(Fraction value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsNormal(Fraction value) => true;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsSubnormal(Fraction value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsCanonical(Fraction value) => true;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsInteger(Fraction value)
        => value.Denominator == 1;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsEvenInteger(Fraction value)
        => value.Denominator == 1
        && long.IsEvenInteger(value.Numerator);

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsNegative(Fraction value) => value.Numerator < 0;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsOddInteger(Fraction value)
        => value.Denominator == 1
        && long.IsOddInteger(value.Numerator);

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsPositive(Fraction value) => value.Numerator >= 0;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Fraction>.IsZero(Fraction value) => value.Numerator == 0;

    /// <inheritdoc />
    [Pure]
    static Fraction INumberBase<Fraction>.MaxMagnitude(Fraction x, Fraction y) => MaxMagnitude(x, y);

    /// <inheritdoc />
    [Pure]
    static Fraction INumberBase<Fraction>.MaxMagnitudeNumber(Fraction x, Fraction y) => MaxMagnitude(x, y);

    [Pure]
    private static Fraction MaxMagnitude(Fraction x, Fraction y)
    {
        var ax = x.Abs();
        var ay = y.Abs();

        if (ax > ay) return x;
        else if (ax == ay) return x < Zero ? y : x;
        else return y;
    }

    /// <inheritdoc />
    [Pure]
    static Fraction INumberBase<Fraction>.MinMagnitude(Fraction x, Fraction y) => MinMagnitude(x, y);

    /// <inheritdoc />
    [Pure]
    static Fraction INumberBase<Fraction>.MinMagnitudeNumber(Fraction x, Fraction y) => MinMagnitude(x, y);

    [Pure]
    private static Fraction MinMagnitude(Fraction x, Fraction y)
    {
        var ax = x.Abs();
        var ay = y.Abs();

        if (ax < ay) return x;
        else if (ax == ay) return x < Zero ? x : y;
        else return y;
    }

    /// <inheritdoc />
    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        charsWritten = 0;
        return false;
    }

    /// <inheritdoc />
    [Pure]
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static Fraction INumberBase<Fraction>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        => Parse(s.ToString(), provider);

    /// <inheritdoc />
    [Pure]
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static Fraction INumberBase<Fraction>.Parse(string s, NumberStyles style, IFormatProvider? provider)
        => Parse(s, provider);

    /// <inheritdoc />
    [Pure]
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static Fraction ISpanParsable<Fraction>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => Parse(s.ToString(), provider);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static bool INumberBase<Fraction>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Fraction result)
        => TryParse(s.ToString(), provider, out result);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static bool INumberBase<Fraction>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out Fraction result)
        => TryParse(s, provider, out result);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static bool ISpanParsable<Fraction>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Fraction result)
        => TryParse(s.ToString(), provider, out result);

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Fraction>.TryConvertFromChecked<TOther>(TOther value, out Fraction result)
       => throw new NotSupportedException();

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Fraction>.TryConvertFromSaturating<TOther>(TOther value, out Fraction result)
        => throw new NotSupportedException();

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Fraction>.TryConvertFromTruncating<TOther>(TOther value, out Fraction result)
        => throw new NotSupportedException();

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Fraction>.TryConvertToChecked<TOther>(Fraction value, out TOther result)
        => throw new NotSupportedException();

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Fraction>.TryConvertToSaturating<TOther>(Fraction value, out TOther result)
        => throw new NotSupportedException();

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Fraction>.TryConvertToTruncating<TOther>(Fraction value, out TOther result)
        => throw new NotSupportedException();
}

#endif
