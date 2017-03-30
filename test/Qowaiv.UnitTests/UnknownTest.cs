using System;
using NUnit.Framework;
using System.Globalization;

namespace Qowaiv.UnitTests
{
    [TestFixture]
    public class UnknownTest
    {
        [Test]
        public void IsUnknown_Null_IsFalse()
        {
            Assert.IsFalse(Unknown.IsUnknown(null));
        }

        [Test]
        public void IsUnknown_StringEmpty_IsFalse()
        {
            Assert.IsFalse(Unknown.IsUnknown(string.Empty));
        }

        [Test]
        public void IsUnknown_QuestionMarkCultureNl_IsTrue()
        {
            Assert.IsTrue(Unknown.IsUnknown("?", new CultureInfo("nl-NL")));
        }

        [Test]
        public void IsUnknown_QuestionMarkCultureNull_IsTrue()
        {
            Assert.IsTrue(Unknown.IsUnknown("?", null));
        }

        [Test]
        public void IsUnknown_NaoSabeCulturePt_IsTrue()
        {
            Assert.IsTrue(Unknown.IsUnknown("Não SABe", new CultureInfo("pt-PT")));
        }

        [Test]
        public void IsUnknown_VreemdCultureInvariant_IsFalse()
        {
            Assert.IsFalse(Unknown.IsUnknown("Vreemd", CultureInfo.InvariantCulture));
        }
    }
}
