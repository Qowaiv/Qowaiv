using NUnit.Framework;
using Qowaiv.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qowaiv.ComponentModel.UnitTests.DataAnnotations
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
