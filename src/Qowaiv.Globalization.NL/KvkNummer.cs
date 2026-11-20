#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

namespace Qowaiv.Globalization.NL;

/// <summary>Represents a KvK-nummer (Kamer van Koophandel).</summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(int))]
[OpenApiDataType(
    description: "KvK-nummer (Kamer van Koophandel)",
    type: "string",
    format: "kvknr",
    example: "08543897",
    pattern: "[0-9]{8}")]
[TypeConverter(typeof(Conversion.KvkNummerTypeConverter))]
#if NET10_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.KvkNummerJsonConverter))]
#endif
public readonly partial struct KvkNummer : IXmlSerializable, IFormattable, IEquatable<KvkNummer>, IComparable, IComparable<KvkNummer>
{
    /// <summary>Represents an unknown (but set) KvK-nummer.</summary>
    public static KvkNummer Unknown => new(int.MaxValue);

    /// <summary>Gets the number of characters of the KvK-nummer (8 when known, 0 otherwise).</summary>
    public int Length => IsKnown ? 8 : 0;

    /// <summary>Returns a <see cref="string" /> that represents the KvK-nummer for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the KvK-nummer.</summary>
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
        else
        {
            Span<char> span = stackalloc char[9];
            TryFormat(span, out var charsWritten, format.AsSpan(), formatProvider);
            return span[..charsWritten].ToString();
        }
    }

    /// <summary>Gets an XML string representation of the KvK-nummer.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the KvK-nummer to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Deserializes the KvK-nummer from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized KvK-nummer.
    /// </returns>
    [Pure]
    public static KvkNummer FromJson(long json) => Create(Cast.ToInt<KvkNummer>(json));

    /// <summary>Casts the KvK-nummer to a <see cref="string" />.</summary>
    public static explicit operator string(KvkNummer val) => val.ToString(CultureInfo.CurrentCulture);

    /// <summary>Casts a <see cref="string" /> to a KvK-nummer.</summary>
    public static explicit operator KvkNummer(string str) => Parse(str, CultureInfo.CurrentCulture);

    /// <summary>Casts the KvK-nummer to a nullable <see cref="int" />; null when not known.</summary>
    public static explicit operator int?(KvkNummer val) => val.IsKnown ? val.m_Value : null;

    /// <summary>Casts a nullable <see cref="int" /> to a KvK-nummer.</summary>
    public static explicit operator KvkNummer(int? val) => Create(val);

    /// <summary>Converts the <see cref="string" /> to <see cref="KvkNummer" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the KvK-nummer to convert.
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
    public static bool TryParse(string? s, IFormatProvider? provider, out KvkNummer result)
        => TryParse(s.AsSpan(), provider, out result);

    /// <summary>Creates a KvK-nummer from an <see cref="int" />.</summary>
    /// <param name="val">
    /// A number describing a KvK-nummer.
    /// </param>
    /// <returns>
    /// The created KvK-nummer.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="val"/> is not a valid KvK-nummer.
    /// </exception>
    [Pure]
    public static KvkNummer Create(int? val)
        => TryCreate(val)
        ?? throw new ArgumentOutOfRangeException(nameof(val), QowaivMessages.FormatExceptionKvkNummer);

    /// <summary>Creates a KvK-nummer from an <see cref="int" />.</summary>
    /// <param name="val">
    /// A number describing a KvK-nummer.
    /// </param>
    /// <returns>
    /// The created KvK-nummer, or null if <paramref name="val"/> is not a valid KvK-nummer.
    /// </returns>
    [Pure]
    public static KvkNummer? TryCreate(int? val) => val switch
    {
        >= 0010_0000 and <= 9999_9999 and int kvk => new(kvk),
        null => Empty,
        _ => null,
    };
}

#if NETSTANDARD2_0
public readonly partial struct KvkNummer
#else
public readonly partial struct KvkNummer : ISpanParsable<KvkNummer>, ISpanFormattable
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
        if (m_Value is int.MaxValue)
        {
            if (destination.Length >= 1)
            {
                destination[charsWritten++] = '?';
                return true;
            }
            else return false;
        }

        charsWritten = format.Equals("F", StringComparison.OrdinalIgnoreCase) ? 9 : 8;

        if (destination.Length < charsWritten)
        {
            charsWritten = 0;
            return false;
        }
        else
        {
            TryFormat(destination, charsWritten);
            return true;
        }
    }

    private void TryFormat(Span<char> destination, int charsWritten)
    {
        var curr = m_Value;

        for (var i = charsWritten - 1; i >= 0; i--)
        {
            if (charsWritten is 9 && i is 4)
            {
                destination[i] = (char)160;
            }
            else
            {
                var next = curr / 10;
                var d = curr - (next * 10);
                destination[i] = (char)(d + '0');
                curr = next;
            }
        }
    }

    /// <summary>Tries to parse a span of characters into a KvK-nummer.</summary>
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
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out KvkNummer result)
    {
        result = default;
        s = s.Trim();

        if (s is { Length: 0 })
        {
            return true;
        }
        else if (TryKvk(s, out var value))
        {
            result = new KvkNummer(value);
            return true;
        }
        else if (Qowaiv.Unknown.IsUnknown(s.ToString()))
        {
            result = Unknown;
            return true;
        }
        else return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryKvk(ReadOnlySpan<char> span, out int kvk)
    {
        var length = 0;
        kvk = 0;

        foreach (var c in span)
        {
            if (c.Is_0_9 && length++ < 8)
            {
                kvk = (kvk * 10) + c.Digit;
            }
            else if (!c.Is_Markup) return false;
        }
        return kvk >= 0010_0000;
    }

    /// <summary>Parses a span of characters into a KvK-nummer.</summary>
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
    public static KvkNummer Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s, provider, out var svo)
        ? svo
        : throw Unparsable.ForValue<KvkNummer>(s.ToString(), QowaivMessages.FormatExceptionKvkNummer);
}
