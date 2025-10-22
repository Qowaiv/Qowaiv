#if NET8_0_OR_GREATER
using Qowaiv.Json.Globalization;
using System.Text.Json;
#endif

namespace Globalization.Culture_info_specs;

public class Supports_type_conversion
{
    [Test]
    public void Via_TypeConverter_as_part_of_dot_NET()
        => typeof(CultureInfo).Should().HaveTypeConverterDefined();
}

#if NET8_0_OR_GREATER
public class Supports_System_Text_JSON_serialization
{
    [TestCase(null, null)]
    [TestCase("es-EC", "es-EC")]
    public void Value_deserialization(object? json, CultureInfo? svo)
        => JsonTester.Read_System_Text_JSON<CultureInfo>(json, Options()).Should().Be(svo);

	[TestCase(null, null)]
	[TestCase("es-EC", "es-EC")]
	public void Value_serialization(object? json, CultureInfo? svo)
        => JsonTester.Write_System_Text_JSON(svo, Options()).Should().Be(json);

    [Test]
    public void Dictionary_deserialization()
    {
        JsonSerializer.Deserialize<Dictionary<CultureInfo, int>>("""{"en-GB":100,"es-EC":200}""", Options())
            .Should().BeEquivalentTo(new Dictionary<CultureInfo, int>
              {
                  [TestCultures.en_GB] = 100,
                  [TestCultures.es_EC] = 200,
              });
    }

    [Test]
    public void Dictionary_serialization()
    {
        JsonSerializer.Serialize(new Dictionary<CultureInfo, int>
        {
            [TestCultures.en_GB] = 100,
            [TestCultures.es_EC] = 200,
        },
        Options())
            .Should().Be("""{"en-GB":100,"es-EC":200}""");
    }

    private static JsonSerializerOptions Options()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new CultureInfoJsonConverter());
        return options;
    }
}
#endif
