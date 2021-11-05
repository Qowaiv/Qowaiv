using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;

namespace Financial.Money_specs
{
    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
           => typeof(Money).Should().HaveTypeConverterDefined();

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From("€42.17").To<Money>().Should().Be(Svo.Money);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.ToString().From(Svo.Money).Should().Be("€42.17");
            }
        }
    }
}
