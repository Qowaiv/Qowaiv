using NUnit.Framework;

namespace Qowaiv.UnitTests
{
    public partial class PostalCodeTest
    {
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

        /// <summary>Tests patterns that should not be valid for Venezuela (VE).</summary>
        [Test]
        public void IsNotValid_VE_All()
        {
            // Not possible to generate.
            // IsNotValid(Country.VE, true, false, 4, 5);
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
