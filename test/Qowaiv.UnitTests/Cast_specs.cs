using NUnit.Framework;
using Qowaiv;
using Qowaiv.Financial;
using Qowaiv.UnitTests.TestTools;

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
        [TestCase(11)]
        public void int_to_Gender(int num) => QAssert.InvalidCast(() => (Gender)num);

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
    public class Invalid_string_to
    {
        private static readonly string InvalidString = "$%$!@$%$D!@@!!!!!";

        [Test]
        public void Date() => QAssert.InvalidCast(() => (Date)InvalidString);

        [Test]
        public void EmailAddress() => QAssert.InvalidCast(() => (EmailAddress)InvalidString);

        [Test]
        public void Gender() => QAssert.InvalidCast(() => (Gender)InvalidString);

        [Test]
        public void HouseNumber() => QAssert.InvalidCast(() => (HouseNumber)InvalidString);

        [Test]
        public void LocalDateTime() => QAssert.InvalidCast(() => (LocalDateTime)InvalidString);

        [Test]
        public void Month() => QAssert.InvalidCast(() => (Month)InvalidString);

        [Test]
        public void Percentage() => QAssert.InvalidCast(() => (Percentage)InvalidString);

        [Test]
        public void PostalCode() => QAssert.InvalidCast(() => (PostalCode)InvalidString);

        [Test]
        public void UUID() => QAssert.InvalidCast(() => (Uuid)InvalidString);
        
        [Test]
        public void WeekDate() => QAssert.InvalidCast(() => (WeekDate)InvalidString);

        [Test]
        public void Year() => QAssert.InvalidCast(() => (Year)InvalidString);

        [Test]
        public void YesNo() => QAssert.InvalidCast(() => (YesNo)InvalidString);
    }
}
