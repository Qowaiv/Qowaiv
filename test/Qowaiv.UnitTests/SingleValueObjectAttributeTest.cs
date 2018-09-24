using NUnit.Framework;

namespace Qowaiv.UnitTests
{
    public class SingleValueObjectAttributeTest
    {
        [Test]
        public void Ctor_Params_AreEqual()
        {
            var act = new SingleValueObjectAttribute(SingleValueStaticOptions.All, typeof(string));

            Assert.AreEqual(SingleValueStaticOptions.All, act.StaticOptions, "act.StaticOptions");
            Assert.AreEqual(typeof(string), act.UnderlyingType, "act.UnderlyingType");
        }
    }
}
