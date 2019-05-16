using NUnit.Framework;
using Qowaiv.DomainModel.EventSourcing;
using System;

namespace Qowaiv.DomainModel.UnitTests.EventSourcing
{
    public class EventStreamTest
    {
        [Test]
        public void FromMessages_CreatedSuccesfully()
        {
            var id = Guid.Parse("7231B710-77CD-11E9-8F9E-2A86E4085A59");

            var messages = new[] 
            {
                new EventMessage(new EventInfo(1, id, Clock.UtcNow()), new DummyEvent()),
                new EventMessage(new EventInfo(2, id, Clock.UtcNow()), new DummyEvent()),
            };

            var stream = EventStream.FromMessages(messages);

            Assert.IsNotNull(stream);
            Assert.AreEqual(id, stream.AggregateId);
            Assert.AreEqual(2, stream.Version);
        }

        [Test]
        public void FromMessages_WithDifferentGuids_Throws()
        {
            var messages = new[]
            {
                new EventMessage(new EventInfo(1, Guid.NewGuid(), Clock.UtcNow()), new DummyEvent()),
                new EventMessage(new EventInfo(2, Guid.NewGuid(), Clock.UtcNow()), new DummyEvent()),
            };

            var x = Assert.Throws<ArgumentException>(() => EventStream.FromMessages(messages));
            Assert.AreEqual("events", x.ParamName);
        }

        [Test]
        public void FromMessages_WithMissingVersion_Throws()
        {
            var id = Guid.Parse("7231B710-77CD-11E9-8F9E-2A86E4085A59");

            var messages = new[]
            {
                new EventMessage(new EventInfo(1, id, Clock.UtcNow()), new DummyEvent()),
                new EventMessage(new EventInfo(3, id, Clock.UtcNow()), new DummyEvent()),
            };

            Assert.Throws<EventsOutOfOrderException>(() => EventStream.FromMessages(messages));
        }


        private class DummyEvent { }
    }
}
