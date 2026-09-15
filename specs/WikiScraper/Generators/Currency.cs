using System.Collections;
using System.Collections.Immutable;
using WikiScraper.Models;

namespace WikiScraper.Generators;

public static class Currency
{
    public static async Task Generate()
    {
        var lemma = new WikiLemma("ISO 4217", TestCultures.en);

        CurrencyInfo[] all = [.. await lemma.TransformRange(CurrencyInfo.TryParse)];
    }
}

public sealed partial record CurrencyInfo
{
    public required string Code { get; init; }

    public required int Num { get; init; }

    public required int? Digits { get; init; }

    public required string Name { get; init; }

    public required ImmutableArray<string> Countries { get; init; }

    internal static IEnumerable<CurrencyInfo> TryParse(string content)
    {
        foreach(var line in content.Split('\n'))
        {
            if(Pattern().Match(line) is { Success: true } match)
            {
                var code = match.Groups[nameof(Code)].Value;
                var num = int.Parse(match.Groups[nameof(Num)].Value);
                var dig = int.TryParse(match.Groups[nameof(Digits)].Value, out var d) ? (int?)d : null;
                var name = match.Groups[nameof(Name)].Value;

                yield return new CurrencyInfo
                {
                    Code = code,
                    Num = num,
                    Digits = dig,
                    Name = name,
                    Countries = [],
                };
            }
        }
    }

    [GeneratedRegex("\\|\\s*(?<Code>[A-Z]{3})\\s*\\|+\\s*(?<Num>[0-9]{3})\\s*\\|+\\s*(?<Digits>[0-9\\-])\\s*\\|+\\s*\\[\\[(?<Name>.+?)\\]\\]\\s*\\|+(?<Remainder>.+)")]
    private static partial Regex Pattern();
}
