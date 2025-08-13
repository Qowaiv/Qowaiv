using Qowaiv.Globalization;
using System.Text;
using WikiScraper.Models;

namespace WikiScraper.Generators;

public static class Iban
{
    public static async Task Generate()
    {
        var lemma = new WikiLemma("International Bank Account Number", TestCultures.en);

        var all = await lemma.TransformRange(IbanInfo.Parse);

        foreach (var iban in all.OrderByDescending(i => i.Official).ThenBy(i => i.Country.IsoAlpha2Code))
        {
            Console.WriteLine(iban);
        }
    }
}

public sealed partial record IbanInfo
{
    public required Country Country { get; init; }

    public required int Length { get; init; }

    public required string Bban { get; init; }

    public string BbanPattern
    {
        get
        {
            var chars = Fields.Replace(" ", "")[4..].ToCharArray();

            var pos = 0;

            foreach (var block in Bban.Split(','))
            {
                var type = block[^1];
                var length = int.Parse(block[..^1]);

                for (var b = 0; b < length; b++)
                {
                    if (!char.IsAsciiDigit(chars[pos]) && !char.IsUpper(chars[pos]))
                    {
                        chars[pos] = type;
                    }
                    pos++;
                }
            }

            var groups = new List<string>();

            pos = 0;

            while (pos < chars.Length)
            {
                var ch = chars[pos];

                if (IsType(ch))
                {
                    var size = 0;
                    while (pos < chars.Length && chars[pos] == ch)
                    {
                        size++;
                        pos++;
                    }
                    groups.Add($"{size}{ch}");
                }
                else
                {
                    var sb = new StringBuilder().Append('[');
                    while (pos < chars.Length && !IsType(chars[pos]))
                    {
                        sb.Append(chars[pos]);
                        pos++;
                    }
                    groups.Add(sb.Append(']').ToString());
                }
            }

            return string.Join(',', groups);

            static bool IsType(char c) => c is 'n' or 'a' or 'c';
        }
    }

    public required string Fields { get; init; }

    public required YesNo Official { get; init; }

    public required int? CheckSum { get; init; }

    public string Example { get; init; } = string.Empty;

    public override string ToString() => string.Empty
        + $"| {NoBreak(Country.EnglishName),-25} "
        + $"| {Length,5} "
        + $"| {BbanPattern,-15} "
        + $"| {CheckSum,5:00} "
        + $"| {NoBreak(Fields),-41} "
        + $"| {Official,-4:f} "
        + $"| {NoBreak(Example),-41} |";

    static string NoBreak(string s) => s.Replace(' ', (char)160);

    public static IEnumerable<IbanInfo> Parse(string content)
    {
        var groups = content.Split("|-\n|scope=\"row\"|");

        foreach (var group in groups)
        {
            var parts = group.Split(["\n|"], StringSplitOptions.TrimEntries);

            if (parts.Length >= 4 && int.TryParse(parts[1], out var length))
            {
                var fields = Wiki.RemoveFormatting(parts[3]);
                var country = Country.Parse(fields[..2]);

                yield return new IbanInfo
                {
                    Country = country,
                    Length = length,
                    Bban = parts[2].Replace(" ", string.Empty),
                    Fields = fields,
                    CheckSum = parts.Length > 4 ? GetCheckSum(parts[4]) : null,
                    // Only the officiel ones have formatting with colors.
                    Official = parts[3].Contains("color") ? YesNo.Yes : YesNo.No,
                    Example = Examples.FirstOrDefault(e => e[..2] == country.IsoAlpha2Code) ?? string.Empty,
                };
            }
        }

        static int? GetCheckSum(string comment)
            => CheckSumPattern().Match(comment) is { Success: true } match
            ? int.Parse(match.Groups[nameof(CheckSum)].Value)
            : null;
    }

    [GeneratedRegex(@"\(always.+""(?<CheckSum>[0-9][0-9])""\)")]
    private static partial Regex CheckSumPattern();

    // Lookups
    private static readonly string[] Examples =
    [
        "AD12 0001 2030 2003 5910 0100",
        "AE95 0210 0000 0069 3123 456",
        "AL47 2121 1009 0000 0002 3569 8741",
        "AO06 0044 0000 6729 5030 1010 2",
        "AT61 1904 3002 3457 3201",
        "AZ21 NABZ 0000 0000 1370 1000 1944",
        "BA39 1290 0794 0102 8494",
        "BE68 5390 0754 7034",
        "BF42 BF08 4010 1300 4635 7400 0390",
        "BG80 BNBG 9661 1020 3456 78",
        "BH29 BMAG 1299 1234 56BH 00",
        "BI13 2000 1100 0100 0012 3456 789",
        "BJ66 BJ06 1010 0100 1443 9000 0769",
        "BR97 0036 0305 0000 1000 9795 493P 1",
        "BY13 NBRB 3600 9000 0000 2Z00 AB00",
        "CF42 2000 1000 0101 2006 9700 160",
        "CG39 3001 1000 1010 1345 1300 019",
        "CH36 0838 7000 0010 8017 3",
        "CI15 QO48 7501 9424 6931 1090 1733",
        "CM21 1000 2000 3002 7797 6315 008",
        "CR05 0152 0200 1026 2840 66",
        "CV64 0005 0000 0020 1082 1514 4",
        "CY17 0020 0128 0000 0012 0052 7600",
        "CZ65 0800 0000 1920 0014 5399",
        "DE68 2105 0170 0012 3456 78",
        "DJ21 1000 2010 0104 0994 3020 008",
        "DK50 0040 0440 1162 43",
        "DO22 ACAU 0000 0000 0001 2345 6789",
        "DZ58 0002 1000 0111 3000 0005 70",
        "EE38 2200 2210 2014 5685",
        "EG38 0019 0005 0000 0000 2631 8000 2",
        "ES91 2100 0418 4502 0005 1332",
        "FI21 1234 5600 0007 85",
        "FO20 0040 0440 1162 43",
        "FR14 2004 1010 0505 0001 3M02 606",
        "GA21 4002 1010 0320 0189 0020 126",
        "GB46 BARC 2078 9863 2748 45",
        "GE29 NB00 0000 0101 9049 17",
        "GI75 NWBK 0000 0000 7099 453",
        "GL20 0040 0440 1162 43",
        "GQ70 5000 2001 0037 1522 8190 196",
        "GR16 0110 1250 0000 0001 2300 695",
        "GT82 TRAJ 0102 0000 0012 1002 9690",
        "GW04 GW14 3001 0181 8006 3760 1",
        "HN54 PISA 0000 0000 0000 0012 3124",
        "HR12 1001 0051 8630 0016 0",
        "HU42 1177 3016 1111 1018 0000 0000",
        "IE29 AIBK 9311 5212 3456 78",
        "IL62 0108 0000 0009 9999 999",
        "IQ98 NBIQ 8501 2345 6789 012",
        "IR58 0540 1051 8002 1273 1130 07",
        "IS14 0159 2600 7654 5510 7303 39",
        "IT60 X054 2811 1010 0000 0123 456",
        "JO94 CBJO 0010 0000 0000 0131 0003 02",
        "KM46 0000 5000 0100 1090 4400 137",
        "KW81 CBKU 0000 0000 0000 1234 5601 01",
        "KZ75 125K ZT20 6910 0100",
        "LB30 0999 0000 0001 0019 2557 9115",
        "LC55 HEMM 0001 0001 0012 0012 0002 3015",
        "LI21 0881 0000 2324 013A A",
        "LT12 1000 0111 0100 1000",
        "LU28 0019 4006 4475 0000",
        "LV80 BANK 0000 4351 9500 1",
        "LY83 0020 4800 0020 1001 2036 1",
        "MA64 0115 1900 0001 2050 0053 4921",
        "MC11 1273 9000 7000 1111 1000 H79",
        "MD24 AG00 0225 1000 1310 4168",
        "ME25 5050 0001 2345 6789 51",
        "MG46 0000 5030 0712 8942 1016 045",
        "MK07 2501 2000 0058 984",
        "ML13 ML01 6012 0102 6001 0066 8497",
        "MR13 0002 0001 0100 0012 3456 753",
        "MT84 MALT 0110 0001 2345 MTLC AST0 01S",
        "MU17 BOMM 0101 1010 3030 0200 000M UR",
        "MZ97 1234 1234 1234 1234 1234 1",
        "NE58 NE03 8010 0100 1303 0500 0268",
        "NI91 BAMC 0112 0203 0000 0355 8124",
        "NL20 INGB 0001 2345 67",
        "NO93 8601 1117 947",
        "OM34 0180 0104 7042 8485 001",
        "PK36 SCBL 0000 0011 2345 6702",
        "PL61 1090 1014 0000 0712 1981 2874",
        "PS92 PALS 0000 0000 0400 1234 5670 2",
        "PT50 0002 0123 1234 5678 9015 4",
        "QA58 DOHB 0000 1234 5678 90AB CDEF G",
        "RO49 AAAA 1B31 0075 9384 0000",
        "RS35 2600 0560 1001 6113 79",
        "RU02 0445 2560 0407 0281 0412 3456 7890 1",
        "SA84 4000 0108 0540 1173 0013",
        "SC18 SSCB 1101 0000 0000 0000 1497 USD",
        "SD21 2901 0501 2340 01",
        "SE35 5000 0000 0549 1000 0003",
        "SI56 1910 0000 0123 438",
        "SK31 1200 0000 1987 4263 7541",
        "SM86 U032 2509 8000 0000 0270 100",
        "SN05 TI80 0835 4151 5881 3959 8706",
        "SO31 0011 0010 0172 1000 063",
        "ST23 0001 0001 0051 8453 1014 6",
        "SV62 CENR 0000 0000 0000 0070 0025",
        "TD89 6000 2000 0102 7109 1600 153",
        "TG53 TG00 9060 4310 3465 0040 0070",
        "TL38 0010 0123 4567 8910 106",
        "TN59 1000 6035 1835 9847 8831",
        "TR33 0006 1005 1978 6457 8413 26",
        "UA21 3996 2200 0002 6007 2335 6600 1",
        "VA59 0011 2300 0012 3456 78",
        "VG96 VPVG 0000 0123 4567 8901",
        "XK05 1212 0123 4567 8906",
        "YE09 YECO 0016 0000 0000 1234 5601 01",
    ];
}
