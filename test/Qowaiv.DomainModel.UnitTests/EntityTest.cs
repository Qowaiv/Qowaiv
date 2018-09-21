using NUnit.Framework;
using System;

namespace Qowaiv.DomainModel.UnitTests
{
    public class EntityTest
    {
        [Test]
        public void Ctor_NewlyCreated_IsTransient()
        {
            var entity = new GuidEntity();
            Assert.True(entity.IsTransient());
        }

        [Test]
        public void SetId_NotTransient()
        {
            var entity = new GuidEntity();
            entity.NewId();

            Assert.False(entity.IsTransient());
        }

        [Test]
        public void UpdateId_NotSupported()
        {
            var entity = new GuidEntity();
            entity.NewId();
            Assert.Throws<NotSupportedException>
            (
                () => entity.NewId()
            );
        }

        [Test]
        public void Equals_Null_False()
        {
            var entity = new GuidEntity();
            Assert.False(entity.Equals((object)null));
        }

        [Test]
        public void Equals_OtherType_False()
        {
            var entity = new GuidEntity();
            var other = new Entity<Guid>();
            Assert.False(entity.Equals(other));
        }

        [Test]
        public void Equals_BothIsTransient_False()
        {
            var entity = new GuidEntity();
            object other = new GuidEntity();
            Assert.False(entity.Equals(other));
        }

        [Test]
        public void Equals_DifferentIds_False()
        {
            var entity = new GuidEntity();
            entity.NewId();
            var other = new GuidEntity();
            other.NewId();
            Assert.False(entity.Equals(other));
        }

        [Test]
        public void Equals_SameIds_True()
        {
            var entity = new GuidEntity();
            entity.NewId();
            var other = new GuidEntity();
            other.NewId(entity.Id);
            Assert.True(entity.Equals(other));
        }

        [Test]
        public void Equality_SameId_IsTrue()
        {
            var left = new GuidEntity();
            left.NewId();
            var right = new GuidEntity();
            right.NewId(left.Id);

            Assert.IsTrue(left == right);
        }
        [Test]
        public void Inequality_SameId_IsTrue()
        {
            var left = new GuidEntity();
            var right = new GuidEntity();
            right.NewId(left.Id);

            Assert.IsTrue(left != right);
        }

        [Test]
        public void GetHashCode_IsTransient_NotSupported()
        {
            Assert.Throws<NotSupportedException>
            (
                () => new GuidEntity().GetHashCode()
            );
        }

        [Test]
        public void GetHashCode_SameValue()
        {
            var entity = new GuidEntity();
            entity.NewId();

            var expected = entity.GetHashCode();

            Assert.AreEqual(expected, entity.GetHashCode());
        }
    }

    internal class GuidEntity : Entity<Guid>
    {
        public void NewId(Guid? id = null) => Id = id ?? Guid.NewGuid();
    }
}
