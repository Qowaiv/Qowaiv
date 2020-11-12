using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using System.Linq;

namespace IBAN_specs
{
    public class Supported
    {
        [Test]
        public void by_102_Countries()
        {
            var supported = new[] 
            {
                Country.AD,
                Country.AE,
                Country.AL,
                Country.AO,
                Country.AT,
                Country.AZ,
                Country.BA,
                Country.BE,
                Country.BF,
                Country.BG,
                Country.BH,
                Country.BI,
                Country.BJ,
                Country.BR,
                Country.BY,
                Country.CG,
                Country.CH,
                Country.CI,
                Country.CM,
                Country.CR,
                Country.CV,
                Country.CY,
                Country.CZ,
                Country.DE,
                Country.DJ,
                Country.DK,
                Country.DO,
                Country.DZ,
                Country.EE,
                Country.EG,
                Country.ES,
                Country.FI,
                Country.FO,
                Country.FR,
                Country.GA,
                Country.GB,
                Country.GE,
                Country.GI,
                Country.GL,
                Country.GQ,
                Country.GR,
                Country.GT,
                Country.GW,
                Country.HN,
                Country.HR,
                Country.HU,
                Country.IE,
                Country.IL,
                Country.IQ,
                Country.IR,
                Country.IS,
                Country.IT,
                Country.JO,
                Country.KM,
                Country.KW,
                Country.KZ,
                Country.LB,
                Country.LC,
                Country.LI,
                Country.LT,
                Country.LU,
                Country.LV,
                Country.MA,
                Country.MC,
                Country.MD,
                Country.ME,
                Country.MG,
                Country.MK,
                Country.ML,
                Country.MR,
                Country.MT,
                Country.MU,
                Country.MZ,
                Country.NE,
                Country.NI,
                Country.NL,
                Country.NO,
                Country.PK,
                Country.PL,
                Country.PS,
                Country.PT,
                Country.QA,
                Country.RO,
                Country.RS,
                Country.SA,
                Country.SC,
                Country.SE,
                Country.SI,
                Country.SK,
                Country.SM,
                Country.SN,
                Country.ST,
                Country.SV,
                Country.TD,
                Country.TG,
                Country.TL,
                Country.TN,
                Country.TR,
                Country.UA,
                Country.VA,
                Country.VG,
                Country.XK,
            };

            Assert.AreEqual(supported, InternationalBankAccountNumber.Supported.OrderBy(c => c.IsoAlpha2Code));
            Assert.AreEqual(102, InternationalBankAccountNumber.Supported.Count);
        }
    }

    public class Input
    {
        [TestCase("?", "Unknown")]
        [TestCase("AE950 2100000006  93123456", "United Arab Emirates ()")]
        [TestCase("AE95 0210 0000 0069 3123 456", "United Arab Emirates")]
        [TestCase("AL47 2121 1009 0000 0002 3569 8741", "Albania")]
        [TestCase("AD12 0001 2030 2003 5910 0100", "Andorra")]
        [TestCase("AT61 1904 3002 3457 3201", "Austria")]
        [TestCase("BA39 1290 0794 0102 8494", "Bosnia and Herzegovina")]
        [TestCase("BE43 0689 9999 9501", "Belgium")]
        [TestCase("BG80 BNBG 9661 1020 3456 78", "Bulgaria")]
        [TestCase("BH29 BMAG 1299 1234 56BH 00", "Bahrain")]
        [TestCase("BY13 NBRB 3600 9000 0000 2Z00 AB00", "Belarus")]
        [TestCase("CH36 0838 7000 0010 8017 3", "Switzerland")]
        [TestCase("CY17 0020 0128 0000 0012 0052 7600", "Cyprus")]
        [TestCase("CZ65 0800 0000 1920 0014 5399", "Czech Republic")]
        [TestCase("DE68 2105 0170 0012 3456 78", "Germany")]
        [TestCase("DK50 0040 0440 1162 43", "Denmark")]
        [TestCase("DO28 BAGR 0000 0001 2124 5361 1324", "Dominican Republic")]
        [TestCase("EE38 2200 2210 2014 5685", "Estonia")]
        [TestCase("ES91 2100 0418 4502 0005 1332", "Spain")]
        [TestCase("FI21 1234 5600 0007 85", "Finland")]
        [TestCase("FO20 0040 0440 1162 43", "Faroe Islands")]
        [TestCase("FR14 2004 1010 0505 0001 3M02 606", "Frankrijk")]
        [TestCase("GB82 WEST 1234 5698 7654 32", "United Kingdom")]
        [TestCase("GE29 NB00 0000 0101 9049 17", "Georgia")]
        [TestCase("GI75 NWBK 0000 0000 7099 453", "Gibraltar")]
        [TestCase("GL20 0040 0440 1162 43", "Greenland")]
        [TestCase("GR16 0110 1250 0000 0001 2300 695", "Greece")]
        [TestCase("HR12 1001 0051 8630 0016 0", "United Kingdom")]
        [TestCase("HU42 1177 3016 1111 1018 0000 0000", "Hungary")]
        [TestCase("IE29 AIBK 9311 5212 3456 78", "Ireland")]
        [TestCase("IL62 0108 0000 0009 9999 999", "Israel")]
        [TestCase("IS14 0159 2600 7654 5510 7303 39", "Iceland")]
        [TestCase("IT60 X054 2811 1010 0000 0123 456", "Italy")]
        [TestCase("KW81 CBKU 0000 0000 0000 1234 5601 01", "Kuwait")]
        [TestCase("KZ75 125K ZT20 6910 0100", "Kazakhstan")]
        [TestCase("LB30 0999 0000 0001 0019 2557 9115", "Lebanon")]
        [TestCase("LI21 0881 0000 2324 013A A", "Liechtenstein")]
        [TestCase("LT12 1000 0111 0100 1000", "Lithuania")]
        [TestCase("LU28 0019 4006 4475 0000", "Luxembourg")]
        [TestCase("LV80 BANK 0000 4351 9500 1", "Latvia")]
        [TestCase("MC11 1273 9000 7000 1111 1000 H79", "Monaco")]
        [TestCase("ME25 5050 0001 2345 6789 51", "Montenegro")]
        [TestCase("MK07 2501 2000 0058 984", "Macedonia")]
        [TestCase("MR13 0002 0001 0100 0012 3456 753", "Mauritania")]
        [TestCase("MT84 MALT 0110 0001 2345 MTLC AST0 01S", "Malta")]
        [TestCase("MU17 BOMM 0101 1010 3030 0200 000M UR", "Mauritius")]
        [TestCase("NL20 INGB 0001 2345 67", "Netherlands")]
        [TestCase("NL44 RABO 0123 4567 89", "Netherlands")]
        [TestCase("NO93 8601 1117 947", "Norway")]
        [TestCase("PL61 1090 1014 0000 0712 1981 2874", "Poland")]
        [TestCase("PT50 0002 0123 1234 5678 9015 4", "Portugal")]
        [TestCase("RO49 AAAA 1B31 0075 9384 0000", "Romania")]
        [TestCase("RS35 2600 0560 1001 6113 79", "Romania")]
        [TestCase("SA84 4000 0108 0540 1173 0013", "Saudi Arabia")]
        [TestCase("SE35 5000 0000 0549 1000 0003", "Sweden")]
        [TestCase("SI56 1910 0000 0123 438", "Slovenia")]
        [TestCase("SK31 1200 0000 1987 4263 7541", "Slovakia")]
        [TestCase("SM86 U032 2509 8000 0000 0270 100", "San Marino")]
        [TestCase("TL38 0010 0123 4567 8910 106", "Timor Leste")]
        [TestCase("TN59 1000 6035 1835 9847 8831", "Tunisia")]
        [TestCase("TR33 0006 1005 1978 6457 8413 26", "Turkey")]
        [TestCase("UA21 3996 2200 0002 6007 2335 6600 1", "Ukraine")]
        [TestCase("VA59 0011 2300 0012 3456 78", "Vatican City")]
        public void is_valid_for(string input, string description)
        {
            Assert.IsTrue(InternationalBankAccountNumber.IsValid(input), description);
        }

        [TestCase("", "string.Empty")]
        [TestCase(null, "(String)null")]
        [TestCase("XX950210000000693123456", "Not existing country.")]
        [TestCase("Garbage in, garbage out", "Wrong pattern.")]
        [TestCase("NL20INGB007", "Too short.")]
        [TestCase("NL20INGB000123456Z", "Wrong Dutch sub pattern.")]
        [TestCase("XX20INGB0001234567", "Non-existing country.")]
        [TestCase("US20INGB0001234567", "Country without pattern.")]
        public void is_invalid_for(string input, string description)
        {
            Assert.IsFalse(InternationalBankAccountNumber.IsValid(input), description);
        }
    }
}
