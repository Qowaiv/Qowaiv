using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using System.Diagnostics.Tracing;

namespace Bench;

[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.Net11_0)]
[MemoryDiagnoser(true)]
[HideColumns(Column.StdDev, Column.Error, Column.Median, Column.RatioSD, Column.AllocRatio)]
public class AnsiBenchmark
{
    [Benchmark]
    public void String()
    {
        foreach (var ansi in Data.Ansis.Svo)
            _ = ansi.ToString();
    }

    [Benchmark(Baseline = true)]
    public void TryFormat()
    {
        Span<char> span = stackalloc char[32];

        foreach (var ansi in Data.Ansis.Svo)
            ansi.TryFormat(span, out var _, default, null);
    }
}
