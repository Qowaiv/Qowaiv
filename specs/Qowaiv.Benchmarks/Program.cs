using BenchmarkDotNet.Running;

namespace Benchmarks;

public static class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<UuidBenchmark.Parse>();
        //BenchmarkRunner.Run<IbanBenchmark.ParseFormatted>();
    }
}
