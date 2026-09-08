namespace HouseNumber_specs;

public class With_domain_logic
{
    [TestCase(true, 123456789)]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, HouseNumber svo) => svo.HasValue.Should().Be(result);

    [TestCase(true, 123456789)]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsKnown_is(bool result, HouseNumber svo) => svo.IsKnown.Should().Be(result);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.HouseNumber.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.HouseNumber.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.HouseNumber.Equals(HouseNumber.MinValue).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.HouseNumber.Equals(HouseNumber.Create(123456789)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (HouseNumber.Create(123456789) == Svo.HouseNumber).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (HouseNumber.Create(123456789) == HouseNumber.MinValue).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (HouseNumber.Create(123456789) != Svo.HouseNumber).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (HouseNumber.Create(123456789) != HouseNumber.MinValue).Should().BeTrue();

    [TestCase("", 0)]
    [TestCase("yes", 665630161)]
    public void hash_code_is_value_based(YesNo svo, int hash)
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
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(HouseNumber.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid house number");
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
            Svo.HouseNumber.ToString().Should().Be("123456789");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.HouseNumber.ToString(default(string)).Should().Be(Svo.HouseNumber.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.HouseNumber.ToString(string.Empty).Should().Be(Svo.HouseNumber.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(HouseNumber).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => HouseNumber.Unknown.ToString().Should().Be("?");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.HouseNumber.ToString(FormatProvider.Empty).Should().Be("123456789");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.HouseNumber.ToString("#,##0", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '123,456,789', format: '#,##0'");
    }

    [TestCase("nl-BE", "0000", "800", "0800")]
    [TestCase("en-GB", "0000", "800", "0800")]
    [TestCase("es-EC", "00000.0", "1700", "01700,0")]
    public void culture_dependent(CultureInfo culture, string format, string input, string formatted)
    {
        using (culture.Scoped())
        {
            HouseNumber.Parse(input).ToString(format).Should().Be(formatted);
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.HouseNumber.ToString(provider: null).Should().Be("123456789");
        }
    }
}
public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.HouseNumber.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_HouseNumber_as_object()
    {
        object obj = Svo.HouseNumber;
        Svo.HouseNumber.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_HouseNumber_only()
        => new object().Invoking(Svo.HouseNumber.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new HouseNumber[]
        {
            HouseNumber.Empty,
            HouseNumber.Empty,
            1,
            12,
            123,
            1234,
        };

        var list = new List<HouseNumber> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        HouseNumber smaller = 17;
        HouseNumber bigger = 19;

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        HouseNumber left = 17;
        HouseNumber right = 17;

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }
}
public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(HouseNumber).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<HouseNumber>().Should().Be(HouseNumber.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<HouseNumber>().Should().Be(HouseNumber.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("123456789").To<HouseNumber>().Should().Be(Svo.HouseNumber);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.HouseNumber).Should().Be("123456789");
        }
    }

    [Test]
    public void from_long()
        => Converting.From(123456789L).To<HouseNumber>().Should().Be(Svo.HouseNumber);

    [Test]
    public void to_long()
        => Converting.To<long>().From(Svo.HouseNumber).Should().Be(123456789L);
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase(null, null)]
    [TestCase("?", "?")]
    [TestCase("17", "17")]
    [TestCase(17d, "17")]
    [TestCase(17L, "17")]
    public void System_Text_JSON_deserialization(object json, HouseNumber svo)
        => JsonTester.Read_System_Text_JSON<HouseNumber>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase(17, "17")]
    public void System_Text_JSON_serialization(HouseNumber svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("?", "?")]
    [TestCase("17", "17")]
    [TestCase(17d, "17")]
    [TestCase(17L, "17")]
    public void convention_based_deserialization(HouseNumber svo, object json)
        => JsonTester.Read<HouseNumber>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase(17, "17")]
    public void convention_based_serialization(HouseNumber svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<HouseNumber>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
       => OpenApiDataType.FromType(typeof(HouseNumber))
       .Should().Be(new OpenApiDataType(
           dataType: typeof(HouseNumber),
           description: "House number notation.",
           example: "13",
           type: "string",
format: "house-number",
            nullable: true));
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.HouseNumber);
        xml.Should().Be("123456789");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<HouseNumber>("123456789");
        svo.Should().Be(Svo.HouseNumber);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.HouseNumber);
        Svo.HouseNumber.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.HouseNumber);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.HouseNumber;
        obj.GetSchema().Should().BeNull();
    }
}
