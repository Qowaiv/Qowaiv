#if NotComparisonOperators // exec
#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable
#endif // exec

namespace @Namespace;

/// <summary>Represents a @FullName.</summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(@type))]
[OpenApiDataType(
    description: "@FullName",
    type: "string",
    format: "@TSvo",
    example: "ABC")]
[TypeConverter(typeof(Conversion.@TSvoTypeConverter))]
#if NET10_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.@TSvoJsonConverter))]
#endif
public readonly partial struct @TSvo : IXmlSerializable, IFormattable, IEquatable<@TSvo>, IComparable, IComparable<@TSvo>
{
    /// <summary>Represents an unknown (but set) @FullName.</summary>
    public static @TSvo Unknown => new @TSvo(default);

    /// <summary>Gets the number of characters of @FullName.</summary>
    public int Length => m_Value == null ? 0 : m_Value.Length;

    /// <summary>Returns a <see cref="string" /> that represents the @FullName for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the @FullName.</summary>
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
        throw new NotImplementedException();
    }

    /// <summary>Gets an XML string representation of the @FullName.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the @FullName to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Deserializes the @FullName from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized @FullName.
    /// </returns>
    [Pure]
    public static @TSvo FromJson(double json) => Create(json);

    /// <summary>Deserializes the @FullName from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized @FullName.
    /// </returns>
    [Pure]
    public static @TSvo FromJson(long json) => Create(json);

    /// <summary>Deserializes the @FullName from a JSON boolean.</summary>
    /// <param name="json">
    /// The number boolean to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized @FullName.
    /// </returns>
    [Pure]
    public static @TSvo FromJson(bool json) => Create(json);

    /// <summary>Casts the @FullName to a <see cref="string" />.</summary>
    public static explicit operator string(@TSvo val) => val.ToString(CultureInfo.CurrentCulture);

    /// <summary>Casts a <see cref="string" /> to a @FullName.</summary>
    public static explicit operator @TSvo(string str) => Parse(str, CultureInfo.CurrentCulture);

    /// <summary>Casts the @FullName to a <see cref="@type" />.</summary>
    public static explicit operator @type(@TSvo val) => val.m_Value;

    /// <summary>Casts a <see cref="@type" /> to a @FullName.</summary>
    public static explicit operator @TSvo(@type val) => Create(val);

#if !NotCultureDependent // exec

    /// <summary>Converts the <see cref="string" /> to <see cref="@TSvo" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
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
    public static bool TryParse(string? s, IFormatProvider? provider, out @TSvo result)
        => TryParse(s.AsSpan(), provider, out result);
#else // exec
    /// <summary>Converts the <see cref="string" /> to <see cref="@TSvo" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the @FullName to convert.
    /// </param>
    /// <param name="result">
    /// The result of the parsing.
    /// </param>
    /// <returns>
    /// True if the string was converted successfully, otherwise false.
    /// </returns>
    [Pure]
    public static bool TryParse(string? s, out @TSvo result)
    {
        result = default;
        if (string.IsNullOrEmpty(s))
        {
            return true;
        }
        if (Qowaiv.Unknown.IsUnknown(s))
        {
            result = Unknown;
            return true;
        }
        throw new NotImplementedException();
    }
#endif // exec
}

#if NETSTANDARD2_0
public readonly partial struct @TSvo
#else
public readonly partial struct @TSvo : ISpanParsable<@TSvo>, ISpanFormattable
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
        => throw new NotImplementedException();

    /// <summary>Tries to parse a span of characters into a @FullName.</summary>
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
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out @TSvo result)
    {
        result = default;
        if (s.IsEmpty)
        {
            return true;
        }
        if (Qowaiv.Unknown.IsUnknown(s.ToString(), provider as CultureInfo))
        {
            result = Unknown;
            return true;
        }
        throw new NotImplementedException();
    }

    /// <summary>Parses a span of characters into a @FullName.</summary>
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
    public static @TSvo Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s, provider, out var svo)
        ? svo
        : throw Unparsable.ForValue<@TSvo>(s.ToString(), @FormatExceptionMessage);
}
