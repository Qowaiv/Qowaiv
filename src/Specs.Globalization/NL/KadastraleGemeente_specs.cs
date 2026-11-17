using Qowaiv.Globalization.NL;
using Specs.Globalization;

namespace KadastraleGemeente_specs;

public class With_domain_logic
{
    [Test]
    [Arguments("")]
    [Arguments("?")]
    public void has_length_zero_for_empty_and_unknown(KadastraleGemeente svo)
        => svo.Length.Should().Be(0);

    [Test]
    [Arguments(5, "DEL00")]
    public void has_length(int length, KadastraleGemeente svo)
        => svo.Length.Should().Be(length);

    [Test]
    [Arguments(false, "DEL00")]
    [Arguments(false, "?")]
    [Arguments(true, "")]
    public void IsEmpty_returns(bool result, KadastraleGemeente svo)
        => svo.IsEmpty().Should().Be(result);

    [Test]
    [Arguments(false, "DEL00")]
    [Arguments(true, "?")]
    [Arguments(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, KadastraleGemeente svo)
        => svo.IsEmptyOrUnknown().Should().Be(result);

    [Test]
    [Arguments(false, "DEL00")]
    [Arguments(true, "?")]
    [Arguments(false, "")]
    public void IsUnknown_returns(bool result, KadastraleGemeente svo)
        => svo.IsUnknown().Should().Be(result);
}

public class Has_constant
{
    [Test]
    public void Empty() => KadastraleGemeente.Empty.Should().Be(default(KadastraleGemeente));

    [Test]
    public void Unknown() => KadastraleGemeente.Unknown.Should().Be(KadastraleGemeente.Parse("?"));
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.KadastraleGemeente.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.KadastraleGemeente.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.KadastraleGemeente.Equals(KadastraleGemeente.Parse("SBR00")).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.KadastraleGemeente.Equals(KadastraleGemeente.Parse("DEL00")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.KadastraleGemeente == KadastraleGemeente.Parse("DEL00")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.KadastraleGemeente == KadastraleGemeente.Parse("SBR00")).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.KadastraleGemeente != KadastraleGemeente.Parse("DEL00")).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.KadastraleGemeente != KadastraleGemeente.Parse("SBR00")).Should().BeTrue();

    [Test]
    public void hash_code_is_value_based() => HashCodes
        .For(Svo.KadastraleGemeente, default).Should().BeConsistent();
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
        => KadastraleGemeente.Parse(null).Should().Be(KadastraleGemeente.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => KadastraleGemeente.Parse(string.Empty).Should().Be(KadastraleGemeente.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => KadastraleGemeente.Parse("?").Should().Be(KadastraleGemeente.Unknown);

    [Test]
    [Arguments("en", "DEL00")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            KadastraleGemeente.Parse(input).Should().Be(Svo.KadastraleGemeente);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Func<KadastraleGemeente> parse = () => KadastraleGemeente.Parse("invalid input");
            parse.Should().Throw<FormatException>()
                .WithMessage("Not a valid Kadastrale Gemeente");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => (KadastraleGemeente.TryParse("invalid input", out _)).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => KadastraleGemeente.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => KadastraleGemeente.TryParse("DEL00").Should().Be(Svo.KadastraleGemeente);
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.KadastraleGemeente.ToString().Should().Be("DEL00");
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.KadastraleGemeente.ToString().Should().Be(Svo.KadastraleGemeente.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.KadastraleGemeente.ToString().Should().Be(Svo.KadastraleGemeente.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(KadastraleGemeente).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => KadastraleGemeente.Unknown.ToString().Should().Be("?");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.KadastraleGemeente.ToString(FormatProvider.Empty).Should().Be("DEL00");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.KadastraleGemeente.ToString("SomeFormat", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: 'DEL00', format: 'SomeFormat'");
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.KadastraleGemeente.ToString(provider: null).Should().Be("DEL00");
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null() => Svo.KadastraleGemeente.CompareTo(null).Should().Be(1);

    [Test]
    public void to_KadastraleGemeente_as_object()
    {
        object obj = Svo.KadastraleGemeente;
        Svo.KadastraleGemeente.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_KadastraleGemeente_only()
        => Assert.Throws<ArgumentException>(() => Svo.KadastraleGemeente.CompareTo(new object()));

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new KadastraleGemeente[]
        {
            default,
            default,
            KadastraleGemeente.Parse("DEL00"),
            KadastraleGemeente.Parse("DEL01"),
            KadastraleGemeente.Parse("EEX00"),
            KadastraleGemeente.Unknown,
        };

        var list = new List<KadastraleGemeente> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(KadastraleGemeente).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From<string>(null).To<KadastraleGemeente>().Should().Be(KadastraleGemeente.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<KadastraleGemeente>().Should().Be(KadastraleGemeente.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("DEL00").To<KadastraleGemeente>().Should().Be(Svo.KadastraleGemeente);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.KadastraleGemeente).Should().Be("DEL00");
        }
    }
}

public class Supports_JSON_serialization
{
    [Test]
    [Arguments("?", "unknown")]
    public void convention_based_deserialization(KadastraleGemeente svo, object json)
        => JsonTester.Read<KadastraleGemeente>(json).Should().Be(svo);

    [Test]
    [Arguments(null, "")]
    public void convention_based_serialization(object json, KadastraleGemeente svo)
        => JsonTester.Write(svo).Should().Be(json);

    [Test]
    [Arguments("Invalid input", typeof(FormatException))]
    [Arguments("2017-06-11", typeof(FormatException))]
    [Arguments(5L, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        Func<KadastraleGemeente> read = () => JsonTester.Read<KadastraleGemeente>(json);
        read.Should().Throw<Exception>().Subject.Single().Should().BeOfType(exceptionType);
    }
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.KadastraleGemeente);
        xml.Should().Be("DEL00");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<KadastraleGemeente>("DEL00");
        svo.Should().Be(Svo.KadastraleGemeente);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.KadastraleGemeente);
        Svo.KadastraleGemeente.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.KadastraleGemeente);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.KadastraleGemeente;
        obj.GetSchema().Should().BeNull();
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(KadastraleGemeente))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(KadastraleGemeente),
           description: "Kadastrale Gemeente (in the Netherlands)",
           example: "DEL00",
           type: "string",
           format: "akr-code",
           pattern: "[A-Z]{3}[0-9]{2}"));

    [Test]
    [Arguments("DEL00")]
    [Arguments("SBR00")]
    public void pattern_matches(string input)
        => OpenApiDataType.FromType(typeof(KadastraleGemeente))!.Matches(input).Should().BeTrue();
}

public class Debugger
{
    [Test]
    [Arguments("{empty}", "")]
    [Arguments("{unknown}", "?")]
    [Arguments("DEL00", "DEL00")]
    public void has_custom_display(object display, KadastraleGemeente svo)
        => svo.Should().HaveDebuggerDisplay(display);
}
