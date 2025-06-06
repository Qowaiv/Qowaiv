#if NET6_0_OR_GREATER

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qowaiv.Json;

/// <summary>A custom <see cref="JsonConverter{T}" /> for SVO's.</summary>
/// <typeparam name="TSvo">
/// The type of SVO.
/// </typeparam>
public abstract class SvoJsonConverter<TSvo> : JsonConverter<TSvo> where TSvo : struct
{
    /// <inheritdoc />
    public sealed override TSvo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => FromJson(reader.GetString()),
                JsonTokenType.True => FromJson(true),
                JsonTokenType.False => FromJson(false),
                JsonTokenType.Number => ReadNumber(ref reader),
                JsonTokenType.Null => default,
                _ => throw new JsonException($"Unexpected token parsing {typeToConvert?.FullName}. {reader.TokenType} is not supported."),
            };
        }
        catch (Exception x)
        {
            if (x is JsonException) throw;
            else throw new JsonException(x.Message, x);
        }
    }

    /// <inheritdoc />
    public sealed override void Write(Utf8JsonWriter writer, TSvo value, JsonSerializerOptions options)
    {
        Guard.NotNull(writer);

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

#if NET6_0_OR_GREATER

    /// <inheritdoc />
    public override TSvo ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => FromJson(reader.GetString());

    /// <inheritdoc />
    public override void WriteAsPropertyName(Utf8JsonWriter writer, TSvo value, JsonSerializerOptions options)
        => writer.WritePropertyName(ToJson(value)?.ToString() ?? string.Empty);
#endif

    /// <summary>Represent the SVO as a JSON node.</summary>
    [Pure]
    protected abstract object? ToJson(TSvo svo);

    /// <summary>Creates the SVO based on its JSON string representation.</summary>
    [Pure]
    protected virtual TSvo? FromJson(ReadOnlySpan<byte> json) => null;

    /// <summary>Creates the SVO based on its JSON string representation.</summary>
    [Pure]
    protected abstract TSvo FromJson(string? json);

    /// <summary>Creates the SVO based on its JSON (long) number representation.</summary>
    [Pure]
    protected virtual TSvo FromJson(long json) => FromJson(json.ToString(CultureInfo.InvariantCulture));

    /// <summary>Creates the SVO based on its JSON (decimal) number representation.</summary>
    [Pure]
    protected virtual TSvo FromJson(decimal json) => FromJson((double)json);

    /// <summary>Creates the SVO based on its JSON (double) number representation.</summary>
    [Pure]
    protected virtual TSvo FromJson(double json) => FromJson(json.ToString(CultureInfo.InvariantCulture));

    /// <summary>Creates the SVO based on its JSON boolean representation.</summary>
    [Pure]
    protected virtual TSvo FromJson(bool json) => FromJson(json ? "true" : "false");

    private TSvo ReadNumber(ref Utf8JsonReader reader) => reader switch
    {
        _ when reader.TryGetInt64(out long num) => FromJson(num),
        _ when reader.TryGetDecimal(out decimal dec) => FromJson(dec),
        _ when reader.TryGetDouble(out double dbl) => FromJson(dbl),
        _ => throw new JsonException($"QowaivJsonConverter does not support reading from {reader.GetString()}."),
    };
}
#endif
