using Qowaiv.TestTools.Wikipedia;

namespace Qowaiv.TestTools.Generation;

public sealed record Iso3166_1(string DisplayName, string Iso2, string Iso3, int Iso)
{
    public override string ToString() => $"{Iso2}/{Iso3}: {DisplayName} ({Iso:000})";

    // Overrides of Wikipedia: 
    private static readonly Dictionary<string, string> Shorten = new()
    {
        ["Bolivia (Plurinational State of)"] = "Bolivia",
        ["Bonaire, Sint Eustatius and Saba"] = "Caribbean Netherlands",
        ["Iran (Islamic Republic of)"] = "Iran",
        ["Korea (Democratic People's Republic of)"] = "North Korea",
        ["Korea, Republic of"] = "South Korea",
        ["Lao People's Democratic Republic"] = "Laos",
        ["Micronesia (Federated States of)"] = "Micronesia",
        ["Moldova, Republic of"] = "Moldova",
        ["Netherlands, Kingdom of the"] = "Netherlands",
        ["Palestine, State of"] = "Palestine",
        ["Russian Federation"] = "Russia",
        ["<!--DO NOT CHANGE-->Taiwan, Province of China<!--This is the name used in ISO 3166: https://www.iso.org/obp/ui/#iso:code:3166:TW. If you disagree with this naming, contact the ISO 3166/MA; we must follow the published standard for this article.-->"] = "Taiwan",
        ["Tanzania, United Republic of"] = "Tanzania",
        ["United Kingdom of Great Britain and Northern Ireland"] = "United Kingdom",
        ["United States of America"] = "United States",
        ["Venezuela (Bolivarian Republic of)"] = "Venezuela",
        ["<!--DO NOT CHANGE-->{{not a typo|Sao Tome and Principe}}<!-- Do not change this to \"São Tomé and Príncipe\" unless https://www.iso.org/obp/ui/#iso:code:3166:ST changes to that spelling. If you disagree with the lack of diacritics, contact the ISO 3166/MA; we must follow the published standard for this article. -->"] = "Sao Tome and Principe",
    };

    public static IEnumerable<Iso3166_1> Parse(string str)
    {
        foreach (var line in str.Split("{{flagdeco|").Skip(1))
        {
            var parts = line.Split("mono|");

            if (parts.Length >= 4)
            {
                var link = WikiLink.Parse(parts[0]).First();
                var name = Wiki.RemoveInParentheses(link.Display);

                var a2 = parts[1][..2];
                var a3 = parts[2][..3];
                var nc = parts[3][..3];

                name = Shorten.TryGetValue(name, out var shorten) ? shorten : name;

                yield return new(name, a2, a3, int.Parse(nc));
            }
        }
    }
}
