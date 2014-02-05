using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Qowaiv.UnitTests
{
    [TestClass]
    public class UnknownTest
    {
        [TestMethod]
        public void IsUnknown_Null_IsFalse()
        {
            Assert.IsFalse(Unknown.IsUnknown(null));
        }

        [TestMethod]
        public void IsUnknown_StringEmpty_IsFalse()
        {
            Assert.IsFalse(Unknown.IsUnknown(String.Empty));
        }

        [TestMethod]
        public void IsUnknown_QuestionMarkCultureNl_IsTrue()
        {
            Assert.IsTrue(Unknown.IsUnknown("?", new CultureInfo("nl-NL")));
        }

        [TestMethod]
        public void IsUnknown_QuestionMarkCultureNull_IsTrue()
        {
            Assert.IsTrue(Unknown.IsUnknown("?", null));
        }

        [TestMethod]
        public void IsUnknown_NaoSabeCulturePt_IsTrue()
        {
            Assert.IsTrue(Unknown.IsUnknown("Não SABe", new CultureInfo("pt-PT")));
        }

        [TestMethod]
        public void IsUnknown_VreemdCultureInvariant_IsFalse()
        {
            Assert.IsFalse(Unknown.IsUnknown("Vreemd", CultureInfo.InvariantCulture));
        }
    }
}
