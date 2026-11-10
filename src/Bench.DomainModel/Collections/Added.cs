using Qowaiv.DomainModel.Collections;

namespace Benchmarks.Collections;

public sealed record Added(double Addition)
{
    public static IReadOnlyCollection<object> Random(int count)
    {
        var rnd = new Random(count);
        var events = new Added[count];
        for (int i = 0; i < count; i++)
        {
            events[i] = new Added(rnd.NextDouble());
        }
        return ImmutableCollection.Empty.AddRange(events);
    }
}
