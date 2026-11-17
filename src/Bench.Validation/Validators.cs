using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Qowaiv.Validation.Abstractions;
using Qowaiv.Validation.DataAnnotations;
using Qowaiv.Validation.TestData;

namespace Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByParams)]
[HideColumns(Column.StdDev, Column.Error, Column.Median, Column.RatioSD)]
public  partial class Validators
{
    internal static readonly IDictionary<string, string[]> Empty = new Dictionary<string, string[]>();

    [SimpleJob(RuntimeMoniker.Net10_0)]
    [SimpleJob(RuntimeMoniker.Net11_0)]
    public partial class Validate_small
    {
        internal readonly BenchmarkModelValidator FluentValidator = new();
        internal BenchmarkModel Model = new();


        [IterationSetup]
        public void Setup() => Model = BenchmarkModel.New(Count);

        [Params(10, 100, 1000, 10_000)]
        public int Count { get; set; }

        [Benchmark]
        public Result Qowaiv() => AnnotatedModelValidator.Validate(Model);


        [Benchmark]
        public FluentValidation.Results.ValidationResult Fluent() => FluentValidator.Validate(Model);

        [Benchmark(Baseline = true)]
        public IDictionary<string, string[]> MiniValidator()
            => MiniValidation.MiniValidator.TryValidate(Model, out var errors)
            ? Empty
            : errors;


    }

}
