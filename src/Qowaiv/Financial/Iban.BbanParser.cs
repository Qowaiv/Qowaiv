using Qowaiv.Globalization;

namespace Qowaiv.Financial;

[DebuggerDisplay("{DebuggerDisplay}")]
internal partial class BbanParser(string pattern)
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

        var iban = IsEndOfString(str, index)
            ? CheckLength(buffer, pos)
            : null;

        return iban is { }
            && Mod97(iban)
            && Validate(iban)
               ? iban
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
    protected virtual string? CheckLength(char[] iban, int length)
        => length == Length ? new(iban) : null;

    /// <summary>Extended validation for specific parsers.</summary>
    [Pure]
    protected virtual bool Validate(string iban) => true;

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

    private string DebuggerDisplay => $"{Pattern} ({Length}), {Country}";
}
