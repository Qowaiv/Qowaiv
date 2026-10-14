using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using System.IO;
using System.Text.Json;

namespace Bench;

public class DateBenchmark
{
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    [HideColumns(Column.StdDev, Column.Error, Column.Median, Column.RatioSD)]
    public class Date_JSON : JsonWriterBenchmark
    {
        [Benchmark(Baseline = true)]
        public Stream Baseline()
        {
            JsonSerializer.Serialize(Writer, Data.Date.DateOnly, options: Default);
            Writer.Flush();
            return Stream;
        }

        [Benchmark]
        public Stream Qowaiv()
        {
            JsonSerializer.Serialize(Writer, Data.Date.Typed, options: Default);
            Writer.Flush();
            return Stream;
        }
    }
}
