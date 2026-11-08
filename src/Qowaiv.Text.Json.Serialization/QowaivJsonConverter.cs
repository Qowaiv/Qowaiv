using System.Collections.Concurrent;

namespace Qowaiv.Text.Json.Serialization;

/// <summary>A JSON converter that converts Single Value Objects based on naming conventions.</summary>
public sealed class QowaivJsonConverter : JsonConverterFactory
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert is { NotNullable.IsPrimitive: false }
        && CreateConverter(typeToConvert, null) is { };

    /// <inheritdoc />
    [Pure]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions? options)
    {
        if (!converters.TryGetValue(typeToConvert, out var converter))
        {
            var converterType = typeof(ConventionBasedSerializer<>).MakeGenericType(typeToConvert);
            converter = (JsonConverter)Activator.CreateInstance(converterType)!;

            if (converter.CanConvert(typeToConvert))
            {
                converters[typeToConvert] = converter;
            }
            else
            {
                converters[typeToConvert] = null;
                return null;
            }
        }
        return converter;
    }

    private readonly ConcurrentDictionary<Type, JsonConverter?> converters = [];
}
