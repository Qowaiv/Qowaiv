using Qowaiv.Json;
using System.IO;
using System.Text.Json;

namespace Bench;

public class YesNoBenchmark
{
    public class YesNo_write_JSON : JsonWriterBenchmark
    {
        [Benchmark(Baseline = true)]
        public Stream String()
        {
            JsonSerializer.Serialize(Writer, Data.YN.Raw, options: Default);
            return Stream;
        }

        [Benchmark]
        public Stream Bool()
        {
            JsonSerializer.Serialize(Writer, Data.YN.Bool, options: Default);
            return Stream;
        }

        [Benchmark]
        public Stream Qowaiv()
        {
            JsonSerializer.Serialize(Writer, Data.YN.Svo, options: Default);
            return Stream;
        }
    }

}
