using NUnit.Framework;
using Qowaiv.DomainModel.EventSourcing;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.TestTools;

namespace Qowaiv.DomainModel.UnitTests.EventSourcing
{
    public class EventTypeNotSupportedExceptionTest
    {
        [Test]
        public void Serialize_RoundTrip()
        {
            var exception = new EventTypeNotSupportedException(typeof(int), typeof(SimpleEntity));
            var actual = SerializationTest.SerializeDeserialize(exception);

            Assert.AreEqual(typeof(int), actual.EventType);
            Assert.AreEqual(typeof(SimpleEntity), actual.AggregateType);
        }
    }
}
