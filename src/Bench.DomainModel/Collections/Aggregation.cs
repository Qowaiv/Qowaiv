namespace Benchmarks.Collections;

public class Aggregation
{
    [Params(100, 10_000)]
    public int Count { get; set; }

    private IReadOnlyCollection<object> Events = [];

    [GlobalSetup]
    public void Setup()
        => Events = Added.Random(Count);

    [Benchmark]
    public double Aggregate_ApplyEvent()
    {
        var aggregate = new Aggregate_id();
        foreach(var e in Events)
        {
            aggregate = aggregate.AddEvent(e);
        }
        return aggregate.Sum;
    }

    [Benchmark]
    public double Aggregate_ApplyEvents()
    {
        var aggregate = new Aggregate_id();
        foreach (var e in Events)
        {
            aggregate = aggregate.AddEvents(e);
        }
        return aggregate.Sum;
    }
}
