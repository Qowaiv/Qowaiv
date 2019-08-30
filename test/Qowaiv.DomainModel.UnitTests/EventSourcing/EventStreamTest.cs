using NUnit.Framework;
using Qowaiv.DomainModel.EventSourcing;
using Qowaiv.TestTools;
using System;
using System.Linq;

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
        public void FromMessages_Null_Throws()
        {
            var x = Assert.Throws<ArgumentNullException>(() => EventStream.FromMessages(null));
            Assert.AreEqual("events", x.ParamName);
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

        [Test]
        public void GetUncommitted_NoCommitted_All()
        {
            var stream = new EventStream
            {
                new DummyEvent(),
                new DummyEvent(),
                new DummyEvent()
            };
            Assert.AreEqual(3, stream.GetUncommitted().Count());
        }

        [Test]
        public void GetUncommitted_3Committed_1ItemWithVersion4()
        {
            var stream = new EventStream
            {
                new DummyEvent(),
                new DummyEvent(),
                new DummyEvent()
            };
            stream.MarkAllAsCommitted();
            stream.Add(new DummyEvent());

            var uncommited = stream.GetUncommitted().ToArray();

            Assert.AreEqual(1, uncommited.Length);
            Assert.AreEqual(4, uncommited[0].Info.Version);
        }

        [Test]
        public void GetUncommitted_ClearCommitted_1ItemWithVersion4()
        {
            var stream = new EventStream
            {
                new DummyEvent(),
                new DummyEvent(),
                new DummyEvent()
            };
            stream.MarkAllAsCommitted();
            stream.ClearCommitted();
            stream.Add(new DummyEvent());

            var uncommited = stream.GetUncommitted().ToArray();

            Assert.AreEqual(1, uncommited.Length);
            Assert.AreEqual(4, uncommited[0].Info.Version);
        }

        [Test]
        public void GetUncommitted_ClearCommittedWithCommitted_1ItemWithVersion5()
        {
            var stream = new EventStream
            {
                new DummyEvent(),
                new DummyEvent(),
                new DummyEvent()
            };
            stream.MarkAllAsCommitted();
            stream.ClearCommitted();
            stream.Add(new DummyEvent());
            stream.MarkAllAsCommitted();
            stream.Add(new DummyEvent());

            var uncommited = stream.GetUncommitted().ToArray();

            Assert.AreEqual(1, uncommited.Length);
            Assert.AreEqual(5, uncommited[0].Info.Version);
        }

        [Test]
        public void ClearCommitted_WithoutUncommitted()
        {
            var stream = new EventStream();
            for (var i = 0; i < 16; i++)
            {
                stream.Add(new DummyEvent());
            }
            stream.MarkAllAsCommitted();
            stream.ClearCommitted();

            Assert.AreEqual(16, stream.Version);
            Assert.AreEqual(16, stream.CommittedVersion);
            Assert.IsEmpty(stream.GetUncommitted());
        }

        [Test]
        public void ClearCommitted_WithUncommitted()
        {
            var stream = new EventStream();
            for (var i = 0; i < 16; i++)
            {
                stream.Add(new DummyEvent());
            }
            stream.MarkAllAsCommitted();
            stream.ClearCommitted();

            stream.Add(new DummyEvent());

            Assert.AreEqual(17, stream.Version);
            Assert.AreEqual(16, stream.CommittedVersion);
            Assert.AreEqual(1, stream.GetUncommitted().Count());
            Assert.AreEqual(1, stream.Count());
        }

        [Test]
        public void DebuggerDisplay_WithUncommited()
        {
            using (Clock.SetTimeForCurrentThread(() => new DateTime(2017, 06, 11)))
            {
                var stream = new EventStream(Guid.Parse("1F8B5071-C03B-457D-B27F-442C5AAC5785"));
                stream.Add(new DummyEvent());

                DebuggerDisplayAssert.HasResult("Version: 1 (Committed: 0), Aggregate: {1f8b5071-c03b-457d-b27f-442c5aac5785}", stream);
            }
        }

        [Test]
        public void DebuggerDisplay_WithoutUncommited()
        {
            using (Clock.SetTimeForCurrentThread(() => new DateTime(2017, 06, 11)))
            {
                var stream = new EventStream(Guid.Parse("1F8B5071-C03B-457D-B27F-442C5AAC5785"));
                stream.Add(new DummyEvent());
                stream.MarkAllAsCommitted();

                DebuggerDisplayAssert.HasResult("Version: 1, Aggregate: {1f8b5071-c03b-457d-b27f-442c5aac5785}", stream);
            }
        }

        private class DummyEvent { }
    }
}
