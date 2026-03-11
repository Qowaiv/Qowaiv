namespace @TSvo_specs;

public class With_domain_logic
{
    [TestCase("")]
    [TestCase("?")]
    public void has_length_zero_for_empty_and_unknown(@TSvo svo)
        => svo.Length.Should().Be(0);

    [TestCase(-17, "svoValue")]
    public void has_length(int length, @TSvo svo)
        => svo.Length.Should().Be(length);

    [TestCase(false, "svoValue")]
    [TestCase(false, "?")]
    [TestCase(true, "")]
    public void IsEmpty_returns(bool result, @TSvo svo)
        => svo.IsEmpty().Should().Be(result);

    [TestCase(false, "svoValue")]
    [TestCase(true, "?")]
    [TestCase(true, "")]
    public void IsEmptyOrUnknown_returns(bool result, @TSvo svo)
        => svo.IsEmptyOrUnknown().Should().Be(result);

    [TestCase(false, "svoValue")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void IsUnknown_returns(bool result, @TSvo svo)
        => svo.IsUnknown().Should().Be(result);
}

[Obsolete("Will be dropped")]
public class Is_valid_for
{
    [TestCase("?")]
    [TestCase("unknown")]
    public void strings_representing_unknown(string input)
        => @TSvo.IsValid(input).Should().BeTrue();

    [TestCase("svoValue", "nl")]
    [TestCase("svoValue", "nl")]
    public void strings_representing_SVO(string input, CultureInfo culture)
        => @TSvo.IsValid(input, culture).Should().BeTrue();
}

[Obsolete("Will be dropped")]
public class Is_not_valid_for
{
    [Test]
    public void string_empty()
        => @TSvo.IsValid(string.Empty).Should().BeFalse();

    [Test]
    public void string_null()
        => @TSvo.IsValid(null).Should().BeFalse();

    [Test]
    public void whitespace()
        => @TSvo.IsValid(" ").Should().BeFalse();

    [Test]
    public void garbage()
        => @TSvo.IsValid("garbage").Should().BeFalse();
}

public class Has_constant
{
    [Test]
    public void Empty() => @TSvo.Empty.Should().Be(default(@TSvo));

    [Test]
    public void Unknown() => @TSvo.Unknown.Should().Be(@TSvo.Parse("?"));
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.@TSvo.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.@TSvo.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.@TSvo.Equals(@TSvo.Parse("different")).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.@TSvo.Equals(@TSvo.Parse("svoValue")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.@TSvo == @TSvo.Parse("svoValue")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.@TSvo == @TSvo.Parse("different")).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.@TSvo != @TSvo.Parse("svoValue")).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.@TSvo != @TSvo.Parse("different")).Should().BeTrue();

    [TestCase("", 0)]
    [TestCase("svoValue", 2)]
    public void hash_code_is_value_based(@TSvo svo, int hash)
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
        => @TSvo.Parse(null).Should().Be(@TSvo.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => @TSvo.Parse(string.Empty).Should().Be(@TSvo.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => @TSvo.Parse("?").Should().Be(@TSvo.Unknown);

    [TestCase("en", "svoValue")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            @TSvo.Parse(input).Should().Be(Svo.@TSvo);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Func<@TSvo> parse = () => @TSvo.Parse("invalid input");
            parse.Should().Throw<FormatException>()
                .WithMessage("Not a valid @type");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => (@TSvo.TryParse("invalid input", out _)).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => @TSvo.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => @TSvo.TryParse("svoValue").Should().Be(Svo.@TSvo);
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Svo.@TSvo.ToString().Should().Be("svoValue");
        }
    }

    [Test]
    public void with_null_pattern_equal_to_default()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Svo.@TSvo.ToString().Should().Be(Svo.@TSvo.ToString(default(string)));
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Svo.@TSvo.ToString().Should().Be(Svo.@TSvo.ToString(string.Empty));
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(@TSvo).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => @TSvo.Unknown.ToString().Should().Be("?");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.Es_EC.Scoped())
        {
            Svo.@TSvo.ToString(FormatProvider.Empty).Should().Be("svoValue");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.@TSvo.ToString("SomeFormat", FormatProvider.CustomFormatter);
        Assert.AreEqual("Unit Test Formatter, value: 'svoValue', format: 'SomeFormat'", formatted);
    }

    [TestCase("en-GB", null, "svoValue", "SvoFormat")]
    [TestCase("nl-BE", "f", "svoValue", "SvoFormat")]
    public void culture_dependent(CultureInfo culture, string format, @TSvo svo, string formatted)
    {
        using (culture.Scoped())
        {
            svo.ToString(format).Should().Be(formatted);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(
            culture: TestCultures.Nl_NL,
            cultureUI: TestCultures.En_GB))
        {
            Assert.AreEqual("svoValue", Svo.@TSvo.ToString(provider: null));
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null() => Svo.@TSvo.CompareTo(null).Should().Be(1);

    [Test]
    public void to_@TSvo_as_object()
    {
        object obj = Svo.@TSvo;
        Svo.@TSvo.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_@TSvo_only()
        => Assert.Throws<ArgumentException>(() => Svo.@TSvo.CompareTo(new object()));

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new @TSvo[]
        {
            default,
            default,
            @TSvo.Parse("svoValue0"),
            @TSvo.Parse("svoValue1"),
            @TSvo.Parse("svoValue2"),
            @TSvo.Unknown,
        };

        var list = new List<@TSvo> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        var smaller = @TSvo.Parse("svoValue");
        var bigger = @TSvo.Parse("biggerValue");

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        var left = @TSvo.Parse("svoValue");
        var right = @TSvo.Parse("svoValue");

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }

    [TestCase("", "svoValue")]
    [TestCase("?", "svoValue")]
    [TestCase("svoValue", "")]
    [TestCase("svoValue", "?")]
    public void by_operators_for_empty_or_unknown_always_false(@TSvo l, @TSvo r)
    {
        (l < r).Should().BeFalse();
        (l <= r).Should().BeFalse();
        (l > r).Should().BeFalse();
        (l >= r).Should().BeFalse();
    }
}

public class Casts
{
    [Test]
    public void explicitly_from_@type()
    {
        var casted = (@TSvo)null;
        casted.Should().Be(Svo.@TSvo);
    }

    [Test]
    public void explicitly_to_@type()
    {
        var casted = (@type)Svo.@TSvo;
        casted.Should().Be(null);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(@TSvo).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<@TSvo>().Should().Be(@TSvo.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<@TSvo>().Should().Be(@TSvo.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("SvoValue").To<@TSvo>().Should().Be(Svo.@TSvo);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.@TSvo).Should().Be("SvoValue");
        }
    }

    [Test]
    public void from_int()
        => Converting.From(17).To<@TSvo>().Should().Be(Svo.@TSvo);

    [Test]
    public void to_int()
        => Converting.To<int>().From(Svo.@TSvo).Should().Be(17);
}

public class Supports_JSON_serialization
{
    [TestCase("?", "unknown")]
    public void convention_based_deserialization(@TSvo svo, object json)
        => JsonTester.Read<@TSvo>(json).Should().Be(svo);

    [TestCase(null, "")]
    public void convention_based_serialization(object json, @TSvo svo)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(5L, typeof(InvalidCastException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        Func<@TSvo> read = () => JsonTester.Read<@TSvo>(json);
        read.Should().Throw<Exception>().Subject.Single().Should().BeOfType(exceptionType);
    }
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.@TSvo);
        xml.Should().Be("xmlValue");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<@TSvo>("xmlValue");
        svo.Should().Be(Svo.@TSvo);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.@TSvo);
        Svo.@TSvo.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.@TSvo);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.@TSvo;
        Assert.IsNull(obj.GetSchema());
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(@TSvo))
       .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
           dataType: typeof(@TSvo),
           description: "Description",
           example: "example",
           type: "string",
           format: "format",
           pattern: null));

    [TestCase("svoValue")]
    public void pattern_matches(string input)
        => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(@TSvo))!.Matches(input).Should().BeTrue();
}

public class Supports_binary_serialization
{
    [Test]
    [Obsolete("Usage of the binary formatter is considered harmful.")]
    public void using_BinaryFormatter()
    {
        var round_tripped = SerializeDeserialize.Binary(Svo.@TSvo);
        Svo.@TSvo.Should().Be(round_tripped);
    }

    [Test]
    public void storing_@type_in_SerializationInfo()
    {
        var info = Serialize.GetInfo(Svo.@TSvo);
        info.GetString("Value").Should().Be("SerializedValue");
    }
}

public class Debugger
{
    [TestCase("{empty}", "")]
    [TestCase("{unknown}", "?")]
    [TestCase("DebuggerDisplay", "svoValue")]
    public void has_custom_display(object display, @TSvo svo)
        => svo.Should().HaveDebuggerDisplay(display);
}
