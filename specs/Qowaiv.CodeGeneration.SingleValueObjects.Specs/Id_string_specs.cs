using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Specs.ID_string_specs;

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.StringId.CompareTo((object?)null).Should().Be(1);

    [Test]
    public void to_StringId_as_object()
    {
        object obj = Svo.StringId;
        Svo.StringId.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_StringId_only()
        => new object().Invoking(Svo.StringId.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            StringId.Empty,
            StringId.Parse("33ef5805c472"),
            StringId.Parse("58617a652a14"),
            StringId.Parse("853634b4e474"),
            StringId.Parse("93ca7b438fb3"),
            StringId.Parse("f5e6c39aadcf"),
        };

        var list = new List<StringId> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(StringId).Should().BeDecoratedWith<TypeConverterAttribute>();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<StringId>().Should().Be(StringId.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<StringId>().Should().Be(StringId.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("Qowaiv-ID").To<StringId>().Should().Be(Svo.StringId);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.StringId).Should().Be("Qowaiv-ID");
        }
    }
}
