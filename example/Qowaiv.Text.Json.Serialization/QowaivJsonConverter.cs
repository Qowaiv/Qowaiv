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
        /// <param name="typeToConvert">
        /// The object type to convert.
        /// </param>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert != null && (QowaivType.IsIJsonSerializable(typeToConvert) || QowaivType.IsNullableIJsonSerializable(typeToConvert));
        }

        /// <summary>Creates a converter for a <see cref="IJsonSerializable"/>.</summary>
        /// <param name="typeToConvert">
        /// The <see cref="IJsonSerializable"/> type.
        /// </param>
        /// <param name="options">
        /// The serialization options to use (are ignored).
        /// </param>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (!_converters.TryGetValue(typeToConvert, out JsonConverter converter))
            {
                lock (locker)
                {
                    if (!_converters.TryGetValue(typeToConvert, out converter))
                    {
                        var converterType = typeof(QowaivJsonConverter<>).MakeGenericType(typeToConvert);
                        converter = (JsonConverter)Activator.CreateInstance(converterType);
                        _converters[typeToConvert] = converter;
                    }
                }
            }
            return converter;
        }

        private readonly object locker = new object();
        private readonly Dictionary<Type, JsonConverter> _converters = new Dictionary<Type, JsonConverter>();
    }

    /// <summary>Specific <see cref="JsonConverter{TSvo}"/>.</summary>
    /// <typeparam name="TSvo">
    /// The type that is <see cref="IJsonSerializable"/> or <see cref="Nullable{IJsonSerializable}"/>.
    /// </typeparam>
    internal class QowaivJsonConverter<TSvo> : JsonConverter<TSvo>
    {
        /// <summary>Reads and converts the JSON to type <typeparamref name="TSvo"/>.</summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="typeToConvert">
        /// The type to convert.
        /// </param>
        /// <param name="options">
        /// An object that specifies serialization options to use (ignored).
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public override TSvo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var type = QowaivType.GetNotNullableType(typeToConvert);
            var result = (IJsonSerializable)Activator.CreateInstance(type);

            var isNullable = typeof(TSvo) != typeToConvert || typeToConvert.IsClass;

            try
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.String:
                        result.FromJson(reader.GetString());
                        break;

                    case JsonTokenType.Number:
                        if (reader.TryGetInt64(out long num))
                        {
                            result.FromJson(num);
                        }
                        else if (reader.TryGetDouble(out double dec))
                        {
                            result.FromJson(dec);
                        }
                        else
                        {
                            throw new JsonException($"QowaivJsonConverter does not support writing from {reader.GetString()}.");
                        }
                        break;

                    case JsonTokenType.Null:
                        if (isNullable)
                        {
                            return default;
                        }
                        result.FromJson();
                        break;

                    default:
                        throw new JsonException($"Unexpected token parsing { typeToConvert.FullName }. { reader.TokenType} is not supported.");
                }
            }
            catch (Exception x)
            {
                if (x is JsonException)
                {
                    throw;
                }
                throw new JsonException(x.Message, x);
            }
            return (TSvo)result;
        }

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">
        /// The writer to write to.
        /// </param>    
        /// <param name="value">
        /// The value to convert to JSON.
        /// </param>
        /// <param name="options">
        /// An object that specifies serialization options to use.
        /// </param>
        public override void Write(Utf8JsonWriter writer, TSvo value, JsonSerializerOptions options)
        {
            var obj = ((IJsonSerializable)value).ToJson();

            if (obj is null)
            {
                writer.WriteNullValue();
            }
            else if (obj is decimal dec)
            {
                writer.WriteNumberValue(dec);
            }
            else if (obj is double dbl)
            {
                writer.WriteNumberValue(dbl);
            }
            else if (obj is long num)
            {
                writer.WriteNumberValue(num);
            }
            else if (obj is DateTime dt)
            {
                writer.WriteStringValue(dt);
            }
            else
            {
                writer.WriteStringValue(obj.ToString());
            }
        }
    }
}
