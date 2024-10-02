#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
namespace Qowaiv.Chemistry;

/// <summary>
/// A CAS Registry Number, is a unique numerical identifier assigned by the
/// Chemical Abstracts Service (CAS), US to every chemical substance described
/// in the open scientific literature. It includes all substances described
/// from 1957 through the present, plus some substances from as far back as the
/// early 1800's.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(long))]
[OpenApiDataType(description: "CAS Registry Number", type: "string", format: "cas-nr", example: "7732-18-5", pattern: "[1-9][0-9]+\\-[0-9]{2}\\-[0-9]", nullable: true)]
[TypeConverter(typeof(Conversion.Chemistry.CasRegistryNumberTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Chemistry.CasRegistryNumberJsonConverter))]
#endif
public readonly partial struct CasRegistryNumber : IXmlSerializable, IFormattable, IEquatable<CasRegistryNumber>, IComparable, IComparable<CasRegistryNumber>
{
    /// <summary>Represents an unknown (but set) CAS Registry Number.</summary>
    public static CasRegistryNumber Unknown => new(long.MaxValue);

    /// <summary>Gets the number of characters of CAS Registry Number.</summary>
    public int Length => IsEmptyOrUnknown() ? 0 : (int)Math.Ceiling(Math.Log10(m_Value));

    /// <summary>Returns a <see cref="string" /> that represents the CAS Registry Number for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:f}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the CAS Registry Number.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    /// <remarks>
    /// The formats:
    /// f: as formatted.
    ///
    /// other (not empty) formats are applied on the number (long).
    /// </remarks>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
        {
            return formatted;
        }
        else if (IsEmpty()) return string.Empty;
        else if (IsUnknown()) return "?";
        else return format.WithDefault("f") == "f"
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

    /// <summary>Casts the CAS Registry Number to a <see cref="long" />.</summary>
    public static explicit operator long(CasRegistryNumber val) => val.m_Value;

    /// <summary>Casts a <see cref="long" /> to a CAS Registry Number.</summary>
    public static explicit operator CasRegistryNumber(long val) => Create(val);

    /// <summary>Converts the <see cref="string" /> to <see cref="CasRegistryNumber" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the CAS Registry Number to convert.
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
    [Pure]
    public static bool TryParse(string? s, IFormatProvider? provider, out CasRegistryNumber result)
    {
        result = default;
        var str = s.Unify();
        if (string.IsNullOrEmpty(str))
        {
            return true;
        }
        else if (Qowaiv.Unknown.IsUnknown(str, provider as CultureInfo))
        {
            result = Unknown;
            return true;
        }
        else return long.TryParse(str, out var number) && TryCreate(number, out result);
    }

    /// <summary>Creates a CAS Registry Number from a long.
    /// A return value indicates whether the creation succeeded.
    /// </summary>
    /// <param name="val">
    /// A long describing a CAS Registry Number.
    /// </param>
    /// <param name="result">
    /// The result of the creation.
    /// </param>
    /// <returns>
    /// True if a CAS Registry Number was created successfully, otherwise false.
    /// </returns>
    public static bool TryCreate(long val, out CasRegistryNumber result)
    {
        result = default;
        if (val == 0) return true;
        else if (val >= 10_000 && val <= 9_999_999_999)
        {
            var checksum = val % 10;
            var buffer = val / 10;
            var sum = 0L;
            var factor = 0;

            while (buffer > 0)
            {
                factor++;
                sum += (buffer % 10) * factor;
                buffer /= 10;
            }

            if (sum % 10 == checksum)
            {
                result = new(val);
                return true;
            }
            else return false;
        }
        else return false;
    }

    /// <summary>Creates a CAS Registry Number from a long.</summary>
    /// <param name="val">
    /// A decimal describing a CAS Registry Number.
    /// </param>
    /// <exception cref="FormatException">
    /// val is not a valid CAS Registry Number.
    /// </exception>
    [Pure]
    public static CasRegistryNumber Create(long val)
        => TryCreate(val, out var result)
        ? result
        : throw new ArgumentOutOfRangeException(nameof(val), QowaivMessages.FormatExceptionCasRegistryNumber);
}
