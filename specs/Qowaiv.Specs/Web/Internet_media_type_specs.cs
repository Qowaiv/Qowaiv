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

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.InternetMediaType.CompareTo(Nil.Object).Should().Be(1);
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
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
