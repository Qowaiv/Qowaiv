namespace Benchmarks.Collections;

[MemoryDiagnoser]
public class Creation
{
    [Params(1000, 10_000, 100_000, 200_000)]
    public int Count { get; set; }

    public IReadOnlyCollection<object> Events { get; private set; } = [];

    [GlobalSetup]
    public void Setup()
        => Events = Added.Random(Count);

    [Benchmark(Baseline = true)]
#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions
    public List<object> List()
    {
        var list = new List<object>();

        foreach (var e in Events)
        {
            if (e is not null)
            {
                list.Add(e);
            }
        }
        return list;
    }
#pragma warning restore S3267 // Loops should be simplified with "LINQ" expressions

    [Benchmark]
    public List<object> List_with_where_clause()
    {
        var list = new List<object>();

        foreach (var e in Events.Where(e => e is { }))
        {
            list.Add(e);
        }
        return list;
    }

    [Benchmark(Description = "Event Buffer")]
    public EventBuffer<int> EventBuffer()
    {
        var buffer = Qowaiv.DomainModel.EventBuffer.Empty(17);
        foreach (var e in Events)
        {
            buffer = buffer.Add(e);
        }
        return buffer;
    }
}
