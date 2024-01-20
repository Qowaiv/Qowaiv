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

public partial struct Money : IEquatable<Money>
#if NET8_0_OR_GREATER
    , IEqualityOperators<Money, Money, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Money other && Equals(other);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(Money left, Money right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(Money left, Money right) => !(left == right);
}

public partial struct Money : IComparable, IComparable<Money>
#if NET8_0_OR_GREATER
    , IComparisonOperators<Money, Money, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is Money other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
    public static bool operator <(Money l, Money r) => l.CompareTo(r) < 0;

    /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
    public static bool operator >(Money l, Money r) => l.CompareTo(r) > 0;

    /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
    public static bool operator <=(Money l, Money r) => l.CompareTo(r) <= 0;

    /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
    public static bool operator >=(Money l, Money r) => l.CompareTo(r) >= 0;
}

public partial struct Money : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the money.</summary>
    [Pure]
    public override string ToString() => ToString(format: null, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the money.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the money.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

public partial struct Money
{
    /// <summary>Creates the money from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized money.
    /// </returns>
    [Pure]
    public static Money FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct Money : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the money.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the money from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the money to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());
}

public partial struct Money
#if NET8_0_OR_GREATER
    : IParsable<Money>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="Money"/>.</summary>
    /// <param name="s">
    /// A string containing the money to convert.
    /// </param>
    /// <returns>
    /// The parsed money.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Money Parse(string? s)
        => TryParse(s, null, out var svo)
            ? svo
            : throw Unparsable.ForValue<Money>(s, QowaivMessages.FormatExceptionMoney);

    /// <summary>Converts the <see cref="string"/> to <see cref="Money"/>.</summary>
    /// <param name="s">
    /// A string containing the money to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed money.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Money Parse(string? s, IFormatProvider? provider)
        => TryParse(s, provider, out var svo)
            ? svo
            : throw Unparsable.ForValue<Money>(s, QowaivMessages.FormatExceptionMoney);

    /// <summary>Converts the <see cref="string"/> to <see cref="Money"/>.</summary>
    /// <param name="s">
    /// A string containing the money to convert.
    /// </param>
    /// <returns>
    /// The money if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Money? TryParse(string? s)
        => TryParse(s, null, out var val)
            ? val
            : default(Money?);

    /// <summary>Converts the <see cref="string"/> to <see cref="Money"/>.</summary>
    /// <param name="s">
    /// A string containing the money to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The money if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Money? TryParse(string? s, IFormatProvider? provider)
        => TryParse(s, provider, out var val)
            ? val
            : default(Money?);

    /// <summary>Converts the <see cref="string"/> to <see cref="Money"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the money to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Impure]
    public static bool TryParse(string? s, out Money result) => TryParse(s, null, out result);
}
