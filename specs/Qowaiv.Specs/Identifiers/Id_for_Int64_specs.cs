using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.Identifiers;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.ComponentModel;
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

        [TestCase("666")]
        [TestCase("PREF17")]
        public void from_invalid_string(string str)
        {
            var converter = TypeDescriptor.GetConverter(typeof(SpecialInt64));
            Action convert = () => converter.ConvertFrom(str);
            convert.Should().Throw<InvalidCastException>()
                .WithMessage("Cast from string to Qowaiv.Identifiers.Id<Identifiers.Id_for_Int64_specs.ForSpecialInt64> is not valid.");
        }

        [TestCase(-17)]
        [TestCase(666)]
        public void from_invalid_number(long number)
        {
            var converter = TypeDescriptor.GetConverter(typeof(SpecialInt64));
            Action convert = () => converter.ConvertFrom(number);
            convert.Should().Throw<InvalidCastException>()
                .WithMessage("Cast from long to Qowaiv.Identifiers.Id<Identifiers.Id_for_Int64_specs.ForSpecialInt64> is not valid.");
        }
    }

    internal sealed class ForSpecialInt64 : Int64IdBehavior
    {
        public override string ToString(object obj, string format, IFormatProvider formatProvider)
            => string.Format(formatProvider, $"PREFIX{{0:{format}}}", obj);

        public override bool TryCreate(object obj, out object id)
            => base.TryCreate(obj, out id)
            && id is long number
            && IsValid(number);

        public override bool TryParse(string str, out object id)
            => (str ?? "").StartsWith("PREFIX")
            ? base.TryParse(str?[6..], out id)
            : base.TryParse(str, out id) && IsValid(id);

        private static bool IsValid(long number) => number % 2 == 1;
    }
}

