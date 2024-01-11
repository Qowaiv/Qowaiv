namespace Qowaiv.TestTools.Wikipedia;

public sealed record WikiQuery
{
    public WikiQuery(string lemma, string language)
    {
        Url = new Uri($"https://{language}.wikipedia.org/w/api.php?action=query&prop=revisions&titles={lemma}&rvprop=content&rvslots=*&format=json");
        Cached = new(Path.Combine(Path.GetTempPath(), "WikiCache", $"{ToFile(lemma)}.{language}.json"));
        if (!Cached.Directory!.Exists)
        {
            Cached.Directory.Create();
        }
    }

    private static string ToFile(string lemma)
    {
        return lemma.Replace(":", "_");
    }

    public Uri Url { get; }

    public FileInfo Cached { get; init; }

    public TimeSpan Expiration { get; init; } = TimeSpan.FromDays(1);

    public bool HasExpired() => !Cached.Exists || (Clock.UtcNow() - Cached.LastWriteTimeUtc) > Expiration;
}
