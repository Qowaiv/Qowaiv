namespace Qowaiv.Bson.MongoDB;

/// <summary>A BSON converter that converts Single Value Objects based on naming conventions.</summary>
/// <typeparam name="TSvo">
/// The specific type of the Single Value object to convert.
/// </typeparam>
[Inheritable]
public class QowaivBsonSerializer<TSvo> : SerializerBase<TSvo>
{
    private readonly ConventionBasedSerializer<TSvo> serializer = new();

    /// <inheritdoc/>
    [Pure]
    public override TSvo Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        => serializer.Deserialize(context, args);

    /// <inheritdoc/>
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TSvo value)
        => serializer.Serialize(context, args, value);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"{GetType().Namespace}.{nameof(QowaivBsonSerializer<TSvo>)}<{typeof(TSvo)}>";

    /// <summary>Returns true if <typeparamref name="TSvo"/> is supported.</summary>
    internal bool TypeIsSupported => serializer.TypeIsSupported;
}
