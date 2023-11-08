#if NET5_0_OR_GREATER

using Qowaiv.Identifiers;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json.Identifiers;

/// <summary>A custom <see cref="JsonConverter{T}"/> for <see cref="Id{TIdentifier}"/>'s.</summary>
public sealed class IdJsonConverter : JsonConverterFactory
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvert(Type typeToConvert)
        => Behavior(typeToConvert) is { };

    /// <inheritdoc />
    [Pure]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => Behavior(typeToConvert) is { } behavior
        ? (JsonConverter?)Activator.CreateInstance(typeof(IdConverter<>).MakeGenericType(behavior))
        : null;

    [Pure]
    private static Type? Behavior(Type type)
        => type is { IsGenericType: true } && type.GetGenericTypeDefinition() == typeof(Id<>)
        ? type.GetGenericArguments().Single()
        : null;

    private sealed class IdConverter<TBehavior> : JsonConverter<Id<TBehavior>>
        where TBehavior : IIdentifierBehavior, new()
    {
        public override Id<TBehavior> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return reader.TokenType switch
                {
                    JsonTokenType.String or
                    JsonTokenType.True or
                    JsonTokenType.False => (Id<TBehavior>)FromJson(reader.GetString(), typeToConvert),
                    JsonTokenType.Number => (Id<TBehavior>)FromJson(reader.GetInt64(), typeToConvert),
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

        public override void Write(Utf8JsonWriter writer, Id<TBehavior> value, JsonSerializerOptions options)
        {
            Guard.NotNull(writer);

            object? obj = value.ToJson();

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

#if NET6_0_OR_GREATER

        /// <inheritdoc />
        public override Id<TBehavior> ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => Id<TBehavior>.FromJson(reader.GetString());

        /// <inheritdoc />
        public override void WriteAsPropertyName(Utf8JsonWriter writer, Id<TBehavior> value, JsonSerializerOptions options)
            => writer.WritePropertyName(value.ToJson()?.ToString() ?? string.Empty);

#endif
    }

    [Pure]
    private static object FromJson(string? str, Type type)
    {
        if (!stringParsers.TryGetValue(type, out var parser))
        {
            parser = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == nameof(FromJson) && m.GetParameters()[0].ParameterType == typeof(string));

            stringParsers[type] = parser;
        }
        return parser.Invoke(null, new object?[] { str })!;
    }

    [Pure]
    private static object FromJson(long number, Type type)
    {
        if (!int64Parsers.TryGetValue(type, out var parser))
        {
            parser = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m => m.Name == nameof(FromJson) && m.GetParameters()[0].ParameterType == typeof(long));

            int64Parsers[type] = parser;
        }
        return parser.Invoke(null, new object?[] { number })!;
    }

    private static readonly ConcurrentDictionary<Type, MethodInfo> stringParsers = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> int64Parsers = new();
}

#endif
