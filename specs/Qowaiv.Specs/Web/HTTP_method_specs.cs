using Qowaiv.Conversion.Web;
using System.Net.Http;
#if NET6_0_OR_GREATER
using Qowaiv.Json.Web;
using System.Text.Json;
#endif
namespace Web.HTTP_method_specs;

public class Supports_type_conversion
{
    [Test]
    public void not_via_TypeConverter_registered_with_attribute()
        => typeof(HttpMethod).Should().HaveNoTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().With<HttpMethodTypeConverter>().To<HttpMethod>().Should().BeNull();
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).With<HttpMethodTypeConverter>().To<HttpMethod>().Should().BeNull();
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("POST").With<HttpMethodTypeConverter>().To<HttpMethod>().Should().Be(Svo.HttpMethod);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.HttpMethod).Should().Be("POST");
        }
    }
}

#if NET6_0_OR_GREATER
public class Supports_JSON_serialization
{
    [TestCase(null)]
    [TestCase("POST")]
    public void System_Text_JSON_deserialization(object? json)
        => JsonTester.Read_System_Text_JSON<HttpMethod>(json, Options()).Should().Be(json is null ? null : HttpMethod.Post);

    [TestCase(null)]
    [TestCase("POST")]
    public void System_Text_JSON_serialization(object? json)
        => JsonTester.Write_System_Text_JSON(json is null ? null : HttpMethod.Post, Options()).Should().Be(json);

    private static JsonSerializerOptions Options()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new HttpMethodJsonConverter());
        return options;
    }
}
#endif
