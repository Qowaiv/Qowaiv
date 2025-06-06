#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
using Qowaiv.Conversion.Identifiers;

namespace Qowaiv.Identifiers;

/// <summary>Represents a strongly typed identifier.</summary>
/// <typeparam name="TIdentifier">
/// The type of the <see cref="IIdentifierBehavior" /> that deals with the
/// identifier specific behavior.
/// </typeparam>
[DebuggerDisplay("{DebuggerDisplay}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.AllExcludingCulture ^ SingleValueStaticOptions.HasUnknownValue, typeof(object))]
[OpenApiDataType(description: "identifier", example: "8a1a8c42-d2ff-e254-e26e-b6abcbf19420", type: "any")]
[TypeConverter(typeof(IdTypeConverter))]
#if NET6_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Identifiers.IdJsonConverter))]
#endif
[Obsolete("Use a generated Id with [Id<IdBehavior>] instead.")]
public readonly struct Id<TIdentifier> : IXmlSerializable, IFormattable, IEquatable<Id<TIdentifier>>, IComparable, IComparable<Id<TIdentifier>>, IEmpty<Id<TIdentifier>>
#if NET8_0_OR_GREATER
, IEqualityOperators<Id<TIdentifier>, Id<TIdentifier>, bool>
, IParsable<Id<TIdentifier>>
#endif
#if NET8_0_OR_GREATER
#else
, ISerializable
#endif
    where TIdentifier : IIdentifierBehavior, new()
{
    /// <summary>An singleton instance that deals with the identifier specific behavior.</summary>
    private static readonly TIdentifier behavior = new();

    /// <summary>Represents an empty/not set identifier.</summary>
    public static Id<TIdentifier> Empty => default;

    /// <summary>Initializes a new instance of the <see cref="Id{TIdentifier}" /> struct.</summary>
    private Id(object? value) => m_Value = value;

#if NET8_0_OR_GREATER
#else
    /// <summary>Initializes a new instance of the <see cref="Id{TIdentifier}" /> struct.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private Id(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        m_Value = info.GetValue("Value", typeof(object));
    }

    /// <summary>Adds the underlying property of the identifier to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info);
        info.AddValue("Value", m_Value);
    }
#endif

    /// <summary>The inner value of the identifier.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly object? m_Value;

    /// <summary>False if the identifier is empty, otherwise true.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public bool HasValue => !IsEmpty();

    /// <summary>Returns true if the identifier is empty, otherwise false.</summary>
    [Pure]
    public bool IsEmpty()
        => m_Value == default
        || m_Value.Equals(Guid.Empty)
        || m_Value.Equals(0L);

    /// <summary>Gets a <see cref="byte" /> array that represents the identifier.</summary>
    [Pure]
    public byte[] ToByteArray() => IsEmpty() ? [] : behavior.ToByteArray(m_Value);

    /// <inheritdoc/>
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        else if (obj is Id<TIdentifier> other) return CompareTo(other);
        else throw new ArgumentException($"Argument must be Id<{typeof(TIdentifier).Name}>.", nameof(obj));
    }

    /// <inheritdoc/>
    [Pure]
    public int CompareTo(Id<TIdentifier> other)
    {
        var isEmpty = IsEmpty();
        var otherIsEmpty = other.IsEmpty();

        if (isEmpty || otherIsEmpty)
        {
            return otherIsEmpty.CompareTo(isEmpty);
        }
        return behavior.Compare(m_Value, other.m_Value);
    }

    /// <inheritdoc/>
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Id<TIdentifier> other && Equals(other);

    /// <summary>Returns true if this instance and the other identifier are equal, otherwise false.</summary>
    /// <param name="other">
    /// The <see cref = "Id{TIdentifier}" /> to compare with.
    /// </param>
    [Pure]
    public bool Equals(Id<TIdentifier> other)
        => m_Value is null || other.m_Value is null
        ? m_Value == other.m_Value
        : behavior.Equals(m_Value, other.m_Value);

    /// <inheritdoc/>
    [Pure]
    public override int GetHashCode() => m_Value is null ? 0 : behavior.GetHashCode(m_Value);

    /// <summary>Returns true if the left and right operand are equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    public static bool operator !=(Id<TIdentifier> left, Id<TIdentifier> right) => !(left == right);

    /// <summary>Returns true if the left and right operand are not equal, otherwise false.</summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    public static bool operator ==(Id<TIdentifier> left, Id<TIdentifier> right) => left.Equals(right);

    /// <summary>Returns a <see cref="string" /> that represents the identifier for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsEmpty()
        ? $"{DebugDisplay.Empty} ({typeof(TIdentifier).Name})"
        : $"{this} ({typeof(TIdentifier).Name})";

    /// <summary>Returns a <see cref = "string" /> that represents the identifier.</summary>
    [Pure]
    public override string ToString() => ToString(CultureInfo.CurrentCulture);

    /// <summary>Returns a formatted <see cref = "string" /> that represents the identifier.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    [Pure]
    public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

    /// <summary>Returns a formatted <see cref = "string" /> that represents the identifier.</summary>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(IFormatProvider provider) => ToString(string.Empty, provider);

    /// <summary>Returns a formatted <see cref="string" /> that represents the identifier.</summary>
    /// <param name="format">
    /// The format that describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted))
        {
            return formatted;
        }
        else return IsEmpty() ? string.Empty : behavior.ToString(m_Value, format, formatProvider);
    }

    /// <summary>Gets the <see href = "XmlSchema" /> to XML (de)serialize the identifier.</summary>
    /// <remarks>
    /// Returns null as no schema is required.
    /// </remarks>
    [Pure]
    XmlSchema? IXmlSerializable.GetSchema() => null;

    /// <summary>Reads the identifier from an <see href = "XmlReader" />.</summary>
    /// <param name="reader">An XML reader.</param>
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        Guard.NotNull(reader);
        var xml = reader.ReadElementString();
        System.Runtime.CompilerServices.Unsafe.AsRef(in this) = Parse(xml);
    }

    /// <summary>Writes the identifier to an <see href = "XmlWriter" />.</summary>
    /// <param name="writer">An XML writer.</param>
    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
        Guard.NotNull(writer);
        writer.WriteString(ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Serializes the identifier to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON node.
    /// </returns>
    [Pure]
    public object? ToJson() => IsEmpty() ? null : behavior.ToJson(m_Value);

    [Pure]
    private TPrimitive? CastToPrimitive<TPrimitive, TTo>()
    {
        if (behavior.BaseType == typeof(TPrimitive))
        {
            return m_Value is TPrimitive primitive ? primitive : default;
        }
        else throw Exceptions.InvalidCast<Id<TIdentifier>, TTo>();
    }

    /// <summary>Casts the identifier to a <see cref="string" />.</summary>
    public static explicit operator string?(Id<TIdentifier> id) => id.CastToPrimitive<string, string>();

    /// <summary>Casts the identifier to a <see cref="int" />.</summary>
    public static explicit operator int(Id<TIdentifier> id) => id.CastToPrimitive<int, int>();

    /// <summary>Casts the identifier to a <see cref="long" />.</summary>
    public static explicit operator long(Id<TIdentifier> id) => id.CastToPrimitive<long, long>();

    /// <summary>Casts the identifier to a <see cref="Guid" />.</summary>
    public static explicit operator Guid(Id<TIdentifier> id) => id.CastToPrimitive<Guid, Guid>();

    /// <summary>Casts the identifier to a <see cref="Uuid" />.</summary>
    public static explicit operator Uuid(Id<TIdentifier> id) => id.CastToPrimitive<Guid, Uuid>();

    /// <summary>Casts the <see cref="string" /> to an identifier.</summary>
    public static explicit operator Id<TIdentifier>(string id) => Create(id);

    /// <summary>Casts the <see cref="int" /> to an identifier.</summary>
    public static explicit operator Id<TIdentifier>(int id) => Create(id);

    /// <summary>Casts the <see cref="long" /> to an identifier.</summary>
    public static explicit operator Id<TIdentifier>(long id) => Create(id);

    /// <summary>Casts the <see cref="Guid" /> to an identifier.</summary>
    public static explicit operator Id<TIdentifier>(Guid id) => Create(id);

    /// <summary>Casts the <see cref="Uuid" /> to an identifier.</summary>
    public static explicit operator Id<TIdentifier>(Uuid id) => Create(id);

    /// <summary>Converts the <see cref="string" /> to <see cref="Id{TIdentifier}" />.</summary>
    /// <param name="s">
    /// A string containing the identifier to convert.
    /// </param>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    /// <returns>
    /// The parsed identifier.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s" /> is not in the correct format.
    /// </exception>
    [Pure]
    public static Id<TIdentifier> Parse(string s, IFormatProvider? provider)
        => Parse(s);

    /// <summary>Converts the <see cref="string" /> to <see cref="Id{TIdentifier}" />.</summary>
    /// <param name="s">
    /// A string containing the identifier to convert.
    /// </param>
    /// <returns>
    /// The parsed identifier.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s" /> is not in the correct format.
    /// </exception>
    [Pure]
    public static Id<TIdentifier> Parse(string? s)
        => TryParse(s, out Id<TIdentifier> val)
        ? val
        : throw Unparsable.ForValue<Id<TIdentifier>>(s, QowaivMessages.FormatExceptionIdentifier);

    /// <summary>Converts the <see cref="string" /> to <see cref="Id{TIdentifier}" />.</summary>
    /// <param name="s">
    /// A string containing the identifier to convert.
    /// </param>
    /// <returns>
    /// The identifier if the string was converted successfully, otherwise default.
    /// </returns>
    [Pure]
    public static Id<TIdentifier> TryParse(string? s)
    {
        return TryParse(s, out Id<TIdentifier> val) ? val : default;
    }

    /// <summary>Converts the <see cref="string" /> to <see cref = "Id{TIdentifier}" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the identifier to convert.
    /// </param>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out Id<TIdentifier> result)
        => TryParse(s, out result);

    /// <summary>Converts the <see cref="string" /> to <see cref = "Id{TIdentifier}" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the identifier to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    public static bool TryParse(string? s, out Id<TIdentifier> result)
    {
        result = default;

        if (string.IsNullOrEmpty(s)) return true;
        else if (behavior.TryParse(s, out var id))
        {
            result = new Id<TIdentifier>(id);
            return true;
        }
        else return false;
    }

    /// <summary>Creates the identifier from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized identifier.
    /// </returns>
    [Pure]
    public static Id<TIdentifier> FromJson(string? json) => Parse(json);

    /// <summary>Deserializes the date from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized date.
    /// </returns>
    [Pure]
    public static Id<TIdentifier> FromJson(long json) => new(behavior.FromJson(json));

    /// <summary>Creates the identfier for the <see cref="byte" /> array.</summary>
    /// <param name="bytes">
    /// The <see cref="byte" /> array that represents the underlying value.
    /// </param>
    [Pure]
    public static Id<TIdentifier> FromBytes(byte[]? bytes)
    {
        return bytes is not { Length: > 0 }
            ? Empty
            : new Id<TIdentifier>(behavior.FromBytes(bytes));
    }

    /// <summary>Creates an identifier from an <see cref="object" />.</summary>
    /// <param name="obj">
    /// The <see cref="object" /> to create an indentifier from.
    /// </param>
    /// <exception cref="InvalidCastException">
    /// if the identifier could not be created from the <see cref="object" />.
    /// </exception>
    [Pure]
    public static Id<TIdentifier> Create(object? obj)
        => TryCreate(obj, out var id)
        ? id
        : throw Exceptions.InvalidCast(obj?.GetType(), typeof(Id<TIdentifier>));

    /// <summary>Tries to create an identifier from an <see cref="object" />.</summary>
    /// <param name="obj">
    /// The <see cref="object" /> to create an indentifier from.
    /// </param>
    /// <param name="id">
    /// The created identifier.
    /// </param>
    /// <returns>
    /// True if the identifier could be created.
    /// </returns>
    public static bool TryCreate(object? obj, out Id<TIdentifier> id)
    {
        id = default;

        if (obj is null)
        {
            return true;
        }
        else if (behavior.TryCreate(obj, out var underlying))
        {
            id = new Id<TIdentifier>(underlying);
            return true;
        }
        else return false;
    }

    /// <summary>Creates a new identifier.</summary>
    [Pure]
    public static Id<TIdentifier> Next() => new(behavior.Next());
}
