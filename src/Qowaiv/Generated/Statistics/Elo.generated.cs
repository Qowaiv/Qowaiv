﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#nullable enable

namespace Qowaiv.Statistics;

public partial struct Elo
{
    private Elo(double value) => m_Value = value;

    /// <summary>The inner value of the elo.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly double m_Value;
}

public partial struct Elo : IEquatable<Elo>
#if NET8_0_OR_GREATER
    , IEqualityOperators<Elo, Elo, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Elo other && Equals(other);

    /// <summary>Returns true if this instance and the other elo are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="Elo" /> to compare with.</param>
    [Pure]
    public bool Equals(Elo other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(Elo left, Elo right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(Elo left, Elo right) => !(left == right);
}

public partial struct Elo : IComparable, IComparable<Elo>
#if NET8_0_OR_GREATER
    , IComparisonOperators<Elo, Elo, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is Elo other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <inheritdoc />
    [Pure]
#nullable disable
    public int CompareTo(Elo other) => Comparer<double>.Default.Compare(m_Value, other.m_Value);
#nullable enable
    /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
    public static bool operator <(Elo l, Elo r) => l.CompareTo(r) < 0;

    /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
    public static bool operator >(Elo l, Elo r) => l.CompareTo(r) > 0;

    /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
    public static bool operator <=(Elo l, Elo r) => l.CompareTo(r) <= 0;

    /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
    public static bool operator >=(Elo l, Elo r) => l.CompareTo(r) >= 0;
}

public partial struct Elo : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the elo.</summary>
    [Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the elo.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the elo.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

#if NET8_0_OR_GREATER
#else
public partial struct Elo : ISerializable
{
    /// <summary>Initializes a new instance of the elo based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private Elo(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        m_Value = info.GetValue("Value", typeof(double)) is double val ? val : default(double);
    }

    /// <summary>Adds the underlying property of the elo to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info).AddValue("Value", m_Value);
}
#endif

public partial struct Elo
{
    /// <summary>Creates the elo from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized elo.
    /// </returns>
    [Pure]
    public static Elo FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct Elo : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the elo.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the elo from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the elo to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());
}

public partial struct Elo
#if NET8_0_OR_GREATER
    : IParsable<Elo>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="Elo"/>.</summary>
    /// <param name="s">
    /// A string containing the elo to convert.
    /// </param>
    /// <returns>
    /// The parsed elo.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Elo Parse(string? s)
        => TryParse(s, null, out var svo)
            ? svo
            : throw Unparsable.ForValue<Elo>(s, QowaivMessages.FormatExceptionElo);

    /// <summary>Converts the <see cref="string"/> to <see cref="Elo"/>.</summary>
    /// <param name="s">
    /// A string containing the elo to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed elo.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Elo Parse(string? s, IFormatProvider? provider)
        => TryParse(s, provider, out var svo)
            ? svo
            : throw Unparsable.ForValue<Elo>(s, QowaivMessages.FormatExceptionElo);

    /// <summary>Converts the <see cref="string"/> to <see cref="Elo"/>.</summary>
    /// <param name="s">
    /// A string containing the elo to convert.
    /// </param>
    /// <returns>
    /// The elo if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Elo? TryParse(string? s)
        => TryParse(s, null, out var val)
            ? val
            : default(Elo?);

    /// <summary>Converts the <see cref="string"/> to <see cref="Elo"/>.</summary>
    /// <param name="s">
    /// A string containing the elo to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The elo if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Elo? TryParse(string? s, IFormatProvider? provider) 
        => TryParse(s, provider, out var val) 
            ? val 
            : default(Elo?);

    /// <summary>Converts the <see cref="string"/> to <see cref="Elo"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the elo to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Impure]
    public static bool TryParse(string? s, out Elo result) => TryParse(s, null, out result);
}
