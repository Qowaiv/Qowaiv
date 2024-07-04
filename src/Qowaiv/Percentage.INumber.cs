namespace Qowaiv;

public readonly partial struct Percentage
{
    /// <summary>Increases the percentage with one percent.</summary>
    public static Percentage operator ++(Percentage p) => p + One;

    /// <summary>Decreases the percentage with one percent.</summary>
    public static Percentage operator --(Percentage p) => p - One;

    /// <summary>Unitary plusses the percentage.</summary>
    public static Percentage operator +(Percentage p) => p;

    /// <summary>Negates the percentage.</summary>
    public static Percentage operator -(Percentage p) => new(-p.m_Value);

    /// <summary>Multiplies the left and the right percentage.</summary>
    public static Percentage operator *(Percentage l, Percentage r) => Create(l.m_Value * r.m_Value);

    /// <summary>Divides the left by the right percentage.</summary>
    public static Percentage operator /(Percentage l, Percentage r) => Create(l.m_Value / r.m_Value);

    /// <summary>Gets the remainder of the percentage.</summary>
    public static Percentage operator %(Percentage left, Percentage right) => new(left.m_Value % right.m_Value);

    /// <summary>Adds the left and the right percentage.</summary>
    public static Percentage operator +(Percentage l, Percentage r) => Create(l.m_Value + r.m_Value);

    /// <summary>Subtracts the right from the left percentage.</summary>
    public static Percentage operator -(Percentage l, Percentage r) => Create(l.m_Value - r.m_Value);

    /// <summary>Multiplies the percentage with the factor.</summary>
    public static Percentage operator *(Percentage p, decimal factor) => Create(p.m_Value * factor);

    /// <summary>Multiplies the percentage with the factor.</summary>
    public static Percentage operator *(Percentage p, double factor) => Create(p.m_Value * (decimal)factor);

    /// <summary>Multiplies the percentage with the factor.</summary>
    public static Percentage operator *(Percentage p, float factor) => Create(p.m_Value * (decimal)factor);

    /// <summary>Multiplies the percentage with the factor.</summary>
    public static Percentage operator *(Percentage p, long factor) => Create(p.m_Value * factor);

    /// <summary>Multiplies the percentage with the factor.</summary>
    public static Percentage operator *(Percentage p, int factor) => Create(p.m_Value * factor);

    /// <summary>Multiplies the percentage with the factor.</summary>
    public static Percentage operator *(Percentage p, short factor) => Create(p.m_Value * factor);

    /// <summary>Multiplies the percentage with the factor.</summary>
    [CLSCompliant(false)]
    public static Percentage operator *(Percentage p, ulong factor) => Create(p.m_Value * factor);

    /// <summary>Multiplies the percentage with the factor.</summary>
    [CLSCompliant(false)]
    public static Percentage operator *(Percentage p, uint factor) => Create(p.m_Value * factor);

    /// <summary>Multiplies the percentage with the factor.</summary>
    [CLSCompliant(false)]
    public static Percentage operator *(Percentage p, ushort factor) => Create(p.m_Value * factor);

    /// <summary>Divides the percentage by the factor.</summary>
    public static Percentage operator /(Percentage p, decimal factor) => Create(p.m_Value / factor);

    /// <summary>Divides the percentage by the factor.</summary>
    public static Percentage operator /(Percentage p, double factor) => Create(p.m_Value / (decimal)factor);

    /// <summary>Divides the percentage by the factor.</summary>
    public static Percentage operator /(Percentage p, float factor) => Create(p.m_Value / (decimal)factor);

    /// <summary>Divides the percentage by the factor.</summary>
    public static Percentage operator /(Percentage p, long factor) => Create(p.m_Value / factor);

    /// <summary>Divides the percentage by the factor.</summary>
    public static Percentage operator /(Percentage p, int factor) => new(p.m_Value / factor);

    /// <summary>Divides the percentage by the factor.</summary>
    public static Percentage operator /(Percentage p, short factor) => new(p.m_Value / factor);

    /// <summary>Divides the percentage by the factor.</summary>
    [CLSCompliant(false)]
    public static Percentage operator /(Percentage p, ulong factor) => new(p.m_Value / factor);

    /// <summary>Divides the percentage by the factor.</summary>
    [CLSCompliant(false)]
    public static Percentage operator /(Percentage p, uint factor) => new(p.m_Value / factor);

    /// <summary>Divides the percentage by the factor.</summary>
    [CLSCompliant(false)]
    public static Percentage operator /(Percentage p, ushort factor) => new(p.m_Value / factor);

    /// <summary>Gets the percentage of the Decimal.</summary>
    public static decimal operator *(decimal d, Percentage p) => d * p.m_Value;

    /// <summary>Gets the percentage of the Double.</summary>
    public static double operator *(double d, Percentage p) => d * (double)p.m_Value;

    /// <summary>Gets the percentage of the Single.</summary>
    public static float operator *(float d, Percentage p) => d * (float)p.m_Value;

    /// <summary>Gets the percentage of the Int64.</summary>
    public static long operator *(long d, Percentage p) => (long)(d * p.m_Value);

    /// <summary>Gets the percentage of the Int32.</summary>
    public static int operator *(int d, Percentage p) => (int)(d * p.m_Value);

    /// <summary>Gets the percentage of the Int16.</summary>
    public static short operator *(short d, Percentage p) => (short)(d * p.m_Value);

    /// <summary>Gets the percentage of the UInt64.</summary>
    [CLSCompliant(false)]
    public static ulong operator *(ulong d, Percentage p) => (ulong)(d * p.m_Value);

    /// <summary>Gets the percentage of the UInt32.</summary>
    [CLSCompliant(false)]
    public static uint operator *(uint d, Percentage p) => (uint)(d * p.m_Value);

    /// <summary>Gets the percentage of the UInt16.</summary>
    [CLSCompliant(false)]
    public static ushort operator *(ushort d, Percentage p) => (ushort)(d * p.m_Value);

    /// <summary>Divides the Decimal by the percentage.</summary>
    public static decimal operator /(decimal d, Percentage p) => d / p.m_Value;

    /// <summary>Divides the Double by the percentage.</summary>
    public static double operator /(double d, Percentage p) => d / (double)p.m_Value;

    /// <summary>Divides the Single by the percentage.</summary>
    public static float operator /(float d, Percentage p) => d / (float)p.m_Value;

    /// <summary>Divides the Int64 by the percentage.</summary>
    public static long operator /(long d, Percentage p) => (long)(d / p.m_Value);

    /// <summary>Divides the Int32 by the percentage.</summary>
    public static int operator /(int d, Percentage p) => (int)(d / p.m_Value);

    /// <summary>Divides the Int16 by the percentage.</summary>
    public static short operator /(short d, Percentage p) => (short)(d / p.m_Value);

    /// <summary>Divides the UInt64 by the percentage.</summary>
    [CLSCompliant(false)]
    public static ulong operator /(ulong d, Percentage p) => (ulong)(d / p.m_Value);

    /// <summary>Divides the UInt32 by the percentage.</summary>
    [CLSCompliant(false)]
    public static uint operator /(uint d, Percentage p) => (uint)(d / p.m_Value);

    /// <summary>Divides the UInt16 by the percentage.</summary>
    [CLSCompliant(false)]
    public static ushort operator /(ushort d, Percentage p) => (ushort)(d / p.m_Value);

    /// <summary>Adds the percentage to the Decimal.</summary>
    public static decimal operator +(decimal d, Percentage p) => d + (d * p);

    /// <summary>Adds the percentage to the Double.</summary>
    public static double operator +(double d, Percentage p) => d + (d * p);

    /// <summary>Adds the percentage to the Single.</summary>
    public static float operator +(float d, Percentage p) => d + (d * p);

    /// <summary>Adds the percentage to the Int64.</summary>
    public static long operator +(long d, Percentage p) => d + (d * p);

    /// <summary>Adds the percentage to the Int32.</summary>
    public static int operator +(int d, Percentage p) => d + (d * p);

    /// <summary>Adds the percentage to the Int16.</summary>
    public static short operator +(short d, Percentage p) => (short)(d + (d * p));

    /// <summary>Adds the percentage to the UInt64.</summary>
    [CLSCompliant(false)]
    public static ulong operator +(ulong d, Percentage p) => d + (d * p);

    /// <summary>Adds the percentage to the UInt32.</summary>
    [CLSCompliant(false)]
    public static uint operator +(uint d, Percentage p) => d + (d * p);

    /// <summary>Adds the percentage to the UInt16.</summary>
    [CLSCompliant(false)]
    public static ushort operator +(ushort d, Percentage p) => (ushort)(d + (d * p));

    /// <summary>Subtracts the percentage to the Decimal.</summary>
    public static decimal operator -(decimal d, Percentage p) => d - (d * p);

    /// <summary>Subtracts the percentage to the Double.</summary>
    public static double operator -(double d, Percentage p) => d - (d * p);

    /// <summary>Subtracts the percentage to the Single.</summary>
    public static float operator -(float d, Percentage p) => d - (d * p);

    /// <summary>Subtracts the percentage to the Int64.</summary>
    public static long operator -(long d, Percentage p) => d - (d * p);

    /// <summary>Subtracts the percentage to the Int32.</summary>
    public static int operator -(int d, Percentage p) => d - (d * p);

    /// <summary>Subtracts the percentage to the Int16.</summary>
    public static short operator -(short d, Percentage p) => (short)(d - (d * p));

    /// <summary>Subtracts the percentage to the UInt64.</summary>
    [CLSCompliant(false)]
    public static ulong operator -(ulong d, Percentage p) => d - (d * p);

    /// <summary>Subtracts the percentage to the UInt32.</summary>
    [CLSCompliant(false)]
    public static uint operator -(uint d, Percentage p) => d - (d * p);

    /// <summary>Subtracts the percentage to the UInt16.</summary>
    [CLSCompliant(false)]
    public static ushort operator -(ushort d, Percentage p) => (ushort)(d - (d * p));
}

#if NET8_0_OR_GREATER
public readonly partial struct Percentage : INumber<Percentage>
    , IMultiplyOperators<Percentage, decimal, Percentage>, IDivisionOperators<Percentage, decimal, Percentage>
    , IMultiplyOperators<Percentage, double, Percentage>, IDivisionOperators<Percentage, double, Percentage>
    , IMultiplyOperators<Percentage, long, Percentage>, IDivisionOperators<Percentage, long, Percentage>
    , IMultiplyOperators<Percentage, int, Percentage>, IDivisionOperators<Percentage, int, Percentage>
    , IMultiplyOperators<Percentage, short, Percentage>, IDivisionOperators<Percentage, short, Percentage>
    , IMultiplyOperators<Percentage, ulong, Percentage>, IDivisionOperators<Percentage, ulong, Percentage>
    , IMultiplyOperators<Percentage, uint, Percentage>, IDivisionOperators<Percentage, uint, Percentage>
    , IMultiplyOperators<Percentage, ushort, Percentage>, IDivisionOperators<Percentage, ushort, Percentage>
{
    /// <inheritdoc />
    static int INumberBase<Percentage>.Radix => 10;

    /// <inheritdoc />
    static Percentage IAdditiveIdentity<Percentage, Percentage>.AdditiveIdentity => One;

    /// <inheritdoc />
    static Percentage IMultiplicativeIdentity<Percentage, Percentage>.MultiplicativeIdentity => Hundred;

    /// <inheritdoc />
    [Pure]
    static Percentage INumberBase<Percentage>.Abs(Percentage value) => value.Abs();

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsCanonical(Percentage value) => decimal.IsCanonical(value.m_Value);

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsComplexNumber(Percentage value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsFinite(Percentage value) => true;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsImaginaryNumber(Percentage value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsInfinity(Percentage value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsNegativeInfinity(Percentage value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsPositiveInfinity(Percentage value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsNaN(Percentage value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsSubnormal(Percentage value) => false;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsRealNumber(Percentage value) => true;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsInteger(Percentage value) => value.m_Value.Abs() % 0.01m == 0;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsEvenInteger(Percentage value) => value.m_Value.Abs() % 0.02m == 0;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsOddInteger(Percentage value) => value.m_Value.Abs() % 0.02m == 0.01m;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsNormal(Percentage value) => value.m_Value != 0;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsNegative(Percentage value) => value < Zero;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsZero(Percentage value) => value == Zero;

    /// <inheritdoc />
    [Pure]
    static bool INumberBase<Percentage>.IsPositive(Percentage value) => value >= Zero;

    /// <inheritdoc />
    [Pure]
    static Percentage INumberBase<Percentage>.MaxMagnitude(Percentage x, Percentage y)
        => new(decimal.MaxMagnitude(x.m_Value, y.m_Value));

    /// <inheritdoc />
    [Pure]
    static Percentage INumberBase<Percentage>.MaxMagnitudeNumber(Percentage x, Percentage y)
        => new(decimal.MaxMagnitude(x.m_Value, y.m_Value));

    /// <inheritdoc />
    [Pure]
    static Percentage INumberBase<Percentage>.MinMagnitude(Percentage x, Percentage y)
        => new(decimal.MinMagnitude(x.m_Value, y.m_Value));

    /// <inheritdoc />
    [Pure]
    static Percentage INumberBase<Percentage>.MinMagnitudeNumber(Percentage x, Percentage y)
        => new(decimal.MinMagnitude(x.m_Value, y.m_Value));

    /// <inheritdoc />
    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => destination.TryWrite(ToString(format.ToString(), provider), out charsWritten);

    /// <inheritdoc />
    [Pure]
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static Percentage INumberBase<Percentage>.Parse(string s, NumberStyles style, IFormatProvider? provider)
        => TryParse(s, style, provider, out var svo)
            ? svo
            : throw Unparsable.ForValue<Percentage>(s, QowaivMessages.FormatExceptionPercentage);

    /// <inheritdoc />
    [Pure]
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static Percentage INumberBase<Percentage>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        => TryParse(s.ToString(), style, provider, out var svo)
            ? svo
            : throw Unparsable.ForValue<Percentage>(s.ToString(), QowaivMessages.FormatExceptionPercentage);

    /// <inheritdoc />
    [Pure]
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static Percentage ISpanParsable<Percentage>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s.ToString(), NumberStyles.Number, provider, out var svo)
            ? svo
            : throw Unparsable.ForValue<Percentage>(s.ToString(), QowaivMessages.FormatExceptionPercentage);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static bool INumberBase<Percentage>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Percentage result)
        => TryParse(s.ToString(), style, provider, out result);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage(Justification = "Only explicitly exposed overload of thoroughly tested parsing method.")]
    static bool ISpanParsable<Percentage>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Percentage result)
        => TryParse(s.ToString(), NumberStyles.Number, provider, out result);

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Percentage>.TryConvertFromChecked<TOther>(TOther value, out Percentage result)
        => throw new NotSupportedException();

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Percentage>.TryConvertToChecked<TOther>(Percentage value, out TOther result)
        => throw new NotSupportedException();

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Percentage>.TryConvertFromSaturating<TOther>(TOther value, out Percentage result)
        => throw new NotSupportedException();

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Percentage>.TryConvertToSaturating<TOther>(Percentage value, out TOther result)
        => throw new NotSupportedException();

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Percentage>.TryConvertFromTruncating<TOther>(TOther value, out Percentage result)
        => throw new NotSupportedException();

    [ExcludeFromCodeCoverage(Justification = "Protected member of the contract that is not supported.")]
    static bool INumberBase<Percentage>.TryConvertToTruncating<TOther>(Percentage value, out TOther result)
        => throw new NotSupportedException();
}
#endif
