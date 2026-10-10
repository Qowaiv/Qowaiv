#pragma warning disable QW0013 // Qowaiv rounding here.

namespace Bench;

public class DecimalBenchmark
{
    public class Scale
    {
        public Scale()
        {
            for (var i = 0; i < Numbers.Length; i++)
            {
                Numbers[i] = Math.Round(Rnd.NextDecimal(), (int)Math.Sqrt(Rnd.Next(1, 29)));
            }
        }

        private readonly decimal[] Numbers = new decimal[1_000];
        private readonly Percentage[] Percentages = new Percentage[1_000];

        [Benchmark]
        public Percentage[] DecimalMath()
        {
            for(var i = 0; i < Percentages.Length; i++)
            {
                Percentages[i] = Numbers[i].Percent();
            }
            return Percentages;
        }

        [Benchmark]
        public Percentage[] Division()
        {
            for (var i = 0; i < Percentages.Length; i++)
            {
                Percentages[i] = Percentage.Create(Numbers[i] / 100);
            }
            return Percentages;
        }

        [Benchmark]
        public Percentage[] Multiplication()
        {
            for (var i = 0; i < Percentages.Length; i++)
            {
                Percentages[i] = Percentage.Create(Numbers[i] * 0.01m);
            }
            return Percentages;
        }
    }

    private static readonly MersenneTwister Rnd = new(42);
}
