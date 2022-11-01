#if NET5_0_OR_GREATER

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a yes-no.</summary>
[Inheritable]
public class YesNoJsonConverter : SvoJsonConverter<YesNo>
{
    /// <inheritdoc />
    [Pure]
    protected override YesNo FromJson(string? json) => YesNo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override YesNo FromJson(long json) => YesNo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override YesNo FromJson(double json) => YesNo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override YesNo FromJson(bool json) => YesNo.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(YesNo svo) => svo.ToJson();
}

#endif
