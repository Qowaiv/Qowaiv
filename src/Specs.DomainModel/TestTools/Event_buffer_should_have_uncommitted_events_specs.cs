using Qowaiv.DomainModel;
using TUnit.Core.Executors;

namespace TestTools.Event_buffer_should_have_uncommitted_events_specs;

public class Ensures
{
    [Test]
    public void Structure_of_uncommitted_events()
    {
        var buffer = EventBuffer.Empty(Guid.NewGuid()).Add(new object[]
        {
            new EmptyEvent(),
            new SimpleEvent(3),
            new OtherEvent(3),
        });

        buffer.Should().HaveUncommittedEvents(
            new EmptyEvent(),
            new SimpleEvent(3),
            new OtherEvent(3)
        );
    }

    [Test]
    public void Structure_of_uncommitted_events_containing_arrays()
    {
        var buffer = EventBuffer.Empty(Guid.NewGuid())
            .Add(new ArrayEvent { Numbers = [17] });

        buffer.Should().HaveUncommittedEvents(
           new ArrayEvent { Numbers = [17] });
    }
}

public class Fails_on
{
    [Test]
    public void No_uncommitted_events()
    {
        var buffer = EventBuffer.Empty(Guid.NewGuid());

        buffer
            .Invoking(b => b.Should().HaveUncommittedEvents(buffer, new EmptyEvent()))
            .Should().Throw<Qowaiv.DomainModel.TestTools.AssertionFailed>()
            .WithMessage("There where no uncommitted events.");
    }

    [Test]
    public void Different_event_types()
    {
        var buffer = EventBuffer.Empty(Guid.NewGuid())
            .Add(new object[]
            {
                new EmptyEvent(),
                new SimpleEvent(1),
                new SimpleEvent(2)
            });

        buffer.Invoking(b => b.Should().HaveUncommittedEvents(
                new EmptyEvent(),
                new OtherEvent(1),
                new SimpleEvent(2)
            ))
            .Should().Throw<Qowaiv.DomainModel.TestTools.AssertionFailed>()
            .WithMessage(@"The uncommitted events where different than expected.
[0] EmptyEvent
[1] Expected: Models.Events.OtherEvent
    Actual:   Models.Events.SimpleEvent
[2] SimpleEvent
");
    }

    [Test]
    [InvariantCulture]
    public void Different_event_values()
    {
        var buffer = EventBuffer.Empty(Guid.NewGuid())
            .Add(new object[]
            {
                new EmptyEvent(),
                new ComplexEvent(17, "Same", new Date(2017, 06, 11)),
                new SimpleEvent(0),
            });

        buffer.Invoking(b => b.Should().HaveUncommittedEvents(
                new EmptyEvent(),
                new ComplexEvent(23, "Same", new Date(1980, 06, 30)),
                new SimpleEvent(0)
        ))
        .Should().Throw<Qowaiv.DomainModel.TestTools.AssertionFailed>()
        .WithMessage(@"The uncommitted events where different than expected.
[0] EmptyEvent
[1] Expected: { Value: 23, Date: 06/30/1980 00:00:00 }
    Actual:   { Value: 17, Date: 06/11/2017 00:00:00 }
[2] SimpleEvent
");
    }

    [Test]
    public void extra_events()
    {
        var buffer = EventBuffer.Empty(Guid.NewGuid())
            .Add(new object[] {
                new EmptyEvent(),
                new SimpleEvent(3),
                new OtherEvent(3)
            });

        buffer.Invoking(b => b.Should().HaveUncommittedEvents(
                new EmptyEvent(),
                new SimpleEvent(3)
        ))
        .Should().Throw<Qowaiv.DomainModel.TestTools.AssertionFailed>()
        .WithMessage(@"The uncommitted events where different than expected.
[0] EmptyEvent
[1] SimpleEvent
[2] Extra:   OtherEvent
");
    }

    [Test]
    public void missing_events()
    {
        var buffer = EventBuffer.Empty(Guid.NewGuid())
           .Add(new object[] {
                new EmptyEvent(),
                new OtherEvent(3),
           });

        buffer.Invoking(b => b.Should().HaveUncommittedEvents(
            new EmptyEvent(),
            new OtherEvent(3),
            new SimpleEvent(5),
            new OtherEvent(9)
        ))
        .Should().Throw<Qowaiv.DomainModel.TestTools.AssertionFailed>()
        .WithMessage(@"The uncommitted events where different than expected.
[0] EmptyEvent
[1] OtherEvent
[2] Missing:  SimpleEvent
[3] Missing:  OtherEvent
");
    }

    [Test]
    public void different_messages()
    {
        var buffer = EventBuffer.Empty(Guid.NewGuid())
          .Add(new ArrayEvent { Numbers = [17] });

        buffer.Invoking(b => b.Should().HaveUncommittedEvents(new ArrayEvent { Numbers = [18] }))
        .Should().Throw<Qowaiv.DomainModel.TestTools.AssertionFailed>()
        .WithMessage(@"The uncommitted events where different than expected.
[0] Expected: { Numbers: [ 18 ] }
    Actual:   { Numbers: [ 17 ] }
");
    }
}
