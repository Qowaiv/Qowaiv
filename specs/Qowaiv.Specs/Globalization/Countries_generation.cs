#if NET8_0_OR_GREATER
#if DEBUG

using Qowaiv.Tooling;
using Qowaiv.Tooling.Resx;
using Qowaiv.Tooling.Wikipedia;

namespace Globalization.Countries_specs;

public class Constants
{
    [Test]
    public void Generates()
    {
        var all = Country.All.OrderBy(c => c.Name.Length).ThenBy(c => c.Name).ToArray();

        using var w = new StreamWriter(Solution.Root.File("src/Qowaiv/Globalization/CountryConstants.cs").FullName);
        w.WriteLine("#pragma warning disable S1210");
        w.WriteLine("// \"Equals\" and the comparison operators should be overridden when implementing \"IComparable\"");
        w.WriteLine("// See README.md => Sortable\r\nnamespace Qowaiv.Globalization;");
        w.WriteLine();
        w.WriteLine("public readonly partial struct Country");
        w.WriteLine("{");

        foreach (var country in all)
        {
            if (country != all[0]) w.WriteLine();

            w.WriteLine($"    /// <summary>Describes the country {country.EnglishName} ({country.Name}).</summary>");
            if (country.EndDate is { } enddate)
            {
                w.WriteLine($"    /// <remarks>End date is {enddate:yyyy-MM-dd}.</remarks>");
            }
            w.WriteLine($"    public static readonly Country {country.Name} = new(\"{country.Name}\");");
        }
        w.WriteLine("}");


        w.Invoking(w => w.Flush()).Should().NotThrow();
    }
}

/// <remarks>
/// As fetching data from Wikipedia is time consuming (no benefits from caching)
/// and therefor not executed on a RELEASE build.
/// </remarks>
public class Resource_files
{
    internal static readonly IReadOnlyCollection<WikiInfo> Infos = new WikiLemma("ISO 3166-1", "en").Transform(WikiInfo.FromEN).Result;

    [TestCaseSource(nameof(Infos))]
    public void reflect_info_of_Wikipedia(WikiInfo info)
    {
        var country = Country.Parse(info.A2);
        var summary = new WikiInfo(country.EnglishName, country.IsoAlpha2Code, country.IsoAlpha3Code, country.IsoNumericCode);
        summary.Should().Be(info);
    }

    public class Display_names
    {
        private static readonly IReadOnlyCollection<Country> Exsiting = Country.GetExisting().ToArray();

        [TestCaseSource(nameof(Exsiting))]
        public async Task match_nl_Wikipedia(Country country)
        {
            var lemma = new WikiLemma($"Sjabloon:{country.Name}", "nl");
            var display = await lemma.Transform(DisplayName.FromNL);
            display.Should().Be(country.GetDisplayName(TestCultures.Nl_NL));
        }
    }

    public class Generates
    {
        [Test]
        public void neutral_culture()
        {
            var all = Country.All.OrderBy(c => c.Name.Length).ThenBy(c => c.Name).ToArray();

            var resource = new XResourceFile();
            resource.Add("All", string.Join(';', all.Select(c => c.Name)));

            foreach (var country in new[] { Country.Unknown }.Concat(all))
            {
                var pref = country.IsUnknown() ? "ZZ" : country.Name;

                resource.Add($"{pref}_DisplayName", country.DisplayName);
                resource.Add($"{pref}_ISO", country.IsoNumericCode.ToString("000"));
                resource.Add($"{pref}_ISO2", country.IsoAlpha2Code);
                resource.Add($"{pref}_ISO3", country.IsoAlpha3Code);
                resource.Add($"{pref}_StartDate", country.StartDate.ToString("yyyy-MM-dd"));
                if (country.EndDate is { } enddate)
                {
                    resource.Add($"{pref}_EndDate", enddate.ToString("yyyy-MM-dd"));
                }
                if (country.CallingCode is { Length: > 0 })
                {
                    resource.Add($"{pref}_CallingCode", country.CallingCode);
                }
            }

            resource.Invoking(r => r.Save(Solution.Root.File("src/Qowaiv/Globalization/CountryLabels.resx")))
                .Should().NotThrow();
        }

        [Test]
        public async Task nl_culture()
        {
            var resource = new XResourceFile(new XResourceFileData("ZZ_DisplayName", "Onbekend", "Unknown (ZZ)"));

            foreach (var country in Country.All.OrderBy(c => c.Name.Length).ThenBy(c => c.Name))
            {
                var name = $"{country.Name}_DisplayName";
                var comment = $"{country.EnglishName} ({country.IsoAlpha2Code})";
                var value = country.Name.Length == 2 && await Display(country) is { } display
                    ? display
                    : country.GetDisplayName(TestCultures.Nl_NL);

                if (value != country.EnglishName)
                {
                    resource.Add(name, value, comment);
                }
            }

            resource.Invoking(r => r.Save(Solution.Root.File("src/Qowaiv/Globalization/CountryLabels.nl.resx")))
                .Should().NotThrow();

            Task<string?> Display(Country country)
            {
                var lemma = new WikiLemma($"Sjabloon:{country.Name}", "nl");
                return lemma.Transform(DisplayName.FromNL);
            }
        }
    }
}

public sealed record WikiInfo(string Name, string A2, string A3, int NC)
{
    public override string ToString() => $"{A2}/{A3}: {Name} ({NC:000})";

    // Overrides of Wikipedia: 
    private static readonly Dictionary<string, string> Shorten = new()
    {
        ["Bolivia (Plurinational State of)"] = "Bolivia",
        ["Bonaire, Sint Eustatius and Saba"] = "Caribbean Netherlands",
        ["Iran (Islamic Republic of)"] = "Iran",
        ["Korea (Democratic People's Republic of)"] = "North Korea",
        ["Korea, Republic of"] = "South Korea",
        ["Lao People's Democratic Republic"] = "Loas",
        ["Micronesia (Federated States of)"] = "Micronesia",
        ["Moldova, Republic of"] = "Moldova",
        ["Netherlands, Kingdom of the"] = "Netherlands",
        ["Palestine, State of"] = "Palestine",
        ["Russian Federation"] = "Russia",
        ["Taiwan, Province of China"] = "Taiwan",
        ["Tanzania, United Republic of"] = "Tanzania",
        ["United Kingdom of Great Britain and Northern Ireland"] = "United Kingdom",
        ["United States of America"] = "United States",
        ["Venezuela (Bolivarian Republic of)"] = "Venezuela",
        ["<!--DO NOT CHANGE-->{{not a typo|Sao Tome and Principe}}<!-- Do not change this to \"São Tomé and Príncipe\" unless https://www.iso.org/obp/ui/#iso:code:3166:ST changes to that spelling. If you disagree with the lack of diacritics, contact the ISO 3166/MA; we must follow the published standard for this article. -->"] = "Sao Tome and Principe",
    };

    public static IEnumerable<WikiInfo> FromEN(string str)
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

internal static class DisplayName
{
    public static string? FromNL(string str)
    {
        var index = str.LastIndexOf($"-VLAG}}}}&nbsp;") + 1;
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

#endif
#endif
