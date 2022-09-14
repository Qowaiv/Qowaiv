using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json;

/// <summary>A custom <see cref="JsonConverter{T}"/> for SVO's.</summary>
/// <typeparam name="TSvo">
/// The type of SVO.
/// </typeparam>
public class SvoJsonConverter<TSvo> : JsonConverter<TSvo> where TSvo: struct
{
    private readonly Func<TSvo, object?> ToJson;
    private readonly Func<string?, TSvo> FromJson;
    private readonly Func<long, TSvo> FromInt64;
    private readonly Func<double, TSvo> FromDouble;
    private readonly Func<bool, TSvo> FromBoolean;

    /// <summary>Creates a new instance of the <see cref="SvoJsonConverter{TSvo}"/>.</summary>
    /// <param name="toJson">
    /// Represents the SVO as JSON node.
    /// </param>
    /// <param name="fromString">
    /// Creates the SVO from a JSON string.
    /// </param>
    /// <param name="fromInt64">
    /// Creates the SVO from a JSON number (optional).
    /// </param>
    /// <param name="fromDouble">
    /// Creates the SVO from a JSON number (optional).
    /// </param>
    /// <param name="fromBoolean">
    /// Creates the SVO form a JSON boolean. (optional)
    /// </param>
    protected SvoJsonConverter(
        Func<TSvo, object?> toJson,
        Func<string?, TSvo> fromString,
        Func<long, TSvo>? fromInt64 = null,
        Func<double, TSvo>? fromDouble = null,
        Func<bool, TSvo>? fromBoolean = null)
    {
        ToJson = Guard.NotNull(toJson, nameof(toJson));
        FromJson = Guard.NotNull(fromString, nameof(fromString));
        FromInt64 = fromInt64 ?? ((n) => fromString(n.ToString(CultureInfo.InvariantCulture)));
        FromDouble = fromDouble ?? ((n) => fromString(n.ToString(CultureInfo.InvariantCulture)));
        FromBoolean = fromBoolean ?? ((n) => fromString(n.ToString(CultureInfo.InvariantCulture)));
    }

    /// <inheritdoc />
    public sealed override TSvo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => FromJson(reader.GetString()),
                JsonTokenType.True => FromBoolean(true),
                JsonTokenType.False => FromBoolean(false),
                JsonTokenType.Number => ReadNumber(ref reader),
                JsonTokenType.Null => default,
                _ => throw new JsonException($"Unexpected token parsing {typeToConvert.FullName}. {reader.TokenType} is not supported."),
            };
        }
        catch (Exception x)
        {
            if (x is JsonException) throw;
            else throw new JsonException(x.Message, x);
        }

        TSvo ReadNumber(ref Utf8JsonReader reader)
        {
            if (reader.TryGetInt64(out long num))
            {
                return FromInt64(num);
            }
            else if (reader.TryGetDouble(out double dec))
            {
                return FromDouble(dec);
            }
            else throw new JsonException($"QowaivJsonConverter does not support writing from {reader.GetString()}.");
        }
    }
   
    /// <inheritdoc />
    public sealed override void Write(Utf8JsonWriter writer, TSvo value, JsonSerializerOptions options)
    {
        var obj = ToJson(value);

        if (obj is null)
        {
            writer.WriteNullValue();
        }
        else if (obj is string str)
        {
            writer.WriteStringValue(str);
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
        else if (obj is int int_)
        {
            writer.WriteNumberValue(int_);
        }
        else if (obj is bool b)
        {
            writer.WriteBooleanValue(b);
        }
        else if (obj is DateTime dt)
        {
            writer.WriteStringValue(dt);
        }
        else if (obj is IFormattable f)
        {
            writer.WriteStringValue(f.ToString(null, CultureInfo.InvariantCulture));
        }
        else
        {
            writer.WriteStringValue(obj.ToString());
        }
    }
}
