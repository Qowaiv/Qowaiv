using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;

namespace Financial.BIC_specs
{
    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
            => typeof(BusinessIdentifierCode).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<BusinessIdentifierCode>().From(null).Should().Be(default);
            }
        }

        [Test]
        public void from_empty_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<BusinessIdentifierCode>().From(string.Empty).Should().Be(default);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<BusinessIdentifierCode>().From("AEGONL2UXXX").Should().Be(Svo.BusinessIdentifierCode);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.Value(Svo.BusinessIdentifierCode).ToString().Should().Be("AEGONL2UXXX");
            }
        }
    }
}
