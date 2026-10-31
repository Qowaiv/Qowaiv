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
            if (char.IsWhiteSpace(c)) chars[c] |= Chars.Catagories.Whitespace;

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

            sb.Append("0x").Append('0', 4 - hex.Length).Append(hex).Append(',');
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
}
