#pragma warning disable S2699 // Tests should include assertions
// Calls IsNotValid with Assert methods.

using NUnit.Framework;
using Qowaiv.Globalization;

namespace Qowaiv.UnitTests
{
    public partial class PostalCodeTest
    {
        #region IsNotValid Country tests

        /// <summary>Tests patterns that should not be valid for Andorra (AD).</summary>
        [Test]
        public void IsNotValidCustomCases_AD_All()
        {
            var country = Country.AD;

            IsNotValid("AD890", country);
            IsNotValid("AD901", country);
            IsNotValid("AD012", country);
        }

        /// <summary>Tests patterns that should not be valid for Afghanistan (AF).</summary>
        [Test]
        public void IsNotValidCustomCases_AF_All()
        {
            var country = Country.AF;

            IsNotValid("0077", country);
            IsNotValid("5301", country);
            IsNotValid("6001", country);
            IsNotValid("7023", country);
            IsNotValid("8100", country);
            IsNotValid("9020", country);
            IsNotValid("4441", country);
            IsNotValid("4300", country);
        }

        /// <summary>Tests patterns that should not be valid for Anguilla (AI).</summary>
        [Test]
        public void IsNotValidCustomCases_AI_All()
        {
            var country = Country.AI;

            IsNotValid("9218", country);
            IsNotValid("AI1890", country);
            IsNotValid("AI2901", country);
            IsNotValid("AI2012", country);
        }

        /// <summary>Tests patterns that should not be valid for Albania (AL).</summary>
        [Test]
        public void IsNotValidCustomCases_AL_All()
        {
            var country = Country.AL;

            IsNotValid("0000", country);
            IsNotValid("0125", country);
            IsNotValid("0279", country);
            IsNotValid("0399", country);
            IsNotValid("0418", country);
            IsNotValid("0540", country);
            IsNotValid("0654", country);
            IsNotValid("0790", country);
            IsNotValid("0852", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Armenia (AM).</summary>
        [Test]
        public void IsNotValidCustomCases_AM_All()
        {
            var country = Country.AM;

            IsNotValid("5697", country);
            IsNotValid("6282", country);
            IsNotValid("7611", country);
            IsNotValid("6767", country);
            IsNotValid("8752", country);
            IsNotValid("9999", country);
        }

        /// <summary>Tests patterns that should not be valid for Argentina (AR).</summary>
        [Test]
        public void IsNotValidCustomCases_AR_All()
        {
            var country = Country.AR;

            IsNotValid("A0400XXX", country);
            IsNotValid("S03004DD", country);
        }

        /// <summary>Tests patterns that should not be valid for American Samoa (AS).</summary>
        [Test]
        public void IsNotValidCustomCases_AS_All()
        {
            var country = Country.AS;

            IsNotValid("00000", country);
            IsNotValid("01449", country);
            IsNotValid("12823", country);
            IsNotValid("20375", country);
            IsNotValid("32434", country);
            IsNotValid("40082", country);
            IsNotValid("56978", country);
            IsNotValid("62824", country);
            IsNotValid("76113", country);
            IsNotValid("67671", country);
            IsNotValid("87523", country);
            IsNotValid("00000000", country);
            IsNotValid("01494232", country);
            IsNotValid("12835343", country);
            IsNotValid("20357004", country);
            IsNotValid("32443647", country);
            IsNotValid("40027947", country);
            IsNotValid("56983645", country);
            IsNotValid("62846908", country);
            IsNotValid("76134349", country);
            IsNotValid("67618550", country);
            IsNotValid("87539183", country);
        }

        /// <summary>Tests patterns that should not be valid for Austria (AT).</summary>
        [Test]
        public void IsNotValidCustomCases_AT_All()
        {
            var country = Country.AT;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Australia (AU).</summary>
        [Test]
        public void IsNotValidCustomCases_AU_All()
        {
            var country = Country.AU;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
        }

        /// <summary>Tests patterns that should not be valid for Åland Islands (AX).</summary>
        [Test]
        public void IsNotValidCustomCases_AX_All()
        {
            var country = Country.AX;

            IsNotValid("0000", country);
            IsNotValid("0144", country);
            IsNotValid("1282", country);
            IsNotValid("2037", country);
            IsNotValid("2000", country);
            IsNotValid("2014", country);
            IsNotValid("2122", country);
            IsNotValid("2323", country);
            IsNotValid("2408", country);
            IsNotValid("2567", country);
            IsNotValid("2622", country);
            IsNotValid("2761", country);
            IsNotValid("2677", country);
            IsNotValid("2872", country);
            IsNotValid("2999", country);
            IsNotValid("0144", country);
            IsNotValid("1282", country);
            IsNotValid("2037", country);
            IsNotValid("3243", country);
            IsNotValid("4008", country);
            IsNotValid("5697", country);
            IsNotValid("6282", country);
            IsNotValid("7611", country);
            IsNotValid("6767", country);
            IsNotValid("8752", country);
            IsNotValid("9999", country);
        }

        /// <summary>Tests patterns that should not be valid for Azerbaijan (AZ).</summary>
        [Test]
        public void IsNotValidCustomCases_AZ_All()
        {
            var country = Country.AZ;

            IsNotValid("DK 6990", country);
            IsNotValid("GL3990", country);
            IsNotValid("FO1990", country);
            IsNotValid("FO990", country);
        }

        /// <summary>Tests patterns that should not be valid for Bosnia And Herzegovina (BA).</summary>
        [Test]
        public void IsNotValidCustomCases_BA_All()
        {
            var country = Country.BA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Barbados (BB).</summary>
        [Test]
        public void IsNotValidCustomCases_BB_All()
        {
            var country = Country.BB;

            IsNotValid("AA00000", country);
            IsNotValid("AS01449", country);
            IsNotValid("BJ12823", country);
            IsNotValid("CD20375", country);
            IsNotValid("DE32434", country);
            IsNotValid("EO40082", country);
            IsNotValid("FN56978", country);
            IsNotValid("GF62824", country);
            IsNotValid("HL76113", country);
            IsNotValid("ID67671", country);
            IsNotValid("JS87523", country);
            IsNotValid("KN99999", country);
            IsNotValid("LO00000", country);
            IsNotValid("ME01449", country);
            IsNotValid("NN12823", country);
            IsNotValid("OL20375", country);
            IsNotValid("PS32434", country);
            IsNotValid("QD40082", country);
            IsNotValid("RN56978", country);
            IsNotValid("SE62824", country);
            IsNotValid("TM76113", country);
            IsNotValid("UF67671", country);
            IsNotValid("VE87523", country);
            IsNotValid("WL99999", country);
            IsNotValid("XM00000", country);
            IsNotValid("YE01449", country);
            IsNotValid("ZL12823", country);
            IsNotValid("ZZ20375", country);
        }

        /// <summary>Tests patterns that should not be valid for Bangladesh (BD).</summary>
        [Test]
        public void IsNotValidCustomCases_BD_All()
        {
            var country = Country.BD;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Belgium (BE).</summary>
        [Test]
        public void IsNotValidCustomCases_BE_All()
        {
            var country = Country.BE;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Bulgaria (BG).</summary>
        [Test]
        public void IsNotValidCustomCases_BG_All()
        {
            var country = Country.BG;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Bahrain (BH).</summary>
        [Test]
        public void IsNotValidCustomCases_BH_All()
        {
            var country = Country.BH;

            IsNotValid("000", country);
            IsNotValid("014", country);
            IsNotValid("1328", country);
            IsNotValid("2037", country);
            IsNotValid("3243", country);
            IsNotValid("4008", country);
            IsNotValid("5697", country);
            IsNotValid("6282", country);
            IsNotValid("7611", country);
            IsNotValid("6767", country);
            IsNotValid("8752", country);
            IsNotValid("9999", country);
        }

        /// <summary>Tests patterns that should not be valid for Saint Barthélemy (BL).</summary>
        [Test]
        public void IsNotValidCustomCases_BL_All()
        {
            var country = Country.BL;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Bermuda (BM).</summary>
        [Test]
        public void IsNotValidCustomCases_BM_All()
        {
            var country = Country.BM;

            IsNotValid("A0", country);
            IsNotValid("A1", country);
            IsNotValid("B2", country);
            IsNotValid("C0", country);
            IsNotValid("D2", country);
            IsNotValid("E0", country);
            IsNotValid("6F", country);
            IsNotValid("2G", country);
            IsNotValid("6H", country);
            IsNotValid("7I", country);
            IsNotValid("7J", country);
            IsNotValid("9K", country);
            IsNotValid("0L", country);
            IsNotValid("013S", country);
            IsNotValid("12RF", country);
            IsNotValid("2034", country);
            IsNotValid("2A34", country);
        }

        /// <summary>Tests patterns that should not be valid for Brunei Darussalam (BN).</summary>
        [Test]
        public void IsNotValidCustomCases_BN_All()
        {
            var country = Country.BN;

            IsNotValid("0000DF", country);
            IsNotValid("2325DS", country);
            IsNotValid("3436RF", country);
            IsNotValid("0044WK", country);
            IsNotValid("6478SD", country);
            IsNotValid("9475PJ", country);
            IsNotValid("6450KF", country);
            IsNotValid("9088LS", country);
            IsNotValid("3495JD", country);
            IsNotValid("5502MO", country);
            IsNotValid("1832DF", country);
            IsNotValid("K999JS", country);
            IsNotValid("L000DF", country);
            IsNotValid("M325DS", country);
            IsNotValid("N436RF", country);
            IsNotValid("O044WK", country);
            IsNotValid("P478SD", country);
            IsNotValid("Q475PJ", country);
            IsNotValid("RN578F", country);
            IsNotValid("SE624S", country);
            IsNotValid("TM713D", country);
            IsNotValid("UF671O", country);
            IsNotValid("VE823F", country);
            IsNotValid("WL999S", country);
            IsNotValid("XMD000", country);
            IsNotValid("YED014", country);
            IsNotValid("ZLR128", country);
            IsNotValid("ZZW203", country);
        }

        /// <summary>Tests patterns that should not be valid for Bolivia (BO).</summary>
        [Test]
        public void IsNotValidCustomCases_BO_All()
        {
            var country = Country.BO;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Brazil (BR).</summary>
        [Test]
        public void IsNotValidCustomCases_BR_All()
        {
            var country = Country.BR;

            IsNotValid("00000000", country);
            IsNotValid("00014494", country);
            IsNotValid("00128235", country);
            IsNotValid("00203757", country);
            IsNotValid("00324343", country);
            IsNotValid("00400827", country);
            IsNotValid("00569783", country);
            IsNotValid("00628246", country);
            IsNotValid("00761134", country);
            IsNotValid("00676718", country);
            IsNotValid("00875239", country);
            IsNotValid("00999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Bhutan (BT).</summary>
        [Test]
        public void IsNotValidCustomCases_BT_All()
        {
            var country = Country.BT;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Belarus (BY).</summary>
        [Test]
        public void IsNotValidCustomCases_BY_All()
        {
            var country = Country.BY;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Canada (CA).</summary>
        [Test]
        public void IsNotValidCustomCases_CA_All()
        {
            var country = Country.CA;

            // No D, F, I, O, Q, and U (and W and Z to start with).

            IsNotValid("H0D0H0", country);
            IsNotValid("A0F0D0", country);
            IsNotValid("A0I1D4", country);
            IsNotValid("B1O2R8", country);
            IsNotValid("C2Q0W3", country);
            IsNotValid("D3U2S4", country);
            IsNotValid("E4O0D0", country);
            IsNotValid("F5N6F9", country);
            IsNotValid("G6F2I8", country);
            IsNotValid("D7L6O1", country);
            IsNotValid("F6D7Q6", country);
            IsNotValid("I8S7U5", country);
            IsNotValid("O9N9J9", country);
            IsNotValid("Q0O0D0", country);
            IsNotValid("U0E1D4", country);
            IsNotValid("W1N2R8", country);
            IsNotValid("Z2L0W3", country);
        }

        /// <summary>Tests patterns that should not be valid for Cocos (CC).</summary>
        [Test]
        public void IsNotValidCustomCases_CC_All()
        {
            var country = Country.CC;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Switzerland (CH).</summary>
        [Test]
        public void IsNotValidCustomCases_CH_All()
        {
            var country = Country.CH;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Chile (CL).</summary>
        [Test]
        public void IsNotValidCustomCases_CL_All()
        {
            var country = Country.CL;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for China (CN).</summary>
        [Test]
        public void IsNotValidCustomCases_CN_All()
        {
            var country = Country.CN;

            IsNotValid("0000", country);
            IsNotValid("0004", country);
            IsNotValid("0018", country);
            IsNotValid("0023", country);
            IsNotValid("0034", country);
            IsNotValid("0040", country);
            IsNotValid("0059", country);
            IsNotValid("0068", country);
            IsNotValid("0071", country);
            IsNotValid("0066", country);
            IsNotValid("0085", country);
            IsNotValid("0099", country);
        }

        /// <summary>Tests patterns that should not be valid for Colombia (CO).</summary>
        [Test]
        public void IsNotValidCustomCases_CO_All()
        {
            var country = Country.CO;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Costa Rica (CR).</summary>
        [Test]
        public void IsNotValidCustomCases_CR_All()
        {
            var country = Country.CR;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Cuba (CU).</summary>
        [Test]
        public void IsNotValidCustomCases_CU_All()
        {
            var country = Country.CU;

            IsNotValid("CP000", country);
            IsNotValid("CP035", country);
            IsNotValid("CP146", country);
            IsNotValid("CP204", country);
            IsNotValid("CP348", country);
        }

        /// <summary>Tests patterns that should not be valid for Cape Verde (CV).</summary>
        [Test]
        public void IsNotValidCustomCases_CV_All()
        {
            var country = Country.CV;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Christmas Island (CX).</summary>
        [Test]
        public void IsNotValidCustomCases_CX_All()
        {
            var country = Country.CX;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Cyprus (CY).</summary>
        [Test]
        public void IsNotValidCustomCases_CY_All()
        {
            var country = Country.CY;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Czech Republic (CZ).</summary>
        [Test]
        public void IsNotValidCustomCases_CZ_All()
        {
            var country = Country.CZ;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);

            IsNotValid("8000", country);
            IsNotValid("8004", country);
            IsNotValid("8018", country);
            IsNotValid("8023", country);
            IsNotValid("8034", country);
            IsNotValid("8040", country);
            IsNotValid("9059", country);
            IsNotValid("9068", country);
            IsNotValid("9071", country);
            IsNotValid("9066", country);
            IsNotValid("9085", country);
            IsNotValid("9099", country);
        }

        /// <summary>Tests patterns that should not be valid for Germany (DE).</summary>
        [Test]
        public void IsNotValidCustomCases_DE_All()
        {
            var country = Country.DE;

            IsNotValid("00007", country);
            IsNotValid("00043", country);
            IsNotValid("00188", country);
            IsNotValid("00237", country);
            IsNotValid("00342", country);
            IsNotValid("00401", country);
            IsNotValid("00597", country);
            IsNotValid("00682", country);
            IsNotValid("00719", country);
            IsNotValid("00665", country);
            IsNotValid("00853", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for Denmark (DK).</summary>
        [Test]
        public void IsNotValidCustomCases_DK_All()
        {
            var country = Country.DK;

            IsNotValid("0000", country);
            IsNotValid("0014", country);
            IsNotValid("0128", country);
            IsNotValid("0203", country);
            IsNotValid("0324", country);
            IsNotValid("0400", country);
            IsNotValid("0569", country);
            IsNotValid("0628", country);
            IsNotValid("0761", country);
            IsNotValid("0676", country);
            IsNotValid("0875", country);
            IsNotValid("0999", country);

            IsNotValid("DK0000", country);
            IsNotValid("DK0014", country);
            IsNotValid("DK0128", country);
            IsNotValid("DK0203", country);
            IsNotValid("DK0324", country);
            IsNotValid("DK0400", country);
            IsNotValid("DK0569", country);
            IsNotValid("DK0628", country);
            IsNotValid("DK0761", country);
            IsNotValid("DK0676", country);
            IsNotValid("DK0875", country);
            IsNotValid("DK0999", country);

            IsNotValid("AA0000", country);
            IsNotValid("AS0144", country);
            IsNotValid("BJ1282", country);
            IsNotValid("CD2037", country);
            IsNotValid("DE3243", country);
            IsNotValid("EO4008", country);
            IsNotValid("FN5697", country);
            IsNotValid("GF6282", country);
            IsNotValid("HL7611", country);
            IsNotValid("ID6767", country);
            IsNotValid("JS8752", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0144", country);
            IsNotValid("NN1282", country);
        }

        /// <summary>Tests patterns that should not be valid for Algeria (DZ).</summary>
        [Test]
        public void IsNotValidCustomCases_DZ_All()
        {
            var country = Country.DZ;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Ecuador (EC).</summary>
        [Test]
        public void IsNotValidCustomCases_EC_All()
        {
            var country = Country.EC;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Estonia (EE).</summary>
        [Test]
        public void IsNotValidCustomCases_EE_All()
        {
            var country = Country.EE;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Egypt (EG).</summary>
        [Test]
        public void IsNotValidCustomCases_EG_All()
        {
            var country = Country.EG;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
        }

        /// <summary>Tests patterns that should not be valid for Spain (ES).</summary>
        [Test]
        public void IsNotValidCustomCases_ES_All()
        {
            var country = Country.ES;

            IsNotValid("00000", country);
            IsNotValid("53434", country);
            IsNotValid("54082", country);
            IsNotValid("55978", country);
            IsNotValid("56824", country);
            IsNotValid("57113", country);
            IsNotValid("56671", country);
            IsNotValid("58523", country);
            IsNotValid("59999", country);
            IsNotValid("62824", country);
            IsNotValid("76113", country);
            IsNotValid("67671", country);
            IsNotValid("87523", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Ethiopia (ET).</summary>
        [Test]
        public void IsNotValidCustomCases_ET_All()
        {
            var country = Country.ET;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Finland (FI).</summary>
        [Test]
        public void IsNotValidCustomCases_FI_All()
        {
            var country = Country.FI;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Falkland Islands (FK).</summary>
        [Test]
        public void IsNotValidCustomCases_FK_All()
        {
            var country = Country.FK;

            IsNotValid("FIAA1ZZ", country);
            IsNotValid("FIAA9ZZ", country);
            IsNotValid("DN551PT", country);
            IsNotValid("DN551PT", country);
            IsNotValid("EC1A1BB", country);
            IsNotValid("EC1A1BB", country);

        }

        /// <summary>Tests patterns that should not be valid for Micronesia (FM).</summary>
        [Test]
        public void IsNotValidCustomCases_FM_All()
        {
            var country = Country.FM;

            IsNotValid("00000", country);
            IsNotValid("01234", country);
            IsNotValid("01449", country);
            IsNotValid("12823", country);
            IsNotValid("20375", country);
            IsNotValid("32434", country);
            IsNotValid("40082", country);
            IsNotValid("56978", country);
            IsNotValid("62824", country);
            IsNotValid("76113", country);
            IsNotValid("67671", country);
            IsNotValid("87523", country);
            IsNotValid("99999", country);

            IsNotValid("000000000", country);
            IsNotValid("012345678", country);
            IsNotValid("014494232", country);
            IsNotValid("128235343", country);
            IsNotValid("203757004", country);
            IsNotValid("324343647", country);
            IsNotValid("400827947", country);
            IsNotValid("569783645", country);
            IsNotValid("628246908", country);
            IsNotValid("761134349", country);
            IsNotValid("676718550", country);
            IsNotValid("875239183", country);
            IsNotValid("999999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Faroe Islands (FO).</summary>
        [Test]
        public void IsNotValidCustomCases_FO_All()
        {
            var country = Country.FO;

            IsNotValid("000", country);
            IsNotValid("004", country);
            IsNotValid("018", country);
            IsNotValid("023", country);
            IsNotValid("034", country);
            IsNotValid("040", country);
            IsNotValid("059", country);
            IsNotValid("068", country);
            IsNotValid("071", country);
            IsNotValid("066", country);
            IsNotValid("085", country);
            IsNotValid("099", country);

            IsNotValid("FO000", country);
            IsNotValid("FO004", country);
            IsNotValid("FO018", country);
            IsNotValid("FO023", country);
            IsNotValid("FO034", country);
            IsNotValid("FO040", country);
            IsNotValid("FO059", country);
            IsNotValid("FO068", country);
            IsNotValid("FO071", country);
            IsNotValid("FO066", country);
            IsNotValid("FO085", country);
            IsNotValid("FO099", country);

            IsNotValid("AA000", country);
            IsNotValid("AS044", country);
            IsNotValid("BJ182", country);
            IsNotValid("CD237", country);
            IsNotValid("DE343", country);
            IsNotValid("EO408", country);
            IsNotValid("FN597", country);
            IsNotValid("GF682", country);
            IsNotValid("HL711", country);
            IsNotValid("ID667", country);
            IsNotValid("JS852", country);
            IsNotValid("KN999", country);
            IsNotValid("LO000", country);
            IsNotValid("ME044", country);
            IsNotValid("NN182", country);
        }

        /// <summary>Tests patterns that should not be valid for France (FR).</summary>
        [Test]
        public void IsNotValidCustomCases_FR_All()
        {
            var country = Country.FR;

            IsNotValid("00007", country);
            IsNotValid("00043", country);
            IsNotValid("00188", country);
            IsNotValid("00237", country);
            IsNotValid("00342", country);
            IsNotValid("00401", country);
            IsNotValid("00597", country);
            IsNotValid("00682", country);
            IsNotValid("00719", country);
            IsNotValid("00665", country);
            IsNotValid("00853", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for Gabon (GA).</summary>
        [Test]
        public void IsNotValidCustomCases_GA_All()
        {
            var country = Country.GA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for United Kingdom (GB).</summary>
        [Test]
        public void IsNotValidCustomCases_GB_All()
        {
            var country = Country.GB;

            IsNotValid("311AA", country);
            IsNotValid("M113A", country);
            IsNotValid("M11A8", country);
            IsNotValid("M1BAA", country);

            IsNotValid("1338TH", country);
            IsNotValid("B338B9", country);

            IsNotValid("CRABXH", country);
            IsNotValid("CR26X9", country);

            IsNotValid("DN55PPT", country);
            IsNotValid("D1551PT", country);

            IsNotValid("WWA1HQ", country);
            IsNotValid("W1A123", country);

            IsNotValid("EC1A112", country);
            IsNotValid("EC1816B", country);
        }

        /// <summary>Tests patterns that should not be valid for Georgia (GE).</summary>
        [Test]
        public void IsNotValidCustomCases_GE_All()
        {
            var country = Country.GE;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for French Guiana (GF).</summary>
        [Test]
        public void IsNotValidCustomCases_GF_All()
        {
            var country = Country.GF;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);

        }

        /// <summary>Tests patterns that should not be valid for Guernsey (GG).</summary>
        [Test]
        public void IsNotValidCustomCases_GG_All()
        {
            var country = Country.GG;

            IsNotValid("336R", country);
            IsNotValid("044W", country);
            IsNotValid("678S", country);
            IsNotValid("975P", country);
            IsNotValid("650K", country);
            IsNotValid("988L", country);
            IsNotValid("395J", country);

            IsNotValid("5502M", country);
            IsNotValid("1832D", country);
            IsNotValid("9999J", country);
            IsNotValid("0000D", country);
            IsNotValid("2325D", country);
            IsNotValid("3436R", country);
            IsNotValid("0044W", country);

            IsNotValid("GG336R", country);
            IsNotValid("GG044W", country);
            IsNotValid("GG678S", country);
            IsNotValid("GG975P", country);
            IsNotValid("GG650K", country);
            IsNotValid("GG988L", country);
            IsNotValid("GG395J", country);

            IsNotValid("GG5502M", country);
            IsNotValid("GG1832D", country);
            IsNotValid("GG9999J", country);
            IsNotValid("GG0000D", country);
            IsNotValid("GG2325D", country);
            IsNotValid("GG3436R", country);
            IsNotValid("GG0044W", country);

            IsNotValid("GF88LS", country);
            IsNotValid("HL95JD", country);
            IsNotValid("ID02MO", country);
            IsNotValid("JS32DF", country);
            IsNotValid("KN99JS", country);
            IsNotValid("LO00DF", country);
            IsNotValid("ME25DS", country);

            IsNotValid("AA000DF", country);
            IsNotValid("AS325DS", country);
            IsNotValid("BJ436RF", country);
            IsNotValid("CD044WK", country);
            IsNotValid("DE478SD", country);
            IsNotValid("EO475PJ", country);
            IsNotValid("FN450KF", country);
        }

        /// <summary>Tests patterns that should not be valid for Gibraltar (GI).</summary>
        [Test]
        public void IsNotValidCustomCases_GI_All()
        {
            var country = Country.GI;

            IsNotValid("DN123AA", country);
            IsNotValid("GX123BB", country);
        }

        /// <summary>Tests patterns that should not be valid for Greenland (GL).</summary>
        [Test]
        public void IsNotValidCustomCases_GL_All()
        {
            var country = Country.GL;

            IsNotValid("5502", country);
            IsNotValid("1832", country);
            IsNotValid("9999", country);
            IsNotValid("0000", country);
            IsNotValid("2325", country);
            IsNotValid("3136", country);
            IsNotValid("3236", country);
            IsNotValid("3436", country);
            IsNotValid("3436", country);
            IsNotValid("3567", country);
            IsNotValid("0044", country);

            IsNotValid("GL3365", country);
            IsNotValid("GL0448", country);
            IsNotValid("GL6789", country);
            IsNotValid("GL9750", country);
            IsNotValid("GL6503", country);
            IsNotValid("GL9881", country);
            IsNotValid("GL3852", country);

            IsNotValid("AA3900", country);
            IsNotValid("AS3935", country);
            IsNotValid("BJ3946", country);
            IsNotValid("CD3904", country);
            IsNotValid("DE3948", country);
            IsNotValid("EO3945", country);
            IsNotValid("FN3940", country);
        }

        /// <summary>Tests patterns that should not be valid for Guadeloupe (GP).</summary>
        [Test]
        public void IsNotValidCustomCases_GP_All()
        {
            var country = Country.GP;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Greece (GR).</summary>
        [Test]
        public void IsNotValidCustomCases_GR_All()
        {
            var country = Country.GR;

            IsNotValid("00072", country);
            IsNotValid("00433", country);
            IsNotValid("01885", country);
            IsNotValid("02374", country);
            IsNotValid("03421", country);
            IsNotValid("04014", country);
            IsNotValid("05957", country);
            IsNotValid("06862", country);
            IsNotValid("07159", country);
            IsNotValid("06685", country);
            IsNotValid("08593", country);
            IsNotValid("09999", country);
        }

        /// <summary>Tests patterns that should not be valid for South Georgia And The South Sandwich Islands (GS).</summary>
        [Test]
        public void IsNotValidCustomCases_GS_All()
        {
            var country = Country.GS;

            IsNotValid("SIQQ1AZ", country);
            IsNotValid("SIQQ9ZZ", country);
            IsNotValid("SIBB1ZZ", country);
            IsNotValid("DN551PT", country);
            IsNotValid("DN551PT", country);
            IsNotValid("EC1A1BB", country);
            IsNotValid("EC1A1BB", country);
        }

        /// <summary>Tests patterns that should not be valid for Guatemala (GT).</summary>
        [Test]
        public void IsNotValidCustomCases_GT_All()
        {
            var country = Country.GT;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Guam (GU).</summary>
        [Test]
        public void IsNotValidCustomCases_GU_All()
        {
            var country = Country.GU;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("96908", country);
            IsNotValid("96938", country);
            IsNotValid("99999", country);

            IsNotValid("000000000", country);
            IsNotValid("012301235", country);
            IsNotValid("123412346", country);
            IsNotValid("200020004", country);
            IsNotValid("326432648", country);
            IsNotValid("409440945", country);
            IsNotValid("566456640", country);
            IsNotValid("629062908", country);
            IsNotValid("763476345", country);
            IsNotValid("675567552", country);
            IsNotValid("871887182", country);
            IsNotValid("969087182", country);
            IsNotValid("969387182", country);
            IsNotValid("999999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Guinea-Bissau (GW).</summary>
        [Test]
        public void IsNotValidCustomCases_GW_All()
        {
            var country = Country.GW;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Heard Island And Mcdonald Islands (HM).</summary>
        [Test]
        public void IsNotValidCustomCases_HM_All()
        {
            var country = Country.HM;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Honduras (HN).</summary>
        [Test]
        public void IsNotValidCustomCases_HN_All()
        {
            var country = Country.HN;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Croatia (HR).</summary>
        [Test]
        public void IsNotValidCustomCases_HR_All()
        {
            var country = Country.HR;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Haiti (HT).</summary>
        [Test]
        public void IsNotValidCustomCases_HT_All()
        {
            var country = Country.HT;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Hungary (HU).</summary>
        [Test]
        public void IsNotValidCustomCases_HU_All()
        {
            var country = Country.HU;

            IsNotValid("0007", country);
            IsNotValid("0043", country);
            IsNotValid("0188", country);
            IsNotValid("0237", country);
            IsNotValid("0342", country);
            IsNotValid("0401", country);
            IsNotValid("0595", country);
            IsNotValid("0686", country);
            IsNotValid("0715", country);
            IsNotValid("0668", country);
            IsNotValid("0859", country);
            IsNotValid("0999", country);
        }

        /// <summary>Tests patterns that should not be valid for Indonesia (ID).</summary>
        [Test]
        public void IsNotValidCustomCases_ID_All()
        {
            var country = Country.ID;

            IsNotValid("00072", country);
            IsNotValid("00433", country);
            IsNotValid("01885", country);
            IsNotValid("02374", country);
            IsNotValid("03421", country);
            IsNotValid("04014", country);
            IsNotValid("05957", country);
            IsNotValid("06862", country);
            IsNotValid("07159", country);
            IsNotValid("06685", country);
            IsNotValid("08593", country);
            IsNotValid("09999", country);
        }

        /// <summary>Tests patterns that should not be valid for Israel (IL).</summary>
        [Test]
        public void IsNotValidCustomCases_IL_All()
        {
            var country = Country.IL;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Isle Of Man (IM).</summary>
        [Test]
        public void IsNotValidCustomCases_IM_All()
        {
            var country = Country.IM;

            IsNotValid("IMAA00DF", country);
            IsNotValid("IMAS04DS", country);
            IsNotValid("IMBJ18RF", country);
            IsNotValid("IMCD23WK", country);
            IsNotValid("IMDE34SD", country);
            IsNotValid("IMEO40PJ", country);
            IsNotValid("IMFN59KF", country);
            IsNotValid("IMGF68LS", country);
            IsNotValid("IMHL71JD", country);
            IsNotValid("IMID66MO", country);
            IsNotValid("IMJS85DF", country);
            IsNotValid("IMKN99JS", country);
            IsNotValid("IMLO00DF", country);

            IsNotValid("AA000DF", country);
            IsNotValid("AS014DS", country);
            IsNotValid("BJ128RF", country);
            IsNotValid("CD203WK", country);
            IsNotValid("DE324SD", country);
            IsNotValid("EO400PJ", country);
            IsNotValid("FN569KF", country);
            IsNotValid("GF628LS", country);
            IsNotValid("HL761JD", country);
            IsNotValid("ID676MO", country);
            IsNotValid("JS875DF", country);
            IsNotValid("KN999JS", country);
            IsNotValid("LO000DF", country);
        }

        /// <summary>Tests patterns that should not be valid for India (IN).</summary>
        [Test]
        public void IsNotValidCustomCases_IN_All()
        {
            var country = Country.IN;

            IsNotValid("00000", country);
            IsNotValid("00149", country);
            IsNotValid("01283", country);
            IsNotValid("02035", country);
            IsNotValid("03244", country);
            IsNotValid("04002", country);
            IsNotValid("05698", country);
            IsNotValid("06284", country);
            IsNotValid("07613", country);
            IsNotValid("06761", country);
            IsNotValid("08753", country);
            IsNotValid("09999", country);
        }

        /// <summary>Tests patterns that should not be valid for British Indian Ocean Territory (IO).</summary>
        [Test]
        public void IsNotValidCustomCases_IO_All()
        {
            var country = Country.IO;

            IsNotValid("AADF0DF", country);
            IsNotValid("ASDS0DS", country);
            IsNotValid("BJRF1RF", country);
            IsNotValid("CDWK2WK", country);
            IsNotValid("DESD3SD", country);
            IsNotValid("EOPJ4PJ", country);
            IsNotValid("FNKF5KF", country);
            IsNotValid("GFLS6LS", country);
            IsNotValid("HLJD7JD", country);
            IsNotValid("IDMO6MO", country);
            IsNotValid("JSDF8DF", country);
            IsNotValid("KNJS9JS", country);
        }

        /// <summary>Tests patterns that should not be valid for Iraq (IQ).</summary>
        [Test]
        public void IsNotValidCustomCases_IQ_All()
        {
            var country = Country.IQ;

            IsNotValid("00000", country);
            IsNotValid("20004", country);
            IsNotValid("76345", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Iran (IR).</summary>
        [Test]
        public void IsNotValidCustomCases_IR_All()
        {
            var country = Country.IR;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Iceland (IS).</summary>
        [Test]
        public void IsNotValidCustomCases_IS_All()
        {
            var country = Country.IS;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Italy (IT).</summary>
        [Test]
        public void IsNotValidCustomCases_IT_All()
        {
            var country = Country.IT;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Jersey (JE).</summary>
        [Test]
        public void IsNotValidCustomCases_JE_All()
        {
            var country = Country.JE;

            IsNotValid("0", country);

            IsNotValid("A00S", country);
            IsNotValid("G25S", country);
            IsNotValid("D36F", country);
            IsNotValid("D44S", country);
            IsNotValid("R78F", country);
            IsNotValid("W75K", country);
            IsNotValid("5D0S", country);
            IsNotValid("8J8P", country);
            IsNotValid("9F5K", country);
            IsNotValid("0LS2", country);
            IsNotValid("3JD2", country);
            IsNotValid("9MO9", country);
            IsNotValid("0AS0", country);
            IsNotValid("G042S", country);
            IsNotValid("D153F", country);
            IsNotValid("D274S", country);
            IsNotValid("3R37F", country);
            IsNotValid("4W77K", country);
            IsNotValid("5S35D", country);
            IsNotValid("66PJ8", country);
            IsNotValid("74KF9", country);
            IsNotValid("6S80L", country);
            IsNotValid("8D93J", country);
            IsNotValid("9O99M", country);
        }

        /// <summary>Tests patterns that should not be valid for Jordan (JO).</summary>
        [Test]
        public void IsNotValidCustomCases_JO_All()
        {
            var country = Country.JO;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Japan (JP).</summary>
        [Test]
        public void IsNotValidCustomCases_JP_All()
        {
            var country = Country.JP;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Kyrgyzstan (KG).</summary>
        [Test]
        public void IsNotValidCustomCases_KG_All()
        {
            var country = Country.KG;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Cambodia (KH).</summary>
        [Test]
        public void IsNotValidCustomCases_KH_All()
        {
            var country = Country.KH;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Korea (KR).</summary>
        [Test]
        public void IsNotValidCustomCases_KR_All()
        {
            var country = Country.KR;

            IsNotValid("000000", country);
            IsNotValid("023456", country);
            IsNotValid("012823", country);
            IsNotValid("020375", country);
            IsNotValid("032434", country);
            IsNotValid("040082", country);
            IsNotValid("056978", country);
            IsNotValid("862824", country);
            IsNotValid("861134", country);
            IsNotValid("876718", country);
            IsNotValid("975239", country);
            IsNotValid("999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Cayman Islands (KY).</summary>
        [Test]
        public void IsNotValidCustomCases_KY_All()
        {
            var country = Country.KY;

            IsNotValid("AA00000", country);
            IsNotValid("AS01449", country);
            IsNotValid("BJ12823", country);
            IsNotValid("CD20375", country);
            IsNotValid("DE32434", country);
            IsNotValid("EO40082", country);
            IsNotValid("FN56978", country);
            IsNotValid("GF62824", country);
            IsNotValid("HL76113", country);
            IsNotValid("ID67671", country);
            IsNotValid("JS87523", country);
            IsNotValid("KN99999", country);
            IsNotValid("LO00000", country);
            IsNotValid("ME01449", country);
            IsNotValid("NN12823", country);
            IsNotValid("OL20375", country);
            IsNotValid("PS32434", country);
            IsNotValid("QD40082", country);
            IsNotValid("RN56978", country);
            IsNotValid("SE62824", country);
            IsNotValid("TM76113", country);
            IsNotValid("UF67671", country);
            IsNotValid("VE87523", country);
            IsNotValid("WL99999", country);
            IsNotValid("XM00000", country);
            IsNotValid("YE01449", country);
            IsNotValid("ZL12823", country);
            IsNotValid("ZZ20375", country);
        }

        /// <summary>Tests patterns that should not be valid for Kazakhstan (KZ).</summary>
        [Test]
        public void IsNotValidCustomCases_KZ_All()
        {
            var country = Country.KZ;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Lao People'S Democratic Republic (LA).</summary>
        [Test]
        public void IsNotValidCustomCases_LA_All()
        {
            var country = Country.LA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Lebanon (LB).</summary>
        [Test]
        public void IsNotValidCustomCases_LB_All()
        {
            var country = Country.LB;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Liechtenstein (LI).</summary>
        [Test]
        public void IsNotValidCustomCases_LI_All()
        {
            var country = Country.LI;

            IsNotValid("0000", country);
            IsNotValid("0144", country);
            IsNotValid("1282", country);
            IsNotValid("2037", country);
            IsNotValid("3243", country);
            IsNotValid("4008", country);
            IsNotValid("5697", country);
            IsNotValid("6282", country);
            IsNotValid("7611", country);
            IsNotValid("6767", country);
            IsNotValid("8752", country);
            IsNotValid("9999", country);

            IsNotValid("9300", country);
            IsNotValid("9499", country);
            IsNotValid("9593", country);
            IsNotValid("9679", country);
            IsNotValid("9480", country);
            IsNotValid("9481", country);
            IsNotValid("9482", country);
            IsNotValid("9483", country);
            IsNotValid("9484", country);
            IsNotValid("9499", country);
        }

        /// <summary>Tests patterns that should not be valid for Sri Lanka (LK).</summary>
        [Test]
        public void IsNotValidCustomCases_LK_All()
        {
            var country = Country.LK;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Liberia (LR).</summary>
        [Test]
        public void IsNotValidCustomCases_LR_All()
        {
            var country = Country.LR;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Lesotho (LS).</summary>
        [Test]
        public void IsNotValidCustomCases_LS_All()
        {
            var country = Country.LS;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Lithuania (LT).</summary>
        [Test]
        public void IsNotValidCustomCases_LT_All()
        {
            var country = Country.LT;

            IsNotValid("AA00000", country);
            IsNotValid("AS01449", country);
            IsNotValid("BJ12823", country);
            IsNotValid("CD20375", country);
            IsNotValid("DE32434", country);
            IsNotValid("EO40082", country);
            IsNotValid("FN56978", country);
            IsNotValid("GF62824", country);
            IsNotValid("HL76113", country);
            IsNotValid("ID67671", country);
            IsNotValid("JS87523", country);
            IsNotValid("KN99999", country);
            IsNotValid("LO00000", country);
            IsNotValid("ME01449", country);
            IsNotValid("NN12823", country);
            IsNotValid("OL20375", country);
            IsNotValid("PS32434", country);
            IsNotValid("QD40082", country);
            IsNotValid("RN56978", country);
            IsNotValid("SE62824", country);
            IsNotValid("TM76113", country);
            IsNotValid("UF67671", country);
            IsNotValid("VE87523", country);
            IsNotValid("WL99999", country);
            IsNotValid("XM00000", country);
            IsNotValid("YE01449", country);
            IsNotValid("ZL12823", country);
            IsNotValid("ZZ20375", country);
        }

        /// <summary>Tests patterns that should not be valid for Luxembourg (LU).</summary>
        [Test]
        public void IsNotValidCustomCases_LU_All()
        {
            var country = Country.LU;

            IsNotValid("AA0000", country);
            IsNotValid("AS0144", country);
            IsNotValid("BJ1282", country);
            IsNotValid("CD2037", country);
            IsNotValid("DE3243", country);
            IsNotValid("EO4008", country);
            IsNotValid("FN5697", country);
            IsNotValid("GF6282", country);
            IsNotValid("HL7611", country);
            IsNotValid("ID6767", country);
            IsNotValid("JS8752", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0144", country);
            IsNotValid("NN1282", country);
            IsNotValid("OL2037", country);
            IsNotValid("PS3243", country);
        }

        /// <summary>Tests patterns that should not be valid for Latvia (LV).</summary>
        [Test]
        public void IsNotValidCustomCases_LV_All()
        {
            var country = Country.KY;

            IsNotValid("AA0000", country);
            IsNotValid("AS0449", country);
            IsNotValid("BJ1823", country);
            IsNotValid("CD2375", country);
            IsNotValid("DE3434", country);
            IsNotValid("EO4082", country);
            IsNotValid("FN5978", country);
            IsNotValid("GF6824", country);
            IsNotValid("HL7113", country);
            IsNotValid("ID6671", country);
            IsNotValid("JS8523", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0449", country);
            IsNotValid("NN1823", country);
            IsNotValid("OL2375", country);
            IsNotValid("PS3434", country);
            IsNotValid("QD4082", country);
            IsNotValid("RN5978", country);
            IsNotValid("SE6824", country);
            IsNotValid("TM7113", country);
            IsNotValid("UF6671", country);
            IsNotValid("VE8523", country);
            IsNotValid("WL9999", country);
            IsNotValid("XM0000", country);
            IsNotValid("YE0449", country);
            IsNotValid("ZL1823", country);
            IsNotValid("ZZ2375", country);
        }

        /// <summary>Tests patterns that should not be valid for Libya (LY).</summary>
        [Test]
        public void IsNotValidCustomCases_LY_All()
        {
            var country = Country.LY;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Morocco (MA).</summary>
        [Test]
        public void IsNotValidCustomCases_MA_All()
        {
            var country = Country.MA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Monaco (MC).</summary>
        [Test]
        public void IsNotValidCustomCases_MC_All()
        {
            var country = Country.MC;

            IsNotValid("00000", country);
            IsNotValid("04249", country);
            IsNotValid("18323", country);
            IsNotValid("23475", country);
            IsNotValid("34734", country);
            IsNotValid("40782", country);
            IsNotValid("59578", country);
            IsNotValid("68824", country);
            IsNotValid("71913", country);
            IsNotValid("66071", country);
            IsNotValid("98123", country);
            IsNotValid("98299", country);
            IsNotValid("98344", country);
            IsNotValid("98402", country);
            IsNotValid("98598", country);
            IsNotValid("98684", country);
            IsNotValid("98713", country);
            IsNotValid("98661", country);
            IsNotValid("98983", country);
            IsNotValid("98989", country);
            IsNotValid("00000", country);

            IsNotValid("MC00000", country);
            IsNotValid("MC04249", country);
            IsNotValid("MC18323", country);
            IsNotValid("MC23475", country);
            IsNotValid("MC34734", country);
            IsNotValid("MC40782", country);
            IsNotValid("MC59578", country);
            IsNotValid("MC68824", country);
            IsNotValid("MC71913", country);
            IsNotValid("MC66071", country);
            IsNotValid("MC85323", country);
            IsNotValid("MC99999", country);
            IsNotValid("MC00000", country);

            IsNotValid("AA98000", country);
            IsNotValid("AS98049", country);
            IsNotValid("BJ98023", country);
            IsNotValid("CD98075", country);
            IsNotValid("DE98034", country);
            IsNotValid("EO98082", country);
            IsNotValid("FN98078", country);
            IsNotValid("GF98024", country);
            IsNotValid("HL98013", country);
            IsNotValid("ID98071", country);
            IsNotValid("JS98023", country);
            IsNotValid("KN98099", country);
            IsNotValid("LO98000", country);
            IsNotValid("ME98049", country);
            IsNotValid("NN98023", country);
            IsNotValid("OL98075", country);
            IsNotValid("PS98034", country);
            IsNotValid("QD98082", country);
            IsNotValid("RN98078", country);
            IsNotValid("SE98024", country);
            IsNotValid("TM98013", country);
            IsNotValid("UF98071", country);
            IsNotValid("VE98023", country);
            IsNotValid("WL98099", country);
            IsNotValid("XM98000", country);
            IsNotValid("YE98049", country);
            IsNotValid("ZL98023", country);
            IsNotValid("ZZ98075", country);
        }

        /// <summary>Tests patterns that should not be valid for Moldova (MD).</summary>
        [Test]
        public void IsNotValidCustomCases_MD_All()
        {
            var country = Country.MD;

            IsNotValid("AA0000", country);
            IsNotValid("AS0144", country);
            IsNotValid("BJ1282", country);
            IsNotValid("CD2037", country);
            IsNotValid("DE3243", country);
            IsNotValid("EO4008", country);
            IsNotValid("FN5697", country);
            IsNotValid("GF6282", country);
            IsNotValid("HL7611", country);
            IsNotValid("ID6767", country);
            IsNotValid("JS8752", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0144", country);
            IsNotValid("NN1282", country);
            IsNotValid("OL2037", country);
            IsNotValid("PS3243", country);
        }

        /// <summary>Tests patterns that should not be valid for Montenegro (ME).</summary>
        [Test]
        public void IsNotValidCustomCases_ME_All()
        {
            var country = Country.MD;

            IsNotValid("00000", country);
            IsNotValid("01449", country);
            IsNotValid("12823", country);
            IsNotValid("20375", country);
            IsNotValid("32434", country);
            IsNotValid("40082", country);
            IsNotValid("56978", country);
            IsNotValid("62824", country);
            IsNotValid("76113", country);
            IsNotValid("67671", country);
            IsNotValid("87523", country);
            IsNotValid("99999", country);

            IsNotValid("80000", country);
            IsNotValid("80149", country);
            IsNotValid("82035", country);
            IsNotValid("83244", country);
            IsNotValid("86284", country);
            IsNotValid("87613", country);
            IsNotValid("86761", country);
            IsNotValid("88753", country);
            IsNotValid("89999", country);
        }

        /// <summary>Tests patterns that should not be valid for Saint Martin (MF).</summary>
        [Test]
        public void IsNotValidCustomCases_MF_All()
        {
            var country = Country.MF;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Madagascar (MG).</summary>
        [Test]
        public void IsNotValidCustomCases_MG_All()
        {
            var country = Country.MG;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Marshall Islands (MH).</summary>
        [Test]
        public void IsNotValidCustomCases_MH_All()
        {
            var country = Country.MH;

            IsNotValid("06000", country);
            IsNotValid("00425", country);
            IsNotValid("11836", country);
            IsNotValid("22304", country);
            IsNotValid("33468", country);
            IsNotValid("44095", country);
            IsNotValid("55960", country);
            IsNotValid("66898", country);
            IsNotValid("77135", country);
            IsNotValid("66652", country);
            IsNotValid("88512", country);
            IsNotValid("99999", country);
            IsNotValid("96932", country);
            IsNotValid("96951", country);
            IsNotValid("96989", country);
            IsNotValid("00000", country);

            IsNotValid("000000000", country);
            IsNotValid("012345678", country);
            IsNotValid("128253436", country);
            IsNotValid("203770044", country);
            IsNotValid("324336478", country);
            IsNotValid("400879475", country);
            IsNotValid("569736450", country);
            IsNotValid("628269088", country);
            IsNotValid("761143495", country);
            IsNotValid("676785502", country);
            IsNotValid("875291832", country);
            IsNotValid("999999999", country);
            IsNotValid("000000000", country);
        }

        /// <summary>Tests patterns that should not be valid for Macedonia (MK).</summary>
        [Test]
        public void IsNotValidCustomCases_MK_All()
        {
            var country = Country.MK;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Myanmar (MM).</summary>
        [Test]
        public void IsNotValidCustomCases_MM_All()
        {
            var country = Country.MM;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Mongolia (MN).</summary>
        [Test]
        public void IsNotValidCustomCases_MN_All()
        {
            var country = Country.MN;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Northern Mariana Islands (MP).</summary>
        [Test]
        public void IsNotValidCustomCases_MP_All()
        {
            var country = Country.MP;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
            IsNotValid("96949", country);
            IsNotValid("96953", country);
            IsNotValid("96954", country);
            IsNotValid("000000000", country);
            IsNotValid("012354423", country);
            IsNotValid("123462534", country);
            IsNotValid("200047700", country);
            IsNotValid("326483364", country);
            IsNotValid("409458794", country);
            IsNotValid("566407364", country);
            IsNotValid("629082690", country);
            IsNotValid("763451434", country);
            IsNotValid("675527855", country);
            IsNotValid("871822918", country);
            IsNotValid("969496831", country);
            IsNotValid("969535348", country);
            IsNotValid("969545607", country);
            IsNotValid("999999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Martinique (MQ).</summary>
        [Test]
        public void IsNotValidCustomCases_MQ_All()
        {
            var country = Country.MQ;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Malta (MT).</summary>
        [Test]
        public void IsNotValidCustomCases_MT_All()
        {
            var country = Country.MT;

            IsNotValid("AA00000", country);
            IsNotValid("ASD01D2", country);
        }

        /// <summary>Tests patterns that should not be valid for Mexico (MX).</summary>
        [Test]
        public void IsNotValidCustomCases_MX_All()
        {
            var country = Country.MX;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Malaysia (MY).</summary>
        [Test]
        public void IsNotValidCustomCases_MY_All()
        {
            var country = Country.MY;

            IsNotValid("00000", country);
            IsNotValid("00035", country);
            IsNotValid("00146", country);
            IsNotValid("00204", country);
            IsNotValid("00348", country);
            IsNotValid("00445", country);
            IsNotValid("00540", country);
            IsNotValid("00608", country);
            IsNotValid("00745", country);
            IsNotValid("00652", country);
            IsNotValid("00882", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for Mozambique (MZ).</summary>
        [Test]
        public void IsNotValidCustomCases_MZ_All()
        {
            var country = Country.MZ;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Namibia (NA).</summary>
        [Test]
        public void IsNotValidCustomCases_NA_All()
        {
            var country = Country.NA;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("89999", country);
            IsNotValid("93000", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for New Caledonia (NC).</summary>
        [Test]
        public void IsNotValidCustomCases_NC_All()
        {
            var country = Country.NC;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);

            IsNotValid("98600", country);
            IsNotValid("98535", country);
            IsNotValid("98346", country);
            IsNotValid("98204", country);
            IsNotValid("98648", country);
            IsNotValid("98545", country);
            IsNotValid("98140", country);
            IsNotValid("98108", country);
            IsNotValid("99045", country);
            IsNotValid("99052", country);
            IsNotValid("97982", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Niger (NE).</summary>
        [Test]
        public void IsNotValidCustomCases_NE_All()
        {
            var country = Country.NE;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Norfolk Island (NF).</summary>
        [Test]
        public void IsNotValidCustomCases_NF_All()
        {
            var country = Country.NF;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Nigeria (NG).</summary>
        [Test]
        public void IsNotValidCustomCases_NG_All()
        {
            var country = Country.NG;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Nicaragua (NI).</summary>
        [Test]
        public void IsNotValidCustomCases_NI_All()
        {
            var country = Country.NI;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Netherlands (NL).</summary>
        [Test]
        public void IsNotValidCustomCases_NL_All()
        {
            var country = Country.NL;

            IsNotValid("0000DF", country);
            IsNotValid("0125DS", country);
            IsNotValid("3278SA", country);
            IsNotValid("8732SD", country);
            IsNotValid("9999SS", country);
        }

        /// <summary>Tests patterns that should not be valid for Norway (NO).</summary>
        [Test]
        public void IsNotValidCustomCases_NO_All()
        {
            var country = Country.NO;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Nepal (NP).</summary>
        [Test]
        public void IsNotValidCustomCases_NP_All()
        {
            var country = Country.NP;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for New Zealand (NZ).</summary>
        [Test]
        public void IsNotValidCustomCases_NZ_All()
        {
            var country = Country.NZ;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Oman (OM).</summary>
        [Test]
        public void IsNotValidCustomCases_OM_All()
        {
            var country = Country.OM;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Panama (PA).</summary>
        [Test]
        public void IsNotValidCustomCases_PA_All()
        {
            var country = Country.PA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Peru (PE).</summary>
        [Test]
        public void IsNotValidCustomCases_PE_All()
        {
            var country = Country.PE;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for French Polynesia (PF).</summary>
        [Test]
        public void IsNotValidCustomCases_PF_All()
        {
            var country = Country.PF;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("98524", country);
            IsNotValid("98600", country);
            IsNotValid("98805", country);
            IsNotValid("98916", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Papua New Guinea (PG).</summary>
        [Test]
        public void IsNotValidCustomCases_PG_All()
        {
            var country = Country.PG;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Philippines (PH).</summary>
        [Test]
        public void IsNotValidCustomCases_PH_All()
        {
            var country = Country.PH;

            IsNotValid("0000", country);
            IsNotValid("0003", country);
            IsNotValid("0014", country);
            IsNotValid("0020", country);
            IsNotValid("0034", country);
            IsNotValid("0044", country);
            IsNotValid("0054", country);
            IsNotValid("0060", country);
            IsNotValid("0074", country);
            IsNotValid("0065", country);
            IsNotValid("0088", country);
            IsNotValid("0099", country);
        }

        /// <summary>Tests patterns that should not be valid for Pakistan (PK).</summary>
        [Test]
        public void IsNotValidCustomCases_PK_All()
        {
            var country = Country.PK;

            IsNotValid("00000", country);
            IsNotValid("00125", country);
            IsNotValid("01236", country);
            IsNotValid("02004", country);
            IsNotValid("03268", country);
            IsNotValid("04095", country);
            IsNotValid("05660", country);
            IsNotValid("06298", country);
            IsNotValid("07635", country);
            IsNotValid("06752", country);
            IsNotValid("08712", country);
            IsNotValid("09854", country);
            IsNotValid("09860", country);
            IsNotValid("09885", country);
            IsNotValid("09896", country);
            IsNotValid("09999", country);
        }

        /// <summary>Tests patterns that should not be valid for Poland (PL).</summary>
        [Test]
        public void IsNotValidCustomCases_PL_All()
        {
            var country = Country.PL;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Saint Pierre And Miquelon (PM).</summary>
        [Test]
        public void IsNotValidCustomCases_PM_All()
        {
            var country = Country.PM;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("97000", country);
            IsNotValid("97101", country);
            IsNotValid("97212", country);
            IsNotValid("97320", country);
            IsNotValid("97432", country);
            IsNotValid("97640", country);
            IsNotValid("97756", country);
            IsNotValid("97862", country);
            IsNotValid("97976", country);
            IsNotValid("99999", country);
            IsNotValid("97502", country);
            IsNotValid("97513", country);
            IsNotValid("97520", country);
            IsNotValid("97536", country);
            IsNotValid("97549", country);
            IsNotValid("97556", country);
            IsNotValid("97569", country);
            IsNotValid("97573", country);
            IsNotValid("97565", country);
            IsNotValid("97581", country);
            IsNotValid("97599", country);
        }

        /// <summary>Tests patterns that should not be valid for Pitcairn (PN).</summary>
        [Test]
        public void IsNotValidCustomCases_PN_All()
        {
            var country = Country.PN;

            IsNotValid("PCRN2ZZ", country);
            IsNotValid("AADF0FD", country);
            IsNotValid("ASDS0KD", country);
            IsNotValid("BJRF1DR", country);
            IsNotValid("CDWK2JW", country);
            IsNotValid("DESD3FS", country);
            IsNotValid("EOPJ4SP", country);
            IsNotValid("FNKF5DK", country);
            IsNotValid("GFLS6OL", country);
            IsNotValid("HLJD7FJ", country);
            IsNotValid("IDMO6SM", country);
            IsNotValid("JSDF8FD", country);
            IsNotValid("KNJS9SJ", country);
            IsNotValid("LODF0FD", country);
            IsNotValid("MEDS0KD", country);
            IsNotValid("NNRF1RF", country);
            IsNotValid("OLWK2WS", country);
            IsNotValid("PSSD3SF", country);
            IsNotValid("QDPJ4PK", country);
            IsNotValid("RNKF5KD", country);
            IsNotValid("SELS6LJ", country);
            IsNotValid("TMJD7JF", country);
            IsNotValid("UFMO6MS", country);
            IsNotValid("VEDF8DD", country);
            IsNotValid("WLJS9JO", country);
            IsNotValid("XMDF0DF", country);
            IsNotValid("YEDS0DS", country);
            IsNotValid("ZLRF1RF", country);
            IsNotValid("ZZWK2WS", country);
        }

        /// <summary>Tests patterns that should not be valid for Puerto Rico (PR).</summary>
        [Test]
        public void IsNotValidCustomCases_PR_All()
        {
            var country = Country.PR;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Palestinian Territory (PS).</summary>
        [Test]
        public void IsNotValidCustomCases_PS_All()
        {
            var country = Country.PS;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Portugal (PT).</summary>
        [Test]
        public void IsNotValidCustomCases_PT_All()
        {
            var country = Country.PT;

            IsNotValid("0000000", country);
            IsNotValid("0014494", country);
            IsNotValid("0123456", country);
            IsNotValid("0203757", country);
            IsNotValid("0324343", country);
            IsNotValid("0400827", country);
            IsNotValid("0569783", country);
            IsNotValid("0628246", country);
            IsNotValid("0761134", country);
            IsNotValid("0676718", country);
            IsNotValid("0875239", country);
            IsNotValid("0999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Palau (PW).</summary>
        [Test]
        public void IsNotValidCustomCases_PW_All()
        {
            var country = Country.PW;

            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("96939", country);
            IsNotValid("96941", country);
            IsNotValid("96948", country);
            IsNotValid("96952", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Paraguay (PY).</summary>
        [Test]
        public void IsNotValidCustomCases_PY_All()
        {
            var country = Country.PY;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Réunion (RE).</summary>
        [Test]
        public void IsNotValidCustomCases_RE_All()
        {
            var country = Country.RE;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("97000", country);
            IsNotValid("97102", country);
            IsNotValid("97213", country);
            IsNotValid("97320", country);
            IsNotValid("97536", country);
            IsNotValid("97649", country);
            IsNotValid("97756", country);
            IsNotValid("97869", country);
            IsNotValid("97973", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Romania (RO).</summary>
        [Test]
        public void IsNotValidCustomCases_RO_All()
        {
            var country = Country.RO;

            IsNotValid("00000", country);
            IsNotValid("00012", country);
            IsNotValid("00123", country);
            IsNotValid("00200", country);
            IsNotValid("00326", country);
            IsNotValid("00409", country);
            IsNotValid("00566", country);
            IsNotValid("00629", country);
            IsNotValid("00763", country);
            IsNotValid("00675", country);
            IsNotValid("00871", country);
            IsNotValid("00970", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for Serbia (RS).</summary>
        [Test]
        public void IsNotValidCustomCases_RS_All()
        {
            var country = Country.RS;

            IsNotValid("000000", country);
            IsNotValid("012345", country);
            IsNotValid("400827", country);
            IsNotValid("569783", country);
            IsNotValid("628246", country);
            IsNotValid("761134", country);
            IsNotValid("676718", country);
            IsNotValid("875239", country);
            IsNotValid("999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Russian Federation (RU).</summary>
        [Test]
        public void IsNotValidCustomCases_RU_All()
        {
            var country = Country.RU;

            IsNotValid("0000000", country);
            IsNotValid("0012345", country);
            IsNotValid("0128235", country);
            IsNotValid("0203757", country);
            IsNotValid("0324343", country);
            IsNotValid("0400827", country);
            IsNotValid("0569783", country);
            IsNotValid("0628246", country);
            IsNotValid("0761134", country);
            IsNotValid("0676718", country);
            IsNotValid("0875239", country);
            IsNotValid("0999999", country);
        }

        /// <summary>Tests patterns that should not be valid for Saudi Arabia (SA).</summary>
        [Test]
        public void IsNotValidCustomCases_SA_All()
        {
            var country = Country.SA;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Sudan (SD).</summary>
        [Test]
        public void IsNotValidCustomCases_SD_All()
        {
            var country = Country.SD;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Sweden (SE).</summary>
        [Test]
        public void IsNotValidCustomCases_SE_All()
        {
            var country = Country.SE;

            IsNotValid("000000", country);
            IsNotValid("001235", country);
            IsNotValid("012825", country);
            IsNotValid("020377", country);
            IsNotValid("032433", country);
            IsNotValid("040087", country);
            IsNotValid("056973", country);
            IsNotValid("062826", country);
            IsNotValid("076114", country);
            IsNotValid("067678", country);
            IsNotValid("087529", country);
            IsNotValid("099999", country);
        }

        /// <summary>Tests patterns that should not be valid for Singapore (SG).</summary>
        [Test]
        public void IsNotValidCustomCases_SG_All()
        {
            var country = Country.SG;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Saint Helena (SH).</summary>
        [Test]
        public void IsNotValidCustomCases_SH_All()
        {
            var country = Country.SH;

            IsNotValid("SAHL1ZZ", country);
            IsNotValid("STBL1AZ", country);
            IsNotValid("STHL2ZZ", country);
            IsNotValid("AADF0DF", country);
            IsNotValid("ASDS2DS", country);
            IsNotValid("BJRF3RF", country);
            IsNotValid("CDWK4WK", country);
            IsNotValid("DESD7SD", country);
            IsNotValid("EOPJ7PJ", country);
            IsNotValid("FNKF5KF", country);
            IsNotValid("GFLS8LS", country);
            IsNotValid("HLJD9JD", country);
            IsNotValid("IDMO0MO", country);
            IsNotValid("JSDF3DF", country);
            IsNotValid("KNJS9JS", country);
            IsNotValid("LODF0DF", country);
            IsNotValid("MEDS2DS", country);
            IsNotValid("NNRF3RF", country);
            IsNotValid("OLWK4WK", country);
            IsNotValid("PSSD7SD", country);
            IsNotValid("QDPJ7PJ", country);
            IsNotValid("RNKF5KF", country);
            IsNotValid("SELS8LS", country);
            IsNotValid("TMJD9JD", country);
            IsNotValid("UFMO0MO", country);
            IsNotValid("VEDF3DF", country);
            IsNotValid("WLJS9JS", country);
            IsNotValid("XMDF0DF", country);
            IsNotValid("YEDS2DS", country);
            IsNotValid("ZLRF3RF", country);
            IsNotValid("ZZWK4WK", country);
        }

        /// <summary>Tests patterns that should not be valid for Slovenia (SI).</summary>
        [Test]
        public void IsNotValidCustomCases_SI_All()
        {
            var country = Country.SI;

            IsNotValid("AA0000", country);
            IsNotValid("AS0144", country);
            IsNotValid("BJ1282", country);
            IsNotValid("CD2037", country);
            IsNotValid("DE3243", country);
            IsNotValid("EO4008", country);
            IsNotValid("FN5697", country);
            IsNotValid("GF6282", country);
            IsNotValid("HL7611", country);
            IsNotValid("ID6767", country);
            IsNotValid("JS8752", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0144", country);
            IsNotValid("NN1282", country);
            IsNotValid("OL2037", country);
            IsNotValid("PS3243", country);
            IsNotValid("QD4008", country);
            IsNotValid("RN5697", country);
            IsNotValid("SE6282", country);
            IsNotValid("TM7611", country);
            IsNotValid("UF6767", country);
            IsNotValid("VE8752", country);
            IsNotValid("WL9999", country);
            IsNotValid("XM0000", country);
            IsNotValid("YE0144", country);
            IsNotValid("ZL1282", country);
            IsNotValid("ZZ2037", country);
        }

        /// <summary>Tests patterns that should not be valid for Slovakia (SK).</summary>
        [Test]
        public void IsNotValidCustomCases_SK_All()
        {
            var country = Country.SK;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for San Marino (SM).</summary>
        [Test]
        public void IsNotValidCustomCases_SM_All()
        {
            var country = Country.SM;

            IsNotValid("00000", country);
            IsNotValid("05125", country);
            IsNotValid("16285", country);
            IsNotValid("27037", country);
            IsNotValid("36243", country);
            IsNotValid("46890", country);
            IsNotValid("47797", country);
            IsNotValid("59693", country);
            IsNotValid("66286", country);
            IsNotValid("76614", country);
            IsNotValid("66768", country);
            IsNotValid("83759", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Senegal (SN).</summary>
        [Test]
        public void IsNotValidCustomCases_SN_All()
        {
            var country = Country.SN;

            IsNotValid("AA00000", country);
            IsNotValid("AB01234", country);
            IsNotValid("BJ12823", country);
            IsNotValid("CO20375", country);
            IsNotValid("DE32434", country);
            IsNotValid("EO40082", country);
            IsNotValid("FN56978", country);
            IsNotValid("GF62824", country);
            IsNotValid("HL76113", country);
            IsNotValid("ID67671", country);
            IsNotValid("JS87523", country);
            IsNotValid("KN99999", country);
            IsNotValid("LO00000", country);
            IsNotValid("ME01449", country);
            IsNotValid("NN12823", country);
            IsNotValid("OL20375", country);
            IsNotValid("PS32434", country);
            IsNotValid("QD40082", country);
            IsNotValid("RN56978", country);
            IsNotValid("SE62824", country);
            IsNotValid("TM76113", country);
            IsNotValid("UF67671", country);
            IsNotValid("VE87523", country);
            IsNotValid("WL99999", country);
            IsNotValid("XM00000", country);
            IsNotValid("YE01449", country);
            IsNotValid("ZL12823", country);
            IsNotValid("ZZ20375", country);
        }

        /// <summary>Tests patterns that should not be valid for El Salvador (SV).</summary>
        [Test]
        public void IsNotValidCustomCases_SV_All()
        {
            var country = Country.SV;

            IsNotValid("00000", country);
            IsNotValid("01001", country);
            IsNotValid("01131", country);
            IsNotValid("02131", country);
            IsNotValid("02331", country);
            IsNotValid("12234", country);
            IsNotValid("27000", country);
            IsNotValid("33248", country);
            IsNotValid("48945", country);
            IsNotValid("57640", country);
            IsNotValid("62208", country);
            IsNotValid("71645", country);
            IsNotValid("67752", country);
            IsNotValid("82782", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Swaziland (SZ).</summary>
        [Test]
        public void IsNotValidCustomCases_SZ_All()
        {
            var country = Country.SZ;

            IsNotValid("A000", country);
            IsNotValid("A014", country);
            IsNotValid("B128", country);
            IsNotValid("C203", country);
            IsNotValid("D324", country);
            IsNotValid("E400", country);
            IsNotValid("F569", country);
            IsNotValid("G628", country);
            IsNotValid("I676", country);
            IsNotValid("J875", country);
            IsNotValid("K999", country);
            IsNotValid("N128", country);
            IsNotValid("O203", country);
            IsNotValid("P324", country);
            IsNotValid("Q400", country);
            IsNotValid("R569", country);
            IsNotValid("T761", country);
            IsNotValid("U676", country);
            IsNotValid("V875", country);
            IsNotValid("W999", country);
            IsNotValid("X000", country);
            IsNotValid("Y014", country);
            IsNotValid("Z128", country);
            IsNotValid("Z999", country);

            IsNotValid("A00Z", country);
            IsNotValid("0A14", country);
            IsNotValid("1B28", country);
            IsNotValid("2C03", country);
            IsNotValid("3D24", country);
            IsNotValid("E40A", country);
            IsNotValid("F5B9", country);
            IsNotValid("G6BB", country);
            IsNotValid("H7A1", country);
            IsNotValid("I6A6", country);
            IsNotValid("875J", country);
            IsNotValid("999K", country);
            IsNotValid("000L", country);
            IsNotValid("014M", country);
            IsNotValid("128N", country);
            IsNotValid("203O", country);
            IsNotValid("324P", country);
            IsNotValid("Q4J0", country);
            IsNotValid("R5K6", country);
            IsNotValid("S6L2", country);
            IsNotValid("T7M6", country);
            IsNotValid("U6N7", country);
            IsNotValid("V8O7", country);
        }

        /// <summary>Tests patterns that should not be valid for Turks And Caicos Islands (TC).</summary>
        [Test]
        public void IsNotValidCustomCases_TC_All()
        {
            var country = Country.TC;

            IsNotValid("AKCA1ZZ", country);
            IsNotValid("TBCA1ZZ", country);
            IsNotValid("TKDA1ZZ", country);
            IsNotValid("TKCE1ZZ", country);
            IsNotValid("TKCA9ZZ", country);
            IsNotValid("TKCA1FZ", country);
            IsNotValid("TKCA1ZG", country);

            IsNotValid("AADF0DF", country);
            IsNotValid("ASDS0DS", country);
            IsNotValid("BJRF1RF", country);
            IsNotValid("CDWK2WK", country);
            IsNotValid("DESD3SD", country);
            IsNotValid("EOPJ4PJ", country);
            IsNotValid("FNKF5KF", country);
            IsNotValid("GFLS6LS", country);
            IsNotValid("HLJD7JD", country);
            IsNotValid("IDMO6MO", country);
            IsNotValid("JSDF8DF", country);
            IsNotValid("KNJS9JS", country);
            IsNotValid("LODF0DF", country);
            IsNotValid("MEDS0DS", country);
            IsNotValid("NNRF1RF", country);
            IsNotValid("OLWK2WK", country);
            IsNotValid("PSSD3SD", country);
            IsNotValid("QDPJ4PJ", country);
            IsNotValid("RNKF5KF", country);
            IsNotValid("SELS6LS", country);
            IsNotValid("TMJD7JD", country);
            IsNotValid("UFMO6MO", country);
            IsNotValid("VEDF8DF", country);
            IsNotValid("WLJS9JS", country);
            IsNotValid("XMDF0DF", country);
            IsNotValid("YEDS0DS", country);
            IsNotValid("ZLRF1RF", country);
            IsNotValid("ZZWK2WK", country);
        }

        /// <summary>Tests patterns that should not be valid for Chad (TD).</summary>
        [Test]
        public void IsNotValidCustomCases_TD_All()
        {
            var country = Country.TD;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Thailand (TH).</summary>
        [Test]
        public void IsNotValidCustomCases_TH_All()
        {
            var country = Country.TH;

            IsNotValid("00000", country);
            IsNotValid("00124", country);
            IsNotValid("01283", country);
            IsNotValid("02035", country);
            IsNotValid("03244", country);
            IsNotValid("04002", country);
            IsNotValid("05698", country);
            IsNotValid("06284", country);
            IsNotValid("07613", country);
            IsNotValid("06761", country);
            IsNotValid("08753", country);
            IsNotValid("09999", country);
        }

        /// <summary>Tests patterns that should not be valid for Tajikistan (TJ).</summary>
        [Test]
        public void IsNotValidCustomCases_TJ_All()
        {
            var country = Country.TJ;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Turkmenistan (TM).</summary>
        [Test]
        public void IsNotValidCustomCases_TM_All()
        {
            var country = Country.TM;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Tunisia (TN).</summary>
        [Test]
        public void IsNotValidCustomCases_TN_All()
        {
            var country = Country.TN;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Turkey (TR).</summary>
        [Test]
        public void IsNotValidCustomCases_TR_All()
        {
            var country = Country.TR;

            IsNotValid("00000", country);
            IsNotValid("00012", country);
            IsNotValid("00128", country);
            IsNotValid("00203", country);
            IsNotValid("00324", country);
            IsNotValid("00400", country);
            IsNotValid("00569", country);
            IsNotValid("00628", country);
            IsNotValid("00761", country);
            IsNotValid("00676", country);
            IsNotValid("00875", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for Trinidad And Tobago (TT).</summary>
        [Test]
        public void IsNotValidCustomCases_TT_All()
        {
            var country = Country.TT;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Taiwan (TW).</summary>
        [Test]
        public void IsNotValidCustomCases_TW_All()
        {
            var country = Country.TW;

            IsNotValid("000000", country);
            IsNotValid("001234", country);
            IsNotValid("012823", country);
            IsNotValid("020375", country);
            IsNotValid("032434", country);
            IsNotValid("040082", country);
            IsNotValid("056978", country);
            IsNotValid("062824", country);
            IsNotValid("076113", country);
            IsNotValid("067671", country);
            IsNotValid("087523", country);
            IsNotValid("099999", country);
        }

        /// <summary>Tests patterns that should not be valid for Ukraine (UA).</summary>
        [Test]
        public void IsNotValidCustomCases_UA_All()
        {
            var country = Country.UA;

            IsNotValid("00000", country);
            IsNotValid("00012", country);
            IsNotValid("00123", country);
            IsNotValid("00200", country);
            IsNotValid("00326", country);
            IsNotValid("00409", country);
            IsNotValid("00566", country);
            IsNotValid("00629", country);
            IsNotValid("00763", country);
            IsNotValid("00675", country);
            IsNotValid("00871", country);
            IsNotValid("00999", country);
        }

        /// <summary>Tests patterns that should not be valid for United States (US).</summary>
        [Test]
        public void IsNotValidCustomCases_US_All()
        {
            var country = Country.US;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Uruguay (UY).</summary>
        [Test]
        public void IsNotValidCustomCases_UY_All()
        {
            var country = Country.UY;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Holy See (VA).</summary>
        [Test]
        public void IsNotValidCustomCases_VA_All()
        {
            var country = Country.VA;

            IsNotValid("00000", country);
            IsNotValid("01234", country);
            IsNotValid("12823", country);
            IsNotValid("20375", country);
            IsNotValid("32434", country);
            IsNotValid("40082", country);
            IsNotValid("56978", country);
            IsNotValid("62824", country);
            IsNotValid("76113", country);
            IsNotValid("67671", country);
            IsNotValid("87523", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Saint Vincent And The Grenadines (VC).</summary>
        [Test]
        public void IsNotValidCustomCases_VC_All()
        {
            var country = Country.VC;

            IsNotValid("AA0000", country);
            IsNotValid("AS0144", country);
            IsNotValid("BJ1282", country);
            IsNotValid("CD2037", country);
            IsNotValid("DE3243", country);
            IsNotValid("EO4008", country);
            IsNotValid("FN5697", country);
            IsNotValid("GF6282", country);
            IsNotValid("HL7611", country);
            IsNotValid("ID6767", country);
            IsNotValid("JS8752", country);
            IsNotValid("KN9999", country);
            IsNotValid("LO0000", country);
            IsNotValid("ME0144", country);
            IsNotValid("NN1282", country);
            IsNotValid("OL2037", country);
            IsNotValid("PS3243", country);
            IsNotValid("QD4008", country);
            IsNotValid("RN5697", country);
            IsNotValid("SE6282", country);
            IsNotValid("TM7611", country);
            IsNotValid("UF6767", country);
            IsNotValid("VE8752", country);
            IsNotValid("WL9999", country);
            IsNotValid("XM0000", country);
            IsNotValid("YE0144", country);
            IsNotValid("ZL1282", country);
            IsNotValid("ZZ2037", country);
        }

        /// <summary>Tests patterns that should not be valid for Venezuela (VE).</summary>
        [Test]
        public void IsNotValidCustomCases_VE_All()
        {
            var country = Country.VE;

            IsNotValid("00000", country);
            IsNotValid("01223", country);
            IsNotValid("12334", country);
            IsNotValid("20400", country);
            IsNotValid("32764", country);
            IsNotValid("40794", country);
            IsNotValid("56564", country);
            IsNotValid("62890", country);
            IsNotValid("76934", country);
            IsNotValid("67055", country);
            IsNotValid("87318", country);
            IsNotValid("99999", country);

            IsNotValid("000A", country);
            IsNotValid("032A", country);
            IsNotValid("143B", country);
            IsNotValid("204C", country);
            IsNotValid("347D", country);
            IsNotValid("447E", country);
            IsNotValid("545F", country);
            IsNotValid("608G", country);
            IsNotValid("749H", country);
            IsNotValid("J650I", country);
            IsNotValid("K8832", country);
            IsNotValid("L9999", country);
            IsNotValid("M0000", country);
            IsNotValid("N0325", country);
            IsNotValid("O1436", country);
            IsNotValid("20412", country);
            IsNotValid("34787", country);
            IsNotValid("44757", country);
            IsNotValid("54505", country);
            IsNotValid("60888", country);
        }

        /// <summary>Tests patterns that should not be valid for Virgin Islands (VG).</summary>
        [Test]
        public void IsNotValidCustomCases_VG_All()
        {
            var country = Country.VG;

            IsNotValid("0123", country);
            IsNotValid("1187", country);
            IsNotValid("1199", country);
            IsNotValid("1234", country);
            IsNotValid("2000", country);
            IsNotValid("3248", country);
            IsNotValid("4945", country);
            IsNotValid("5640", country);
            IsNotValid("6208", country);
            IsNotValid("7645", country);
            IsNotValid("6752", country);
            IsNotValid("8782", country);
            IsNotValid("9999", country);

            IsNotValid("VG0123", country);
            IsNotValid("VG1187", country);
            IsNotValid("VG1199", country);
            IsNotValid("VG1234", country);
            IsNotValid("VG2000", country);
            IsNotValid("VG3248", country);
            IsNotValid("VG4945", country);
            IsNotValid("VG5640", country);
            IsNotValid("VG6208", country);
            IsNotValid("VG7645", country);
            IsNotValid("VG6752", country);
            IsNotValid("VG8782", country);
            IsNotValid("VG9999", country);
        }

        /// <summary>Tests patterns that should not be valid for Virgin Islands (VI).</summary>
        [Test]
        public void IsNotValidCustomCases_VI_All()
        {
            var country = Country.VI;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
            IsNotValid("000000000", country);
            IsNotValid("013582794", country);
            IsNotValid("124678364", country);
            IsNotValid("200424690", country);
            IsNotValid("324813434", country);
            IsNotValid("404571855", country);
            IsNotValid("564023918", country);
            IsNotValid("620899999", country);
            IsNotValid("764500000", country);
            IsNotValid("675249423", country);
            IsNotValid("878227947", country);
            IsNotValid("999999999", country);

            IsNotValid("00800", country);
            IsNotValid("00804", country);
            IsNotValid("00869", country);
            IsNotValid("00870", country);
            IsNotValid("00860", country);
            IsNotValid("00881", country);
            IsNotValid("00892", country);
            IsNotValid("008000000", country);
            IsNotValid("008041235", country);
            IsNotValid("008692908", country);
            IsNotValid("008706345", country);
            IsNotValid("008607552", country);
            IsNotValid("008817182", country);
            IsNotValid("008929999", country);
        }

        /// <summary>Tests patterns that should not be valid for Viet Nam (VN).</summary>
        [Test]
        public void IsNotValidCustomCases_VN_All()
        {
            var country = Country.VN;

            IsNotValid("0", country);
        }

        /// <summary>Tests patterns that should not be valid for Wallis And Futuna (WF).</summary>
        [Test]
        public void IsNotValidCustomCases_WF_All()
        {
            var country = Country.WF;

            IsNotValid("00000", country);
            IsNotValid("01235", country);
            IsNotValid("12346", country);
            IsNotValid("20004", country);
            IsNotValid("32648", country);
            IsNotValid("40945", country);
            IsNotValid("56640", country);
            IsNotValid("62908", country);
            IsNotValid("76345", country);
            IsNotValid("67552", country);
            IsNotValid("87182", country);
            IsNotValid("99999", country);
        }

        /// <summary>Tests patterns that should not be valid for Mayotte (YT).</summary>
        [Test]
        public void IsNotValidCustomCases_YT_All()
        {
            var country = Country.YT;

            IsNotValid("M11AA", country);
            IsNotValid("M11aA", country);
            IsNotValid("M11AA", country);
            IsNotValid("m11AA", country);
            IsNotValid("m11aa", country);

            IsNotValid("B338TH", country);
            IsNotValid("B338TH", country);

            IsNotValid("CR26XH", country);
            IsNotValid("CR26XH", country);

            IsNotValid("DN551PT", country);
            IsNotValid("DN551PT", country);

            IsNotValid("W1A1HQ", country);
            IsNotValid("W1A1HQ", country);

            IsNotValid("EC1A1BB", country);
            IsNotValid("EC1A1BB", country);
        }

        /// <summary>Tests patterns that should not be valid for South Africa (ZA).</summary>
        [Test]
        public void IsNotValidCustomCases_ZA_All()
        {
            var country = Country.ZA;

            IsNotValid("0000", country);
        }

        /// <summary>Tests patterns that should not be valid for Zambia (ZM).</summary>
        [Test]
        public void IsNotValidCustomCases_ZM_All()
        {
            var country = Country.ZM;

            IsNotValid("0", country);
        }

        #endregion

        /// <summary>Tests patterns that should not be valid for Andorra (AD).</summary>
        [Test]
        public void IsNotValid_AD_All()
        {
            IsNotValid(Country.AD, false, true, 3);
        }

        /// <summary>Tests patterns that should not be valid for Afghanistan (AF).</summary>
        [Test]
        public void IsNotValid_AF_All()
        {
            IsNotValid(Country.AF, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Anguilla (AI).</summary>
        [Test]
        public void IsNotValid_AI_All()
        {
            IsNotValid(Country.AI, false, true, 4);
        }

        /// <summary>Tests patterns that should not be valid for Albania (AL).</summary>
        [Test]
        public void IsNotValid_AL_All()
        {
            IsNotValid(Country.AL, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Armenia (AM).</summary>
        [Test]
        public void IsNotValid_AM_All()
        {
            IsNotValid(Country.AM, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Argentina (AR).</summary>
        [Test]
        public void IsNotValid_AR_All()
        {
            IsNotValid(Country.AR, true, false, 8);
        }

        /// <summary>Tests patterns that should not be valid for American Samoa (AS).</summary>
        [Test]
        public void IsNotValid_AS_All()
        {
            IsNotValid(Country.AS, false, false, 5, 9);
        }

        /// <summary>Tests patterns that should not be valid for Austria (AT).</summary>
        [Test]
        public void IsNotValid_AT_All()
        {
            IsNotValid(Country.AT, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Australia (AU).</summary>
        [Test]
        public void IsNotValid_AU_All()
        {
            IsNotValid(Country.AU, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Åland Islands (AX).</summary>
        [Test]
        public void IsNotValid_AX_All()
        {
            IsNotValid(Country.AX, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Azerbaijan (AZ).</summary>
        [Test]
        public void IsNotValid_AZ_All()
        {
            IsNotValid(Country.AZ, false, true, 4);
        }

        /// <summary>Tests patterns that should not be valid for Bosnia And Herzegovina (BA).</summary>
        [Test]
        public void IsNotValid_BA_All()
        {
            IsNotValid(Country.BA, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Barbados (BB).</summary>
        [Test]
        public void IsNotValid_BB_All()
        {
            IsNotValid(Country.BB, false, true, 5);
        }

        /// <summary>Tests patterns that should not be valid for Bangladesh (BD).</summary>
        [Test]
        public void IsNotValid_BD_All()
        {
            IsNotValid(Country.BD, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Belgium (BE).</summary>
        [Test]
        public void IsNotValid_BE_All()
        {
            IsNotValid(Country.BE, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Bulgaria (BG).</summary>
        [Test]
        public void IsNotValid_BG_All()
        {
            IsNotValid(Country.BG, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Bahrain (BH).</summary>
        [Test]
        public void IsNotValid_BH_All()
        {
            IsNotValid(Country.BH, false, false, 3, 4);
        }

        /// <summary>Tests patterns that should not be valid for Saint Barthélemy (BL).</summary>
        [Test]
        public void IsNotValid_BL_All()
        {
            IsNotValid(Country.BL, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Bermuda (BM).</summary>
        [Test]
        public void IsNotValid_BM_All()
        {
            IsNotValid(Country.BM, true, false, 2, 4);
        }

        /// <summary>Tests patterns that should not be valid for Brunei Darussalam (BN).</summary>
        [Test]
        public void IsNotValid_BN_All()
        {
            IsNotValid(Country.BN, true, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Bolivia (BO).</summary>
        [Test]
        public void IsNotValid_BO_All()
        {
            IsNotValid(Country.BO, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Brazil (BR).</summary>
        [Test]
        public void IsNotValid_BR_All()
        {
            IsNotValid(Country.BR, false, false, 8);
        }

        /// <summary>Tests patterns that should not be valid for Bhutan (BT).</summary>
        [Test]
        public void IsNotValid_BT_All()
        {
            IsNotValid(Country.BT, false, false, 3);
        }

        /// <summary>Tests patterns that should not be valid for Belarus (BY).</summary>
        [Test]
        public void IsNotValid_BY_All()
        {
            IsNotValid(Country.BY, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Canada (CA).</summary>
        [Test]
        public void IsNotValid_CA_All()
        {
            IsNotValid(Country.CA, true, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Cocos (CC).</summary>
        [Test]
        public void IsNotValid_CC_All()
        {
            IsNotValid(Country.CC, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Switzerland (CH).</summary>
        [Test]
        public void IsNotValid_CH_All()
        {
            IsNotValid(Country.CH, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Chile (CL).</summary>
        [Test]
        public void IsNotValid_CL_All()
        {
            IsNotValid(Country.CL, false, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for China (CN).</summary>
        [Test]
        public void IsNotValid_CN_All()
        {
            IsNotValid(Country.CN, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Colombia (CO).</summary>
        [Test]
        public void IsNotValid_CO_All()
        {
            IsNotValid(Country.CO, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Costa Rica (CR).</summary>
        [Test]
        public void IsNotValid_CR_All()
        {
            IsNotValid(Country.CR, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Cuba (CU).</summary>
        [Test]
        public void IsNotValid_CU_All()
        {
            IsNotValid(Country.CU, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Cape Verde (CV).</summary>
        [Test]
        public void IsNotValid_CV_All()
        {
            IsNotValid(Country.CV, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Christmas Island (CX).</summary>
        [Test]
        public void IsNotValid_CX_All()
        {
            IsNotValid(Country.CX, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Cyprus (CY).</summary>
        [Test]
        public void IsNotValid_CY_All()
        {
            IsNotValid(Country.CY, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Czech Republic (CZ).</summary>
        [Test]
        public void IsNotValid_CZ_All()
        {
            IsNotValid(Country.CZ, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Germany (DE).</summary>
        [Test]
        public void IsNotValid_DE_All()
        {
            IsNotValid(Country.DE, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Denmark (DK).</summary>
        [Test]
        public void IsNotValid_DK_All()
        {
            IsNotValid(Country.DK, false, true, 4);
        }

        /// <summary>Tests patterns that should not be valid for Algeria (DZ).</summary>
        [Test]
        public void IsNotValid_DZ_All()
        {
            IsNotValid(Country.DZ, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Ecuador (EC).</summary>
        [Test]
        public void IsNotValid_EC_All()
        {
            IsNotValid(Country.EC, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Estonia (EE).</summary>
        [Test]
        public void IsNotValid_EE_All()
        {
            IsNotValid(Country.EE, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Egypt (EG).</summary>
        [Test]
        public void IsNotValid_EG_All()
        {
            IsNotValid(Country.EG, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Spain (ES).</summary>
        [Test]
        public void IsNotValid_ES_All()
        {
            IsNotValid(Country.ES, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Ethiopia (ET).</summary>
        [Test]
        public void IsNotValid_ET_All()
        {
            IsNotValid(Country.ET, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Finland (FI).</summary>
        [Test]
        public void IsNotValid_FI_All()
        {
            IsNotValid(Country.FI, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Falkland Islands (FK).</summary>
        [Test]
        public void IsNotValid_FK_All()
        {
            IsNotValid(Country.FK, true, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Micronesia (FM).</summary>
        [Test]
        public void IsNotValid_FM_All()
        {
            IsNotValid(Country.FM, false, false, 5, 9);
        }

        /// <summary>Tests patterns that should not be valid for Faroe Islands (FO).</summary>
        [Test]
        public void IsNotValid_FO_All()
        {
            IsNotValid(Country.FO, false, true, 3);
        }

        /// <summary>Tests patterns that should not be valid for France (FR).</summary>
        [Test]
        public void IsNotValid_FR_All()
        {
            IsNotValid(Country.FR, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Gabon (GA).</summary>
        [Test]
        public void IsNotValid_GA_All()
        {
            IsNotValid(Country.GA, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for United Kingdom (GB).</summary>
        [Test]
        public void IsNotValid_GB_All()
        {
            IsNotValid(Country.GB, true, false, 5, 6, 7);
        }

        /// <summary>Tests patterns that should not be valid for Georgia (GE).</summary>
        [Test]
        public void IsNotValid_GE_All()
        {
            IsNotValid(Country.GE, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for French Guiana (GF).</summary>
        [Test]
        public void IsNotValid_GF_All()
        {
            IsNotValid(Country.GF, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Guernsey (GG).</summary>
        [Test]
        public void IsNotValid_GG_All()
        {
            IsNotValid(Country.GG, true, true, 4, 5);
        }

        /// <summary>Tests patterns that should not be valid for Gibraltar (GI).</summary>
        [Test]
        public void IsNotValid_GI_All()
        {
            IsNotValid(Country.GI, true, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Greenland (GL).</summary>
        [Test]
        public void IsNotValid_GL_All()
        {
            IsNotValid(Country.GL, false, true, 4);
        }

        /// <summary>Tests patterns that should not be valid for Guadeloupe (GP).</summary>
        [Test]
        public void IsNotValid_GP_All()
        {
            IsNotValid(Country.GP, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Greece (GR).</summary>
        [Test]
        public void IsNotValid_GR_All()
        {
            IsNotValid(Country.GR, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for South Georgia And The South Sandwich Islands (GS).</summary>
        [Test]
        public void IsNotValid_GS_All()
        {
            IsNotValid(Country.GS, true, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Guatemala (GT).</summary>
        [Test]
        public void IsNotValid_GT_All()
        {
            IsNotValid(Country.GT, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Guam (GU).</summary>
        [Test]
        public void IsNotValid_GU_All()
        {
            IsNotValid(Country.GU, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Guinea-Bissau (GW).</summary>
        [Test]
        public void IsNotValid_GW_All()
        {
            IsNotValid(Country.GW, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Heard Island And Mcdonald Islands (HM).</summary>
        [Test]
        public void IsNotValid_HM_All()
        {
            IsNotValid(Country.HM, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Honduras (HN).</summary>
        [Test]
        public void IsNotValid_HN_All()
        {
            IsNotValid(Country.HN, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Croatia (HR).</summary>
        [Test]
        public void IsNotValid_HR_All()
        {
            IsNotValid(Country.HR, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Haiti (HT).</summary>
        [Test]
        public void IsNotValid_HT_All()
        {
            IsNotValid(Country.HT, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Hungary (HU).</summary>
        [Test]
        public void IsNotValid_HU_All()
        {
            IsNotValid(Country.HU, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Indonesia (ID).</summary>
        [Test]
        public void IsNotValid_ID_All()
        {
            IsNotValid(Country.ID, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Israel (IL).</summary>
        [Test]
        public void IsNotValid_IL_All()
        {
            IsNotValid(Country.IL, false, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Isle Of Man (IM).</summary>
        [Test]
        public void IsNotValid_IM_All()
        {
            IsNotValid(Country.IM, true, true, 4, 5);
        }

        /// <summary>Tests patterns that should not be valid for India (IN).</summary>
        [Test]
        public void IsNotValid_IN_All()
        {
            IsNotValid(Country.IN, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for British Indian Ocean Territory (IO).</summary>
        [Test]
        public void IsNotValid_IO_All()
        {
            IsNotValid(Country.IO, true, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Iraq (IQ).</summary>
        [Test]
        public void IsNotValid_IQ_All()
        {
            IsNotValid(Country.IQ, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Iran (IR).</summary>
        [Test]
        public void IsNotValid_IR_All()
        {
            IsNotValid(Country.IR, false, false, 10);
        }

        /// <summary>Tests patterns that should not be valid for Iceland (IS).</summary>
        [Test]
        public void IsNotValid_IS_All()
        {
            IsNotValid(Country.IS, false, false, 3);
        }

        /// <summary>Tests patterns that should not be valid for Italy (IT).</summary>
        [Test]
        public void IsNotValid_IT_All()
        {
            IsNotValid(Country.IT, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Jersey (JE).</summary>
        [Test]
        public void IsNotValid_JE_All()
        {
            IsNotValid(Country.JE, true, true, 4, 5);
        }

        /// <summary>Tests patterns that should not be valid for Jordan (JO).</summary>
        [Test]
        public void IsNotValid_JO_All()
        {
            IsNotValid(Country.JO, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Japan (JP).</summary>
        [Test]
        public void IsNotValid_JP_All()
        {
            IsNotValid(Country.JP, false, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Kyrgyzstan (KG).</summary>
        [Test]
        public void IsNotValid_KG_All()
        {
            IsNotValid(Country.KG, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Cambodia (KH).</summary>
        [Test]
        public void IsNotValid_KH_All()
        {
            IsNotValid(Country.KH, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Korea (KR).</summary>
        [Test]
        public void IsNotValid_KR_All()
        {
            IsNotValid(Country.KR, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Cayman Islands (KY).</summary>
        [Test]
        public void IsNotValid_KY_All()
        {
            IsNotValid(Country.KY, false, true, 5);
        }

        /// <summary>Tests patterns that should not be valid for Kazakhstan (KZ).</summary>
        [Test]
        public void IsNotValid_KZ_All()
        {
            IsNotValid(Country.KZ, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Lao People'S Democratic Republic (LA).</summary>
        [Test]
        public void IsNotValid_LA_All()
        {
            IsNotValid(Country.LA, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Lebanon (LB).</summary>
        [Test]
        public void IsNotValid_LB_All()
        {
            IsNotValid(Country.LB, false, false, 8);
        }

        /// <summary>Tests patterns that should not be valid for Liechtenstein (LI).</summary>
        [Test]
        public void IsNotValid_LI_All()
        {
            IsNotValid(Country.LI, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Sri Lanka (LK).</summary>
        [Test]
        public void IsNotValid_LK_All()
        {
            IsNotValid(Country.LK, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Liberia (LR).</summary>
        [Test]
        public void IsNotValid_LR_All()
        {
            IsNotValid(Country.LR, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Lesotho (LS).</summary>
        [Test]
        public void IsNotValid_LS_All()
        {
            IsNotValid(Country.LS, false, false, 3);
        }

        /// <summary>Tests patterns that should not be valid for Lithuania (LT).</summary>
        [Test]
        public void IsNotValid_LT_All()
        {
            IsNotValid(Country.LT, false, true, 5);
        }

        /// <summary>Tests patterns that should not be valid for Luxembourg (LU).</summary>
        [Test]
        public void IsNotValid_LU_All()
        {
            IsNotValid(Country.LU, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Latvia (LV).</summary>
        [Test]
        public void IsNotValid_LV_All()
        {
            IsNotValid(Country.LV, false, true, 4);
        }

        /// <summary>Tests patterns that should not be valid for Libya (LY).</summary>
        [Test]
        public void IsNotValid_LY_All()
        {
            IsNotValid(Country.LY, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Morocco (MA).</summary>
        [Test]
        public void IsNotValid_MA_All()
        {
            IsNotValid(Country.MA, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Monaco (MC).</summary>
        [Test]
        public void IsNotValid_MC_All()
        {
            IsNotValid(Country.MC, false, true, 5);
        }

        /// <summary>Tests patterns that should not be valid for Moldova (MD).</summary>
        [Test]
        public void IsNotValid_MD_All()
        {
            IsNotValid(Country.MD, false, true, 4);
        }

        /// <summary>Tests patterns that should not be valid for Montenegro (ME).</summary>
        [Test]
        public void IsNotValid_ME_All()
        {
            IsNotValid(Country.ME, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Saint Martin (MF).</summary>
        [Test]
        public void IsNotValid_MF_All()
        {
            IsNotValid(Country.MF, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Madagascar (MG).</summary>
        [Test]
        public void IsNotValid_MG_All()
        {
            IsNotValid(Country.MG, false, false, 3);
        }

        /// <summary>Tests patterns that should not be valid for Marshall Islands (MH).</summary>
        [Test]
        public void IsNotValid_MH_All()
        {
            IsNotValid(Country.MH, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Macedonia (MK).</summary>
        [Test]
        public void IsNotValid_MK_All()
        {
            IsNotValid(Country.MK, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Myanmar (MM).</summary>
        [Test]
        public void IsNotValid_MM_All()
        {
            IsNotValid(Country.MM, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Mongolia (MN).</summary>
        [Test]
        public void IsNotValid_MN_All()
        {
            IsNotValid(Country.MN, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Northern Mariana Islands (MP).</summary>
        [Test]
        public void IsNotValid_MP_All()
        {
            IsNotValid(Country.MP, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Martinique (MQ).</summary>
        [Test]
        public void IsNotValid_MQ_All()
        {
            IsNotValid(Country.MQ, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Malta (MT).</summary>
        [Test]
        public void IsNotValid_MT_All()
        {
            IsNotValid(Country.MT, true, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Mexico (MX).</summary>
        [Test]
        public void IsNotValid_MX_All()
        {
            IsNotValid(Country.MX, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Malaysia (MY).</summary>
        [Test]
        public void IsNotValid_MY_All()
        {
            IsNotValid(Country.MY, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Mozambique (MZ).</summary>
        [Test]
        public void IsNotValid_MZ_All()
        {
            IsNotValid(Country.MZ, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Namibia (NA).</summary>
        [Test]
        public void IsNotValid_NA_All()
        {
            IsNotValid(Country.NA, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for New Caledonia (NC).</summary>
        [Test]
        public void IsNotValid_NC_All()
        {
            IsNotValid(Country.NC, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Niger (NE).</summary>
        [Test]
        public void IsNotValid_NE_All()
        {
            IsNotValid(Country.NE, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Norfolk Island (NF).</summary>
        [Test]
        public void IsNotValid_NF_All()
        {
            IsNotValid(Country.NF, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Nigeria (NG).</summary>
        [Test]
        public void IsNotValid_NG_All()
        {
            IsNotValid(Country.NG, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Nicaragua (NI).</summary>
        [Test]
        public void IsNotValid_NI_All()
        {
            IsNotValid(Country.NI, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Netherlands (NL).</summary>
        [Test]
        public void IsNotValid_NL_All()
        {
            IsNotValid(Country.NL, true, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Norway (NO).</summary>
        [Test]
        public void IsNotValid_NO_All()
        {
            IsNotValid(Country.NO, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Nepal (NP).</summary>
        [Test]
        public void IsNotValid_NP_All()
        {
            IsNotValid(Country.NP, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for New Zealand (NZ).</summary>
        [Test]
        public void IsNotValid_NZ_All()
        {
            IsNotValid(Country.NZ, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Oman (OM).</summary>
        [Test]
        public void IsNotValid_OM_All()
        {
            IsNotValid(Country.OM, false, false, 3);
        }

        /// <summary>Tests patterns that should not be valid for Panama (PA).</summary>
        [Test]
        public void IsNotValid_PA_All()
        {
            IsNotValid(Country.PA, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Peru (PE).</summary>
        [Test]
        public void IsNotValid_PE_All()
        {
            IsNotValid(Country.PE, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for French Polynesia (PF).</summary>
        [Test]
        public void IsNotValid_PF_All()
        {
            IsNotValid(Country.PF, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Papua New Guinea (PG).</summary>
        [Test]
        public void IsNotValid_PG_All()
        {
            IsNotValid(Country.PG, false, false, 3);
        }

        /// <summary>Tests patterns that should not be valid for Philippines (PH).</summary>
        [Test]
        public void IsNotValid_PH_All()
        {
            IsNotValid(Country.PH, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Pakistan (PK).</summary>
        [Test]
        public void IsNotValid_PK_All()
        {
            IsNotValid(Country.PK, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Poland (PL).</summary>
        [Test]
        public void IsNotValid_PL_All()
        {
            IsNotValid(Country.PL, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Saint Pierre And Miquelon (PM).</summary>
        [Test]
        public void IsNotValid_PM_All()
        {
            IsNotValid(Country.PM, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Pitcairn (PN).</summary>
        [Test]
        public void IsNotValid_PN_All()
        {
            IsNotValid(Country.PN, true, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Puerto Rico (PR).</summary>
        [Test]
        public void IsNotValid_PR_All()
        {
            IsNotValid(Country.PR, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Palestinian Territory (PS).</summary>
        [Test]
        public void IsNotValid_PS_All()
        {
            IsNotValid(Country.PS, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Portugal (PT).</summary>
        [Test]
        public void IsNotValid_PT_All()
        {
            IsNotValid(Country.PT, false, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Palau (PW).</summary>
        [Test]
        public void IsNotValid_PW_All()
        {
            IsNotValid(Country.PW, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Paraguay (PY).</summary>
        [Test]
        public void IsNotValid_PY_All()
        {
            IsNotValid(Country.PY, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Réunion (RE).</summary>
        [Test]
        public void IsNotValid_RE_All()
        {
            IsNotValid(Country.RE, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Romania (RO).</summary>
        [Test]
        public void IsNotValid_RO_All()
        {
            IsNotValid(Country.RO, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Serbia (RS).</summary>
        [Test]
        public void IsNotValid_RS_All()
        {
            IsNotValid(Country.RS, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Russian Federation (RU).</summary>
        [Test]
        public void IsNotValid_RU_All()
        {
            IsNotValid(Country.RU, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Saudi Arabia (SA).</summary>
        [Test]
        public void IsNotValid_SA_All()
        {
            IsNotValid(Country.SA, false, false, 5, 9);
        }

        /// <summary>Tests patterns that should not be valid for Sudan (SD).</summary>
        [Test]
        public void IsNotValid_SD_All()
        {
            IsNotValid(Country.SD, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Sweden (SE).</summary>
        [Test]
        public void IsNotValid_SE_All()
        {
            IsNotValid(Country.SE, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Singapore (SG).</summary>
        [Test]
        public void IsNotValid_SG_All()
        {
            IsNotValid(Country.SG, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Saint Helena (SH).</summary>
        [Test]
        public void IsNotValid_SH_All()
        {
            IsNotValid(Country.SH, true, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Slovenia (SI).</summary>
        [Test]
        public void IsNotValid_SI_All()
        {
            IsNotValid(Country.SI, false, true, 4);
        }

        /// <summary>Tests patterns that should not be valid for Slovakia (SK).</summary>
        [Test]
        public void IsNotValid_SK_All()
        {
            IsNotValid(Country.SK, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for San Marino (SM).</summary>
        [Test]
        public void IsNotValid_SM_All()
        {
            IsNotValid(Country.SM, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Senegal (SN).</summary>
        [Test]
        public void IsNotValid_SN_All()
        {
            IsNotValid(Country.SN, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for El Salvador (SV).</summary>
        [Test]
        public void IsNotValid_SV_All()
        {
            IsNotValid(Country.SV, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Swaziland (SZ).</summary>
        [Test]
        public void IsNotValid_SZ_All()
        {
            IsNotValid(Country.SZ, true, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Turks And Caicos Islands (TC).</summary>
        [Test]
        public void IsNotValid_TC_All()
        {
            IsNotValid(Country.TC, true, false, 7);
        }

        /// <summary>Tests patterns that should not be valid for Chad (TD).</summary>
        [Test]
        public void IsNotValid_TD_All()
        {
            IsNotValid(Country.TD, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Thailand (TH).</summary>
        [Test]
        public void IsNotValid_TH_All()
        {
            IsNotValid(Country.TH, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Tajikistan (TJ).</summary>
        [Test]
        public void IsNotValid_TJ_All()
        {
            IsNotValid(Country.TJ, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Turkmenistan (TM).</summary>
        [Test]
        public void IsNotValid_TM_All()
        {
            IsNotValid(Country.TM, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Tunisia (TN).</summary>
        [Test]
        public void IsNotValid_TN_All()
        {
            IsNotValid(Country.TN, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Turkey (TR).</summary>
        [Test]
        public void IsNotValid_TR_All()
        {
            IsNotValid(Country.TR, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Trinidad And Tobago (TT).</summary>
        [Test]
        public void IsNotValid_TT_All()
        {
            IsNotValid(Country.TT, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Taiwan (TW).</summary>
        [Test]
        public void IsNotValid_TW_All()
        {
            IsNotValid(Country.TW, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Ukraine (UA).</summary>
        [Test]
        public void IsNotValid_UA_All()
        {
            IsNotValid(Country.UA, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for United States (US).</summary>
        [Test]
        public void IsNotValid_US_All()
        {
            IsNotValid(Country.US, false, false, 5, 9);
        }

        /// <summary>Tests patterns that should not be valid for Uruguay (UY).</summary>
        [Test]
        public void IsNotValid_UY_All()
        {
            IsNotValid(Country.UY, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Holy See (VA).</summary>
        [Test]
        public void IsNotValid_VA_All()
        {
            IsNotValid(Country.VA, false, false, 3);
        }

        /// <summary>Tests patterns that should not be valid for Saint Vincent And The Grenadines (VC).</summary>
        [Test]
        public void IsNotValid_VC_All()
        {
            IsNotValid(Country.VC, false, true, 4);
        }
        
        /// <summary>Tests patterns that should not be valid for Virgin Islands (VG).</summary>
        [Test]
        public void IsNotValid_VG_All()
        {
            IsNotValid(Country.VG, false, true, 4);
        }

        /// <summary>Tests patterns that should not be valid for Virgin Islands (VI).</summary>
        [Test]
        public void IsNotValid_VI_All()
        {
            IsNotValid(Country.VI, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Viet Nam (VN).</summary>
        [Test]
        public void IsNotValid_VN_All()
        {
            IsNotValid(Country.VN, false, false, 6);
        }

        /// <summary>Tests patterns that should not be valid for Wallis And Futuna (WF).</summary>
        [Test]
        public void IsNotValid_WF_All()
        {
            IsNotValid(Country.WF, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Kosovo (XK).</summary>
        [Test]
        public void IsNotValid_XK_All()
        {
            IsNotValid(Country.XK, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for Mayotte (YT).</summary>
        [Test]
        public void IsNotValid_YT_All()
        {
            IsNotValid(Country.YT, false, false, 5);
        }

        /// <summary>Tests patterns that should not be valid for South Africa (ZA).</summary>
        [Test]
        public void IsNotValid_ZA_All()
        {
            IsNotValid(Country.ZA, false, false, 4);
        }

        /// <summary>Tests patterns that should not be valid for Zambia (ZM).</summary>
        [Test]
        public void IsNotValid_ZM_All()
        {
            IsNotValid(Country.ZM, false, false, 5);
        }

    }
}
