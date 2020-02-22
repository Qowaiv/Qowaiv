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
    }
}
