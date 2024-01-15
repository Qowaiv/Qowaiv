﻿using Qowaiv.TestTools.Resx;
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

    public static async Task<string?> de(Country country)
    {
        if (country.Name.Length == 2)
        {
            var lemma = new WikiLemma($"Vorlage:{country.IsoAlpha3Code}", TestCultures.De);
            return await lemma.Transform(DisplayName);
        }
        else return null;

        static string? DisplayName(string str)
        {
            var index = str.LastIndexOf(Prefix.DE);
            if (index > -1)
            {
                var text = str[(index + Prefix.DE.Length)..];
                return text[..text.IndexOf('}')];
            }
            else return null;
        }
    }

    public static async Task<string?> es(Country country)
    {
        var overrides = new Dictionary<Country, string>()
        {
            [Country.BQ] = "Caribe Neerlandés",
            [Country.XK] = "Kósovo",
        };

        if (overrides.TryGetValue(country, out var overidden))
        {
            return overidden;
        }
        else if (country.Name.Length == 2)
        {
            var all = new WikiLemma("ISO 3166-1", TestCultures.Es);
            var x = await all.Content();
            var lemma = new WikiLemma($"Plantilla:{country.IsoAlpha3Code}", TestCultures.Es);
            return await lemma.Transform(DisplayName);
        }
        else return null;

        string? DisplayName(string str)
        {
            var index = str.LastIndexOf(Prefix.ES);
            if (index > -1)
            {
                var text = str[(index + Prefix.ES.Length)..];
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

    public static async Task<string?> fr(Country country)
    {
        if (Lookup.FR.Count == 0)
        {
            foreach (var item in await fr())
            {
                Lookup.FR[Country.Parse(item.Iso2)] = item.Name;
            }
        }

        return Lookup.FR.TryGetValue(country, out var display)
             ? display : null;
    }

    private static async Task<IEnumerable<Display>> fr()
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

        return await lemma.TransformRange(Display);

        IEnumerable<Display> Display(string content)
        {
            var parts = content.Split("|-");

            foreach (var o in overrides)
            {
                yield return new(o.Key, o.Value);
            }

            foreach (var part in parts.Skip(1))
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

    public static async Task<string?> nl(Country country)
    {
        if (country.Name.Length == 2)
        {
            var lemma = new WikiLemma($"Sjabloon:{country.IsoAlpha2Code}", TestCultures.Nl);
            return await lemma.Transform(DisplayName);
        }
        else return null;

        static string? DisplayName(string str)
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
    }

    private record Display(string Iso2, string Name);

    private static class Prefix
    {
        public const string DE = "{{{3|";
        public const string ES = "{{1|";
    }

    private static class Lookup
    {
        public static readonly Dictionary<Country, string> FR = [];
    }
}