namespace Qowaiv.Internals;

internal partial class ConventionBasedSerializer<TSvo> : SerializerBase<TSvo>
{
    /// <inheritdoc/>
    [Pure]
    public override TSvo Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        Guard.NotNull(context, nameof(context));

        var reader = context.Reader;
        var bsonType = reader.GetCurrentBsonType();

        switch (bsonType)
        {
            case BsonType.Null:
                // consume and ignore.
                reader.SkipValue();
                return default!;

            case BsonType.DateTime:
            case BsonType.String:
                return FromJson(reader.ReadString());

            case BsonType.Double:
                return FromJson(reader.ReadDouble());

            case BsonType.Boolean:
                return FromJson(reader.ReadBoolean());

            case BsonType.Int32:
                return FromJson(reader.ReadInt32());

            case BsonType.Int64:
                return FromJson(reader.ReadInt64());

            // These values are not supported:
            //
            // case BsonType.Decimal128:
            // case BsonType.EndOfDocument:
            // case BsonType.Document:
            // case BsonType.Array:
            // case BsonType.Binary:
            // case BsonType.Undefined:
            // case BsonType.ObjectId:
            // case BsonType.RegularExpression:
            // case BsonType.JavaScript:
            // case BsonType.Symbol:
            // case BsonType.JavaScriptWithScope:
            // case BsonType.Timestamp:
            // case BsonType.MinKey:
            // case BsonType.MaxKey:
            default:
                throw CreateCannotDeserializeFromBsonTypeException(reader.GetCurrentBsonType());
        }
    }

    /// <inheritdoc/>
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TSvo value)
    {
        Guard.NotNull(context, nameof(context));

        var writer = context.Writer;
        var obj = ToJson(value);

        if (obj is null)
        {
            writer.WriteNull();
        }
        else if (obj is string str)
        {
            writer.WriteString(str);
        }
        else if (obj is decimal dec)
        {
            writer.WriteDouble((double)dec);
        }
        else if (obj is double dbl)
        {
            writer.WriteDouble(dbl);
        }
        else if (obj is long num)
        {
            if (num is >= int.MinValue and <= int.MaxValue)
            {
                writer.WriteInt32((int)num);
            }
            else
            {
                writer.WriteInt64(num);
            }
        }
        else if (obj is int int_)
        {
            writer.WriteInt32(int_);
        }
        else if (obj is bool b)
        {
            writer.WriteBoolean(b);
        }
        else if (obj is DateTime dt)
        {
            writer.WriteDateTime(dt.Ticks);
        }
        else if (obj is IFormattable f)
        {
            writer.WriteString(f.ToString(null, CultureInfo.InvariantCulture));
        }
        else
        {
            writer.WriteString(obj.ToString());
        }
    }
}
