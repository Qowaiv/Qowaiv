#if NET5_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a postal code.</summary>
public sealed class WeekDateJsonConverter : SvoJsonConverter<WeekDate>
{
    /// <inheritdoc />
    [Pure]
    protected override WeekDate FromJson(string? json) => WeekDate.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(WeekDate svo) => svo.ToJson();
}

#endif
