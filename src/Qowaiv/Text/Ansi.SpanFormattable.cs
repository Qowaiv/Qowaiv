namespace Qowaiv.Text;

public readonly partial struct Ansi
{
    /// <inheritdoc />
    [Pure]
    public override string ToString()
    {
        Span<char> buffer = stackalloc char[80];
        TryFormat(buffer, out var length);
        return buffer[..length].ToString();
    }

    /// <summary>Tries to format the ANSI code into the provided span of characters.</summary>
    private bool TryFormat(Span<char> destination, out int charsWritten)
    {
        charsWritten = 0;
        if (destination.Length < 4) return false;

        Span<byte> codes = stackalloc byte[24];
        var count = Codes(codes);

        destination[0] = '\x1b';
        destination[1] = '[';
        var pos = 2;

        if (!Write(codes[0], destination)) return false;

        for (var i = 1; i < count; i++)
        {
            if (pos >= destination.Length) return false;
            destination[pos++] = ';';
            if (!Write(codes[i], destination)) return false;
        }

        destination[pos++] = 'm';

        charsWritten = pos;
        return true;

        // Returns false if the code plus at least one extra character can not be written.
        bool Write(byte code, Span<char> destination)
        {
            if (code >= 100)
            {
                if (pos + 3 >= destination.Length) return false;
                var hun = code / 100;
                var ten = (code - (hun * 100)) / 10;
                var one = code - (hun * 100) - (ten * 10);
                destination[pos++] = (char)('0' + hun);
                destination[pos++] = (char)('0' + ten);
                destination[pos++] = (char)('0' + one);
            }
            else if (code >= 10)
            {
                if (pos + 2 >= destination.Length) return false;
                var ten = code / 10;
                var one = code - (ten * 10);
                destination[pos++] = (char)('0' + ten);
                destination[pos++] = (char)('0' + one);
            }
            else
            {
                if (pos + 1 >= destination.Length) return false;
                destination[pos++] = (char)('0' + code);
            }
            return true;
        }
    }

    /// <summary>Gets the SGR codes (in canonical order) that represent the instruction.</summary>
    /// <returns>
    /// The numbers of codes added to the span.
    /// </returns>
    [Pure]
    private int Codes(Span<byte> codes)
    {
        var length = 0;
        var styles = m_Styles & Bits.Style;

#if NET10_0_OR_GREATER
        // Jump from set bit to set bit (lowest first, so the order is ascending).
        while (styles is not 0)
        {
            codes[length++] = (byte)BitOperations.TrailingZeroCount(styles);
            styles &= styles - 1;
        }
#else
        byte code = 0;

        while (styles is not 0)
        {
            if ((styles & 1) is not 0) codes[length++] = code;
            code++;
            styles >>= 1;
        }
#endif

        if (length is 0 && (m_Styles & (Bits.Fore.Set | Bits.Back.Set)) is 0) codes[length++] = 0;

        length = Color(codes, length, m_Styles & (Bits.Fore.Set | Bits.Fore.Rgb | Bits.Fore.Pal), Bits.Fore.Rgb, Bits.Fore.Pal, m_Fore, 38);

        return Color(codes, length, m_Styles & (Bits.Back.Set | Bits.Back.Rgb | Bits.Back.Pal), Bits.Back.Rgb, Bits.Back.Pal, m_Back, 48);
    }

    /// <summary>Adds the color instruction to the codes span.</summary>
    /// <returns>
    /// The updated length.
    /// </returns>
    [Pure]
    private static int Color(Span<byte> codes, int length, uint kind, uint rgb, uint palette, uint color, byte extended)
    {
        if (kind is 0) return length;

        if ((kind & rgb) is not 0)
        {
            codes[length++] = extended;
            codes[length++] = 2;
            codes[length++] = (byte)(color >> 16);
            codes[length++] = (byte)(color >> 8);
            codes[length++] = (byte)color;
        }
        else if ((kind & palette) is not 0)
        {
            codes[length++] = extended;
            codes[length++] = 5;
            codes[length++] = (byte)color;
        }
        else
        {
            codes[length++] = (byte)color;
        }
        return length;
    }
}

#if !NETSTANDARD2_0
public readonly partial struct Ansi : ISpanFormattable
{
    /// <inheritdoc />
    [Pure]
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    /// <summary>Tries to format the value of the current instance into the provided span of characters.</summary>
    /// <remarks>
    /// The <paramref name="format"/> and <paramref name="provider"/> are ignored.
    /// </remarks>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => TryFormat(destination, out charsWritten);
}
#endif
