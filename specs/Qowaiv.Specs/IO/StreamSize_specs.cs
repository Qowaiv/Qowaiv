using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.IO;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;

namespace IO.StreamSize_specs
{
    public class Supports_type_conversion
    {
        [Test]
        public void via_TypeConverter_registered_with_attribute()
            => typeof(StreamSize).Should().HaveTypeConverterDefined();

        [Test]
        public void from_null_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<StreamSize>().From(null).Should().Be(default);
            }
        }

        [Test]
        public void from_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.To<StreamSize>().From("123456789").Should().Be(Svo.StreamSize);
            }
        }

        [Test]
        public void to_string()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Converting.Value(Svo.StreamSize).ToString().Should().Be("123456789 byte");
            }
        }

        [Test]
        public void from_long()
            => Converting.To<StreamSize>().From(123456789L).Should().Be(Svo.StreamSize);

        [Test]
        public void to_long()
            => Converting.Value(Svo.StreamSize).To<long>().Should().Be(123456789);
    }
}
