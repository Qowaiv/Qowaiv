#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

namespace Qowaiv.Locale.NL;

/// <summary>Represents a BAG-identificatienummer.</summary>
/// <remarks>
/// A BAG-identificatienummer is a 16-digit identifier that consists of a four-digit
/// municipality code, a two-digit object type code, and a ten-digit sequence number.
/// There is no checksum.
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(long))]
[OpenApiDataType(
    description: "BAG-identificatienummer",
    type: "string",
    format: "bagid",
    example: "0363010123456789",
    pattern: "[0-9]{16}")]
[TypeConverter(typeof(Conversion.BagIdTypeConverter))]
#if NET10_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.BagIdJsonConverter))]
#endif
public readonly partial struct BagId : IXmlSerializable, IFormattable, IEquatable<BagId>, IComparable, IComparable<BagId>
{
    /// <summary>Represents an unknown (but set) BAG-identificatienummer.</summary>
    public static BagId Unknown => new(9_999_999_999_999_999);

    /// <summary>Gets the number of characters of the BAG-identificatienummer (16 when known, 0 otherwise).</summary>
    public int Length => IsKnown ? 16 : 0;

    /// <summary>Gets the gemeentecode (first four digits).</summary>
    public int Gemeente => (int)(m_Value / 1_000_000_000_000L);

    /// <summary>Gets the object type (fifth and sixth digits).</summary>
    public ObjectType Object => (ObjectType)((m_Value / 10_000_000_000L) % 100);

    /// <summary>Gets the volgnummer (last ten digits).</summary>
    public long Volgnummer => m_Value % 10_000_000_000;

    /// <summary>Returns a <see cref="string" /> that represents the BAG-identificatienummer for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the BAG-identificatienummer.</summary>
    /// <param name="format">
    /// The format that describes the formatting. Use <c>F</c> to group the
    /// municipality code, object type code, and sequence number with dots.
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
        Span<char> span = stackalloc char[18];
        TryFormat(span, out var charsWritten, format.AsSpan(), formatProvider);
        return span[..charsWritten].ToString();
    }

    /// <summary>Gets an XML string representation of the BAG-identificatienummer.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the BAG-identificatienummer to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Deserializes the BAG-identificatienummer from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized BAG-identificatienummer.
    /// </returns>
    [Pure]
    public static BagId FromJson(long json) => Create(json);

    /// <summary>Casts the BAG-identificatienummer to a <see cref="long" />.</summary>
    public static explicit operator long(BagId val) => val.m_Value;

    /// <summary>Casts a <see cref="long" /> to a BAG-identificatienummer.</summary>
    public static explicit operator BagId(long val) => Create(val);

    /// <summary>Converts the <see cref="string" /> to <see cref="BagId" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the BAG-identificatienummer to convert.
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
    public static bool TryParse(string? s, IFormatProvider? provider, out BagId result)
        => TryParse(s.AsSpan(), provider, out result);

    /// <summary>Creates a BAG-identificatienummer from a <see cref="long" />.</summary>
    /// <param name="value">
    /// The number describing a BAG-identificatienummer.
    /// </param>
    /// <returns>
    /// The created BAG-identificatienummer.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> is not a valid BAG-identificatienummer.
    /// </exception>
    [Pure]
    public static BagId Create(long value)
        => TryCreate(value)
        ?? throw new ArgumentOutOfRangeException(nameof(value), QowaivMessages.FormatExceptionBagId);

    /// <summary>Tries to create a BAG-identificatienummer from a <see cref="long" />.</summary>
    /// <param name="value">
    /// The number describing a BAG-identificatienummer.
    /// </param>
    /// <returns>
    /// The created BAG-identificatienummer, or null if <paramref name="value"/> is not valid.
    /// </returns>
    [Pure]
    public static BagId? TryCreate(long value)
        => TryCreate(value, out var bag)
        ? bag
        : null;

    private static bool TryCreate(long value, out BagId result)
    {
        if (value is 0)
        {
            result = Empty;
            return true;
        }
        else if (value is 9_999_999_999_999_999)
        {
            result = Unknown;
            return true;
        }
        else if (value is >= 0_001_010_000_000_000 and < 9_999_999_999_999_999
            && IsValidObject((ObjectType)((value / 10_000_000_000L) % 100)))
        {
            result = new(value);
            return true;
        }
        else
        {
            result = Empty;
            return false;
        }
    }
}

#if NETSTANDARD2_0
public readonly partial struct BagId
#else
public readonly partial struct BagId : ISpanParsable<BagId>, ISpanFormattable
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
    {
        charsWritten = 0;
        if (m_Value is 0)
        {
            return true;
        }
        if (m_Value is 9_999_999_999_999_999)
        {
            if (destination.Length is 0)
            {
                return false;
            }
            destination[0] = '?';
            charsWritten = 1;
            return true;
        }

        var grouped = format.Equals("F", StringComparison.OrdinalIgnoreCase);
        var length = grouped ? 18 : 16;
        if (destination.Length < length)
        {
            return false;
        }

        var remaining = m_Value;
        for (var i = length - 1; i >= 0; i--)
        {
            if (grouped && i is 4 or 7)
            {
                destination[i] = '.';
            }
            else
            {
                var next = remaining / 10;
                destination[i] = (char)((remaining - (next * 10)) + '0');
                remaining = next;
            }
        }
        charsWritten = length;
        return true;
    }

    /// <summary>Tries to parse a span of characters into a BAG-identificatienummer.</summary>
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
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BagId result)
    {
        s = s.Trim();
        result = default;

        if (s is { Length: 0 })
        {
            return true;
        }
        else if (TryBagId(s, out var value) && TryCreate(value, out result))
        {
            return true;
        }
        else if (Qowaiv.Unknown.TryParse(s, provider, out _))
        {
            result = Unknown;
            return true;
        }
        else return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryBagId(ReadOnlySpan<char> span, out long value)
    {
        value = 0;
        var length = 0;

        foreach (var c in span)
        {
            if (c.Is_0_9 && length < 16)
            {
                value = (value * 10) + c.Digit;
                length++;
            }
            else if (!c.Is_Markup) return false;
        }
        return length is 16;
    }

    /// <summary>Parses a span of characters into a BAG-identificatienummer.</summary>
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
    public static BagId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s, provider, out var svo)
        ? svo
        : throw Unparsable.ForValue<BagId>(s.ToString(), QowaivMessages.FormatExceptionBagId);
}
