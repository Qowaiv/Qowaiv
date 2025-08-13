using System.Text.Json.Serialization;

namespace WikiScraper.Models;

public sealed class WikiQueryResult
{
    public Dictionary<string, Page> Pages { get; init; } = [];

    public sealed class Page
    {
        public int PageId { get; init; }
        public string? Title { get; init; }

        public Revision[] Revisions { get; init; } = [];

        public sealed class Revision
        {
            public Dictionary<string, Slot> Slots { get; init; } = [];

            public class Slot
            {
                [JsonPropertyName("*")]
                public string? Content { get; init; }
            }
        }
    }

    internal sealed class Wrapper
    {
        public WikiQueryResult Query { get; init; } = new();
    }
}
