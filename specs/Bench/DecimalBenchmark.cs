#pragma warning disable QW0013 // Qowaiv rounding here.

using BenchmarkDotNet.Configs;

namespace Bench;

public class DecimalBenchmark
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class Scale
    {
        public Scale()
        {
            for (var i = 0; i < _96Bit.Length; i++)
            {
                _96Bit[i] = Math.Round(Rnd.NextDecimal(), (int)Math.Sqrt(Rnd.Next(4, 29 * 29)));
                _32Bit[i] = Math.Round(Rnd.NextDecimal(), (int)Math.Sqrt(Rnd.Next(4, 9 * 9)));
            }
        }

        private readonly decimal[] _96Bit = new decimal[1_000];
        private readonly decimal[] _32Bit = new decimal[1_000];
        private readonly Percentage[] Percentages = new Percentage[1_000];

        [Benchmark(Description = "Trim")]
        [BenchmarkCategory("96 bit")]
        public Percentage[] _96Bits()
        {
            for(var i = 0; i < Percentages.Length; i++)
            {
                Percentages[i] = _96Bit[i].Percent();
            }
            return Percentages;
        }

        [Benchmark(Description = "Trim")]
        [BenchmarkCategory("32 bit")]
        public Percentage[] _32Bits()
        {
            for (var i = 0; i < Percentages.Length; i++)
            {
                Percentages[i] = _32Bit[i].Percent();
            }
            return Percentages;
        }

        [Benchmark(Description = "No trim", Baseline = true)]
        [BenchmarkCategory("96 bit")]
        public Percentage[] _96Bits_Create()
        {
            for (var i = 0; i < Percentages.Length; i++)
            {
                Percentages[i] = Percentage.Create(_96Bit[i]*0.01m);
            }
            return Percentages;
        }

        [Benchmark(Description = "No trim", Baseline = true)]
        [BenchmarkCategory("32 bit")]
        public Percentage[] _32Bits_Create()
        {
            for (var i = 0; i < Percentages.Length; i++)
            {
                Percentages[i] = Percentage.Create(_32Bit[i] * 0.01m);
            }
            return Percentages;
        }
    }

    private static readonly MersenneTwister Rnd = new(42);
}
