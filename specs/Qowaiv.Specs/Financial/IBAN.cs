using System.Runtime.InteropServices;

namespace Financial.IBAN;

public class Markdown_file
{
    private static readonly FileInfo Location = new("../../../../../IBAN.md");

    [Test]
    public void Exists() => Location.Exists.Should().BeTrue();

    [Test]
    public void Regenerates()
    {
        foreach(var info in Infos.OrderByDescending(i => i.Official).ThenBy(i => i.Country))
        {
            Console.WriteLine(info.Markdown());
        }
        Assert.Inconclusive("Manually check output.");
    }

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
    public string Markdown()
    {
        return $"| {Country,-25} | {Length,5} | {Bban,-12} | {CheckSum,5:00} | {Fields,-41} | {Official,-4:f} | {Example,-41} |";
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
