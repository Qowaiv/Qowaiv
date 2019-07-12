using NUnit.Framework;
using Qowaiv.Validation.DataAnnotations.UnitTests.Models;
using System.Collections.Generic;

namespace Qowaiv.Validation.DataAnnotations.UnitTests
{
    public class ReferenceComparerTest
    {
        [Test]
        public void Add_TwoInstancesWithSameHashCodeAndEquals_2Items()
        {
            var set = new HashSet<object>(ReferenceComparer.Instance)
            {
                new ModelWithOneState(),
                new ModelWithOneState()
            };
            Assert.AreEqual(2, set.Count);
        }

        [Test]
        public void Add_SameItemTwice_1Item()
        {
            var model = new ModelWithOneState();
            var set = new HashSet<object>(ReferenceComparer.Instance)
            {
                model,
                model
            };
            Assert.AreEqual(1, set.Count);
        }
    }
}
