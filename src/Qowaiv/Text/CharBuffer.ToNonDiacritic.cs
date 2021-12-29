namespace Qowaiv.Text;

internal partial class CharBuffer
{
    [Pure]
    public CharBuffer ToNonDiacritic(string ignore = "")
    {
        if (IsEmpty())
        {
            return this;
        }
        var charBuffer = new CharBuffer(Length * 2);

        foreach (var ch in this)
        {
            if (ignore.IndexOf(ch) != NotFound)
            {
                charBuffer.Add(ch);
            }
            else
            {
                var replace = DiacriticSearch.IndexOf(ch);

                if (replace != NotFound)
                {
                    charBuffer.Add(DiacriticReplace[replace]);
                }
                else if (DiacriticLookup.TryGetValue(ch, out var chs))
                {
                    charBuffer.Add(chs);
                }
                else
                {
                    charBuffer.Add(ch);
                }
            }
        }
        return charBuffer;
    }

    private const string DiacriticSearch = /*  outlining */ "ÀÁÂÃÄÅĀĂĄǍǺàáâãäåāăąǎǻÇĆĈĊČƠçćĉċčơÐĎďđÈÉÊËĒĔĖĘĚèéêëēĕėęěÌÍÎÏĨĪĬĮİǏìíîïıĩīĭįǐĴĵĜĞĠĢĝğġģĤĦĥħĶķĸĹĻĽĿŁĺļľŀłÑŃŅŇŊñńņňŉŋÒÓÔÕÖØŌŎŐǑǾðòóôõöøōŏőǒǿŔŖŘŕŗřŚŜŞŠśŝşšŢŤŦţťŧÙÚÛÜŨŪŬŮŰŲƯǓǕǗǙǛùúûüũūŭůűųưǔǖǘǚǜŴŵÝŶŸýÿŷŹŻŽźżž";
    private const string DiacriticReplace = /* outlining */ "AAAAAAAAAAAaaaaaaaaaaaCCCCCCccccccDDddEEEEEEEEEeeeeeeeeeIIIIIIIIIIiiiiiiiiiiJjGGGGggggHHhhKkkLLLLLlllllNNNNNnnnnnnOOOOOOOOOOOooooooooooooRRRrrrSSSSssssTTTtttUUUUUUUUUUUUUUUUuuuuuuuuuuuuuuuuWwYYYyyyZZZzzz";

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
}
