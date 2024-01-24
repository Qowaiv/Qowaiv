
namespace Qowaiv.Financial;

public readonly partial struct Amount
{
    /// <summary>Represents an Amount of zero.</summary>
    public static Amount Zero => default;

    /// <summary>Represents an Amount of one.</summary>
    public static Amount One => new(1);

    /// <summary>Unitary plusses the amount.</summary>
    public static Amount operator +(Amount amount) => amount;

    /// <summary>Negates the amount.</summary>
    public static Amount operator -(Amount amount) => new(-amount.m_Value);

    /// <summary>Increases the amount with one.</summary>
    public static Amount operator ++(Amount amount) => amount + One;

    /// <summary>Decreases the amount with one.</summary>
    public static Amount operator --(Amount amount) => amount - One;

    /// <summary>Adds the left and the right amount.</summary>
    public static Amount operator +(Amount l, Amount r) => new(l.m_Value + r.m_Value);

    /// <summary>Subtracts the right from the left amount.</summary>
    public static Amount operator -(Amount l, Amount r) => new(l.m_Value - r.m_Value);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, Percentage factor) => new(amount.m_Value * factor);

    /// <summary>Divides the amount by the percentage.</summary>
    public static Amount operator /(Amount amount, Percentage p) => new(amount.m_Value / p);

    /// <summary>Adds the percentage to the amount.</summary>
    public static Amount operator +(Amount amount, Percentage p) => new(amount.m_Value + p);

    /// <summary>Subtracts the percentage from the amount.</summary>
    public static Amount operator -(Amount amount, Percentage p) => new(amount.m_Value - p);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, decimal factor) => new(amount.m_Value * factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, double factor) => new(amount.m_Value * (decimal)factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, float factor) => new(amount.m_Value * (decimal)factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, long factor) => new(amount.m_Value * factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, int factor) => new(amount.m_Value * factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    public static Amount operator *(Amount amount, short factor) => new(amount.m_Value * factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator *(Amount amount, ulong factor) => new(amount.m_Value * factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator *(Amount amount, uint factor) => new(amount.m_Value * factor);

    /// <summary>Multiplies the amount with the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator *(Amount amount, ushort factor) => new(amount.m_Value * factor);

    /// <summary>Divides the amount by an other amount.</summary>
    public static decimal operator /(Amount numerator, Amount denominator) => numerator.m_Value / denominator.m_Value;

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, decimal factor) => new(amount.m_Value / factor);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, double factor) => new(amount.m_Value / (decimal)factor);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, float factor) => new(amount.m_Value / (decimal)factor);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, long factor) => new(amount.m_Value / factor);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, int factor) => new(amount.m_Value / factor);

    /// <summary>Divides the amount by the factor.</summary>
    public static Amount operator /(Amount amount, short factor) => new(amount.m_Value / factor);

    /// <summary>Divides the amount by the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator /(Amount amount, ulong factor) => new(amount.m_Value / factor);

    /// <summary>Divides the amount by the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator /(Amount amount, uint factor) => new(amount.m_Value / factor);

    /// <summary>Divides the amount by the factor.</summary>
    [CLSCompliant(false)]
    public static Amount operator /(Amount amount, ushort factor) => new(amount.m_Value / factor);

    /// <summary>Gets the remainder of the amount.</summary>
    public static Amount operator %(Amount left, Amount right) => new(left.m_Value % right.m_Value);
}

#if NET8_0_OR_GREATER

public readonly partial struct Amount : INumber<Amount>
    , IAdditionOperators<Amount, Percentage, Amount>, ISubtractionOperators<Amount, Percentage, Amount>
    , IMultiplyOperators<Amount, Percentage, Amount>, IDivisionOperators<Amount, Percentage, Amount>
    , IMultiplyOperators<Amount, decimal, Amount>, IDivisionOperators<Amount, decimal, Amount>
    , IMultiplyOperators<Amount, double, Amount>, IDivisionOperators<Amount, double, Amount>
    , IMultiplyOperators<Amount, long, Amount>, IDivisionOperators<Amount, long, Amount>
    , IMultiplyOperators<Amount, int, Amount>, IDivisionOperators<Amount, int, Amount>
    , IMultiplyOperators<Amount, short, Amount>, IDivisionOperators<Amount, short, Amount>
    , IMultiplyOperators<Amount, ulong, Amount>, IDivisionOperators<Amount, ulong, Amount>
    , IMultiplyOperators<Amount, uint, Amount>, IDivisionOperators<Amount, uint, Amount>
    , IMultiplyOperators<Amount, ushort, Amount>, IDivisionOperators<Amount, ushort, Amount>
{
    /// <inheritdoc />
    static int INumberBase<Amount>.Radix => 10;

    /// <inheritdoc />
    static Amount IAdditiveIdentity<Amount, Amount>.AdditiveIdentity => One;

    /// <inheritdoc />
    static Amount IMultiplicativeIdentity<Amount, Amount>.MultiplicativeIdentity => One;

    /// <inheritdoc />
    [Pure]
    static Amount INumberBase<Amount>.Abs(Amount value) => value.Abs();

    static bool INumberBase<Amount>.IsCanonical(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsComplexNumber(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsEvenInteger(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsFinite(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsImaginaryNumber(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsInfinity(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsInteger(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsNaN(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsNegative(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsNegativeInfinity(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsNormal(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsOddInteger(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsPositive(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsPositiveInfinity(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsRealNumber(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsSubnormal(Amount value)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.IsZero(Amount value)
    {
        throw new NotImplementedException();
    }

    static Amount INumberBase<Amount>.MaxMagnitude(Amount x, Amount y)
    {
        throw new NotImplementedException();
    }

    static Amount INumberBase<Amount>.MaxMagnitudeNumber(Amount x, Amount y)
    {
        throw new NotImplementedException();
    }

    static Amount INumberBase<Amount>.MinMagnitude(Amount x, Amount y)
    {
        throw new NotImplementedException();
    }

    static Amount INumberBase<Amount>.MinMagnitudeNumber(Amount x, Amount y)
    {
        throw new NotImplementedException();
    }

    static Amount INumberBase<Amount>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    static Amount INumberBase<Amount>.Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    static Amount ISpanParsable<Amount>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.TryConvertFromChecked<TOther>(TOther value, out Amount result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.TryConvertFromSaturating<TOther>(TOther value, out Amount result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.TryConvertFromTruncating<TOther>(TOther value, out Amount result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.TryConvertToChecked<TOther>(Amount value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.TryConvertToSaturating<TOther>(Amount value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.TryConvertToTruncating<TOther>(Amount value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Amount result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<Amount>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out Amount result)
    {
        throw new NotImplementedException();
    }

    static bool ISpanParsable<Amount>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Amount result)
    {
        throw new NotImplementedException();
    }

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    static Amount IMultiplyOperators<Amount, Amount, Amount>.operator *(Amount left, Amount right)
        => throw new NotSupportedException();

    static Amount IDivisionOperators<Amount, Amount, Amount>.operator /(Amount left, Amount right)
        => throw new NotSupportedException();
 
}
#endif
