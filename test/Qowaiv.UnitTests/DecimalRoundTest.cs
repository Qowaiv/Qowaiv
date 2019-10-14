using NUnit.Framework;

namespace Qowaiv.UnitTests
{
    public class DecimalRoundTest
    {
        [TestCase(125, 123, +0, DecimalRounding.Default, 5)]
        [TestCase(125, 150, -1, DecimalRounding.Default, 5)]
        public void Round(decimal exp, decimal value, int digits, DecimalRounding rounding, int multipyOf)
        {
            var act = value.Round(digits, rounding, multipyOf);
            Assert.AreEqual(exp, act);
        }
    }
}
