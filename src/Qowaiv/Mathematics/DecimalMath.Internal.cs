namespace Qowaiv.Mathematics;

public static partial class DecimalMath
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

    /// <summary>Changes the scale part of the <see cref="decimal" />.</summary>
    /// <remarks>
    /// This is equivalent to multiplying (or dividing) by a power of 10.
    /// </remarks>
    [Pure]
    internal static decimal ChangeScale(decimal d, int delta)
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

#if NETSTANDARD2_0
    /// <summary>Gets a (thread static) instance of <see cref="Random" />.</summary>
    /// <remarks>
    /// creates a new instance if required.
    /// </remarks>
    [Pure]
    private static Random Random()
    {
#pragma warning disable S2245 // Pseudorandom number generators can be used when security is not an issue.
        _rnd ??= new Random();
#pragma warning restore S2245 // Pseudorandom number generators (PRNGs) should not be used in security contexts
        return _rnd;
    }

    [ThreadStatic]
    private static Random? _rnd;
#else
    /// <summary>Gets a (thread static) instance of <see cref="Random" />.</summary>
    /// <remarks>
    /// <see cref="Random.Shared" /> is thread-safe.
    /// </remarks>
    [Pure]
    private static Random Random() => System.Random.Shared;
#endif
}
