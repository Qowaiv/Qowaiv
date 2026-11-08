using MongoDB.Bson.Serialization.Serializers;
using Qowaiv.Bson.MongoDB;
using Qowaiv.Json.UnitTests.Models;

namespace BSON.MongoDB.BSON_serialization_provider_specs;

public class Gets_serializer
{
    private static readonly QowaivBsonSerializationProvider provider = new();

    [Test]
    public void FromJson_string_only_SVO()
        => provider.GetSerializer(typeof(SvoWithFromJsonStringOnly))
        .Should().BeOfType<QowaivBsonSerializer<SvoWithFromJsonStringOnly>>();

    [Test]
    public void nullable_FromJson_string_only_SVO()
        => provider.GetSerializer(typeof(SvoWithFromJsonStringOnly?))
        .Should().BeOfType<NullableSerializer<SvoWithFromJsonStringOnly>>();

    [Test]
    public void FromJson_SVO_class()
        => provider.GetSerializer(typeof(SvoWithFromJsonClass))
        .Should().BeOfType<QowaivBsonSerializer<SvoWithFromJsonClass>>();
}

public class Gets_null
{
    private static readonly QowaivBsonSerializationProvider provider = new();

    [Test]
    public void null_type()
        => provider.GetSerializer(null).Should().BeNull();

    [Test]
    public void SVO_without_FromJson()
        => provider.GetSerializer(typeof(int)).Should().BeNull();

    [Test]
    public void SVO_with_FromJson_bool_only()
        => provider.GetSerializer(typeof(SvoWithFromJsonBoolOnly)).Should().BeNull();
 
    [Test]
    public void DTO()
        => provider.GetSerializer(typeof(DtoClass)).Should().BeNull();
}

public class Caches_for_performance_reasons
{
    [Test]
    public void serializers_already_requested()
    {
        var provider = new QowaivBsonSerializationProvider();
        var serializer = provider.GetSerializer(typeof(SvoWithFromJsonStringOnly));

        provider.GetSerializer(typeof(SvoWithFromJsonStringOnly))
            .Should().BeSameAs(serializer);
    }

    /// <remarks>This not fully proves that the cache has been hit.</remarks>
    [Test]
    public void unsupported_types()
    {
        var provider = new QowaivBsonSerializationProvider();
        provider.GetSerializer(typeof(int)).Should().BeNull();
        provider.GetSerializer(typeof(int)).Should().BeNull();
    }
}
