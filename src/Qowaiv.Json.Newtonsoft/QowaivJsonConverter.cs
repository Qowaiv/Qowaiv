using System.Collections.Concurrent;

namespace Qowaiv.Json.Newtonsoft;

/// <summary>A JSON converter that converts Single Value Objects based on naming conventions.</summary>
[Inheritable]
public class QowaivJsonConverter : JsonConverter
{
    /// <summary>Registers the Qowaiv JSON converter.</summary>
    public static void Register()
    {
        if (JsonConvert.DefaultSettings is null)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings { Converters = { new QowaivJsonConverter() } };
        }
        else
        {
            var settings = JsonConvert.DefaultSettings.Invoke();
            if (!settings.Converters.Any(c => c.GetType() == typeof(QowaivJsonConverter)))
            {
                settings.Converters.Add(new QowaivJsonConverter());
            }
        }
    }

    /// <inheritdoc />
    [Pure]
    public override bool CanConvert(Type objectType)
        => objectType.NotNullable is { IsPrimitive: false } type
        && CreateConverter(type) is { };

    /// <inheritdoc />
    [Impure]
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        Guard.NotNull(objectType);
        var converter = CreateConverter(objectType) ?? throw new NotSupportedException($"Type '{objectType}' is not supported.");
        return converter.ReadJson(reader, objectType, existingValue, serializer);
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var objectType = Guard.NotNull(value).GetType();
        var converter = CreateConverter(objectType) ?? throw new NotSupportedException($"Type '{objectType}' is not supported.");
        converter.WriteJson(writer, value, serializer);
    }

    [Pure]
    private JsonConverter? CreateConverter(Type objectType)
    {
        var type = objectType.NotNullable!;

        if (!converters.TryGetValue(type, out var converter))
        {
            var converterType = typeof(ConventionBasedSerializer<>).MakeGenericType(type);
            converter = (JsonConverter)Activator.CreateInstance(converterType)!;

            if (converter.CanConvert(type))
            {
                converters[type] = converter;
            }
            else
            {
                converters[type] = null;
                return null;
            }
        }
        return converter;
    }

    private readonly ConcurrentDictionary<Type, JsonConverter?> converters = [];
}
