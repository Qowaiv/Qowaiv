﻿using NUnit.Framework;
using Qowaiv.Globalization;

namespace Qowaiv.Validation.DataAnnotations.Tests.ValidationAttributes
{
    public class AllowedValuesAtttributeTest
    {
        [Test]
        public void IsValid_Null_True()
        {
            var attr = new AllowedValuesAttribute("DE", "FR", "GB");
            var act = attr.IsValid(null);
            Assert.IsTrue(act);
        }

        [Test]
        public void IsValid_GB_True()
        {
            var attr = new AllowedValuesAttribute("DE", "FR", "GB");
            var act = attr.IsValid(Country.GB);
            Assert.IsTrue(act);
        }

        [Test]
        public void IsValid_TR_False()
        {
            var attr = new AllowedValuesAttribute("DE", "FR", "GB");
            var act = attr.IsValid(Country.TR);
            Assert.IsFalse(act);
        }
    }
}
