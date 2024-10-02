#if NET6_0_OR_GREATER

using Qowaiv.Security.Cryptography;

namespace Qowaiv.Json.Security.Cryptography;

/// <summary>Provides a JSON conversion for a cryptographic seed.</summary>
public sealed class CryptographicSeedJsonConverter : SvoJsonConverter<CryptographicSeed>
{
    /// <inheritdoc />
    [Pure]
    protected override CryptographicSeed FromJson(string? json) => CryptographicSeed.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(CryptographicSeed svo) => svo.ToJson();
}

#endif
