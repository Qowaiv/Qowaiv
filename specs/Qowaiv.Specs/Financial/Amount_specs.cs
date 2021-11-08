using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System.Globalization;

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
                Converting.From<string>(null).To<Amount>().Should().Be(Amount.Zero);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From("42.17").To<Amount>().Should().Be(Svo.Amount);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<string>().From(Svo.Amount).Should().Be("42.17");
            }
        }

        [Test]
        public void from_decimal()
            => Converting.From(42.17m).To<Amount>().Should().Be(Svo.Amount);

        [Test]
        public void from_double()
            => Converting.From(42.17).To<Amount>().Should().Be(Svo.Amount);

        [Test]
        public void to_decimal()
            => Converting.To<decimal>().From(Svo.Amount).Should().Be(42.17m);

        [Test]
        public void to_double()
            => Converting.To<double>().From(Svo.Amount).Should().Be(42.17);
    }

}
