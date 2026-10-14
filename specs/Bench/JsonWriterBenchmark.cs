using BenchmarkDotNet.Columns;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bench;

[MemoryDiagnoser(true)]
[HideColumns(Column.StdDev, Column.Error, Column.Median, Column.RatioSD, Column.AllocRatio)]
public class JsonWriterBenchmark : IDisposable
{
    public JsonWriterBenchmark()
    {
        Stream = new MemoryStream(4096);
        Writer = new(Stream);
    }

    [IterationSetup]
    public void Setup()
    {
        Stream.Position = 0;
        Writer.Reset();
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

    protected readonly MemoryStream Stream;
    protected readonly Utf8JsonWriter Writer;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Stream.Dispose();
                Writer.Dispose();
            }
            disposed = true;
        }
    }

    private bool disposed;

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
