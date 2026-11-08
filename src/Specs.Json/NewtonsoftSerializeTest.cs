using Newtonsoft.Json;
using Qowaiv.Json.Newtonsoft;

namespace Qowaiv.Json.UnitTests;

[InheritsTests]
public class NewtonsoftSerializeTest : JsonSerializeTestBase<JsonSerializationException>
{
    public NewtonsoftSerializeTest()
    {
        JsonConvert.DefaultSettings ??= () => new JsonSerializerSettings { Converters = { new QowaivJsonConverter() } };
        QowaivJsonConverter.Register();
    }

    [Test]
    public void Register_ClearAll_Successful()
    {
        var settings = JsonConvert.DefaultSettings!.Invoke();

        settings.Converters.Clear();
        settings.Converters.Should().BeEmpty();

        QowaivJsonConverter.Register();

        settings = JsonConvert.DefaultSettings.Invoke();
        settings.Converters.Single().Should().BeOfType<QowaivJsonConverter>();
    }

    protected override T Deserialize<T>(string? jsonString)
        => JsonConvert.DeserializeObject<T>(jsonString!)!;

    protected override string Serialize(object obj)
        => JsonConvert.SerializeObject(obj);
}
