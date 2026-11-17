#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

namespace Qowaiv.Globalization.NL;

/// <summary>Represents a kadastrale gemeente via its AKR-code.</summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(ulong))]
[OpenApiDataType(
    description: "kadastrale gemeente (in the Netherlands)",
    type: "string",
    format: "akr-code",
    example: "DEL00",
    pattern: "[A-Z]{3}[0-9]{2}")]
[TypeConverter(typeof(Conversion.KadastraleGemeenteTypeConverter))]
#if !NETSTANDARD2_0
[System.Text.Json.Serialization.JsonConverter(typeof(Json.KadastraleGemeenteJsonConverter))]
#endif
public readonly partial struct KadastraleGemeente : IXmlSerializable, IFormattable, IEquatable<KadastraleGemeente>, IComparable, IComparable<KadastraleGemeente>
{
    /// <summary>Represents an unknown (but set) kadastrale gemeente.</summary>
    public static KadastraleGemeente Unknown => new("?");

    /// <summary>Gets the number of characters of kadastrale gemeente.</summary>
    public int Length => IsKnown ? m_Value!.Length : 0;

    /// <summary>Returns a <see cref="string" /> that represents the kadastrale gemeente for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the kadastrale gemeente.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
    /// </param>
    /// <param name="formatProvider">
    /// The format provider.
    /// </param>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => StringFormatter.TryApplyCustomFormatter(format, this, formatProvider, out string formatted)
        ? formatted
        : m_Value ?? string.Empty;

    /// <summary>Gets an XML string representation of the kadastrale gemeente.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the kadastrale gemeente to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Casts the kadastrale gemeente to a <see cref="string" />.</summary>
    public static explicit operator string(KadastraleGemeente val) => val.ToString(CultureInfo.CurrentCulture);

    /// <summary>Casts a <see cref="string" /> to a kadastrale gemeente.</summary>
    public static explicit operator KadastraleGemeente(string str) => Parse(str, CultureInfo.CurrentCulture);

    /// <summary>Converts the <see cref="string" /> to <see cref="KadastraleGemeente" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the kadastrale gemeente to convert.
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
    public static bool TryParse(string? s, IFormatProvider? provider, out KadastraleGemeente result)
        => TryParse(s.AsSpan(), provider, out result);
}

#if NETSTANDARD2_0
public readonly partial struct KadastraleGemeente
#else
public readonly partial struct KadastraleGemeente : ISpanParsable<KadastraleGemeente>, ISpanFormattable
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
        if (m_Value is null)
        {
            charsWritten = 0;
            return true;
        }
        else if (m_Value.AsSpan().TryCopyTo(destination))
        {
            charsWritten = m_Value.Length;
            return true;
        }
        else
        {
            charsWritten = 0;
            return false;
        }
    }

    /// <summary>Tries to parse a span of characters into a Burgerservicenummer.</summary>
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
    /// <returs>
    /// true if s was successfully parsed; otherwise, false.
    /// </returs>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out KadastraleGemeente result)
    {
        s = s.Trim();
        result = Empty;

        if (s.Length is 0)
        {
            return true;
        }
        else if (s is
        [
            { is_a_zA_Z: true },
            { is_a_zA_Z: true },
            { is_a_zA_Z: true },
            { Is_0_9: true },
            { Is_0_9: true },
        ])
        {
            result = new(s.ASCII_Upper());
            return true;
        }
        else if (Qowaiv.Unknown.IsUnknown(s.ToString()))
        {
            result = Unknown;
            return true;
        }
        else return false;
    }

    /// <summary>Parses a span of characters into a Burgerservicenummer.</summary>
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
    public static KadastraleGemeente Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s, provider, out var svo)
        ? svo
        : throw Unparsable.ForValue<KadastraleGemeente>(s.ToString(), QowaivMessages.FormatExceptionKadastraleGemeente);
}
