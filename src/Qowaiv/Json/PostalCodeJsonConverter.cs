#if NET8_0_OR_GREATER

using System.Text.Json;

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a postal code.</summary>
[Inheritable]
public class PostalCodeJsonConverter : SvoJsonConverter<PostalCode>
{
    /// <inheritdoc />
    [Pure]
    protected override PostalCode FromJson(string? json) => PostalCode.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(PostalCode svo) => svo.ToJson();
}

#endif
