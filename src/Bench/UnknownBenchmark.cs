using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using System.Buffers;
using System.Collections.Frozen;

namespace Bench;

public class UnknownBenchmark
{
    private const int Iterations = 1_000_000;

    [HideColumns(Column.StdDev, Column.Error, Column.Median, Column.RatioSD)]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    public class Unknown_TryParse
    {
        private readonly FrozenSet<string> Frozen = new HashSet<string>(["?", "UNKNOWN", "NOT KNOWN", "NOTKNOWN"]).ToFrozenSet(StringComparer.OrdinalIgnoreCase);
        private readonly SearchValues<string> Search = System.Buffers.SearchValues.Create(["?", "UNKNOWN", "NOT KNOWN", "NOTKNOWN"], StringComparison.OrdinalIgnoreCase);

        [Benchmark(Baseline = true, OperationsPerInvoke = Iterations)]
        public bool FrozenSet()
        {
            var o = true;
            foreach (var v in Data.Unknown.Values)
                o ^= Frozen.Contains(v);
            return o;
        }

        [Benchmark(OperationsPerInvoke = Iterations)]
        public bool SearchValues()
        {
            var o = true;
            foreach (var v in Data.Unknown.Values)
                o ^= Search.Contains(v);
            return o;
        }
    }
}
