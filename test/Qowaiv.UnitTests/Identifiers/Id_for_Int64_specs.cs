using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Identifiers;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using SpecialInt64 = Qowaiv.Identifiers.Id<Identifiers.Id_for_Int64_specs.ForSpecialInt64>;

namespace Identifiers.Id_for_Int64_specs
{
    public class Supports_type_conversion
    {
        [Test]
        public void from_long()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(SpecialInt64.Create(17), 17L);
            }
        }

        [Test]
        public void from_int()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(SpecialInt64.Create(17), 17);
            }
        }

        [Test]
        public void from_string_reperesenting_number()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(SpecialInt64.Create(17), "17");
            }
        }

        [Test]
        public void from_string_not_reperesenting_number()
        {
            using (TestCultures.En_GB.Scoped())
            {
                TypeConverterAssert.ConvertFromEquals(SpecialInt64.Create(17), "PREFIX17");
            }
        }
    }

    internal sealed class ForSpecialInt64 : Int64IdBehavior
    {
        public override string ToString(object obj, string format, IFormatProvider formatProvider)
            => $"PREFIX{obj:format}";

        public override bool TryParse(string str, out object id)
            => (str ?? "").StartsWith("PREFIX")
            ? base.TryParse(str?[6..], out id)
            : base.TryParse(str, out id);
    }
}

