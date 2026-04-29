namespace Qowaiv.Text;

internal static class CharBuffer
{
    /// <summary>Gets the max length of an array.</summary>
    /// <remarks>
    /// Array.MaxLength is not available for .NET standard 2.0.
    /// </remarks>
    private const int Array_MaxLength = 0X7FFFFFC7;

    [Pure]
    public static int BufferSize(this ReadOnlySpan<char> span)
    {
        var size = span.Length * 2;
        return (uint)size > Array_MaxLength ? Array_MaxLength : size;
    }

    [CollectionMutation]
    public static int Unify(this ReadOnlySpan<char> span, Span<char> buffer)
    {
        var length = 0;

        foreach (var ch in span)
        {
            if (IsMarkup(ch)) continue;

            if (!ASCII.IsAscii(ch) && DiacriticLookup.TryGetValue(ch, out var chs))
            {
                buffer[length++] = char.ToUpperInvariant(chs[0]);

                if (chs.Length == 2) buffer[length++] = char.ToUpperInvariant(chs[1]);
            }
            else
            {
                buffer[length++] = char.ToUpperInvariant(ch);
            }
        }
        return length;
    }

    [CollectionMutation]
    public static int ToNonDiacritic(this ReadOnlySpan<char> span, Span<char> buffer, string ignore = "")
    {
        var length = 0;

        foreach (var ch in span)
        {
            if (ignore.Contains(ch))
            {
                buffer[length++] = ch;
                continue;
            }

            if (!ASCII.IsAscii(ch) && DiacriticLookup.TryGetValue(ch, out var chs))
            {
                buffer[length++] = chs[0];
                if (chs.Length == 2) buffer[length++] = chs[1];
            }
            else
            {
                buffer[length++] = ch;
            }
        }
        return length;
    }

    private static readonly Dictionary<char, string> DiacriticLookup = new()
    {
        ['À'] = "A",
        ['Á'] = "A",
        ['Â'] = "A",
        ['Ã'] = "A",
        ['Ä'] = "A",
        ['Å'] = "A",
        ['Ā'] = "A",
        ['Ă'] = "A",
        ['Ą'] = "A",
        ['Ǎ'] = "A",
        ['Ǻ'] = "A",
        ['à'] = "a",
        ['á'] = "a",
        ['â'] = "a",
        ['ã'] = "a",
        ['ä'] = "a",
        ['å'] = "a",
        ['ā'] = "a",
        ['ă'] = "a",
        ['ą'] = "a",
        ['ǎ'] = "a",
        ['ǻ'] = "a",
        ['Ç'] = "C",
        ['Ć'] = "C",
        ['Ĉ'] = "C",
        ['Ċ'] = "C",
        ['Č'] = "C",
        ['Ơ'] = "C",
        ['ç'] = "c",
        ['ć'] = "c",
        ['ĉ'] = "c",
        ['ċ'] = "c",
        ['č'] = "c",
        ['ơ'] = "c",
        ['Ð'] = "D",
        ['Ď'] = "D",
        ['ď'] = "d",
        ['đ'] = "d",
        ['È'] = "E",
        ['É'] = "E",
        ['Ê'] = "E",
        ['Ë'] = "E",
        ['Ē'] = "E",
        ['Ĕ'] = "E",
        ['Ė'] = "E",
        ['Ę'] = "E",
        ['Ě'] = "E",
        ['è'] = "e",
        ['é'] = "e",
        ['ê'] = "e",
        ['ë'] = "e",
        ['ē'] = "e",
        ['ĕ'] = "e",
        ['ė'] = "e",
        ['ę'] = "e",
        ['ě'] = "e",
        ['Ì'] = "I",
        ['Í'] = "I",
        ['Î'] = "I",
        ['Ï'] = "I",
        ['Ĩ'] = "I",
        ['Ī'] = "I",
        ['Ĭ'] = "I",
        ['Į'] = "I",
        ['İ'] = "I",
        ['Ǐ'] = "I",
        ['ì'] = "i",
        ['í'] = "i",
        ['î'] = "i",
        ['ï'] = "i",
        ['ı'] = "i",
        ['ĩ'] = "i",
        ['ī'] = "i",
        ['ĭ'] = "i",
        ['į'] = "i",
        ['ǐ'] = "i",
        ['Ĵ'] = "J",
        ['ĵ'] = "j",
        ['Ĝ'] = "G",
        ['Ğ'] = "G",
        ['Ġ'] = "G",
        ['Ģ'] = "G",
        ['ĝ'] = "g",
        ['ğ'] = "g",
        ['ġ'] = "g",
        ['ģ'] = "g",
        ['Ĥ'] = "H",
        ['Ħ'] = "H",
        ['ĥ'] = "h",
        ['ħ'] = "h",
        ['Ķ'] = "K",
        ['ķ'] = "k",
        ['ĸ'] = "k",
        ['Ĺ'] = "L",
        ['Ļ'] = "L",
        ['Ľ'] = "L",
        ['Ŀ'] = "L",
        ['Ł'] = "L",
        ['ĺ'] = "l",
        ['ļ'] = "l",
        ['ľ'] = "l",
        ['ŀ'] = "l",
        ['ł'] = "l",
        ['Ñ'] = "N",
        ['Ń'] = "N",
        ['Ņ'] = "N",
        ['Ň'] = "N",
        ['Ŋ'] = "N",
        ['ñ'] = "n",
        ['ń'] = "n",
        ['ņ'] = "n",
        ['ň'] = "n",
        ['ŉ'] = "n",
        ['ŋ'] = "n",
        ['Ò'] = "O",
        ['Ó'] = "O",
        ['Ô'] = "O",
        ['Õ'] = "O",
        ['Ö'] = "O",
        ['Ø'] = "O",
        ['Ō'] = "O",
        ['Ŏ'] = "O",
        ['Ő'] = "O",
        ['Ǒ'] = "O",
        ['Ǿ'] = "O",
        ['ð'] = "o",
        ['ò'] = "o",
        ['ó'] = "o",
        ['ô'] = "o",
        ['õ'] = "o",
        ['ö'] = "o",
        ['ø'] = "o",
        ['ō'] = "o",
        ['ŏ'] = "o",
        ['ő'] = "o",
        ['ǒ'] = "o",
        ['ǿ'] = "o",
        ['Ŕ'] = "R",
        ['Ŗ'] = "R",
        ['Ř'] = "R",
        ['ŕ'] = "r",
        ['ŗ'] = "r",
        ['ř'] = "r",
        ['Ś'] = "S",
        ['Ŝ'] = "S",
        ['Ş'] = "S",
        ['Š'] = "S",
        ['ś'] = "s",
        ['ŝ'] = "s",
        ['ş'] = "s",
        ['š'] = "s",
        ['Ţ'] = "T",
        ['Ť'] = "T",
        ['Ŧ'] = "T",
        ['ţ'] = "t",
        ['ť'] = "t",
        ['ŧ'] = "t",
        ['Ù'] = "U",
        ['Ú'] = "U",
        ['Û'] = "U",
        ['Ü'] = "U",
        ['Ũ'] = "U",
        ['Ū'] = "U",
        ['Ŭ'] = "U",
        ['Ů'] = "U",
        ['Ű'] = "U",
        ['Ų'] = "U",
        ['Ư'] = "U",
        ['Ǔ'] = "U",
        ['Ǖ'] = "U",
        ['Ǘ'] = "U",
        ['Ǚ'] = "U",
        ['Ǜ'] = "U",
        ['ù'] = "u",
        ['ú'] = "u",
        ['û'] = "u",
        ['ü'] = "u",
        ['ũ'] = "u",
        ['ū'] = "u",
        ['ŭ'] = "u",
        ['ů'] = "u",
        ['ű'] = "u",
        ['ų'] = "u",
        ['ư'] = "u",
        ['ǔ'] = "u",
        ['ǖ'] = "u",
        ['ǘ'] = "u",
        ['ǚ'] = "u",
        ['ǜ'] = "u",
        ['Ŵ'] = "W",
        ['ŵ'] = "w",
        ['Ý'] = "Y",
        ['Ŷ'] = "Y",
        ['Ÿ'] = "Y",
        ['ý'] = "y",
        ['ÿ'] = "y",
        ['ŷ'] = "y",
        ['Ź'] = "Z",
        ['Ż'] = "Z",
        ['Ž'] = "Z",
        ['ź'] = "z",
        ['ż'] = "z",
        ['ž'] = "z",
        ['Æ'] = "AE",
        ['Ǽ'] = "AE",
        ['æ'] = "ae",
        ['ǽ'] = "ae",
        ['ß'] = "sz",
        ['Œ'] = "OE",
        ['œ'] = "oe",
        ['Ĳ'] = "IJ",
        ['ĳ'] = "ij",
    };

#pragma warning disable S1067 // Expressions should not be too complex
    [Pure]
    private static bool IsMarkup(char ch)
        => char.IsWhiteSpace(ch)
        || ch is '-'
        or '.'
        or '_'
        or (char)0x00B7 // middle dot
        or (char)0x22C5 // dot operator
        or (char)0x2202 // bullet
        or (char)0x2012 // figure dash / minus
        or (char)0x2013 // en dash
        or (char)0x2014 // em dash
        or (char)0x2015 // horizontal bar
    ;
}
