using MongoDB.Bson.Serialization;
using Qowaiv.Bson.MongoDB;
using Qowaiv.Json.UnitTests.Models;
using Qowaiv.Json.UnitTests.TestTools;

namespace Qowaiv.Json.UnitTests;

[InheritsTests]
public class BsonMongoDBSerializeTest : JsonSerializeTestBase<FormatException>
{
    public BsonMongoDBSerializeTest()
    {
        BsonSerializer.RegisterSerializationProvider(new QowaivBsonSerializationProvider());
    }

    [Test, Skip("For BSON a Date() is generated, that is default behaviour we don't want to interfere with.")]
    public override void Serialize_DateTime_Successful() { }

    [Test]
    public void Serialize_LongBiggerThanIntMax_Successful()
        => Serialize(new SvoWithFromJson(123456789000)).Should().Be("123456789000");

    protected override T Deserialize<T>(string? jsonString)
        => BsonSerializer.Deserialize<T>(jsonString);

    protected override string Serialize(object obj) => TestSerializer.BsonSerialize(obj);
}
