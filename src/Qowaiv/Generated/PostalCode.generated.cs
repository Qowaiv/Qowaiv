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

public partial struct PostalCode
{
    private PostalCode(string? value) => m_Value = value;

    /// <summary>The inner value of the postal code.</summary>
    private readonly string? m_Value;

    /// <summary>Returns true if the postal code is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => m_Value == default;
    /// <summary>Returns true if the postal code is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == Unknown.m_Value;
    /// <summary>Returns true if the postal code is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
}

public partial struct PostalCode : IEquatable<PostalCode>
#if NET7_0_OR_GREATER
    , IEqualityOperators<PostalCode, PostalCode, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is PostalCode other && Equals(other);

    /// <summary>Returns true if this instance and the other postal code are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="PostalCode" /> to compare with.</param>
    [Pure]
    public bool Equals(PostalCode other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(PostalCode left, PostalCode right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(PostalCode left, PostalCode right) => !(left == right);
}

public partial struct PostalCode : IComparable, IComparable<PostalCode>
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is PostalCode other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <inheritdoc />
    [Pure]
#nullable disable
    public int CompareTo(PostalCode other) => Comparer<string>.Default.Compare(m_Value, other.m_Value);
#nullable enable
}

public partial struct PostalCode : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the postal code.</summary>
    [Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the postal code.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the postal code.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

public partial struct PostalCode : ISerializable
{
    /// <summary>Initializes a new instance of the postal code based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private PostalCode(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info, nameof(info));
        m_Value = info.GetValue("Value", typeof(string)) is string val ? val : default(string);
    }

    /// <summary>Adds the underlying property of the postal code to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info, nameof(info)).AddValue("Value", m_Value);
}

public partial struct PostalCode
{
    /// <summary>Creates the postal code from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized postal code.
    /// </returns>
    [Pure]
    public static PostalCode FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct PostalCode : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the postal code.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the postal code from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader, nameof(reader));
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the postal code to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer, nameof(writer)).WriteString(ToXmlString());
}

public partial struct PostalCode
#if NET7_0_OR_GREATER
    : IParsable<PostalCode>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="PostalCode"/>.</summary>
    /// <param name="s">
    /// A string containing the postal code to convert.
    /// </param>
    /// <returns>
    /// The parsed postal code.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static PostalCode Parse(string? s) => Parse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="PostalCode"/>.</summary>
    /// <param name="s">
    /// A string containing the postal code to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed postal code.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static PostalCode Parse(string? s, IFormatProvider? formatProvider) => TryParse(s, formatProvider) ?? throw new FormatException(QowaivMessages.FormatExceptionPostalCode);

    /// <summary>Converts the <see cref="string"/> to <see cref="PostalCode"/>.</summary>
    /// <param name="s">
    /// A string containing the postal code to convert.
    /// </param>
    /// <returns>
    /// The postal code if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static PostalCode? TryParse(string? s) => TryParse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="PostalCode"/>.</summary>
    /// <param name="s">
    /// A string containing the postal code to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The postal code if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static PostalCode? TryParse(string? s, IFormatProvider? formatProvider) => TryParse(s, formatProvider, out var val) ? val : default(PostalCode?);

    /// <summary>Converts the <see cref="string"/> to <see cref="PostalCode"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the postal code to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Pure]
    public static bool TryParse(string? s, out PostalCode result) => TryParse(s, null, out result);
}

public partial struct PostalCode
{
    /// <summary>Returns true if the value represents a valid postal code.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    [Obsolete("Use the SVO itself instead. Will be dropped when the next major version is released.")]
    public static bool IsValid(string? val) => IsValid(val, (IFormatProvider?)null);

    /// <summary>Returns true if the value represents a valid postal code.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The <see cref="IFormatProvider"/> to interpret the <see cref="string"/> value with.
    /// </param>
    [Pure]
    [Obsolete("Use the SVO itself instead. Will be dropped when the next major version is released.")]
    public static bool IsValid(string? val, IFormatProvider? formatProvider)
        => !string.IsNullOrWhiteSpace(val)
        && TryParse(val, formatProvider, out _);
}
