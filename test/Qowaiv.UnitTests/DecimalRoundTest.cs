using NUnit.Framework;

namespace Qowaiv.UnitTests
{
    public class DecimalRoundTest
    {
        [TestCase(125, 125.4, +0, DecimalRounding.ToEven, 1)]
        [TestCase(126, 125.5, +0, DecimalRounding.ToEven, 1)]
        [TestCase(126, 125.1, +0, DecimalRounding.Ceiling, 1)]
        [TestCase(125, 125.9, +0, DecimalRounding.Floor, 1)]
        [TestCase(125, 125.8, +0, DecimalRounding.Truncate, 1)]

        [TestCase(-25, -25.4, +0, DecimalRounding.ToEven, 1)]
        [TestCase(-26, -25.5, +0, DecimalRounding.ToEven, 1)]
        [TestCase(-25, -25.1, +0, DecimalRounding.Ceiling, 1)]
        [TestCase(-26, -25.9, +0, DecimalRounding.Floor, 1)]
        [TestCase(-25, -25.8, +0, DecimalRounding.Truncate, 1)]
        //[TestCase(125.0, 150.0, -1, DecimalRounding.ToEven, 5)]
        //[TestCase(125.0, 123.0, +0, DecimalRounding.ToEven, 5)]
        //[TestCase(125.0, 150.0, -1, DecimalRounding.ToEven, 5)]
        public void Round(decimal exp, decimal value, int digits, DecimalRounding rounding, int multipyOf)
        {
            var act = value.Round(digits, rounding, multipyOf);
            Assert.AreEqual(exp, act);
        }
    }
}
