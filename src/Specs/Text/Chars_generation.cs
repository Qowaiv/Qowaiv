#if !NET48

namespace Text.Chars_generation;

[Explicit]
public class Generates
{
    /// <summary>Generates the Chars category lookup.</summary>
    [Test]
    public void Category_lookup()
    {
        var chars = new Chars.Categories[128];

        for (var d = 0; d <= 9; d++)
            chars[d + '0'] |= (Chars.Categories)(1UL << d);

        for (var c = 'A'; c <= 'Z'; c++)
            chars[c] |= Chars.Categories.Upper;

        for (var c = 'a'; c <= 'z'; c++)
            chars[c] |= Chars.Categories.Lower;

        for (char c = default; c <= 127; c++)
        {
            if (char.IsWhiteSpace(c)) chars[c] |= Chars.Categories.Whitespace;
            if (Email.IsLocal(c)) chars[c] |= Chars.Categories.Email_Local;
            if (Email.IsDomain(c)) chars[c] |= Chars.Categories.Email_Domain;
            if (Email.IsTopDomain(c)) chars[c] |= Chars.Categories.Email_TopDomain;
            if (Email.IsPunycode(c)) chars[c] |= Chars.Categories.Email_Punycode;
        }

        foreach (var c in "_-.")
            chars[c] |= Chars.Categories.Markup;

        var sb = new StringBuilder();
        sb.AppendLine("[");
        AppendRows(sb, chars, 0, 127, c => $"0x{(uint)c:x5}");
        sb.AppendLine("];");

        Console.WriteLine(sb);

        Assert.Inconclusive("Copy output to code file.");
    }

    private static void AppendRows(StringBuilder sb, Chars.Categories[] chars, int from, int to, Func<Chars.Categories, string> format)
    {
        for (var i = from; i <= to; i++)
        {
            sb.Append(format(chars[i])).Append(',');
            if ((i & 15) is 15) sb.AppendLine();
            else sb.Append(' ');
        }
    }

    private static class Email
    {
        public static bool IsLocal(char c)
            => c.Is_0_9
            || c.Is_A_Z
            || c.Is_a_z
            || "{}|/%$&#~!?*`'^=+_-".Contains(c);

        public static bool IsDomain(char c)
            => IsTopDomain(c)
            || c.Is_0_9
            || c == '_';

        public static bool IsTopDomain(char c)
            => c.Is_A_Z
            || c.Is_a_z;

        public static bool IsPunycode(char c)
            => IsTopDomain(c)
            || c.Is_0_9
            || c == '-';
    }
}
#endif
