namespace Sex_specs;

public class With_domain_logic
{
    [TestCase(false, "Male")]
    [TestCase(false, "Female")]
    [TestCase(false, "?")]
    [TestCase(true, "")]
    public void IsEmpty_returns(bool result, Sex svo)
        => svo.IsEmpty().Should().Be(result);

    [TestCase(false, "Male")]
    [TestCase(false, "Female")]
    [TestCase(true, "?")]
    [TestCase(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, Sex svo)
        => svo.IsEmptyOrUnknown().Should().Be(result);

    [TestCase(false, "Male")]
    [TestCase(false, "Female")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void IsUnknown_returns(bool result, Sex svo)
    {
        Assert.AreEqual(result, svo.IsUnknown());
    }

    [TestCase(true, "Male")]
    [TestCase(true, "Female")]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsMaleOrFemale_returns(bool result, Sex svo)
    {
        Assert.AreEqual(result, svo.IsMaleOrFemale());
    }
}

public class Display_name
{
    [Test]
    public void for_current_culture_by_default()
    {
        using (TestCultures.Nl_BE.Scoped())
        {
            Assert.AreEqual("Vrouwelijk", Svo.Sex.DisplayName);
        }
    }

    [Test]
    public void for_custom_culture_if_specified()
    {
        Assert.AreEqual("Mujer", Svo.Sex.GetDisplayName(TestCultures.Es_EC));
    }
}

public class Is_valid_for
{
    [TestCase("?")]
    [TestCase("unknown")]
    public void strings_representing_unknown(string input)
    {
        Assert.IsTrue(Sex.IsValid(input));
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(9)]
    public void numbers(int? number)
    {
        Assert.IsTrue(Sex.IsValid(number));
    }

    [TestCase("Female", "nl")]
    [TestCase("Female", "nl")]
    public void strings_representing_SVO(string input, CultureInfo culture)
    {
        Assert.IsTrue(Sex.IsValid(input, culture));
    }
}

public class Is_not_valid_for
{
    [Test]
    public void string_empty()
    {
        Assert.IsFalse(Sex.IsValid(string.Empty));
    }

    [Test]
    public void string_null()
    {
        Assert.IsFalse(Sex.IsValid((string)null));
    }

    [Test]
    public void whitespace()
    {
        Assert.IsFalse(Sex.IsValid(" "));
    }

    [Test]
    public void garbage()
    {
        Assert.IsFalse(Sex.IsValid("garbage"));
    }

    [TestCase(null)]
    [TestCase(0)]
    [TestCase(3)]
    [TestCase(10)]
    public void numbers(int? number)
    {
        Assert.IsFalse(Sex.IsValid(number));
    }
}

public class Has_constant
{
    [Test]
    public void Empty_represent_default_value()
    {
        Assert.AreEqual(default(Sex), Sex.Empty);
    }
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

    [TestCase("", 0)]
    [TestCase("Male", 665630161)]
    [TestCase("Female", 665630167)]
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

    [TestCase("en", "Female")]
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
        using (TestCultures.En_GB.Scoped())
        {
            Func<Sex> parse = () => Sex.Parse("invalid input");
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
        using (TestCultures.En_GB.Scoped())
        {
            Svo.Sex.ToString().Should().Be("Female");
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.AreEqual(Svo.Sex.ToString(), Svo.Sex.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Assert.AreEqual(Svo.Sex.ToString(), Svo.Sex.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
    {
        Assert.AreEqual(string.Empty, default(Sex).ToString());
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Sex.ToString("s", FormatProvider.CustomFormatter);
        Assert.AreEqual("Unit Test Formatter, value: '♀', format: 's'", formatted);
    }

    [TestCase("en-GB", null, "Female", "Female")]
    [TestCase("en-GB", "c", "Female", "F")]
    [TestCase("ru-RU", "s", "Female", "♀")]
    [TestCase("nl-BE", "i", "Female", "2")]
    [TestCase("nl-BE", "h", "Female", "Mevr.")]
    [TestCase("nl-BE", "f", "Female", "Vrouwelijk")]
    public void culture_dependent(CultureInfo culture, string format, Sex svo, string expected)
    {
        using (culture.Scoped())
        {
            Assert.AreEqual(expected, svo.ToString(format));
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.Nl_NL, cultureUI: TestCultures.En_GB))
        {
            Assert.AreEqual("Vrouwelijk", Svo.Sex.ToString(provider: null));
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1()
    {
        object obj = null;
        Assert.AreEqual(1, Svo.Sex.CompareTo(obj));
    }

    [Test]
    public void to_Sex_as_object()
    {
        object obj = Svo.Sex;
        Assert.AreEqual(0, Svo.Sex.CompareTo(obj));
    }

    [Test]
    public void to_Sex_only()
    {
        Assert.Throws<ArgumentException>(() => Svo.Sex.CompareTo(new object()));
    }

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
        Assert.AreEqual(sorted, list);
    }
}

public class Casts
{
    [Test]
    public void explicitly_to_byte()
    {
        var casted = (byte)Svo.Sex;
        Assert.AreEqual((byte)2, casted);
    }

    [Test]
    public void explicitly_to_int()
    {
        var casted = (int)Svo.Sex;
        Assert.AreEqual(2, casted);
    }

    [TestCase(2, "Female")]
    [TestCase(null, "?")]
    public void explicitly_to_nullable_int(int casted, Sex sex)
    {
        Assert.AreEqual(casted, (int?)sex);
    }

    [TestCase("Female", 2)]
    [TestCase("", null)]
    public void implictly_from_nullable_int(Sex casted, int? value)
    {
        Sex sex = value;
        Assert.AreEqual(casted, sex);
    }

    [TestCase("Female", 2)]
    [TestCase("?", 0)]
    public void implictly_from_int(Sex casted, int value)
    {
        Sex sex = value;
        Assert.AreEqual(casted, sex);
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
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<Sex>().Should().Be(Sex.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<Sex>().Should().Be(Sex.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("Female").To<Sex>().Should().Be(Svo.Sex);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.Sex).Should().Be("Female");
        }
    }
}

public class Supports_JSON_serialization
{
    [TestCase("?", "unknown")]
    [TestCase("Female", 2L)]
    [TestCase("Female", 2d)]
    public void convention_based_deserialization(Sex expected, object json)
    {
        var actual = JsonTester.Read<Sex>(json);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(null, "")]
    public void convention_based_serialization(object expected, Sex svo)
    {
        var serialized = JsonTester.Write(svo);
        Assert.AreEqual(expected, serialized);
    }

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(5L, typeof(ArgumentOutOfRangeException))]
    [TestCase(long.MaxValue, typeof(ArgumentOutOfRangeException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<Sex>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
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
        Assert.IsNull(obj.GetSchema());
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(Sex))
       .Should().BeEquivalentTo(new Qowaiv.OpenApi.OpenApiDataType(
           dataType: typeof(Sex),
           description: "Sex as specified by ISO/IEC 5218.",
           type: "string",
           example: "female",
           format: "sex",
           @enum: new[] { "NotKnown", "Male", "Female", "NotApplicable" },
           nullable: true));

    internal static readonly OpenApiDataTypeAttribute Attribute = OpenApiDataTypeAttribute.From(typeof(Sex)).FirstOrDefault();
    
    [Test]
    public void with_description()
    {
        Assert.AreEqual("Sex as specified by ISO/IEC 5218.", Attribute.Description);
    }

    [Test]
    public void has_type()
    {
        Assert.AreEqual("string", Attribute.Type);
    }

    [Test]
    public void has_format()
    {
        Assert.AreEqual("sex", Attribute.Format);
    }

    [Test]
    public void has_enum()
    {
        Assert.AreEqual(new[] { "NotKnown", "Male", "Female", "NotApplicable" }, Attribute.Enum);
    }

    [Test]
    public void pattern_is_null()
    {
        Assert.IsNull(Attribute.Pattern);
    }
}

public class Supports_binary_serialization
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void using_BinaryFormatter()
    {
        var round_tripped = SerializeDeserialize.Binary(Svo.Sex);
        Svo.Sex.Should().Be(round_tripped);
    }

    [Test]
    public void storing_Byte_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.Sex);
        info.GetByte("Value").Should().Be((byte)4);
    }
}

public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("Not known", "?")]
    [TestCase("Female", "Female")]
    public void has_custom_display(object display, Sex svo)
       => svo.Should().HaveDebuggerDisplay(display);
}

