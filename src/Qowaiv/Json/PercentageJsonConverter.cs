#if NET6_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a percentage.</summary>
[Inheritable]
public class PercentageJsonConverter : SvoJsonConverter<Percentage>
{
    /// <inheritdoc />
    [Pure]
    protected override Percentage FromJson(string? json) => Percentage.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Percentage FromJson(long json) => Percentage.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Percentage FromJson(double json) => Percentage.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Percentage svo) => svo.ToJson();
}

#endif
