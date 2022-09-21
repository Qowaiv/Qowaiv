namespace Web.InternetMediaType_specs;

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(InternetMediaType).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From<string>(null).To<InternetMediaType>().Should().Be(InternetMediaType.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<InternetMediaType>().Should().Be(InternetMediaType.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("application/x-chess-pgn").To<InternetMediaType>().Should().Be(Svo.InternetMediaType);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.InternetMediaType).Should().Be("application/x-chess-pgn");
        }
    }
}

public class Supports_JSON_serialization
{
    [TestCase(null, "")]
    [TestCase("application/x-chess-pgn", "application/x-chess-pgn")]
    public void System_Text_JSON_deserialization(InternetMediaType svo, object json)
    => JsonTester.Read_System_Text_JSON<InternetMediaType>(json).Should().Be(svo);

    [TestCase(null, "")]
    [TestCase("application/x-chess-pgn", "application/x-chess-pgn")]
    public void System_Text_JSON_serialization(object json, InternetMediaType svo)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);

    [TestCase(null, "")]
    [TestCase("application/x-chess-pgn", "application/x-chess-pgn")]
    public void convention_based_deserialization(InternetMediaType svo, object json)
        => JsonTester.Read<InternetMediaType>(json).Should().Be(svo);

    [TestCase(null, "")]
    [TestCase("application/x-chess-pgn", "application/x-chess-pgn")]
    public void convention_based_serialization(object json, InternetMediaType svo)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<InternetMediaType>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }

    [TestCase("Invalid input")]
    [TestCase("2017-06-11")]
    public void FromJson_Invalid_Throws(object json)
    {
        Assert.Catch<FormatException>(() => JsonTester.Read<InternetMediaType>(json));
    }
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => Qowaiv.OpenApi.OpenApiDataType.FromType(typeof(InternetMediaType))
        .Should().Be(new Qowaiv.OpenApi.OpenApiDataType(
            dataType: typeof(InternetMediaType),
            description: "Media type notation as defined by RFC 6838.",
            example: "text/html",
            type: "string",
            format: "internet-media-type",
            nullable: true));
}
