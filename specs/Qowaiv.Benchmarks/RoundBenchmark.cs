using Qowaiv;
using Qowaiv.Mathematics;

namespace Benchmarks;

public partial class RoundBenchmark
{
    private const int Count = 1000;

    private readonly decimal[] Unrounded = new decimal[Count];
    private readonly decimal[] Rounded = new decimal[Count];

    public RoundBenchmark()
    {
        var rnd = new MersenneTwister();
        for (var i = 0; i < Count; i++)
        {
            var n = rnd.Next(500, 1000) * rnd.Next(100, 200);
            var d = (decimal)Math.Pow(10, rnd.Next(1, 6));
            Unrounded[i] = n / d;
        }
    }

    [Params(/*-2, -1, 0, 1, */2)]
    public int Decimals { get; set; }

    [Benchmark]
    public decimal[] Reference_()
    {
        for (var i = 0; i < Count; i++)
        {
            Rounded[i] = DecimalMath.Round_Old(Unrounded[i], Decimals, DecimalRounding.ToEven);
        }
        return Rounded;
    }

    [Benchmark]
    public decimal[] DecimalMath_()
    {
        for (var i = 0; i < Count; i++)
        {
            Rounded[i] = DecimalMath.Round(Unrounded[i], Decimals, DecimalRounding.ToEven);
        }
        return Rounded;
    }

    [Benchmark(Baseline = true)]
    public decimal[] Math_Round()
    {
        for (var i = 0; i < Count; i++)
        {
            Rounded[i] = decimal.Round(Unrounded[i], Decimals, MidpointRounding.AwayFromZero);
        }
        return Rounded;
    }
}
