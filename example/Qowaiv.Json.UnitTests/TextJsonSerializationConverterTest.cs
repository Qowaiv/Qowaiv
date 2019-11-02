using System;
using System.Text.Json;

namespace Qowaiv.Json.UnitTests
{
    public class TextJsonSerializationConverterTest : JsonSerializerTestBase<JsonException>
    {
        private static readonly JsonSerializerOptions options = GetOptions();

        private static JsonSerializerOptions GetOptions()
        {
            var o = new JsonSerializerOptions();
            o.Converters.Add(new Text.Json.Serialization.QowaivJsonConverter());
            return o;
        }

        protected override bool CanConvert(Type type)
        {
            var factory = new Text.Json.Serialization.QowaivJsonConverter();
            return factory.CanConvert(type);
        }

        protected override T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }

        protected override string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, options);
        }
    }
}
