using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Security.Cryptography;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;

namespace Security.Cryptography.CryptographicSeed_specs
{
    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
            => typeof(CryptographicSeed).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From<string>(null).To<CryptographicSeed>().Should().Be(default);
            }
        }

        [Test]
        public void from_empty_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From(string.Empty).To<CryptographicSeed>().Should().Be(default);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.From("Qowaiv==").To<CryptographicSeed>().Should().Be(Svo.CryptographicSeed);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.ToString().From(Svo.CryptographicSeed).Should().Be("Qowaig==");
            }
        }
    }
}
