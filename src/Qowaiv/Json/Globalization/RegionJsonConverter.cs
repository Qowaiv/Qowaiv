#if NET6_0_OR_GREATER

using Qowaiv.Globalization;

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a region.</summary>
public sealed class RegionJsonConverter : SvoJsonConverter<Region>
{
    /// <inheritdoc />
    [Pure]
    protected override Region FromJson(string? json) => Region.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Region svo) => svo.ToJson();
}

#endif
