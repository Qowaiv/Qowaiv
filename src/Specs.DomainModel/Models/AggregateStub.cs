using Qowaiv.DomainModel;
using System.Diagnostics.Contracts;

namespace Models;

internal sealed class AggregateStub : Aggregate<AggregateStub, int>
{
    public AggregateStub()
        : base(0, Qowaiv.Validation.Abstractions.Validator.Empty<AggregateStub>()) { }

    [Pure]
    public AggregateStub Add(params object[] events)
        => ApplyEvents(events).Value;
}
