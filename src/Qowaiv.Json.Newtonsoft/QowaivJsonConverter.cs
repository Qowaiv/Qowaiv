using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;

namespace Qowaiv.Json
{
	/// <summary>The Qowaiv JSON converter converts types from and to JSON that implement the Qowaiv.Json.IJsonSerializable.</summary>
	public class QowaivJsonConverter : Newtonsoft.Json.JsonConverter
	{
		/// <summary>Registers the Qowaiv JSON converter.</summary>
		public static void Register()
		{
			if (JsonConvert.DefaultSettings == null)
			{
				JsonConvert.DefaultSettings = () => new JsonSerializerSettings() { Converters = { new QowaivJsonConverter() } };
			}
			else
			{
				var settings = Newtonsoft.Json.JsonConvert.DefaultSettings.Invoke();
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
			return objectType != null && (IsIJsonSerializable(objectType) || IsNullableIJsonSerializable(objectType));
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
			Guard.NotNull(objectType, "objectType");

			var isNullable = objectType.IsGenericType;
			var result = (IJsonSerializable)Activator.CreateInstance(isNullable ? objectType.GetGenericArguments()[0] : objectType);

			switch (reader.TokenType)
			{
				// Empty value for nullables.
				case JsonToken.Null:
					if (isNullable) { return null; }
					result.FromJson();
					break;

				// A number without digits.
				case JsonToken.Integer:
					result.FromJson((Int64)reader.Value);
					break;

				// A number with digits.
				case JsonToken.Float:
					result.FromJson((Double)reader.Value);
					break;

				// A string.
				case JsonToken.String:
					result.FromJson((String)reader.Value);
					break;
				// Other scenario's are not supported.    
				default:
					throw new JsonSerializationException(String.Format(CultureInfo.CurrentCulture, QowaivMessages.JsonSerialization_TokenNotSupported, objectType.FullName, reader.TokenType));
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
			if (value == null) { writer.WriteNull(); }

			var json = (IJsonSerializable)value;

			var jsonValue = json.ToJson();

			if (jsonValue == null)
			{
				writer.WriteNull();
			}
			else
			{
				writer.WriteValue(jsonValue);
			}
		}

		/// <summary>Returns true if the object type is an IJsonSerializable.</summary>
		private static bool IsIJsonSerializable(Type objectType)
		{
			return objectType.GetInterfaces().Any(iface => iface == typeof(IJsonSerializable));
		}

		/// <summary>Returns true if the object type is a nullable IJsonSerializable.</summary>
		private static bool IsNullableIJsonSerializable(Type objectType)
		{
			return
				objectType.IsGenericType &&
				objectType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
				IsIJsonSerializable(objectType.GetGenericArguments()[0]);
		}
	}
}
