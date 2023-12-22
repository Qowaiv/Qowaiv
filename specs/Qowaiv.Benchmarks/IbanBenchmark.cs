using Iban = Qowaiv.Financial.InternationalBankAccountNumber;

namespace Benchmarks;

public partial class IbanBenchmark
{
    public const int Iterations = 100;
    public string[] Inputs { get; set; } = [];
    public readonly string?[] Outputs = new string?[Iterations];
    public readonly Iban?[] Ibans = new Iban?[Iterations];

    public class ParseUnformatted : IbanBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            Inputs = Unformatted.OrderBy(_ => Rnd.Next()).Take(Iterations).ToArray();
        }
            
        [Benchmark(Baseline = true)]
        public Iban?[] BBAN()
        {
            for (var i = 0; i < Inputs.Length; i++)
            {
                Ibans[i] = Iban.TryParse(Inputs[i]);
            }
            return Ibans;
        }

        [Benchmark]
        public string?[] Regex()
        {
            for (var i = 0; i < Inputs.Length; i++)
            {
                Outputs[i] = Reference.RegexBased.Parse(Inputs[i]);
            }
            return Outputs;
        }

        [Benchmark]
        public string?[] Improved()
        {
            for (var i = 0; i < Inputs.Length; i++)
            {
                Outputs[i] = Reference.RegexBased.Improved(Inputs[i]);
            }
            return Outputs;
        }
    }

    public class ParseFormatted : IbanBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            Inputs = Formatted.OrderBy(_ => Rnd.Next()).Take(Iterations).ToArray();
        }

        [Benchmark(Baseline = true)]
        public Iban?[] BBAN()
        {
            for (var i = 0; i < Inputs.Length; i++)
            {
                Ibans[i] = Iban.TryParse(Inputs[i]);
            }
            return Ibans;
        }

        [Benchmark]
        public string?[] Regex()
        {
            for (var i = 0; i < Inputs.Length; i++)
            {
                Outputs[i] = Reference.RegexBased.Parse(Inputs[i]);
            }
            return Outputs;
        }

        [Benchmark]
        public string?[] Improved()
        {
            for (var i = 0; i < Inputs.Length; i++)
            {
                Outputs[i] = Reference.RegexBased.Improved(Inputs[i]);
            }
            return Outputs;
        }
    }

    internal static readonly string[] Formatted =
    [
        "AD12 0001 2030 2003 5910 0100",
        "AE95 0210 0000 0069 3123 456",
        "AL47 2121 1009 0000 0002 3569 8741",
        "AT61 1904 3002 3457 3201",
        "AZ21 NABZ 0000 0000 1370 1000 1944",
        "BA39 1290 0794 0102 8494",
        "BE43 0689 9999 9501",
        "BG80 BNBG 9661 1020 3456 78",
        "BH29 BMAG 1299 1234 56BH 00",
        "BR97 0036 0305 0000 1000 9795 493P 1",
        "BY13 NBRB 3600 9000 0000 2Z00 AB00",
        "CH36 0838 7000 0010 8017 3",
        "CR05 0152 0200 1026 2840 66",
        "CY17 0020 0128 0000 0012 0052 7600",
        "CZ65 0800 0000 1920 0014 5399",
        "DE68 2105 0170 0012 3456 78",
        "DK50 0040 0440 1162 43",
        "DO22 ACAU 0000 0000 0001 2345 6789",
        "EE38 2200 2210 2014 5685",
        "EG38 0019 0005 0000 0000 2631 8000 2",
        "ES91 2100 0418 4502 0005 1332",
        "FI21 1234 5600 0007 85",
        "FO20 0040 0440 1162 43",
        "FR14 2004 1010 0505 0001 3M02 606",
        "GB46 BARC 2078 9863 2748 45",
        "GE29 NB00 0000 0101 9049 17",
        "GI75 NWBK 0000 0000 7099 453",
        "GL20 0040 0440 1162 43",
        "GR16 0110 1250 0000 0001 2300 695",
        "GT82 TRAJ 0102 0000 0012 1002 9690",
        "HR12 1001 0051 8630 0016 0",
        "HU42 1177 3016 1111 1018 0000 0000",
        "IE29 AIBK 9311 5212 3456 78",
        "IL62 0108 0000 0009 9999 999",
        "IQ98 NBIQ 8501 2345 6789 012",
        "IS14 0159 2600 7654 5510 7303 39",
        "IT60 X054 2811 1010 0000 0123 456",
        "JO94 CBJO 0010 0000 0000 0131 0003 02",
        "KW81 CBKU 0000 0000 0000 1234 5601 01",
        "KZ75 125K ZT20 6910 0100",
        "LB30 0999 0000 0001 0019 2557 9115",
        "LC55 HEMM 0001 0001 0012 0012 0002 3015 ",
        "LI21 0881 0000 2324 013A A",
        "LT12 1000 0111 0100 1000",
        "LU28 0019 4006 4475 0000",
        "LV80 BANK 0000 4351 9500 1",
        "LY83 0020 4800 0020 1001 2036 1",
        "MC11 1273 9000 7000 1111 1000 H79",
        "MD24 AG00 0225 1000 1310 4168",
        "ME25 5050 0001 2345 6789 51",
        "MK07 2501 2000 0058 984",
        "MR13 0002 0001 0100 0012 3456 753",
        "MT84 MALT 0110 0001 2345 MTLC AST0 01S",
        "MU17 BOMM 0101 1010 3030 0200 000M UR",
        "NL20 INGB 0001 2345 67",
        "NO93 8601 1117 947",
        "PK36 SCBL 0000 0011 2345 6702",
        "PL61 1090 1014 0000 0712 1981 2874",
        "PS92 PALS 0000 0000 0400 1234 5670 2",
        "PT50 0002 0123 1234 5678 9015 4",
        "QA58 DOHB 0000 1234 5678 90AB CDEF G",
        "RO49 AAAA 1B31 0075 9384 0000",
        "RS35 2600 0560 1001 6113 79",
        "RU02 0445 2560 0407 0281 0412 3456 7890 ",
        "SA84 4000 0108 0540 1173 0013",
        "SC18 SSCB 1101 0000 0000 0000 1497 USD",
        "SD21 2901 0501 2340 01",
        "SE35 5000 0000 0549 1000 0003",
        "SI56 1910 0000 0123 438",
        "SK31 1200 0000 1987 4263 7541",
        "SM86 U032 2509 8000 0000 0270 100",
        "ST23 0001 0001 0051 8453 1014 6",
        "SV62 CENR 0000 0000 0000 0070 0025",
        "TL38 0010 0123 4567 8910 106",
        "TN59 1000 6035 1835 9847 8831",
        "TR33 0006 1005 1978 6457 8413 26",
        "UA21 3996 2200 0002 6007 2335 6600 1",
        "VA59 0011 2300 0012 3456 78",
        "VG96 VPVG 0000 0123 4567 8901",
        "XK05 1212 0123 4567 8906",
        "AO06 0044 0000 6729 5030 1010 2",
        "BF42 BF08 4010 1300 4635 7400 0390",
        "BI13 2000 1100 0100 0012 3456 789",
        "BJ66 BJ06 1010 0100 1443 9000 0769",
        "CF42 2000 1000 0101 2006 9700 160",
        "CG39 3001 1000 1010 1345 1300 019",
        "CI02 N259 9162 9182 8879 7488 1965",
        "CM21 1000 2000 3002 7797 6315 008",
        "CV64 0005 0000 0020 1082 1514 4",
        "DJ21 1000 2010 0104 0994 3020 008",
        "DZ58 0002 1000 0111 3000 0005 70",
        "GA21 4002 1010 0320 0189 0020 126",
        "GQ70 5000 2001 0037 1522 8190 196",
        "GW04 GW14 3001 0181 8006 3760 1",
        "HN54 PISA 0000 0000 0000 0012 3124",
        "IR58 0540 1051 8002 1273 1130 07",
        "KM46 0000 5000 0100 1090 4400 137",
        "MA64 0115 1900 0001 2050 0053 4921",
        "MG46 0000 5030 0712 8942 1016 045",
        "ML13 ML01 6012 0102 6001 0066 8497",
        "MZ97 1234 1234 1234 1234 1234 1",
        "NE58 NE03 8010 0100 1303 0500 0268",
        "NI92 BAMC 0000 0000 0000 0000 03123 123",
        "SN05 TI80 0835 4151 5881 3959 8706",
        "TD89 6000 2000 0102 7109 1600 153",
        "TG53 TG00 9060 4310 3465 0040 0070"
    ];

    private static string[] Unformatted { get; } = Formatted.Select(f => f.Replace(" ", string.Empty)).ToArray();

    private static readonly RandomSource Rnd = new MersenneTwister();
}
