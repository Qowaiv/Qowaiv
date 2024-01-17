namespace Qowaiv;

/// <summary>Represents a house number.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(int))]
[OpenApiDataType(description: "House number notation.", example: "13", type: "string", format: "house-number", nullable: true)]
[TypeConverter(typeof(HouseNumberTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.HouseNumberJsonConverter))]
#endif
public readonly partial struct HouseNumber : IXmlSerializable, IFormattable, IEquatable<HouseNumber>, IComparable, IComparable<HouseNumber>
#if NET8_0_OR_GREATER
    , IMinMaxValue<HouseNumber>
#endif
{
    /// <summary>Represents an empty/not set house number.</summary>
    public static readonly HouseNumber Empty;

    /// <summary>Represents an unknown (but set) house number.</summary>
    public static readonly HouseNumber Unknown = new(int.MaxValue);

    /// <summary>Represents the smallest possible House number 1.</summary>
    public static HouseNumber MinValue => new(1);

    /// <summary>Represents the largest possible House number 999999999.</summary>
    public static HouseNumber MaxValue => new(999_999_999);

    /// <summary>Returns true if the house number is odd, otherwise false.</summary>
    /// <remarks>
    /// The empty and unknown value are not odd.
    /// </remarks>
    public bool IsOdd => !IsEmptyOrUnknown() && m_Value.Mod(2) == 1;

    /// <summary>Returns true if the House number is even, otherwise false.</summary>
    /// <remarks>
    /// The empty and unknown value are not even.
    /// </remarks>
    public bool IsEven => !IsEmptyOrUnknown() && m_Value.Mod(2) == 0;

    /// <summary>Gets the number of digits.</summary>
    public int Length
    {
        get
        {
            if (IsEmptyOrUnknown())
            {
                return 0;
            }
            var length = 0;
            var val = m_Value;
            while (val != 0)
            {
                val /= 10;
                length++;
            }
            return length;
        }
    }

    /// <summary>Deserializes the house number from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized house number.
    /// </returns>
    [Pure]
    public static HouseNumber FromJson(double json) => Create((int)json);

    /// <summary>Deserializes the house number from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized house number.
    /// </returns>
    [Pure]
    public static HouseNumber FromJson(long json) => Create((int)json);

    /// <summary>Serializes the house number to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Returns a <see cref="string"/> that represents the current house number for debug purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0}");

    /// <summary>Returns a formatted <see cref="string"/> that represents the current house number.</summary>
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
        else if (IsUnknown()) return "?";
        else if (IsEmpty()) return string.Empty;
        else return m_Value.ToString(format, formatProvider);
    }

    /// <summary>Gets an XML string representation of the house number.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Casts a house number to a System.Int32.</summary>
    public static explicit operator int(HouseNumber val) => val.m_Value;

    /// <summary>Casts an System.Int32 to a house number.</summary>
    public static implicit operator HouseNumber(int val) => Cast.Primitive<int, HouseNumber>(TryCreate, val);

    /// <summary>Casts a house number to a System.Int64.</summary>
    public static explicit operator long(HouseNumber val) => val.m_Value;

    /// <summary>Casts a System.Int64 to a house number.</summary>
    public static implicit operator HouseNumber(long val) => Cast.Primitive<int, HouseNumber>(TryCreate, (int)val);

    /// <summary>Converts the string to a house number.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing a house number to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out HouseNumber result)
    {
        result = default;
        if (string.IsNullOrEmpty(s))
        {
            return true;
        }
        var culture = provider as CultureInfo ?? CultureInfo.InvariantCulture;
        if (Qowaiv.Unknown.IsUnknown(s, culture))
        {
            result = Unknown;
            return true;
        }
        else if (int.TryParse(s, NumberStyles.None, culture, out var number)
            && number >= MinValue.m_Value
            && number <= MaxValue.m_Value)
        {
            result = new(number);
            return true;
        }
        else return false;
    }

    /// <summary>Creates a house number from a Int32. </summary >
    /// <param name="val" >
    /// A decimal describing a house number.
    /// </param >
    /// <exception cref="FormatException" >
    /// val is not a valid house number.
    /// </exception >
    [Pure]
    public static HouseNumber Create(int? val)
    {
        if (TryCreate(val, out HouseNumber result))
        {
            return result;
        }
        throw new ArgumentOutOfRangeException(nameof(val), QowaivMessages.FormatExceptionHouseNumber);
    }

    /// <summary>Creates a house number from a Int32.
    /// A return value indicates whether the conversion succeeded.
    /// </summary >
    /// <param name="val" >
    /// A decimal describing a house number.
    /// </param >
    /// <returns >
    /// A house number if the creation was successfully, otherwise Empty.
    /// </returns >
    [Pure]
    public static HouseNumber TryCreate(int? val)
    {
        if (TryCreate(val, out HouseNumber result))
        {
            return result;
        }
        return Empty;
    }

    /// <summary>Creates a house number from a Int32.
    /// A return value indicates whether the creation succeeded.
    /// </summary >
    /// <param name="val" >
    /// A Int32 describing a house number.
    /// </param >
    /// <param name="result" >
    /// The result of the creation.
    /// </param >
    /// <returns >
    /// True if a house number was created successfully, otherwise false.
    /// </returns >
    public static bool TryCreate(int? val, out HouseNumber result)
    {
        result = Empty;

        if (!val.HasValue)
        {
            return true;
        }
        else if (IsValid(val.Value))
        {
            result = new HouseNumber(val.Value);
            return true;
        }
        else return false;
    }

    /// <summary>Returns true if the val represents a valid house number, otherwise false.</summary>
    [Pure]
    private static bool IsValid(int? val)
        => val.HasValue
        && val >= MinValue.m_Value
        && val <= MaxValue.m_Value;
}
