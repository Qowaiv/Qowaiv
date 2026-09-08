namespace Financial.BIC_specs;

public class With_domain_logic
{
    [TestCase(true, "AEGONL2UXXX")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, BusinessIdentifierCode svo) => svo.HasValue.Should().Be(result);

    [TestCase(true, "AEGONL2UXXX")]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsKnown_is(bool result, BusinessIdentifierCode svo) => svo.IsKnown.Should().Be(result);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.BusinessIdentifierCode.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.BusinessIdentifierCode.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.BusinessIdentifierCode.Equals(BusinessIdentifierCode.Empty).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.BusinessIdentifierCode.Equals(BusinessIdentifierCode.Parse("AEGONL2UXXX")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (BusinessIdentifierCode.Parse("AEGONL2UXXX") == Svo.BusinessIdentifierCode).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (BusinessIdentifierCode.Parse("AEGONL2UXXX") == BusinessIdentifierCode.Empty).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (BusinessIdentifierCode.Parse("AEGONL2UXXX") != Svo.BusinessIdentifierCode).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (BusinessIdentifierCode.Parse("AEGONL2UXXX") != BusinessIdentifierCode.Empty).Should().BeTrue();

    [Test]
    public void formatted_and_unformatted_are_equal()
    {
        var l = BusinessIdentifierCode.Parse("AEGONL2UXXX", CultureInfo.InvariantCulture);
        var r = BusinessIdentifierCode.Parse("AEgonL2Uxxx", CultureInfo.InvariantCulture);
        l.Equals(r).Should().BeTrue();
    }

    [TestCase("", 0)]
    [TestCase("AEGONL2UXXX", -238769066)]
    public void hash_code_is_value_based(BusinessIdentifierCode svo, int hash)
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
        => BusinessIdentifierCode.Parse(null).Should().Be(BusinessIdentifierCode.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => BusinessIdentifierCode.Parse(string.Empty).Should().Be(BusinessIdentifierCode.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => BusinessIdentifierCode.Parse("?").Should().Be(BusinessIdentifierCode.Unknown);

    [TestCase("en-GB", "AEGONL2UXXX")]
    [TestCase("es-EC", "AEGONL2UXXX")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            BusinessIdentifierCode.Parse(input).Should().Be(Svo.BusinessIdentifierCode);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(BusinessIdentifierCode.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid BIC");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => BusinessIdentifierCode.TryParse("invalid input", out _).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => BusinessIdentifierCode.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => BusinessIdentifierCode.TryParse("AEGONL2UXXX").Should().Be(Svo.BusinessIdentifierCode);
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.BusinessIdentifierCode.ToString().Should().Be("AEGONL2UXXX");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.BusinessIdentifierCode.ToString(default(string)).Should().Be(Svo.BusinessIdentifierCode.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.BusinessIdentifierCode.ToString(string.Empty).Should().Be(Svo.BusinessIdentifierCode.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(BusinessIdentifierCode).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => BusinessIdentifierCode.Unknown.ToString().Should().Be("?");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.BusinessIdentifierCode.ToString(FormatProvider.Empty).Should().Be("AEGONL2UXXX");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.BusinessIdentifierCode.ToString("Unit Test Format", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: 'AEGONL2UXXX', format: 'Unit Test Format'");
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.BusinessIdentifierCode.ToString(provider: null).Should().Be("AEGONL2UXXX");
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.BusinessIdentifierCode.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_BusinessIdentifierCode_as_object()
    {
        object obj = Svo.BusinessIdentifierCode;
        Svo.BusinessIdentifierCode.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_BusinessIdentifierCode_only()
        => new object().Invoking(Svo.BusinessIdentifierCode.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            BusinessIdentifierCode.Empty,
            BusinessIdentifierCode.Empty,
            BusinessIdentifierCode.Parse("AEGONL2UXXX"),
            BusinessIdentifierCode.Parse("CEBUNL2U"),
            BusinessIdentifierCode.Parse("DSSBNL22"),
            BusinessIdentifierCode.Parse("FTSBNL2R"),
        };

        var list = new List<BusinessIdentifierCode> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(BusinessIdentifierCode).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<BusinessIdentifierCode>().Should().Be(default);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<BusinessIdentifierCode>().Should().Be(default);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("AEGONL2UXXX").To<BusinessIdentifierCode>().Should().Be(Svo.BusinessIdentifierCode);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.BusinessIdentifierCode).Should().Be("AEGONL2UXXX");
        }
    }
}

public class Is_valid
{
    [TestCase("PSTBNL21")]
    [TestCase("ABNANL2A")]
    [TestCase("BACBBEBB")]
    [TestCase("GEBABEBB36A")]
    [TestCase("DEUTDEFF")]
    [TestCase("NEDSZAJJ")]
    [TestCase("DABADKKK")]
    [TestCase("UNCRIT2B912")]
    [TestCase("DSBACNBXSHA")]
    [TestCase("AAAANLBË")]
    public void For(string str)
        => BusinessIdentifierCode.Parse(str).IsEmptyOrUnknown().Should().BeFalse();
}

public class Is_invalid
{
    [TestCase("1AAANL01", "digit in first four characters")]
    [TestCase("AAAANLBB1", "Branch length of 1")]
    [TestCase("AAAANLBB12", "Branch length of 2")]
    [TestCase("ABCDXX01", "Not existing country")]
    public void For(string str, string because)
        => BusinessIdentifierCode.TryParse(str).Should().BeNull(because);
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase(null, null)]
    [TestCase("AEGONL2UXXX", "AEGONL2UXXX")]
    public void System_Text_JSON_deserialization(object json, BusinessIdentifierCode svo)
        => JsonTester.Read_System_Text_JSON<BusinessIdentifierCode>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("AEGONL2UXXX", "AEGONL2UXXX")]
    public void System_Text_JSON_serialization(BusinessIdentifierCode svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("AEGONL2UXXX", "AEGONL2UXXX")]
    public void convention_based_deserialization(object json, BusinessIdentifierCode svo)
        => JsonTester.Read<BusinessIdentifierCode>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("AEGONL2UXXX", "AEGONL2UXXX")]
    public void convention_based_serialization(BusinessIdentifierCode svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<BusinessIdentifierCode>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.BusinessIdentifierCode);
        xml.Should().Be("AEGONL2UXXX");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<BusinessIdentifierCode>("AEGONL2UXXX");
        svo.Should().Be(Svo.BusinessIdentifierCode);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.BusinessIdentifierCode);
        Svo.BusinessIdentifierCode.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.BusinessIdentifierCode);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.BusinessIdentifierCode;
        obj.GetSchema().Should().BeNull();
    }
}
