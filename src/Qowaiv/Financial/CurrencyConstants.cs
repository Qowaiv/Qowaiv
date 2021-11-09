#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

namespace Qowaiv.Financial;

public partial struct Currency
{
    /// <summary>Describes the currency United Arab Emirates dirham (AED).</summary>
    public static readonly Currency AED = new("AED");

    /// <summary>Describes the currency Afghan afghani (AFN).</summary>
    public static readonly Currency AFN = new("AFN");

    /// <summary>Describes the currency Albanian lek (ALL).</summary>
    public static readonly Currency ALL = new("ALL");

    /// <summary>Describes the currency Armenian dram (AMD).</summary>
    public static readonly Currency AMD = new("AMD");

    /// <summary>Describes the currency Netherlands Antillean guilder (ANG).</summary>
    public static readonly Currency ANG = new("ANG");

    /// <summary>Describes the currency Angolan kwanza (AOA).</summary>
    public static readonly Currency AOA = new("AOA");

    /// <summary>Describes the currency Argentine peso (ARS).</summary>
    public static readonly Currency ARS = new("ARS");

    /// <summary>Describes the currency Australian dollar (AUD).</summary>
    public static readonly Currency AUD = new("AUD");

    /// <summary>Describes the currency Aruban florin (AWG).</summary>
    public static readonly Currency AWG = new("AWG");

    /// <summary>Describes the currency Azerbaijani manat (AZN).</summary>
    public static readonly Currency AZN = new("AZN");

    /// <summary>Describes the currency Bosnia and Herzegovina convertible mark (BAM).</summary>
    public static readonly Currency BAM = new("BAM");

    /// <summary>Describes the currency Barbados dollar (BBD).</summary>
    public static readonly Currency BBD = new("BBD");

    /// <summary>Describes the currency Bangladeshi taka (BDT).</summary>
    public static readonly Currency BDT = new("BDT");

    /// <summary>Describes the currency Bulgarian lev (BGN).</summary>
    public static readonly Currency BGN = new("BGN");

    /// <summary>Describes the currency Bahraini dinar (BHD).</summary>
    public static readonly Currency BHD = new("BHD");

    /// <summary>Describes the currency Burundian franc (BIF).</summary>
    public static readonly Currency BIF = new("BIF");

    /// <summary>Describes the currency Bermudian dollar (BMD).</summary>
    public static readonly Currency BMD = new("BMD");

    /// <summary>Describes the currency Brunei dollar (BND).</summary>
    public static readonly Currency BND = new("BND");

    /// <summary>Describes the currency Boliviano (BOB).</summary>
    public static readonly Currency BOB = new("BOB");

    /// <summary>Describes the currency Bolivian Mvdol (BOV).</summary>
    public static readonly Currency BOV = new("BOV");

    /// <summary>Describes the currency Brazilian real (BRL).</summary>
    public static readonly Currency BRL = new("BRL");

    /// <summary>Describes the currency Bahamian dollar (BSD).</summary>
    public static readonly Currency BSD = new("BSD");

    /// <summary>Describes the currency Bhutanese ngultrum (BTN).</summary>
    public static readonly Currency BTN = new("BTN");

    /// <summary>Describes the currency Botswana pula (BWP).</summary>
    public static readonly Currency BWP = new("BWP");

    /// <summary>Describes the currency Belarusian ruble (BYR).</summary>
    public static readonly Currency BYR = new("BYR");

    /// <summary>Describes the currency Belize dollar (BZD).</summary>
    public static readonly Currency BZD = new("BZD");

    /// <summary>Describes the currency Canadian dollar (CAD).</summary>
    public static readonly Currency CAD = new("CAD");

    /// <summary>Describes the currency Congolese franc (CDF).</summary>
    public static readonly Currency CDF = new("CDF");

    /// <summary>Describes the currency WIR Euro (CHE).</summary>
    public static readonly Currency CHE = new("CHE");

    /// <summary>Describes the currency Swiss franc (CHF).</summary>
    public static readonly Currency CHF = new("CHF");

    /// <summary>Describes the currency WIR Franc (CHW).</summary>
    public static readonly Currency CHW = new("CHW");

    /// <summary>Describes the currency Unidad de Fomento (CLF).</summary>
    public static readonly Currency CLF = new("CLF");

    /// <summary>Describes the currency Chilean peso (CLP).</summary>
    public static readonly Currency CLP = new("CLP");

    /// <summary>Describes the currency Chinese yuan when traded in Hong Kong (CNH).</summary>
    public static readonly Currency CNH = new("CNH");

    /// <summary>Describes the currency Chinese yuan (CNY).</summary>
    public static readonly Currency CNY = new("CNY");

    /// <summary>Describes the currency Colombian peso (COP).</summary>
    public static readonly Currency COP = new("COP");

    /// <summary>Describes the currency Unidad de Valor Real (COU).</summary>
    public static readonly Currency COU = new("COU");

    /// <summary>Describes the currency Costa Rican colon (CRC).</summary>
    public static readonly Currency CRC = new("CRC");

    /// <summary>Describes the currency Cuban convertible peso (CUC).</summary>
    public static readonly Currency CUC = new("CUC");

    /// <summary>Describes the currency Cuban peso (CUP).</summary>
    public static readonly Currency CUP = new("CUP");

    /// <summary>Describes the currency Cape Verde escudo (CVE).</summary>
    public static readonly Currency CVE = new("CVE");

    /// <summary>Describes the currency Czech koruna (CZK).</summary>
    public static readonly Currency CZK = new("CZK");

    /// <summary>Describes the currency Djiboutian franc (DJF).</summary>
    public static readonly Currency DJF = new("DJF");

    /// <summary>Describes the currency Danish krone (DKK).</summary>
    public static readonly Currency DKK = new("DKK");

    /// <summary>Describes the currency Dominican peso (DOP).</summary>
    public static readonly Currency DOP = new("DOP");

    /// <summary>Describes the currency Algerian dinar (DZD).</summary>
    public static readonly Currency DZD = new("DZD");

    /// <summary>Describes the currency Egyptian pound (EGP).</summary>
    public static readonly Currency EGP = new("EGP");

    /// <summary>Describes the currency Eritrean nakfa (ERN).</summary>
    public static readonly Currency ERN = new("ERN");

    /// <summary>Describes the currency Ethiopian birr (ETB).</summary>
    public static readonly Currency ETB = new("ETB");

    /// <summary>Describes the currency Euro (EUR).</summary>
    public static readonly Currency EUR = new("EUR");

    /// <summary>Describes the currency Fiji dollar (FJD).</summary>
    public static readonly Currency FJD = new("FJD");

    /// <summary>Describes the currency Falkland Islands pound (FKP).</summary>
    public static readonly Currency FKP = new("FKP");

    /// <summary>Describes the currency Pound sterling (GBP).</summary>
    public static readonly Currency GBP = new("GBP");

    /// <summary>Describes the currency Georgian lari (GEL).</summary>
    public static readonly Currency GEL = new("GEL");

    /// <summary>Describes the currency Ghanaian cedi (GHS).</summary>
    public static readonly Currency GHS = new("GHS");

    /// <summary>Describes the currency Gibraltar pound (GIP).</summary>
    public static readonly Currency GIP = new("GIP");

    /// <summary>Describes the currency Gambian dalasi (GMD).</summary>
    public static readonly Currency GMD = new("GMD");

    /// <summary>Describes the currency Guinean franc (GNF).</summary>
    public static readonly Currency GNF = new("GNF");

    /// <summary>Describes the currency Guatemalan quetzal (GTQ).</summary>
    public static readonly Currency GTQ = new("GTQ");

    /// <summary>Describes the currency Guyanese dollar (GYD).</summary>
    public static readonly Currency GYD = new("GYD");

    /// <summary>Describes the currency Hong Kong dollar (HKD).</summary>
    public static readonly Currency HKD = new("HKD");

    /// <summary>Describes the currency Honduran lempira (HNL).</summary>
    public static readonly Currency HNL = new("HNL");

    /// <summary>Describes the currency Croatian kuna (HRK).</summary>
    public static readonly Currency HRK = new("HRK");

    /// <summary>Describes the currency Haitian gourde (HTG).</summary>
    public static readonly Currency HTG = new("HTG");

    /// <summary>Describes the currency Hungarian forint (HUF).</summary>
    public static readonly Currency HUF = new("HUF");

    /// <summary>Describes the currency Indonesian rupiah (IDR).</summary>
    public static readonly Currency IDR = new("IDR");

    /// <summary>Describes the currency Israeli new shekel (ILS).</summary>
    public static readonly Currency ILS = new("ILS");

    /// <summary>Describes the currency Indian rupee (INR).</summary>
    public static readonly Currency INR = new("INR");

    /// <summary>Describes the currency Iraqi dinar (IQD).</summary>
    public static readonly Currency IQD = new("IQD");

    /// <summary>Describes the currency Iranian rial (IRR).</summary>
    public static readonly Currency IRR = new("IRR");

    /// <summary>Describes the currency Icelandic króna (ISK).</summary>
    public static readonly Currency ISK = new("ISK");

    /// <summary>Describes the currency Jamaican dollar (JMD).</summary>
    public static readonly Currency JMD = new("JMD");

    /// <summary>Describes the currency Jordanian dinar (JOD).</summary>
    public static readonly Currency JOD = new("JOD");

    /// <summary>Describes the currency Japanese yen (JPY).</summary>
    public static readonly Currency JPY = new("JPY");

    /// <summary>Describes the currency Kenyan shilling (KES).</summary>
    public static readonly Currency KES = new("KES");

    /// <summary>Describes the currency Kyrgyzstani som (KGS).</summary>
    public static readonly Currency KGS = new("KGS");

    /// <summary>Describes the currency Cambodian riel (KHR).</summary>
    public static readonly Currency KHR = new("KHR");

    /// <summary>Describes the currency Comoro franc (KMF).</summary>
    public static readonly Currency KMF = new("KMF");

    /// <summary>Describes the currency North Korean won (KPW).</summary>
    public static readonly Currency KPW = new("KPW");

    /// <summary>Describes the currency South Korean won (KRW).</summary>
    public static readonly Currency KRW = new("KRW");

    /// <summary>Describes the currency Kuwaiti dinar (KWD).</summary>
    public static readonly Currency KWD = new("KWD");

    /// <summary>Describes the currency Cayman Islands dollar (KYD).</summary>
    public static readonly Currency KYD = new("KYD");

    /// <summary>Describes the currency Kazakhstani tenge (KZT).</summary>
    public static readonly Currency KZT = new("KZT");

    /// <summary>Describes the currency Lao kip (LAK).</summary>
    public static readonly Currency LAK = new("LAK");

    /// <summary>Describes the currency Lebanese pound (LBP).</summary>
    public static readonly Currency LBP = new("LBP");

    /// <summary>Describes the currency Sri Lankan rupee (LKR).</summary>
    public static readonly Currency LKR = new("LKR");

    /// <summary>Describes the currency Liberian dollar (LRD).</summary>
    public static readonly Currency LRD = new("LRD");

    /// <summary>Describes the currency Lesotho loti (LSL).</summary>
    public static readonly Currency LSL = new("LSL");

    /// <summary>Describes the currency Libyan dinar (LYD).</summary>
    public static readonly Currency LYD = new("LYD");

    /// <summary>Describes the currency Moroccan dirham (MAD).</summary>
    public static readonly Currency MAD = new("MAD");

    /// <summary>Describes the currency Moldovan leu (MDL).</summary>
    public static readonly Currency MDL = new("MDL");

    /// <summary>Describes the currency Malagasy ariary (MGA).</summary>
    public static readonly Currency MGA = new("MGA");

    /// <summary>Describes the currency Macedonian denar (MKD).</summary>
    public static readonly Currency MKD = new("MKD");

    /// <summary>Describes the currency Myanmar kyat (MMK).</summary>
    public static readonly Currency MMK = new("MMK");

    /// <summary>Describes the currency Mongolian tugrik (MNT).</summary>
    public static readonly Currency MNT = new("MNT");

    /// <summary>Describes the currency Macanese pataca (MOP).</summary>
    public static readonly Currency MOP = new("MOP");

    /// <summary>Describes the currency Mauritanian ouguiya (MRO).</summary>
    public static readonly Currency MRO = new("MRO");

    /// <summary>Describes the currency Mauritian rupee (MUR).</summary>
    public static readonly Currency MUR = new("MUR");

    /// <summary>Describes the currency Maldivian rufiyaa (MVR).</summary>
    public static readonly Currency MVR = new("MVR");

    /// <summary>Describes the currency Malawian kwacha (MWK).</summary>
    public static readonly Currency MWK = new("MWK");

    /// <summary>Describes the currency Mexican peso (MXN).</summary>
    public static readonly Currency MXN = new("MXN");

    /// <summary>Describes the currency Mexican Unidad de Inversion (UDI) (MXV).</summary>
    public static readonly Currency MXV = new("MXV");

    /// <summary>Describes the currency Malaysian ringgit (MYR).</summary>
    public static readonly Currency MYR = new("MYR");

    /// <summary>Describes the currency Mozambican metical (MZN).</summary>
    public static readonly Currency MZN = new("MZN");

    /// <summary>Describes the currency Namibian dollar (NAD).</summary>
    public static readonly Currency NAD = new("NAD");

    /// <summary>Describes the currency Nigerian naira (NGN).</summary>
    public static readonly Currency NGN = new("NGN");

    /// <summary>Describes the currency Nicaraguan córdoba (NIO).</summary>
    public static readonly Currency NIO = new("NIO");

    /// <summary>Describes the currency Norwegian krone (NOK).</summary>
    public static readonly Currency NOK = new("NOK");

    /// <summary>Describes the currency Nepalese rupee (NPR).</summary>
    public static readonly Currency NPR = new("NPR");

    /// <summary>Describes the currency New Zealand dollar (NZD).</summary>
    public static readonly Currency NZD = new("NZD");

    /// <summary>Describes the currency Omani rial (OMR).</summary>
    public static readonly Currency OMR = new("OMR");

    /// <summary>Describes the currency Panamanian balboa (PAB).</summary>
    public static readonly Currency PAB = new("PAB");

    /// <summary>Describes the currency Peruvian nuevo sol (PEN).</summary>
    public static readonly Currency PEN = new("PEN");

    /// <summary>Describes the currency Papua New Guinean kina (PGK).</summary>
    public static readonly Currency PGK = new("PGK");

    /// <summary>Describes the currency Philippine peso (PHP).</summary>
    public static readonly Currency PHP = new("PHP");

    /// <summary>Describes the currency Pakistani rupee (PKR).</summary>
    public static readonly Currency PKR = new("PKR");

    /// <summary>Describes the currency Polish złoty (PLN).</summary>
    public static readonly Currency PLN = new("PLN");

    /// <summary>Describes the currency Paraguayan guaraní (PYG).</summary>
    public static readonly Currency PYG = new("PYG");

    /// <summary>Describes the currency Qatari riyal (QAR).</summary>
    public static readonly Currency QAR = new("QAR");

    /// <summary>Describes the currency Romanian new leu (RON).</summary>
    public static readonly Currency RON = new("RON");

    /// <summary>Describes the currency Serbian dinar (RSD).</summary>
    public static readonly Currency RSD = new("RSD");

    /// <summary>Describes the currency Russian ruble (RUB).</summary>
    public static readonly Currency RUB = new("RUB");

    /// <summary>Describes the currency Rwandan franc (RWF).</summary>
    public static readonly Currency RWF = new("RWF");

    /// <summary>Describes the currency Saudi riyal (SAR).</summary>
    public static readonly Currency SAR = new("SAR");

    /// <summary>Describes the currency Solomon Islands dollar (SBD).</summary>
    public static readonly Currency SBD = new("SBD");

    /// <summary>Describes the currency Seychelles rupee (SCR).</summary>
    public static readonly Currency SCR = new("SCR");

    /// <summary>Describes the currency Sudanese pound (SDG).</summary>
    public static readonly Currency SDG = new("SDG");

    /// <summary>Describes the currency Swedish krona/kronor (SEK).</summary>
    public static readonly Currency SEK = new("SEK");

    /// <summary>Describes the currency Singapore dollar (SGD).</summary>
    public static readonly Currency SGD = new("SGD");

    /// <summary>Describes the currency Saint Helena pound (SHP).</summary>
    public static readonly Currency SHP = new("SHP");

    /// <summary>Describes the currency Sierra Leonean leone (SLL).</summary>
    public static readonly Currency SLL = new("SLL");

    /// <summary>Describes the currency Somali shilling (SOS).</summary>
    public static readonly Currency SOS = new("SOS");

    /// <summary>Describes the currency Surinamese dollar (SRD).</summary>
    public static readonly Currency SRD = new("SRD");

    /// <summary>Describes the currency South Sudanese pound (SSP).</summary>
    public static readonly Currency SSP = new("SSP");

    /// <summary>Describes the currency São Tomé and Príncipe dobra (STD).</summary>
    public static readonly Currency STD = new("STD");

    /// <summary>Describes the currency Syrian pound (SYP).</summary>
    public static readonly Currency SYP = new("SYP");

    /// <summary>Describes the currency Swazi lilangeni (SZL).</summary>
    public static readonly Currency SZL = new("SZL");

    /// <summary>Describes the currency Thai baht (THB).</summary>
    public static readonly Currency THB = new("THB");

    /// <summary>Describes the currency Tajikistani somoni (TJS).</summary>
    public static readonly Currency TJS = new("TJS");

    /// <summary>Describes the currency Turkmenistani manat (TMT).</summary>
    public static readonly Currency TMT = new("TMT");

    /// <summary>Describes the currency Tunisian dinar (TND).</summary>
    public static readonly Currency TND = new("TND");

    /// <summary>Describes the currency Tongan paʻanga (TOP).</summary>
    public static readonly Currency TOP = new("TOP");

    /// <summary>Describes the currency Turkish lira (TRY).</summary>
    public static readonly Currency TRY = new("TRY");

    /// <summary>Describes the currency Trinidad and Tobago dollar (TTD).</summary>
    public static readonly Currency TTD = new("TTD");

    /// <summary>Describes the currency New Taiwan dollar (TWD).</summary>
    public static readonly Currency TWD = new("TWD");

    /// <summary>Describes the currency Tanzanian shilling (TZS).</summary>
    public static readonly Currency TZS = new("TZS");

    /// <summary>Describes the currency Ukrainian hryvnia (UAH).</summary>
    public static readonly Currency UAH = new("UAH");

    /// <summary>Describes the currency Ugandan shilling (UGX).</summary>
    public static readonly Currency UGX = new("UGX");

    /// <summary>Describes the currency United States dollar (USD).</summary>
    public static readonly Currency USD = new("USD");

    /// <summary>Describes the currency United States dollar (next day) (USN).</summary>
    public static readonly Currency USN = new("USN");

    /// <summary>Describes the currency United States dollar (same day) (USS).</summary>
    public static readonly Currency USS = new("USS");

    /// <summary>Describes the currency Uruguay Peso en Unidades Indexadas (URUIURUI) (UYI).</summary>
    public static readonly Currency UYI = new("UYI");

    /// <summary>Describes the currency Uruguayan peso (UYU).</summary>
    public static readonly Currency UYU = new("UYU");

    /// <summary>Describes the currency Uzbekistan som (UZS).</summary>
    public static readonly Currency UZS = new("UZS");

    /// <summary>Describes the currency Venezuelan bolívar (VEF).</summary>
    public static readonly Currency VEF = new("VEF");

    /// <summary>Describes the currency Vietnamese dong (VND).</summary>
    public static readonly Currency VND = new("VND");

    /// <summary>Describes the currency Vanuatu vatu (VUV).</summary>
    public static readonly Currency VUV = new("VUV");

    /// <summary>Describes the currency Samoan tala (WST).</summary>
    public static readonly Currency WST = new("WST");

    /// <summary>Describes the currency CFA franc BEAC (XAF).</summary>
    public static readonly Currency XAF = new("XAF");

    /// <summary>Describes the currency Silver (one troy ounce) (XAG).</summary>
    public static readonly Currency XAG = new("XAG");

    /// <summary>Describes the currency Gold (one troy ounce) (XAU).</summary>
    public static readonly Currency XAU = new("XAU");

    /// <summary>Describes the currency European Composite Unit (EURCO) (bond market unit) (XBA).</summary>
    public static readonly Currency XBA = new("XBA");

    /// <summary>Describes the currency European Monetary Unit (E.M.U.-6) (bond market unit) (XBB).</summary>
    public static readonly Currency XBB = new("XBB");

    /// <summary>Describes the currency European Unit of Account 9 (E.U.A.-9) (bond market unit) (XBC).</summary>
    public static readonly Currency XBC = new("XBC");

    /// <summary>Describes the currency European Unit of Account 17(E.U.A.-17) (bond market unit) (XBD).</summary>
    public static readonly Currency XBD = new("XBD");

    /// <summary>Describes the currency East Caribbean dollar (XCD).</summary>
    public static readonly Currency XCD = new("XCD");

    /// <summary>Describes the currency Special drawing rights (XDR).</summary>
    public static readonly Currency XDR = new("XDR");

    /// <summary>Describes the currency UIC franc (special settlement currency) (XFU).</summary>
    public static readonly Currency XFU = new("XFU");

    /// <summary>Describes the currency CFA franc BCEAO (XOF).</summary>
    public static readonly Currency XOF = new("XOF");

    /// <summary>Describes the currency Palladium (one troy ounce) (XPD).</summary>
    public static readonly Currency XPD = new("XPD");

    /// <summary>Describes the currency CFP franc (franc Pacifique) (XPF).</summary>
    public static readonly Currency XPF = new("XPF");

    /// <summary>Describes the currency Platinum (one troy ounce) (XPT).</summary>
    public static readonly Currency XPT = new("XPT");

    /// <summary>Describes the currency SUCRE (XSU).</summary>
    public static readonly Currency XSU = new("XSU");

    /// <summary>Describes the currency Code reserved for testing purposes (XTS).</summary>
    public static readonly Currency XTS = new("XTS");

    /// <summary>Describes the currency ADB Unit of Account (XUA).</summary>
    public static readonly Currency XUA = new("XUA");

    /// <summary>Describes the currency No currency (XXX).</summary>
    public static readonly Currency XXX = new("XXX");

    /// <summary>Describes the currency Yemeni rial (YER).</summary>
    public static readonly Currency YER = new("YER");

    /// <summary>Describes the currency South African rand (ZAR).</summary>
    public static readonly Currency ZAR = new("ZAR");

    /// <summary>Describes the currency Zambian kwacha (ZMW).</summary>
    public static readonly Currency ZMW = new("ZMW");

    /// <summary>Describes the currency Andorran peseta (ADP).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency ADP = new("ADP");

    /// <summary>Describes the currency Austrian schilling (ATS).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency ATS = new("ATS");

    /// <summary>Describes the currency Belgian franc (BEF).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency BEF = new("BEF");

    /// <summary>Describes the currency Serbian dinar (CSD).</summary>
    /// <remarks>End date is 2005-12-31.</remarks>
    public static readonly Currency CSD = new("CSD");

    /// <summary>Describes the currency Czechoslovak koruna (CSK).</summary>
    /// <remarks>End date is 1993-02-08.</remarks>
    public static readonly Currency CSK = new("CSK");

    /// <summary>Describes the currency Cypriot pound (CYP).</summary>
    /// <remarks>End date is 2007-12-31.</remarks>
    public static readonly Currency CYP = new("CYP");

    /// <summary>Describes the currency East German Mark (DDM).</summary>
    /// <remarks>End date is 1990-06-30.</remarks>
    public static readonly Currency DDM = new("DDM");

    /// <summary>Describes the currency German mark (DEM).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency DEM = new("DEM");

    /// <summary>Describes the currency Ecuadorian sucre (ECS).</summary>
    /// <remarks>End date is 2000-03-12.</remarks>
    public static readonly Currency ECS = new("ECS");

    /// <summary>Describes the currency Estonian kroon (EEK).</summary>
    /// <remarks>End date is 2010-12-13.</remarks>
    public static readonly Currency EEK = new("EEK");

    /// <summary>Describes the currency Spanish peseta (ESP).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency ESP = new("ESP");

    /// <summary>Describes the currency Finnish markka (FIM).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency FIM = new("FIM");

    /// <summary>Describes the currency French franc (FRF).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency FRF = new("FRF");

    /// <summary>Describes the currency Greek drachma (GRD).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency GRD = new("GRD");

    /// <summary>Describes the currency Irish pound (IEP).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency IEP = new("IEP");

    /// <summary>Describes the currency Italian lira (ITL).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency ITL = new("ITL");

    /// <summary>Describes the currency Lithuanian litas (LTL).</summary>
    /// <remarks>End date is 2014-12-31.</remarks>
    public static readonly Currency LTL = new("LTL");

    /// <summary>Describes the currency Luxembourgish franc (LUF).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency LUF = new("LUF");

    /// <summary>Describes the currency Latvian lats (LVL).</summary>
    /// <remarks>End date is 2014-01-01.</remarks>
    public static readonly Currency LVL = new("LVL");

    /// <summary>Describes the currency Monégasque franc (MCF).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency MCF = new("MCF");

    /// <summary>Describes the currency Maltese lira (MTL).</summary>
    /// <remarks>End date is 2007-12-31.</remarks>
    public static readonly Currency MTL = new("MTL");

    /// <summary>Describes the currency Dutch guilder (NLG).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency NLG = new("NLG");

    /// <summary>Describes the currency Portuguese escudo (PTE).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency PTE = new("PTE");

    /// <summary>Describes the currency Slovenian tolar (SIT).</summary>
    /// <remarks>End date is 2007-01-01.</remarks>
    public static readonly Currency SIT = new("SIT");

    /// <summary>Describes the currency Slovak koruna (SKK).</summary>
    /// <remarks>End date is 2009-01-01.</remarks>
    public static readonly Currency SKK = new("SKK");

    /// <summary>Describes the currency Sammarinese lira (SML).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency SML = new("SML");

    /// <summary>Describes the currency Soviet Union ruble (SUR).</summary>
    /// <remarks>End date is 1990-12-31.</remarks>
    public static readonly Currency SUR = new("SUR");

    /// <summary>Describes the currency Vatican lira (VAL).</summary>
    /// <remarks>End date is 2001-12-31.</remarks>
    public static readonly Currency VAL = new("VAL");

    /// <summary>Describes the currency Yugoslav dinar (YOU).</summary>
    /// <remarks>End date is 1993-12-31.</remarks>
    public static readonly Currency YOU = new("YOU");

    /// <summary>Describes the currency Yugoslav dinar (YUD).</summary>
    /// <remarks>End date is 1989-12-31.</remarks>
    public static readonly Currency YUD = new("YUD");

    /// <summary>Describes the currency Yugoslav dinar (YUG).</summary>
    /// <remarks>End date is 1994-01-23.</remarks>
    public static readonly Currency YUG = new("YUG");

    /// <summary>Describes the currency Yugoslav dinar (YUM).</summary>
    /// <remarks>End date is 2003-07-02.</remarks>
    public static readonly Currency YUM = new("YUM");

    /// <summary>Describes the currency Yugoslav dinar (YUN).</summary>
    /// <remarks>End date is 1992-06-30.</remarks>
    public static readonly Currency YUN = new("YUN");

    /// <summary>Describes the currency Yugoslav dinar (YUR).</summary>
    /// <remarks>End date is 1993-09-30.</remarks>
    public static readonly Currency YUR = new("YUR");

    /// <summary>Describes the currency Zaïrean new zaïre (ZRN).</summary>
    /// <remarks>End date is 1996-12-31.</remarks>
    public static readonly Currency ZRN = new("ZRN");

    /// <summary>Describes the currency Zaïrean zaïre (ZRZ).</summary>
    /// <remarks>End date is 1992-12-31.</remarks>
    public static readonly Currency ZRZ = new("ZRZ");

    /// <summary>Describes the currency Zimbabwe dollar (ZWC).</summary>
    /// <remarks>End date is 1980-04-17.</remarks>
    public static readonly Currency ZWC = new("ZWC");

    /// <summary>Describes the currency Zimbabwe dollar (ZWD).</summary>
    /// <remarks>End date is 2006-07-31.</remarks>
    public static readonly Currency ZWD = new("ZWD");

    /// <summary>Describes the currency Zimbabwe dollar (ZWL).</summary>
    /// <remarks>End date is 2009-04-12.</remarks>
    public static readonly Currency ZWL = new("ZWL");

    /// <summary>Describes the currency Zimbabwe dollar (ZWN).</summary>
    /// <remarks>End date is 2008-07-31.</remarks>
    public static readonly Currency ZWN = new("ZWN");

    /// <summary>Describes the currency Zimbabwe dollar (ZWR).</summary>
    /// <remarks>End date is 2009-02-02.</remarks>
    public static readonly Currency ZWR = new("ZWR");

}
