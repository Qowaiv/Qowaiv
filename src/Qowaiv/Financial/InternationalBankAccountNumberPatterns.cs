#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

using Qowaiv.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Qowaiv.Financial
{
    public partial struct InternationalBankAccountNumber
    {
        /// <summary>Gets the localized patterns.</summary>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/International_Bank_Account_Number.
        /// </remarks>
        private static readonly Dictionary<Country, Regex> LocalizedPatterns = new Dictionary<Country, Regex>()
        {
            // Albania, Length: 28, BBAN: 8n, 16c, Fields: ALkk bbbs sssx cccc cccc cccc cccc (b = National bank code, s = Branch code, x = National check digit, c = Account number)
            { Country.AL, new Regex(@"^AL[0-9]{10}[0-9A-Z]{16}$", RegexOptions.Compiled) },

            // Algeria, Length: 24, BBAN: 20n, Fields: DZkk nnnn nnnn nnnn nnnn nnnn (Nordea catalogued)
            { Country.DZ, new Regex(@"^DZ[0-9]{22}$", RegexOptions.Compiled) },

            // Andorra, Length: 24, BBAN: 8n,12c, Fields: ADkk bbbb ssss cccc cccc cccc (b = National bank code, s = Branch code, c = Account number)
            { Country.AD, new Regex(@"^AD[0-9]{10}[0-9A-Z]{12}$", RegexOptions.Compiled) },

            // Angola, Length: 25, BBAN: 21n, Fields: AOkk nnnn nnnn nnnn nnnn nnnn n (Nordea catalogued)
            { Country.AO, new Regex(@"^AO[0-9]{23}$", RegexOptions.Compiled) },

            // Austria, Length: 20, BBAN: 16n, Fields: ATkk bbbb bccc cccc cccc (b = National bank code, c = Account number)
            { Country.AT, new Regex(@"^AT[0-9]{18}$", RegexOptions.Compiled) },

            // Azerbaijan, Length: 28, BBAN: 4c,20n, Fields: AZkk bbbb cccc cccc cccc cccc cccc (b = National bank code, c = Account number)
            { Country.AZ, new Regex(@"^AZ[0-9]{2}[0-9A-Z]{4}[0-9]{20}$", RegexOptions.Compiled) },

            // Bahrain, Length: 22, BBAN: 4a,14c, Fields: BHkk bbbb cccc cccc cccc cc (b = National bank code, c = Account number)
            { Country.BH, new Regex(@"^BH[0-9]{2}[A-Z]{4}[0-9A-Z]{14}$", RegexOptions.Compiled) },

            // Belgium, Length: 16, BBAN: 12n, Fields: BEkk bbbc cccc ccxx (b = National bank code, c = Account number, x = National check digits)
            { Country.BE, new Regex(@"^BE[0-9]{14}$", RegexOptions.Compiled) },

            // Benin, Length: 28, BBAN: 1a, 23n, Fields: BJkk annn nnnn nnnn nnnn nnnn nnnn (Nordea catalogued)
            { Country.BJ, new Regex(@"^BJ[0-9]{2}[A-Z][0-9]{23}$", RegexOptions.Compiled) },

            // Bosnia and Herzegovina, Length: 20, BBAN: 16n, Fields: BAkk bbbs sscc cccc ccxx (k = IBAN check digits (always 39), b = National bank code, s = Branch code, c = Account number, x = National check digits)
            { Country.BA, new Regex(@"^BA39[0-9]{16}$", RegexOptions.Compiled) },

            // Brazil, Length: 29, BBAN: 23n, 1a, 1c, Fields: BRkk bbbb bbbb ssss sccc cccc ccct n (k = IBAN check digits, b = National bank code, s = Branch code, c = Account number, t = Account type (Cheque account, Savings account etc), n = Owner account number ("1", "2" etc))
            { Country.BR, new Regex(@"^BR[0-9]{25}[A-Z][0-9A-Z]$", RegexOptions.Compiled) },

            // Bulgaria, Length: 22, BBAN: 4a,6n,8c, Fields: BGkk bbbb ssss ddcc cccc cc (b = BIC bank code, s = Branch (BAE) number, d = Account type, c = Account number)
            { Country.BG, new Regex(@"^BG[0-9]{2}[A-Z]{4}[0-9]{6}[0-9A-Z]{8}$", RegexOptions.Compiled) },

            // Burkina Faso, Length: 27, BBAN: 23n, Fields: BFkk nnnn nnnn nnnn nnnn nnnn nnn (Nordea catalogued)
            { Country.BF, new Regex(@"^BF[0-9]{25}$", RegexOptions.Compiled) },

            // Burundi, Length: 16, BBAN: 12n, Fields: BIkk nnnn nnnn nnnn (Nordea catalogued)
            { Country.BI, new Regex(@"^BI[0-9]{14}$", RegexOptions.Compiled) },

            // Cameroon, Length: 27, BBAN: 23n, Fields: CMkk nnnn nnnn nnnn nnnn nnnn nnn (Nordea catalogued)
            { Country.CM, new Regex(@"^CM[0-9]{25}$", RegexOptions.Compiled) },

            // Cape Verde, Length: 25, BBAN: 21n, Fields: CVkk nnnn nnnn nnnn nnnn nnnn n (Nordea catalogued)
            { Country.CV, new Regex(@"^CV[0-9]{23}$", RegexOptions.Compiled) },

            // Costa Rica, Length: 21, BBAN: 17n, Fields: CRkk bbbc cccc cccc cccc c (b = bank code, c = Account number)
            { Country.CR, new Regex(@"^CR[0-9]{19}$", RegexOptions.Compiled) },

            // Croatia, Length: 21, BBAN: 17n, Fields: HRkk bbbb bbbc cccc cccc c (b = Bank code, c = Account number)
            { Country.HR, new Regex(@"^HR[0-9]{19}$", RegexOptions.Compiled) },

            // Cyprus, Length: 28, BBAN: 8n,16c, Fields: CYkk bbbs ssss cccc cccc cccc cccc (b = National bank code, s = Branch code, c = Account number)
            { Country.CY, new Regex(@"^CY[0-9]{10}[0-9A-Z]{16}$", RegexOptions.Compiled) },

            // Czech Republic, Length: 24, BBAN: 20n, Fields: CZkk bbbb ssss sscc cccc cccc (b = National bank code, s = Account number prefix, c = Account number)
            { Country.CZ, new Regex(@"^CZ[0-9]{22}$", RegexOptions.Compiled) },

            // Denmark, Length: 18, BBAN: 14n, Fields: DKkk bbbb cccc cccc cc (b = National bank code, c = Account number)
            { Country.DK, new Regex(@"^DK[0-9]{16}$", RegexOptions.Compiled) },

            // Dominican Republic, Length: 28, BBAN: 4a,20n, Fields: DOkk bbbb cccc cccc cccc cccc cccc (b = Bank identifier, c = Account number)
            { Country.DO, new Regex(@"^DO[0-9]{2}[A-Z]{4}[0-9]{20}$", RegexOptions.Compiled) },

            // Estonia, Length: 20, BBAN: 16n, Fields: EEkk bbss cccc cccc cccx (b = National bank code, s = Branch code, c = Account number, x = National check digit)
            { Country.EE, new Regex(@"^EE[0-9]{18}$", RegexOptions.Compiled) },

            // Faroe Islands, Length: 18, BBAN: 14n, Fields: FOkk bbbb cccc cccc cx (b = National bank code, c = Account number, x = National check digit)
            { Country.FO, new Regex(@"^FO[0-9]{16}$", RegexOptions.Compiled) },

            // Finland, Length: 18, BBAN: 14n, Fields: FIkk bbbb bbcc cccc cx (b = Bank and branch code, c = Account number, x = National check digit)
            { Country.FI, new Regex(@"^FI[0-9]{16}$", RegexOptions.Compiled) },

            // France, Length: 27, BBAN: 10n,11c,2n, Fields: FRkk bbbb bggg ggcc cccc cccc cxx (b = National bank code, g = Branch code (fr:code guichet), c = Account number, x = National check digits (fr:clé RIB))
            { Country.FR, new Regex(@"^FR[0-9]{12}[0-9A-Z]{11}[0-9][0-9]$", RegexOptions.Compiled) },

            // Georgia, Length: 22, BBAN: 2c,16n, Fields: GEkk bbcc cccc cccc cccc cc (b = National bank code, c = Account number)
            { Country.GE, new Regex(@"^GE[0-9]{2}[0-9A-Z]{2}[0-9]{16}$", RegexOptions.Compiled) },

            // Germany, Length: 22, BBAN: 18n, Fields: DEkk bbbb bbbb cccc cccc cc (b = Bank and branch identifier (de:Bankleitzahlor BLZ), c = Account number)
            { Country.DE, new Regex(@"^DE[0-9]{20}$", RegexOptions.Compiled) },

            // Gibraltar, Length: 23, BBAN: 4a,15c, Fields: GIkk bbbb cccc cccc cccc ccc (b = BIC bank code, c = Account number)
            { Country.GI, new Regex(@"^GI[0-9]{2}[A-Z]{4}[0-9A-Z]{15}$", RegexOptions.Compiled) },

            // Greece, Length: 27, BBAN: 7n,16c, Fields: GRkk bbbs sssc cccc cccc cccc ccc (b = National bank code, s = Branch code, c = Account number)
            { Country.GR, new Regex(@"^GR[0-9]{9}[0-9A-Z]{16}$", RegexOptions.Compiled) },

            // Greenland, Length: 18, BBAN: 14n, Fields: GLkk bbbb cccc cccc cc (b = National bank code, c = Account number)
            { Country.GL, new Regex(@"^GL[0-9]{16}$", RegexOptions.Compiled) },

            // Guatemala, Length: 28, BBAN: 4c,20c, Fields: GTkk bbbb cccc cccc cccc cccc cccc (b = National bank code, c = Account number)
            { Country.GT, new Regex(@"^GT[0-9]{2}[0-9A-Z]{4}[0-9A-Z]{20}$", RegexOptions.Compiled) },

            // Hungary, Length: 28, BBAN: 24n, Fields: HUkk bbbs sssk cccc cccc cccc cccx (b = National bank code, s = Branch code, c = Account number, x = National check digit)
            { Country.HU, new Regex(@"^HU[0-9]{26}$", RegexOptions.Compiled) },

            // Iceland, Length: 26, BBAN: 22n, Fields: ISkk bbbb sscc cccc iiii iiii ii (b = National bank code, s = Branch code, c = Account number, i = holder's kennitala (national identification number).)
            { Country.IS, new Regex(@"^IS[0-9]{24}$", RegexOptions.Compiled) },

            // Iran, Length: 26, BBAN: 22n, Fields: IRkk nnnn nnnn nnnn nnnn nnnn nn (Nordea catalogued)
            { Country.IR, new Regex(@"^IR[0-9]{24}$", RegexOptions.Compiled) },

            // Ireland, Length: 22, BBAN: 4c,14n, Fields: IEkk aaaa bbbb bbcc cccc cc (a = BIC bank code, b = Bank/branch code (sort code), c = Account number)
            { Country.IE, new Regex(@"^IE[0-9]{2}[0-9A-Z]{4}[0-9]{14}$", RegexOptions.Compiled) },

            // Israel, Length: 23, BBAN: 19n, Fields: ILkk bbbn nncc cccc cccc ccc (b = National bank code, n = Branch number, c = Account number 13 digits (padded with zeros))
            { Country.IL, new Regex(@"^IL[0-9]{21}$", RegexOptions.Compiled) },

            // Italy, Length: 27, BBAN: 1a,10n,12c, Fields: ITkk xaaa aabb bbbc cccc cccc ccc (x = Check char (CIN), a = National bank code (it:Associazione bancaria italiana or Codice ABI ), b = Branch code (it:Coordinate bancarie or CAB– Codice d'Avviamento Bancario), c = Account number)
            { Country.IT, new Regex(@"^IT[0-9]{2}[A-Z][0-9]{10}[0-9A-Z]{12}$", RegexOptions.Compiled) },

            // Ivory Coast, Length: 28, BBAN: 1a, 23n, Fields: CIkk annn nnnn nnnn nnnn nnnn nnnn (Nordea catalogued)
            { Country.CI, new Regex(@"^CI[0-9]{2}[A-Z][0-9]{23}$", RegexOptions.Compiled) },

            // Jordan, Length: 30, BBAN: 4a, 22n, Fields: JOkk bbbb nnnn 0000 0000 cccc cccc cc (b = National bank, n = Branch code, c = Account number)
            { Country.JO, new Regex(@"^JO[0-9]{2}[A-Z]{4}[0-9]{22}$", RegexOptions.Compiled) },

            // Kazakhstan, Length: 20, BBAN: 3n,13c, Fields: KZkk bbbc cccc cccc cccc (b = National bank code, c = Account number)
            { Country.KZ, new Regex(@"^KZ[0-9]{5}[0-9A-Z]{13}$", RegexOptions.Compiled) },

            // Kosovo, Length: 20, BBAN: 4n,10n,2n, Fields: XKkk bbbb cccc cccc cccc (b = National bank code, c = Account number)
            { Country.XK, new Regex(@"^XK[0-9]{6}[0-9]{10}[0-9][0-9]$", RegexOptions.Compiled) },

            // Kuwait, Length: 30, BBAN: 4a, 22c, Fields: KWkk bbbb cccc cccc cccc cccc cccc cc (b = National bank code, c = Account number)
            { Country.KW, new Regex(@"^KW[0-9]{2}[A-Z]{4}[0-9A-Z]{22}$", RegexOptions.Compiled) },

            // Latvia, Length: 21, BBAN: 4a,13c, Fields: LVkk bbbb cccc cccc cccc c (b = BIC Bank code, c = Account number)
            { Country.LV, new Regex(@"^LV[0-9]{2}[A-Z]{4}[0-9A-Z]{13}$", RegexOptions.Compiled) },

            // Lebanon, Length: 28, BBAN: 4n,20c, Fields: LBkk bbbb cccc cccc cccc cccc cccc (b = National bank code, c = Account number)
            { Country.LB, new Regex(@"^LB[0-9]{6}[0-9A-Z]{20}$", RegexOptions.Compiled) },

            // Liechtenstein, Length: 21, BBAN: 5n,12c, Fields: LIkk bbbb bccc cccc cccc c (b = National bank code, c = Account number)
            { Country.LI, new Regex(@"^LI[0-9]{7}[0-9A-Z]{12}$", RegexOptions.Compiled) },

            // Lithuania, Length: 20, BBAN: 16n, Fields: LTkk bbbb bccc cccc cccc (b = National bank code, c = Account number)
            { Country.LT, new Regex(@"^LT[0-9]{18}$", RegexOptions.Compiled) },

            // Luxembourg, Length: 20, BBAN: 3n,13c, Fields: LUkk bbbc cccc cccc cccc (b = National bank code, c = Account number)
            { Country.LU, new Regex(@"^LU[0-9]{5}[0-9A-Z]{13}$", RegexOptions.Compiled) },

            // Macedonia, Length: 19, BBAN: 3n,10c,2n, Fields: MKkk bbbc cccc cccc cxx (k = IBAN check digits (always = "07"), b = National bank code, c = Account number, x = National check digits)
            { Country.MK, new Regex(@"^MK07[0-9]{3}[0-9A-Z]{10}[0-9][0-9]$", RegexOptions.Compiled) },

            // Madagascar, Length: 27, BBAN: 23n, Fields: MGkk nnnn nnnn nnnn nnnn nnnn nnn (Nordea catalogued)
            { Country.MG, new Regex(@"^MG[0-9]{25}$", RegexOptions.Compiled) },

            // Mali, Length: 28, BBAN: 1a, 23n, Fields: MLkk annn nnnn nnnn nnnn nnnn nnnn (Nordea catalogued)
            { Country.ML, new Regex(@"^ML[0-9]{2}[A-Z][0-9]{23}$", RegexOptions.Compiled) },

            // Malta, Length: 31, BBAN: 4a,5n,18c, Fields: MTkk bbbb ssss sccc cccc cccc cccc ccc (b = BIC bank code, s = Branch code, c = Account number)
            { Country.MT, new Regex(@"^MT[0-9]{2}[A-Z]{4}[0-9]{5}[0-9A-Z]{18}$", RegexOptions.Compiled) },

            // Mauritania, Length: 27, BBAN: 23n, Fields: MRkk bbbb bsss sscc cccc cccc cxx (k = IBAN check digits (always 13), b = National bank code, s = Branch code (fr:code guichet), c = Account number, x = National check digits (fr:clé RIB))
            { Country.MR, new Regex(@"^MR13[0-9]{23}$", RegexOptions.Compiled) },

            // Mauritius, Length: 30, BBAN: 4a,19n,3a, Fields: MUkk bbbb bbss cccc cccc cccc cccc cc (b = National bank code, s = Branch identifier, c = Account number)
            { Country.MU, new Regex(@"^MU[0-9]{2}[A-Z]{4}[0-9]{19}[A-Z]{3}$", RegexOptions.Compiled) },

            // Moldova, Length: 24, BBAN: 2c,18n, Fields: MDkk bbcc cccc cccc cccc cccc (b = National bank code, c = Account number)
            { Country.MD, new Regex(@"^MD[0-9]{2}[0-9A-Z]{2}[0-9]{18}$", RegexOptions.Compiled) },

            // Monaco, Length: 27, BBAN: 10n,11c,2n, Fields: MCkk bbbb bsss sscc cccc cccc cxx (b = National bank code, s = Branch code (fr:code guichet), c = Account number, x = National check digits (fr:clé RIB).)
            { Country.MC, new Regex(@"^MC[0-9]{12}[0-9A-Z]{11}[0-9][0-9]$", RegexOptions.Compiled) },

            // Montenegro, Length: 22, BBAN: 18n, Fields: MEkk bbbc cccc cccc cccc xx (k = IBAN check digits (always = "25"), b = Bank code, c = Account number, x = National check digits)
            { Country.ME, new Regex(@"^ME25[0-9]{18}$", RegexOptions.Compiled) },

            // Mozambique, Length: 25, BBAN: 21n, Fields: MZkk nnnn nnnn nnnn nnnn nnnn n (Nordea catalogued)
            { Country.MZ, new Regex(@"^MZ[0-9]{23}$", RegexOptions.Compiled) },

            // Netherlands, Length: 18, BBAN: 4a,10n, Fields: NLkk bbbb cccc cccc cc (b = BIC Bank code, c = Account number)
            { Country.NL, new Regex(@"^NL[0-9]{2}[A-Z]{4}[0-9]{10}$", RegexOptions.Compiled) },

            // Norway, Length: 15, BBAN: 11n, Fields: NOkk bbbb cccc ccx (b = National bank code, c = Account number, x = Modulo-11 national check digit)
            { Country.NO, new Regex(@"^NO[0-9]{13}$", RegexOptions.Compiled) },

            // Pakistan, Length: 24, BBAN: 4c,16n, Fields: PKkk bbbb cccc cccc cccc cccc (b = National bank code, c = Account number)
            { Country.PK, new Regex(@"^PK[0-9]{2}[0-9A-Z]{4}[0-9]{16}$", RegexOptions.Compiled) },

            // Palestinian, Length: 29, BBAN: 4c,21n, Fields: PSkk bbbb xxxx xxxx xccc cccc cccc c (b = National bank code, c = Account number, x = Not specified)
            { Country.PS, new Regex(@"^PS[0-9]{2}[0-9A-Z]{4}[0-9]{21}$", RegexOptions.Compiled) },

            // Poland, Length: 28, BBAN: 24n, Fields: PLkk bbbs sssx cccc cccc cccc cccc (b = National bank code, s = Branch code, x = National check digit, c = Account number)
            { Country.PL, new Regex(@"^PL[0-9]{26}$", RegexOptions.Compiled) },

            // Portugal, Length: 25, BBAN: 21n, Fields: PTkk bbbb ssss cccc cccc cccx x (k = IBAN check digits (always = "50"), b = National bank code, s = Branch code, C = Account number, x = National check digit)
            { Country.PT, new Regex(@"^PT50[0-9]{21}$", RegexOptions.Compiled) },

            // Qatar, Length: 29, BBAN: 4a, 21c, Fields: QAkk bbbb cccc cccc cccc cccc cccc c (b = National bank code, c = Account number)
            { Country.QA, new Regex(@"^QA[0-9]{2}[A-Z]{4}[0-9A-Z]{21}$", RegexOptions.Compiled) },

            // Romania, Length: 24, BBAN: 4a,16c, Fields: ROkk bbbb cccc cccc cccc cccc (b = BIC Bank code, c = Branch code and account number (bank-specific format))
            { Country.RO, new Regex(@"^RO[0-9]{2}[A-Z]{4}[0-9A-Z]{16}$", RegexOptions.Compiled) },

            // San Marino, Length: 27, BBAN: 1a,10n,12c, Fields: SMkk xaaa aabb bbbc cccc cccc ccc (x = Check char (it:CIN), a = National bank code (it:Associazione bancaria italiana or Codice ABI), b = Branch code (it:Coordinate bancarie or CAB– Codice d'Avviamento Bancario), c = Account number)
            { Country.SM, new Regex(@"^SM[0-9]{2}[A-Z][0-9]{10}[0-9A-Z]{12}$", RegexOptions.Compiled) },

            // Saudi Arabia, Length: 24, BBAN: 2n,18c, Fields: SAkk bbcc cccc cccc cccc cccc (b = National bank code, c = Account number preceded by zeros, if required)
            { Country.SA, new Regex(@"^SA[0-9]{4}[0-9A-Z]{18}$", RegexOptions.Compiled) },

            // Senegal, Length: 28, BBAN: 1a, 23n, Fields: SNkk annn nnnn nnnn nnnn nnnn nnnn (Nordea catalogued)
            { Country.SN, new Regex(@"^SN[0-9]{2}[A-Z][0-9]{23}$", RegexOptions.Compiled) },

            // Serbia, Length: 22, BBAN: 18n, Fields: RSkk bbbc cccc cccc cccc xx (b = National bank code, c = Account number, x = Account check digits)
            { Country.RS, new Regex(@"^RS[0-9]{20}$", RegexOptions.Compiled) },

            // Slovakia, Length: 24, BBAN: 20n, Fields: SKkk bbbb ssss sscc cccc cccc (b = National bank code, s = Account number prefix, c = Account number)
            { Country.SK, new Regex(@"^SK[0-9]{22}$", RegexOptions.Compiled) },

            // Slovenia, Length: 19, BBAN: 15n, Fields: SIkk bbss sccc cccc cxx (k = IBAN check digits (always = "56"), b = National bank code, s = Branch code, c = Account number, x = National check digits)
            { Country.SI, new Regex(@"^SI56[0-9]{15}$", RegexOptions.Compiled) },

            // Spain, Length: 24, BBAN: 20n, Fields: ESkk bbbb gggg xxcc cccc cccc (b = National bank code, g = Branch code, x = Check digits, c = Account number)
            { Country.ES, new Regex(@"^ES[0-9]{22}$", RegexOptions.Compiled) },

            // Sweden, Length: 24, BBAN: 20n, Fields: SEkk bbbc cccc cccc cccc cccx (b = National bank code, c = Account number, x = Checksum)
            { Country.SE, new Regex(@"^SE[0-9]{22}$", RegexOptions.Compiled) },

            // Switzerland, Length: 21, BBAN: 5n,12c, Fields: CHkk bbbb bccc cccc cccc c (b = National bank code, c = Account number)
            { Country.CH, new Regex(@"^CH[0-9]{7}[0-9A-Z]{12}$", RegexOptions.Compiled) },

            // Tunisia, Length: 24, BBAN: 20n, Fields: TNkk bbss sccc cccc cccc cccc (k = IBAN check digits (always 59), b = National bank code, s = Branch code, c = Account number)
            { Country.TN, new Regex(@"^TN59[0-9]{20}$", RegexOptions.Compiled) },

            // Turkey, Length: 26, BBAN: 5n,17c, Fields: TRkk bbbb bxcc cccc cccc cccc cc (b = National bank code, x = Reserved for future use (currently "0"), c = Account number)
            { Country.TR, new Regex(@"^TR[0-9]{7}[0-9A-Z]{17}$", RegexOptions.Compiled) },

            // Ukraine, Length: 29, BBAN: 6n,19c, Fields: UAkk bbbb bbcc cccc cccc cccc cccc c (b = National bank code, c = Account number)
            { Country.UA, new Regex(@"^UA[0-9]{8}[0-9A-Z]{19}$", RegexOptions.Compiled) },

            // United Arab Emirates, Length: 23, BBAN: 3n,16n, Fields: AEkk bbbc cccc cccc cccc ccc (b = National bank code, c = Account number)
            { Country.AE, new Regex(@"^AE[0-9]{5}[0-9]{16}$", RegexOptions.Compiled) },

            // United Kingdom, Length: 22, BBAN: 4a,14n, Fields: GBkk bbbb ssss sscc cccc cc (b = BIC bank code, s = Bank and branch code (sort code), c = Account number)
            { Country.GB, new Regex(@"^GB[0-9]{2}[A-Z]{4}[0-9]{14}$", RegexOptions.Compiled) },

            // Virgin Islands, British, Length: 24, BBAN: 4c,16n, Fields: VGkk bbbb cccc cccc cccc cccc (b = National bank code, c = Account number)
            { Country.VG, new Regex(@"^VG[0-9]{2}[0-9A-Z]{4}[0-9]{16}$", RegexOptions.Compiled) },

        };
    }
}
