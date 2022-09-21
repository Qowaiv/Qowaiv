#if NET5_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a postal code.</summary>
public sealed class LocalDateTimeJsonConverter : SvoJsonConverter<LocalDateTime>
{
    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime FromJson(string? json) => LocalDateTime.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override LocalDateTime FromJson(long json) => LocalDateTime.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(LocalDateTime svo) => svo.ToJson();
}

#endif
