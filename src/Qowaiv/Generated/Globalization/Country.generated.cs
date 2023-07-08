﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#nullable enable

namespace Qowaiv.Globalization;

public partial struct Country
{
    private Country(string? value) => m_Value = value;

    /// <summary>The inner value of the country.</summary>
    private readonly string? m_Value;

    /// <summary>Returns true if the country is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => m_Value == default;
    /// <summary>Returns true if the country is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == Unknown.m_Value;
    /// <summary>Returns true if the country is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
}

public partial struct Country : IEquatable<Country>
#if NET7_0_OR_GREATER
    , IEqualityOperators<Country, Country, bool>
#endif
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is Country other && Equals(other);

    /// <summary>Returns true if this instance and the other country are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="Country" /> to compare with.</param>
    [Pure]
    public bool Equals(Country other) => m_Value == other.m_Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(Country left, Country right) => left.Equals(right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(Country left, Country right) => !(left == right);
}

public partial struct Country : IComparable, IComparable<Country>
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is Country other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
    /// <inheritdoc />
    [Pure]
#nullable disable
    public int CompareTo(Country other) => Comparer<string>.Default.Compare(m_Value, other.m_Value);
#nullable enable
}

public partial struct Country : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the country.</summary>
    [Pure]
    public override string ToString() => ToString(provider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the country.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string? format) => ToString(format, formatProvider: null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the country.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider? provider) => ToString(format: null, provider);
}

public partial struct Country : ISerializable
{
    /// <summary>Initializes a new instance of the country based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private Country(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        m_Value = info.GetValue("Value", typeof(string)) is string val ? val : default(string);
    }

    /// <summary>Adds the underlying property of the country to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info).AddValue("Value", m_Value);
}

public partial struct Country
{
    /// <summary>Creates the country from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized country.
    /// </returns>
    [Pure]
    public static Country FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
}

public partial struct Country : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the country.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => (XmlSchema?)null;

    /// <summary>Reads the country from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(this) = Parse(xml, CultureInfo.InvariantCulture);
    }

    /// <summary>Writes the country to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer).WriteString(ToXmlString());
}

public partial struct Country
#if NET7_0_OR_GREATER
    : IParsable<Country>
#endif
{
    /// <summary>Converts the <see cref="string"/> to <see cref="Country"/>.</summary>
    /// <param name="s">
    /// A string containing the country to convert.
    /// </param>
    /// <returns>
    /// The parsed country.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Country Parse(string? s) => Parse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="Country"/>.</summary>
    /// <param name="s">
    /// A string containing the country to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed country.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Country Parse(string? s, IFormatProvider? formatProvider) => TryParse(s, formatProvider) ?? throw new FormatException(QowaivMessages.FormatExceptionCountry);

    /// <summary>Converts the <see cref="string"/> to <see cref="Country"/>.</summary>
    /// <param name="s">
    /// A string containing the country to convert.
    /// </param>
    /// <returns>
    /// The country if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Country? TryParse(string? s) => TryParse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="Country"/>.</summary>
    /// <param name="s">
    /// A string containing the country to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The country if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Country? TryParse(string? s, IFormatProvider? formatProvider) => TryParse(s, formatProvider, out var val) ? val : default(Country?);

    /// <summary>Converts the <see cref="string"/> to <see cref="Country"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the country to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Impure]
    public static bool TryParse(string? s, out Country result) => TryParse(s, null, out result);
}

public partial struct Country
{
    /// <summary>Returns true if the value represents a valid country.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use Country.TryParse(str) is { } instead. Will be dropped when the next major version is released.")]
    public static bool IsValid(string? val) => IsValid(val, (IFormatProvider?)null);

    /// <summary>Returns true if the value represents a valid country.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The <see cref="IFormatProvider"/> to interpret the <see cref="string"/> value with.
    /// </param>
    [Pure]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use Country.TryParse(str, formatProvider) is { } instead. Will be dropped when the next major version is released.")]
    public static bool IsValid(string? val, IFormatProvider? formatProvider)
        => !string.IsNullOrWhiteSpace(val)
        && TryParse(val, formatProvider, out _);
}
