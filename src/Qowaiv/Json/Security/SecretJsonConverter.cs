#if NET5_0_OR_GREATER

namespace Qowaiv.Json.Security;

/// <summary>Provides a JSON conversion for a secret.</summary>
public sealed class SecretJsonConverter : SvoJsonConverter<Secret>
{
    /// <inheritdoc />
    [Pure]
    protected override Secret FromJson(string? json) => Secret.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Secret svo) => svo.ToJson();
}

#endif
