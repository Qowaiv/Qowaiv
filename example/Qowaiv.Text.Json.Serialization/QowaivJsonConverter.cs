using Qowaiv.Json;
using Qowaiv.Reflection;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Text.Json.Serialization
{
    /// <summary>The Qowaiv JSON converter converts types from and to JSON that implement the Qowaiv.Json.IJsonSerializable.</summary>
    public class QowaivJsonConverter : JsonConverterFactory
    {
        /// <summary>Returns true if the object type is (nullable) IJsonSerializable.</summary>
        /// <param name="objectType">
        /// The object type to convert.
        /// </param>
        public override bool CanConvert(Type objectType)
        {
            return objectType != null && (QowaivType.IsIJsonSerializable(objectType) || QowaivType.IsNullableIJsonSerializable(objectType));
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var type = QowaivType.GetNotNullableType(typeToConvert);

            if (!_converters.TryGetValue(type, out JsonConverter converter))
            {
                lock (locker)
                {
                    if (!_converters.TryGetValue(type, out converter))
                    {
                        var converterType = typeof(QowaivJsonConverter<>).MakeGenericType(type);
                        converter = (JsonConverter)Activator.CreateInstance(converterType);
                        _converters[type] = converter;
                    }
                }
            }
            return converter;
        }

        private readonly object locker = new object();
        private readonly Dictionary<Type, JsonConverter> _converters = new Dictionary<Type, JsonConverter>();
    }

    internal class QowaivJsonConverter<TSvo> : JsonConverter<TSvo> where TSvo : IJsonSerializable
    {
        public override TSvo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TSvo svo = default;

            var isNullable = typeof(TSvo) != typeToConvert;

            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    svo.FromJson(reader.GetString());
                    break;

                case JsonTokenType.Number:
                    if(reader.TryGetInt64(out long num))
                    {
                        svo.FromJson(num);
                    }
                    else if(reader.TryGetDouble(out double dec))
                    {
                        svo.FromJson(dec);
                    }
                    else
                    {
                        throw new JsonException($"QowaivJsonConverter does not support writing from {reader.GetString()}.");
                    }
                    break;

                case JsonTokenType.Null:
                    if(isNullable)
                    {
                        //return null;
                    }
                    svo.FromJson();
                    break;

                default:
                    throw new JsonException($"QowaivJsonConverter does not support token type {reader.TokenType}.");
            }

            return svo;
        }

        public override void Write(Utf8JsonWriter writer, TSvo value, JsonSerializerOptions options)
        {
            var obj = value.ToJson();
            
            if(obj is string str)
            {
                writer.WriteStringValue(str);
            }
            else if(obj is decimal dec)
            {
                writer.WriteNumberValue(dec);
            }
            else if(obj is long num)
            {
                writer.WriteNumberValue(num);
            }
            else if(obj is DateTime dt)
            {
                writer.WriteStringValue(dt);
            }
            else if (obj is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                throw new JsonException($"QowaivJsonConverter does not support writing to {obj.GetType()}.");
            }
        }
    }
}
