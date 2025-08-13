using Specs_Generated;

namespace Specs.Customization.ID_string_specs;

public class With_domain_logic
{
    [Test]
    public void Next_generates_unique_ids()
        => Enumerable
            .Range(0, 100)
            .Select(_ => StringBasedId.Next())
            .ToHashSet()
            .Should().HaveCount(100);
}

public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
        => StringBasedId.Empty.Should().Be(default);

    [TestCase(true, "Qowaiv-ID")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, StringBasedId svo) => svo.HasValue.Should().Be(result);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Generated.StringId.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Generated.StringId.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Generated.StringId.Equals(StringBasedId.Next()).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Generated.StringId.Equals(StringBasedId.Parse("Qowaiv-ID")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.Generated.StringId == StringBasedId.Parse("Qowaiv-ID")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.Generated.StringId == StringBasedId.Next()).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.Generated.StringId != StringBasedId.Parse("Qowaiv-ID")).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.Generated.StringId != StringBasedId.Next()).Should().BeTrue();

    [TestCase("", 0)]
    [TestCase("Qowaiv-ID", 890829876)]
    public void hash_code_is_value_based(StringBasedId svo, int hashCode)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hashCode);
        }
    }
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
        => StringBasedId.Parse(null).Should().Be(StringBasedId.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => StringBasedId.Parse(string.Empty).Should().Be(StringBasedId.Empty);

    [TestCase("en", "Qowaiv-ID")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            StringBasedId.Parse(input).Should().Be(Svo.Generated.StringId);
        }
    }

    [Test]
    public void with_TryParse_returns_SVO()
        => StringBasedId.TryParse("Qowaiv-ID").Should().Be(Svo.Generated.StringId);
}


public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Generated.StringId.ToString().Should().Be("Qowaiv-ID");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Generated.StringId.ToString().Should().Be(Svo.Generated.StringId.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Generated.StringId.ToString().Should().Be(Svo.Generated.StringId.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(StringBasedId).ToString().Should().BeEmpty();

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Generated.StringId.ToString(FormatProvider.Empty).Should().Be("Qowaiv-ID");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Generated.StringId.ToString("B", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: 'Qowaiv-ID', format: 'B'");
    }

    [TestCase(null, "Qowaiv-ID")]
    [TestCase("", "Qowaiv-ID")]
    [TestCase("P", "Qowaiv-ID")]
    public void with_current_thread_culture_as_default(string? format, string formattted)
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.Generated.StringId.ToString(format, formatProvider: null).Should().Be(formattted);
        }
    }
}


public class Bytes
{
    [Test]
    public void describe_the_id()
        => Svo.Generated.StringId.ToByteArray()
        .Should()
        .BeEquivalentTo<byte>([0x51, 0x6F, 0x77, 0x61, 0x69, 0x76, 0x2D, 0x49, 0x44]);

    [Test]
    public void init_the_id()
        => StringBasedId.FromBytes([0x51, 0x6F, 0x77, 0x61, 0x69, 0x76, 0x2D, 0x49, 0x44])
        .Should().Be(Svo.Generated.StringId);
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Generated.StringId.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_StringId_as_object()
    {
        object obj = Svo.Generated.StringId;
        Svo.Generated.StringId.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_StringId_only()
        => new object().Invoking(Svo.Generated.StringId.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            StringBasedId.Empty,
            StringBasedId.Parse("33ef5805c472"),
            StringBasedId.Parse("58617a652a14"),
            StringBasedId.Parse("853634b4e474"),
            StringBasedId.Parse("93ca7b438fb3"),
            StringBasedId.Parse("f5e6c39aadcf"),
        };

        var list = new List<StringBasedId> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(StringBasedId).Should().BeDecoratedWith<TypeConverterAttribute>();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<StringBasedId>().Should().Be(StringBasedId.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<StringBasedId>().Should().Be(StringBasedId.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("Qowaiv-ID").To<StringBasedId>().Should().Be(Svo.Generated.StringId);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Generated.StringId).Should().Be("Qowaiv-ID");
        }
    }

    [TestCase(typeof(string))]
    public void to(Type type)
        => Converting.To(type).From<StringBasedId>().Should().BeTrue();
}

public class Does_not_support
{
    [Test]
    public void casting()
    {
        var op_Explicit = typeof(StringBasedId).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(m => m.IsSpecialName && m.Name == "op_Explicit")
            .ToArray();

        op_Explicit.Should().BeEmpty();
    }
}
public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Generated.StringId);
        xml.Should().Be("Qowaiv-ID");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<StringBasedId>("Qowaiv-ID");
        svo.Should().Be(Svo.Generated.StringId);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Generated.StringId);
        round_tripped.Should().Be(Svo.Generated.StringId);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Generated.StringId);
        var round_tripped = SerializeDeserialize.Xml(structure);
        round_tripped.Should().Be(structure);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Generated.StringId;
        obj.GetSchema().Should().BeNull();
    }
}

public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("Qowaiv-ID", "Qowaiv-ID")]
    public void has_custom_display(object display, StringBasedId id)
        => id.Should().HaveDebuggerDisplay(display);
}
