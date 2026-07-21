using BenchmarkDotNet.Configs;
using Qowaiv.Financial;
using System.IO;
using System.Text.Json;

namespace Bench;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[MemoryDiagnoser(true)]
[MinColumn]
public class AmountBenchmark
{
    private const int Iterations = 1000;
    private readonly decimal[] Decimals = new decimal[Iterations];
    private readonly Amount[] SVOs = new Amount[Iterations];
    private readonly Stream Write = new MemoryStream();
    private readonly Stream ReadDecimal = new MemoryStream();
    private readonly Stream ReadSvo = new MemoryStream();

    public AmountBenchmark()
    {
        var rnd = new MersenneTwister(Iterations);
        for(var i = 0; i< Iterations; i++)
        {
            var nr = (rnd.NextDecimal() * rnd.NextDecimal() * 50_000m).Round(2);
            Decimals[i] = nr;
            SVOs[i] = nr.Amount();
        }

        JsonSerializer.Serialize(ReadDecimal, Decimals);
        JsonSerializer.Serialize(ReadSvo, SVOs);
    }

    [Benchmark(Description = nameof(Decimal), Baseline = true)]
    [BenchmarkCategory(Cat.Serialization)]
    public Stream Decimal_serialization()
    {
        Write.Position = 0;
        JsonSerializer.Serialize(Write, Decimals);
        return Write;
    }

    [Benchmark(Description = nameof(Amount))]
    [BenchmarkCategory(Cat.Serialization)]
    public Stream SVO_serialization()
    {
        Write.Position = 0;
        JsonSerializer.Serialize(Write, SVOs);
        return Write;
    }


    [Benchmark(Description = nameof(Decimal), Baseline = true)]
    [BenchmarkCategory(Cat.Deserialization)]
    public decimal[]? Decimal_deserialization()
    {
        ReadDecimal.Position = 0;
        return JsonSerializer.Deserialize<decimal[]>(ReadDecimal);
    }

    [Benchmark(Description = nameof(Amount))]
    [BenchmarkCategory(Cat.Deserialization)]
    public Amount[]? SVO_deserialization()
    {
        ReadSvo.Position = 0;
        return JsonSerializer.Deserialize<Amount[]>(ReadSvo);
    }

    private static class Cat
    {
        public const string Serialization = nameof(Serialization);
        public const string Deserialization = nameof(Deserialization);
    }
}
