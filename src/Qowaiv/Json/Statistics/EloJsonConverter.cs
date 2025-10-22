#if NET8_0_OR_GREATER

using Qowaiv.Statistics;

namespace Qowaiv.Json.Statistics;

/// <summary>Provides a JSON conversion for an Elo.</summary>
[Inheritable]
public class EloJsonConverter : SvoJsonConverter<Elo>
{
    /// <inheritdoc />
    [Pure]
    protected override Elo FromJson(string? json) => Elo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Elo FromJson(long json) => Elo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Elo FromJson(double json) => Elo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Elo svo) => svo.ToJson();
}

#endif
