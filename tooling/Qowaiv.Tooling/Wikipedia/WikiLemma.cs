using System.Text.Json;

namespace Qowaiv.Tooling.Wikipedia;

public sealed record WikiLemma
{
    public WikiLemma(string title, string language)
    {
        Title = title;
        Language = new(language);
        Url = new Uri($"https://{language}.wikipedia.org/w/api.php?action=query&prop=revisions&titles={title}&rvprop=content&rvslots=*&format=json");
        Cached = new(Path.Combine(Path.GetTempPath(), "WikiCache", $"{ToFile(title)}.{language}.wiki.txt"));
        if (!Cached.Directory!.Exists)
        {
            Cached.Directory.Create();
        }
    }

    private static string ToFile(string lemma)
    {
        return lemma.Replace(":", "_");
    }

    public string Title { get; }

    public CultureInfo Language { get; }

    public Uri Url { get; }

    public FileInfo Cached { get; init; }

    public TimeSpan Expiration { get; init; } = TimeSpan.FromDays(2);

    [Pure]
    public async Task<string> Content()
    {
        if (HasExpired)
        {
            await Update();
        }
        using var reader = Cached.OpenText();
        return await reader.ReadToEndAsync();
    }

    [Pure]
    public async Task<T?> Transform<T>(Func<string, T?> tryParse) where T : class
        => tryParse(await Content());

    [Pure]
    public async Task<IReadOnlyCollection<T>> Transform<T>(Func<string, IEnumerable<T>> tryParse) where T : class
        => tryParse(await Content()).ToArray();

    private async Task Update()
    {
        var client = new HttpClient();
        var response = await client.GetAsync(Url);

        var body = await response.Content.ReadAsStringAsync();

        if (response is { StatusCode: HttpStatusCode.OK })
        {
            var wrapper = JsonSerializer.Deserialize<WikiQueryResult.Wrapper>(body, Options);
            var content = wrapper?.query.Pages.Values
                .SingleOrDefault()?
                .Revisions.SingleOrDefault()?
                .Slots.Values.SingleOrDefault()?
                .Content ?? throw new InvalidOperationException($"Lemma '{Title}' not available at {Url}.");

            using var stream = Cached.Open(new FileStreamOptions { Access = FileAccess.Write, Mode = FileMode.Create });
            using var writer = new StreamWriter(stream);
            await writer.WriteAsync(content);
        }
        else
        {
            throw new InvalidOperationException($"GET {Url} responded with {response.StatusCode}: {body}");
        }
    }

    public bool HasExpired => !Cached.Exists || (Clock.UtcNow() - Cached.LastWriteTimeUtc) > Expiration;

    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
    };
}
