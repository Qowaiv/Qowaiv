using NUnit.Framework;
using Qowaiv.DomainModel.EventSourcing;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.Validation.Abstractions;
using System;

namespace Qowaiv.DomainModel.UnitTests.EventSourcing
{
    public class EventSourcedAggregateRootTest
    {
        [Test]
        public void FromEvents_NoFullHistory_Throws()
        {
            var stream = new EventStream();
            stream.Add(new SimpleInitEvent());
            stream.MarkAllAsCommitted();
            stream.Add(new UpdateNameEvent());

            Assert.Throws<EventStreamNoFullHistoryException>(() => AggregateRoot.FromEvents<SimpleEventSourcedRoot>(stream));
        }


        [Test]
        public void FromEvents_AggregateShouldHaveIdOfEvents()
        {
            var aggregateId = Guid.Parse("4BC26714-F8B9-4E88-8435-BA8383B5DFC8");
            var stream = EventStream.FromMessages(new[] { new EventMessage(new EventInfo(1, aggregateId, Clock.UtcNow()), new SimpleInitEvent()) });

            var aggregate = AggregateRoot.FromEvents<SimpleEventSourcedRoot>(stream).Value;

            Assert.AreEqual(aggregateId, aggregate.Id);
            Assert.AreEqual(aggregateId, aggregate.EventStream.AggregateId);
            Assert.AreEqual(1, aggregate.Version);
        }

        [Test]
        public void Ctor_NoParameters_SetsId()
        {
            var aggregate = new SimpleEventSourcedRoot();

            Assert.AreNotEqual(Guid.Empty, aggregate.Id);
            Assert.AreEqual(aggregate.Id, aggregate.EventStream.AggregateId);
            Assert.AreEqual(0, aggregate.Version);
        }

        [Test]
        public void ApplyEvent_SomeEvent_UpdatesAggregate()
        {
            var aggregate = new SimpleEventSourcedRoot();
            aggregate = aggregate.SetName(new UpdateNameEvent { Name = "Nelis Bijl" }).Value;

            Assert.AreEqual("Nelis Bijl", aggregate.Name);
            Assert.AreEqual(0, aggregate.EventStream.CommittedVersion);
            Assert.AreEqual(1, aggregate.Version);
        }

        [Test]
        public void ApplyEvent_NotSupported()
        {
            var aggregate = new TestApplyChangeAggregate();
            var exception = Assert.Throws<EventTypeNotSupportedException>(() => aggregate.TestApplyChange(new UpdateNameEvent()));
            Assert.AreEqual(typeof(UpdateNameEvent), exception.EventType);
        }


        private class TestApplyChangeAggregate : EventSourcedAggregateRoot<TestApplyChangeAggregate>
        {
            public TestApplyChangeAggregate()
                : base(Guid.NewGuid(), Validator.Empty<TestApplyChangeAggregate>()) { }

            public void TestApplyChange(object @event)
            {
                ApplyChange(@event);
            }
        }
    }
}
