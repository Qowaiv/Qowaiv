using NUnit.Framework;
using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.TestTools.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.UnitTests.DataAnnotations
{
    public class ImmutableAttributeTest
    {
        [Test]
        public void GetValidationResult_PropertyId_WithError()
        {
            var attribute = new ImmutableAttribute();
            var context = new ValidationContext(new TestClass() { Id = 4 }) { MemberName = nameof(TestClass.Id) };
            ValidationAssert.WithError("The Id field is immutable and can not be changed.", attribute, 3, context);
        }

        [Test]
        public void GetValidationResult_FieldId_WithError()
        {
            var attribute = new ImmutableAttribute();
            var context = new ValidationContext(new TestClass() { _Id = 4 }) { MemberName = nameof(TestClass._Id) };
            ValidationAssert.WithError("The _Id field is immutable and can not be changed.", attribute, 3, context);
        }

        [Test]
        public void GetValidationResult_KeepSameValue_IsSuccessful()
        {
            var attribute = new ImmutableAttribute();
            var context = new ValidationContext(new TestClass() { Id = 4 }) { MemberName = nameof(TestClass.Id) };
            ValidationAssert.IsSuccessful(attribute, 4, context);
        }

        [Test]
        public void GetValidationResult_PropertyId_IsSuccessful()
        {
            var attribute = new ImmutableAttribute();
            var context = new ValidationContext(new TestClass()) { MemberName = nameof(TestClass.Id) };
            ValidationAssert.IsSuccessful(attribute, 3, context);
        }

        [Test]
        public void GetValidationResult_FieldId_IsSuccessful()
        {
            var attribute = new ImmutableAttribute();
            var context = new ValidationContext(new TestClass()) { MemberName = nameof(TestClass._Id) };
            ValidationAssert.IsSuccessful(attribute, 3, context);
        }

        [Test]
        public void GetValidationResult_NonExsitingMember_Throws()
        {
            var attribute = new ImmutableAttribute();
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var context = new ValidationContext(new TestClass()) { MemberName = "NonExsitingMember" };
                attribute.GetValidationResult(3, context);
            });
            Assert.AreEqual(
                @"The member 'NonExsitingMember' could not be resolved for the type 'Qowaiv.ComponentModel.UnitTests.DataAnnotations.TestClass'.
Parameter name: validationContext", 
                exception.Message);
        }
    }
    internal class TestClass
    {
        [Immutable]
        public int Id { get; set; }

        [Immutable]
        public int _Id;
    }
}
