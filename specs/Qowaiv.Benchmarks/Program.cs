﻿using BenchmarkDotNet.Running;

namespace Benchmarks;

public static class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<UuidBenchmark.Parse>();
        BenchmarkRunner.Run<UuidBenchmark.StringOutput>();
        BenchmarkRunner.Run<UuidBenchmark.Version>();
        BenchmarkRunner.Run<IbanBenchmark.ParseUnformatted>();
        BenchmarkRunner.Run<IbanBenchmark.ParseFormatted>();
    }
}
