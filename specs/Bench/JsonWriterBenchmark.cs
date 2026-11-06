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
        Stream1 = new MemoryStream(4096);
        Stream2 = new MemoryStream(4096);
        Writer1 = new(Stream1);
        Writer2 = new(Stream2);
    }

    [IterationSetup]
    public void Setup()
    {
        Stream1.Position = 0;
        Stream2.Position = 0;
        Writer1.Reset();
        Writer2.Reset();
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

    protected readonly MemoryStream Stream1;
    protected readonly MemoryStream Stream2;
    protected readonly Utf8JsonWriter Writer1;
    protected readonly Utf8JsonWriter Writer2;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Stream1.Dispose();
                Stream2.Dispose();
                Writer1.Dispose();
                Writer2.Dispose();
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
