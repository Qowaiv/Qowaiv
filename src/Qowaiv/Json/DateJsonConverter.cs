#if NET6_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a date.</summary>
[Inheritable]
public class DateJsonConverter : SvoJsonConverter<Date>
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

