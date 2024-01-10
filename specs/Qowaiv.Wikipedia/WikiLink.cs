namespace Qowaiv.Wikipedia;

public sealed class WikiLink
{
    public WikiLink(string lemma, string? display)
    {
        Lemma = lemma;
        Display = display ?? lemma;
    }

    public string Lemma { get; }
    
    public string Display { get; }

    /// <inheritdoc />
    public override string ToString()
        => Lemma == Display
        ? $"[[{Lemma}]]"
        : $"[[{Lemma}|{Display}]]";


    public static IEnumerable<WikiLink> Parse(string text) => Pattern
        .Matches(text)
        .Where(m => m.Success)
        .Select(AsLink);

    private static WikiLink AsLink(Match m)
        => new(m.Groups[nameof(Lemma)].Value, m.Groups[nameof(Display)].Value is { Length: > 0 } display ? display : null);

    private static readonly Regex Pattern = new(@"\[\[(?<Lemma>.+?)(\|(?<Display>.*?))?\]\]", RegexOptions.None, TimeSpan.FromMinutes(2));
}
