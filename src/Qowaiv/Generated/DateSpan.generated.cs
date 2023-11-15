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

public partial struct DateSpan
{
    private DateSpan(ulong value) => m_Value = value;

    /// <summary>The inner value of the date span.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ulong m_Value;
}

public partial struct DateSpan : IEquatable<DateSpan>
#if NET7_0_OR_GREATER
    , IEqualityOperators<DateSpan, DateSpan, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is DateSpan other && Equals(other);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(DateSpan left, DateSpan right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(DateSpan left, DateSpan right) => !(left == right);
}

public partial struct DateSpan : IComparable, IComparable<DateSpan>
#if NET7_0_OR_GREATER
    , IComparisonOperators<DateSpan, DateSpan, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is DateSpan other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
    public static bool operator <(DateSpan l, DateSpan r) => l.CompareTo(r) < 0;

    /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
    public static bool operator >(DateSpan l, DateSpan r) => l.CompareTo(r) > 0;

    /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
    public static bool operator <=(DateSpan l, DateSpan r) => l.CompareTo(r) <= 0;

    /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
    public static bool operator >=(DateSpan l, DateSpan r) => l.CompareTo(r) >= 0;
}

public partial struct DateSpan : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the date span.</summary>
    [Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the date span.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the date span.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

#if NET8_0_OR_GREATER
#else
public partial struct DateSpan : ISerializable
{
    /// <summary>Initializes a new instance of the date span based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private DateSpan(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        m_Value = info.GetValue("Value", typeof(ulong)) is ulong val ? val : default(ulong);
    }

    /// <summary>Adds the underlying property of the date span to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info).AddValue("Value", m_Value);
}
#endif

public partial struct DateSpan
{
    /// <summary>Creates the date span from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized date span.
    /// </returns>
    [Pure]
    public static DateSpan FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct DateSpan : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the date span.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the date span from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the date span to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());
}

public partial struct DateSpan
#if NET7_0_OR_GREATER
    : IParsable<DateSpan>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="DateSpan"/>.</summary>
    /// <param name="s">
    /// A string containing the date span to convert.
    /// </param>
    /// <returns>
    /// The parsed date span.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static DateSpan Parse(string? s) => Parse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="DateSpan"/>.</summary>
    /// <param name="s">
    /// A string containing the date span to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed date span.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static DateSpan Parse(string? s, IFormatProvider? formatProvider) 
        => TryParse(s, formatProvider) 
        ?? throw Unparsable.ForValue<DateSpan>(s, QowaivMessages.FormatExceptionDateSpan);

    /// <summary>Converts the <see cref="string"/> to <see cref="DateSpan"/>.</summary>
    /// <param name="s">
    /// A string containing the date span to convert.
    /// </param>
    /// <returns>
    /// The date span if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static DateSpan? TryParse(string? s) => TryParse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="DateSpan"/>.</summary>
    /// <param name="s">
    /// A string containing the date span to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The date span if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static DateSpan? TryParse(string? s, IFormatProvider? formatProvider) => TryParse(s, formatProvider, out var val) ? val : default(DateSpan?);

    /// <summary>Converts the <see cref="string"/> to <see cref="DateSpan"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the date span to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Impure]
    public static bool TryParse(string? s, out DateSpan result) => TryParse(s, null, out result);
}

public partial struct DateSpan
{
    /// <summary>Returns true if the value represents a valid date span.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use DateSpan.TryParse(str) is { } instead. Will be dropped when the next major version is released.")]
    public static bool IsValid(string? val) => IsValid(val, (IFormatProvider?)null);

    /// <summary>Returns true if the value represents a valid date span.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The <see cref="IFormatProvider"/> to interpret the <see cref="string"/> value with.
    /// </param>
    [Pure]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use DateSpan.TryParse(str, formatProvider) is { } instead. Will be dropped when the next major version is released.")]
    public static bool IsValid(string? val, IFormatProvider? formatProvider)
        => !string.IsNullOrWhiteSpace(val)
        && TryParse(val, formatProvider, out _);
}
