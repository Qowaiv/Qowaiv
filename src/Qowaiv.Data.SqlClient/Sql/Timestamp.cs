namespace Qowaiv.Sql;

/// <summary>Represents a timestamp.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable, SingleValueObject(SingleValueStaticOptions.Continuous, typeof(ulong))]
[OpenApiDataType(description: "SQL Server timestamp notation.", example: "0x00000000000007D9", type: "string", format: "timestamp")]
[TypeConverter(typeof(Conversion.Sql.TimestampTypeConverter))]
public partial struct Timestamp : ISerializable, IXmlSerializable, IFormattable, IEquatable<Timestamp>, IComparable, IComparable<Timestamp>
{
    /// <summary>Gets the minimum value of a timestamp.</summary>
    public static readonly Timestamp MinValue;

    /// <summary>Gets the maximum value of a timestamp.</summary>
    public static readonly Timestamp MaxValue = new(ulong.MaxValue);

    /// <summary>Represents the timestamp .</summary>
    [Pure]
    public byte[] ToByteArray() => BitConverter.GetBytes(m_Value);

    /// <summary>Deserializes the timestamp from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized timestamp.
    /// </returns>
    [Pure]
    public static Timestamp FromJson(double json) => Create((long)json);

    /// <summary>Deserializes the timestamp from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized timestamp.
    /// </returns>
    [Pure]
    public static Timestamp FromJson(long json) => Create(json);

    /// <summary>Serializes the timestamp to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string ToJson() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Returns a <see cref="string"/> that represents the current timestamp for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => ToString(CultureInfo.InvariantCulture);

    /// <summary>Returns a formatted <see cref="string"/> that represents the current timestamp.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
        {
            return formatted;
        }
        else if (string.IsNullOrEmpty(format))
        {
            return string.Format(formatProvider, "0x{0:X16}", m_Value);
        }
        else return m_Value.ToString(format, formatProvider);
    }

    /// <summary>Gets an XML string representation of the timestamp.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Casts a timestamp to a System.Int32.</summary>
    public static explicit operator byte[](Timestamp val) => val.ToByteArray();

    /// <summary>Casts an System.Int32 to a timestamp.</summary>
    public static explicit operator Timestamp(byte[] val) => Create(val);

    /// <summary>Casts a timestamp to a System.Int64.</summary>
    public static explicit operator long(Timestamp val) => BitConverter.ToInt64(val.ToByteArray(), 0);

    /// <summary>Casts a System.Int64 to a timestamp.</summary>
    public static explicit operator Timestamp(long val) => Create(val);

    /// <summary>Casts a timestamp to a System.UInt64.</summary>
    [CLSCompliant(false)]
    public static explicit operator ulong(Timestamp val) => val.m_Value;

    /// <summary>Casts a System.UInt64 to a timestamp.</summary>
    [CLSCompliant(false)]
    public static implicit operator Timestamp(ulong val) => Create(val);

    /// <summary>Converts the string to a timestamp.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing a timestamp to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out Timestamp result)
    {
        result = default;
        if (s is not { Length: > 0 }) { return false; }
        if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase) &&
            ulong.TryParse(s.Substring(2), NumberStyles.HexNumber, formatProvider, out var val))
        {
            result = Create(val);
            return true;
        }
        else if (ulong.TryParse(s, NumberStyles.Number, formatProvider, out val))
        {
            result = new Timestamp(val);
            return true;
        }
        else return false;
    }

    /// <summary>Creates a timestamp from a Int64. </summary >
    /// <param name="val" >
    /// A decimal describing a timestamp.
    /// </param>
    [Pure]
    [CLSCompliant(false)]
    public static Timestamp Create(ulong val) => new(val);

    /// <summary>Creates a timestamp from a Int64. </summary >
    /// <param name="val" >
    /// A decimal describing a timestamp.
    /// </param>
    [Pure]
    public static Timestamp Create(long val) => Create(BitConverter.GetBytes(val));

    /// <summary>Creates a timestamp from a Int64. </summary >
    /// <param name="bytes" >
    /// A byte array describing a timestamp.
    /// </param>
    [Pure]
    public static Timestamp Create(byte[] bytes)
    {
        Guard.HasAny(bytes, nameof(bytes));
        if (bytes.Length == 8) return Create(BitConverter.ToUInt64(bytes, 0));
        else throw new ArgumentException(QowaivMessages.ArgumentException_TimestampArrayShouldHaveSize8, nameof(bytes));
    }
}
