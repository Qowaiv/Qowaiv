using Qowaiv.Text;

namespace Text.Chars_generation;

[Explicit]
public class Generates
{
    /// <summary>Generates the Chars category lookup.</summary>
    [Test]
    public void Catagory_loopup()
    {
        var chars = new Chars.Catagories[256];

        // Digits
        for (var d = 0; d <= 9; d++)
            chars[d + '0'] |= (Chars.Catagories)(1Ul << d);

        for (var c = 'A'; c <= 'Z'; c++)
            chars[c] |= Chars.Catagories.Upper;

        for (var c = 'a'; c <= 'z'; c++)
            chars[c] |= Chars.Catagories.Lower;

        for (char c = default; c <= 127; c++)
        {
            if (char.IsWhiteSpace(c)) chars[c] |= Chars.Catagories.Whitespace;
            if (Email.IsLocal(c)) chars[c] |= Chars.Catagories.Email_Local;
            if (Email.IsDomain(c)) chars[c] |= Chars.Catagories.Email_Domain;
            if (Email.IsTopDomain(c)) chars[c] |= Chars.Catagories.Email_TopDomain;
            if (Email.IsPunycode(c)) chars[c] |= Chars.Catagories.Email_Punycode;
        }

        foreach (var c in "_-.")
            chars[c] |= Chars.Catagories.Markup;

        var sb = new StringBuilder();
        sb.AppendLine("[");
        for (uint i = 0; i <= 127; i++)
        {
            if (i is 128)
                sb.AppendLine("// Non-ASCII");

            var cat = chars[i];
            var hex = $"{Convert.ToString((uint)cat, 16)}";

            sb.Append("0x").Append('0', 5 - hex.Length).Append(hex).Append(',');
            if (i % 16 == 15)
            {
                sb.AppendLine();
            }
            else
            {
                sb.Append(' ');
            }
        }
        
        sb.AppendLine().AppendLine("// Non-ASCII");

        for (uint i = 128; i <= 255; i++)
        {
        
            var cat = chars[i];
            sb.Append((uint)cat).Append(',');
            if (i % 16 == 15)
            {
                sb.AppendLine();
            }
            else
            {
                sb.Append(' ');
            }
        }

        sb.AppendLine("];");

        Console.WriteLine(sb);

        Assert.Inconclusive("Copy output to code file.");
    }

    private class Email
    {
        public static bool IsLocal(char c)
            => Chars.get_Is_0_9(c)
            || Chars.get_Is_A_Z(c)
            || Chars.get_Is_a_z(c)
            || "{}|/%$&#~!?*`'^=+_-".Contains(c);

        public static bool IsDomain(char c)
            => IsTopDomain(c)
            || Chars.get_Is_0_9(c)
            || c == '_';

        public static bool IsTopDomain(char c)
            => Chars.get_Is_A_Z(c)
            || Chars.get_Is_a_z(c);

        public static bool IsPunycode(char c)
            => IsTopDomain(c)
            || Chars.get_Is_0_9(c)
            || c == '-'; 
    }
}
