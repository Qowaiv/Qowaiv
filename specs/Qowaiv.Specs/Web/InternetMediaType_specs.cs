namespace Web.Internet_media_type_specs;

public class With_domain_logic
{
    [TestCase(true, "application/x-chess-pgn")]
    [TestCase(true, "application/octet-stream")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, InternetMediaType svo) => svo.HasValue.Should().Be(result);

    [TestCase(true, "application/x-chess-pgn")]
    [TestCase(false, "application/octet-stream")]
    [TestCase(false, "")]
    public void IsKnown_is(bool result, InternetMediaType svo) => svo.IsKnown.Should().Be(result);
}

public class Created_from_file
{
    [Test]
    public void IO_File_info()
        => InternetMediaType.FromFile(new FileInfo("games.pgn")).Should().Be(Svo.InternetMediaType);

    [TestCase(".unknown")]
    [TestCase(".other")]
    public void IO_File_info_with_unknown_extension(string extension)
       => InternetMediaType.FromFile(new FileInfo($"file{extension}")).Should().Be(InternetMediaType.Unknown);

    [Test]
    public void null_IO_File_info_is_empty()
        => InternetMediaType.FromFile(Nil.FileInfo).Should().Be(InternetMediaType.Empty);

    [Test]
    public void null_string_is_empty()
        => InternetMediaType.FromFile(Nil.String).Should().Be(InternetMediaType.Empty);

    [Test]
    public void empty_string_is_empty()
        => InternetMediaType.FromFile(string.Empty).Should().Be(InternetMediaType.Empty);
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.InternetMediaType.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.InternetMediaType.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.InternetMediaType.Equals(InternetMediaType.Empty).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.InternetMediaType.Equals(InternetMediaType.Parse("application/x-chess-pgn")).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (InternetMediaType.Parse("application/x-chess-pgn") == Svo.InternetMediaType).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (InternetMediaType.Parse("application/x-chess-pgn") == InternetMediaType.Empty).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (InternetMediaType.Parse("application/x-chess-pgn") != Svo.InternetMediaType).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (InternetMediaType.Parse("application/x-chess-pgn") != InternetMediaType.Empty).Should().BeTrue();

    [Test]
    public void formatted_and_unformatted_are_equal()
    {
        var l = InternetMediaType.Parse("application/x-chess-pgn");
        var r = InternetMediaType.Parse("application/X-chess-PGN");
        l.Equals(r).Should().BeTrue();
    }

    [TestCase("", 0)]
    [TestCase("application/x-chess-pgn", 787633777)]
    public void hash_code_is_value_based(InternetMediaType svo, int hash)
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
        => InternetMediaType.Parse(null).Should().Be(InternetMediaType.Empty);

    [Test]
    public void from_empty_string_represents_Empty()
        => InternetMediaType.Parse(string.Empty).Should().Be(InternetMediaType.Empty);

    [Test]
    public void from_question_mark_represents_Unknown()
        => InternetMediaType.Parse("?").Should().Be(InternetMediaType.Unknown);

    [TestCase("en-GB", "application/x-chess-pgn")]
    [TestCase("es-EC", "application/x-chess-pgn")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            InternetMediaType.Parse(input).Should().Be(Svo.InternetMediaType);
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(InternetMediaType.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid internet media type");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => InternetMediaType.TryParse("invalid input", out _).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => InternetMediaType.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
        => InternetMediaType.TryParse("application/x-chess-pgn").Should().Be(Svo.InternetMediaType);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(InternetMediaType).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<InternetMediaType>().Should().Be(InternetMediaType.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<InternetMediaType>().Should().Be(InternetMediaType.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("application/x-chess-pgn").To<InternetMediaType>().Should().Be(Svo.InternetMediaType);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.InternetMediaType).Should().Be("application/x-chess-pgn");
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
            Svo.InternetMediaType.ToString().Should().Be("application/x-chess-pgn");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.InternetMediaType.ToString(default(string)).Should().Be(Svo.InternetMediaType.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.InternetMediaType.ToString(string.Empty).Should().Be(Svo.InternetMediaType.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_string_empty()
        => default(InternetMediaType).ToString().Should().BeEmpty();

    [Test]
    public void unknown_value_is_represented_as_unknown()
        => InternetMediaType.Unknown.ToString().Should().Be("application/octet-stream");

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.InternetMediaType.ToString(FormatProvider.Empty).Should().Be("application/x-chess-pgn");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.InternetMediaType.ToString("Unit Test Format", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: 'application/x-chess-pgn', format: 'Unit Test Format'");
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.InternetMediaType.ToString(provider: null).Should().Be("application/x-chess-pgn");
        }
    }
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.InternetMediaType.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_InternetMediaType_as_object()
    {
        object obj = Svo.InternetMediaType;
        Svo.InternetMediaType.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_InternetMediaType_only()
        => new object().Invoking(Svo.InternetMediaType.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            InternetMediaType.Empty,
            InternetMediaType.Empty,
            InternetMediaType.Parse("audio/mp3"),
            InternetMediaType.Parse("image/jpeg"),
            InternetMediaType.Parse("text/x-markdown"),
            InternetMediaType.Parse("video/quicktime"),
        };

        var list = new List<InternetMediaType> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase(null, null)]
    [TestCase("application/x-chess-pgn", "application/x-chess-pgn")]
    public void System_Text_JSON_deserialization(object json, InternetMediaType svo)
        => JsonTester.Read_System_Text_JSON<InternetMediaType>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("application/x-chess-pgn", "application/x-chess-pgn")]
    public void System_Text_JSON_serialization(InternetMediaType svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("application/x-chess-pgn", "application/x-chess-pgn")]
    public void convention_based_deserialization(object json, InternetMediaType svo)
        => JsonTester.Read<InternetMediaType>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("application/x-chess-pgn", "application/x-chess-pgn")]
    public void convention_based_serialization(InternetMediaType svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<InternetMediaType>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);

    [TestCase("Invalid input")]
    [TestCase("2017-06-11")]
    public void FromJson_Invalid_Throws(object json)
        => json.Invoking(JsonTester.Read<InternetMediaType>)
            .Should().Throw<FormatException>();
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => OpenApiDataType.FromType(typeof(InternetMediaType))
        .Should().Be(new OpenApiDataType(
            dataType: typeof(InternetMediaType),
            description: "Media type notation as defined by RFC 6838.",
            example: "text/html",
            type: "string",
            format: "internet-media-type",
            nullable: true));
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.InternetMediaType);
        xml.Should().Be("application/x-chess-pgn");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<InternetMediaType>("application/x-chess-pgn");
        svo.Should().Be(Svo.InternetMediaType);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.InternetMediaType);
        Svo.InternetMediaType.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.InternetMediaType);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.InternetMediaType;
        obj.GetSchema().Should().BeNull();
    }
}
