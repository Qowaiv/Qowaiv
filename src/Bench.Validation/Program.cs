using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Benchmarks;

internal static class Program
{
    public static void Main(string[] args) => BenchmarkSwitcher
        .FromAssembly(typeof(Program).Assembly)
        .Run(
            args,
            ManualConfig.Create(DefaultConfig.Instance)
                .WithBuildTimeout(TimeSpan.FromMinutes(10))
                .WithOptions(ConfigOptions.DisableOptimizationsValidator));
}
