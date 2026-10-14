#if NET10_0_OR_GREATER

using System.Text.Json;

namespace Qowaiv.Json;

/// <summary>Provides a JSON conversion for a yes-no.</summary>
internal sealed class YesNoJsonConverter : SvoJsonConverter<YesNo>
{
    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, YesNo value, JsonSerializerOptions options)
        => Guard.NotNull(writer).WriteRawValue(value.ToUtf8Json(), skipInputValidation: true);

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
