#if NET5_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a postal code.</summary>
public sealed class DateJsonConverter : SvoJsonConverter<Date>
{
    /// <inheritdoc />
    [Pure]
    protected override Date FromJson(string? json) => Date.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Date FromJson(long json) => Date.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Date svo) => svo.ToJson();
}

#endif

