#if NET8_0_OR_GREATER

using Qowaiv.Financial;
using System.Buffers.Text;
using System.Text.Json;

namespace Qowaiv.Json.Financial;

/// <summary>Provides a JSON conversion for an amount.</summary>
[Inheritable]
public class AmountJsonConverter : SvoJsonConverter<Amount>
{
    /// <inheritdoc />
    public override Amount Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Utf8Parser.TryParse(reader.ValueSpan, out decimal dec, out int consumed)
        && reader.ValueSpan.Length == consumed
        ? Amount.Create(dec)
        : base.Read(ref reader, typeToConvert, options);

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Amount value, JsonSerializerOptions options)
        => writer.WriteNumberValue((decimal)value);

    /// <inheritdoc />
    [Pure]
    protected override Amount FromJson(string? json) => Amount.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Amount FromJson(long json) => Amount.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Amount FromJson(decimal json) => Amount.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override Amount FromJson(double json) => Amount.FromJson(json);

    /// <inheritdoc />
    [Pure]
    protected override object? ToJson(Amount svo) => svo.ToJson();
}

#endif
