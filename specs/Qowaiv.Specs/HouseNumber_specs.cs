using FluentAssertions;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Hashing;
using Qowaiv.Specs;

namespace HouseNumber_specs
{
    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
            => Svo.HouseNumber.Equals(null).Should().BeFalse();

        [Test]
        public void not_equal_to_other_type()
            => Svo.HouseNumber.Equals(new object()).Should().BeFalse();

        [Test]
        public void not_equal_to_different_value()
            => Svo.HouseNumber.Equals(HouseNumber.MinValue).Should().BeFalse();

        [Test]
        public void equal_to_same_value()
            => Svo.HouseNumber.Equals(HouseNumber.Create(123456789)).Should().BeTrue();

        [Test]
        public void equal_operator_returns_true_for_same_values()
            => (HouseNumber.Create(123456789) == Svo.HouseNumber).Should().BeTrue();

        [Test]
        public void equal_operator_returns_false_for_different_values()
            => (HouseNumber.Create(123456789) == HouseNumber.MinValue).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
            => (HouseNumber.Create(123456789) != Svo.HouseNumber).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
            => (HouseNumber.Create(123456789) != HouseNumber.MinValue).Should().BeTrue();

        [TestCase("", 0)]
        [TestCase("yes", 665630161)]
        public void hash_code_is_value_based(YesNo svo, int hash)
        {
            using (Hash.WithoutRandomizer())
            {
                svo.GetHashCode().Should().Be(hash);
            }
        }
    }
}
