using NUnit.Framework;
using Qowaiv;
using Qowaiv.Globalization;

namespace TelephoneNumber_pattern_specs
{
    public class International_Numbers
    {
        [Test]
        public void Can_start_with_00()
        {
            var number = TelephoneNumber.Parse("0031 123456789");
            Assert.AreEqual(TelephoneNumberType.International, number.Type);
        }

        [Test]
        public void Can_start_with_a_plus()
        {
            var number = TelephoneNumber.Parse("+31 123456789");
            Assert.AreEqual(TelephoneNumberType.International, number.Type);
        }

        [Test]
        public void Plus_can_not_be_followed_by_zero()
        {
            Assert.IsFalse(TelephoneNumber.IsValid("+031123456789"));
        }

        [Test]
        public void Plus_can_not_be_followed_by_space()
        {
            Assert.IsFalse(TelephoneNumber.IsValid("+ 31123456789"));
        }

        [Test]
        public void Clears_region_prefix_with_parenthis()
        {
            var number = TelephoneNumber.Parse("+31(0)123456789");
            Assert.AreEqual("+31123456789", number.ToString());
        }

        [Test]
        public void Clears_region_prefix_with_brackets()
        {
            var number = TelephoneNumber.Parse("+31[0]123456789");
            Assert.AreEqual("+31123456789", number.ToString());
        }

        [Test]
        public void Matches_country_on_calling_code()
        {
            var number = TelephoneNumber.Parse("+31 123456789");
            Assert.AreEqual(Country.NL, number.Country);
        }

        [Test]
        public void Matches_coutry_with_longest_calling_code_first()
        {
            var number = TelephoneNumber.Parse("+1242 123456789");
            Assert.AreEqual(Country.BS, number.Country);
        }
    }
}
