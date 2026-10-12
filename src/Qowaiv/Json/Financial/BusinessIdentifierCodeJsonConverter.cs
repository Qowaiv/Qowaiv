#if NET8_0_OR_GREATER

using Qowaiv.Financial;
using System.Text.Json;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for a BIC.</summary>
[Inheritable]
public class BusinessIdentifierCodeJsonConverter : SvoJsonConverter<BusinessIdentifierCode>
{
    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, BusinessIdentifierCode value, JsonSerializerOptions options)
        => writer.WriteAsciiString(value.ToJson());

    /// <inheritdoc />
    [Pure]
    protected override BusinessIdentifierCode FromJson(string? json) => BusinessIdentifierCode.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(BusinessIdentifierCode svo) => svo.ToJson();
}

#endif
