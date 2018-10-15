using NUnit.Framework;
using Qowaiv.TestTools;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests
{
    public class EntityTest
    {
        [Test]
        public void Ctor_NewlyCreated_IsTransient()
        {
            var entity = new SimpleEntity();
            Assert.True(entity.IsTransient);
        }

        [Test]
        public void SetId_NotTransient()
        {
            var entity = new SimpleEntity();
            entity.NewId();

            Assert.False(entity.IsTransient);
        }

        [Test]
        public void UpdateId_Default_ArgumentException()
        {
            var entity = new SimpleEntity();
            Assert.Throws<ArgumentException>
            (
                () => entity.NewId(Guid.Empty)
            );
        }

        [Test]
        public void UpdateId_NotSupported()
        {
            var entity = new SimpleEntity();
            entity.NewId();
            Assert.Throws<NotSupportedException>
            (
                () => entity.NewId()
            );
        }

        [Test]
        public void SetFullName_EmptyValue_Throws()
        {
            var entity = new SimpleEntity();

            var exception = Assert.Throws<ValidationException>
            (
                () => entity.FullName = ""
            );
            Assert.AreEqual("The Full name field is required.", exception.Message);
        }

        [Test]
        public void SetFullName_TooLong_Throws()
        {
            var entity = new SimpleEntity();

            var exception = Assert.Throws<ValidationException>
            (
                () => entity.FullName = "1234"
            );
            Assert.AreEqual("The field Full name must be a string or array type with a maximum length of '3'.", exception.Message);
        }

        [Test]
        public void SetFullName_IsSetAndDirty()
        {
            var entity = new SimpleEntity
            {
                FullName = "ABC"
            };
            Assert.AreEqual("ABC", entity.FullName);
            Assert.IsTrue(entity.IsDirty);
        }

        [Test]
        public void Equals_Null_False()
        {
            var entity = new SimpleEntity();
            Assert.False(entity.Equals((object)null));
        }

        [Test]
        public void Equals_OtherType_False()
        {
            var entity = new SimpleEntity();
            var other = new Entity<Guid>();
            Assert.False(entity.Equals(other));
        }

        [Test]
        public void Equals_BothIsTransient_False()
        {
            var entity = new SimpleEntity();
            object other = new SimpleEntity();
            Assert.False(entity.Equals(other));
        }

        [Test]
        public void Equals_DifferentIds_False()
        {
            var entity = new SimpleEntity();
            entity.NewId();
            var other = new SimpleEntity();
            other.NewId();
            Assert.False(entity.Equals(other));
        }

        [Test]
        public void Equals_SameIds_True()
        {
            var entity = new SimpleEntity();
            entity.NewId();
            var other = new SimpleEntity();
            other.NewId(entity.Id);
            Assert.True(entity.Equals(other));
        }

        [Test]
        public void Equality_SameId_IsTrue()
        {
            var left = new SimpleEntity();
            left.NewId();
            var right = new SimpleEntity();
            right.NewId(left.Id);

            Assert.IsTrue(left == right);
        }
        [Test]
        public void Inequality_SameId_IsFalse()
        {
            var left = new SimpleEntity();
            left.NewId();
            var right = new SimpleEntity();
            right.NewId(left.Id);

            Assert.False(left != right);
        }

        [Test]
        public void GetHashCode_IsTransient_NotSupported()
        {
            Assert.Throws<NotSupportedException>
            (
                () => new SimpleEntity().GetHashCode()
            );
        }

        [Test]
        public void GetHashCode_SameValue()
        {
            var entity = new SimpleEntity();
            entity.NewId();

            var expected = entity.GetHashCode();

            Assert.AreEqual(expected, entity.GetHashCode());
        }

        [Test]
        public void DebuggerDisplay_IsSupported()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Entity<>));
        }

        [Test]
        public void DebuggerDisplay_IsTransient()
        {
            DebuggerDisplayAssert.HasResult(
                "Qowaiv.DomainModel.Entity`1[System.Guid], ID: ?",
                new Entity<Guid>());
        }

        [Test]
        public void DebuggerDisplay_NotTransient()
        {
            var id = Guid.Parse("10FC7CA7-A781-45DF-81FA-35F3246A5E39");

            DebuggerDisplayAssert.HasResult(
                "Qowaiv.DomainModel.Entity`1[System.Guid], ID: 10fc7ca7-a781-45df-81fa-35f3246a5e39",
                new Entity<Guid>(id));
        }
    }
}
