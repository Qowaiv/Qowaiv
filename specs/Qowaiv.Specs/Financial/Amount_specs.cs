using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;

namespace Financial.Amount_specs
{
    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
           => typeof(Amount).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<Amount>().From(null).Should().Be(Amount.Zero);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<Amount>().From("42.17").Should().Be(Svo.Amount);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.Value(Svo.Amount).ToString().Should().Be("42.17");
            }
        }

        [Test]
        public void from_decimal()
            => Converting.To<Amount>().From(42.17m).Should().Be(Svo.Amount);

        [Test]
        public void from_double()
            => Converting.To<Amount>().From(42.17).Should().Be(Svo.Amount);

        [Test]
        public void to_decimal()
            => Converting.Value(Svo.Amount).To<decimal>().Should().Be(42.17m);

        [Test]
        public void to_double()
            => Converting.Value(Svo.Amount).To<double>().Should().Be(42.17);
    }

}
