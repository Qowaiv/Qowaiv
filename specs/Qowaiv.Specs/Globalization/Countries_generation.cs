﻿#if NET8_0_OR_GREATER
#if DEBUG

using Qowaiv.Tooling.Resx;
using Qowaiv.Tooling.Wikipedia;

namespace Globalization.Countries_specs;

public class Display_name
{
    private static readonly IReadOnlyCollection<Country> All = Country.All;

    [TestCaseSource(nameof(All))]
    public async Task matches_nl_Wikipedia(Country country)
    {
        var display = await Wiki.Scrape(lemma: $"Sjabloon:{country.Name}", language: "nl", s => DisplayName.FromNL(s, country.IsoAlpha2Code));

        display!.Name.Should().Be(country.GetDisplayName(TestCultures.Nl_NL));
    }
}

public class Resource_files
{
    [Test]
    public async Task generates_nl()
    {
        var resource = new XResourceFile(new XResourceFileData("ZZ_DisplayName", "Onbekend", "Unknown (ZZ)"));

        foreach (var country in Country.All.OrderBy(c => c.Name.Length).ThenBy(c => c.Name))
        {
            var name = $"{country.Name}_DisplayName";
            var comment = $"{country.EnglishName} ({country.IsoAlpha2Code})";
            var value = country.Name.Length == 2 && (await Wiki.Scrape(lemma: $"Sjabloon:{country.Name}", language: "nl", s => DisplayName.FromNL(s, country.IsoAlpha2Code))) is { } display
                ? display.Name
                : country.GetDisplayName(TestCultures.Nl_NL);

            if (value != country.EnglishName)
            {
                resource.Add(name, value, comment);
            }
        }

        resource.Invoking(r => r.Save(new FileInfo("../../../../../src/Qowaiv/Globalization/CountryLabels.nl.resx")))
            .Should().NotThrow();
    }

}

public class Scrape
{
     [Test]
    public async Task Scrape_en()
    {
        var resources = XResourceCollection.Load(new("../../../../../src/Qowaiv/Globalization"));

        var infos = await Wiki.Scrape(lemma: "List of ISO 3166 country codes", language: "en", CountryInfo.FromEN);

        var nls = new List<DisplayName>();

        foreach (var info in infos)
        {
            var nl = await Wiki.Scrape(lemma: $"Sjabloon:{info.A2}", language: "nl", s=> DisplayName.FromNL(s, info.A2));
            if (nl is { })
            {
                nls.Add(nl);
            }
        }

        infos.Should().NotBeEmpty();


        foreach(var nl  in nls)
        {
            var country = Country.Parse(nl.A2);

            var display = country.GetDisplayName(new CultureInfo("nl"));

            if (display != nl.Name)
            {
                Console.WriteLine($"{nl.A2}: {nl.Name} [{display}]");
            }
        }

        //foreach (var info in infos)
        //{
        //    var country = Country.Parse(info.A2);

        //    if (country.EnglishName != info.Name)
        //    {
        //        Console.WriteLine($"{info.A2}: {info.Name} [{country.EnglishName}]");
        //    }
        //}

        //foreach (var info in infos)
        //{
        //    var country = Country.Parse(info.A2);

        //    if (country.EnglishName == info.Name)
        //    {
        //        Console.WriteLine($"{info.A2}: {info.Name}");
        //    }
        //}

    }

    internal sealed record CountryInfo(string Name, string A2, string A3, int NC, Dictionary<string, string> DisplayNames)
    {
        public static IEnumerable<CountryInfo> FromEN(string str)
        {
            foreach (var line in str.Split("| id=").Skip(1))
            {
                var parts = line.Split("\n");

                if (parts.Length >= 6)
                {
                    var link = WikiLink.Parse(parts[0]).First();
                    var name = Wiki.RemoveInParentheses(link.Display);

                    var a2 = parts[3].Substring(parts[3].IndexOf("mono|") + 5, 2);
                    var a3 = parts[4].Substring(parts[4].IndexOf("mono|") + 5, 3);
                    var nc = parts[5].Substring(parts[5].IndexOf("mono|") + 5, 3);

                    yield return new(name, a2, a3, int.Parse(nc), new() { ["en"] = name });
                }
            }
        }
    }
}

internal sealed record DisplayName(string A2, string Name)
{
    public static DisplayName? FromNL(string str, string a2)
    {
        var index = str.LastIndexOf($"-VLAG}}}}&nbsp;") + 1;
        var text = str[index..];

        if(a2 == "DE")
        {

        }

        if (WikiLink.Parse(text).FirstOrDefault() is { } link)
        {

            var display = link.Display;
            display = display[(display.IndexOf('|') + 1)..];
            display = display.Trim('{').Trim('}');
            return new(a2, display);
        }
        else return null;
    }
}

#endif
#endif
