using BenchmarkDotNet.Jobs;
using System.IO;
using System.Text.Json;

namespace Bench;

public class DateBenchmark
{
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    public class Date_read_JSON : JsonReaderBenchmark
    {
        public Date_read_JSON()
            => JsonSerializer.Serialize(Stream, Data.Date.Svo, options: Default);

        [Benchmark(Baseline = true)]
        public DateOnly[]? DateOnly()
            => JsonSerializer.Deserialize<DateOnly[]>(Stream, options: Default);

        [Benchmark]
        public Date[]? Date()
            => JsonSerializer.Deserialize<Date[]>(Stream, options: Default);
    }

    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    public class Date_write_JSON : JsonWriterBenchmark
    {
        [Benchmark(Baseline = true)]
        public Stream DateOnly()
        {
            JsonSerializer.Serialize(Writer, Data.Date.DateOnly, options: Default);
            return Stream;
        }

        [Benchmark]
        public Stream Date()
        {
            JsonSerializer.Serialize(Writer, Data.Date.Svo, options: Default);
            return Stream;
        }
    }
}
