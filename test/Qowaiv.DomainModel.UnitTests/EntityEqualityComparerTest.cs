using NUnit.Framework;
using Qowaiv.DomainModel.Tracking;
using Qowaiv.DomainModel.UnitTests.Models;
using System;

namespace Qowaiv.DomainModel.UnitTests
{
    public class EntityEqualityComparerTest
    {
        [Test]
        public void Equals_NullAndNull_True()
        {
            var comparer = new EntityEqualityComparer<SimpleEntity>();
            Assert.IsTrue(comparer.Equals(null, null));
        }

        [Test]
        public void Equals_SameIdDifferentTypes_IsFalse()
        {
            var id = Guid.NewGuid();
            var x = new CustomEntity(id);
            var y = new OtherEntity(id);

            var comparer = new EntityEqualityComparer<CustomEntity>();
            Assert.IsFalse(comparer.Equals(x, y));
        }

        [Test]
        public void Equals_DifferentIds_IsFalse()
        {
            var x = new CustomEntity();
            var y = new CustomEntity();

            var comparer = new EntityEqualityComparer<CustomEntity>();
            Assert.IsFalse(comparer.Equals(x, y));
        }

        [Test]
        public void Equals_SameId_IsTrue()
        {
            var id = Guid.NewGuid();
            var x = new CustomEntity(id);
            var y = new CustomEntity(id);

            var comparer = new EntityEqualityComparer<CustomEntity>();
            Assert.IsTrue(comparer.Equals(x, y));
        }

        private class CustomEntity : Entity<CustomEntity>
        {
            public CustomEntity() : base(new MyTracker()) { }
            public CustomEntity(Guid id) : base(id, new MyTracker()) { }
        }

        private class OtherEntity : CustomEntity
        {
            public OtherEntity(Guid id) : base(id) { }
        }

        private class MyTracker : ChangeTracker
        {
            protected override void OnAddComplete() { /* done */ }
        }
    }
}
