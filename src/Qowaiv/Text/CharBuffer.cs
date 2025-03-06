namespace Qowaiv.Text;

internal static class CharBuffer
{
    /// <summary>Gets the max length of an array.</summary>
    /// <remarks>
    /// Array.MaxLength is not available for .NET standard 2.0.
    /// </remarks>
    private const int Array_MaxLength = 0X7FFFFFC7;

    private const int NotFound = -1;

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
            if (!IsMarkup(ch))
            {
                var relace = DiacriticSearch.IndexOf(ch);

                if (relace != NotFound)
                {
                    buffer[length++] = char.ToUpperInvariant(DiacriticReplace[relace]);
                }
                else if (DiacriticLookup.TryGetValue(ch, out var chs))
                {
                    buffer[length++] = char.ToUpperInvariant(chs[0]);
                    buffer[length++] = char.ToUpperInvariant(chs[1]);
                }
                else
                {
                    buffer[length++] = char.ToUpperInvariant(ch);
                }
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
            }
            else
            {
                var relace = DiacriticSearch.IndexOf(ch);

                if (relace != NotFound)
                {
                    buffer[length++] = DiacriticReplace[relace];
                }
                else if (DiacriticLookup.TryGetValue(ch, out var chs))
                {
                    buffer[length++] = chs[0];
                    buffer[length++] = chs[1];
                }
                else
                {
                    buffer[length++] = ch;
                }
            }
        }
        return length;
    }

    private const string DiacriticSearch = /*..*/ "ÀÁÂÃÄÅĀĂĄǍǺàáâãäåāăąǎǻÇĆĈĊČƠçćĉċčơÐĎďđÈÉÊËĒĔĖĘĚèéêëēĕėęěÌÍÎÏĨĪĬĮİǏìíîïıĩīĭįǐĴĵĜĞĠĢĝğġģĤĦĥħĶķĸĹĻĽĿŁĺļľŀłÑŃŅŇŊñńņňŉŋÒÓÔÕÖØŌŎŐǑǾðòóôõöøōŏőǒǿŔŖŘŕŗřŚŜŞŠśŝşšŢŤŦţťŧÙÚÛÜŨŪŬŮŰŲƯǓǕǗǙǛùúûüũūŭůűųưǔǖǘǚǜŴŵÝŶŸýÿŷŹŻŽźżž";
    private const string DiacriticReplace = /*.*/ "AAAAAAAAAAAaaaaaaaaaaaCCCCCCccccccDDddEEEEEEEEEeeeeeeeeeIIIIIIIIIIiiiiiiiiiiJjGGGGggggHHhhKkkLLLLLlllllNNNNNnnnnnnOOOOOOOOOOOooooooooooooRRRrrrSSSSssssTTTtttUUUUUUUUUUUUUUUUuuuuuuuuuuuuuuuuWwYYYyyyZZZzzz";

    private static readonly Dictionary<char, string> DiacriticLookup = new()
    {
        { 'Æ', "AE" },
        { 'Ǽ', "AE" },
        { 'æ', "ae" },
        { 'ǽ', "ae" },
        { 'ß', "sz" },
        { 'Œ', "OE" },
        { 'œ', "oe" },
        { 'Ĳ', "IJ" },
        { 'ĳ', "ij" },
    };

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
