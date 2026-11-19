using Qowaiv.Globalization.NL;
using Specs.Globalization;

namespace KvkNummer_specs;

public class With_domain_logic
{
    [Test]
    [Arguments("")]
    [Arguments("?")]
    public void has_length_zero_for_empty_and_unknown(KvkNummer svo)
        => svo.Length.Should().Be(0);

    [Test]
    [Arguments(0, null)]
    [Arguments(0, "?")]
    [Arguments(8, "00640540")]
    [Arguments(8, "08640540")]
    [Arguments(8, "87640540")]
    public void has_length(int length, KvkNummer svo)
        => svo.Length.Should().Be(length);

    [Test]
    [Arguments(false, "87640540")]
    [Arguments(false, "?")]
    [Arguments(true, "")]
    public void IsEmpty_returns(bool result, KvkNummer svo)
        => svo.IsEmpty().Should().Be(result);

    [Test]
    [Arguments(false, "87640540")]
    [Arguments(true, "?")]
    [Arguments(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, KvkNummer svo)
        => svo.IsEmptyOrUnknown().Should().Be(result);

    [Test]
    [Arguments(false, "87640540")]
    [Arguments(true, "?")]
    [Arguments(false, "")]
    public void IsUnknown_returns(bool result, KvkNummer svo)
        => svo.IsUnknown().Should().Be(result);
}

public class Has_constant
{
    [Test]
    public void Empty() => KvkNummer.Empty.Should().Be(default(KvkNummer));

    [Test]
    public void Unknown() => KvkNummer.Unknown.Should().Be(KvkNummer.Parse("?"));
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.KvkNummer.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.KvkNummer.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.KvkNummer.Equals(0764_0542.KvK()).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.KvkNummer.Equals(8764_0540.KvK()).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.KvkNummer == 8764_0540.KvK()).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.KvkNummer == 0764_0542.KvK()).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.KvkNummer != 8764_0540.KvK()).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.KvkNummer != 0764_0542.KvK()).Should().BeTrue();

    [Test]
    public void hash_code_is_value_based() => HashCodes
        .For(Svo.KvkNummer, default).Should().BeConsistent();
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
        => KvkNummer.Parse(null).Should().Be(KvkNummer.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => KvkNummer.Parse(string.Empty).Should().Be(KvkNummer.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => KvkNummer.Parse("?").Should().Be(KvkNummer.Unknown);

    [Test]
    [Arguments("123456", "6 digits")]
    [Arguments("1234567", "7 digits")]
    [Arguments("12345678", "8 digits")]
    [Arguments("01234567", "1 leading zero")]
    [Arguments("00123456", "2 leading zeros")]
    public void from(string str, string because)
        => KvkNummer.TryParse(str).Should().NotBeNull(because);

    [Test]
    [Arguments("en", "87640540")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            KvkNummer.Parse(input).Should().Be(Svo.KvkNummer);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Func<KvkNummer> parse = () => KvkNummer.Parse("invalid input");
            parse.Should().Throw<FormatException>()
                .WithMessage("Not a valid KvK number");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => (KvkNummer.TryParse("invalid input", out _)).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => KvkNummer.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => KvkNummer.TryParse("87640540").Should().Be(Svo.KvkNummer);
}

public class Can_not_parse
{
    [Test]
    [Arguments("12345", "5 digits")]
    [Arguments("123456789", "9 digits")]
    [Arguments("012345678", "9 digits wiht leading zeros")]
    [Arguments("012345", "6 digits with 1 leading zero")]
    [Arguments("00012345", "3 leading zeros")]
    public void from(string str, string because)
        => KvkNummer.TryParse(str).Should().BeNull(because);

}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.KvkNummer.ToString().Should().Be("87640540");
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.KvkNummer.ToString().Should().Be(Svo.KvkNummer.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.KvkNummer.ToString().Should().Be(Svo.KvkNummer.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(KvkNummer).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => KvkNummer.Unknown.ToString().Should().Be("?");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.KvkNummer.ToString(FormatProvider.Empty).Should().Be("87640540");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.KvkNummer.ToString("F", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '8764 0540', format: 'F'");
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.KvkNummer.ToString(provider: null).Should().Be("87640540");
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null() => Svo.KvkNummer.CompareTo(null).Should().Be(1);

    [Test]
    public void to_KvkNummer_as_object()
    {
        object obj = Svo.KvkNummer;
        Svo.KvkNummer.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_KvkNummer_only()
        => Assert.Throws<ArgumentException>(() => Svo.KvkNummer.CompareTo(new object()));

    [Test]
    public void can_be_sorted_using_compare()
    {
        KvkNummer[] sorted = new KvkNummer[]
        {
            default,
            default,
           01234567.KvK(),
           41234659.KvK(),
           81230067.KvK(),
            KvkNummer.Unknown,
        };

        var list = new List<KvkNummer> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Casts
{
    [Test]
    [Arguments(null, null)]
    [Arguments(8764_0540, 8764_0540)]
    public void explicitly_from_int(int? value, KvkNummer kvk)
    {
        var casted = (KvkNummer)value;
        casted.Should().Be(kvk);
    }

    [Test]
    [Arguments(null, null)]
    [Arguments("?", null)]
    [Arguments(8764_0540, 8764_0540)]
    public void explicitly_to_int(KvkNummer kvk, int? result)
    {
        var casted = (int?)kvk;
        casted.Should().Be(result);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(KvkNummer).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From<string>(null).To<KvkNummer>().Should().Be(KvkNummer.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<KvkNummer>().Should().Be(KvkNummer.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("87640540").To<KvkNummer>().Should().Be(Svo.KvkNummer);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.KvkNummer).Should().Be("87640540");
        }
    }

    [Test]
    public void from_int()
        => Converting.From(87640540).To<KvkNummer>().Should().Be(Svo.KvkNummer);

    [Test]
    public void to_int()
        => Converting.To<int>().From(Svo.KvkNummer).Should().Be(87640540);
}

public class Supports_JSON_serialization
{
    [Test]
    [Arguments("?", "unknown")]
    public void convention_based_deserialization(KvkNummer svo, object json)
        => JsonTester.Read<KvkNummer>(json).Should().Be(svo);

    [Test]
    [Arguments(null, "")]
    public void convention_based_serialization(object json, KvkNummer svo)
        => JsonTester.Write(svo).Should().Be(json);

#if NET10_0_OR_GREATER
    [Test]
    [Arguments("87640540", "87640540")]
    [Arguments("?", "?")]
    public void System_Text_JSON_deserialization(object json, KvkNummer svo)
        => JsonTester.Read_System_Text_JSON<KvkNummer>(json).Should().Be(svo);

    [Test]
    [Arguments("87640540", "87640540")]
    [Arguments("?", "?")]
    public void System_Text_JSON_serialization(KvkNummer svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif

    [Test]
    [Arguments("Invalid input", typeof(FormatException))]
    [Arguments(12345L, typeof(ArgumentOutOfRangeException))]
    [Arguments(12345678902342L, typeof(InvalidCastException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        Func<KvkNummer> read = () => JsonTester.Read<KvkNummer>(json);
        read.Should().Throw<Exception>().Subject.Single().Should().BeOfType(exceptionType);
    }
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.KvkNummer);
        xml.Should().Be("87640540");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<KvkNummer>("87640540");
        svo.Should().Be(Svo.KvkNummer);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.KvkNummer);
        Svo.KvkNummer.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.KvkNummer);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.KvkNummer;
        obj.GetSchema().Should().BeNull();
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(KvkNummer))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(KvkNummer),
           description: "KvK-nummer (Kamer van Koophandel)",
           example: "08543897",
           type: "string",
           format: "kvknr",
           pattern: "[0-9]{8}"));

    [Test]
    [Arguments("87640540")]
    [Arguments("00640540")]
    public void pattern_matches(string input)
        => OpenApiDataType.FromType(typeof(KvkNummer))!.Matches(input).Should().BeTrue();
}

public class Debugger
{
    [Test]
    [Arguments("{empty}", "")]
    [Arguments("{unknown}", "?")]
    [Arguments("8764 0540", "87640540")]
    public void has_custom_display(object display, KvkNummer svo)
        => svo.Should().HaveDebuggerDisplay(display);
}
