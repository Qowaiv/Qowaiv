using Qowaiv.DomainModel;

namespace Pre_processing_events_specs;

internal class Pre_processing_events_specs
{
    private static readonly Guid TestId = Guid.Parse("4E54C133-E978-4B98-AFD8-3546044BA380");
    
    [Test]
    public void Executes_pre_processing_on_Apply()
    {
        var aggregate = new SimpleAggregate(TestId);
        var result = aggregate.Add1().Act(a => a.Add2());

        result.Should().BeValid().WithoutMessages()
            .Value.Buffer.Should().BeEquivalentTo(new object[]
            {
                new Event1(TestId),
                new Event2(TestId),
            });
    }

    [Test]
    public void Does_not_execute_pre_processing_on_Replay()
    {
        var buffer = EventBuffer.Empty(Guid.NewGuid())
            .Add(new Event1(Guid.NewGuid()));

        var aggregate = Aggregate.FromStorage<SimpleAggregate, Guid>(buffer);
        aggregate.Buffer.Cast<EventBase>()
            .Should().AllSatisfy(@event => @event.AggregateId.Should().NotBe(TestId));
    }
}

internal class SimpleAggregate(Guid aggregateId)
    : Aggregate<SimpleAggregate, Guid>(aggregateId, Qowaiv.Validation.Abstractions.Validator.Empty<SimpleAggregate>())
{
    public SimpleAggregate() : this(Guid.NewGuid()) { }

    public Result<SimpleAggregate> Add1() => ApplyEvent(new Event1(Guid.NewGuid()));
    public Result<SimpleAggregate> Add2() => ApplyEvent(new Event1(Guid.NewGuid()));

    protected override object PreProcessEvent(object @event)
        => @event is EventBase e
            ? e with { AggregateId = Id }
            : @event;
}

internal record EventBase(Guid AggregateId);
internal record Event1(Guid AggregateId) : EventBase(AggregateId);
internal record Event2(Guid AggregateId) : EventBase(AggregateId);
