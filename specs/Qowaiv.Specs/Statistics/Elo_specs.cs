using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Hashing;
using Qowaiv.Specs;
using Qowaiv.Statistics;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;

namespace Statistics.Elo_specs
{
    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
            => Svo.Elo.Equals(null).Should().BeFalse();

        [Test]
        public void not_equal_to_other_type()
            => Svo.Elo.Equals(new object()).Should().BeFalse();

        [Test]
        public void not_equal_to_different_value()
            => Svo.Elo.Equals(Elo.MinValue).Should().BeFalse();

        [Test]
        public void equal_to_same_value()
            => Svo.Elo.Equals(Elo.Create(1732.4)).Should().BeTrue();

        [Test]
        public void equal_operator_returns_true_for_same_values()
            => (Elo.Create(1732.4) == Svo.Elo).Should().BeTrue();

        [Test]
        public void equal_operator_returns_false_for_different_values()
            => (Elo.Create(1732.4) == Elo.MinValue).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
            => (Elo.Create(1732.4) != Svo.Elo).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
            => (Elo.Create(1732.4) != Elo.MinValue).Should().BeTrue();

        [TestCase("0", 0)]
        [TestCase("1732.4", -22135344)]
        public void hash_code_is_value_based(Elo svo, int hash)
        {
            using (Hash.WithoutRandomizer())
            {
                svo.GetHashCode().Should().Be(hash);
            }
        }
    }

    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
           => typeof(Elo).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From<string>(null).To<Elo>().Should().Be(Elo.Zero);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From("1732.4").To<Elo>().Should().Be(Svo.Elo);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.ToString().From(Svo.Elo).Should().Be("1732.4");
            }
        }

        [Test]
        public void from_decimal()
            => Converting.From(1732.4m).To<Elo>().Should().Be(Svo.Elo);

        [Test]
        public void from_double()
            => Converting.From(1732.4).To<Elo>().Should().Be(Svo.Elo);

        [Test]
        public void to_decimal()
            => Converting.To<decimal>().From(Svo.Elo).Should().Be(1732.4m);

        [Test]
        public void to_double()
            => Converting.To<double>().From(Svo.Elo).Should().Be(1732.4);
    
        [TestCase("0", 0)]
        [TestCase("1732.4", -22135344)]
        public void hash_code_is_value_based(Elo svo, int hash)
        {
            using (Hash.WithoutRandomizer())
            {
                svo.GetHashCode().Should().Be(hash);
            }
        }
    }
}
