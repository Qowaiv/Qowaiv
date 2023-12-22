using Qowaiv.Globalization;

namespace Qowaiv.Financial;

[DebuggerDisplay("{DebuggerDisplay}")]
internal class BbanParser(string pattern)
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string Pattern { get; } = pattern;

    public int Length => Pattern.Length;

    public Country Country => Country.Parse(Pattern[..2]);

    [Pure]
    public string? Parse(string str, int start, int id)
    {
        var pos = 2;
        var buffer = Buffer(id);
        var index = start;

        while (index < str.Length && pos < Length)
        {
            var ch = str[index++];
            if (ch <= 'Z' && IsMatch(ch, Pattern[pos]))
            {
                buffer[pos++] = ASCII.Upper(ch);
            }

            // Markup within the ckecksum is not allowed.
            else if (pos == 3 || !IbanParser.IsMarkup(ch))
            {
                return null;
            }
        }

        return IsEndOfString(str, index)
           && CheckLength(buffer, pos) is { } iban
           && Mod97(iban)
               ? Validate(new(iban))
               : null;
    }

    [Pure]
    protected virtual char[] Buffer(int id)
    {
        var buffer = new char[Length];
        buffer[0] = Pattern[0];
        buffer[1] = Pattern[1];
        return buffer;
    }

    [Pure]
    protected virtual char[]? CheckLength(char[] iban, int length)
        => length == Length ? iban : null;

    [Pure]
    protected virtual string? Validate(string iban) => iban;

    [Pure]
    private static bool IsEndOfString(string str, int index)
    {
        while (index < str.Length)
        {
            if (!IbanParser.IsMarkup(str[index++]))
            {
                return false;
            }
        }
        return true;
    }

    [Pure]
    private static bool IsMatch(char ch, char pattern)
    {
        if /*.*/(pattern == 'n') return ASCII.IsDigit(ch);
        else if (pattern == 'a') return ASCII.IsLetter(ch);
        else if (pattern == 'c') return ASCII.IsLetterOrDigit(ch);
        else return ch == pattern;
    }

    [Pure]
    private static bool Mod97(char[] iban)
    {
        var mod = 0;
        for (var i = 0; i < iban.Length; i++)
        {
            // Calculate the first 4 characters (country and checksum) last
            var ch = iban[(i + 4) % iban.Length];
            var index = Index(ch);
            mod *= index > 9 ? 100 : 10;
            mod += index;
            mod %= 97;
        }
        return mod == 1;

        static int Index(char ch)
            => ch <= '9'
                ? ch - '0'
                : ch - 'A' + 10;
    }

    private string DebuggerDisplay => $"{Pattern} ({Length}), {Country}";
}
