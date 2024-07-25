using Qowaiv.Globalization;

namespace Qowaiv.Financial;

[DebuggerDisplay("{Pattern} ({Length}), {Country}")]
internal partial class BbanParser(string pattern)
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string Pattern { get; } = pattern;

    public int Length => Pattern.Length;

    public Country Country => Country.Parse(Pattern[..2]);

    [Pure]
    public string? Parse(CharSpan str, int id)
        => Parse(str, Buffer(id)) is { } iban
        && Mod97(iban)
        && Validate(iban)
            ? iban
            : null;

    [Pure]
    private string? Parse(CharSpan span, Chars buffer)
    {
        while (span.NotEmpty && buffer.Length < Length)
        {
            var ch = span.First;
            if (ch <= 'Z' && IsMatch(ch, Pattern[buffer.Length]))
            {
                buffer += ASCII.Upper(ch);
            }

            // Markup within the ckecksum is not allowed.
            else if (buffer.Length == 3 || !IbanParser.IsMarkup(ch))
            {
                return null;
            }
            span++;
        }

        return IsEndOfString(span)
            ? CheckLength(buffer)
            : null;
    }

    [Pure]
    protected virtual Chars Buffer(int id)
        => Chars.Init(Length)
        + Pattern[0]
        + Pattern[1];

    [Pure]
    protected virtual string? CheckLength(Chars iban)
        => iban.Length == Length ? iban.ToString() : null;

    /// <summary>Extended validation for specific parsers.</summary>
    [Pure]
    protected virtual bool Validate(string iban) => true;

    [Pure]
    private static bool IsEndOfString(CharSpan span)
    {
        while (span.NotEmpty)
        {
            if (!IbanParser.IsMarkup(span.First))
            {
                return false;
            }
            span++;
        }
        return true;
    }

    [Pure]
    private static bool IsMatch(char ch, char pattern) => pattern switch
    {
        'n' => ASCII.IsDigit(ch),
        'a' => ASCII.IsLetter(ch),
        'c' => ASCII.IsLetterOrDigit(ch),
        _ => pattern == ch,
    };
}
