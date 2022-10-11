#if NET5_0_OR_GREATER

using Qowaiv.Customization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json.Customization;

/// <summary>A custom <see cref="JsonConverter{T}"/> for <see cref="Svo{TBehavior}"/>'s.</summary>
public sealed class GenericSvoJsonConverter : JsonConverterFactory
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvert(Type typeToConvert)
        => Behavior(typeToConvert) is { };

    /// <inheritdoc />
    [Pure]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => Behavior(typeToConvert) is { } behavior
        ? (JsonConverter?)Activator.CreateInstance(typeof(GenericSvoConverter<>).MakeGenericType(behavior))
        : null;

    [Pure]
    private static Type? Behavior(Type type)
        => type is { IsGenericType: true } && type.GetGenericTypeDefinition() == typeof(Svo<>)
        ? type.GetGenericArguments().Single()
        : null;

    /// <summary>A custom <see cref="JsonConverter{T}"/> for <see cref="Svo{TBehavior}"/>'s.</summary>
    public sealed class GenericSvoConverter<TBehavior> : JsonConverter<Svo<TBehavior>>
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
                    JsonTokenType.Number => (Svo<TBehavior>)FromJson(reader.GetString(), typeToConvert),
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
            Guard.NotNull(writer, nameof(writer));

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

    [Pure]
    private static object FromJson(string? str, Type type)
    {
        if (!parsers.TryGetValue(type, out var parser))
        {
            parser = type.GetMethod(nameof(FromJson), BindingFlags.Public | BindingFlags.Static)!;
            parsers[type] = parser;
        }
        return parser.Invoke(null, new object?[] { str })!;
    }

    private static readonly Dictionary<Type, MethodInfo> parsers = new();
}

#endif
