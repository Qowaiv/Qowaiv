using BenchmarkDotNet.Jobs;
using System.IO;
using System.Text.Json;

namespace Bench;

public class PercentageBenchmark
{
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    public class Percentage_read_JSON : JsonReaderBenchmark
    {
        public Percentage_read_JSON()
        {
            JsonSerializer.Serialize(Stream1, Data.Percentage.String, options: Default);
            JsonSerializer.Serialize(Stream2, Data.Percentage.Decimal, options: Default);
            JsonSerializer.Serialize(Stream3, Data.Percentage.Svo, options: Default);
        }

        [Benchmark(Baseline = true)]
        public string[]? String()
            => JsonSerializer.Deserialize<string[]>(Stream1, options: Default);

        [Benchmark]
        public decimal[]? Decimal()
            => JsonSerializer.Deserialize<decimal[]>(Stream2, options: Default);

        [Benchmark]
        public Percentage[]? Percentage()
            => JsonSerializer.Deserialize<Percentage[]>(Stream3, options: Default);
    }

    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    public class Percentage_write_JSON : JsonWriterBenchmark
    {
        [Benchmark(Baseline = true)]
        public Stream Percentage()
        {
            JsonSerializer.Serialize(Writer1, Data.Percentage.Decimal, options: Default);
            return Stream1;
        }

        [Benchmark]
        public Stream YesNo()
        {
            JsonSerializer.Serialize(Writer2, Data.Percentage.Svo, options: Default);
            return Stream2;
        }
    }
}
