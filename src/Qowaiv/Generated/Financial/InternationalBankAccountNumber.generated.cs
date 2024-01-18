﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#nullable enable

namespace Qowaiv.Financial;

public partial struct InternationalBankAccountNumber
{
    private InternationalBankAccountNumber(string? value) => m_Value = value;

    /// <summary>The inner value of the IBAN.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? m_Value;

    /// <summary>False if the IBAN is empty or unknown, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool IsKnown => m_Value != default && m_Value != Unknown.m_Value;

    /// <summary>Returns true if the IBAN is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == Unknown.m_Value;

    /// <summary>Returns true if the IBAN is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
}

public partial struct InternationalBankAccountNumber : IEmpty<InternationalBankAccountNumber>
{
    /// <summary>Represents an empty/not set IBAN.</summary>
    public static InternationalBankAccountNumber Empty => default;

    /// <summary>False if the IBAN is empty, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool HasValue => m_Value != default;

    /// <summary>Returns true if the IBAN is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => !HasValue;
}

public partial struct InternationalBankAccountNumber : IEquatable<InternationalBankAccountNumber>
#if NET8_0_OR_GREATER
    , IEqualityOperators<InternationalBankAccountNumber, InternationalBankAccountNumber, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is InternationalBankAccountNumber other && Equals(other);

    /// <summary>Returns true if this instance and the other IBAN are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="InternationalBankAccountNumber" /> to compare with.</param>
    [Pure]
    public bool Equals(InternationalBankAccountNumber other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(InternationalBankAccountNumber left, InternationalBankAccountNumber right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(InternationalBankAccountNumber left, InternationalBankAccountNumber right) => !(left == right);
}

public partial struct InternationalBankAccountNumber : IComparable, IComparable<InternationalBankAccountNumber>
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is InternationalBankAccountNumber other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <inheritdoc />
    [Pure]
#nullable disable
    public int CompareTo(InternationalBankAccountNumber other) => Comparer<string>.Default.Compare(m_Value, other.m_Value);
#nullable enable
}

public partial struct InternationalBankAccountNumber : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the IBAN.</summary>
    [Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the IBAN.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the IBAN.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

#if NET8_0_OR_GREATER
#else
public partial struct InternationalBankAccountNumber : ISerializable
{
    /// <summary>Initializes a new instance of the IBAN based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private InternationalBankAccountNumber(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        m_Value = info.GetValue("Value", typeof(string)) is string val ? val : default(string);
    }

    /// <summary>Adds the underlying property of the IBAN to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info).AddValue("Value", m_Value);
}
#endif

public partial struct InternationalBankAccountNumber
{
    /// <summary>Creates the IBAN from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized IBAN.
    /// </returns>
    [Pure]
    public static InternationalBankAccountNumber FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct InternationalBankAccountNumber : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the IBAN.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the IBAN from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the IBAN to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());
}

public partial struct InternationalBankAccountNumber
#if NET8_0_OR_GREATER
    : IParsable<InternationalBankAccountNumber>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="InternationalBankAccountNumber"/>.</summary>
    /// <param name="s">
    /// A string containing the IBAN to convert.
    /// </param>
    /// <returns>
    /// The parsed IBAN.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static InternationalBankAccountNumber Parse(string? s)
        => TryParse(s, null, out var svo)
            ? svo
            : throw Unparsable.ForValue<InternationalBankAccountNumber>(s, QowaivMessages.FormatExceptionInternationalBankAccountNumber);

    /// <summary>Converts the <see cref="string"/> to <see cref="InternationalBankAccountNumber"/>.</summary>
    /// <param name="s">
    /// A string containing the IBAN to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed IBAN.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static InternationalBankAccountNumber Parse(string? s, IFormatProvider? provider)
        => TryParse(s, provider, out var svo)
            ? svo
            : throw Unparsable.ForValue<InternationalBankAccountNumber>(s, QowaivMessages.FormatExceptionInternationalBankAccountNumber);

    /// <summary>Converts the <see cref="string"/> to <see cref="InternationalBankAccountNumber"/>.</summary>
    /// <param name="s">
    /// A string containing the IBAN to convert.
    /// </param>
    /// <returns>
    /// The IBAN if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static InternationalBankAccountNumber? TryParse(string? s)
        => TryParse(s, null, out var val)
            ? val
            : default(InternationalBankAccountNumber?);

    /// <summary>Converts the <see cref="string"/> to <see cref="InternationalBankAccountNumber"/>.</summary>
    /// <param name="s">
    /// A string containing the IBAN to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The IBAN if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static InternationalBankAccountNumber? TryParse(string? s, IFormatProvider? provider) 
        => TryParse(s, provider, out var val) 
            ? val 
            : default(InternationalBankAccountNumber?);

    /// <summary>Converts the <see cref="string"/> to <see cref="InternationalBankAccountNumber"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the IBAN to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Impure]
    public static bool TryParse(string? s, out InternationalBankAccountNumber result) => TryParse(s, null, out result);
}
