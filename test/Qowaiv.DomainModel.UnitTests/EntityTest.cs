using NUnit.Framework;
using Qowaiv.DomainModel.UnitTests.Models;
using Qowaiv.TestTools;
using Qowaiv.TestTools.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel.UnitTests
{
    public class EntityTest
    {
        [Test]
        public void UpdateId_ValidationException()
        {
            var entity = new EmptyEntity(Guid.NewGuid());
            Assert.Throws<ValidationException>
            (
                () => entity.Id = Guid.NewGuid()
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
        public void Equals_Null_False()
        {
            var entity = new EmptyEntity();
            Assert.False(entity.Equals((object)null));
        }

        [Test]
        public void Equals_OtherType_False()
        {
            var entity = new EmptyEntity();
            var other = new EmptyEntity();
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
            var entity = new EmptyEntity(Guid.NewGuid());
            var other = new EmptyEntity(Guid.NewGuid());
            Assert.False(entity.Equals(other));
        }

        [Test]
        public void Equals_SameIds_True()
        {
            var entity = new EmptyEntity(Guid.NewGuid());
            var other = new EmptyEntity(entity.Id);
            Assert.True(entity.Equals(other));
        }

        [Test]
        public void Equality_SameId_IsTrue()
        {
            var left = new EmptyEntity(Guid.NewGuid());
            var right = new EmptyEntity(left.Id);

            Assert.IsTrue(left == right);
        }
        [Test]
        public void Inequality_SameId_IsFalse()
        {
            var left = new EmptyEntity(Guid.NewGuid());
            var right = new EmptyEntity(left.Id);

            Assert.False(left != right);
        }

        [Test]
        public void InitProperties_TwoErrors_WithErrors()
        {
            var entity =  EntityWithInitScope.FromData(17, string.Empty, Date.MinValue);

            ValidationResultAssert.WithErrors(entity,
                ValidationTestMessage.Error("The Name field is required.", nameof(EntityWithInitScope.Name)),
                ValidationTestMessage.Error("The StartDate field is required.", nameof(EntityWithInitScope.StartDate))
            );
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
            var entity = new EmptyEntity(Guid.NewGuid());

            var expected = entity.GetHashCode();

            Assert.AreEqual(expected, entity.GetHashCode());
        }

        [Test]
        public void DebuggerDisplay_IsSupported()
        {
            DebuggerDisplayAssert.HasAttribute(typeof(Entity<,>));
        }
    }
}
