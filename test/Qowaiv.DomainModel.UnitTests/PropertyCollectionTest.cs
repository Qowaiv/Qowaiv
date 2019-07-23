using NUnit.Framework;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.TestTools;

namespace Qowaiv.DomainModel.UnitTests
{
    public class PropertyCollectionTest
    {
        [Test]
        public void Serialize_RoundTrip()
        {
            var properties = PropertyCollection.Create(typeof(SimpleEntity));
            properties[nameof(SimpleEntity.FullName)] = "ABC";

            var actual = SerializationTest.SerializeDeserialize(properties);

            Assert.AreEqual("ABC", actual[nameof(SimpleEntity.FullName)]);
        }
    }
}
