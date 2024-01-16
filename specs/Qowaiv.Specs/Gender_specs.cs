namespace Gender_specs;

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class With_domain_logic
{
    [TestCase(true, "Male")]
    [TestCase(true, "Female")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, Gender svo) => svo.HasValue.Should().Be(result);

    [TestCase(true, "Male")]
    [TestCase(true, "Female")]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsKnown_is(bool result, Gender svo) => svo.IsKnown.Should().Be(result);

    [TestCase(false, "Male")]
    [TestCase(false, "Female")]
    [TestCase(false, "?")]
    [TestCase(true, "")]
    public void IsEmpty_returns(bool result, Gender svo)
    {
        svo.IsEmpty().Should().Be(result);
    }

    [TestCase(false, "Male")]
    [TestCase(false, "Female")]
    [TestCase(true, "?")]
    [TestCase(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, Gender svo)
    {
        svo.IsEmptyOrUnknown().Should().Be(result);
    }

    [TestCase(false, "Male")]
    [TestCase(false, "Female")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void IsUnknown_returns(bool result, Gender svo)
    {
        svo.IsUnknown().Should().Be(result);
    }

    [TestCase(true, "Male")]
    [TestCase(true, "Female")]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsMaleOrFemale_returns(bool result, Gender svo)
    {
        svo.IsMaleOrFemale().Should().Be(result);
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Display_name
{
    [Test]
    public void for_current_culture_by_default()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            Svo.Gender.DisplayName.Should().Be("Vrouwelijk");
        }
    }

    [Test]
    public void for_custom_culture_if_specified()
    {
        Svo.Gender.GetDisplayName(TestCultures.es_EC).Should().Be("Mujer");
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Is_valid_for
{
    [TestCase("?")]
    [TestCase("unknown")]
    public void strings_representing_unknown(string input)
    {
        Gender.IsValid(input).Should().BeTrue();
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(9)]
    public void numbers(int? number)
    {
        Gender.IsValid(number).Should().BeTrue();
    }

    [TestCase("Female", "nl")]
    [TestCase("Female", "nl")]
    public void strings_representing_SVO(string input, CultureInfo culture)
    {
        Gender.IsValid(input, culture).Should().BeTrue();
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Is_not_valid_for
{
    [Test]
    public void string_empty()
    {
        Gender.IsValid(string.Empty).Should().BeFalse();
    }

    [Test]
    public void string_null()
    {
        Gender.IsValid(Nil.String).Should().BeFalse();
    }

    [Test]
    public void whitespace()
    {
        Gender.IsValid(" ").Should().BeFalse();
    }

    [Test]
    public void garbage()
    {
        Gender.IsValid("garbage").Should().BeFalse();
    }

    [TestCase(null)]
    [TestCase(0)]
    [TestCase(3)]
    [TestCase(10)]
    public void numbers(int? number)
    {
        Gender.IsValid(number).Should().BeFalse();
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
    {
        Gender.Empty.Should().Be(default);
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
    {
        Svo.Gender.Equals(null).Should().BeFalse();
    }

    [Test]
    public void not_equal_to_other_type()
    {
        Svo.Gender.Equals(new object()).Should().BeFalse();
    }

    [Test]
    public void not_equal_to_different_value()
    {
        Svo.Gender.Equals(Gender.Male).Should().BeFalse();
    }

    [Test]
    public void equal_to_same_value()
    {
        Svo.Gender.Equals(Gender.Female).Should().BeTrue();
    }

    [Test]
    public void equal_operator_returns_true_for_same_values()
    {
        (Svo.Gender == Gender.Female).Should().BeTrue();
    }

    [Test]
    public void equal_operator_returns_false_for_different_values()
    {
        (Svo.Gender == Gender.Male).Should().BeFalse();
    }

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
    {
        (Svo.Gender != Gender.Female).Should().BeFalse();
    }

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
    {
        (Svo.Gender != Gender.Male).Should().BeTrue();
    }

    [TestCase("", 0)]
    [TestCase("Male", 665630161)]
    [TestCase("Female", 665630167)]
    public void hash_code_is_value_based(Gender svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
    {
        Gender.Parse(null).Should().Be(Gender.Empty);
    }

    [Test]
    public void from_empty_string_represents_Empty()
    {
        Gender.Parse(string.Empty).Should().Be(Gender.Empty);
    }

    [Test]
    public void from_question_mark_represents_Unknown()
    {
        Gender.Parse("?").Should().Be(Gender.Unknown);
    }

    [TestCase("en", "Female")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            var parsed = Gender.Parse(input);
            parsed.Should().Be(Svo.Gender);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var exception = Assert.Throws<FormatException>(() => Gender.Parse("invalid input"));
            exception.Message.Should().Be("Not a valid gender");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
    {
        Gender.TryParse("invalid input", out _).Should().BeFalse();
    }

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Gender.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
    {
        Gender.TryParse("Female").Should().Be(Svo.Gender);
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Gender.ToString().Should().Be("Female");
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Gender.ToString(default(string)).Should().Be(Svo.Gender.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Gender.ToString(string.Empty).Should().Be(Svo.Gender.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
    {
        default(Gender).ToString().Should().Be(string.Empty);
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Gender.ToString("s", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '♀', format: 's'");
    }

    [TestCase("en-GB", null, "Female", "Female")]
    [TestCase("en-GB", "c", "Female", "F")]
    [TestCase("ru-RU", "s", "Female", "♀")]
    [TestCase("nl-BE", "i", "Female", "2")]
    [TestCase("nl-BE", "h", "Female", "Mevr.")]
    [TestCase("nl-BE", "f", "Female", "Vrouwelijk")]
    public void culture_dependent(CultureInfo culture, string format, Gender svo, string expected)
    {
        using (culture.Scoped())
        {
            svo.ToString(format).Should().Be(expected);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.Gender.ToString(provider: null).Should().Be("Vrouwelijk");
        }
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Gender.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Gender_as_object()
    {
        object obj = Svo.Gender;
        Svo.Gender.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Gender_only()
    {
        Assert.Throws<ArgumentException>(() => Svo.Gender.CompareTo(new object()));
    }

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
                default,
                default,
                Gender.Unknown,
                Gender.Male,
                Gender.Female,
            };
        var list = new List<Gender> { sorted[3], sorted[4], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Casts
{
    [Test]
    public void explicitly_to_byte()
    {
        var casted = (byte)Svo.Gender;
        casted.Should().Be((byte)2);
    }

    [Test]
    public void explicitly_to_int()
    {
        var casted = (int)Svo.Gender;
        casted.Should().Be(2);
    }

    [TestCase(2, "Female")]
    [TestCase(null, "?")]
    public void explicitly_to_nullable_int(int casted, Gender gender)
    {
        ((int?)gender).Should().Be(casted);
    }

    [TestCase("Female", 2)]
    [TestCase("", null)]
    public void implicitly_from_nullable_int(Gender casted, int? value)
    {
        Gender gender = value;
        gender.Should().Be(casted);
    }

    [TestCase("Female", 2)]
    [TestCase("?", 0)]
    public void implicitly_from_int(Gender casted, int value)
    {
        Gender gender = value;
        gender.Should().Be(casted);
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase("?", "unknown")]
    [TestCase("Female", 2L)]
    [TestCase("Female", 2d)]
    public void System_Text_JSON_deserialization(Gender svo, object json)
        => JsonTester.Read_System_Text_JSON<Gender>(json).Should().Be(svo);

    [TestCase(null, "")]
    [TestCase("Female", "Female")]
    public void System_Text_JSON_serialization(object json, Gender svo)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("?", "unknown")]
    [TestCase("Female", 2L)]
    [TestCase("Female", 2d)]
    public void convention_based_deserialization(Gender expected, object json)
    {
        var actual = JsonTester.Read<Gender>(json);
        actual.Should().Be(expected);
    }

    [TestCase(null, "")]
    public void convention_based_serialization(object expected, Gender svo)
    {
        var serialized = JsonTester.Write(svo);
        serialized.Should().Be(expected);
    }

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(5L, typeof(ArgumentOutOfRangeException))]
    [TestCase(long.MaxValue, typeof(ArgumentOutOfRangeException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<Gender>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Supports_XML_serialization
{

    [TestCase("", "")]
    [TestCase("Female", "F")]
    public void using_XmlSerializer_to_serialize(string xml, Gender gender)
    {
        Serialize.Xml(gender).Should().Be(xml);
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<Gender>("Female");
        svo.Should().Be(Svo.Gender);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Gender);
        round_tripped.Should().Be(Svo.Gender);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Gender);
        var round_tripped = SerializeDeserialize.Xml(structure);
        round_tripped.Should().Be(structure);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Gender;
        obj.GetSchema().Should().BeNull();
    }
}

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Is_Open_API_data_type
{
    internal static readonly OpenApiDataTypeAttribute Attribute = OpenApiDataTypeAttribute.From(typeof(Gender)).Single();

    [Test]
    public void with_description()
    {
        Attribute.Description.Should().Be("Gender as specified by ISO/IEC 5218.");
    }

    [Test]
    public void has_type()
    {
        Attribute.Type.Should().Be("string");
    }

    [Test]
    public void has_format()
    {
        Attribute.Format.Should().Be("gender");
    }

    [Test]
    public void has_enum()
    {
        Attribute.Enum.Should().BeEquivalentTo("NotKnown", "Male", "Female", "NotApplicable");
    }

    [Test]
    public void pattern_is_null()
    {
        Attribute.Pattern.Should().BeNull();
    }
}

#if NET8_0_OR_GREATER
#else
[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Supports_binary_serialization
{
    [Test]
    public void using_BinaryFormatter()
    {
        var round_tripped = SerializeDeserialize.Binary(Svo.Gender);
        round_tripped.Should().Be(Svo.Gender);
    }

    [Test]
    public void storing_byte_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.Gender);
        info.GetByte("Value").Should().Be((byte)4);
    }
}
#endif

[Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("Not known", "?")]
    [TestCase("Female", "Female")]
    public void has_custom_display(object display, Gender svo)
        => svo.Should().HaveDebuggerDisplay(display);
}

