using FluentAssertions;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Hashing;
using Qowaiv.Specs;

namespace MonthSpan_specs
{
    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
            => Svo.MonthSpan.Equals(null).Should().BeFalse();

        [Test]
        public void not_equal_to_other_type()
            => Svo.MonthSpan.Equals(new object()).Should().BeFalse();

        [Test]
        public void not_equal_to_different_value()
            => Svo.MonthSpan.Equals(MonthSpan.MinValue).Should().BeFalse();

        [Test]
        public void equal_to_same_value()
            => Svo.MonthSpan.Equals(MonthSpan.FromMonths(69)).Should().BeTrue();

        [Test]
        public void equal_operator_returns_true_for_same_values()
            => (MonthSpan.FromMonths(69) == Svo.MonthSpan).Should().BeTrue();

        [Test]
        public void equal_operator_returns_false_for_different_values()
            => (MonthSpan.FromMonths(69) == MonthSpan.MinValue).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
            => (MonthSpan.FromMonths(69) != Svo.MonthSpan).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
            => (MonthSpan.FromMonths(69) != MonthSpan.MinValue).Should().BeTrue();

        [TestCase("0Y+0M", 0)]
        [TestCase("5Y+9M", 20170550)]
        public void hash_code_is_value_based(MonthSpan svo, int hash)
        {
            using (Hash.WithoutRandomizer())
            {
                svo.GetHashCode().Should().Be(hash);
            }
        }
    }
}
