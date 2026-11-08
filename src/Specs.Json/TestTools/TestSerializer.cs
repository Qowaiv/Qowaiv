using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System.IO;
using System.Text;

namespace Qowaiv.Json.UnitTests.TestTools;

public static class TestSerializer
{
    public static string BsonSerialize(object obj)
    {
        using (var stream = new MemoryStream())
        {
            var writer = new StreamWriter(stream);
            var jsonWriter = new JsonWriter(writer);
            var nominaleType = obj?.GetType();
            BsonSerializer.Serialize(jsonWriter, nominaleType, obj);
            jsonWriter.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
