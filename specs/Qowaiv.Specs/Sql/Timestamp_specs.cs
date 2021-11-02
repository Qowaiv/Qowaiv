using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Hashing;
using Qowaiv.Specs;
using Qowaiv.Sql;

namespace Sql.Timestamp_specs
{
    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
            => Svo.Timestamp.Equals(null).Should().BeFalse();

        [Test]
        public void not_equal_to_other_type()
            => Svo.Timestamp.Equals(new object()).Should().BeFalse();

        [Test]
        public void not_equal_to_different_value()
            => Svo.Timestamp.Equals(Timestamp.MinValue).Should().BeFalse();

        [Test]
        public void equal_to_same_value()
            => Svo.Timestamp.Equals(Timestamp.Create(1234567890L)).Should().BeTrue();

        [Test]
        public void equal_operator_returns_true_for_same_values()
            => (Timestamp.Create(1234567890L) == Svo.Timestamp).Should().BeTrue();

        [Test]
        public void equal_operator_returns_false_for_different_values()
            => (Timestamp.Create(1234567890L) == Timestamp.MinValue).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
            => (Timestamp.Create(1234567890L) != Svo.Timestamp).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
            => (Timestamp.Create(1234567890L) != Timestamp.MinValue).Should().BeTrue();

        [TestCase("0", 0)]
        [TestCase("1234567890", 1218823585)]
        public void hash_code_is_value_based(Timestamp svo, int hash)
        {
            using (Hash.WithFixedRandomizer())
            {
                svo.GetHashCode().Should().Be(hash);
            }
        }
    }
}
