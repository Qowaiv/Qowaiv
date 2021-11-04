using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Hashing;
using Qowaiv.IO;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;

namespace IO.StreamSize_specs
{
    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
            => Svo.StreamSize.Equals(null).Should().BeFalse();

        [Test]
        public void not_equal_to_other_type()
            => Svo.StreamSize.Equals(new object()).Should().BeFalse();

        [Test]
        public void not_equal_to_different_value()
            => Svo.StreamSize.Equals(StreamSize.MinValue).Should().BeFalse();

        [Test]
        public void equal_to_same_value()
            => Svo.StreamSize.Equals(StreamSize.Byte * 123456789).Should().BeTrue();

        [Test]
        public void equal_operator_returns_true_for_same_values()
            => (StreamSize.Byte * 123456789 == Svo.StreamSize).Should().BeTrue();

        [Test]
        public void equal_operator_returns_false_for_different_values()
            => (StreamSize.Byte * 123456789 == StreamSize.MinValue).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
            => (StreamSize.Byte * 123456789 != Svo.StreamSize).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
            => (StreamSize.Byte * 123456789 != StreamSize.MinValue).Should().BeTrue();

        [TestCase("0 byte", 0)]
        [TestCase("123456789 byte", 107481702)]
        public void hash_code_is_value_based(StreamSize svo, int hash)
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
            => typeof(StreamSize).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<StreamSize>().From(null).Should().Be(default);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<StreamSize>().From("123456789").Should().Be(Svo.StreamSize);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.Value(Svo.StreamSize).ToString().Should().Be("123456789 byte");
            }
        }

        [Test]
        public void from_long()
            => Converting.To<StreamSize>().From(123456789L).Should().Be(Svo.StreamSize);

        [Test]
        public void to_long()
            => Converting.Value(Svo.StreamSize).To<long>().Should().Be(123456789);
    }
}
