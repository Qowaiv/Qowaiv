using NUnit.Framework;
using Qowaiv.Reflection;

namespace Qowaiv.UnitTests.Reflection
{
    public class QowaivTypeTest
    {
        [Test]
        public void IsNullOrDefaultValue_Null()
        {
            Assert.IsTrue(QowaivType.IsNullOrDefaultValue(null));
        }

        [Test]
        public void IsNullOrDefaultValue_HouseNumberEmpty()
        {
            Assert.IsTrue(QowaivType.IsNullOrDefaultValue(HouseNumber.Empty));
        }

        [Test]
        public void IsNotNullOrDefaultValue_17()
        {
            Assert.IsFalse(QowaivType.IsNullOrDefaultValue(17));
        }

        [Test]
        public void IsNotNullOrDefaultValue_SomeObject()
        {
            Assert.IsFalse(QowaivType.IsNullOrDefaultValue(new QowaivTypeTest()));
        }
    }
}
