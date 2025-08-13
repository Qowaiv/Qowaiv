namespace WikiScraper.Models;

public sealed class WikiLink(string lemma, string? display)
{
    public string Lemma { get; } = lemma;

    public string Display { get; } = display ?? lemma;

    /// <inheritdoc />
    [Pure]
    public override string ToString()
        => Lemma == Display
        ? $"[[{Lemma}]]"
        : $"[[{Lemma}|{Display}]]";

    [Pure]
    public static IEnumerable<WikiLink> Parse(string text) => Pattern
        .Matches(text)
        .Where(m => m.Success)
        .Select(AsLink);

    private static WikiLink AsLink(Match m)
        => new(m.Groups[nameof(Lemma)].Value, m.Groups[nameof(Display)].Value is { Length: > 0 } dis ? dis : null);

    private static readonly Regex Pattern = new(@"\[\[(?<Lemma>.+?)(\|(?<Display>.*?))?\]\]", RegexOptions.None, TimeSpan.FromMinutes(2));
}
