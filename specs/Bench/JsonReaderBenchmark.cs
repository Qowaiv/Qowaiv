using BenchmarkDotNet.Columns;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bench;

[MemoryDiagnoser(true)]
[HideColumns(Column.StdDev, Column.Error, Column.Median, Column.RatioSD)]
public class JsonReaderBenchmark : IDisposable
{
    [IterationSetup]
    public void Setup()
    {
        Stream1.Position = 0;
        Stream2.Position = 0;
        Stream3.Position = 0;
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

    protected readonly MemoryStream Stream1 = new();
    protected readonly MemoryStream Stream2 = new();
    protected readonly MemoryStream Stream3 = new();

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Stream1.Dispose();
                Stream2.Dispose();
                Stream3.Dispose();
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
