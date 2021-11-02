using FluentAssertions;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Hashing;
using Qowaiv.Specs;

namespace LocalDateTime_specs
{
    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
            => Svo.LocalDateTime.Equals(null).Should().BeFalse();

        [Test]
        public void not_equal_to_other_type()
            => Svo.LocalDateTime.Equals(new object()).Should().BeFalse();

        [Test]
        public void not_equal_to_different_value()
            => Svo.LocalDateTime.Equals(LocalDateTime.MinValue).Should().BeFalse();

        [Test]
        public void equal_to_same_value()
            => Svo.LocalDateTime.Equals(new LocalDateTime(2017, 06, 11, 06, 15, 00)).Should().BeTrue();

        [Test]
        public void equal_operator_returns_true_for_same_values()
            => (new LocalDateTime(2017, 06, 11, 06, 15, 00) == Svo.LocalDateTime).Should().BeTrue();

        [Test]
        public void equal_operator_returns_false_for_different_values()
            => (new LocalDateTime(2017, 06, 11, 06, 15, 00) == LocalDateTime.MinValue).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
            => (new LocalDateTime(2017, 06, 11, 06, 15, 00) != Svo.LocalDateTime).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
            => (new LocalDateTime(2017, 06, 11, 06, 15, 00) != LocalDateTime.MinValue).Should().BeTrue();

        [TestCase("0001-01-01", 0)]
        [TestCase("2017-06-11 06:15", 961707490)]
        public void hash_code_is_value_based(LocalDateTime svo, int hash)
        {
            using (Hash.WithFixedRandomizer())
            {
                svo.GetHashCode().Should().Be(hash);
            }
        }
    }
}
