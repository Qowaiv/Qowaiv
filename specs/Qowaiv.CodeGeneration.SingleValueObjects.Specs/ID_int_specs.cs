using Qowaiv.Customization;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Specs.ID_int_specs;

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Int32Id.CompareTo((object?)null).Should().Be(1);

    [Test]
    public void to_Int32Id_as_object()
    {
        object obj = Svo.Int32Id;
        Svo.Int32Id.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Int32Id_only()
        => new object().Invoking(Svo.Int32Id.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            default,
            Int32Id.Create(1),
            Int32Id.Create(2),
            Int32Id.Create(3),
            Int32Id.Create(4),
            Int32Id.Create(17),
        };

        var list = new List<Int32Id> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_JSON_serialization
{
    [TestCase("", "")]
    [TestCase(12345678L, 12345678)]
    [TestCase("PREFIX12345678", 12345678)]
    [TestCase("12345678", 12345678)]
    public void System_Text_JSON_deserialization(object json, Int32Id svo)
        => JsonTester.Read_System_Text_JSON<Int32Id>(json).Should().Be(svo);

    [TestCase("", null)]
    [TestCase("12345678", 12345678L)]
    public void System_Text_JSON_serialization(Int32Id svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);

    [TestCase(-2)]
    [TestCase(17)]
    [TestCase(16)]
    [TestCase("17")]
    [TestCase(int.MaxValue + 1L)]
    public void taking_constrains_into_account(object json)
    {
        json.Invoking(JsonTester.Read_System_Text_JSON<EvenOnlyId>)
            .Should().Throw<System.Text.Json.JsonException>()
            .WithMessage("Not a valid identifier");
    }

    [Id<Behavior, int>]
    public readonly partial struct EvenOnlyId
    {
        internal sealed class Behavior : Int32IdBehavior
        {
            public override bool TryTransform(int value, [NotNullWhen(true)] out int transformed)
            {
                if (value % 2 == 0)
                {
                    transformed = value;
                    return true;
                }
                else
                {
                    transformed = default;
                    return false;
                }
            }
        }
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Int32Id).Should().BeDecoratedWith<TypeConverterAttribute>();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Int32Id>().Should().Be(Int32Id.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<Int32Id>().Should().Be(Int32Id.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("PREFIX17").To<Int32Id>().Should().Be(Svo.Int32Id);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Int32Id).Should().Be("PREFIX17");
        }
    }

    [Test]
    public void from_int()
        => Converting.From(17).To<Int32Id>().Should().Be(Svo.Int32Id);

    [Test]
    public void to_int()
        => Converting.To<int>().From(Svo.Int32Id).Should().Be(17);
}
