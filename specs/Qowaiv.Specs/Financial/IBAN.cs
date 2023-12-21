namespace Financial.IBAN;

public class Markdown_file
{
    private static readonly FileInfo Location = new("../../../../../IBAN.md");

    [Test]
    public void Exists() => Location.Exists.Should().BeTrue();

    [Test]
    public void Generate_Markdown()
    {
        foreach(var info in Infos.OrderByDescending(i => i.Official).ThenBy(i => i.Country))
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
    public void Example_is_valid(IbanInfo info)
        => InternationalBankAccountNumber.TryParse(info.Example).Should().NotBeNull();

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

public sealed record IbanInfo(string Country, int Length, string Bban, int? CheckSum, string Fields, YesNo Official, string Example)
{
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
        => $"| {Country,-25} | {Length,5} | {Bban,-15} | {CheckSum,5:00} | {Fields,-41} | {Official,-4:f} | {Example,-41} |";

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
                Country: parts[0].Trim(),
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

    
}
