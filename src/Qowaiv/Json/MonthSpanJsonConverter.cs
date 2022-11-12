#if NET5_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a month span.</summary>
[Inheritable]
public class MonthSpanJsonConverter : SvoJsonConverter<MonthSpan>
{
    /// <inheritdoc />
    [Pure]
    protected override MonthSpan FromJson(string? json) => MonthSpan.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override MonthSpan FromJson(long json) => MonthSpan.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(MonthSpan svo) => svo.ToJson();
}

#endif
