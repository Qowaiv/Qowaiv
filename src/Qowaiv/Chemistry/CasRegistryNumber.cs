#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

namespace Qowaiv.Chemistry;

/// <summary>Represents a CAS Registry Number.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(long))]
[OpenApiDataType(description: "CAS Registry Number", type: "CasRegistryNumber", format: "CasRegistryNumber", example: "ABC")]
[TypeConverter(typeof(Conversion.Chemistry.CasRegistryNumberTypeConverter))]
public readonly partial struct CasRegistryNumber : ISerializable, IXmlSerializable, IFormattable, IEquatable<CasRegistryNumber>, IComparable, IComparable<CasRegistryNumber>
{
    /// <summary>Represents an empty/not set CAS Registry Number.</summary>
    public static readonly CasRegistryNumber Empty;

    /// <summary>Represents an unknown (but set) CAS Registry Number.</summary>
    public static readonly CasRegistryNumber Unknown = new(long.MaxValue);

    /// <summary>Gets the number of characters of CAS Registry Number.</summary>
    public int Length => IsEmptyOrUnknown() ? 0 : (int)Math.Ceiling(Math.Log10(m_Value));

    /// <summary>Returns a <see cref="string" /> that represents the CAS Registry Number for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the CAS Registry Number.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
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
        return format.WithDefault("f") == "f"
            ? m_Value.ToString(@"#00\-00\-0", formatProvider)
            : m_Value.ToString(format, formatProvider);
    }

    /// <summary>Gets an XML string representation of the CAS Registry Number.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the CAS Registry Number to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Deserializes the CAS Registry Number from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized CAS Registry Number.
    /// </returns>
    [Pure]
    public static CasRegistryNumber FromJson(long json) => Create(json);

    /// <summary>Casts the CAS Registry Number to a <see cref="string"/>.</summary>
    public static explicit operator string(CasRegistryNumber val) => val.ToString(CultureInfo.CurrentCulture);

    /// <summary>Casts a <see cref="string"/> to a CAS Registry Number.</summary>
    public static explicit operator CasRegistryNumber(string str) => Parse(str, CultureInfo.CurrentCulture);

    /// <summary>Casts the CAS Registry Number to a <see cref="long"/>.</summary>
    public static explicit operator long(CasRegistryNumber val) => val.m_Value;

    /// <summary>Casts a <see cref="long"/> to a CAS Registry Number.</summary>
    public static explicit operator CasRegistryNumber(long val) => Create(val);

    /// <summary>Converts the <see cref="string"/> to <see cref="CasRegistryNumber"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the CAS Registry Number to convert.
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
    [Pure]
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out CasRegistryNumber result)
    {
        result = default;
        var str = s.Unify();
        if (string.IsNullOrEmpty(str))
        {
            return true;
        }
        else if (Qowaiv.Unknown.IsUnknown(str, formatProvider as CultureInfo))
        {
            result = Unknown;
            return true;
        }
        else return long.TryParse(str, out var number) && TryCreate(number, out result);
    }

    public static bool TryCreate(long? val, out CasRegistryNumber result)
    {
        result = default;
        if (val is null) return true;
        else if (val >= 1000)
        {
            var checksum = val.Value % 10;
            var buffer = val.Value / 10;
            var sum = 0L;
            var factor = 0;

            while (buffer > 0)
            {
                sum += (buffer % 10) * ++factor;
                buffer /= 10;
            }

            if (sum % 10 == checksum)
            {
                result = new(val.Value);
                return true;
            }
            else return false;
        }
        else return false;
    }

    [Pure]
    public static CasRegistryNumber Create(long? val)
        => TryCreate(val, out var result)
        ? result
        : throw new ArgumentOutOfRangeException(nameof(val), QowaivMessages.FormatExceptionCasRegistryNumber);
}

