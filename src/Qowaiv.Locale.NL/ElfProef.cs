namespace Qowaiv.Locale.NL;

internal static class ElfProef
{
    /// <summary>Validates a 9-digit identifier.</summary>
    /// <remarks>
    /// ABCDEFGHI:
    /// (9*A + 8*B + 7*C + 6*D + 5*E + 4*F + 3*G + 2*H - 1*I) % 11 = 0.
    /// </remarks>
    [Pure]
    public static bool IsValid(long num)
    {
        if (num is < 0010_00_000 or > 9999_99_999) return false;

        var cur = num / 10;
        var sum = (cur * 10) - num;

        for (var pos = 2; pos <= 9; pos++)
        {
            var nxt = cur / 10;
            sum += (cur - (nxt * 10)) * pos;
            cur = nxt;
        }
        return sum % 11 == 0;
    }

    public static bool TryFormat(long value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (value is 0)
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
            TryFormat(value, destination, charsWritten);
            return true;
        }
    }

    private static void TryFormat(long value, Span<char> destination, int charsWritten)
    {
        var curr = value;

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

    /// <summary>Tries to parse a 9-digit identifier.</summary>
    public static bool TryParse(ReadOnlySpan<char> span, out long value)
    {
        var length = 1;
        var pos = span.Length - 1;
        var factor = 10;
        var c = span[pos--];
        value = c.Digit;

        if (!c.Is_0_9) return false;

        // Last digit is * -1;
        var check = -value;

        // By reading from right to left we can both parse the number and
        // create the checksum at the same time.
        while (pos >= 0)
        {
            c = span[pos--];

            // Not more than 9 digits
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
}
