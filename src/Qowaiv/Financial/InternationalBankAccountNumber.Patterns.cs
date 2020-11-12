#pragma warning disable S1210
// "Equals" and the comparison operators should be overridden when implementing "IComparable"
// See README.md => Sortable

using Qowaiv.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Qowaiv.Financial
{
    public partial struct InternationalBankAccountNumber
    {
        /// <summary>Gets the localized patterns.</summary>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/International_Bank_Account_Number.
        /// </remarks>
        private static readonly Dictionary<Country, Regex> LocalizedPatterns = new[]
        {
            Bban(Country.AD, "8n,12c"),
            Bban(Country.AE, "3n,16n"),
            Bban(Country.AL, "8n,16c"),
            Bban(Country.AT, "16n"),
            Bban(Country.AZ, "4c,20n"),
            Bban(Country.BA, "16n", checksum: 39),
            Bban(Country.BE, "12n"),
            Bban(Country.BG, "4a,6n,8c"),
            Bban(Country.BH, "4a,14c"),
            Bban(Country.BR, "23n,1a,1c"),
            Bban(Country.BY, "4c,4n,16c"),
            Bban(Country.CH, "5n,12c"),
            Bban(Country.CR, "[0],17n"),
            Bban(Country.CY, "8n,16c"),
            Bban(Country.CZ, "20n"),
            Bban(Country.DE, "18n"),
            Bban(Country.DK, "14n"),
            Bban(Country.DO, "4a,20n"),
            Bban(Country.EE, "16n"),
            Bban(Country.EG, "25n"),
            Bban(Country.ES, "20n"),
            Bban(Country.FI, "14n"),
            Bban(Country.FO, "14n"),
            Bban(Country.FR, "10n,11c,2n"),
            Bban(Country.GB, "4a,14n"),
            Bban(Country.GE, "2c,16n"),
            Bban(Country.GI, "4a,15c"),
            Bban(Country.GL, "14n"),
            Bban(Country.GR, "7n,16c"),
            Bban(Country.GT, "4c,20c"),
            Bban(Country.HR, "17n"),
            Bban(Country.HU, "24n"),
            Bban(Country.IE, "4c,14n"),
            Bban(Country.IL, "19n"),
            Bban(Country.IQ, "4a,15n"),
            Bban(Country.IS, "22n"),
            Bban(Country.IT, "1a,10n,12c"),
            Bban(Country.JO, "4a,22n"),
            Bban(Country.KW, "4a,22c"),
            Bban(Country.KZ, "3n,13c"),
            Bban(Country.LB, "4n,20c"),
            Bban(Country.LC, "4a,24c"),
            Bban(Country.LI, "5n,12c"),
            Bban(Country.LT, "16n"),
            Bban(Country.LU, "3n,13c"),
            Bban(Country.LV, "4a,13c"),
            Bban(Country.MC, "10n,11c,2n"),
            Bban(Country.MD, "2c,18c"),
            Bban(Country.ME, "18n", checksum: 25),
            Bban(Country.MK, "3n,10c,2n", checksum: 07),
            Bban(Country.MR, "23n", checksum: 13),
            Bban(Country.MT, "4a,5n,18c"),
            Bban(Country.MU, "4a,16n,[000],3a"),
            Bban(Country.NL, "4a,10n"),
            Bban(Country.NO, "11n"),
            Bban(Country.PK, "4c,16n"),
            Bban(Country.PL, "24n"),
            Bban(Country.PS, "4c,21n"),
            Bban(Country.PT, "21n",checksum: 50),
            Bban(Country.QA, "4a,21c"),
            Bban(Country.RO, "4a,16c"),
            Bban(Country.RS, "18n", checksum: 35),
            Bban(Country.SA, "2n,18c"),
            Bban(Country.SC, "4a,20n,3a"),
            Bban(Country.SE, "20n"),
            Bban(Country.SI, "15n", checksum: 56),
            Bban(Country.SK, "20n"),
            Bban(Country.SM, "1a,10n,12c"),
            Bban(Country.ST, "21n"),
            Bban(Country.SV, "4a,20n"),
            Bban(Country.TL, "19n", checksum: 38),
            Bban(Country.TN, "20n", checksum: 59),
            Bban(Country.TR, "5n,17c"),
            Bban(Country.UA, "6n,19c"),
            Bban(Country.VA, "3n,15n"),
            Bban(Country.VG, "4c,16n"),
            Bban(Country.XK, "4n,10n,2n"),

            // Not officially
            Bban(Country.AO, "21n"),
            Bban(Country.BF, "24n"),
            Bban(Country.BI, "12n"),
            Bban(Country.BJ, "1a,23n"),
            Bban(Country.CG, "23n"),
            Bban(Country.CI, "1a,23n"),
            Bban(Country.CM, "23n"),
            Bban(Country.CV, "21n"),
            Bban(Country.DJ, "23n"),
            Bban(Country.DZ, "20n"),
            Bban(Country.GA, "23n"),
            Bban(Country.GQ, "23n"),
            Bban(Country.GW, "21n"),
            Bban(Country.HN, "4a,20n"),
            Bban(Country.IR, "4c,18n"),
            Bban(Country.KM, "23n"),
            Bban(Country.MA, "24n"),
            Bban(Country.MG, "23n"),
            Bban(Country.ML, "1a,23n"),
            Bban(Country.MZ, "21n"),
            Bban(Country.NE, "2a,22n"),
            Bban(Country.NI, "4a,24n"),
            Bban(Country.SN, "1a,23n"),
            Bban(Country.TD, "23n"),
            Bban(Country.TG, "2a,22n"),
        }
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        /// <summary>A list with countries supporting IBAN.</summary>
        public static readonly IReadOnlyCollection<Country> Supported = new ReadOnlyCollection<Country>(LocalizedPatterns.Select(kvp => kvp.Key).ToList());
    }
}
