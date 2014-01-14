using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Qowaiv.UnitTests
{
    [TestClass]
    public class SingleValueObjectAttributeTest
    {
        [TestMethod]
        public void Ctor_Params_AreEqual()
        {
            var act = new SingleValueObjectAttribute(SingleValueStaticOptions.All, typeof(String));

            Assert.AreEqual(SingleValueStaticOptions.All, act.StaticOptions, "act.StaticOptions");
            Assert.AreEqual(typeof(String), act.UnderlyingType, "act.UnderlyingType");
        }
    }
}
