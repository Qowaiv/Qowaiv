using BenchmarkDotNet.Jobs;
using System.IO;
using System.Text.Json;

namespace Bench;

public class YesNoBenchmark
{
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    public class YesNo_read_JSON : JsonReaderBenchmark
    {
        public YesNo_read_JSON()
        {
            JsonSerializer.Serialize(Stream1, Data.YN.String, options: Default);
            JsonSerializer.Serialize(Stream2, Data.YN.Bool, options: Default);
            JsonSerializer.Serialize(Stream3, Data.YN.Svo, options: Default);
        }

        [Benchmark(Baseline = true)]
        public string[]? String()
            => JsonSerializer.Deserialize<string[]>(Stream1, options: Default);

        [Benchmark]
        public bool?[]? Bool()
            => JsonSerializer.Deserialize<bool?[]>(Stream2, options: Default);

        [Benchmark]
        public YesNo[]? YesNo()
            => JsonSerializer.Deserialize<YesNo[]>(Stream3, options: Default);
    }

    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    public class YesNo_write_JSON : JsonWriterBenchmark
    {
        [Benchmark(Baseline = true)]
        public Stream String()
        {
            JsonSerializer.Serialize(Writer, Data.YN.String, options: Default);
            return Stream;
        }

        [Benchmark]
        public Stream Bool()
        {
            JsonSerializer.Serialize(Writer, Data.YN.Bool, options: Default);
            return Stream;
        }

        [Benchmark]
        public Stream YesNo()
        {
            JsonSerializer.Serialize(Writer, Data.YN.Svo, options: Default);
            return Stream;
        }
    }

}
