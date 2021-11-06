using FluentAssertions;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Globalization;
using Qowaiv.Hashing;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;

namespace Date_specs
{
    public class Is_equal_by_value
    {
        [Test]
        public void not_equal_to_null()
            => Svo.Date.Equals(null).Should().BeFalse();

        [Test]
        public void not_equal_to_other_type()
            => Svo.Date.Equals(new object()).Should().BeFalse();

        [Test]
        public void not_equal_to_different_value()
            => Svo.Date.Equals(Date.MinValue).Should().BeFalse();

        [Test]
        public void equal_to_same_value()
            => Svo.Date.Equals(new Date(2017, 06, 11)).Should().BeTrue();

        [Test]
        public void equal_operator_returns_true_for_same_values()
            => (new Date(2017, 06, 11) == Svo.Date).Should().BeTrue();

        [Test]
        public void equal_operator_returns_false_for_different_values()
            => (new Date(2017, 06, 11) == Date.MinValue).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_false_for_same_values()
            => (new Date(2017, 06, 11) != Svo.Date).Should().BeFalse();

        [Test]
        public void not_equal_operator_returns_true_for_different_values()
            => (new Date(2017, 06, 11) != Date.MinValue).Should().BeTrue();

        [TestCase("", 0)]
        [TestCase("yes", 20170609)]
        public void hash_code_is_value_based(YesNo svo, int hash)
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
            => typeof(Date).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From<string>(null).To<Date>().Should().Be(default);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From("2017-06-11").To<Date>().Should().Be(Svo.Date);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.ToString().From(Svo.Date).Should().Be("11/06/2017");
            }
        }

        [Test]
        public void from_DateTime()
            => Converting.From(new DateTime(2017, 06, 11)).To<Date>().Should().Be(Svo.Date);

        [Test]
        public void from_DateTimeOffset()
            => Converting.From(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero)).To<Date>().Should().Be(Svo.Date);

        [Test]
        public void from_LocalDateTime()
            => Converting.From(new LocalDateTime(2017, 06, 11)).To<Date>().Should().Be(Svo.Date);

        [Test]
        public void from_WeekDate()
            => Converting.From(new WeekDate(2017, 23, 7)).To<Date>().Should().Be(Svo.Date);

        [Test]
        public void to_DateTime()
            => Converting.To<DateTime>().From(Svo.Date).Should().Be(new DateTime(2017, 06, 11));

        [Test]
        public void to_DateTimeOffset()
            => Converting.To<DateTimeOffset>().From(Svo.Date).Should().Be(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero));

        [Test]
        public void to_LocalDateTime()
            => Converting.To<LocalDateTime>().From(Svo.Date).Should().Be(new LocalDateTime(2017, 06, 11));

        [Test]
        public void to_WeekDate()
            => Converting.To<WeekDate>().From(Svo.Date).Should().Be(new WeekDate(2017, 23, 7));
    }
}
