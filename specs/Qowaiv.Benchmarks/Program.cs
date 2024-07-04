using BenchmarkDotNet.Running;

namespace Benchmarks;

public static class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<DecimalBenchmark>();

        BenchmarkRunner.Run<IbanBenchmark.ParseUnformatted>();
        BenchmarkRunner.Run<IbanBenchmark.ParseFormatted>();

        BenchmarkRunner.Run<RoundBenchmark>();

        BenchmarkRunner.Run<UuidBenchmark.Parse>();
        BenchmarkRunner.Run<UuidBenchmark.StringOutput>();
        BenchmarkRunner.Run<UuidBenchmark.Version>();
    }
}
