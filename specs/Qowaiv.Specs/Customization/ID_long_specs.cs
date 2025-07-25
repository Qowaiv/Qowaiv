using Specs_Generated;

namespace Specs.Customization.ID_long_specs;

public class With_domain_logic
{
    [Test]
    public void Next_is_not_supported()
    {
        Func<Int64BasedId> next = () => Int64BasedId.Next();
        next.Should().Throw<NotSupportedException>();
    }
}

public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
        => Int64BasedId.Empty.Should().Be(default);

    [TestCase(true, 17)]
    [TestCase(false, "")]
    public void HasValue_is(bool result, Int64BasedId svo) => svo.HasValue.Should().Be(result);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Generated.Int64Id.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Generated.Int64Id.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Generated.Int64Id.Equals(Int64BasedId.Create(42)).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Generated.Int64Id.Equals(Int64BasedId.Create(987654321L)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.Generated.Int64Id == Int64BasedId.Create(987654321L)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.Generated.Int64Id == Int64BasedId.Create(42)).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.Generated.Int64Id != Int64BasedId.Create(987654321L)).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.Generated.Int64Id != Int64BasedId.Create(42)).Should().BeTrue();

    [TestCase("", 0)]
    [TestCase(17, 665630146)]
    public void hash_code_is_value_based(Int64BasedId svo, int hashCode)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hashCode);
        }
    }
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Generated.Int64Id.ToString().Should().Be("PREFIX987654321");
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Generated.Int64Id.ToString().Should().Be(Svo.Generated.Int64Id.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Generated.Int64Id.ToString().Should().Be(Svo.Generated.Int64Id.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(Int64BasedId).ToString().Should().BeEmpty();

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Generated.Int64Id.ToString(FormatProvider.Empty).Should().Be("PREFIX987654321");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Generated.Int64Id.ToString("X", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: 'PREFIX3ADE68B1', format: 'X'");
    }

    [TestCase(null, "PREFIX987654321")]
    [TestCase("", "PREFIX987654321")]
    [TestCase("X", "PREFIX3ADE68B1")]
    public void with_current_thread_culture_as_default(string? format, string formattted)
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.Generated.Int64Id.ToString(format, formatProvider: null).Should().Be(formattted);
        }
    }
}

public class Bytes
{
    [Test]
    public void describe_the_id()
        => Svo.Generated.Int64Id.ToByteArray()
        .Should()
        .BeEquivalentTo<byte>([0xB1, 0x68, 0xDE, 0x3A, 0x00, 0x00, 0x00, 0x00]);

    [Test]
    public void init_the_id()
        => Int64BasedId.FromBytes([0xB1, 0x68, 0xDE, 0x3A, 0x00, 0x00, 0x00, 0x00])
        .Should().Be(Svo.Generated.Int64Id);
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Generated.Int64Id.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Int64Id_as_object()
    {
        object obj = Svo.Generated.Int64Id;
        Svo.Generated.Int64Id.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Int64Id_only()
        => new object().Invoking(Svo.Generated.Int64Id.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            Int64BasedId.Empty,
            Int64BasedId.Create(1L),
            Int64BasedId.Create(3L),
            Int64BasedId.Create(7L),
            Int64BasedId.Create(11L),
            Int64BasedId.Create(17L),
        };

        var list = new List<Int64BasedId> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

#if NET6_0_OR_GREATER
public class Supports_JSON_serialization
{
    [TestCase("", null)]
    [TestCase(null, null)]
    [TestCase(123456789L, 123456789L)]
    [TestCase("123456789", 123456789L)]
    public void System_Text_JSON_deserialization(object json, Int64BasedId svo)
        => JsonTester.Read_System_Text_JSON<Int64BasedId>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase(123456789L, 123456789)]
    public void System_Text_JSON_serialization(Int64BasedId svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
}
#endif

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Int64BasedId).Should().BeDecoratedWith<TypeConverterAttribute>();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Int64BasedId>().Should().Be(Int64BasedId.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<Int64BasedId>().Should().Be(Int64BasedId.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("PREFIX987654321").To<Int64BasedId>().Should().Be(Svo.Generated.Int64Id);
        }
    }

    [Test]
    public void to_string()
        => Converting.ToString().From(Svo.Generated.Int64Id).Should().Be("PREFIX987654321");

    [Test]
    public void from_int()
       => Converting.From(42).To<Int64BasedId>().Should().Be(Int64BasedId.Create(42));

    [Test]
    public void to_int()
        => Converting.To<int>().From(Int64BasedId.Create(42)).Should().Be(42);

    [Test]
    public void from_long()
        => Converting.From(987654321L).To<Int64BasedId>().Should().Be(Svo.Generated.Int64Id);

    [Test]
    public void to_long()
        => Converting.To<long>().From(Svo.Generated.Int64Id).Should().Be(987654321L);

    [TestCase("8a1a8c42-d2ff-e254-e26e-b6abcbf19420")]
    [TestCase("PREF17")]
    public void from_invalid_string(string str)
    {
        Func<Int64BasedId> convert = () => Converting.From(str).To<Int64BasedId>();
        convert.Should().Throw<InvalidCastException>()
            .WithMessage("Cast from string to Specs_Generated.Int64BasedId is not valid.");
    }

    [Test]
    public void from_invalid_number()
    {
        Func<Int64BasedId> convert = () => Converting.From(-18L).To<Int64BasedId>();
        convert.Should().Throw<InvalidCastException>()
            .WithMessage("Cast from long to Specs_Generated.Int64BasedId is not valid.");
    }

    [TestCase(typeof(int))]
    [TestCase(typeof(long))]
    [TestCase(typeof(string))]
    public void to(Type type)
        => Converting.To(type).From<Int64BasedId>().Should().BeTrue();
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Generated.Int64Id);
        xml.Should().Be("PREFIX987654321");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<Int64BasedId>("PREFIX987654321");
        svo.Should().Be(Svo.Generated.Int64Id);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Generated.Int64Id);
        round_tripped.Should().Be(Svo.Generated.Int64Id);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Generated.Int64Id);
        var round_tripped = SerializeDeserialize.Xml(structure);
        round_tripped.Should().Be(structure);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Generated.Int64Id;
        obj.GetSchema().Should().BeNull();
    }

    [Test]
    public void to_underlying_via_explicit_cast()
    {
        var cast = (long)Svo.Generated.Int64Id;
        cast.Should().Be(987654321);
    }
}

public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("PREFIX42", "42")]
    public void has_custom_display(object display, Int64BasedId id)
        => id.Should().HaveDebuggerDisplay(display);
}
