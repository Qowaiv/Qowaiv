using BenchmarkDotNet.Configs;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bench;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[MemoryDiagnoser(true)]
[MinColumn]
public class PercentageBenchmark
{
    private const int Iterations = 1000;
    private readonly decimal[] Decimals = new decimal[Iterations];
    private readonly Percentage[] Percentages = new Percentage[Iterations];
    private readonly Stream Stream = new MemoryStream();

    public PercentageBenchmark()
    {
        var rnd = new MersenneTwister(Iterations);
        for(var i = 0; i< Iterations; i++)
        {
            var nr = (rnd.NextDecimal() * rnd.NextDecimal() * 200m).Round(2);
            Decimals[i] = nr;
            Percentages[i] = nr.Percent();
        }
    }

    [Benchmark(Description = nameof(Decimal), Baseline = true)]
    [BenchmarkCategory(Cat.Serialization)]
    public Stream Decimal_serialization()
    {
        Stream.Position = 0;
        JsonSerializer.Serialize(Stream, Decimals);
        return Stream;
    }

    [Benchmark(Description = nameof(Percentage))]
    [BenchmarkCategory(Cat.Serialization)]
    public Stream Percentage_serialization()
    {
        Stream.Position = 0;
        JsonSerializer.Serialize(Stream, Percentages);
        return Stream;
    }

    private static class Cat
    {
        public const string Serialization = nameof(Serialization);
    }
}
