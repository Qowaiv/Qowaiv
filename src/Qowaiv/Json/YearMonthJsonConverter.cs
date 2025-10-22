#if NET8_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a year-month.</summary>
public sealed class YearMonthJsonConverter : SvoJsonConverter<YearMonth>
{
    /// <inheritdoc />
    [Pure]
    protected override YearMonth FromJson(string? json) => YearMonth.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(YearMonth svo) => svo.ToJson();
}

#endif
