using Newtonsoft.Json;
using Qowaiv.Reflection;
using System;
using System.Globalization;
using System.Linq;

namespace Qowaiv.Json
{
    /// <summary>The Qowaiv JSON converter converts types from and to JSON that implement the Qowaiv.Json.IJsonSerializable.</summary>
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

        /// <summary>Returns true if the object type is (nullable) IJsonSerializable.</summary>
        /// <param name="objectType">
        /// The object type to convert.
        /// </param>
        public override bool CanConvert(Type objectType)
        {
            return objectType != null && (QowaivType.IsIJsonSerializable(objectType) || QowaivType.IsNullableIJsonSerializable(objectType));
        }

        /// <summary>Reads the JSON representation of an IJsonSerializable.</summary>
        /// <param name="reader">
        /// The Newtonsoft.Json.JsonReader to read from.
        /// </param>
        /// <param name="objectType">
        /// The type of the object.
        /// </param>
        /// <param name="existingValue">
        /// The existing value of object being read.
        /// </param>
        /// <param name="serializer">
        /// The calling serializer.
        /// </param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Guard.NotNull(reader, nameof(reader));
            Guard.NotNull(objectType, nameof(objectType));

            var isNullable = QowaivType.IsNullable(objectType);
            var resultType = isNullable ? objectType.GetGenericArguments()[0] : objectType;
            var result = (IJsonSerializable)Activator.CreateInstance(resultType);

            try
            {
                switch (reader.TokenType)
                {
                    // Empty value for null-ables.
                    case JsonToken.Null:
                        if (isNullable) { return null; }
                        result.FromJson();
                        break;

                    // A number without digits.
                    case JsonToken.Integer:
                        result.FromJson((long)reader.Value);
                        break;

                    // A number with digits.
                    case JsonToken.Float:
                        result.FromJson((double)reader.Value);
                        break;

                    // A string.
                    case JsonToken.String:
                        result.FromJson((string)reader.Value);
                        break;
                    // Other scenario's are not supported.    
                    default:
                        throw new JsonSerializationException(string.Format(CultureInfo.CurrentCulture, QowaivMessages.JsonSerialization_TokenNotSupported, objectType.FullName, reader.TokenType));
                }
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
            return result;
        }

        /// <summary>Writes the JSON representation of an IJsonSerializable.</summary>
        /// <param name="writer">
        /// The Newtonsoft.Json.JsonWriter to write to.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="serializer">
        /// The calling serializer.
        /// </param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guard.NotNull(writer, nameof(writer));

            if (value is null)
            {
                writer.WriteNull();
            }

            var json = Guard.IsTypeOf<IJsonSerializable>(value, nameof(value));

            var jsonValue = json.ToJson();

            if (jsonValue is null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(jsonValue);
            }
        }
    }
}
