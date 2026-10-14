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
    public void Setup() => Stream.Position = 0;

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

    protected readonly MemoryStream Stream = new();
    
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Stream.Dispose();
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
