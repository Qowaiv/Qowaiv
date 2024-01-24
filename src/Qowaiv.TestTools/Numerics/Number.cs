#if NET8_0_OR_GREATER

using System.Numerics;

namespace Qowaiv.TestTools.Numerics;

/// <summary>An static helper to call <see cref="INumber{TSelf}"/> methods.</summary>
/// <remarks>
/// As some implementations of <see cref="INumber{TSelf}"/> are explicit,
/// this helper accesses these methods via the interface.
/// </remarks>
public static class Number
{
    /// <inheritdoc cref="IAdditiveIdentity{T,T}.AdditiveIdentity" />
    [Pure]
    public static T AdditiveIdentity<T>() where T : IAdditiveIdentity<T, T>
        => T.AdditiveIdentity;

    /// <inheritdoc cref="IMultiplicativeIdentity{T,T}.MultiplicativeIdentity" />
    [Pure]
    public static T MultiplicativeIdentity<T>() where T : IMultiplicativeIdentity<T, T>
        => T.MultiplicativeIdentity;

    /// <inheritdoc cref="INumberBase{T}.Radix" />
    [Pure]
    public static int Radix<T>() where T : INumberBase<T>
        => T.Radix;

    /// <inheritdoc cref="INumberBase{T}.Abs(T)" />
    [Pure]
    public static T Abs<T>(T value) where T : INumberBase<T>
        => T.Abs(value);

    /// <inheritdoc cref="INumberBase{T}.IsCanonical(T)" />
    [Pure]
    public static bool IsCanonical<T>(T number) where T : INumberBase<T>
        => T.IsCanonical(number);

    /// <inheritdoc cref="INumberBase{T}.IsEvenInteger(T)" />
    [Pure]
    public static bool IsEvenInteger<T>(T number) where T : INumberBase<T>
         => T.IsEvenInteger(number);

    /// <inheritdoc cref="INumberBase{T}.IsOddInteger(T)" />
    [Pure]
    public static bool IsOddInteger<T>(T number) where T : INumberBase<T>
         => T.IsOddInteger(number);

    /// <inheritdoc cref="INumberBase{T}.IsFinite(T)" />
    [Pure]
    public static bool IsFinite<T>(T number) where T : INumberBase<T>
        => T.IsFinite(number);

    /// <inheritdoc cref="INumberBase{T}.IsInfinity(T)" />
    [Pure]
    public static bool IsInfinity<T>(T number) where T : INumberBase<T>
        => T.IsInfinity(number);

    /// <inheritdoc cref="INumberBase{T}.IsNegativeInfinity(T)" />
    [Pure]
    public static bool IsNegativeInfinity<T>(T number) where T : INumberBase<T>
        => T.IsNegativeInfinity(number);

    /// <inheritdoc cref="INumberBase{T}.IsNegativeInfinity(T)" />
    [Pure]
    public static bool IsPositiveInfinity<T>(T number) where T : INumberBase<T>
        => T.IsPositiveInfinity(number);

    /// <inheritdoc cref="INumberBase{T}.IsInteger(T)" />
    [Pure]
    public static bool IsInteger<T>(T number) where T : INumberBase<T>
        => T.IsInteger(number);

    /// <inheritdoc cref="INumberBase{T}.IsInteger(T)" />
    [Pure]
    public static bool IsNaN<T>(T number) where T : INumberBase<T>
        => T.IsNaN(number);

    /// <inheritdoc cref="INumberBase{T}.IsInteger(T)" />
    [Pure]
    public static bool IsNegative<T>(T number) where T : INumberBase<T>
        => T.IsNegative(number);

    /// <inheritdoc cref="INumberBase{T}.IsZero(T)" />
    [Pure]
    public static bool IsZero<T>(T number) where T : INumberBase<T>
        => T.IsZero(number);

    /// <inheritdoc cref="INumberBase{T}.IsPositive(T)" />
    [Pure]
    public static bool IsPositive<T>(T number) where T : INumberBase<T>
        => T.IsPositive(number);

    /// <inheritdoc cref="INumberBase{T}.IsNormal(T)" />
    [Pure]
    public static bool IsNormal<T>(T number) where T : INumberBase<T>
        => T.IsNormal(number);

    /// <inheritdoc cref="INumberBase{T}.IsSubnormal(T)" />
    [Pure]
    public static bool IsSubnormal<T>(T number) where T : INumberBase<T>
        => T.IsSubnormal(number);

    /// <inheritdoc cref="INumberBase{T}.IsRealNumber(T)" />
    [Pure]
    public static bool IsRealNumber<T>(T number) where T : INumberBase<T>
         => T.IsRealNumber(number);

    /// <inheritdoc cref="INumberBase{T}.IsComplexNumber(T)" />
    [Pure]
    public static bool IsComplexNumber<T>(T number) where T : INumberBase<T>
         => T.IsComplexNumber(number);

    /// <inheritdoc cref="INumberBase{T}.IsImaginaryNumber(T)" />
    [Pure]
    public static bool IsImaginaryNumber<T>(T number) where T : INumberBase<T>
        => T.IsImaginaryNumber(number);

    /// <inheritdoc cref="INumberBase{T}.MaxMagnitude(T, T)" />
    [Pure]
    public static T MaxMagnitude<T>(T x, T y) where T : INumberBase<T>
        => T.MaxMagnitude(x, y);

    /// <inheritdoc cref="INumberBase{T}.MaxMagnitudeNumber(T, T)" />
    [Pure]
    public static T MaxMagnitudeNumber<T>(T x, T y) where T : INumberBase<T>
        => T.MaxMagnitudeNumber(x, y);

    /// <inheritdoc cref="INumberBase{T}.MinMagnitude(T, T)" />
    [Pure]
    public static T MinMagnitude<T>(T x, T y) where T : INumberBase<T>
        => T.MinMagnitude(x, y);

    /// <inheritdoc cref="INumberBase{T}.MinMagnitudeNumber(T, T)" />
    [Pure]
    public static T MinMagnitudeNumber<T>(T x, T y) where T : INumberBase<T>
        => T.MinMagnitudeNumber(x, y);
}

#endif
