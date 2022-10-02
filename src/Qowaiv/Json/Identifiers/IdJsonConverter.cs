#if NET5_0_OR_GREATER

using Qowaiv.Identifiers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json.Identifiers;


/// <summary>A custom <see cref="JsonConverter{T}"/> for <see cref="Id{TIdentifier}"/>'s.</summary>
public sealed class IdJsonConverter : JsonConverter<object>
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert is { IsGenericType: true }
        && typeToConvert.GetGenericTypeDefinition() == typeof(Id<>);

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
                JsonTokenType.False => FromJson(reader.GetString(), typeToConvert),
                JsonTokenType.Number => FromJson(reader.GetInt64(), typeToConvert),
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

        object? obj = ((dynamic)value).ToJson();
        
        if (obj is null)
        {
            writer.WriteNullValue();
        }
        else if (obj is long int64)
        {
            writer.WriteNumberValue(int64);
        }
        else if (obj is int int32)
        {
            writer.WriteNumberValue(int32);
        }
        else
        {
            writer.WriteStringValue(obj.ToString());
        }
    }

    [Pure]
    private object? FromJson(string? str, Type type)
    {
        if(!stringParsers.TryGetValue(type, out var parser))
        {
            parser = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == nameof(FromJson) && m.GetParameters()[0].ParameterType == typeof(string));

            stringParsers[type] = parser;
        }
        return parser.Invoke(null, new object?[] { str });
    }

    [Pure]
    private object? FromJson(long number, Type type)
    {
        if (!int64Parsers.TryGetValue(type, out var parser))
        {
            parser = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == nameof(FromJson) && m.GetParameters()[0].ParameterType == typeof(long));

            int64Parsers[type] = parser;
        }
        return parser.Invoke(null, new object?[] { number });
    }

    private static readonly Dictionary<Type, MethodInfo> stringParsers = new();
    private static readonly Dictionary<Type, MethodInfo> int64Parsers = new();
}

#endif
