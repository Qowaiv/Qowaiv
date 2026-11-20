using Qowaiv.Extensions;
using Qowaiv.Locale.NL;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using Specs.Locale;

namespace NL.Burgerservicenummer_specs;

public class With_domain_logic
{
    [Test]
    [Arguments(false, 1234_56_782L)]
    [Arguments(false, "?")]
    [Arguments(true, "")]
    public void IsEmpty_returns(bool result, Burgerservicenummer svo)
        => svo.IsEmpty().Should().Be(result);

    [Test]
    [Arguments(false, 1234_56_782L)]
    [Arguments(true, "?")]
    [Arguments(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, Burgerservicenummer svo)
        => svo.IsEmptyOrUnknown().Should().Be(result);

    [Test]
    [Arguments(false, 1234_56_782L)]
    [Arguments(true, "?")]
    [Arguments(false, "")]
    public void IsUnknown_returns(bool result, Burgerservicenummer svo)
        => svo.IsUnknown().Should().Be(result);

    [Test]
    [Arguments(true, 1234_56_782L)]
    [Arguments(false, "?")]
    [Arguments(false, "")]
    public void IsKnown_returns(bool result, Burgerservicenummer svo)
        => svo.IsKnown.Should().Be(result);
}

public class Has_constant
{
    [Test]
    public void Empty() => Burgerservicenummer.Empty.Should().Be(default);

    [Test]
    public void Unknown() => Burgerservicenummer.Unknown.Should().Be(Burgerservicenummer.Parse("?"));
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Burgerservicenummer.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Burgerservicenummer.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Burgerservicenummer.Equals(1112_22_333L.BSN()).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Burgerservicenummer.Equals(1234_56_782L.BSN()).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.Burgerservicenummer == 1234_56_782L.BSN()).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.Burgerservicenummer == 1112_22_333L.BSN()).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.Burgerservicenummer != 1234_56_782L.BSN()).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.Burgerservicenummer != 1112_22_333L.BSN()).Should().BeTrue();

    [Test]
    public void hash_code_is_value_based()
        => HashCodes.For(Svo.Burgerservicenummer, Burgerservicenummer.Unknown, Burgerservicenummer.Empty).Should().BeConsistent();
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
        => Burgerservicenummer.Parse(null).Should().Be(Burgerservicenummer.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => Burgerservicenummer.Parse(string.Empty).Should().Be(Burgerservicenummer.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => Burgerservicenummer.Parse("?").Should().Be(Burgerservicenummer.Unknown);

    [Test]
    [Arguments("en", "1234.56.782")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            Burgerservicenummer.Parse(input).Should().Be(Svo.Burgerservicenummer);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Func<Burgerservicenummer> parse = () => Burgerservicenummer.Parse("invalid input");
            parse.Should().Throw<FormatException>()
                .WithMessage("Not a valid Burgerservicenummer");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => Burgerservicenummer.TryParse("invalid input", out _).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Burgerservicenummer.TryParse("invalid input").Should().BeNull();

    [Test]
    public void from_invalid_as_null_with_TryParse_and_provider()
        => Burgerservicenummer.TryParse("invalid input", CultureInfo.InvariantCulture).Should().BeNull();

    [Test]
    public void from_invalid_span_throws_on_Parse()
    {
        var parse = () => Burgerservicenummer.Parse("invalid input".AsSpan(), provider: null);
        parse.Should().Throw<FormatException>();
    }

    [Test]
    public void with_TryParse_returns_SVO()
        => Burgerservicenummer.TryParse("123456782").Should().Be(Svo.Burgerservicenummer);
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Burgerservicenummer.ToString().Should().Be("123456782");
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Burgerservicenummer.ToString().Should().Be(Svo.Burgerservicenummer.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Burgerservicenummer.ToString().Should().Be(Svo.Burgerservicenummer.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(Burgerservicenummer).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => Burgerservicenummer.Unknown.ToString().Should().Be("999999999");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Burgerservicenummer.ToString(FormatProvider.Empty).Should().Be("123456782");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Burgerservicenummer.ToString("F", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '1234.56.782', format: 'F'");
    }

    [Test]
    [Arguments("en-GB", null, 1234_56_782L, "123456782")]
    [Arguments("nl-BE", "f", 1234_56_782L, "1234.56.782")]
    public void culture_dependent(CultureInfo culture, string format, Burgerservicenummer svo, string formatted)
    {
        using (culture.Scoped())
        {
            svo.ToString(format).Should().Be(formatted);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.Burgerservicenummer.ToString(provider: null).Should().Be("123456782");
        }
    }

    [Test]
    public void with_destination_too_small()
    {
        var destination = new char[10];

        Svo.Burgerservicenummer.TryFormat(destination, out var charsWritten, "F", provider: null).Should().BeFalse();
        charsWritten.Should().Be(0);
    }
}

public class Is_comparable
{
    [Test]
    public void to_null() => Svo.Burgerservicenummer.CompareTo(null).Should().Be(1);

    [Test]
    public void to_Burgerservicenummer_as_object()
    {
        object obj = Svo.Burgerservicenummer;
        Svo.Burgerservicenummer.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Burgerservicenummer_only()
    {
        var compare = () => Svo.Burgerservicenummer.CompareTo(new object());
        compare.Should().Throw<ArgumentException>();
    }

    [Test]
    public void can_be_sorted_using_compare()
    {
        Burgerservicenummer[] sorted =
        [
            default,
            default,
            1112_22_333L.BSN(),
            1234_56_782L.BSN(),
            2607_74_881L.BSN(),
            Burgerservicenummer.Unknown,
        ];

        var list = new List<Burgerservicenummer> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Casts
{
    [Test]
    public void explicitly_from_long()
    {
        var casted = (Burgerservicenummer)1234_56_782L;
        casted.Should().Be(Svo.Burgerservicenummer);
    }

    [Test]
    public void explicitly_to_long()
    {
        var casted = (long)Svo.Burgerservicenummer;
        casted.Should().Be(1234_56_782L);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Burgerservicenummer).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From<string>(null!).To<Burgerservicenummer>().Should().Be(Burgerservicenummer.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<Burgerservicenummer>().Should().Be(Burgerservicenummer.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("123456782").To<Burgerservicenummer>().Should().Be(Svo.Burgerservicenummer);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Burgerservicenummer).Should().Be("123456782");
        }
    }

    [Test]
    public void from_long()
        => Converting.From(123456782L).To<Burgerservicenummer>().Should().Be(Svo.Burgerservicenummer);

    [Test]
    public void to_long()
        => Converting.To<long>().From(Svo.Burgerservicenummer).Should().Be(123456782);
}

public class Supports_JSON_serialization
{
#if NET10_0_OR_GREATER
    [Test]
    [Arguments("?", "?")]
    [Arguments(1234_56_782L, "1234.56.782")]
    public void System_Text_JSON_deserialization(object json, Burgerservicenummer svo)
        => JsonTester.Read_System_Text_JSON<Burgerservicenummer>(json).Should().Be(svo);

    [Test]
    [Arguments(null, null)]
    [Arguments("1234.56.782", "123456782")]
    public void System_Text_JSON_serialization(Burgerservicenummer svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [Test]
    [Arguments("?", "unknown")]
    [Arguments(1234_56_782L, 1234_56_782L)]
    public void convention_based_deserialization(Burgerservicenummer svo, object json)
        => JsonTester.Read<Burgerservicenummer>(json).Should().Be(svo);

    [Test]
    [Arguments(null, "")]
    public void convention_based_serialization(object json, Burgerservicenummer svo)
        => JsonTester.Write(svo).Should().Be(json);

    [Test]
    [Arguments("Invalid input", typeof(FormatException))]
    [Arguments("2017-06-11", typeof(FormatException))]
    [Arguments(5.0, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        Func<Burgerservicenummer> read = () => JsonTester.Read<Burgerservicenummer>(json);
        read.Should().Throw<Exception>().Subject.Single().Should().BeOfType(exceptionType);
    }
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Burgerservicenummer);
        xml.Should().Be("123456782");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<Burgerservicenummer>("123456782");
        svo.Should().Be(Svo.Burgerservicenummer);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Burgerservicenummer);
        Svo.Burgerservicenummer.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Burgerservicenummer);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Burgerservicenummer;
        obj.GetSchema().Should().BeNull();
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(Burgerservicenummer))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(Burgerservicenummer),
           description: "Burgerservicenummer",
           example: "111222333",
           type: "string",
           format: "bsn",
           pattern: "[0-9]{8,9}"));

    [Test]
    [Arguments("33360030")]
    [Arguments("033360030")]
    public void pattern_matches(string input)
        => OpenApiDataType.FromType(typeof(Burgerservicenummer))!.Matches(input).Should().BeTrue();
}

public class Debugger
{
    [Test]
    [Arguments("{empty}", "")]
    [Arguments("{unknown}", "?")]
    [Arguments("1234.56.782", 1234_56_782L)]
    public void has_custom_display(object display, Burgerservicenummer svo)
        => svo.Should().HaveDebuggerDisplay(display);
}
