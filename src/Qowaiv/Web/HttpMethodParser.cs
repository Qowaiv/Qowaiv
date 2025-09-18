using System.Net.Http;

namespace Qowaiv.Web;

/// <summary>Helper method to facility parsing for an <see cref="HttpMethod"/>.</summary>
/// <remarks>
/// Required to support .NET 6.0 and earlier.
/// </remarks>
internal static class HttpMethodParser
{
#if NET8_0_OR_GREATER
    /// <inheritdoc cref="HttpMethod.Parse(ReadOnlySpan{char})" />
    [Pure]
    public static HttpMethod? Parse(string? str)
        => str is { Length: > 0 }
        ? HttpMethod.Parse(str)
        : null;
#else
    /// <summary>Parses an <see cref="HttpMethod"/>.</summary>
    [Pure]
    public static HttpMethod? Parse(string? str) => str?.ToUpperInvariant() switch
    {
        null => null,
        _ when Methods.TryGetValue(str, out var method) => method,
        { Length: > 0 } => new(str),
        _ => null,
    };

    private static readonly FrozenDictionary<string, HttpMethod> Methods = new Dictionary<string, HttpMethod>
    {
        ["DELETE"] = HttpMethod.Delete,
        ["GET"] = HttpMethod.Get,
        ["HEAD"] = HttpMethod.Head,
        ["OPTIONS"] = HttpMethod.Options,
        ["POST"] = HttpMethod.Post,
        ["PUT"] = HttpMethod.Put,
        ["TRACE"] = HttpMethod.Trace,
    }
    .ToFrozenDictionary();
#endif
}
