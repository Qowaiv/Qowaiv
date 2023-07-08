﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#nullable enable

namespace Qowaiv.Chemistry;

public partial struct CasRegistryNumber
{
    private CasRegistryNumber(long value) => m_Value = value;

    /// <summary>The inner value of the CAS Registry Number.</summary>
    private readonly long m_Value;

    /// <summary>Returns true if the CAS Registry Number is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => m_Value == default;
    /// <summary>Returns true if the CAS Registry Number is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == Unknown.m_Value;
    /// <summary>Returns true if the CAS Registry Number is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
}

public partial struct CasRegistryNumber : IEquatable<CasRegistryNumber>
#if NET7_0_OR_GREATER
    , IEqualityOperators<CasRegistryNumber, CasRegistryNumber, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is CasRegistryNumber other && Equals(other);

    /// <summary>Returns true if this instance and the other CAS Registry Number are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="CasRegistryNumber" /> to compare with.</param>
    [Pure]
    public bool Equals(CasRegistryNumber other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(CasRegistryNumber left, CasRegistryNumber right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(CasRegistryNumber left, CasRegistryNumber right) => !(left == right);
}

public partial struct CasRegistryNumber : IComparable, IComparable<CasRegistryNumber>
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is CasRegistryNumber other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <inheritdoc />
    [Pure]
#nullable disable
    public int CompareTo(CasRegistryNumber other) => Comparer<long>.Default.Compare(m_Value, other.m_Value);
#nullable enable
}

public partial struct CasRegistryNumber : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the CAS Registry Number.</summary>
    [Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the CAS Registry Number.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the CAS Registry Number.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

public partial struct CasRegistryNumber : ISerializable
{
    /// <summary>Initializes a new instance of the CAS Registry Number based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private CasRegistryNumber(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        m_Value = info.GetValue("Value", typeof(long)) is long val ? val : default(long);
    }

    /// <summary>Adds the underlying property of the CAS Registry Number to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info).AddValue("Value", m_Value);
}

public partial struct CasRegistryNumber
{
    /// <summary>Creates the CAS Registry Number from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized CAS Registry Number.
    /// </returns>
    [Pure]
    public static CasRegistryNumber FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct CasRegistryNumber : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the CAS Registry Number.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the CAS Registry Number from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the CAS Registry Number to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());
}

public partial struct CasRegistryNumber
#if NET7_0_OR_GREATER
    : IParsable<CasRegistryNumber>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="CasRegistryNumber"/>.</summary>
    /// <param name="s">
    /// A string containing the CAS Registry Number to convert.
    /// </param>
    /// <returns>
    /// The parsed CAS Registry Number.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static CasRegistryNumber Parse(string? s) => Parse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="CasRegistryNumber"/>.</summary>
    /// <param name="s">
    /// A string containing the CAS Registry Number to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed CAS Registry Number.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static CasRegistryNumber Parse(string? s, IFormatProvider? formatProvider) => TryParse(s, formatProvider) ?? throw new FormatException(QowaivMessages.FormatExceptionCasRegistryNumber);

    /// <summary>Converts the <see cref="string"/> to <see cref="CasRegistryNumber"/>.</summary>
    /// <param name="s">
    /// A string containing the CAS Registry Number to convert.
    /// </param>
    /// <returns>
    /// The CAS Registry Number if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static CasRegistryNumber? TryParse(string? s) => TryParse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="CasRegistryNumber"/>.</summary>
    /// <param name="s">
    /// A string containing the CAS Registry Number to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The CAS Registry Number if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static CasRegistryNumber? TryParse(string? s, IFormatProvider? formatProvider) => TryParse(s, formatProvider, out var val) ? val : default(CasRegistryNumber?);

    /// <summary>Converts the <see cref="string"/> to <see cref="CasRegistryNumber"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the CAS Registry Number to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Impure]
    public static bool TryParse(string? s, out CasRegistryNumber result) => TryParse(s, null, out result);
}

public partial struct CasRegistryNumber
{
    /// <summary>Returns true if the value represents a valid CAS Registry Number.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use CasRegistryNumber.TryParse(str) is { } instead. Will be dropped when the next major version is released.")]
    public static bool IsValid(string? val) => IsValid(val, (IFormatProvider?)null);

    /// <summary>Returns true if the value represents a valid CAS Registry Number.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The <see cref="IFormatProvider"/> to interpret the <see cref="string"/> value with.
    /// </param>
    [Pure]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use CasRegistryNumber.TryParse(str, formatProvider) is { } instead. Will be dropped when the next major version is released.")]
    public static bool IsValid(string? val, IFormatProvider? formatProvider)
        => !string.IsNullOrWhiteSpace(val)
        && TryParse(val, formatProvider, out _);
}
