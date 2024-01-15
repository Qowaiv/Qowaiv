using Qowaiv.TestTools.Resx;
using Qowaiv.TestTools.Wikipedia;

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
            displayName.Trim(),
            $"{country.EnglishName} ({country.IsoAlpha2Code})");

    public static async Task<string?> ar(Country country)
    {
        if (Lookup.AR.Count == 0)
        {
            foreach (var item in await ar())
            {
                Lookup.AR[Country.Parse(item.Iso2)] = item.Name;
            }
        }
        return Lookup.AR.TryGetValue(country, out var display)
                  ? display : null;
    }
    
    private static async Task<IEnumerable<Display>> ar()
    {
        var lemma = new WikiLemma("قائمة_الدول_حسب_المعيار_الدولي_أيزو_3166-1", TestCultures.ar);
        var overrides = new Dictionary<string, string>()
        {
            ["XK"] = "كوسوفو",
        };

        return await lemma.TransformRange(Display);

        IEnumerable<Display> Display(string content)
        {
            var parts = content.Split("|-");

            foreach (var o in overrides)
            {
                yield return new(o.Key, o.Value);
            }

            foreach (var part in parts.Skip(1).Select(p => p.Trim('\n')))
            {
                var cols = part.Split("||");
                if (cols.Length == 5)
                {
                    var iso2 = cols[0].TrimStart('|').Trim();
                    var name = cols[3].TrimStart('{').TrimEnd('}');
                    var pipe = name.IndexOf('|');
                    yield return new(iso2, pipe == -1 ? name : name[..pipe]);
                }
            }
        }
    }

    public static async Task<string?> de(Country country)
    {
        var overrides = new Dictionary<Country, string>()
        {
            [Country.BQ] = "Karibische Niederlande",
            [Country.BV] = "Bouvetinsel",
            [Country.HM] = "Heard und McDonaldinseln",
            [Country.SJ] = "Svalbard und Jan Mayen",
            [Country.UM] = "United States Minor Outlying Islands",
            [Country.XK] = "Kosovo",
        };

        if (overrides.TryGetValue(country, out var display))
        {
            return display;
        }
        else if (country.Name.Length == 2)
        {
            var lemma = new WikiLemma($"Vorlage:{country.IsoAlpha3Code}", TestCultures.de);
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
            var all = new WikiLemma("ISO 3166-1", TestCultures.es);
            var x = await all.Content();
            var lemma = new WikiLemma($"Plantilla:{country.IsoAlpha3Code}", TestCultures.es);
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
        var lemma = new WikiLemma("ISO 3166-1", TestCultures.fr);
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

    public static Task<string?> it(Country country)
    {
        return Task.FromResult<string?>(country.GetDisplayName(TestCultures.it));
    }

    public static Task<string?> ja(Country country)
    {
        return Task.FromResult<string?>(country.GetDisplayName(TestCultures.ja));
    }

    public static async Task<string?> nl(Country country)
    {
        if (country.Name.Length == 2)
        {
            var lemma = new WikiLemma($"Sjabloon:{country.IsoAlpha2Code}", TestCultures.nl);
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

    public static async Task<string?> pt(Country country)
    {
        if (Lookup.PT.Count == 0)
        {
            foreach (var item in await pt())
            {
                Lookup.PT[Country.Parse(item.Iso2)] = item.Name;
            }
        }
        return Lookup.PT.TryGetValue(country, out var display)
                  ? display : null;
    }
    private static async Task<IEnumerable<Display>> pt()
    {
        var lemma = new WikiLemma("Comparação entre códigos de países COI, FIFA, e ISO 3166", TestCultures.pt);
        var overrides = new Dictionary<string, string>()
        {
            ["BQ"] = "Países Baixos Caribenhos",
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

            foreach (var part in parts.Skip(1).Select(p => p))
            {
                var cols = part.Split("||");
                if (cols.Length >= 6)
                {
                    var iso2 = cols[4].Trim();
                    var name = WikiLink.Parse(cols[5]).First();
                    yield return new(iso2, name.Display);
                }
            }
        }
    }

    public static Task<string?> ru(Country country)
    {
       return Task.FromResult<string?>(country.GetDisplayName(TestCultures.ru));
    }

    public static Task<string?> zh(Country country)
    {
        return Task.FromResult<string?>(country.GetDisplayName(TestCultures.zh));
    }

    private record Display(string Iso2, string Name);

    private static class Prefix
    {
        public const string DE = "{{{3|";
        public const string ES = "{{1|";
    }

    private static class Lookup
    {
        public static readonly Dictionary<Country, string> AR = [];
        public static readonly Dictionary<Country, string> FR = [];
        public static readonly Dictionary<Country, string> PT = [];
    }
}
