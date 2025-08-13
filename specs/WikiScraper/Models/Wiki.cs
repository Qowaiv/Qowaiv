using System.Text;

namespace WikiScraper.Models;

public static class Wiki
{
    [Pure]
    public static string RemoveFormatting(string text)
    {
        text = text.Replace("'''", string.Empty);
        var sb = new StringBuilder(text.Length);
        var lemma = new StringBuilder();

        var mode = FormattingMode.None;


        for(var i = 0; i < text.Length; i++)
        {
            var ch = text[i];

            if (mode is FormattingMode.Tag)
            {
                if (ch == '>') mode = FormattingMode.None;
            }
            else if(mode is FormattingMode.Lemma)
            {
                if (ch is '}' && text[i - 1] is '}')
                {
                    sb.Append(lemma.ToString()[1..^1].Split('|')[^1]);
                    lemma.Clear();
                    mode = FormattingMode.None;
                }
                else lemma.Append(ch);
            }
            else
            {
                if (ch is '<') mode = FormattingMode.Tag;
                else if (ch is '{' && text[i + 1] is '{') mode = FormattingMode.Lemma;
                else
                {
                    sb.Append(ch);
                }
            }
        }

        return sb.ToString();
    }

    private enum FormattingMode
    {
        None,
        Lemma,
        Tag,
    }

    [Pure]
    public static string RemoveInParentheses(string text)
        => InParentheses.Replace(text, " ").Trim();

    private static readonly Regex InParentheses = new(@" *\(.+?\) *", RegexOptions.None, TimeSpan.FromMinutes(2));
}
