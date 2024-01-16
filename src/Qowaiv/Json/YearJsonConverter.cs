#if NET6_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a year.</summary>
[Inheritable]
public class YearJsonConverter : SvoJsonConverter<Year>
{
    /// <inheritdoc />
    [Pure]
    protected override Year FromJson(string? json) => Year.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Year FromJson(long json) => Year.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Year FromJson(double json) => Year.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Year svo) => svo.ToJson();
}

#endif
