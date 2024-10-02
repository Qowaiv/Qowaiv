#if NET6_0_OR_GREATER

using Qowaiv.Sustainability;

namespace Qowaiv.Json.Sustainability;

/// <summary>Provides a JSON conversion for a EU energy label.</summary>
public sealed class EnergyLabelJsonConverter : SvoJsonConverter<EnergyLabel>
{
    /// <inheritdoc />
    [Pure]
    protected override EnergyLabel FromJson(string? json) => EnergyLabel.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(EnergyLabel svo) => svo.ToJson();
}

#endif
