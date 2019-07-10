using NUnit.Framework;
using Qowaiv.ComponentModel.DataAnnotations;
using Qowaiv.Globalization;

namespace Qowaiv.Validation.DataAnnotations.Tests.ValidationAttributes
{
    public class ForbiddenValuesAtttributeTest
    {
        [Test]
        public void IsValid_Null_True()
        {
            var attr = new ForbiddenValuesAttribute("DE", "FR", "GB");
            var act = attr.IsValid(null);
            Assert.IsTrue(act);
        }

        [Test]
        public void IsValid_GB_False()
        {
            var attr = new ForbiddenValuesAttribute("DE", "FR", "GB");
            var act = attr.IsValid(Country.GB);
            Assert.IsFalse(act);
        }

        [Test]
        public void IsValid_TR_True()
        {
            var attr = new ForbiddenValuesAttribute("DE", "FR", "GB");
            var act = attr.IsValid(Country.TR);
            Assert.IsTrue(act);
        }
    }
}
