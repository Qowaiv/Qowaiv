using NUnit.Framework;
using System;

namespace Qowaiv.DomainModel.UnitTests
{
    public class EntityEqualityComparerTest
    {
        [Test]
        public void Equals_NullAndNull_True()
        {
            var comparer = new EntityEqualityComparer<Guid>();
            Assert.IsTrue(comparer.Equals(null, null));
        }
    }
}
