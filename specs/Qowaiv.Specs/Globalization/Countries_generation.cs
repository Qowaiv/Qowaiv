#if NET8_0_OR_GREATER
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
        Action generate = CountryConstants.Generate;
        generate.Should().NotThrow();
    }
}

/// <remarks>
/// As fetching data from Wikipedia is time consuming (no benefits from caching)
/// and therefor not executed on a RELEASE build.
/// </remarks>
public class Resource_files
{
    internal static readonly IReadOnlyCollection<Iso3166_1> Iso3166_1s = new WikiLemma("ISO 3166-1", TestCultures.en).TransformRange(Iso3166_1.Parse).Result;
    internal static readonly IReadOnlyCollection<Iso3166_3> Iso3166_3s = new WikiLemma("ISO_3166-3", TestCultures.en).TransformRange(Iso3166_3.Parse).Result;

    [TestCaseSource(nameof(Iso3166_1s))]
    public void existing_reflect_info_of_Wikipedia(Iso3166_1 info)
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

    public class Display_names
    {
        private static readonly IReadOnlyCollection<Country> Existing = Country.GetExisting().ToArray();

        [TestCaseSource(nameof(Existing))]
        public async Task ar(Country country)
        {
            var display = country.GetDisplayName(TestCultures.ar);
            display.Should().MatchWikipedia(await CountryDisplayName.ar(country))
                .And.BeArabic()
                .And.BeTrimmed();
        }

        [TestCaseSource(nameof(Existing))]
        public async Task de(Country country)
        {
            var display = country.GetDisplayName(TestCultures.de);
            display.Should().MatchWikipedia(await CountryDisplayName.de(country))
                .And.BeTrimmed();
        }

        [TestCaseSource(nameof(Existing))]
        public async Task es(Country country)
        {
            var display = country.GetDisplayName(TestCultures.es);
            display.Should().MatchWikipedia(await CountryDisplayName.es(country))
                .And.BeTrimmed(); 
        }

        [TestCaseSource(nameof(Existing))]
        public async Task fr(Country country)
        {
            var display = country.GetDisplayName(TestCultures.fr);
            display.Should().MatchWikipedia(await CountryDisplayName.fr(country))
                .And.BeTrimmed();
        }

        [TestCaseSource(nameof(Existing))]
        public async Task it(Country country)
        {
            var display = country.GetDisplayName(TestCultures.it);
            display.Should().MatchWikipedia(await CountryDisplayName.it(country))
                .And.BeTrimmed();
        }

        [TestCaseSource(nameof(Existing))]
        public async Task ja(Country country)
        {
            var display = country.GetDisplayName(TestCultures.ja);
            display.Should().MatchWikipedia(await CountryDisplayName.ja(country))
                .And.BeTrimmed();
        }

        [TestCaseSource(nameof(Existing))]
        public async Task nl(Country country)
        {
            var display = country.GetDisplayName(TestCultures.nl);
            display.Should().MatchWikipedia(await CountryDisplayName.nl(country))
                .And.BeTrimmed();
        }

        [TestCaseSource(nameof(Existing))]
        public async Task pt(Country country)
        {
            var display = country.GetDisplayName(TestCultures.pt);
            display.Should().MatchWikipedia(await CountryDisplayName.pt(country))
                .And.BeTrimmed();
        }

        [TestCaseSource(nameof(Existing))]
        public async Task ru(Country country)
        {
            var display = country.GetDisplayName(TestCultures.ru);
            display.Should().MatchWikipedia(await CountryDisplayName.ru(country))
                .And.BeTrimmed();
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
        public async Task ar()
            => (await CountryDisplayName.Update("مجهولة", TestCultures.ar, CountryDisplayName.ar))
            .Should().NotThrow();

        [Test]
        public async Task de()
            => (await CountryDisplayName.Update("Unbekannt", TestCultures.de, CountryDisplayName.de))
            .Should().NotThrow();

        [Test]
        public async Task es()
            => (await CountryDisplayName.Update("Desconocido", TestCultures.es, CountryDisplayName.es))
            .Should().NotThrow();

        [Test]
        public async Task fr()
            => (await CountryDisplayName.Update("Inconnu", TestCultures.fr, CountryDisplayName.fr))
            .Should().NotThrow();
    
        [Test]
        public async Task it()
            => (await CountryDisplayName.Update("Sconosciuto", TestCultures.it, CountryDisplayName.it))
            .Should().NotThrow();

        [Test]
        public async Task ja()
           => (await CountryDisplayName.Update("不明", TestCultures.ja, CountryDisplayName.ja))
           .Should().NotThrow();

        [Test]
        public async Task nl()
            => (await CountryDisplayName.Update("Onbekend",TestCultures.nl,CountryDisplayName.nl))
            .Should().NotThrow();

        [Test]
        public async Task pt()
            => (await CountryDisplayName.Update("Não sabe", TestCultures.pt, CountryDisplayName.pt))
            .Should().NotThrow();

        [Test]
        public async Task ru()
            => (await CountryDisplayName.Update("неизвестно", TestCultures.ru, CountryDisplayName.ru))
            .Should().NotThrow();

        [Test]
        public async Task zh()
          => (await CountryDisplayName.Update("未知項", TestCultures.zh, CountryDisplayName.zh))
          .Should().NotThrow();
    }
}

#endif
#endif
