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

public partial struct EmailAddress
{
    private EmailAddress(string? value) => m_Value = value;

    /// <summary>The inner value of the email address.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? m_Value;
}

public partial struct EmailAddress : IEmpty<EmailAddress>
{
    /// <summary>Represents an empty/not set email address.</summary>
    public static EmailAddress Empty => default;

    /// <summary>False if the email address is empty, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool HasValue => m_Value != default;

    /// <summary>Returns true if the email address is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => !HasValue;
}

public partial struct EmailAddress : IUnknown<EmailAddress>
{
    /// <summary>False if the email address is empty or unknown, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsKnown => m_Value != default && m_Value != Unknown.m_Value;

    /// <summary>Returns true if the email address is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == Unknown.m_Value;

    /// <summary>Returns true if the email address is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
}

public partial struct EmailAddress : IEquatable<EmailAddress>
#if NET8_0_OR_GREATER
    , IEqualityOperators<EmailAddress, EmailAddress, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is EmailAddress other && Equals(other);

    /// <summary>Returns true if this instance and the other email address are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="EmailAddress" /> to compare with.</param>
    [Pure]
    public bool Equals(EmailAddress other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(EmailAddress left, EmailAddress right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(EmailAddress left, EmailAddress right) => !(left == right);
}

public partial struct EmailAddress : IComparable, IComparable<EmailAddress>
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is EmailAddress other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <inheritdoc />
    [Pure]
#nullable disable
    public int CompareTo(EmailAddress other) => Comparer<string>.Default.Compare(m_Value, other.m_Value);
#nullable enable
}

public partial struct EmailAddress : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the email address.</summary>
    [Pure]
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the email address.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the email address.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

#if NET8_0_OR_GREATER
#else
public partial struct EmailAddress : ISerializable
{
    /// <summary>Initializes a new instance of the email address based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private EmailAddress(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        m_Value = info.GetValue("Value", typeof(string)) is string val ? val : default(string);
    }

    /// <summary>Adds the underlying property of the email address to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info).AddValue("Value", m_Value);
}
#endif

public partial struct EmailAddress
{
    /// <summary>Creates the email address from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized email address.
    /// </returns>
    [Pure]
    public static EmailAddress FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct EmailAddress : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the email address.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the email address from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the email address to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());
}

public partial struct EmailAddress
#if NET8_0_OR_GREATER
    : IParsable<EmailAddress>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="EmailAddress"/>.</summary>
    /// <param name="s">
    /// A string containing the email address to convert.
    /// </param>
    /// <returns>
    /// The parsed email address.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static EmailAddress Parse(string? s)
        => TryParse(s, null, out var svo)
            ? svo
            : throw Unparsable.ForValue<EmailAddress>(s, QowaivMessages.FormatExceptionEmailAddress);

    /// <summary>Converts the <see cref="string"/> to <see cref="EmailAddress"/>.</summary>
    /// <param name="s">
    /// A string containing the email address to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed email address.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static EmailAddress Parse(string? s, IFormatProvider? provider)
        => TryParse(s, provider, out var svo)
            ? svo
            : throw Unparsable.ForValue<EmailAddress>(s, QowaivMessages.FormatExceptionEmailAddress);

    /// <summary>Converts the <see cref="string"/> to <see cref="EmailAddress"/>.</summary>
    /// <param name="s">
    /// A string containing the email address to convert.
    /// </param>
    /// <returns>
    /// The email address if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static EmailAddress? TryParse(string? s)
        => TryParse(s, null, out var val)
            ? val
            : default(EmailAddress?);

    /// <summary>Converts the <see cref="string"/> to <see cref="EmailAddress"/>.</summary>
    /// <param name="s">
    /// A string containing the email address to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The email address if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static EmailAddress? TryParse(string? s, IFormatProvider? provider)
        => TryParse(s, provider, out var val)
            ? val
            : default(EmailAddress?);

    /// <summary>Converts the <see cref="string"/> to <see cref="EmailAddress"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the email address to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Impure]
    public static bool TryParse(string? s, out EmailAddress result) => TryParse(s, null, out result);
}
