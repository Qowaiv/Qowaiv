namespace Qowaiv.Text;

internal static class Parser
{
    [Pure]
    public static string Unify(string? chars)
    {
        if (chars == null) return string.Empty;
        else
        {
            var unified = new char[chars.Length * 2];
            var length = 0;

            foreach (var ch in chars.Where(c => !char.IsWhiteSpace(c) && !Markup.Contains(c)))
            {
                if (DiacriticSearch.IndexOf(ch) is var diacritic && diacritic != -1)
                {
                    unified[length++] = DiacriticReplace[diacritic];
                }
                else if (DiacriticLookup.TryGetValue(ch, out var found))
                {
                    unified[length++] = found[0];
                    unified[length++] = found[1];
                }
                else unified[length++] = char.ToUpperInvariant(ch);
            }
            return new(unified, 0, length);
        }
    }

    private const string DiacriticSearch = /*  outlining */ "ÀÁÂÃÄÅĀĂĄǍǺàáâãäåāăąǎǻÇĆĈĊČƠçćĉċčơÐĎďđÈÉÊËĒĔĖĘĚèéêëēĕėęěÌÍÎÏĨĪĬĮİǏìíîïıĩīĭįǐĴĵĜĞĠĢĝğġģĤĦĥħĶķĸĹĻĽĿŁĺļľŀłÑŃŅŇŊñńņňŉŋÒÓÔÕÖØŌŎŐǑǾðòóôõöøōŏőǒǿŔŖŘŕŗřŚŜŞŠśŝşšŢŤŦţťŧÙÚÛÜŨŪŬŮŰŲƯǓǕǗǙǛùúûüũūŭůűųưǔǖǘǚǜŴŵÝŶŸýÿŷŹŻŽźżž";
    private const string DiacriticReplace = /* outlining */ "AAAAAAAAAAAAAAAAAAAAAACCCCCCCCCCCCDDDDEEEEEEEEEEEEEEEEEEIIIIIIIIIIIIIIIIIIIIJJGGGGGGGGHHHHKKKLLLLLLLLLLNNNNNNNNNNNOOOOOOOOOOOOOOOOOOOOOOORRRRRRSSSSSSSSTTTTTTUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUWWYYYYYYZZZZZZ";

    private static readonly Dictionary<char, string> DiacriticLookup = new()
    {
        { 'Æ', "AE" },
        { 'Ǽ', "AE" },
        { 'æ', "AE" },
        { 'ǽ', "AE" },
        { 'ß', "SZ" },
        { 'Œ', "OE" },
        { 'œ', "OE" },
        { 'Ĳ', "IJ" },
        { 'ĳ', "IJ" },
    };

    private static readonly string Markup = "-._"
        + (char)0x00B7 // middle dot
        + (char)0x22C5 // dot operator
        + (char)0x2202 // bullet
        + (char)0x2012 // figure dash / minus
        + (char)0x2013 // en dash
        + (char)0x2014 // em dash
        + (char)0x2015 // horizontal bar
    ;
}
