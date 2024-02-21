namespace Qowaiv.TestTools;

/// <summary>Wrapper used by <see cref="Serialize.Xml{T}(T)"/>
/// and <see cref="SerializeDeserialize.Xml{T}(T)"/>.
/// </summary>
[Serializable]
[XmlRoot("Wrapper")]
public sealed class SerializationWrapper<T>
{
    /// <summary>The generic part of the wrapper.</summary>
    public T? Value { get; init; }
}
