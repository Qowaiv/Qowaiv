﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#nullable enable

namespace Qowaiv;

public partial struct YesNo
{
    private YesNo(byte value) => m_Value = value;

    /// <summary>The inner value of the yes-no.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly byte m_Value;

    /// <summary>False if the yes-no is empty, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool HasValue => m_Value != default;

    /// <summary>False if the yes-no is empty or unknown, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsKnown => m_Value != default && m_Value != Unknown.m_Value;

    /// <summary>Returns true if the yes-no is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => m_Value == default;

    /// <summary>Returns true if the yes-no is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == Unknown.m_Value;

    /// <summary>Returns true if the yes-no is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
}

public partial struct YesNo : IEquatable<YesNo>
#if NET8_0_OR_GREATER
    , IEqualityOperators<YesNo, YesNo, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is YesNo other && Equals(other);

    /// <summary>Returns true if this instance and the other yes-no are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="YesNo" /> to compare with.</param>
    [Pure]
    public bool Equals(YesNo other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(YesNo left, YesNo right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(YesNo left, YesNo right) => !(left == right);
}

public partial struct YesNo : IComparable, IComparable<YesNo>
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is YesNo other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <inheritdoc />
    [Pure]
#nullable disable
    public int CompareTo(YesNo other) => Comparer<byte>.Default.Compare(m_Value, other.m_Value);
#nullable enable
}

public partial struct YesNo : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the yes-no.</summary>
    [Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the yes-no.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the yes-no.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

#if NET8_0_OR_GREATER
#else
public partial struct YesNo : ISerializable
{
    /// <summary>Initializes a new instance of the yes-no based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private YesNo(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        m_Value = info.GetValue("Value", typeof(byte)) is byte val ? val : default(byte);
    }

    /// <summary>Adds the underlying property of the yes-no to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info).AddValue("Value", m_Value);
}
#endif

public partial struct YesNo
{
    /// <summary>Creates the yes-no from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized yes-no.
    /// </returns>
    [Pure]
    public static YesNo FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct YesNo : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the yes-no.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the yes-no from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the yes-no to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());
}

public partial struct YesNo
#if NET7_0_OR_GREATER
    : IParsable<YesNo>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="YesNo"/>.</summary>
    /// <param name="s">
    /// A string containing the yes-no to convert.
    /// </param>
    /// <returns>
    /// The parsed yes-no.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static YesNo Parse(string? s) => Parse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="YesNo"/>.</summary>
    /// <param name="s">
    /// A string containing the yes-no to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed yes-no.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static YesNo Parse(string? s, IFormatProvider? provider) 
        => TryParse(s, provider) 
        ?? throw Unparsable.ForValue<YesNo>(s, QowaivMessages.FormatExceptionYesNo);

    /// <summary>Converts the <see cref="string"/> to <see cref="YesNo"/>.</summary>
    /// <param name="s">
    /// A string containing the yes-no to convert.
    /// </param>
    /// <returns>
    /// The yes-no if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static YesNo? TryParse(string? s) => TryParse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="YesNo"/>.</summary>
    /// <param name="s">
    /// A string containing the yes-no to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The yes-no if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static YesNo? TryParse(string? s, IFormatProvider? provider) => TryParse(s, provider, out var val) ? val : default(YesNo?);

    /// <summary>Converts the <see cref="string"/> to <see cref="YesNo"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the yes-no to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Impure]
    public static bool TryParse(string? s, out YesNo result) => TryParse(s, null, out result);
}
