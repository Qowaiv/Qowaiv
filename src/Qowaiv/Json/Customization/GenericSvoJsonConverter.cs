#if NET5_0_OR_GREATER

using Qowaiv.Customization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json.Customization;


/// <summary>A custom <see cref="JsonConverter{T}"/> for <see cref="Svo{TBehavior}"/>'s.</summary>
public sealed class GenericSvoJsonConverter : JsonConverter<object>
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert is { IsGenericType: true }
        && typeToConvert.GetGenericTypeDefinition() == typeof(Svo<>);

    /// <inheritdoc />
    [Pure]
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonTokenType.String or 
                JsonTokenType.True or
                JsonTokenType.False or
                JsonTokenType.Number => FromJson(reader.GetString(), typeToConvert),
                JsonTokenType.Null => Activator.CreateInstance(typeToConvert),
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
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        Guard.NotNull(writer, nameof(writer));

        string? json = ((dynamic)value).ToJson();
        if (json is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(json);
        }
    }

    [Pure]
    private object? FromJson(string? str, Type type)
    {
        if(!parsers.TryGetValue(type, out var parser))
        {
            parser = type.GetMethod(nameof(FromJson), BindingFlags.Public | BindingFlags.Static)!;
            parsers[type] = parser;
        }
        return parser.Invoke(null, new object?[] { str });
    }

    private static readonly Dictionary<Type, MethodInfo> parsers = new();
}

#endif
