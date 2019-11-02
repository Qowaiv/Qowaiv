﻿using Newtonsoft.Json;
using Qowaiv.Reflection;
using System;
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
            if (reader is null) { throw new ArgumentNullException(nameof(reader)); }
            if (objectType is null) { throw new ArgumentNullException(nameof(objectType)); }

            var type = QowaivType.GetNotNullableType(objectType);
            var result = (IJsonSerializable)Activator.CreateInstance(type);

            var isNullable = type != objectType || objectType.IsClass;

            try
            {
                switch (reader.TokenType)
                {
                    // Empty value for null-ables.
                    case JsonToken.Null:
                        if (isNullable)
                        {
                            return null; 
                        }
                        result.FromJson();
                        break;

                    // A string.
                    case JsonToken.String:
                        result.FromJson((string)reader.Value);
                        break;

                    // A number without digits.
                    case JsonToken.Integer:
                        result.FromJson((long)reader.Value);
                        break;

                    // A number with digits.
                    case JsonToken.Float:
                        result.FromJson((double)reader.Value);
                        break;

                    // A date.
                    case JsonToken.Date:
                        result.FromJson((DateTime)reader.Value);
                        break;

                    // Other scenario's are not supported.    
                    default:
                        throw new JsonSerializationException($"Unexpected token parsing {objectType.FullName}. {reader.TokenType} is not supported.");
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
            if (writer is null) { throw new ArgumentNullException(nameof(writer)); }
            
            var json = (IJsonSerializable)value;

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
