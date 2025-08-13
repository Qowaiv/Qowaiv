namespace WikiScraper.Models;

public class UnknownLemma : Exception
{
    /// <summary>Initializes a new instance of the <see cref="UnknownLemma" /> class.</summary>
    public UnknownLemma() { }

    /// <summary>Initializes a new instance of the <see cref="UnknownLemma" /> class.</summary>
    public UnknownLemma(string? message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="UnknownLemma" /> class.</summary>
    public UnknownLemma(string? message, Exception? innerException) : base(message, innerException) { }

    public static UnknownLemma For(string title, CultureInfo language, Uri url)
        => new($"Lemma '{title}' ({language}) not available at {url}.");
}
