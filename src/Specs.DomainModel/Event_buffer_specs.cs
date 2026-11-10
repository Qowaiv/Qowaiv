using Qowaiv.DomainModel;
using TUnit.Core.Executors;

namespace Event_buffer_specs;

public class Creation
{
    [Test]
    public void Empty_has_version_0()
        => EventBuffer.Empty(Guid.NewGuid()).Version.Should().Be(0);

    [Test]
    [Arguments(1)]
    [Arguments(17)]
    [Arguments(666)]
    public void Empty_with_initial_has_equals(int version)
        => EventBuffer.Empty(Guid.NewGuid(), version).Version.Should().Be(version);

    [Test]
    public void FromStorage_contains_all_events_as_committed()
    {
        var stored = new[] { new EmptyEvent(), new EmptyEvent(), new EmptyEvent() };
        var buffer = EventBuffer.FromStorage(Guid.NewGuid(), stored, (@event) => @event);

        buffer.Should().HaveCount(3)
            .And.Subject.As<EventBuffer<Guid>>().CommittedVersion.Should().Be(3);
    }

    [Test]
    public void FromStorage_takes_initial_version_into_account()
    {
        var stored = new[] { new EmptyEvent(), new EmptyEvent(), new EmptyEvent() };
        var buffer = EventBuffer.FromStorage(Guid.NewGuid(), initialVersion: 6, stored, (@event) => @event);

        buffer.Should().HaveCount(3)
         .And.Subject.As<EventBuffer<Guid>>().CommittedVersion.Should().Be(9);
    }
}

public class Select_uncommitted
{
    private const string My_id_007 = nameof(My_id_007);

    [Test]
    public void convert_uses_version_according_to_buffer()
    {
        var buffer = EventBuffer.Empty(Guid.NewGuid())
            .Add(new EmptyEvent())
            .Add(new EmptyEvent())
            .Add(new EmptyEvent())
            .MarkAllAsCommitted()
            .Add(new EmptyEvent())
            .Add(new EmptyEvent());

        var selected = buffer.SelectUncommitted((_, version, _) => version);

        selected.Should().HaveCount(2)
            .And.BeEquivalentTo([4, 5]);
    }

    [Test]
    public void convert_uses_aggregate_id()
    {
        var buffer = EventBuffer.Empty(My_id_007)
            .Add(new EmptyEvent())
            .Add(new EmptyEvent())
            .Add(new EmptyEvent());

        var selected = buffer.SelectUncommitted((id, version, @event) => new StoredEvent(id, version, @event));
        selected.Should().BeEquivalentTo(
        [
            new StoredEvent(My_id_007, 1, new EmptyEvent()),
            new StoredEvent(My_id_007, 2, new EmptyEvent()),
            new StoredEvent(My_id_007, 3, new EmptyEvent()),
        ]);
    }
}

public class IsEmpty
{
    [Test]
    public void True_for_empty_buffer() => EventBuffer.Empty(17).IsEmpty.Should().BeTrue();

    [Test]
    public void True_for_empty_buffer_with_offset() => EventBuffer.Empty(17, version: 2000).IsEmpty.Should().BeTrue();

    [Test]
    public void False_for_non_empty_buffer() => EventBuffer.Empty(17).Add(new EmptyEvent()).IsEmpty.Should().BeFalse();
}

public class Default
{
    [Test]
    public void can_be_queried()
        => default(EventBuffer<int>).Should().BeEquivalentTo(Array.Empty<object>());

    [Test]
    public void Committed_can_be_queried()
        => default(EventBuffer<int>).Committed.Should().BeEquivalentTo(Array.Empty<object>());

    [Test]
    public void Uncommitted_can_be_queried_with_Take()
        => default(EventBuffer<int>).Uncommitted.Should().BeEquivalentTo(Array.Empty<object>());

    [Test]
    public void can_be_extended()
        => default(EventBuffer<int>).Add(1).Should().BeEquivalentTo([1]);
}

public class HasUncommitted
{
    [Test]
    public void False_for_empty_buffer() => EventBuffer.Empty(17).HasUncommitted.Should().BeFalse();

    [Test]
    public void True_for_non_empty_buffer() => EventBuffer.Empty(17).Add(new EmptyEvent()).HasUncommitted.Should().BeTrue();

    [Test]
    public void False_for_non_empty_buffer_all_marked_as_committed()
        => EventBuffer.Empty(17).Add(new EmptyEvent()).MarkAllAsCommitted().HasUncommitted.Should().BeFalse();
}

public class Debugger_display
{
    [Test]
    public void with_uncommitted()
    {
        object buffer = EventBuffer.Empty(Guid.Parse("1F8B5071-C03B-457D-B27F-442C5AAC5785"))
            .Add(new EmptyEvent());

        buffer.Should().HaveDebuggerDisplay("Version: 1 (Committed: 0), Aggregate: 1f8b5071-c03b-457d-b27f-442c5aac5785");
    }

    [Test]
    public void committed_only()
    {
        object buffer = EventBuffer.Empty(Guid.Parse("1F8B5071-C03B-457D-B27F-442C5AAC5785"))
            .Add(new EmptyEvent())
            .MarkAllAsCommitted();

        buffer.Should().HaveDebuggerDisplay("Version: 1, Aggregate: 1f8b5071-c03b-457d-b27f-442c5aac5785");
    }
}

public class Implements_ICollection
{
    [Test]
    public void Count_is_equal_to_buffer_count()
    {
        var collection = EventBuffer.Empty(17).Add(new[] { 1, 2, 3, 4 });
        collection.Should().HaveCount(4);
    }

    [Test]
    public void returns_true_for_contained_events()
    {
        var @event = new EmptyEvent();
        var buffer = EventBuffer.Empty(17).Add(@event);
        buffer.Contains(@event).Should().BeTrue();
    }

    [Test]
    public void returns_false_for_contained_events()
    {
        var buffer = EventBuffer.Empty(17).Add(new EmptyEvent());
        buffer.Contains(new object()).Should().BeFalse();
    }

    [Test]
    public void Copies_events_to_other_array()
    {
        var @event0 = new EmptyEvent();
        var @event1 = new EmptyEvent();
        var buffer = EventBuffer.Empty(17).Add(@event0).Add(event1);

        var array = new object[5];

        buffer.CopyTo(array, 1);

        array.Should().BeEquivalentTo(new object?[]
        {
            null,
            event0,
            event1,
            null,
            null
        });
    }

    [Test]
    public void Copies_nothing_when_empty()
    {
        var buffer = default(EventBuffer<int>);

        var array = new object[5];

        buffer.CopyTo(array, 1);

        array.Should().BeEquivalentTo(new object?[]
        {
            null,
            null,
            null,
            null,
            null
        });
    }

    /// <remarks>
    /// This is only exposed when explicitly <see cref="ICollection{object}"/>.
    /// </remarks>
    public class Explicitly
    {
        [Test]
        public void Is_read_only()
        {
            ICollection<object> collection = EventBuffer.Empty(17);
            collection.IsReadOnly.Should().BeTrue();
        }

        public class Does_not_support
        {
            [Test]
            public void Add()
            {
                ICollection<object> collection = EventBuffer.Empty(17);
                collection.Invoking(c => c.Add(new EmptyEvent()))
                    .Should().Throw<NotSupportedException>();
            }

            [Test]
            public void Remove()
            {
                ICollection<object> collection = EventBuffer.Empty(17);
                collection.Invoking(c => c.Remove(new EmptyEvent()))
                    .Should().Throw<NotSupportedException>();
            }

            [Test]
            public void Clear()
            {
                ICollection<object> collection = EventBuffer.Empty(17);
                collection.Invoking(c => c.Clear())
                    .Should().Throw<NotSupportedException>();
            }
        }
    }
}
