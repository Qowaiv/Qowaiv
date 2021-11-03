using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Specs;
using Qowaiv.Statistics;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;

namespace Statistics.Elo_specs
{
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
                Converting.To<Elo>().From(null).Should().Be(Elo.Zero);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<Elo>().From("1732.4").Should().Be(Svo.Elo);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.Value(Svo.Elo).ToString().Should().Be("1732.4");
            }
        }

        [Test]
        public void from_decimal()
            => Converting.To<Elo>().From(1732.4m).Should().Be(Svo.Elo);

        [Test]
        public void from_double()
            => Converting.To<Elo>().From(1732.4).Should().Be(Svo.Elo);

        [Test]
        public void to_decimal()
            => Converting.Value(Svo.Elo).To<decimal>().Should().Be(1732.4m);

        [Test]
        public void to_double()
            => Converting.Value(Svo.Elo).To<double>().Should().Be(1732.4);
    }

}
