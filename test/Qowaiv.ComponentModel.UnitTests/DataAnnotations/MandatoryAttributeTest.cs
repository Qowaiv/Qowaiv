using NUnit.Framework;
using Qowaiv.ComponentModel.DataAnnotations;
using System;

namespace Qowaiv.ComponentModel.Tests.DataAnnotations
{
    public class MandatoryAttributeTest
    {
        [Test]
        public void IsValid_NewGuid_True()
        {
            var attr = new MandatoryAttribute();
            var act = attr.IsValid(Guid.NewGuid());
            Assert.IsTrue(act);
        }

        [Test]
        public void IsValid_GuidEmpty_False()
        {
            var attr = new MandatoryAttribute();
            var act = attr.IsValid(Guid.Empty);
            Assert.IsFalse(act);
        }

        [Test]
        public void IsValid_SomeEmailAddress_True()
        {
            var attr = new MandatoryAttribute();
            var act = attr.IsValid(EmailAddress.Parse("test@exact.com"));
            Assert.IsTrue(act);
        }

        [Test]
        public void IsValid_EmailAddressEmpty_False()
        {
            var attr = new MandatoryAttribute();
            var act = attr.IsValid(EmailAddress.Empty);
            Assert.IsFalse(act);
        }
    }
}
