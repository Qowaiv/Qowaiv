#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

#pragma warning disable S2328
// "GetHashCode" should not reference mutable fields
// See README.md => Hashing

namespace Qowaiv;

/// <summary>Represents a Single Value Object.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
[OpenApiDataType(description: "Single Value Object", type: "Svo", format: "Svo", example: "ABC")]
[TypeConverter(typeof(Conversion.SvoConverter))]
public readonly partial struct Svo : ISerializable, IXmlSerializable, IFormattable, IEquatable<Svo>, IComparable, IComparable<Svo>
{
    /// <summary>Represents an empty/not set Single Value Object.</summary>
    public static readonly Svo Empty;

    /// <summary>Represents an unknown (but set) Single Value Object.</summary>
    public static readonly Svo Unknown = new Svo(default);

    /// <summary>Gets the number of characters of Single Value Object.</summary>
    public int Length => m_Value == null ? 0 : m_Value.Length;

    /// <summary>Returns a <see cref="string" /> that represents the Single Value Object for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => ToString("F", CultureInfo.InvariantCulture);

    /// <summary>Returns a formatted <see cref="string" /> that represents the Single Value Object.</summary>
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
        throw new NotImplementedException();
    }

    /// <summary>Gets an XML string representation of the Single Value Object.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the Single Value Object to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Casts the Single Value Object to a <see cref="string"/>.</summary>
    public static explicit operator string(Svo val) => val.ToString(CultureInfo.CurrentCulture);

    /// <summary>Casts a <see cref="string"/> to a Single Value Object.</summary>
    public static explicit operator Svo(string str) => Parse(str, CultureInfo.CurrentCulture);

    /// <summary>Converts the <see cref="string"/> to <see cref="Svo"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the Single Value Object to convert.
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
    public static bool TryParse(string? s, IFormatProvider? formatProvider, out Svo result)
    {
        result = default;
        if (string.IsNullOrEmpty(s))
        {
            return true;
        }
        if (Qowaiv.Unknown.IsUnknown(s, formatProvider as CultureInfo))
        {
            result = Unknown;
            return true;
        }
        throw new NotImplementedException();
    }
}

