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
            decimal n = rnd.Next(0, 100) * rnd.Next(5, 10);
            var d = rnd.Next(3, 9) * rnd.Next(1, 3);
            var f = 0.1m;
            for(var p = 0; p < d; p++)
            {
                n += f * rnd.Next(0, 10);
                f *= 0.1m;
            }
            Unrounded[i] = n;
        }
    }

    [Params(/*-2, -1, 0, 1, */2)]
    public int Decimals { get; set; }
    
    [Benchmark(Baseline = true)]
    public decimal[] Math_Round()
    {
        for (var i = 0; i < Count; i++)
        {
            Rounded[i] = decimal.Round(Unrounded[i], Decimals, MidpointRounding.ToEven);
        }
        return Rounded;
    }

    [Benchmark]
    public decimal[] DecimalMath_()
    {
        for (var i = 0; i < Count; i++)
        {
            Rounded[i] = Unrounded[i].Round(Decimals, DecimalRounding.ToEven);
        }
        return Rounded;
    }
}
