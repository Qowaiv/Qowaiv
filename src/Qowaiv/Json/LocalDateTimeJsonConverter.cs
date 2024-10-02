#if NET6_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a local date time.</summary>
[Inheritable]
public class LocalDateTimeJsonConverter : SvoJsonConverter<LocalDateTime>
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
