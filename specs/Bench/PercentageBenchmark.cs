using BenchmarkDotNet.Configs;
using System.IO;
using System.Text.Json;

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
    private readonly Stream Write = new MemoryStream();
    private readonly Stream ReadDecimal = new MemoryStream();
    private readonly Stream ReadPercentage = new MemoryStream();

    public PercentageBenchmark()
    {
        var rnd = new MersenneTwister(Iterations);
        for(var i = 0; i< Iterations; i++)
        {
            var nr = (rnd.NextDecimal() * rnd.NextDecimal() * 200m).Round(2);
            Decimals[i] = nr;
            Percentages[i] = nr.Percent();
        }

        JsonSerializer.Serialize(ReadDecimal, Decimals);
        JsonSerializer.Serialize(ReadPercentage, Percentages);
    }

    [Benchmark(Description = nameof(Decimal), Baseline = true)]
    [BenchmarkCategory(Cat.Serialization)]
    public Stream Decimal_serialization()
    {
        Write.Position = 0;
        JsonSerializer.Serialize(Write, Decimals);
        return Write;
    }

    [Benchmark(Description = nameof(Percentage))]
    [BenchmarkCategory(Cat.Serialization)]
    public Stream Percentage_serialization()
    {
        Write.Position = 0;
        JsonSerializer.Serialize(Write, Percentages);
        return Write;
    }


    [Benchmark(Description = nameof(Decimal), Baseline = true)]
    [BenchmarkCategory(Cat.Deserialization)]
    public decimal[]? Decimal_deserialization()
    {
        ReadDecimal.Position = 0;
        return JsonSerializer.Deserialize<decimal[]>(ReadDecimal);
    }

    [Benchmark(Description = nameof(Percentage))]
    [BenchmarkCategory(Cat.Deserialization)]
    public Percentage[]? Percentage_deserialization()
    {
        ReadPercentage.Position = 0;
        return JsonSerializer.Deserialize<Percentage[]>(ReadPercentage);
    }

    private static class Cat
    {
        public const string Serialization = nameof(Serialization);
        public const string Deserialization = nameof(Deserialization);
    }
}
