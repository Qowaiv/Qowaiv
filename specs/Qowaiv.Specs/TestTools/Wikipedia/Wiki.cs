namespace Qowaiv.TestTools.Wikipedia;

public static class Wiki
{
    [Pure]
    public static string RemoveInParentheses(string text)
        => InParentheses.Replace(text, " ").Trim();

    private static readonly Regex InParentheses = new(@" *\(.+?\) *", RegexOptions.None, TimeSpan.FromMinutes(2));
}
