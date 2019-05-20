using NUnit.Framework;
using Qowaiv.DomainModel.EventSourcing;
using Qowaiv.DomainModel.UnitTests.Models;
using System;

namespace Qowaiv.DomainModel.UnitTests.EventSourcing
{
    public class EventSourcedAggregateRootTest
    {
        [Test]
        public void FromEventStream_AggregateShouldHaveIdOfEvents()
        {
            var aggregateId = Guid.Parse("4BC26714-F8B9-4E88-8435-BA8383B5DFC8");
            var stream = EventStream.FromMessages(new[] { new EventMessage(new EventInfo(1, aggregateId, Clock.UtcNow()), new SimpleInitEvent()) });

            var aggregate = AggregateRoot.FromEvents<SimpleEventSourcedRoot>(stream).Value;

            Assert.AreEqual(aggregateId, aggregate.Id);
        }
    }
}
