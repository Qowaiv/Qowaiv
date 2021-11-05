using FluentAssertions;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Globalization;

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
                Converting.From<string>(null).To<WeekDate>().Should().Be(default);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From("2017-W23-7").To<WeekDate>().Should().Be(Svo.WeekDate);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.ToString().From(Svo.WeekDate).Should().Be("2017-W23-7");
            }
        }

        [Test]
        public void from_Date()
            => Converting.From(new Date(2017, 06, 11)).To<WeekDate>().Should().Be(Svo.WeekDate);

        [Test]
        public void from_DateTime()
            => Converting.From(new DateTime(2017, 06, 11)).To<WeekDate>().Should().Be(Svo.WeekDate);

        [Test]
        public void from_DateTimeOffset()
            => Converting.From(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero)).To<WeekDate>().Should().Be(Svo.WeekDate);

        [Test]
        public void from_LocalDateTime()
            => Converting.From(new LocalDateTime(2017, 06, 11)).To<WeekDate>().Should().Be(Svo.WeekDate);

        [Test]
        public void to_Date()
            => Converting.To<Date>().From(Svo.WeekDate).Should().Be(new Date(2017, 06, 11));

        [Test]
        public void to_DateTime()
            => Converting.To<DateTime>().From(Svo.WeekDate).Should().Be(new DateTime(2017, 06, 11));

        [Test]
        public void to_DateTimeOffset()
            => Converting.To<DateTimeOffset>().From(Svo.WeekDate).Should().Be(new DateTimeOffset(2017, 06, 11, 00, 00, 00, TimeSpan.Zero));

        [Test]
        public void to_LocalDateTime()
            => Converting.To<LocalDateTime>().From(Svo.WeekDate).Should().Be(new LocalDateTime(2017, 06, 11));
    }
}
