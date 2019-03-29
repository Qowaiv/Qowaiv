using NUnit.Framework;
using Qowaiv.DomainModel.UnitTests.Models;
using System;

namespace Qowaiv.DomainModel.UnitTests
{
    public class EntityEqualityComparerTest
    {
        [Test]
        public void Equals_NullAndNull_True()
        {
            var comparer = new EntityEqualityComparer<SimpleEntity, Guid>();
            Assert.IsTrue(comparer.Equals(null, null));
        }
    }
}
