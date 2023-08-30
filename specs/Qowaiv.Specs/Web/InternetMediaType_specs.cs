namespace Web.InternetMediaType_specs;

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
            Converting.FromNull<string>().To<InternetMediaType>().Should().Be(InternetMediaType.Empty);
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
