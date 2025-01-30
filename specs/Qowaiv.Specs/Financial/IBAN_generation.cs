namespace Financial.IBAN_generation;

internal class Markdown_file
{
    private static readonly FileInfo Location = new("../../../../../IBAN.md");

    [Test]
    public void Exists() => Location.Exists.Should().BeTrue();

    [Test]
    public void Generate_Markdown()
    {
        foreach (var info in Infos.OrderByDescending(i => i.Official).ThenBy(i => i.Country))
        {
            Console.WriteLine(info.Markdown());
        }
        Assert.Inconclusive("Copy output to code file.");
    }

    [Test]
    public void Generate_LocalizedPattern()
    {
        foreach (var info in Infos.OrderBy(i => i.Example))
        {
            Console.WriteLine(info.LocalizedPattern());
        }
        Assert.Inconclusive("Copy output to code file.");
    }

    [Test]
    public void Generate_JS_RegEx_Pattern()
    {
        foreach (var info in Infos.OrderBy(i => i.Example))
        {
            Console.WriteLine(info.JsRegex());
        }
        Assert.Inconclusive("Copy output to code file.");
    }

    [TestCaseSource(nameof(Infos))]
    public void BBan_length_equals_length(IbanInfo info)
        => info.BbanLength.Should().Be(info.Length);

    [TestCaseSource(nameof(Infos))]
    public void Reserved_zeros_match(IbanInfo info)
    {
        var fields = Regex.Split(info.Fields, "[^0]").Where(s => s.Length > 0).ToArray();
        var bban = new Regex(string.Join(".*", fields.Select(f => $"\\[{f}\\]")));

        bban.IsMatch(info.Bban).Should().BeTrue(because: $"Zero's of {info.Bban} and {info.Fields} should match");
    }

    [TestCaseSource(nameof(Infos))]
    public void Example_is_valid(IbanInfo info)
        => InternationalBankAccountNumber.TryParse(info.Example).Should().NotBeNull(because: $"{info.Example} should be valid for {info.CountryName}.");

    [Test]
    public void Supported_countries_match_overview()
        => InternationalBankAccountNumber.Supported
            .Should().BeEquivalentTo(Infos.Select(i => i.Country));

    [TestCase("ALkk 2121 1007 nnnn nnnn nnnn nnnn", "AL with invalid weighted checksum.")]
    [TestCase("BEkk nnnn nnnn nnnn", "BE with invalid MOD97 checksum.")]
    [TestCase("CZkk nnnn nnnn nnnn nnnn nnnn", "CZ with invalid MOD11 10 checksum.")]
    [TestCase("EEkk 0nnn nnnn nnnn nnnn", "EE bankcode can not start with 0.")]
    [TestCase("EEkk nnnn nnnn nnnn nnnn", "EE with invalid checksum.")]
    [TestCase("FIkk 1234 5600 0007 89", "FI with invalid Luhn checksum.")]
    [TestCase("MUkk BOMM nnnn nnnn nnnn nnnn 000Z ZZ", "MU with a non-existing currency.")]
    [TestCase("NLkk aaaa nnnn nnnn n", "NL too short.")]
    [TestCase("NLkk aaaa nnnn nnnn nnn", "NL too long.")]
    [TestCase("SCkk BANK nnnn nnnn nnnn nnnn nnnn ZZZ", "SC with a non-existing currency.")]
    [TestCase("USkk cccc aann ", "A non IBAN country with length 12.")]
    [TestCase("USkk cccc cccc nnnn nnnn nnnn nnnn nnnn nnnn", "A non IBAN country with length 36.")]
    [TestCase("BAkk nnnn nnnn nnnn nnnn", "BA with non-fixed checksum.")]
    [TestCase("MEkk nnnn nnnn nnnn nnnn nn", "ME with non-fixed checksum.")]
    [TestCase("MKkk nnnn nnnn nnnn nnn", "MK with non-fixed checksum.")]
    [TestCase("MRkk nnnn nnnn nnnn nnnn nnnn nnn", "MR with non-fixed checksum.")]
    [TestCase("PLkk nnnn nnnn nnnn nnnn nnnn nnnn", "PL with invalid checksum.")]
    [TestCase("PTkk nnnn nnnn nnnn nnnn nnnn n", "PT with non-fixed checksum.")]
    [TestCase("RSkk nnnn nnnn nnnn nnnn nn", "RS with non-fixed checksum.")]
    [TestCase("SIkk nnnn nnnn nnnn nnn", "SI with non-fixed checksum.")]
    [TestCase("SKkk nnnn nnnn nnnn nnnn nnnn", "SK with invalid MOD11 10 checksum.")]
    [TestCase("TLkk nnnn nnnn nnnn nnnn nnn", "TL with non-fixed checksum.")]
    [TestCase("TNkk nnnn nnnn nnnn nnnn nnnn", "TN with non-fixed checksum.")]
    [TestCase("CIkkaannnnnnnnnnnnnnnnnnnnnn")]
    public void Generate_IBAN(string bban, string? because = null)
    {
        var rnd = new Random(bban.GetHashCode());
        var sb = new StringBuilder();
        foreach (var ch in bban)
        {
            var digit = rnd.Next(0, 10).ToString()[0];
            var letter = (char)('A' + rnd.Next(0, 26));
            switch (ch)
            {
                case ' ': break;
                case 'a': sb.Append(letter); break;
                case 'c': sb.Append(rnd.NextDouble() > 0.5 ? digit : letter); break;
                case 'n': sb.Append(digit); break;
                default: sb.Append(ch); break;
            }
        }

        var str = sb.ToString();

        var iban = Enumerable.Range(0, 100)
            .Select(kk => str.Replace("kk", kk.ToString("00")))
            .Select(Mod97)
            .First(i => i is { });

        iban.Should().NotBeNull(because);

        Console.WriteLine(iban);
    }

    private static readonly IbanInfo[] Infos = Init();

    private static IbanInfo[] Init()
    {
        using var reader = Location.OpenText();

        var infos = new List<IbanInfo>();

        while (reader.ReadLine() is { } line)
        {
            if (IbanInfo.Parse(line) is { } info)
            {
                infos.Add(info);
            }
        }
        return [.. infos];
    }

    private static string? Mod97(string iban)
    {
        iban = iban.Replace(" ", "");

        var mod = 0;
        for (var i = 0; i < iban.Length; i++)
        {
            // Calculate the first 4 characters (country and checksum) last
            var ch = iban[(i + 4) % iban.Length];
            var index = Mod97(ch);
            mod *= index > 9 ? 100 : 10;
            mod += index;
            mod %= 97;
        }
        return mod == 1 ? string.Join(" ", Chunk(iban)) : null;
    }

    [Pure]
    private static int Mod97(char ch)
        => ch <= '9'
            ? ch - '0'
            : ch - 'A' + 10;

    private static IEnumerable<string> Chunk(string str)
    {
        for (var i = 0; i < str.Length; i += 4)
        {
            yield return str.Length - i > 4
                ? str.Substring(i, 4)
                : str[i..];
        }
    }
}

internal sealed record IbanInfo(string CountryName, int Length, string Bban, int? CheckSum, string Fields, YesNo Official, string Example)
{
    public Country Country => Country.Parse(Example[..2]);

    public int BbanLength
    {
        get
        {
            var total = 4;
            foreach (var block in Bban.Split(','))
            {
                total += int.TryParse(block[..^1], out int length) && block[^1] != '}'
                    ? length
                    : block.Length - 2;
            }

            return total;
        }
    }

    public string Markdown()
        => $"| {NoBreak(CountryName),-25} | {Length,5} | {Bban,-15} | {CheckSum,5:00} | {NoBreak(Fields),-41} | {Official,-4:f} | {NoBreak(Example),-41} |";

    internal string LocalizedPattern()
        => CheckSum is { } c
        ? $@"Bban(Country.{Example[..2]}, ""{Bban}"", checksum: {c:00}),"
        : $@"Bban(Country.{Example[..2]}, ""{Bban}""),";

    public string JsRegex()
    {
        var sb = new StringBuilder("['")
            .Append(Country.IsoAlpha2Code)
            .Append("', /^");
        
        var first = true;

        foreach (var part in Bban.Split(','))
        {
            // put in checksum and/or merge it with the first part.
            if (first)
            {
                first = false;
                if (CheckSum is { } c)
                {
                    sb.Append(c.ToString("00"));
                }
                // on a merge, we continue.
                else if (part[^1] == 'n')
                {
                    var n = int.Parse(part[..^1]) + 2;
                    sb.Append($"[0-9]{{{n}}}");
                    continue;
                }
                else
                {
                    sb.Append("[0-9]{2}");
                }
            }


            sb.Append(part[^1] switch
            {
                'n' => $"[0-9]{{{part[..^1]}}}",
                'a' => $"[A-Z]{{{part[..^1]}}}",
                'c' => $"[A-Z0-9]{{{part[..^1]}}}",
                _ => part[1..^1],
            });
        }

        return sb
            .Append("$/],")
            // We do not have to make a single instance that explicit.
            .Replace("{1}", string.Empty)
            .ToString();
    }

    public override string ToString() => Example;

    public static IbanInfo? Parse(string line)
    {
        var parts = line.Split('|');
        if (parts.Length < 9) return null;
        parts = parts[1..];
        try
        {
            int? checksum = parts[3].Trim() is { Length: > 0 } t ? int.Parse(t) : null;
            return new(
                CountryName: parts[0].Trim(),
                Length: int.Parse(parts[1].Trim()),
                Bban: parts[2].Replace(" ", ""),
                CheckSum: checksum,
                Fields: parts[4].Trim(),
                Official: YesNo.Parse(parts[5]),
                Example: parts[6].Trim());
        }
        catch (Exception x)
        {
            Console.Error.WriteLine($"{x.Message}: {line}");
            return null;
        }
    }

    static string NoBreak(string s) => s.Replace(' ', (char)160);
}
