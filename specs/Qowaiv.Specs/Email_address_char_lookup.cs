namespace Email_address_char_lookup;

public class Generate
{
#if !DEBUG
    [Ignore("Generates code. Irrelevant for the build.")]
#endif
    [Test]
    public void Category()
    {
        var sb = new StringBuilder();
        sb.AppendLine("[");
        for (uint c = 0; c <= 127; c++)
        {
            var ch = (char)c;

            var cat = 0;
            cat |= IsLocal(ch) ? 1: 0;
            cat |= IsDomain(ch) ? 2 : 0;
            cat |= IsTopDomain(ch) ? 4 : 0;
            cat |= IsPunycode(ch) ? 8 : 0;

            sb.Append($"0x0{Convert.ToString(cat, 16)},");
            if(c % 16 == 15)
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

        sb.Should().NotBeNull();

    }

    private static bool IsLocal(char c)
        => ASCII.IsDigit(c)
        || ASCII.IsLetter(c)
        || "{}|/%$&#~!?*`'^=+._-".Contains(c);

    private static bool IsDomain(char c)
        => IsTopDomain(c)
        || ASCII.IsDigit(c)
        || c == '_';

    private static bool IsTopDomain(char c) => ASCII.IsLetter(c);

    private static bool IsPunycode(char c)
        => IsTopDomain(c)
        || ASCII.IsDigit(c)
        || c == '-';
}

