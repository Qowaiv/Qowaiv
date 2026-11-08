namespace Qowaiv.Internals;

internal partial class ConventionBasedSerializer<TSvo> : JsonConverter
{
    /// <inheritdoc />
    [Pure]
    public override bool CanConvert(Type objectType)
        => objectType == typeof(TSvo) && TypeIsSupported;

    /// <inheritdoc />
    [Impure]
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        Guard.NotNull(reader, nameof(reader));
        Guard.NotNull(objectType, nameof(objectType));

        var type = objectType;
        var isNullable = type != objectType || objectType.IsClass;

        try
        {
            return reader.TokenType switch
            {
                // Empty value for null-ables.
                JsonToken.Null /*....*/ => isNullable ? null : Activator.CreateInstance(objectType),
                JsonToken.String /*..*/ => fromJsonString(reader.Value as string),
                JsonToken.Float /*...*/ => fromJsonDouble((double)reader.Value!),
                JsonToken.Integer /*.*/ => fromJsonLong((long)reader.Value!),
                JsonToken.Boolean /*.*/ => fromJsonBool(true.Equals(reader.Value)),
                JsonToken.Date /*....*/ => fromJsonString(((DateTime)reader.Value!).ToString(CultureInfo.InvariantCulture)),

                // Other scenario's are not supported.
                _ => throw new JsonSerializationException($"Unexpected token parsing {objectType.FullName}. {reader.TokenType} is not supported."),
            };
        }

        // We want to communicate exceptions as JSON serialization exceptions.
        catch (Exception x)
        {
            if (x is JsonSerializationException || x is JsonReaderException)
            {
                throw;
            }

            throw new JsonSerializationException(x.Message, x);
        }
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        Guard.NotNull(writer, nameof(writer));

        if (value is TSvo json && ToJson(json) is { } node)
        {
            writer.WriteValue(node);
        }
        else
        {
            writer.WriteNull();
        }
    }
}
