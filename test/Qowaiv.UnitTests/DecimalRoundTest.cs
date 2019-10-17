using NUnit.Framework;

namespace Qowaiv.UnitTests
{
    public class DecimalRoundTest
    {
        [TestCase(150.0, 125.0, 50, DecimalRounding.AwayFromZero)]
        [TestCase(125.0, 123.0, 5, DecimalRounding.AwayFromZero)]
        [TestCase(123.25, 123.3085, 0.25, DecimalRounding.AwayFromZero)]
        [TestCase(666, 666, 3, DecimalRounding.AwayFromZero)]
        public void Round(decimal exp, decimal value, decimal factor, DecimalRounding mode)
        {
            var act = value.MultipleOf(factor, mode);
            Assert.AreEqual(exp, act);
        }

        [TestCase(126, 125.5, +0, DecimalRounding.ToEven)]
        [TestCase(126, 126.5, +0, DecimalRounding.ToEven)]

        [TestCase(000, 126.3, -3, DecimalRounding.AwayFromZero)]
        [TestCase(100, 126.3, -2, DecimalRounding.AwayFromZero)]
        [TestCase(130, 126.3, -1, DecimalRounding.AwayFromZero)]
        [TestCase(126.4, 126.36, +1, DecimalRounding.AwayFromZero)]

        [TestCase(125, 125.4, +0, DecimalRounding.AwayFromZero)]
        [TestCase(126, 125.5, +0, DecimalRounding.AwayFromZero)]
        [TestCase(126, 125.1, +0, DecimalRounding.Ceiling)]
        [TestCase(125, 125.9, +0, DecimalRounding.Floor)]
        [TestCase(125, 125.8, +0, DecimalRounding.Truncate)]

        [TestCase(-25, -25.4, +0, DecimalRounding.AwayFromZero)]
        [TestCase(-26, -25.5, +0, DecimalRounding.AwayFromZero)]
        [TestCase(-25, -25.1, +0, DecimalRounding.Ceiling)]
        [TestCase(-26, -25.9, +0, DecimalRounding.Floor)]
        [TestCase(-25, -25.8, +0, DecimalRounding.Truncate)]
        public void Round(decimal exp, decimal value, int digits, DecimalRounding mode)
        {
            var act = value.Round(digits, mode);
            Assert.AreEqual(exp, act);
        }
    }
}
