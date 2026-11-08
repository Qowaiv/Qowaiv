using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bench;

[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.Net11_0)]
public class ModifyTypeInfoBenchmark
{
    private readonly Func<object, bool> useEquals = (o) => !o.Equals(EmailAddress.Empty);
    private readonly Func<object, bool> useCast = (o) => ((EmailAddress)o) != default;
    private readonly Func<object, bool> useInteface = (o) => o is not IEmpty { HasValue: false };

    [Benchmark(Baseline = true)]
    public void UseEquals()
    {
        foreach (var svo in Data.ModifyTypeInfo.Svo) _ = useEquals(svo);
    }

    [Benchmark]
    public void UseCast()
    {
        foreach (var svo in Data.ModifyTypeInfo.Svo) _ = useCast(svo);
    }

    [Benchmark]
    public void UseInteface()
    {
        foreach (var svo in Data.ModifyTypeInfo.Svo) _ = useInteface(svo);
    }
}
