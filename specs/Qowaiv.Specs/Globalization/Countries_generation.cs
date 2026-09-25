using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.Syntax;
using Qowaiv.TestTools.Resx;
using System.Xml.Linq;

namespace Globalization.Countries_generation;

[Explicit]
public class Generates
{
    private static readonly CultureInfo[] Supported =
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
            TestCultures.zh_TW,
            TestCultures.ar
        ];

    [Test]
    public void Resources()
    {
        var unknowns = XResourceCollection.Load(new("../../../../../src/Qowaiv"), "UnknownLabels");

        using var reader = new FileInfo("../../../../../Countries.md").OpenText();

        var resources = new XResourceCollection();

        var zz = "ZZ";

        resources.Invariant.AddKey(zz, "ISO2", "ZZ");
        resources.Invariant.AddKey(zz, "ISO3", "ZZZ");
        resources.Invariant.AddKey(zz, "ISO", "999");
        resources.Invariant.AddKey(zz, "StartDate", "0001-01-01");
        resources.Invariant.AddDisplay(zz, nameof(Unknown));
        foreach (var culture in Supported.Skip(1))
        {
            resources[culture].AddDisplay("ZZ", unknowns.TryGetValue("Values", culture)!.Split(';')[0]);
        }

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
                resources.Invariant.AddKey(name, "CallingCode", Country.Parse(name).CallingCode);
                if (end is not "-") resources.Invariant.AddKey(name, "EndDate", end);

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
                if (zh != zh_TW) resources[TestCultures.zh_TW].AddDisplay(name, zh_TW);
                if (en != ar) resources[TestCultures.ar].AddDisplay(name, ar);
            }
        }

        resources.Should().HaveCount(13);


        LogDeltas(resources, Supported);

        foreach (var resource in resources)
        {
            resource.Value.Data.Sort();
        }

        resources.Save(new("../../../../../src/Qowaiv/Globalization"), "CountryLabels");

        static void LogDeltas(XResourceCollection resources, CultureInfo[] cultures)
        {
            foreach (var country in Country.All)
            {
                foreach (var culture in cultures)
                {
                    var curr = country.GetDisplayName(culture);
                    var next = resources.Display(culture, country);

                    if (curr != next)
                        Console.WriteLine($"{country.Name}[{culture.Name}]: {next} != {curr}");
                }
            }
        }
    }

    [Test]
    public void Consts()
    {
        using var reader = new FileInfo("../../../../../Countries.md").OpenText();

        var consts = new List<CountryConst>();

        while (reader.ReadLine() is { } line)
        {
            if (line.Length > 0
                && line[1..].Split('|', StringSplitOptions.TrimEntries) is { Length: > 8 } parts
                && IsName(parts[0]))
            {
                var (name, end, en) = (parts[0], parts[6], parts[7]);
                consts.Add(new(name, en, end));
            }
        }

        var settings = new CSharpWriterSettings();
        using var file = new StreamWriter("../../../../../src/Qowaiv/Generated/Globalization/Country.consts.generated.cs", settings.Encoding, new() { Access = FileAccess.Write, Mode = FileMode.Create });
        var writer = new CSharpWriter(file, settings);

        writer.Write(Headers.AutoGenerated([]));
        using (writer.NamespaceDeclaration("Qowaiv.Globalization"))
        {
            writer.Line("public readonly partial struct Country");

            using (writer.CodeBlock())
            {
                foreach (var c in consts)
                {
                    writer.Indent().Line($"/// <summary>Describes the country {c.Display} ({c.Name}).</summary>");

                    if (c.EndDate is not "-")
                        writer.Indent().Line($"/// <remarks>End date is {c.EndDate}.</remarks>.");

                    writer.Indent().Line($"""public static readonly Country {c.Name} = new("{c.Name}");""")
                        .Line();
                }

                writer.Indent().Line("/// <summary>Gets a collection of all countries.</summary>");
                writer.Indent().Write("public static readonly System.Collections.ObjectModel.ReadOnlyCollection<Country> All = new([");
                writer.Write(string.Join(',', consts.Select(c => c.Name)));
                writer.Line("]);");
            }
        }

        consts.Should().NotBeEmpty();
    }

    private sealed record CounrtyConst(string Name, string Display, string EndDate);

    private static bool IsName(string s) => s.Length is >= 2 and <= 4 && s.All(char.IsAsciiLetterUpper);
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
        public string? Display(CultureInfo culture, Country country)
            => files.TryGetValue($"{country.Name}_DisplayName", culture);
    }
}
