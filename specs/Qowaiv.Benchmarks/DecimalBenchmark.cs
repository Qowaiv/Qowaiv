using Qowaiv;

namespace Benchmarks;

public class DecimalBenchmark
{
    public class Scale
    {
        [ParamsSource(nameof(Values))]
        public decimal Value { get; set; }

        public static decimal[] Values => [100, 3.14m, 23.4326m];

        [Benchmark]
        public Percentage DecimalMath()
            => Value.Percent();

        [Benchmark]
        public Percentage Division()
            => Percentage.Create(Value / 100m);

        [Benchmark]
        public Percentage Multiplication()
            => Percentage.Create(Value * 0.01m);
    }
}
