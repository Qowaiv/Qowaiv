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

public partial struct Gender
{
    private Gender(byte value) => m_Value = value;

    /// <summary>The inner value of the gender.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly byte m_Value;

    /// <summary>False if the gender is empty, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool HasValue => m_Value != default;

    /// <summary>False if the gender is empty or unknown, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsKnown => m_Value != default && m_Value != Unknown.m_Value;

    /// <summary>Returns true if the gender is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => m_Value == default;

    /// <summary>Returns true if the gender is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == Unknown.m_Value;

    /// <summary>Returns true if the gender is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
}

public partial struct Gender : IEquatable<Gender>
#if NET8_0_OR_GREATER
    , IEqualityOperators<Gender, Gender, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Gender other && Equals(other);

    /// <summary>Returns true if this instance and the other gender are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="Gender" /> to compare with.</param>
    [Pure]
    public bool Equals(Gender other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(Gender left, Gender right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(Gender left, Gender right) => !(left == right);
}

public partial struct Gender : IComparable, IComparable<Gender>
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is Gender other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <inheritdoc />
    [Pure]
#nullable disable
    public int CompareTo(Gender other) => Comparer<byte>.Default.Compare(m_Value, other.m_Value);
#nullable enable
}

public partial struct Gender : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the gender.</summary>
    [Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the gender.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the gender.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

#if NET8_0_OR_GREATER
#else
public partial struct Gender : ISerializable
{
    /// <summary>Initializes a new instance of the gender based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private Gender(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        m_Value = info.GetValue("Value", typeof(byte)) is byte val ? val : default(byte);
    }

    /// <summary>Adds the underlying property of the gender to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info).AddValue("Value", m_Value);
}
#endif

public partial struct Gender
{
    /// <summary>Creates the gender from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized gender.
    /// </returns>
    [Pure]
    public static Gender FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct Gender : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the gender.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the gender from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the gender to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());
}

public partial struct Gender
#if NET8_0_OR_GREATER
    : IParsable<Gender>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="Gender"/>.</summary>
    /// <param name="s">
    /// A string containing the gender to convert.
    /// </param>
    /// <returns>
    /// The parsed gender.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Gender Parse(string? s) => Parse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="Gender"/>.</summary>
    /// <param name="s">
    /// A string containing the gender to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed gender.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Gender Parse(string? s, IFormatProvider? formatProvider) 
        => TryParse(s, formatProvider) 
        ?? throw Unparsable.ForValue<Gender>(s, QowaivMessages.FormatExceptionGender);

    /// <summary>Converts the <see cref="string"/> to <see cref="Gender"/>.</summary>
    /// <param name="s">
    /// A string containing the gender to convert.
    /// </param>
    /// <returns>
    /// The gender if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Gender? TryParse(string? s) => TryParse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="Gender"/>.</summary>
    /// <param name="s">
    /// A string containing the gender to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The gender if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Gender? TryParse(string? s, IFormatProvider? formatProvider) => TryParse(s, formatProvider, out var val) ? val : default(Gender?);

    /// <summary>Converts the <see cref="string"/> to <see cref="Gender"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the gender to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Impure]
    public static bool TryParse(string? s, out Gender result) => TryParse(s, null, out result);
}

public partial struct Gender
{
    /// <summary>Returns true if the value represents a valid gender.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use Gender.TryParse(str) is { } instead. Will be dropped when the next major version is released.")]
    public static bool IsValid(string? val) => IsValid(val, (IFormatProvider?)null);

    /// <summary>Returns true if the value represents a valid gender.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The <see cref="IFormatProvider"/> to interpret the <see cref="string"/> value with.
    /// </param>
    [Pure]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use Gender.TryParse(str, formatProvider) is { } instead. Will be dropped when the next major version is released.")]
    public static bool IsValid(string? val, IFormatProvider? formatProvider)
        => !string.IsNullOrWhiteSpace(val)
        && TryParse(val, formatProvider, out _);
}
