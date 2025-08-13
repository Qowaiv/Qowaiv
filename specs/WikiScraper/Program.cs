using WikiScraper.Generators;

namespace WikiScraper;

internal static class Program
{
    public static Task Main(string[] args)
    {
        return Iban.Generate();
    }
}
