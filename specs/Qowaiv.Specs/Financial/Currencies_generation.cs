using Qowaiv.CodeGeneration;
using Qowaiv.TestTools.Resx;

namespace Financial.Currencies_generation;

[NUnit.Framework.Category(nameof(CodeGeneration))]
[Explicit]
public class Generates
{
    private static readonly FileInfo Markdown = new("../../../../../Currencies.md");

    [Test]
    public void Resources_and_constants()
    {
        var resx = new XResourceCollection();

        using var reader = Markdown.OpenText();

        var read = true;

        while (reader.ReadLine() is { } line)
        {
            if (line.StartsWith("## Countries"))
            {
                read = false;
            }
            else if (read
                && line.StartsWith('|')
                && line[1..].Split('|', StringSplitOptions.TrimEntries) is { Length: > 9 } parts
                && parts[0] is { Length: 3 } code)
            {
                var (num, s, d, start, end, en, de, es, nl) = (parts[1], parts[2], parts[3], parts[4], parts[5], parts[6], parts[7], parts[8], parts[9]);

                var display = $"{code}_DisplayName";
                var comment = $"{en} ({code})";

                if (num is not "???") /*.*/ resx.Invariant.Add($"{code}_Num", num);
                if (s is not "") /*......*/ resx.Invariant.Add($"{code}_Symbol", s);
                if (d is not ".") /*.....*/ resx.Invariant.Add($"{code}_Digits", d);
                if (start is not "-") /*.*/ resx.Invariant.Add($"{code}_StartDate", start);
                if (end is not "-") /*...*/ resx.Invariant.Add($"{code}_EndDate", end);

                resx.Invariant.Add($"{code}_ISO", code);
                resx.Invariant.Add(display, en);
                if (de is { Length: > 0 }) resx.Lang(TestCultures.de).Add(display, de, comment);
                if (es is { Length: > 0 }) resx.Lang(TestCultures.es).Add(display, es, comment);
                if (nl is { Length: > 0 }) resx.Lang(TestCultures.nl).Add(display, nl, comment);

                Console.WriteLine();
                Console.WriteLine($"/// <summary>Describes the currency {comment}.</summary>");
                if(end is not "-")
                {
                    Console.WriteLine($"/// <remarks>End date is {end}.</remarks>");
                }
                Console.WriteLine($"""public static readonly Currency {code} = new("{code}");""");
            }
        }

        foreach (var f in resx.Values) f.Data.Sort();

        resx.Save(new("../../../../../src/Qowaiv/Financial"), "CurrencyLabels");
    }

    [Test]
    public void Country_to_currency_mappings()
    {
        using var reader = Markdown.OpenText();

        var read = false;

        while (reader.ReadLine() is { } line)
        {
            if (line.StartsWith("## Countries"))
            {
                read = true;
            }
            else if (read
                && line.StartsWith('|')
                && line[1..].Split('|', StringSplitOptions.TrimEntries) is { Length: 2 } parts
                && Country.TryParse(parts[0]) is { IsKnown: true } country)
            {
                var currencies = parts[1].Split('←', StringSplitOptions.TrimEntries);

                Console.Write($".. New({(country.Name is "IO" ? "Country." : "")}{country.Name}, {Args(currencies[0])})");

                foreach (var arg in currencies[1..])
                {
                    Console.Write($".And({Args(arg)})");
                }

                Console.WriteLine(',');
            }
        }

        static string Args(string arg) => $"{arg.Replace(" (", ", ").Replace("-", ", ").Replace(")", "")}";
    }
}
