using Qowaiv.DomainModel;

namespace Event_sourced_aggregate_specs;

public class Replays_events
{
    [Test]
    public void from_buffer()
    {
        var id = Guid.Parse("58B82A50-B906-4178-87EC-A8C31B49368B");
        var buffer = EventBuffer.Empty(id).Add(new NameUpdated("Jimi Hendrix"));

        var replayed = Aggregate.FromStorage<SimpleEventSourcedAggregate, Guid>(buffer);
        replayed.Should().BeEquivalentTo(new
        {
            Version = 1,
            Id = id,
        });
        replayed.Buffer.Should().ContainSingle();
    }
}

public class Applies_events
{
    [Test]
    public void changes_for_single_event()
    {
        var origin = new SimpleEventSourcedAggregate();
        var updated = origin.SetName("Jimi Hendrix").Should().BeValid().Value;

        updated.Name.Should().Be("Jimi Hendrix");
        updated.Buffer.Uncommitted.Should().BeEquivalentTo([new NameUpdated("Jimi Hendrix")]);
    }

    [Test]
    public void changes_for_multiple_events()
    {
        var origin = new SimpleEventSourcedAggregate();
        var updated = origin.SetPerson("Jimi Hendrix", new Date(1942, 11, 27)).Should().BeValid().Value;

        updated.Buffer.Uncommitted.Should().BeEquivalentTo(new object[]
        {
            new NameUpdated("Jimi Hendrix"),
            new DateOfBirthUpdated(new Date(1942, 11, 27)),
        });
    }

    [Test]
    public void as_uncommitted_to_the_buffer()
    {
        var origin = new SimpleEventSourcedAggregate();
        var updated =origin.SetName("Jimi Hendrix").Should().BeValid().Value;
        updated.Buffer.Uncommitted.Should().ContainSingle();
    }

    [Test]
    public void with_origin_unchanged()
    {
        var origin = new SimpleEventSourcedAggregate();
        origin.SetName("Jimi Hendrix");
        var updated = origin.SetName("Jimi Hendrix").Should().BeValid().Value;
        origin.Name.Should().BeNull();
        updated.Name.Should().Be("Jimi Hendrix");
    }

    [Test]
    public void skips_unknown_event_types()
    {
        var aggregate = new TestApplyChangeAggregate();
        var updated = aggregate.TestApplyChange(new NameUpdated("unknown")).Should().BeValid().Value;
        updated.Version.Should().Be(1);
    }

    [Test]
    public void pre_processed_in_applied_scope()
    {
        var aggregate = new TestPreProcessEventAggregate();
        var updated = aggregate
            .TestPreprocessEvents(new NameUpdated("Jimi Hendrix"), new NameUpdated("unknown"))
            .Should().BeValid().Value;

        updated.Buffer.Uncommitted.Should().BeEquivalentTo(new object[]
        {
            new NameUpdated("Jimi Hendrix"),
            new NameUpdated("Jimi Hendrix"),
        });
    }
}

internal class TestApplyChangeAggregate : Aggregate<TestApplyChangeAggregate, Guid>
{
    public TestApplyChangeAggregate()
        : base(Guid.NewGuid(), Qowaiv.Validation.Abstractions.Validator.Empty<TestApplyChangeAggregate>()) { }

    public Result<TestApplyChangeAggregate> TestApplyChange(object @event) => ApplyEvent(@event);
}

internal class TestPreProcessEventAggregate : Aggregate<TestPreProcessEventAggregate, Guid>
{
    private string Name = string.Empty;
    
    public TestPreProcessEventAggregate()
        : base(Guid.NewGuid(), Qowaiv.Validation.Abstractions.Validator.Empty<TestPreProcessEventAggregate>()) { }

    public Result<TestPreProcessEventAggregate> TestPreprocessEvents(params object[] events) => ApplyEvents(events);

    protected override object PreProcessEvent(object @event)
    {
        if (@event is not NameUpdated e)
        {
            return @event;
        }

        var newName = Name == string.Empty ? e.Name : Name;
        return new NameUpdated(newName);
    }


    internal void When(NameUpdated @event)
        => Name = @event.Name;
}
