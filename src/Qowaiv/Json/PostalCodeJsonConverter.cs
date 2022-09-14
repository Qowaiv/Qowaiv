namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a postal code.</summary>
public sealed class PostalCodeJsonConverter : SvoJsonConverter<PostalCode>
{
    /// <summary>Creates a new instance of the <see cref="PostalCodeJsonConverter"/>.</summary>
    public PostalCodeJsonConverter()
        : base((svo) => svo.ToJson(), PostalCode.FromJson) { }
}
