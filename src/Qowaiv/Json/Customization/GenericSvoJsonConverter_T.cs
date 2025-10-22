#if NET8_0_OR_GREATER

using Qowaiv.Customization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json.Customization;

/// <summary>A custom <see cref="JsonConverter{T}" /> for <see cref="Svo{TBehavior}" />'s.</summary>
internal sealed class GenericSvoJsonConverter<TBehavior> : JsonConverter<Svo<TBehavior>>
        where TBehavior : SvoBehavior, new()
{
    /// <inheritdoc />
    [Pure]
    public override Svo<TBehavior> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonTokenType.String or
                JsonTokenType.True or
                JsonTokenType.False or
                JsonTokenType.Number => GenericSvoJsonConverter.FromJson<TBehavior>(reader.GetString(), typeToConvert),
                JsonTokenType.Null => default,
                _ => throw new JsonException($"Unexpected token parsing {typeToConvert.FullName}. {reader.TokenType} is not supported."),
            };
        }
        catch (Exception x)
        {
            if (x is JsonException) throw;
            else throw new JsonException(x.Message, x);
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Svo<TBehavior> value, JsonSerializerOptions options)
    {
        Guard.NotNull(writer);

        if (value.ToJson() is { } json)
        {
            writer.WriteStringValue(json);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
#endif
