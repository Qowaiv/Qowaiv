using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;

namespace Globalization.Country_specs
{
    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
            => typeof(Country).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<Country>().From(null).Should().Be(default);
            }
        }

        [Test]
        public void from_empty_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<Country>().From(string.Empty).Should().Be(default);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<Country>().From("SvoValue").Should().Be(Svo.Country);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.Value(Svo.Country).ToString().Should().Be("SvoValue");
            }
        }

        [Test]
        public void from_int()
            => Converting.To<Country>().From(17).Should().Be(Svo.Country);

        [Test]
        public void to_int()
            => Converting.Value(Svo.Country).To<int>().Should().Be(17);
    }

}
