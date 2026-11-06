namespace Sex_specs;

public class With_domain_logic
{
    [Test]
    public void DefinedValues_are_Male_Female_and_NotApplicable()
        => Sex.DefinedValues.Should().Equal(Sex.Male, Sex.Female, Sex.NotApplicable);

    [Test]
    [Arguments(true, "Male")]
    [Arguments(true, "Female")]
    [Arguments(true, "?")]
    [Arguments(false, "")]
    public void HasValue_is(bool result, Sex svo) => svo.HasValue.Should().Be(result);

    [Test]
    [Arguments(true, "Male")]
    [Arguments(true, "Female")]
    [Arguments(false, "?")]
    [Arguments(false, "")]
    public void IsKnown_is(bool result, Sex svo) => svo.IsKnown.Should().Be(result);

    [Test]
    [Arguments(false, "Male")]
    [Arguments(false, "Female")]
    [Arguments(false, "?")]
    [Arguments(true, "")]
    public void IsEmpty_returns(bool result, Sex svo)
        => svo.IsEmpty().Should().Be(result);

    [Test]
    [Arguments(false, "Male")]
    [Arguments(false, "Female")]
    [Arguments(true, "?")]
    [Arguments(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, Sex svo)
        => svo.IsEmptyOrUnknown().Should().Be(result);

    [Test]
    [Arguments(false, "Male")]
    [Arguments(false, "Female")]
    [Arguments(true, "?")]
    [Arguments(false, "")]
    public void IsUnknown_returns(bool result, Sex svo) => svo.IsUnknown().Should().Be(result);

    [Test]
    [Arguments(true, "Male")]
    [Arguments(true, "Female")]
    [Arguments(false, "?")]
    [Arguments(false, "")]
    public void IsMaleOrFemale_returns(bool result, Sex svo) => svo.IsMaleOrFemale().Should().Be(result);
}

public class Display_name
{
    [Test]
    public void for_current_culture_by_default()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            Svo.Sex.DisplayName.Should().Be("Vrouwelijk");
        }
    }

    [Test]
    public void for_custom_culture_if_specified()
        => Svo.Sex.GetDisplayName(TestCultures.es_EC).Should().Be("Mujer");
}

public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
        => Sex.Empty.Should().Be(default);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Sex.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Sex.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Sex.Equals(Sex.Male).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Sex.Equals(Sex.Female).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.Sex == Sex.Female).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.Sex == Sex.Male).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.Sex != Sex.Female).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.Sex != Sex.Male).Should().BeTrue();

    [Test]
    [Arguments("", 0)]
    [Arguments("Male", 665630161)]
    [Arguments("Female", 665630167)]
    public void hash_code_is_value_based(Sex svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Can_be_parsed
{
    [Test]
    public void from_null_string_represents_Empty()
        => Sex.Parse(null).Should().Be(Sex.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => Sex.Parse(string.Empty).Should().Be(Sex.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
       => Sex.Parse("?").Should().Be(Sex.Unknown);

    [Test]
    [Arguments("en", "Female")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            Sex.Parse(input).Should().Be(Svo.Sex);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Func<Sex> parse = ()
        => Sex.Parse("invalid input");
            parse.Should().Throw<FormatException>()
                .WithMessage("Not a valid sex");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => (Sex.TryParse("invalid input", out _)).Should().BeFalse();

    [Test]
    public void from_invalid_as_empty_with_TryParse()
        => Sex.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => Sex.TryParse("Female").Should().Be(Svo.Sex);
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Sex.ToString().Should().Be("Female");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Sex.ToString(default(string)).Should().Be(Svo.Sex.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Sex.ToString(string.Empty).Should().Be(Svo.Sex.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(Sex).ToString().Should().Be(string.Empty);

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Sex.ToString("s", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '♀', format: 's'");
    }

    [Test]
    [Arguments("en-GB", null, "Female", "Female")]
    [Arguments("en-GB", "c", "Female", "F")]
    [Arguments("ru-RU", "s", "Female", "♀")]
    [Arguments("nl-BE", "i", "Female", "2")]
    [Arguments("nl-BE", "h", "Female", "Mevr.")]
    [Arguments("nl-BE", "f", "Female", "Vrouwelijk")]
    public void culture_dependent(CultureInfo culture, string format, Sex svo, string expected)
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
            Svo.Sex.ToString(provider: null).Should().Be("Vrouwelijk");
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1()
        => Svo.Sex.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Sex_as_object()
    {
        object obj = Svo.Sex;
        Svo.Sex.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Sex_only()
        => new object().Invoking(Svo.Sex.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
                default,
                default,
                Sex.Unknown,
                Sex.Male,
                Sex.Female,
            };
        var list = new List<Sex> { sorted[3], sorted[4], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Casts
{
    [Test]
    public void explicitly_to_byte()
    {
        var casted = (byte)Svo.Sex;
        casted.Should().Be(2);
    }

    [Test]
    public void explicitly_to_int()
    {
        var casted = (int)Svo.Sex;
        casted.Should().Be(2);
    }

    [Test]
    [Arguments(2, "Female")]
    [Arguments(null, "?")]
    public void explicitly_to_nullable_int(int casted, Sex sex) => ((int?)sex).Should().Be(casted);

    [Test]
    [Arguments("Female", 2)]
    [Arguments("", null)]
    public void implicitly_from_nullable_int(Sex casted, int? value)
    {
        Sex sex = value;
        sex.Should().Be(casted);
    }

    [Test]
    [Arguments("Female", 2)]
    [Arguments("?", 0)]
    public void implicitly_from_int(Sex casted, int value)
    {
        Sex sex = value;
        sex.Should().Be(casted);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(Sex).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Sex>().Should().Be(Sex.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<Sex>().Should().Be(Sex.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("Female").To<Sex>().Should().Be(Svo.Sex);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Sex).Should().Be("Female");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET10_0_OR_GREATER
    [Test]
    [Arguments("?", "?")]
    [Arguments(null, null)]
    [Arguments(2L, "Female")]
    [Arguments(2d, "Female")]
    [Arguments("Female", "Female")]
    public void System_Text_JSON_deserialization(object json, Sex svo)
        => JsonTester.Read_System_Text_JSON<Sex>(json).Should().Be(svo);

    [Test]
    [Arguments(null, null)]
    [Arguments("Female", "Female")]
    public void System_Text_JSON_serialization(Sex svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [Test]
    [Arguments("?", "?")]
    [Arguments(2L, "Female")]
    [Arguments(2d, "Female")]
    [Arguments("Female", "Female")]
    public void convention_based_deserialization(object json, Sex svo)
        => JsonTester.Read<Sex>(json).Should().Be(svo);

    [Test]
    [Arguments(null, null)]
    [Arguments("Female", "Female")]
    public void convention_based_serialization(Sex svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [Test]
    [Arguments("Invalid input", typeof(FormatException))]
    [Arguments("2017-06-11", typeof(FormatException))]
    [Arguments(5L, typeof(ArgumentOutOfRangeException))]
    [Arguments(long.MaxValue, typeof(ArgumentOutOfRangeException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<Sex>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Sex);
        xml.Should().Be("Female");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<Sex>("Female");
        Svo.Sex.Should().Be(svo);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Sex);
        Svo.Sex.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Sex);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Sex;
        obj.GetSchema().Should().BeNull();
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(Sex))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(Sex),
           description: "Sex as specified by ISO/IEC 5218.",
           type: "string",
           example: "female",
           format: "sex",
           @enum: ["NotKnown", "Male", "Female", "NotApplicable"]));
}

public class Debugger
{
    [Test]
    [Arguments("{empty}", "")]
    [Arguments("Not known", "?")]
    [Arguments("Female", "Female")]
    public void has_custom_display(object display, Sex svo)
       => svo.Should().HaveDebuggerDisplay(display);
}

