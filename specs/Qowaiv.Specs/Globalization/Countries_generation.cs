﻿#if NET8_0_OR_GREATER
#if DEBUG

using Qowaiv.TestTools.Generation;
using Qowaiv.TestTools.Resx;
using Qowaiv.TestTools.Wikipedia;

namespace Globalization.Countries_specs;

public class Constants
{
    [Test]
    public void Generates()
    {
        var all = Country.All.OrderBy(c => c.Name.Length).ThenBy(c => c.Name).ToArray();

        using var w = new StreamWriter(Solution.Root.File("src/Qowaiv/Generated/Globalization/Country.consts.generated.cs").FullName);
        
        w.WriteLine(@"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------");
        w.WriteLine();
        w.WriteLine("namespace Qowaiv.Globalization;");
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
    internal static readonly IReadOnlyCollection<Iso3166_1> Iso3166_1s = new WikiLemma("ISO 3166-1", TestCultures.En).Transform(Iso3166_1.Parse).Result;
    internal static readonly IReadOnlyCollection<Iso3166_3> Iso3166_3s = new WikiLemma("ISO_3166-3", TestCultures.En).Transform(Iso3166_3.Parse).Result;

    [TestCaseSource(nameof(Iso3166_1s))]
    public void exsiting_reflect_info_of_Wikipedia(Iso3166_1 info)
    {
        var country = Country.Parse(info.Iso2);
        var summary = new Iso3166_1(country.EnglishName, country.IsoAlpha2Code, country.IsoAlpha3Code, country.IsoNumericCode);
        summary.Should().Be(info);
    }

    [TestCaseSource(nameof(Iso3166_3s))]
    public void former_reflect_info_of_Wikipedia(Iso3166_3 info)
    {
        var country = Country.Parse(info.Name);
        var summary = new Iso3166_3(
            country.EnglishName, 
            country.IsoAlpha2Code,
            country.IsoAlpha3Code,
            country.IsoNumericCode,
            0,
            0,
            country.Name);

        summary.Should().BeEquivalentTo(info, c => c.Excluding(m => m.Start).Excluding(m => m.End));
    }

    public class Display_names_match_Wikipedia
    {
        private static readonly IReadOnlyCollection<Country> Existing = Country.GetExisting().ToArray();

        private static readonly Dictionary<string, string> French = CountryDisplayName.FR().Result;

        [TestCaseSource(nameof(Existing))]
        public async Task de(Country country)
        {
            try
            {
                var lemma = new WikiLemma($"Vorlage:{country.IsoAlpha3Code}", TestCultures.De);
                var display = await lemma.Transform(CountryDisplayName.DE);
                display.Should().Be(country.GetDisplayName(TestCultures.De));
            }
            catch (UnknownLemma) { /* Some do not follow this pattern. */ }
        }

        [TestCaseSource(nameof(Existing))]
        public void fr(Country country)
        {
            var display = country.GetDisplayName(TestCultures.Fr);
            display.Should().Be(French[country.IsoAlpha2Code]);
        }

        [TestCaseSource(nameof(Existing))]
        public async Task nl(Country country)
        {
            var lemma = new WikiLemma($"Sjabloon:{country.IsoAlpha2Code}", TestCultures.Nl);
            var display = await lemma.Transform(CountryDisplayName.NL);
            display.Should().Be(country.GetDisplayName(TestCultures.Nl));
        }
    }

    public class Generates
    {
        [Test]
        public void neutral_culture()
        {
            var unknown = new CountryData("ZZ")
            {
                DisplayName = "Unknown",
                ISO = 999,
                ISO2 = "ZZ",
                ISO3 = "ZZZ"
            };

            // Including Kosovo, that is still disputed: https://en.wikipedia.org/wiki/XK_(user_assigned_code)
            var data = new Dictionary<string, CountryData>()
            { 
                ["XK"] = new CountryData("XK")
                {
                    DisplayName = "Kosovo",
                    ISO2 = "XK",
                    ISO3 = "XKK",
                    StartDate = new(2008, 02, 01),
                }
            };

            foreach (var former in Iso3166_3s)
            {
                data[former.Name] = new(former.Name)
                {
                    DisplayName = former.DisplayName,
                    ISO = former.Iso,
                    ISO2 = former.Iso2,
                    ISO3 = former.Iso3,
                    StartDate = new(former.Start, 01, 01),
                    EndDate = new(former.End - 1, 12, 31),
                };
            }

            foreach(var c in Iso3166_1s)
            {
                data[c.Iso2] = new(c.Iso2)
                {
                    DisplayName = c.DisplayName,
                    ISO = c.Iso,
                    ISO2 = c.Iso2,
                    ISO3 = c.Iso3,
                };
            }

            foreach (var c in Country.All)
            {
                var dat = data[c.Name];
                var updated = dat with
                {
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    CallingCode = c.CallingCode,
                };
                data[c.Name] = updated;
            }

            var resource = new XResourceFile();

            resource.Add("All", string.Join(';', data.Keys.OrderBy(c => c.Length).ThenBy(c => c)));
            resource.AddRange(unknown.Data());

            foreach (var info in data.Values.OrderBy(c => c.Name.Length).ThenBy(c => c.Name))
            {
                resource.AddRange(info.Data());
            }

            resource.Invoking(r => r.Save(Solution.Root.File("src/Qowaiv/Globalization/CountryLabels.resx")))
                .Should().NotThrow();
        }

        [Test]
        public async Task de()
        {
            (await CountryDisplayName.Update(
              "Unbekannt",
              TestCultures.De,
              GetDisplayName)
          )
          .Should().NotThrow();

            async Task<string?> GetDisplayName(Country country)
            {
                if (country.Name.Length != 2) return null;
                try
                {
                    var lemma = new WikiLemma($"Vorlage:{country.IsoAlpha3Code}", TestCultures.De);
                    return await lemma.Transform(CountryDisplayName.DE);
                }
                catch (UnknownLemma)
                {
                    return null;
                }
            }
        }

        [Test]
        public async Task fr()
        {
            var lookup = await CountryDisplayName.FR();

            (await CountryDisplayName.Update(
                "Inconnu",
                TestCultures.Fr,
                GetDisplayName)
            )
            .Should().NotThrow();

            Task<string?> GetDisplayName(Country country)
            {
                var display = lookup!.TryGetValue(country.Name, out var name)
                    ? name : null;

                return Task.FromResult(display);
            }
        }

        [Test]
        public async Task nl()
        {
            (await CountryDisplayName.Update(
               "Onbekend",
               TestCultures.Nl,
               GetDisplayName)
           )
           .Should().NotThrow();

            async Task<string?> GetDisplayName(Country country)
            {
                if (country.Name.Length != 2) return null;
                try
                {
                    var lemma = new WikiLemma($"Sjabloon:{country.Name}", TestCultures.Nl);
                    return await lemma.Transform(CountryDisplayName.NL);
                }
                catch (UnknownLemma)
                {
                    return null;
                }
            }
        }
    }
}

#endif
#endif
