using NUnit.Framework;
using Qowaiv;
using Qowaiv.Financial;
using Qowaiv.TestTools;

namespace Cast_specs
{
    public class Invalid
    {
        [TestCase(double.MaxValue)]
        [TestCase(double.MinValue)]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        public void double_to_Percentage(double num) => QAssert.InvalidCast(() => (Percentage)num);

        [TestCase(double.MaxValue)]
        [TestCase(double.MinValue)]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        public void double_to_Amount(double num) => QAssert.InvalidCast(() => (Amount)num);


        [TestCase(int.MaxValue)]
        [TestCase(1_000_000_000)]
        public void int_to_HouseNumber(int num) => QAssert.InvalidCast(() => (HouseNumber)num);

        [TestCase(int.MaxValue)]
        [TestCase(1_000_000)]
        [TestCase(-1)]
        [TestCase(13)]
        public void int_to_Month(int num) => QAssert.InvalidCast(() => (Month)num);

        [TestCase(int.MaxValue)]
        [TestCase(1_000_000)]
        [TestCase(-1)]
        [TestCase(10_000)]
        public void int_to_Year(int num) => QAssert.InvalidCast(() => (Year)num);
    }
}
