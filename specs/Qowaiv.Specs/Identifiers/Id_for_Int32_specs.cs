using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Specs;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;

namespace Identifiers.Id_for_Int32_specs
{
    public class Supports_type_conversion
    {
        [Test]
        public void from_int()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Svo.Int32Id, 17);
            }
        }

        [Test]
        public void from_string_reperesenting_number()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Svo.Int32Id, "17");
            }
        }

        [Test]
        public void from_string_not_reperesenting_number()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(Svo.Int32Id, "PREFIX17");
            }
        }
    }
}

