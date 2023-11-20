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
        ? (JsonConverter?)Activator.CreateInstance(typeof(GenericSvoJsonConverter<>).MakeGenericType(behavior))
        : null;

    [Pure]
    private static Type? Behavior(Type type)
        => type is { IsGenericType: true } && type.GetGenericTypeDefinition() == typeof(Svo<>)
        ? type.GetGenericArguments().Single()
        : null;

    [Pure]
    internal static Svo<TBehavior> FromJson<TBehavior>(string? str, Type type) where TBehavior : SvoBehavior, new()
    {
        if (!parsers.TryGetValue(type, out var parser))
        {
            parser = type.GetMethod(nameof(FromJson), BindingFlags.Public | BindingFlags.Static)!;
            parsers[type] = parser;
        }
        return (Svo<TBehavior>)parser.Invoke(null, [str])!;
    }

    private static readonly Dictionary<Type, MethodInfo> parsers = new();

    /// <summary>A custom <see cref="JsonConverter{T}"/> for <see cref="Svo{TBehavior}"/>'s.</summary>
    [Obsolete("Use GenericSvoJson<TBehavior> instead.")]
    public sealed class GenericSvoConverter<TBehavior> : JsonConverter<Svo<TBehavior>>
        where TBehavior : SvoBehavior, new()
    {
        private readonly GenericSvoJsonConverter<TBehavior> converter = new();

        /// <inheritdoc />
        [Pure]
        public override Svo<TBehavior> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => converter.Read(ref reader, typeToConvert, options);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Svo<TBehavior> value, JsonSerializerOptions options)
            => converter.Write(writer, value, options);
    }
}

#endif
