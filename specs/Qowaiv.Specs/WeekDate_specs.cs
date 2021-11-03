using FluentAssertions;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Globalization;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;

namespace WeekDate_specs
{
    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
            => typeof(WeekDate).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<WeekDate>().From(null).Should().Be(default);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<WeekDate>().From("2017-W23-7").Should().Be(Svo.WeekDate);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.Value(Svo.WeekDate).ToString().Should().Be("2017-W23-7");
            }
        }

        [Test]
        public void from_Date()
            => Converting.To<WeekDate>().From(new Date(2017, 06, 11)).Should().Be(Svo.WeekDate);

        [Test]
        public void from_DateTime()
            => Converting.To<WeekDate>().From(new DateTime(2017, 06, 11)).Should().Be(Svo.WeekDate);

        [Test]
        public void from_DateTimeOffset()
            => Converting.To<WeekDate>().From(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero)).Should().Be(Svo.WeekDate);

        [Test]
        public void from_LocalDateTime()
            => Converting.To<WeekDate>().From(new LocalDateTime(2017, 06, 11)).Should().Be(Svo.WeekDate);

        [Test]
        public void to_Date()
            => Converting.Value(Svo.WeekDate).To<Date>().Should().Be(new Date(2017, 06, 11));

        [Test]
        public void to_DateTime()
            => Converting.Value(Svo.WeekDate).To<DateTime>().Should().Be(new DateTime(2017, 06, 11));

        [Test]
        public void to_DateTimeOffset()
            => Converting.Value(Svo.WeekDate).To<DateTimeOffset>().Should().Be(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero));

        [Test]
        public void to_LocalDateTime()
            => Converting.Value(Svo.WeekDate).To<LocalDateTime>().Should().Be(new LocalDateTime(2017, 06, 11));
    }
}
