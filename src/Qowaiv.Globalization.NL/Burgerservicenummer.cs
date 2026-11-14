#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

namespace Qowaiv.Globalization.NL;

/// <summary>Representeert een Burgerservicenummer (BSN).</summary>
/// <remarks>
/// Het Burgerservicenummer (BSN) is op 26 november 2007 ingevoerd
/// in Nederland. Het BSN is een uniek persoonsnummer dat als nummer gelijk
/// is aan het sofinummer. Het heeft echter een ander wettelijk kader
/// waardoor een breder gebruik mogelijk is.
///
/// Het burgerservicenummer bestaat uit 9 cijfers en voldoet aan een variant
/// op de zogenaamde elfproef: Als het burgerservicenummer wordt voorgesteld
/// door ABCDEFGHI, dan moet gelden:<br/>
/// (9*A + 8*B + 7*C + 6*D + 5*E + 4*F + 3*G + 2*H - 1*I) % 11 = 0
///
/// Indien bij de communicatie tussen burger en overheid een persoonsgebonden
/// nummer wordt gebruikt dan mag dat alleen het BSN zijn.
///
/// Iedereen die een meervoudige relatie met de overheid heeft, goed te
/// identificeren is en opgenomen is in de Gemeentelijke Basisadministratie
/// Persoonsgegevens (GBA) of in de (nog te realiseren) Registratie Niet
/// Ingezetenen (RNI) krijgt een BSN.
///
/// Bij gegevensuitwisseling tussen overheden mag alleen het BSN worden
/// gebruikt (met uitzondering van het A-nummer bij de uitwisseling van
/// GBA-gegevens).
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
[Serializable]
[SingleValueObject(SingleValueStaticOptions.All, typeof(long))]
[OpenApiDataType(description: "Burgerservicenummer", type: "string", format: "bsn", example: "111222333", pattern: "[0-9]{8,9}")]
[TypeConverter(typeof(Conversion.BurgerservicenummerTypeConverter))]
#if NET10_0_OR_GREATER
[System.Text.Json.Serialization.JsonConverter(typeof(Json.BurgerservicenummerJsonConverter))]
#endif
public readonly partial struct Burgerservicenummer : IXmlSerializable, IFormattable, IEquatable<Burgerservicenummer>, IComparable, IComparable<Burgerservicenummer>
{
    /// <summary>Represents an unknown (but set) Burgerservicenummer (9999.99.999).</summary>
    public static Burgerservicenummer Unknown => new(9999_99_999);

    /// <summary>Returns a <see cref="string" /> that represents the Burgerservicenummer for DEBUG purposes.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.DebuggerDisplay("{0:F}");

    /// <summary>Returns a formatted <see cref="string" /> that represents the Burgerservicenummer.</summary>
    /// <param name="format">
    /// The format that this describes the formatting.
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

    /// <summary>Gets an XML string representation of the Burgerservicenummer.</summary>
    [Pure]
    private string ToXmlString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>Serializes the Burgerservicenummer to a JSON node.</summary>
    /// <returns>
    /// The serialized JSON string.
    /// </returns>
    [Pure]
    public string? ToJson() => m_Value == default ? null : ToString(CultureInfo.InvariantCulture);

    /// <summary>Deserializes the Burgerservicenummer from a JSON number.</summary>
    /// <param name="json">
    /// The JSON number to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized Burgerservicenummer.
    /// </returns>
    [Pure]
    public static Burgerservicenummer FromJson(long json) => Create(json);

    /// <summary>Casts the Burgerservicenummer to a <see cref="string" />.</summary>
    public static explicit operator string(Burgerservicenummer val) => val.ToString(CultureInfo.CurrentCulture);

    /// <summary>Casts a <see cref="string" /> to a Burgerservicenummer.</summary>
    public static explicit operator Burgerservicenummer(string str) => Parse(str, CultureInfo.CurrentCulture);

    /// <summary>Casts the Burgerservicenummer to a <see cref="ulong" />.</summary>
    public static explicit operator long(Burgerservicenummer val) => val.m_Value;

    /// <summary>Casts a <see cref="ulong" /> to a Burgerservicenummer.</summary>
    public static explicit operator Burgerservicenummer(long val) => Create(val);

    /// <summary>Converts the <see cref="string" /> to <see cref="Burgerservicenummer" />.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">
    /// A string containing the Burgerservicenummer to convert.
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
    public static bool TryParse(string? s, IFormatProvider? provider, out Burgerservicenummer result)
        => TryParse(s.AsSpan(), provider, out result);

    /// <summary>Tries to create a Burgerservicenummer.</summary>
    /// <param name="value">
    /// The number describing a Burgerservicenummer.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Not a Burgerservicenummer. 
    /// </exception>
    [Pure]
    public static Burgerservicenummer Create(long value)
        => TryCreate(value)
        ?? throw new ArgumentOutOfRangeException(nameof(value), QowaivMessages.FormatExceptionBurgerservicenummer);

    /// <summary>Tries to create a Burgerservicenummer.</summary>
    /// <param name="value">
    /// The number describing a Burgerservicenummer.
    /// </param>
    /// <returns>
    /// A Burgerservicenummer when valid, otherwise null.
    /// </returns>
    [Pure]
    public static Burgerservicenummer? TryCreate(long value)
        => IsValid(value)
        ? new(value)
        : null;

    /// <summary>Valideert een Burgerservicenummer.</summary>
    /// <remarks>
    /// Het burgerservicenummer bestaat uit 9 cijfers en voldoet aan een variant
    /// op de zogenaamde elfproef: Als het burgerservicenummer wordt voorgesteld
    /// door ABCDEFGHI, dan moet gelden:
    /// (9*A + 8*B + 7*C + 6*D + 5*E + 4*F + 3*G + 2*H - 1*I) % 11 = 0.
    /// </remarks>
    [Pure]
    private static bool IsValid(long bsn)
    {
        if (bsn is < 0010_00_000 or > 9999_99_999) return false;

        var cur = bsn / 10;
        var sum = (cur * 10) - bsn;

        for (var pos = 2; pos <= 9; pos++)
        {
            var nxt = cur / 10;
            sum += (cur - (nxt * 10)) * pos;
            cur = nxt;
        }
        return sum % 11 == 0;
    }
}

#if NETSTANDARD2_0
public readonly partial struct Burgerservicenummer
#else
public readonly partial struct Burgerservicenummer : ISpanParsable<Burgerservicenummer>, ISpanFormattable
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
        if (m_Value is 0)
        {
            charsWritten = 0;
            return true;
        }

        charsWritten = format.Equals("F", StringComparison.OrdinalIgnoreCase) ? 11 : 9;

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
            if (charsWritten is 11 && i is 4 or 7)
            {
                destination[i] = '.';
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
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Burgerservicenummer result)
    {
        result = default;
        s = s.Trim();

        if (s is { Length: 0 })
        {
            return true;
        }
        else if (TryBurgerservicenummer(s, out var value))
        {
            result = new Burgerservicenummer(value);
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
    private static bool TryBurgerservicenummer(ReadOnlySpan<char> span, out long value)
    {
        var length = 1;
        var pos = span.Length - 1;
        var factor = 10;
        var c = span[pos--];
        value = c.Digit;

        if (!c.Is_0_9) return false;

        // Laatste digit is * -1;
        var check = -value;

        // By reading from right to left we can both parse the number and
        // create the checksum at the same time.
        while (pos >= 0)
        {
            c = span[pos--];

            // Not more then 9 digits
            if (c.Is_0_9 && ++length <= 9)
            {
                var digit = c.Digit;
                value += digit * factor;
                check += digit * length;
                factor *= 10;
            }
            else if (!c.Is_Markup) return false;
        }
        return value >= 0010_00_000
            && (check % 11 is 0 || check is 9999_99_999);
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
    public static Burgerservicenummer Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s, provider, out var bsn)
        ? bsn
        : throw Unparsable.ForValue<Burgerservicenummer>(s.ToString(), QowaivMessages.FormatExceptionBurgerservicenummer);
}
