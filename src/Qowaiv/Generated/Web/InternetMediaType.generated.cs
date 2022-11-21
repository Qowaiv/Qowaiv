﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#nullable enable

namespace Qowaiv.Web;

public partial struct InternetMediaType
{
    private InternetMediaType(string? value) => m_Value = value;

    /// <summary>The inner value of the Internet media type.</summary>
    private readonly string? m_Value;

    /// <summary>Returns true if the Internet media type is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => m_Value == default;
    /// <summary>Returns true if the Internet media type is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == Unknown.m_Value;
    /// <summary>Returns true if the Internet media type is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
}

public partial struct InternetMediaType : IEquatable<InternetMediaType>
#if NET7_0_OR_GREATER
    , IEqualityOperators<InternetMediaType, InternetMediaType, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is InternetMediaType other && Equals(other);

    /// <summary>Returns true if this instance and the other Internet media type are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="InternetMediaType" /> to compare with.</param>
    [Pure]
    public bool Equals(InternetMediaType other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(InternetMediaType left, InternetMediaType right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(InternetMediaType left, InternetMediaType right) => !(left == right);
}

public partial struct InternetMediaType : IComparable, IComparable<InternetMediaType>
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is InternetMediaType other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <inheritdoc />
    [Pure]
#nullable disable
    public int CompareTo(InternetMediaType other) => Comparer<string>.Default.Compare(m_Value, other.m_Value);
#nullable enable
}

public partial struct InternetMediaType : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the Internet media type.</summary>
    [Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the Internet media type.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the Internet media type.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

public partial struct InternetMediaType : ISerializable
{
    /// <summary>Initializes a new instance of the Internet media type based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private InternetMediaType(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info, nameof(info));
        m_Value = info.GetValue("Value", typeof(string)) is string val ? val : default(string);
    }

    /// <summary>Adds the underlying property of the Internet media type to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info, nameof(info)).AddValue("Value", m_Value);
}

public partial struct InternetMediaType
{
    /// <summary>Creates the Internet media type from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized Internet media type.
    /// </returns>
    [Pure]
    public static InternetMediaType FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct InternetMediaType : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the Internet media type.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the Internet media type from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader, nameof(reader));
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the Internet media type to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer, nameof(writer)).WriteString(ToXmlString());
}

public partial struct InternetMediaType
#if NET7_0_OR_GREATER
    : IParsable<InternetMediaType>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="InternetMediaType"/>.</summary>
    /// <param name="s">
    /// A string containing the Internet media type to convert.
    /// </param>
    /// <returns>
    /// The parsed Internet media type.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static InternetMediaType Parse(string? s) => Parse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="InternetMediaType"/>.</summary>
    /// <param name="s">
    /// A string containing the Internet media type to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed Internet media type.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static InternetMediaType Parse(string? s, IFormatProvider? provider) => TryParse(s, provider) ?? throw new FormatException(QowaivMessages.FormatExceptionInternetMediaType);

    /// <summary>Converts the <see cref="string"/> to <see cref="InternetMediaType"/>.</summary>
    /// <param name="s">
    /// A string containing the Internet media type to convert.
    /// </param>
    /// <returns>
    /// The Internet media type if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static InternetMediaType? TryParse(string? s) => TryParse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="InternetMediaType"/>.</summary>
    /// <param name="s">
    /// A string containing the Internet media type to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The Internet media type if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static InternetMediaType? TryParse(string? s, IFormatProvider? provider) => TryParse(s, provider, out var val) ? val : default(InternetMediaType?);

    /// <summary>Converts the <see cref="string"/> to <see cref="InternetMediaType"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the Internet media type to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Pure]
    public static bool TryParse(string? s, out InternetMediaType result) => TryParse(s, null, out result);
}

public partial struct InternetMediaType
{
    /// <summary>Returns true if the value represents a valid Internet media type.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    public static bool IsValid(string? val) => IsValid(val, (IFormatProvider?)null);

    /// <summary>Returns true if the value represents a valid Internet media type.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    /// <param name="provider">
    /// The <see cref="IFormatProvider"/> to interpret the <see cref="string"/> value with.
    /// </param>
    [Pure]
    public static bool IsValid(string? val, IFormatProvider? provider)
        => !string.IsNullOrWhiteSpace(val)
        && TryParse(val, provider, out _);
}
