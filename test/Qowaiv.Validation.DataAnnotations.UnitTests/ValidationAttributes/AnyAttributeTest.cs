using NUnit.Framework;
using System.Collections;

namespace Qowaiv.Validation.DataAnnotations.Tests.ValidationAttributes
{
    public class AnyAttributeTest
    {
        [Test]
        public void IsValid_Null_IsFalse()
        {
            var attribute = new AnyAttribute();
            IEnumerable collection = null;
            Assert.IsFalse(attribute.IsValid(collection));
        }

        [Test]
        public void IsValid_Empty_IsFalse()
        {
            var attribute = new AnyAttribute();
            var collection = new int[0];
            Assert.IsFalse(attribute.IsValid(collection));
        }

        [Test]
        public void IsValid_1Item_IsTrue()
        {
            var attribute = new AnyAttribute();
            var collection = new int[1];
            Assert.IsTrue(attribute.IsValid(collection));
        }
    }
}
