#if !NETSTANDARD2_0

using Qowaiv.Json;

namespace Qowaiv.Locale.NL.Json;

/// <summary>Provides a JSON conversion for a KvK-nummer.</summary>
public sealed class KvkNummerJsonConverter : SvoJsonConverter<KvkNummer>
{
    /// <inheritdoc />
    [Pure]
    protected override KvkNummer FromJson(string? json) => KvkNummer.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override KvkNummer FromJson(long json) => KvkNummer.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(KvkNummer svo) => svo.ToJson();
}

#endif
