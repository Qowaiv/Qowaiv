#if !NETSTANDARD2_0

using Qowaiv.Json;

namespace Qowaiv.Locale.NL.Json;

/// <summary>Provides a JSON conversion for a RSIN.</summary>
public sealed class RsinJsonConverter : SvoJsonConverter<Rsin>
{
    /// <inheritdoc />
    [Pure]
    protected override Rsin FromJson(string? json) => Rsin.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Rsin FromJson(long json) => Rsin.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Rsin svo) => svo.ToJson();
}

#endif
