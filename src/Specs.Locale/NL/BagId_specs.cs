using Qowaiv.Locale.NL;
using Specs.Locale;

namespace BagId_specs;

public class With_domain_logic
{
    [Test]
    [Arguments("")]
    [Arguments("?")]
    public void has_length_zero_for_empty_and_unknown(BagId svo)
        => svo.Length.Should().Be(0);

    [Test]
    [Arguments(0, null)]
    [Arguments(0, "?")]
    [Arguments(16, "0363010123456789")]
    public void has_length(int length, BagId svo)
        => svo.Length.Should().Be(length);

    [Test]
    [Arguments(false, "0363010123456789")]
    [Arguments(false, "?")]
    [Arguments(true, "")]
    public void IsEmpty_returns(bool result, BagId svo)
        => svo.IsEmpty().Should().Be(result);

    [Test]
    [Arguments(false, "0363010123456789")]
    [Arguments(true, "?")]
    [Arguments(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, BagId svo)
        => svo.IsEmptyOrUnknown().Should().Be(result);

    [Test]
    [Arguments(false, "0363010123456789")]
    [Arguments(true, "?")]
    [Arguments(false, "")]
    public void IsUnknown_returns(bool result, BagId svo)
        => svo.IsUnknown().Should().Be(result);

    [Test]
    [Arguments(0363, "0363010123456789")]
    [Arguments(9999, "?")]
    [Arguments(0000, "")]
    public void Gemeente_code(int gemeente, BagId svo)
        => svo.Gemeente.Should().Be(gemeente);

    [Test]
    [Arguments(BagId.ObjectType.Verblijfsobject, "0363010123456789")]
    [Arguments(BagId.ObjectType.Unknown, "?")]
    [Arguments(BagId.ObjectType.None, "")]
    public void Object_type(BagId.ObjectType type, BagId svo)
        => svo.Object.Should().Be(type);

    [Test]
    [Arguments(8_123_456_789, "0363018123456789")]
    [Arguments(9_999_999_999, "?")]
    [Arguments(0, "")]
    public void Uniek_volgnummer(long volgnummer, BagId svo)
        => svo.Volgnummer.Should().Be(volgnummer);
}

public class Has_constant
{
    [Test]
    public void Empty() => BagId.Empty.Should().Be(default);

    [Test]
    public void Unknown() => BagId.Unknown.Should().Be(BagId.Parse("?"));
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.BagId.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.BagId.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.BagId.Equals(BagId.Parse("0363020123456789")).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.BagId.Equals(363_010_123_456_789L.BagId()).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.BagId == 363_010_123_456_789L.BagId()).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.BagId == BagId.Parse("0363020123456789")).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.BagId != 363_010_123_456_789L.BagId()).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.BagId != BagId.Parse("0363020123456789")).Should().BeTrue();

    [Test]
    public void hash_code_is_value_based() => HashCodes
        .For(Svo.BagId, default).Should().BeConsistent();
}

public class Can_be_created
{
    [Test]
    [Arguments(363_010_123_456_789L, "known value")]
    [Arguments(9_930_300_000_000_000L, "maximum with object type 30")]
    public void from_valid_long(long value, string because)
        => BagId.TryCreate(value).Should().NotBeNull(because);

    [Test]
    [Arguments(0L, "zero")]
    [Arguments(-1L, "negative")]
    [Arguments(9_999_999_999_999_999L, "object type 99")]
    [Arguments(long.MaxValue, "too large")]
    public void from_invalid_long(long value, string because)
        => BagId.TryCreate(value).Should().BeNull(because);

    [Test]
    public void Create_retains_leading_zeros()
        => BagId.Create(363_010_123_456_789L).ToString().Should().Be("0363010123456789");

    [Test]
    public void Create_throws_for_invalid_long()
    {
        Func<BagId> create = () => BagId.Create(long.MaxValue);
        create.Should().Throw<ArgumentOutOfRangeException>();
    }
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
        => BagId.Parse(null).Should().Be(BagId.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => BagId.Parse(string.Empty).Should().Be(BagId.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => BagId.Parse("?").Should().Be(BagId.Unknown);

    [Test]
    [Arguments("0363010123456789", "verblijfsobject")]
    [Arguments("0363020123456789", "ligplaats")]
    [Arguments("0363030123456789", "standplaats")]
    [Arguments("0363100123456789", "pand")]
    [Arguments("0363200123456789", "nummeraanduiding")]
    [Arguments("0363300123456789", "openbare ruimte")]
    public void from_valid_object_type_codes(string input, string because)
        => BagId.TryParse(input).Should().NotBeNull(because);

    [Test]
    [Arguments("0363.01.0123456789")]
    [Arguments("0363\u00A001\u00A00123456789")]
    [Arguments("  0363010123456789  ")]
    public void from_string_with_markup(string input)
        => BagId.Parse(input).Should().Be(Svo.BagId);

    [Test]
    public void round_trips_its_formatted_representation()
        => BagId.Parse(Svo.BagId.ToString("F")).Should().Be(Svo.BagId);

    [Test]
    [Arguments("en", "0363010123456789")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            BagId.Parse(input).Should().Be(Svo.BagId);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Func<BagId> parse = () => BagId.Parse("invalid input");
            parse.Should().Throw<FormatException>()
                .WithMessage("Not a valid BAG-identificatienummer");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => (BagId.TryParse("invalid input", out _)).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => BagId.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => BagId.TryParse("0363010123456789").Should().Be(Svo.BagId);
}

public class Can_not_parse
{
    [Test]
    [Arguments("036301012345678", "15 digits")]
    [Arguments("03630101234567890", "17 digits")]
    [Arguments("0363000123456789", "object type 00")]
    [Arguments("0363040123456789", "object type 04")]
    [Arguments("0363990123456789", "object type 99")]
    [Arguments("036301012345678X", "not numeric")]
    public void from(string str, string because)
        => BagId.TryParse(str).Should().BeNull(because);
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.BagId.ToString().Should().Be("0363010123456789");
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.BagId.ToString().Should().Be(Svo.BagId.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.BagId.ToString().Should().Be(Svo.BagId.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(BagId).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => BagId.Unknown.ToString().Should().Be("?");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.BagId.ToString(FormatProvider.Empty).Should().Be("0363010123456789");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.BagId.ToString("F", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '0363.01.0123456789', format: 'F'");
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.BagId.ToString(provider: null).Should().Be("0363010123456789");
        }
    }

    [Test]
    public void TryFormat_empty_as_empty_string()
    {
        Span<char> destination = stackalloc char[18];
        BagId.Empty.TryFormat(destination, out var charsWritten, default, null).Should().BeTrue();
        charsWritten.Should().Be(0);
    }

    [Test]
    public void TryFormat_writes_value()
    {
        Span<char> destination = stackalloc char[18];
        Svo.BagId.TryFormat(destination, out var charsWritten, default, null).Should().BeTrue();
        charsWritten.Should().Be(16);
        destination[..charsWritten].ToString().Should().Be("0363010123456789");
    }

    [Test]
    public void TryFormat_writes_grouped_value()
    {
        Span<char> destination = stackalloc char[18];
        Svo.BagId.TryFormat(destination, out var charsWritten, "F", null).Should().BeTrue();
        charsWritten.Should().Be(18);
        destination[..charsWritten].ToString().Should().Be("0363.01.0123456789");
    }

    [Test]
    public void TryFormat_returns_false_if_destination_too_small()
    {
        Svo.BagId.TryFormat(new char[15], out var charsWritten, default, null).Should().BeFalse();
        charsWritten.Should().Be(0);

        Svo.BagId.TryFormat(new char[17], out charsWritten, "F", null).Should().BeFalse();
        charsWritten.Should().Be(0);
    }

    [Test]
    public void TryFormat_writes_unknown()
    {
        Span<char> destination = stackalloc char[1];
        BagId.Unknown.TryFormat(destination, out var charsWritten, default, null).Should().BeTrue();
        charsWritten.Should().Be(1);
        destination[0].Should().Be('?');
    }

    [Test]
    public void TryFormat_unknown_with_empty_destination_returns_false()
    {
        BagId.Unknown.TryFormat([], out var charsWritten, default, null).Should().BeFalse();
        charsWritten.Should().Be(0);
    }
}

public class Is_comparable
{
    [Test]
    public void to_null() => Svo.BagId.CompareTo(null).Should().Be(1);

    [Test]
    public void to_BagId_as_object()
    {
        object obj = Svo.BagId;
        Svo.BagId.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_BagId_only()
        => new object().Invoking(Svo.BagId.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        BagId[] sorted =
        [
            default,
            default,
            BagId.Parse("0101010000000001"),
            BagId.Parse("2020200000000002"),
            BagId.Parse("3030300000000003"),
            BagId.Unknown,
        ];

        var list = new List<BagId> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Casts
{
    [Test]
    public void explicitly_from_long()
        => ((BagId)363_010_123_456_789L).Should().Be(Svo.BagId);

    [Test]
    public void explicitly_to_long()
        => ((long)Svo.BagId).Should().Be(363_010_123_456_789L);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(BagId).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<BagId>().Should().Be(BagId.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<BagId>().Should().Be(BagId.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("0363010123456789").To<BagId>().Should().Be(Svo.BagId);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.BagId).Should().Be("0363010123456789");
        }
    }

    [Test]
    public void from_long()
        => Converting.From(363_010_123_456_789L).To<BagId>().Should().Be(Svo.BagId);

    [Test]
    public void to_long()
        => Converting.To<long>().From(Svo.BagId).Should().Be(363_010_123_456_789L);

    [Test]
    public void to_long_from_empty_as_zero()
        => Converting.To<long>().From(BagId.Empty).Should().Be(0L);
}

public class Supports_JSON_serialization
{
    [Test]
    [Arguments("?", "unknown")]
    public void convention_based_deserialization(BagId svo, object json)
        => JsonTester.Read<BagId>(json).Should().Be(svo);

    [Test]
    public void convention_based_deserialization_from_number()
        => JsonTester.Read<BagId>(363_010_123_456_789L).Should().Be(Svo.BagId);

    [Test]
    [Arguments(null, "")]
    public void convention_based_serialization(object json, BagId svo)
        => JsonTester.Write(svo).Should().Be(json);

#if NET10_0_OR_GREATER
    [Test]
    [Arguments("0363010123456789", "0363010123456789")]
    [Arguments("?", "?")]
    public void System_Text_JSON_deserialization(object json, BagId svo)
        => JsonTester.Read_System_Text_JSON<BagId>(json).Should().Be(svo);

    [Test]
    [Arguments(363_010_123_456_789L, "0363010123456789")]
    public void System_Text_JSON_deserialization_from_number(object json, BagId svo)
        => JsonTester.Read_System_Text_JSON<BagId>(json).Should().Be(svo);

    [Test]
    public void System_Text_JSON_deserialization_from_decimal()
        => JsonTester.Read_System_Text_JSON<BagId>(363_010_123_456_789.0).Should().Be(Svo.BagId);

    [Test]
    [Arguments("0363010123456789", "0363010123456789")]
    [Arguments("?", "?")]
    public void System_Text_JSON_serialization(BagId svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif

    [Test]
    [Arguments("Invalid input", typeof(FormatException))]
    [Arguments(12345L, typeof(ArgumentOutOfRangeException))]
    [Arguments(long.MaxValue, typeof(ArgumentOutOfRangeException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        Func<BagId> read = () => JsonTester.Read<BagId>(json);
        read.Should().Throw<Exception>().Subject.Single().Should().BeOfType(exceptionType);
    }
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
        => Serialize.Xml(Svo.BagId).Should().Be("0363010123456789");

    [Test]
    public void using_XmlSerializer_to_deserialize()
        => Deserialize.Xml<BagId>("0363010123456789").Should().Be(Svo.BagId);

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.BagId);
        Svo.BagId.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.BagId);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.BagId;
        obj.GetSchema().Should().BeNull();
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(BagId))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(BagId),
           description: "BAG-identificatienummer",
           example: "0363010123456789",
           type: "string",
           format: "bagid",
           pattern: "[0-9]{16}"));

    [Test]
    [Arguments("0363010123456789")]
    [Arguments("0001010000000000")]
    public void pattern_matches(string input)
        => OpenApiDataType.FromType(typeof(BagId))!.Matches(input).Should().BeTrue();
}

public class Debugger
{
    [Test]
    [Arguments("{empty}", "")]
    [Arguments("{unknown}", "?")]
    [Arguments("0363.01.0123456789", "0363010123456789")]
    public void has_custom_display(object display, BagId svo)
        => svo.Should().HaveDebuggerDisplay(display);
}
