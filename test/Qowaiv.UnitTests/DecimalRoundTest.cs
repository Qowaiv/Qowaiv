using NUnit.Framework;
using System.Globalization;

namespace Qowaiv.UnitTests
{
    public class DecimalRoundTest
    {
        [Test]
        public void Round_RemainderZero_NoChange()
        {
            // initializes a decimal with scale of 4, instead of 0.
            var value = decimal.Parse("1000.0000", CultureInfo.InvariantCulture);
            var rounded = value.Round(-2);
            Assert.AreEqual(1000m, rounded);
        }

        [Test]
        public void Round_Postive_ShoulRoundAwayFromZero()
        {
            var rounded = 24.5m.Round();
            Assert.AreEqual(25m, rounded);
        }
        [Test]
        public void Round_Negative_ShoulRoundAwayFromZero()
        {
            var rounded = -25.5m.Round();
            Assert.AreEqual(-26m, rounded);
        }

        [Test]
        public void Round_ALotOf3s_WithoutIssues()
        {
            var value = 9_876_543_210m + 1m / 3m;
            var rounded = value.Round(-8);
            var expected = 10_000_000_000m;

            Assert.AreEqual(expected, rounded);
        }


        [Test]
        public void Round_PostiveWithMulpleOf_ShoulRoundAwayFromZero()
        {
            var rounded = 24.5m.Round(1m);
            Assert.AreEqual(25m, rounded);
        }
        [Test]
        public void Round_NegativeWithMulpleOf_ShoulRoundAwayFromZero()
        {
            var rounded = -25.5m.Round(1m);
            Assert.AreEqual(-26m, rounded);
        }


        [TestCase(150.0, 125.0, 50, DecimalRounding.AwayFromZero)]
        [TestCase(125.0, 123.0, 5, DecimalRounding.AwayFromZero)]
        [TestCase(123.25, 123.3085, 0.25, DecimalRounding.AwayFromZero)]
        [TestCase(666, 666, 3, DecimalRounding.AwayFromZero)]
        public void Round_MultipleOf(decimal exp, decimal value, decimal factor, DecimalRounding mode)
        {
            var act = value.Round(factor, mode);
            Assert.AreEqual(exp, act);
        }

        /// <remarks>Use strings as doubles lack precision.</remarks>
        [TestCase("123456789.1234567890", +10)]
        [TestCase("123456789.123456789", +9)]
        [TestCase("123456789.12345679", +8)]
        [TestCase("123456789.1234568", +7)]
        [TestCase("123456789.123457", +6)]
        [TestCase("123456789.12346", +5)]
        [TestCase("123456789.1235", +4)]
        [TestCase("123456789.123", +3)]
        [TestCase("123456789.12", +2)]
        [TestCase("123456789.1", +1)]
        [TestCase("123456789", +0)]
        [TestCase("123456790", -1)]
        [TestCase("123456800", -2)]
        [TestCase("123457000", -3)]
        [TestCase("123460000", -4)]
        [TestCase("123500000", -5)]
        [TestCase("123000000", -6)]
        [TestCase("120000000", -7)]
        [TestCase("100000000", -8)]
        [TestCase(0, -9)]
        public void Round_Digits(decimal exp, int digits)
        {
            var act = 123456789.123456789m.Round(digits, DecimalRounding.AwayFromZero);
            Assert.AreEqual(exp, act);
        }

        // Halfway/nearest rounding
        [TestCase(-26, -25.5, DecimalRounding.AwayFromZero)]
        [TestCase(+26, +25.5, DecimalRounding.AwayFromZero)]
        [TestCase(-25, -25.5, DecimalRounding.TowardsZero)]
        [TestCase(+25, +25.5, DecimalRounding.TowardsZero)]
        [TestCase(+26, +25.5, DecimalRounding.ToEven)]
        [TestCase(+24, +24.5, DecimalRounding.ToEven)]
        [TestCase(+25, +25.5, DecimalRounding.ToOdd)]
        [TestCase(+25, +24.5, DecimalRounding.ToOdd)]
        [TestCase(-25, -25.5, DecimalRounding.Up)]
        [TestCase(+26, +25.5, DecimalRounding.Up)]
        [TestCase(-26, -25.5, DecimalRounding.Down)]
        [TestCase(+25, +25.5, DecimalRounding.Down)]
        // Direct rounding
        [TestCase(-26, -25.1, DecimalRounding.DirectAwayFromZero)]
        [TestCase(+26, +25.1, DecimalRounding.DirectAwayFromZero)]
        [TestCase(-25, -25.1, DecimalRounding.DirectTowardsZero)]
        [TestCase(+25, +25.1, DecimalRounding.DirectTowardsZero)]
        [TestCase(-25, -25.1, DecimalRounding.Ceiling)]
        [TestCase(+26, +25.1, DecimalRounding.Ceiling)]
        [TestCase(-26, -25.1, DecimalRounding.Floor)]
        [TestCase(+25, +25.1, DecimalRounding.Floor)]
        [TestCase(-25, -25.1, DecimalRounding.Truncate)]
        [TestCase(+25, +25.1, DecimalRounding.Truncate)]
        public void Round_NearestAndDirect(decimal exp, decimal value, DecimalRounding mode)
        {
            var act = value.Round(0, mode);
            Assert.AreEqual(exp, act);
        }

        [Test]
        public void Round_RandomTieBreaking()
        {
            var value = 17.5m;

            var runs = 100_000;
            var sum = 0m;

            for(var i = 0; i < runs; i++)
            {
                var rounded = value.Round(0, DecimalRounding.RandomTieBreaking);
                Assert.IsTrue(rounded == 17 || rounded == 18);
                sum += rounded;
            }

            var avg = sum / runs;

            Assert.That(avg, Is.EqualTo(value).Within(0.05m));
        }

        [TestCase(17.1)]
        [TestCase(17.3)]
        [TestCase(17.6)]
        [TestCase(17.8)]
        public void Round_StochasticRounding(decimal value)
        {
            var runs = 10_000;
            var sum = 0m;

            for (var i = 0; i < runs; i++)
            {
                var rounded = value.Round(0, DecimalRounding.StochasticRounding);
                Assert.IsTrue(rounded == 17 || rounded == 18);
                sum += rounded;
            }

            var avg = sum / runs;

            Assert.That(avg, Is.EqualTo(value).Within(0.05m));
        }
    }
}
