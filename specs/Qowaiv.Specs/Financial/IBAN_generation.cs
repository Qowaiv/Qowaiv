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
        => InternationalBankAccountNumber.TryParse(info.Example).Should().NotBeNull();

    [Test]
    public void Supported_countries_match_overview()
        => InternationalBankAccountNumber.Supported
            .Should().BeEquivalentTo(Infos.Select(i => i.Country));

    // [TestCase("MUkk BOMM nnnn nnnn nnnn nnnn 000Z ZZ", "MU but with a non-existing currency.")]
    // [TestCase("SCkk BANK nnnn nnnn nnnn nnnn nnnn ZZZ", "SC but with a non-existing currency.")]
    [TestCase("USkk cccc aann ", "A non IBAN country with length 12.")]
    [TestCase("USkk cccc cccc nnnn nnnn nnnn nnnn nnnn nnnn", "A non IBAN country with length 36.")]
    public void Generate_IBAN(string bban, string? because = null)
    {
        var rnd = new Random(17);
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
            .Select(InternationalBankAccountNumber.TryParse)
            .First(i => i.HasValue);

        iban.HasValue.Should().BeTrue(because);

        Console.WriteLine(iban.GetValueOrDefault().ToString("F"));
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
