using Qowaiv.TestTools.Resx;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Globalization.Countries_generation;

[Explicit]
public class Generates
{
    [Test]
    public void Resources()
    {
        using var reader = new FileInfo("../../../../../Countries.md").OpenText();

        var resources = new XResourceCollection();

        while (reader.ReadLine() is { } line)
        {
            if (line.Length > 0 
                && line[1..].Split('|', StringSplitOptions.TrimEntries) is { Length: > 8 } parts
                && IsName(parts[0]))
            {
                var (name, iso2, iso3, num, i, start, end, en, de, es, fr, it, nl, pt, ru, ja, zh, zh_HK, zh_TW, ar)
                    = (parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], parts[6], parts[7], parts[8], parts[9], parts[10], parts[11], parts[12], parts[13], parts[14], parts[15], parts[16], parts[17], parts[18], parts[19]);

                resources.Invariant.AddKey(name, "ISO2", iso2);
                resources.Invariant.AddKey(name, "ISO3", iso3);
                resources.Invariant.AddKey(name, "ISO", num);
                resources.Invariant.AddKey(name, "Independent", i is "Y" ? "true" : "false");
                resources.Invariant.AddKey(name, "StartDate", start);
                if(end is not "-") resources.Invariant.AddKey(name, "EndDate", end);

                resources.Invariant.AddDisplay(name, en);
                if (en != de) resources[TestCultures.de].AddDisplay(name, de);
                if (en != es) resources[TestCultures.es].AddDisplay(name, es);
                if (en != fr) resources[TestCultures.fr].AddDisplay(name, fr);
                if (en != it) resources[TestCultures.it].AddDisplay(name, it);
                if (en != nl) resources[TestCultures.nl].AddDisplay(name, nl);
                if (en != pt) resources[TestCultures.pt].AddDisplay(name, pt);
                if (en != ru) resources[TestCultures.ru].AddDisplay(name, ru);
                if (en != ja) resources[TestCultures.ja].AddDisplay(name, ja);
                if (en != zh) resources[TestCultures.zh].AddDisplay(name, zh);
                if (zh != zh_HK) resources[TestCultures.zh_HK].AddDisplay(name, zh_HK);
                if (zh != zh_HK) resources[TestCultures.zh_HK].AddDisplay(name, zh_TW);
                if (en != ar) resources[TestCultures.ar].AddDisplay(name, ar);
            }
        }

        CultureInfo[] cultures =
        [
            CultureInfo.InvariantCulture,
            TestCultures.de,
            TestCultures.es,
            TestCultures.fr,
            TestCultures.it,
            TestCultures.nl,
            TestCultures.pt,
            TestCultures.ru,
            TestCultures.ja,
            TestCultures.zh,
            TestCultures.zh_HK,
            TestCultures.zh_HK,
            TestCultures.ar
        ];

        foreach(var country in Country.All)
        {
            foreach(var culture in cultures)
            {
                var curr = country.GetDisplayName(culture);
                var next = resources.Display(culture, country);

                if (curr != next)
                    Console.WriteLine($"{country.Name}[{culture.Name}]: {next} != {curr}");
            }
        }

        static bool IsName(string s) => s.Length is >= 2 and <= 4 && s.All(char.IsAsciiLetterUpper);
    }
}

file static class Extensions
{
    extension(XResourceFile file)
    {
        public void AddDisplay(string name, string val)
            => file.AddKey(name, "DisplayName", val);

        public void AddKey(string name, string key, string val)
            => file.Add($"{name}_{key}", val);
    }
    extension(XResourceCollection files)
    {
        public string? Display(CultureInfo culture, Country country) => culture switch
        {
            _ when files[culture][$"{country.Name}_DisplayName"]?.Value is { Length: > 0 } v => v,
            _ when culture.Parent is { } parent => files.Display(parent, country),
            _ => null,
        };
    }
}
