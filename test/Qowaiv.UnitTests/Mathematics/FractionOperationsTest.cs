using NUnit.Framework;
using Qowaiv.Mathematics;
using System;

namespace Qowaiv.UnitTests.Mathematics
{
    public class FractionOperationsTest
    {
        [Test]
        public void Multiply_Overflow()
        {
            var fraction = new Fraction(int.MaxValue, 1);
            Assert.Throws<OverflowException>(() => fraction.Multiply(long.MaxValue));
        }
        [Test]
        public void Multiply_long()
        {
            var fraction = new Fraction(19, 72);
            var actual = fraction.Multiply(48);

            var expected = new Fraction(38, 3);

            Assert.AreEqual(expected, actual);
        }

        [TestCase("1/3", "1/4", "7/12")]
        [TestCase("1/4", "1/3", "7/12")]
        [TestCase("1/4", "1/12", "1/3")]
        public void Add(Fraction left, Fraction right, Fraction expected)
        {
            var actual = left + right;
            Assert.AreEqual(expected, actual);
        }

        [TestCase("1/3", "1/4", "1/12")]
        [TestCase("1/4", "1/3", "-1/12")]
        public void Subtract(Fraction left, Fraction right, Fraction expected)
        {
            var actual = left - right;
            Assert.AreEqual(expected, actual);
        }
    }
}
