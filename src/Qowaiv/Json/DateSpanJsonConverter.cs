#if NET6_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a date span.</summary>
[Inheritable]
public class DateSpanJsonConverter : SvoJsonConverter<DateSpan>
{
    /// <inheritdoc />
    [Pure]
    protected override DateSpan FromJson(string? json) => DateSpan.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override DateSpan FromJson(long json) => DateSpan.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(DateSpan svo) => svo.ToJson();
}

#endif
