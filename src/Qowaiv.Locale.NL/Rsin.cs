#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

namespace Qowaiv.Locale.NL;

/// <summary>Represents a Rechtspersonen en Samenwerkingsverbanden Informatie Nummer (RSIN).</summary>
/// <remarks>
/// The RSIN is an identification number for legal entities and partnerships.
/// Dutch government organisations such as the Netherlands Tax Administration,
/// the Land Registry Office (Kadaster), and municipalities use it to exchange
/// information about businesses. It consists of nine digits that satisfy the
/// elfproef (eleven test).
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(long))]
[OpenApiDataType(description: "RSIN (Rechtspersonen en Samenwerkingsverbanden Informatie Nummer)", type: "string", format: "rsin", example: "111222333", pattern: "[0-9]{8,9}")]
[TypeConverter(typeof(Conversion.RsinTypeConverter))]
#if NET10_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.RsinJsonConverter))]
#endif
public readonly partial struct Rsin : IXmlSerializable, IFormattable, IEquatable<Rsin>, IComparable, IComparable<Rsin>
{
    /// <summary>Represents an unknown (but set) RSIN (9999.99.999).</summary>
    public static Rsin Unknown => new(9999_99_999);

    /// <summary>Returns a <see cref="string" /> that represents the RSIN for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the RSIN.</summary>
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
        Span<char> s = stackalloc char[11];
        TryFormat(s, out var written, format.AsSpan(), null);
        return s[..written].ToString();
    }

    /// <summary>Gets an XML string representation of the RSIN.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the RSIN to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Deserializes the RSIN from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized RSIN.
    /// </returns>
    [Pure]
    public static Rsin FromJson(long json) => Create(json);

    /// <summary>Casts the RSIN to a <see cref="long" />.</summary>
    public static explicit operator long(Rsin val) => val.m_Value;

    /// <summary>Casts a <see cref="long" /> to an RSIN.</summary>
    public static explicit operator Rsin(long val) => Create(val);

    /// <summary>Converts the <see cref="string" /> to <see cref="Rsin" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the RSIN to convert.
    /// </param>
    /// <param name="provider">
    /// The specified format provider.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Pure]
    public static bool TryParse(string? s, IFormatProvider? provider, out Rsin result)
        => TryParse(s.AsSpan(), provider, out result);

    /// <summary>Tries to create an RSIN.</summary>
    /// <param name="value">
    /// The number describing an RSIN.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Not an RSIN.
    /// </exception>
    [Pure]
    public static Rsin Create(long value)
        => TryCreate(value)
        ?? throw new ArgumentOutOfRangeException(nameof(value), QowaivMessages.FormatExceptionRsin);

    /// <summary>Tries to create an RSIN.</summary>
    /// <param name="value">
    /// The number describing an RSIN.
    /// </param>
    /// <returns>
    /// An RSIN when valid, otherwise null.
    /// </returns>
    [Pure]
    public static Rsin? TryCreate(long value)
        => Elfproef.IsValid(value)
        ? new(value)
        : null;
}

#if NETSTANDARD2_0
public readonly partial struct Rsin
#else
public readonly partial struct Rsin : ISpanParsable<Rsin>, ISpanFormattable
#endif
{
    /// <summary>
    /// Tries to format the value of the current instance into the provided span of characters.
    /// </summary>
    /// <param name="destination">
    /// The span in which to write this instance's value formatted as a span of characters.
    /// </param>
    /// <param name="charsWritten">
    /// When this method returns, contains the number of characters that were written
    /// in destination.
    /// </param>
    /// <param name="format">
    /// A span containing the characters that represent a standard or custom format string
    /// that defines the acceptable format for destination.
    /// </param>
    /// <param name="provider">
    /// An optional object that supplies culture-specific formatting information for
    /// destination.
    /// </param>
    /// <returns>
    /// true if the formatting was successful; otherwise, false.
    /// </returns>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => Elfproef.TryFormat(m_Value, destination, out charsWritten, format, provider);

    /// <summary>Tries to parse a span of characters into an RSIN.</summary>
    /// <param name="s">
    /// The span of characters to parse.
    /// </param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about s.
    /// </param>
    /// <param name="result">
    /// When this method returns, contains the result of successfully parsing s, or an
    /// undefined value on failure.
    /// </param>
    /// <returns>
    /// true if s was successfully parsed; otherwise, false.
    /// </returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Rsin result)
    {
        result = default;
        s = s.Trim();

        if (s is { Length: 0 })
        {
            return true;
        }
        else if (Elfproef.TryParse(s, out var value))
        {
            result = new(value);
            return true;
        }
        else if (Qowaiv.Unknown.IsUnknown(s.ToString()))
        {
            result = Unknown;
            return true;
        }
        else return false;
    }

    /// <summary>Parses a span of characters into an RSIN.</summary>
    /// <param name="s">
    /// The span of characters to parse.
    /// </param>
    /// <param name="provider">
    /// An object that provides culture-specific formatting information about s.
    /// </param>
    /// <returns>
    /// The result of parsing s.
    /// </returns>
    /// <exception cref="FormatException">
    /// s is not in the correct format.
    /// </exception>
    [Pure]
    public static Rsin Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s, provider, out var rsin)
        ? rsin
        : throw Unparsable.ForValue<Rsin>(s.ToString(), QowaivMessages.FormatExceptionRsin);
}
