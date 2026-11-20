#if !NETSTANDARD2_0

using Qowaiv.Json;

namespace Qowaiv.Globalization.NL.Json;

/// <summary>Provides a JSON conversion for a kadastrale gemeente.</summary>
public sealed class KadastraleGemeenteJsonConverter : SvoJsonConverter<KadastraleGemeente>
{
    /// <inheritdoc />
    [Pure]
    protected override KadastraleGemeente FromJson(string? json) => KadastraleGemeente.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(KadastraleGemeente svo) => svo.ToJson();
}

#endif
