using NUnit.Framework;
using Qowaiv.DomainModel.EventSourcing;
using System;

namespace Qowaiv.DomainModel.UnitTests.EventSourcing
{
    public class EventInfoTest
    {
        private static readonly EventInfo TestStruct = new EventInfo(1, Guid.Parse("6E1D3455-D1E2-484E-8C54-27F9B0BFE8BA"), new DateTime(2017, 06, 11, 6, 15, 0));

        [Test]
        public void Equals_Null_IsFalse()
        {
            Assert.IsFalse(TestStruct.Equals(null));
        }

        [Test]
        public void Equals_Object_IsTrue()
        {
            object obj = TestStruct;
            Assert.IsTrue(TestStruct.Equals(obj));
        }

        [Test]
        public void ToString_TestStruct_AreEqual()
        {
            var act = TestStruct.ToString();
            var exp = "Version: 1, 2017-06-11 06:15:00, AggregateId: {6e1d3455-d1e2-484e-8c54-27f9b0bfe8ba}";
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void GetHashcode_TestStruct()
        {
            var act = TestStruct.GetHashCode();
            var exp = 1576907803;
            Assert.AreEqual(exp, act);
        }
    }
}
