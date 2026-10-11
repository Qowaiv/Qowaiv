using Qowaiv.Json;
using System.IO;
using System.Text.Json;

namespace Bench;

public class YesNoBenchmark
{
    public class YesNo_JSON : JsonWriterBenchmark
    {
        private static readonly JsonSerializerOptions qowaiv = Options(new YesNoJsonConverter());

        [Benchmark(Baseline = true)]
        public Stream String()
        {
            JsonSerializer.Serialize(Writer, Data.YN.Raw, options: Default);
            Writer.Flush();
            return Stream;
        }

        [Benchmark]
        public Stream Bool()
        {
            JsonSerializer.Serialize(Writer, Data.YN.Bools, options: Default);
            Writer.Flush();
            return Stream;
        }

        [Benchmark]
        public Stream Qowaiv()
        {
            JsonSerializer.Serialize(Writer, Data.YN.Typed, options: qowaiv);
            Writer.Flush();
            return Stream;
        }
    }

}
