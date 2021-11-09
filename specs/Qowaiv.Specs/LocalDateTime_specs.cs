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
        [TestCase("2017-06-11 06:15", 533532482)]
        public void hash_code_is_value_based(LocalDateTime svo, int hash)
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
                Converting.From<string>(null).To<LocalDateTime>().Should().Be(default);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From("2017-06-11 06:15:00").To<LocalDateTime>().Should().Be(Svo.LocalDateTime);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.ToString().From(Svo.LocalDateTime).Should().Be("11/06/2017 06:15:00");
            }
        }

        [Test]
        public void from_DateTime()
            => Converting.From(Svo.DateTime).To<LocalDateTime>().Should().Be(Svo.LocalDateTime);

        [Test]
        public void from_DateTimeOffset()
            => Converting.From(Svo.DateTimeOffset).To<LocalDateTime>().Should().Be(Svo.LocalDateTime);

        [Test]
        public void from_Date()
            => Converting.From(Svo.Date).To<LocalDateTime>().Should().Be(new LocalDateTime(2017, 06, 11));

        [Test]
        public void from_WeekDate()
            => Converting.From(Svo.WeekDate).To<LocalDateTime>().Should().Be(new LocalDateTime(2017, 06, 11));

        [Test]
        public void to_DateTime()
            => Converting.To<DateTime>().From(Svo.LocalDateTime).Should().Be(Svo.DateTime);

        [Test]
        public void to_DateTimeOffset()
            => Converting.To<DateTimeOffset>().From(Svo.LocalDateTime).Should().Be(Svo.DateTimeOffset);
           
        [Test]
        public void to_Date()
            => Converting.To<Date>().From(Svo.LocalDateTime).Should().Be(Svo.Date);

        [Test]
        public void to_WeekDate()
            => Converting.To<WeekDate>().From(Svo.LocalDateTime).Should().Be(Svo.WeekDate);
    }
}
