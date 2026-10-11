using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bench;

[MemoryDiagnoser(true)]
[MinColumn]
public class JsonWriterBenchmark
{
    [IterationSetup]
    public void Setup()
    {
        Stream = new(4096);
        Writer = new Utf8JsonWriter(Stream);
    }

    [IterationCleanup]
    public void Cleanup()
    {
        Writer.Dispose();
        Stream.Dispose();
    }

    protected static JsonSerializerOptions Options(JsonConverter converter)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Strict)
        {
            WriteIndented = false,
        };
        options.Converters.Add(converter);
        return options;
    }

    protected static readonly JsonSerializerOptions Default = new(JsonSerializerDefaults.Strict)
    {
        WriteIndented = false,
    };

    protected MemoryStream Stream = null!;
    protected Utf8JsonWriter Writer = null!;
}
