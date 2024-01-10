#if NET8_0_OR_GREATER

using Qowaiv.Wikipedia;
using System.Threading.Tasks;

namespace Globalization.Country_generation;

public class Scrape
{
    [Test]
    public async Task Scrape_en()
    {
        var infos = await Wiki.Scrape(lemma: "List of ISO 3166 country codes", language: "en", CountryInfo.FromEN);

        var nls = new List<DisplayName>();

        foreach (var info in infos)
        {
            var nl = await Wiki.Scrape(lemma: $"Sjabloon:{info.A2}", language: "nl", DisplayName.FromNL);
            if (nl is { })
            {
                nls.Add(nl with { A2 = info.A2 });
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

    internal sealed record DisplayName(string A2, string Name)
    {
        public static DisplayName? FromNL(string str)
        {
            var links = WikiLink.Parse(str).ToArray();

            return new("", links[0].Display);
        }
    }
}
#endif
