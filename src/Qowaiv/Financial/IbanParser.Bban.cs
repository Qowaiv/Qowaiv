using Qowaiv.Globalization;

namespace Qowaiv.Financial;

internal static partial class IbanParser
{
    internal static readonly BbanParser[] Parsers = Init();

    [Pure]
    private static BbanParser[] Init()
    {
        var validators = new BbanParser[26 * 26];

        var generic = new BbanGenericParser();

        foreach (var country in Country.GetExisting())
        {
            validators[Id(country)] = generic;
        }

        foreach (var bban in Bbans)
        {
            validators[bban.Index] = bban.Code switch
            {
                "AL" => new BbanAlbaniaParser(bban.Pattern),
                "FI" => new BbanFinlandParser(bban.Pattern),
                "MU" or "SC" => new BbanWithCurrencyCodeParser(bban.Pattern),
                _ => new BbanParser(bban.Pattern),
            };
        }
        return validators;
    }

    [Pure]
    private static int Id(Country country) => ((country.Name[0] - 'A') * 26) + country.Name[1] - 'A';

    private record struct BbanData(int Index, string Pattern)
    {
        public string Code => Pattern[..2];
    }

    [Pure]
    private static BbanData Bban(Country country, string bban, int? checksum = null)
    {
        var pattern = new StringBuilder(32)
           .Append(country.IsoAlpha2Code)
           .Append(checksum.HasValue ? checksum.Value.ToString("00") : "nn");

        foreach (var block in bban.Split(','))
        {
            var type = block.Last();

            if (!int.TryParse(block[..^1], out int length) && type == ']')
            {
                pattern.Append(block[1..^1]);
            }
            else
            {
                pattern.Append(new string(type, length));
            }
        }
        return new BbanData(Id(country), pattern.ToString());
    }

    [Pure]
    private static BbanData[] Bbans =>
    [
        Bban(Country.AD, "8n,12c"),
        Bban(Country.AE, "3n,16n"),
        Bban(Country.AL, "8n,16c"),
        Bban(Country.AO, "21n"),
        Bban(Country.AT, "16n"),
        Bban(Country.AZ, "4a,20c"),
        Bban(Country.BA, "16n", checksum: 39),
        Bban(Country.BE, "12n"),
        Bban(Country.BF, "2c,22n"),
        Bban(Country.BG, "4a,6n,8c"),
        Bban(Country.BH, "4a,14c"),
        Bban(Country.BI, "5n,5n,11n,2n"),
        Bban(Country.BJ, "2c,22n"),
        Bban(Country.BR, "23n,1a,1c"),
        Bban(Country.BY, "4c,4n,16c"),
        Bban(Country.CF, "23n"),
        Bban(Country.CG, "23n"),
        Bban(Country.CH, "5n,12c"),
        Bban(Country.CI, "1a,23n"),
        Bban(Country.CM, "23n"),
        Bban(Country.CR, "[0],17n"),
        Bban(Country.CV, "21n"),
        Bban(Country.CY, "8n,16c"),
        Bban(Country.CZ, "20n"),
        Bban(Country.DE, "18n"),
        Bban(Country.DJ, "23n"),
        Bban(Country.DK, "14n"),
        Bban(Country.DO, "4c,20n"),
        Bban(Country.DZ, "22n"),
        Bban(Country.EE, "16n"),
        Bban(Country.EG, "25n"),
        Bban(Country.ES, "20n"),
        Bban(Country.FI, "14n"),
        Bban(Country.FO, "14n"),
        Bban(Country.FR, "10n,11c,2n"),
        Bban(Country.GA, "23n"),
        Bban(Country.GB, "4a,14n"),
        Bban(Country.GE, "2a,16n"),
        Bban(Country.GI, "4a,15c"),
        Bban(Country.GL, "14n"),
        Bban(Country.GQ, "23n"),
        Bban(Country.GR, "7n,16c"),
        Bban(Country.GT, "4c,20c"),
        Bban(Country.GW, "2c,19n"),
        Bban(Country.HN, "4a,20n"),
        Bban(Country.HR, "17n"),
        Bban(Country.HU, "24n"),
        Bban(Country.IE, "4a,14n"),
        Bban(Country.IL, "19n"),
        Bban(Country.IQ, "4a,15n"),
        Bban(Country.IR, "[0],2n,[0],18n"),
        Bban(Country.IS, "22n"),
        Bban(Country.IT, "1a,10n,12c"),
        Bban(Country.JO, "4a,4n,18c"),
        Bban(Country.KM, "23n"),
        Bban(Country.KW, "4a,22c"),
        Bban(Country.KZ, "3n,13c"),
        Bban(Country.LB, "4n,20c"),
        Bban(Country.LC, "4a,24c"),
        Bban(Country.LI, "5n,12c"),
        Bban(Country.LT, "16n"),
        Bban(Country.LU, "3n,13c"),
        Bban(Country.LV, "4a,13c"),
        Bban(Country.LY, "21n"),
        Bban(Country.MA, "24n"),
        Bban(Country.MC, "10n,11c,2n"),
        Bban(Country.MD, "2c,18c"),
        Bban(Country.ME, "18n", checksum: 25),
        Bban(Country.MG, "23n"),
        Bban(Country.MK, "3n,10c,2n", checksum: 07),
        Bban(Country.ML, "2c,22n"),
        Bban(Country.MR, "23n", checksum: 13),
        Bban(Country.MT, "4a,5n,18c"),
        Bban(Country.MU, "4a,16n,[000],3a"),
        Bban(Country.MZ, "21n"),
        Bban(Country.NE, "2a,22n"),
        Bban(Country.NI, "4a,24n"),
        Bban(Country.NL, "4a,10n"),
        Bban(Country.NO, "11n"),
        Bban(Country.PK, "4a,16c"),
        Bban(Country.PL, "24n"),
        Bban(Country.PS, "4a,21c"),
        Bban(Country.PT, "21n", checksum: 50),
        Bban(Country.QA, "4a,21c"),
        Bban(Country.RO, "4a,16c"),
        Bban(Country.RS, "18n", checksum: 35),
        Bban(Country.RU, "14n,15c"),
        Bban(Country.SA, "2n,18c"),
        Bban(Country.SC, "4a,20n,3a"),
        Bban(Country.SD, "14n"),
        Bban(Country.SE, "20n"),
        Bban(Country.SI, "15n", checksum: 56),
        Bban(Country.SK, "20n"),
        Bban(Country.SM, "1a,10n,12c"),
        Bban(Country.SN, "2a,22n"),
        Bban(Country.ST, "21n"),
        Bban(Country.SV, "4a,20n"),
        Bban(Country.TD, "23n"),
        Bban(Country.TG, "2a,22n"),
        Bban(Country.TL, "19n", checksum: 38),
        Bban(Country.TN, "20n", checksum: 59),
        Bban(Country.TR, "5n,[0],16c"),
        Bban(Country.UA, "6n,19c"),
        Bban(Country.VA, "3n,15n"),
        Bban(Country.VG, "4a,16n"),
        Bban(Country.XK, "4n,10n,2n"),
    ];
}
