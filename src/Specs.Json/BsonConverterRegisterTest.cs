using Qowaiv.Bson.MongoDB;
using Qowaiv.Json.UnitTests.TestTools;

namespace Qowaiv.Json.UnitTests;

[Obsolete("Will be dropped with the next major release.")]
public class BsonConverterRegisterTest
{
    [Test]
    public void RegisterAssembly_Qowaiv_SerializesSvoFromAssembly()
    {
        QowaivBsonConverter.RegisterAssembly(typeof(Date).Assembly);
        TestSerializer.BsonSerialize(new Date(2017, 06, 11)).Should().Be(@"""2017-06-11""");
    }

    [Test]
    public void RegisterTypeGeneric_Int_Throws()
        => 42.Invoking(_ => QowaivBsonConverter.RegisterType<int>()).Should().Throw<NotSupportedException>();
    
    [Test]
    public void RegisterType_Int_Throws()
        => 42.Invoking(_ => QowaivBsonConverter.RegisterType(typeof(int))).Should().Throw<NotSupportedException>();
}
