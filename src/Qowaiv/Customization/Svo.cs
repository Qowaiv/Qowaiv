#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

using Qowaiv.Conversion.Customization;

namespace Qowaiv.Customization;

/// <summary>Represents a Single Value Object.</summary>
/// <typeparam name="TSvoBehavior">
/// The type that implements the SVO behavior.
/// </typeparam>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(string))]
[OpenApiDataType(description: "Single Value Object", type: "Svo", format: "Svo", example: "ABC")]
[TypeConverter(typeof(SvoTypeConverter))]
#if NET5_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Customization.GenericSvoJsonConverter))]
#endif
public readonly partial struct Svo<TSvoBehavior> : ISerializable, IXmlSerializable, IFormattable, IEquatable<Svo<TSvoBehavior>>, IComparable, IComparable<Svo<TSvoBehavior>>
    where TSvoBehavior : SvoBehavior, new()
{
    /// <summary>An singleton instance that deals with the identifier specific behavior.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly TSvoBehavior behavior = new();

    /// <summary>Represents an empty/not set Single Value Object.</summary>
    public static readonly Svo<TSvoBehavior> Empty;

    /// <summary>Represents an unknown (but set) Single Value Object.</summary>
    public static readonly Svo<TSvoBehavior> Unknown = new(SvoBehavior.unknown);

    /// <summary>Initializes a new instance of the <see cref="Svo{TSvoBehavior}"/> struct.</summary>
    private Svo(string? value) => m_Value = value;

    /// <summary>Initializes a new instance of the <see cref="Svo{TSvoBehavior}"/> struct.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private Svo(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        m_Value = Parse(info.GetString("Value")).m_Value;
    }

    /// <summary>Adds the underlying property of the Single Value Object to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info).AddValue("Value", m_Value);

    /// <summary>The inner value of the Single Value Object.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? m_Value;

    /// <summary>Returns true if the Single Value Object is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => m_Value == default;

    /// <summary>Returns true if the Single Value Object is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == SvoBehavior.unknown;

    /// <summary>Returns true if the Single Value Object is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();

    /// <summary>Gets the number of characters of Single Value Object.</summary>
    public int Length => IsEmptyOrUnknown() ? 0 : behavior.Length(m_Value!);

    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        else if (obj is Svo<TSvoBehavior> other) return CompareTo(other);
        else throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj));
    }

    /// <inheritdoc />
    [Pure]
#if NET5_0_OR_GREATER
    public int CompareTo(Svo<TSvoBehavior> other) => behavior.Compare(m_Value, other.m_Value);
#else
    public int CompareTo(Svo<TSvoBehavior> other)
    {
        // Comparing with char.max value does not work as expected in older versions of .NET
        if (IsUnknown() || other.IsUnknown())
        {
            if (IsUnknown())
            {
                return other.IsUnknown() ? 0 : +1;
            }
            else return -1;
        }
        else return behavior.Compare(m_Value, other.m_Value);
    }
#endif

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is Svo<TSvoBehavior> other && Equals(other);

    /// <summary>Returns true if this instance and the other Single Value Object are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="Svo{TSvoBehavior}" /> to compare with.</param>
    [Pure]
    public bool Equals(Svo<TSvoBehavior> other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    public static bool operator ==(Svo<TSvoBehavior> left, Svo<TSvoBehavior> right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    public static bool operator !=(Svo<TSvoBehavior> left, Svo<TSvoBehavior> right) => !(left == right);

    /// <summary>Returns a <see cref="string" /> that represents the Single Value Object for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Returns a <see cref="string"/> that represents the Single Value Object.</summary>
    [Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the Single Value Object.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the Single Value Object.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);

    /// <summary>Returns a formatted <see cref="string" /> that represents the Single Value Object.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => behavior.ToString(m_Value, format, formatProvider);

    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the Single Value Object.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => null;

    /// <summary>Reads the Single Value Object from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the Single Value Object to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());

    /// <summary>Gets an XML string representation of the Single Value Object.</summary>
    [Pure]
    private string? ToXmlString() => behavior.ToXml(m_Value);

    /// <summary>Creates the Single Value Object from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized Single Value Object.
    /// </returns>
    [Pure]
    public static Svo<TSvoBehavior> FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);

    /// <summary>Serializes the Single Value Object to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => behavior.ToJson(m_Value);

    /// <summary>Casts the Single Value Object to a <see cref="string"/>.</summary>
    public static explicit operator string(Svo<TSvoBehavior> val) => val.ToString(CultureInfo.CurrentCulture);

    /// <summary>Casts a <see cref="string"/> to a Single Value Object.</summary>
    public static explicit operator Svo<TSvoBehavior>(string str) => Parse(str, CultureInfo.CurrentCulture);

    /// <summary>Returns true if the value represents a valid month.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    public static bool IsValid(string? val) => IsValid(val, formatProvider: null);

    /// <summary>Returns true if the value represents a valid month.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The <see cref="IFormatProvider"/> to interpret the <see cref="string"/> value with.
    /// </param>
    [Pure]
    public static bool IsValid(string? val, IFormatProvider? formatProvider)
        => !string.IsNullOrWhiteSpace(val)
        && !behavior.IsUnknown(val!, formatProvider)
        && behavior.TryParse(val, formatProvider, out _);

    /// <summary>Converts the <see cref="string"/> to <see cref="Svo{TSvoBehavior}"/>.</summary>
    /// <param name="s">
    /// A string containing the Single Value Object to convert.
    /// </param>
    /// <returns>
    /// The parsed Single Value Object.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Svo<TSvoBehavior> Parse(string? s) => Parse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="Svo{TSvoBehavior}"/>.</summary>
    /// <param name="s">
    /// A string containing the Single Value Object to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed Single Value Object.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Svo<TSvoBehavior> Parse(string? s, IFormatProvider? formatProvider)
        => TryParse(s, formatProvider)
        ?? throw behavior.InvalidFormat(s, formatProvider);

    /// <summary>Converts the <see cref="string"/> to <see cref="Svo{TSvoBehavior}"/>.</summary>
    /// <param name="s">
    /// A string containing the Single Value Object to convert.
    /// </param>
    /// <returns>
    /// The Single Value Object if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Svo<TSvoBehavior>? TryParse(string? s) => TryParse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="Svo{TSvoBehavior}"/>.</summary>
    /// <param name="s">
    /// A string containing the Single Value Object to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The Single Value Object if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Svo<TSvoBehavior>? TryParse(string? s, IFormatProvider? formatProvider)
        => TryParse(s, formatProvider, out var val)
        ? val
        : default(Svo<TSvoBehavior>?);

    /// <summary>Converts the <see cref="string"/> to <see cref="Svo{TSvoBehavior}"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the Single Value Object to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Pure]
    public static bool TryParse(string? s, out Svo<TSvoBehavior> result)
        => TryParse(s, null, out result);

    /// <summary>Converts the <see cref="string"/> to <see cref="Svo{TSvoBehavior}"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="str">
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
    public static bool TryParse(string? str, IFormatProvider? formatProvider, out Svo<TSvoBehavior> result)
    {
        var success = behavior.TryParse(str, formatProvider, out var parsed);
        result = new(parsed);
        return success;
    }
}
