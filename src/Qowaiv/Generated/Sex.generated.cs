﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#define NoComparisonOperators

namespace Qowaiv;

public partial struct Sex
{
#if !NotField
    private Sex(byte value) => m_Value = value;

    /// <summary>The inner value of the sex.</summary>
    private byte m_Value;
#endif

#if !NotIsEmpty
    /// <summary>Returns true if the  sex is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty() => m_Value == default;
#endif
#if !NotIsUnknown
    /// <summary>Returns true if the  sex is unknown, otherwise false.</summary>
    [Pure]
    public bool IsUnknown() => m_Value == Unknown.m_Value;
#endif
#if !NotIsEmptyOrUnknown
    /// <summary>Returns true if the  sex is empty or unknown, otherwise false.</summary>
    [Pure]
    public bool IsEmptyOrUnknown() => IsEmpty() || IsUnknown();
#endif
}

public partial struct Sex : IEquatable<Sex>
{
    /// <inheritdoc />
    [Pure]
    public override bool Equals(object obj) => obj is Sex other && Equals(other);

#if !NotEqualsSvo
    /// <summary>Returns true if this instance and the other sex are equal, otherwise false.</summary>
    /// <param name="other">The <see cref="Sex" /> to compare with.</param>
    [Pure]
    public bool Equals(Sex other) => m_Value == other.m_Value;

#if !NotGetHashCode
    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash.Code(m_Value);
#endif
#endif
    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator !=(Sex left, Sex right) => !(left == right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand</param>
    public static bool operator ==(Sex left, Sex right) => left.Equals(right);
}

public partial struct Sex : IComparable, IComparable<Sex>
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object obj)
    {
        if (obj is null) { return 1; }
        else if (obj is Sex other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
#if !NotEqualsSvo
    /// <inheritdoc />
    [Pure]
    public int CompareTo(Sex other) => Comparer<byte>.Default.Compare(m_Value, other.m_Value);
#endif
#if !NoComparisonOperators
    /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
    public static bool operator <(Sex l, Sex r) => l.CompareTo(r) < 0;

    /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
    public static bool operator >(Sex l, Sex r) => l.CompareTo(r) > 0;

    /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
    public static bool operator <=(Sex l, Sex r) => l.CompareTo(r) <= 0;

    /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
    public static bool operator >=(Sex l, Sex r) => l.CompareTo(r) >= 0;
#endif
}

public partial struct Sex : IFormattable
{
    /// <summary>Returns a <see cref="string"/> that represents the sex.</summary>
    [Pure]
    public override string ToString() => ToString((IFormatProvider)null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the sex.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string format) => ToString(format, null);

    /// <summary>Returns a formatted <see cref="string"/> that represents the sex.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider provider) => ToString(null, provider);
}

public partial struct Sex : ISerializable
{
    /// <summary>Initializes a new instance of the sex based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private Sex(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info, nameof(info));
        m_Value = (byte)info.GetValue("Value", typeof(byte));
    }

    /// <summary>Adds the underlying property of the sex to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info, nameof(info)).AddValue("Value", m_Value);
}

public partial struct Sex
{
    /// <summary>Creates the sex from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized sex.
    /// </returns>
#if !NotCultureDependent
    [Pure]
    public static Sex FromJson(string json) => Parse(json, CultureInfo.InvariantCulture);
#else
    [Pure]
    public static Sex FromJson(string json) => Parse(json);
#endif
}

public partial struct Sex : IXmlSerializable
{
    /// <summary>Gets the <see href="XmlSchema" /> to XML (de)serialize the sex.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema IXmlSerializable.GetSchema() => null;

    /// <summary>Reads the sex from an <see href="XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader, nameof(reader));
        var xml = reader.ReadElementString();
#if !NotCultureDependent
        var val = Parse(xml, CultureInfo.InvariantCulture);
#else
        var val = Parse(xml);
#endif
#if !NotField
        m_Value = val.m_Value;
#endif
        OnReadXml(val);
    }
    partial void OnReadXml(Sex other);

    /// <summary>Writes the sex to an <see href="XmlWriter" />.</summary>
    /// <remarks>
    /// Uses <see cref="ToXmlString()"/>.
    /// </remarks>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
        => Guard.NotNull(writer, nameof(writer)).WriteString(ToXmlString());
}

public partial struct Sex
{
#if !NotCultureDependent
    /// <summary>Converts the <see cref="string"/> to <see cref="Sex"/>.</summary>
    /// <param name="s">
    /// A string containing the sex to convert.
    /// </param>
    /// <returns>
    /// The parsed sex.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Sex Parse(string s) => Parse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="Sex"/>.</summary>
    /// <param name="s">
    /// A string containing the sex to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The parsed sex.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Sex Parse(string s, IFormatProvider formatProvider) => TryParse(s, formatProvider) ?? throw new FormatException(QowaivMessages.FormatExceptionSex);

    /// <summary>Converts the <see cref="string"/> to <see cref="Sex"/>.</summary>
    /// <param name="s">
    /// A string containing the sex to convert.
    /// </param>
    /// <returns>
    /// The sex if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Sex? TryParse(string s) => TryParse(s, null);

    /// <summary>Converts the <see cref="string"/> to <see cref="Sex"/>.</summary>
    /// <param name="s">
    /// A string containing the sex to convert.
    /// </param>
    /// <param name="formatProvider">
    /// The specified format provider.
    /// </param>
    /// <returns>
    /// The sex if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Sex? TryParse(string s, IFormatProvider formatProvider) => TryParse(s, formatProvider, out Sex val) ? val : default(Sex?);

    /// <summary>Converts the <see cref="string"/> to <see cref="Sex"/>.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the sex to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Pure]
    public static bool TryParse(string s, out Sex result) => TryParse(s, null, out result);
#else
    /// <summary>Converts the <see cref="string"/> to <see cref="Sex"/>.</summary>
    /// <param name="s">
    /// A string containing the sex to convert.
    /// </param>
    /// <returns>
    /// The parsed sex.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not in the correct format.
    /// </exception>
    [Pure]
    public static Sex Parse(string s) => TryParse(s) ?? throw new FormatException(QowaivMessages.FormatExceptionSex);

    /// <summary>Converts the <see cref="string"/> to <see cref="Sex"/>.</summary>
    /// <param name="s">
    /// A string containing the sex to convert.
    /// </param>
    /// <returns>
    /// The sex if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Sex? TryParse(string s) => TryParse(s, out Sex val) ? val : default(Sex?);
#endif
}

public partial struct Sex
{
#if !NotCultureDependent

    /// <summary>Returns true if the value represents a valid sex.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    public static bool IsValid(string val) => IsValid(val, (IFormatProvider)null);

    /// <summary>Returns true if the value represents a valid sex.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The <see cref="IFormatProvider"/> to interpret the <see cref="string"/> value with.
    /// </param>
    [Pure]
    public static bool IsValid(string val, IFormatProvider formatProvider)
        => !string.IsNullOrWhiteSpace(val)
        && TryParse(val, formatProvider, out _);
#else
    /// <summary>Returns true if the value represents a valid sex.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    public static bool IsValid(string val)
        => !string.IsNullOrWhiteSpace(val)
        && TryParse(val, out _);
#endif
}

