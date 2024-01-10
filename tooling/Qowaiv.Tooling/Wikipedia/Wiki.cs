using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Qowaiv.Tooling.Wikipedia;

public static class Wiki
{
    public static string RemoveInParentheses(string text)
        => InParentheses.Replace(text, " ").Trim();

    private static readonly Regex InParentheses = new(@" *\(.+?\) *", RegexOptions.None, TimeSpan.FromMinutes(2));


    public static async Task<WikiQueryResult> Query(string lemma, string language = "en")
    {
        var query = new WikiQuery(lemma, language);
        
        if (query.HasExpired())
        {
            await Download(query);
        }
        var response = await JsonSerializer.DeserializeAsync<WikiQueryResult.Wrapper>(query.Cached.OpenRead(), Options);
        return response?.query ?? throw new InvalidOperationException();
    }

    public static async Task<IReadOnlyCollection<T>> Scrape<T>(string lemma, string language, Func<string, IEnumerable<T>> tryParse) where T : class
    {
        var result = await Query(lemma, language);

        var content = result.Pages.Values.Single().Revisions.Single().Slots.Values.Single().Content!;
        return tryParse(content).ToArray();
    }

    public static async Task<T?> Scrape<T>(string lemma, string language, Func<string, T?> tryParse) where T: class
    {
        var result = await Query(lemma, language);
        var content = result.Pages.Values.Single().Revisions.Single().Slots.Values.Single().Content!;
        return tryParse(content);
    }

    private static async Task Download(WikiQuery query)
    {
        var client = new HttpClient();
        var response = await client.GetAsync(query.Url);

        if (response is { StatusCode: HttpStatusCode.OK })
        {
            using var stream = query.Cached.OpenWrite();
            await response.Content.CopyToAsync(stream);
        }
        else
        {
            var content = await response?.RequestMessage?.Content?.ReadAsStringAsync()!;
            throw new InvalidOperationException($"GET {query.Url} responded with {response.StatusCode}: {content}");
        }
    }

    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
    };
}
