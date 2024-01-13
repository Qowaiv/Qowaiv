using Qowaiv.TestTools.Resx;
using Qowaiv.TestTools.Wikipedia;
using Qowaiv.UnitTests.Statistics;
using System.Xml.Linq;


namespace Qowaiv.TestTools.Generation;

public static class CountryDisplayName
{
    public static async Task<Action> Update(
        string unknown,
        CultureInfo language,
        Func<Country, Task<string?>> getDisplay)
    {
        var resource = new XResourceFile(Data(unknown, Country.Unknown));

        foreach (var country in Country.All)
        {
            var displayName = await getDisplay(country) ?? country.GetDisplayName(language);

            if (displayName != country.EnglishName)
            {
                resource.Add(Data(displayName, country));
            }
        }
        return () => resource.Save(Solution.Root.File($"src/Qowaiv/Globalization/CountryLabels.{language}.resx"));
    }


    public static XResourceFileData Data(string displayName, Country country)
        => new(
            $"{(country.IsUnknown() ? "ZZ" : country.Name)}_DisplayName",
            displayName,
            $"{country.EnglishName} ({country.IsoAlpha2Code})");

    private const string DE_prefix = "{{{3|";
    private const string ES_prefix = "{{1|";

    public static string? DE(string str)
    {
        var index = str.LastIndexOf(DE_prefix);
        if (index > -1)
        {
            var text = str[(index + DE_prefix.Length)..];
            return text[..text.IndexOf('}')];
        }
        else return null;
    }

    public static async Task<string?> ES(Country country)
    {
        var overrides = new Dictionary<Country, string>()
        {
            [Country.BQ] = "Caribe Neerlandés",
            [Country.XK] = "Kósovo",
        };

        if(country == Country.WF)
        {

        }

        if (overrides.TryGetValue(country, out var overidden))
        {
            return overidden;
        }
        else if (country.Name.Length == 2)
        {
            var lemma = new WikiLemma($"Plantilla:{country.IsoAlpha3Code}", TestCultures.Es);
            return await lemma.Transform(DisplayName);
        }
        else return null;

        string? DisplayName(string str)
        {
            var index = str.LastIndexOf(ES_prefix);
            if (index > -1)
            {
                var text = str[(index + ES_prefix.Length)..];
                var display = text[..text.IndexOf('}')];
                if (display.Length > 0)
                {
                    return display.Replace(" Y ", " y ");
                }
                else return null;
            }
            else return null;
        }
    }

    public static string? NL(string str)
    {
        var index = str.LastIndexOf("-VLAG}}&nbsp;") + 1;
        var text = str[index..];

        if (WikiLink.Parse(text).FirstOrDefault() is { } link)
        {
            var display = link.Display;
            display = display[(display.IndexOf('|') + 1)..];
            display = display.Trim('{').Trim('}');
            return display;
        }
        else return null;
    }

    public static async Task<Dictionary<string, string>> FR()
    {
        var lemma = new WikiLemma("ISO 3166-1", TestCultures.Fr);
        var overrides = new Dictionary<string, string>()
        {
            ["EH"] = "Sahara occidental",
            ["MK"] = "Macédoine du Nord",
            ["SZ"] = "Eswatini",
            ["TF"] = "Terres australes françaises",
            ["XK"] = "Kosovo",
        };

        return (await lemma.Transform<IEnumerable<Display>>(Display))!.ToDictionary(c => c.Iso2, c => c.Name);

        IEnumerable<Display> Display(string content)
        {
            var parts = content.Split("|-");

            foreach(var o in overrides)
            {
                yield return new(o.Key, o.Value);
            }

            foreach(var part in parts.Skip(1))
            {
                var codes = part.Split(new[] { "<code>", "</code>" }, StringSplitOptions.None);
                if (codes.Length >= 7 
                    && Regex.Match(codes[4], @"id=""(?<iso>[A-Z]{2})""") is { Success: true } iso)
                {
                    var iso2 = iso.Groups[nameof(iso)].Value;

                    if (!overrides.ContainsKey(iso2) 
                        && Regex.Match(codes[6], "{{(?<match>.+?)}}") is { Success: true } match)
                    {
                        var name = match.Groups[nameof(match)].Value;

                        yield return new(iso2, name);
                    }
                }
            }
        }
    }

    private record Display(string Iso2, string Name);
}
