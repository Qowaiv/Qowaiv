using Qowaiv.TestTools.Wikipedia;

namespace Qowaiv.TestTools.Generation;

public sealed record Iso3166_3(string DisplayName, string Iso2, string Iso3, int Iso, int Start, int End, string Name)
{
    public override string ToString() => $"{Name}/{Iso2}: {DisplayName} ({Start}-{End})";

    public static IEnumerable<Iso3166_3> Parse(string str)
    {
        var entries = str.Split("|- ");

        foreach (var entry in entries.Skip(1))
        {
            var name = entry.Substring(entry.IndexOf(@"id=""") + 4, 4);

            if (name.Any(c => c < 'A' || c > 'Z')) continue;

            var display = WikiLink.Parse(entry).First().Display;

            var isos = entry.Split("mono|");
            var iso2 = isos[1][..2];
            var iso3 = isos[2][..3];
            var iso = int.TryParse(isos[3][..3], out var n) ? n : 0;

            var years = Regex.Match(entry, "(?<start>[0-9]{4}).(?<end>[0-9]{4})");
            var start = int.Parse(years.Groups["start"].Value);
            var end = int.Parse(years.Groups["end"].Value) - 1;

            yield return new(display, iso2, iso3, iso, start, end, name);
        }
    }
}

