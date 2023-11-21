namespace IBAN_specs;

public class Supported
{
    [Test]
    public void by_103_Countries()
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
                Country.LY,
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
        Assert.AreEqual(103, InternationalBankAccountNumber.Supported.Count);
    }
}

public class With_domain_logic
{
    [TestCase(true, "NL20INGB0001234567")]
    [TestCase(true, "?")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, InternationalBankAccountNumber svo) => svo.HasValue.Should().Be(result);

    [TestCase(true, "NL20INGB0001234567")]
    [TestCase(false, "?")]
    [TestCase(false, "")]
    public void IsKnown_is(bool result, InternationalBankAccountNumber svo) => svo.IsKnown.Should().Be(result);
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Iban.CompareTo(Nil.Object).Should().Be(1);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(InternationalBankAccountNumber).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.FromNull<string>().To<InternationalBankAccountNumber>().Should().Be(InternationalBankAccountNumber.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From(string.Empty).To<InternationalBankAccountNumber>().Should().Be(InternationalBankAccountNumber.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("NL20 INGB 0001 2345 67").To<InternationalBankAccountNumber>().Should().Be(Svo.Iban);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.ToString().From(Svo.Iban).Should().Be("NL20INGB0001234567");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
    [TestCase(null, null)]
    [TestCase("NL20INGB0001234567", "NL20INGB0001234567")]
    public void System_Text_JSON_deserialization(object json, InternationalBankAccountNumber svo)
        => JsonTester.Read_System_Text_JSON<InternationalBankAccountNumber>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("NL20INGB0001234567", "NL20INGB0001234567")]
    public void System_Text_JSON_serialization(InternationalBankAccountNumber svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("NL20INGB0001234567", "NL20INGB0001234567")]
    public void convention_based_deserialization(object json, InternationalBankAccountNumber svo)
        => JsonTester.Read<InternationalBankAccountNumber>(json).Should().Be(svo);

    [TestCase(null, null)]
    [TestCase("NL20INGB0001234567", "NL20INGB0001234567")]
    public void convention_based_serialization(InternationalBankAccountNumber svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
    {
        var exception = Assert.Catch(() => JsonTester.Read<InternationalBankAccountNumber>(json));
        Assert.IsInstanceOf(exceptionType, exception);
    }
}


public class Input_is_invalid_when
{
    [Test]
    public void country_does_not_exist()
        => InternationalBankAccountNumber.TryParse("XX950210000000693123456").Should().BeNull();

    [Test]
    public void shorter_than_12()
        => InternationalBankAccountNumber.TryParse("NL20INGB007").Should().BeNull();

    [Test]
    public void longer_than_32()
        => InternationalBankAccountNumber.TryParse("NL20 INGB 0070 3456 7890 1234 5678 9012 1").Should().BeNull();

    [Test]
    public void other_than_alpha_numeric()
        => InternationalBankAccountNumber.TryParse("AE20 #$12 0070 3456 7890 1234 5678").Should().BeNull();

    [Test]
    public void country_does_not_support_IBAN()
        => InternationalBankAccountNumber.TryParse("US20INGB0001234567").Should().BeNull();
}

public class Input_is_valid
{
    [Test]
    public void despite_irregular_white_spacing()
        => InternationalBankAccountNumber.Parse("AE950 2100000006  93123456").IsEmptyOrUnknown().Should().BeFalse();

    [TestCaseSource(nameof(ValidForCountry))]
    public void for_country(Country country, string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            Assert.Inconclusive($"No IBAN test for {country.EnglishName} ({country.IsoAlpha2Code}).");
        }
        else
        {
            Assert.IsTrue(InternationalBankAccountNumber.TryParse(input, out InternationalBankAccountNumber iban),
                $"{input} is invalid for {country.EnglishName}.");
            Assert.AreEqual(country, iban.Country);
        }
    }

    [Test]
    public void for_country_has_tests_for_all_supported()
    {
        var supported = ValidForCountry.Select(array => array[0]).Cast<Country>();
        Assert.AreEqual(supported, InternationalBankAccountNumber.Supported.OrderBy(c => c.IsoAlpha2Code));
    }

    private static readonly object[][] ValidForCountry =
    [
        [Country.AD, "AD12 0001 2030 2003 5910 0100"],
        [Country.AE, "AE95 0210 0000 0069 3123 456"],
        [Country.AL, "AL47 2121 1009 0000 0002 3569 8741"],
        [Country.AO, "AO06 0044 0000 6729 5030 1010 2"],
        [Country.AT, "AT61 1904 3002 3457 3201"],
        [Country.AZ, "AZ21 NABZ 0000 0000 1370 1000 1944"],
        [Country.BA, "BA39 1290 0794 0102 8494"],
        [Country.BE, "BE43 0689 9999 9501"],
        [Country.BF, "BF42 BF08 4010 1300 4635 7400 0390"],
        [Country.BG, "BG80 BNBG 9661 1020 3456 78"],
        [Country.BH, "BH29 BMAG 1299 1234 56BH 00"],
        [Country.BI, "BI43 2010 1106 7444"],
        [Country.BJ, "BJ66 BJ06 1010 0100 1443 9000 0769"],
        [Country.BR, "BR97 0036 0305 0000 1000 9795 493P 1"],
        [Country.BY, "BY13 NBRB 3600 9000 0000 2Z00 AB00"],
        [Country.CG, "CG39 3001 1000 1010 1345 1300 019"],
        [Country.CH, "CH36 0838 7000 0010 8017 3"],
        [Country.CI, "CI02 N259 9162 9182 8879 7488 1965"],
        [Country.CM, "CM21 1000 2000 3002 7797 6315 008"],
        [Country.CR, "CR05 0152 0200 1026 2840 66"],
        [Country.CV, "CV64 0005 0000 0020 1082 1514 4"],
        [Country.CY, "CY17 0020 0128 0000 0012 0052 7600"],
        [Country.CZ, "CZ65 0800 0000 1920 0014 5399"],
        [Country.DE, "DE68 2105 0170 0012 3456 78"],
        [Country.DJ, "DJ21 1000 2010 0104 0994 3020 008"],
        [Country.DK, "DK50 0040 0440 1162 43"],
        [Country.DO, "DO22 ACAU 0000 0000 0001 2345 6789"],
        [Country.DZ, "DZ58 0002 1000 0111 3000 0005 70"],
        [Country.EE, "EE38 2200 2210 2014 5685"],
        [Country.EG, "EG38 0019 0005 0000 0000 2631 8000 2"],
        [Country.ES, "ES91 2100 0418 4502 0005 1332"],
        [Country.FI, "FI21 1234 5600 0007 85"],
        [Country.FO, "FO20 0040 0440 1162 43"],
        [Country.FR, "FR14 2004 1010 0505 0001 3M02 606"],
        [Country.GA, "GA21 4002 1010 0320 0189 0020 126"],
        [Country.GB, "GB46 BARC 2078 9863 2748 45"],
        [Country.GE, "GE29 NB00 0000 0101 9049 17"],
        [Country.GI, "GI75 NWBK 0000 0000 7099 453"],
        [Country.GL, "GL20 0040 0440 1162 43"],
        [Country.GQ, "GQ70 5000 2001 0037 1522 8190 196"],
        [Country.GR, "GR16 0110 1250 0000 0001 2300 695"],
        [Country.GT, "GT82 TRAJ 0102 0000 0012 1002 9690"],
        [Country.GW, "GW04 GW14 3001 0181 8006 3760 1"],
        [Country.HN, "HN54 PISA 0000 0000 0000 0012 3124"],
        [Country.HR, "HR12 1001 0051 8630 0016 0"],
        [Country.HU, "HU42 1177 3016 1111 1018 0000 0000"],
        [Country.IE, "IE29 AIBK 9311 5212 3456 78"],
        [Country.IL, "IL62 0108 0000 0009 9999 999"],
        [Country.IQ, "IQ98 NBIQ 8501 2345 6789 012"],
        [Country.IR, "IR58 0540 1051 8002 1273 1130 07"],
        [Country.IS, "IS14 0159 2600 7654 5510 7303 39"],
        [Country.IT, "IT60 X054 2811 1010 0000 0123 456"],
        [Country.JO, "JO94 CBJO 0010 0000 0000 0131 0003 02"],
        [Country.KM, "KM46 0000 5000 0100 1090 4400 137"],
        [Country.KW, "KW81 CBKU 0000 0000 0000 1234 5601 01"],
        [Country.KZ, "KZ75 125K ZT20 6910 0100"],
        [Country.LB, "LB30 0999 0000 0001 0019 2557 9115"],
        [Country.LC, "LC55 HEMM 0001 0001 0012 0012 0002 3015"],
        [Country.LI, "LI21 0881 0000 2324 013A A"],
        [Country.LT, "LT12 1000 0111 0100 1000"],
        [Country.LU, "LU28 0019 4006 4475 0000"],
        [Country.LV, "LV80 BANK 0000 4351 9500 1"],
        [Country.LY, "LY83 0020 4800 0020 1001 2036 1"],
        [Country.MA, "MA64 0115 1900 0001 2050 0053 4921"],
        [Country.MC, "MC11 1273 9000 7000 1111 1000 H79"],
        [Country.MD, "MD24 AG00 0225 1000 1310 4168"],
        [Country.ME, "ME25 5050 0001 2345 6789 51"],
        [Country.MG, "MG46 0000 5030 0712 8942 1016 045"],
        [Country.MK, "MK07 2501 2000 0058 984"],
        [Country.ML, "ML13 ML01 6012 0102 6001 0066 8497"],
        [Country.MR, "MR13 0002 0001 0100 0012 3456 753"],
        [Country.MT, "MT84 MALT 0110 0001 2345 MTLC AST0 01S"],
        [Country.MU, "MU17 BOMM 0101 1010 3030 0200 000M UR"],
        [Country.MZ, "MZ97 1234 1234 1234 1234 1234 1"],
        [Country.NE, "NE58 NE03 8010 0100 1303 0500 0268"],
        [Country.NI, "NI92 BAMC 0000 0000 0000 0000 03123 123"],
        [Country.NL, "NL20 INGB 0001 2345 67"],
        [Country.NO, "NO93 8601 1117 947"],
        [Country.PK, "PK36 SCBL 0000 0011 2345 6702"],
        [Country.PL, "PL61 1090 1014 0000 0712 1981 2874"],
        [Country.PS, "PS92 PALS 0000 0000 0400 1234 5670 2"],
        [Country.PT, "PT50 0002 0123 1234 5678 9015 4"],
        [Country.QA, "QA58 DOHB 0000 1234 5678 90AB CDEF G"],
        [Country.RO, "RO49 AAAA 1B31 0075 9384 0000"],
        [Country.RS, "RS35 2600 0560 1001 6113 79"],
        [Country.SA, "SA84 4000 0108 0540 1173 0013"],
        [Country.SC, "SC18 SSCB 1101 0000 0000 0000 1497 USD"],
        [Country.SE, "SE35 5000 0000 0549 1000 0003"],
        [Country.SI, "SI56 1910 0000 0123 438"],
        [Country.SK, "SK31 1200 0000 1987 4263 7541"],
        [Country.SM, "SM86 U032 2509 8000 0000 0270 100"],
        [Country.SN, "SN15 A123 1234 1234 1234 1234 1234"],
        [Country.ST, "ST23 0001 0001 0051 8453 1014 6"],
        [Country.SV, "SV62 CENR 0000 0000 0000 0070 0025"],
        [Country.TD, "TD89 6000 2000 0102 7109 1600 153"],
        [Country.TG, "TG53 TG00 9060 4310 3465 0040 0070"],
        [Country.TL, "TL38 0010 0123 4567 8910 106"],
        [Country.TN, "TN59 1000 6035 1835 9847 8831"],
        [Country.TR, "TR33 0006 1005 1978 6457 8413 26"],
        [Country.UA, "UA21 3996 2200 0002 6007 2335 6600 1"],
        [Country.VA, "VA59 0011 2300 0012 3456 78"],
        [Country.VG, "VG96 VPVG 0000 0123 4567 8901"],
        [Country.XK, "XK05 1212 0123 4567 8906"],
    ];
}
