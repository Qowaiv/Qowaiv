using System.Collections.Generic;

namespace Qowaiv.Globalization
{
    public partial class PostalCodeCountryInfo
    {
        /// <summary>Gets the country based settings.</summary>
        private static readonly Dictionary<Country, PostalCodeCountryInfo> Instances = new()
        {
            // AD: Andorra, http://en.wikipedia.org/wiki/Postal_codes_in_Andorra
            { Country.AD, new(Country.AD, @"^(AD)?[1-7][0-9]{2}$", "^(AD)?(...)$", "AD-$2") },

            // AF: Afghanistan, http://en.wikipedia.org/wiki/Postal_codes_in_Afghanistan. The range is 10-43.
            { Country.AF, new(Country.AF, @"^(0[1-9]|[1-3][0-9]|4[0-3])([0-9]{2})(?<!00)$") },

            // AI: Anguilla, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.AI, new(Country.AI, @"^(AI)?2640$", "^.+$", "AI-2640", isSingle:true) },

            // AL: Albania, http://en.wikipedia.org/wiki/Postal_codes_in_Albania
            { Country.AL, new(Country.AL, @"^[1-9][0-9]{3}$") },

            // AM: Armenia, http://en.wikipedia.org/wiki/Postal_codes_in_Armenia. Range 0-4.
            { Country.AM, new(Country.AM, @"^[0-4][0-9]{3}$") },

            // AR: Argentina, http://en.wikipedia.org/wiki/Argentine_postal_code
            { Country.AR, new(Country.AR, @"^[A-Z][1-9][0-9]{3}[A-Z]{3}$", "^(.)(....)(...)$", "$1 $2 $3") },

            // AS: American Samoa, http://en.wikipedia.org/wiki/ZIP_Code
            { Country.AS, new(Country.AS, @"^9[0-9]{4}([0-9]{4})?$", "^(.{5})(....)?$", "$1 $2") },

            // AT: Austria, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Austria
            { Country.AT, new(Country.AT, @"^[1-9][0-9]{3}$") },

            // AU: Australia, http://en.wikipedia.org/wiki/Postcodes_in_Australia
            { Country.AU, new(Country.AU, @"^(0[89]|[1-9][0-9])[0-9]{2}$") },

            // AX: Åland Islands, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Finland
            { Country.AX, new(Country.AX, @"^22[0-9]{3}$", "^(..)(...)$", "$1-$2") },

            // AZ: Azerbaijan, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.AZ, new(Country.AZ, @"^(AZ)?[0-9]{4}$", "^(AZ)?(....)$", "AZ-$2") },

            // BA: Bosnia And Herzegovina, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.BA, new(Country.BA, @"^[0-9]{5}$") },

            // BB: Barbados, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.BB, new(Country.BB, @"^(BB)?[0-9]{5}$", "^(BB)?(.{5})$", "BB-$2") },

            // BD: Bangladesh, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.BD, new(Country.BD, @"^[0-9]{4}$") },

            // BE: Belgium, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Belgium
            { Country.BE, new(Country.BE, @"^[1-9][0-9]{3}$") },

            // BG: Bulgaria, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Bulgaria
            { Country.BG, new(Country.BG, @"^[1-9][0-9]{3}$") },

            // BH: Bahrain, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.BH, new(Country.BH, @"^(1[0-2]|[1-9])[0-9]{2}$") },

            // BL: Saint Barthélemy, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.BL, new(Country.BL, @"^977[0-9]{2}$") },

            // BM: Bermuda, http://en.wikipedia.org/wiki/Postal_codes_in_Bermuda
            { Country.BM, new(Country.BM, @"^[A-Z]{2}([A-Z0-9]{2})?$", "^(..)(..)$", "$1 $2") },

            // BN: Brunei Darussalam, http://en.wikipedia.org/wiki/Postal_codes_in_Brunei
            { Country.BN, new(Country.BN, @"^[A-Z]{2}[0-9]{4}$") },

            // BO: Bolivia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.BO, new(Country.BO, @"^[0-9]{4}$") },

            // BR: Brazil, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Brazil
            { Country.BR, new(Country.BR, @"^([0-9]{2})(?<!00)[0-9]{6}$", "^(.{5})(...)$", "$1-$2") },

            // BT: Bhutan, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.BT, new(Country.BT, @"^[0-9]{3}$") },

            // BY: Belarus, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.BY, new(Country.BY, @"^[0-9]{6}$") },

            // CA: Canada, http://en.wikipedia.org/wiki/Canadian_postal_code. No D, F, I, O, Q, and U (and W and Z to start with).
            { Country.CA, new(Country.CA, @"^[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$", "^(...)(...)$", "$1 $2") },

            // CC: Cocos, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.CC, new(Country.CC, @"^[0-9]{4}$") },

            // CH: Switzerland, http://en.wikipedia.org/wiki/Postal_codes_in_Switzerland_and_Liechtenstein
            { Country.CH, new(Country.CH, @"^[1-9][0-9]{3}$") },

            // CL: Chile, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.CL, new(Country.CL, @"^[0-9]{7}$", "^(...)(....)$", "$1-$2") },

            // CN: China, http://en.wikipedia.org/wiki/Postal_code_of_China
            { Country.CN, new(Country.CN, @"^([0-9]{2})(?<!00)[0-9]{4}$") },

            // CO: Colombia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.CO, new(Country.CO, @"^[0-9]{6}$") },

            // CR: Costa Rica, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.CR, new(Country.CR, @"^[0-9]{5}$") },

            // CU: Cuba, http://en.wikipedia.org/wiki/List_of_postal_codes. The letters CP are frequently used before the postal code. This is not a country code, but an abbreviation for "codigo postal" or postal code.
            { Country.CU, new(Country.CU, @"^(CP)?[0-9]{5}$", "^(..)?(.{5})$", "CP$2") },

            // CV: Cape Verde, http://en.wikipedia.org/wiki/Cape_Verde
            { Country.CV, new(Country.CV, @"^[0-9]{4}$") },

            // CX: Christmas Island, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.CX, new(Country.CX, @"^[0-9]{4}$") },

            // CY: Cyprus, http://en.wikipedia.org/wiki/Postal_codes_in_Cyprus
            { Country.CY, new(Country.CY, @"^[1-9][0-9]{3}$") },

            // CZ: Czech Republic, http://en.wikipedia.org/wiki/List_of_postal_codes_in_the_Czech_Republic
            { Country.CZ, new(Country.CZ, @"^[1-7][0-9]{4}$", "^(...)(..)$", "$1 $2") },

            // DE: Germany, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Germany
            { Country.DE, new(Country.DE, @"^([0-9]{2})(?<!00)[0-9]{3}$") },

            // DK: Denmark, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Denmark
            { Country.DK, new(Country.DK, @"^(DK)?[1-9][0-9]{3}$", "^(DK)?(....)$", "DK-$2") },

            // DZ: Algeria, http://en.wikipedia.org/wiki/List_of_postal_codes_of_Algerian_cities
            { Country.DZ, new(Country.DZ, @"^[0-9]{5}$") },

            // EC: Ecuador, http://en.wikipedia.org/wiki/Postal_codes_in_Ecuador
            { Country.EC, new(Country.EC, @"^[0-9]{6}$") },

            // EE: Estonia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.EE, new(Country.EE, @"^[0-9]{5}$") },

            // EG: Egypt, http://en.wikipedia.org/wiki/Postal_codes_in_Egypt
            { Country.EG, new(Country.EG, @"^[1-9][0-9]{4}$") },

            // ES: Spain, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Spain. Start with 01-52.
            { Country.ES, new(Country.ES, @"^((0[1-9])|([1-4][0-9])|(5[012]))[0-9]{3}$") },

            // ET: Ethiopia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.ET, new(Country.ET, @"^[0-9]{4}$") },

            // FI: Finland, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Finland
            { Country.FI, new(Country.FI, @"^[0-9]{5}$", "^(..)(...)$", "$1-$2") },

            // FK: Falkland Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.FK, new(Country.FK, @"^FIQQ1ZZ$", "^.+$", "FIQQ 1ZZ", isSingle:true) },

            // FM: Micronesia, http://en.wikipedia.org/wiki/ZIP_Code
            { Country.FM, new(Country.FM, @"^9694[1234]([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

            // FO: Faroe Islands, http://en.wikipedia.org/wiki/List_of_postal_codes_in_the_Faroe_Islands
            { Country.FO, new(Country.FO, @"^(FO)?[1-9][0-9]{2}$", "^(FO)?(...)$", "FO-$2") },

            // FR: France, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.FR, new(Country.FR, @"^([0-9]{2})(?<!00)[0-9]{3}$") },

            // GA: Gabon, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.GA, new(Country.GA, @"^[0-9]{4}$", "^(..)(..)$", "$1 $2") },

            // GB: United Kingdom, http://en.wikipedia.org/wiki/UK_postcodes
            { Country.GB, new(Country.GB, @"^[A-Z]?[A-Z][0-9][A-Z0-9]?[0-9][A-Z]{2}$", "^(.+)(...)$", "$1 $2") },

            // GE: Georgia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.GE, new(Country.GE, @"^[0-9]{4}$") },

            // GF: French Guiana, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.GF, new(Country.GF, @"^973[0-9]{2}$") },

            // GG: Guernsey, https://en.wikipedia.org/wiki/GY_postcode_area
            { Country.GG, new(Country.GG, @"^(GY)?[0-9]{2,3}[A-Z]{2}$", "^(GY)?(...?)(...)$", "GY$2 $3") },

            // GI: Gibraltar, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.GI, new(Country.GI, @"^GX111AA$", "^.+$", "GX11 1AA", isSingle:true) },

            // GL: Greenland, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Greenland
            { Country.GL, new(Country.GL, @"^(GL)?39[0-9]{2}$", "^(GL)?(....)$", "GL-$2") },

            // GP: Guadeloupe, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.GP, new(Country.GP, @"^971[0-9]{2}$") },

            // GR: Greece, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Greece
            { Country.GR, new(Country.GR, @"^[1-9][0-9]{4}$", "^(...)(..)$", "$1 $2") },

            // GS: South Georgia And The South Sandwich Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.GS, new(Country.GS, @"^SIQQ1ZZ$", "^.+$", "SIQQ 1ZZ", isSingle:true) },

            // GT: Guatemala, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.GT, new(Country.GT, @"^[0-9]{5}$") },

            // GU: Guam, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.GU, new(Country.GU, @"^969([12][0-9]|3[0-3])([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

            // GW: Guinea-Bissau, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.GW, new(Country.GW, @"^[0-9]{4}$") },

            // HM: Heard Island And Mcdonald Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.HM, new(Country.HM, @"^[0-9]{4}$") },

            // HN: Honduras, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.HN, new(Country.HN, @"^[0-9]{5}$") },

            // HR: Croatia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.HR, new(Country.HR, @"^[0-9]{5}$") },

            // HT: Haiti, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.HT, new(Country.HT, @"^[0-9]{4}$") },

            // HU: Hungary, http://en.wikipedia.org/wiki/Postal_codes_in_Hungary
            { Country.HU, new(Country.HU, @"^[1-9][0-9]{3}$") },

            // ID: Indonesia, http://en.wikipedia.org/wiki/Postal_codes_in_Indonesia
            { Country.ID, new(Country.ID, @"^[1-9][0-9]{4}$") },

            // IL: Israel, http://en.wikipedia.org/wiki/Postal_codes_in_Israel
            { Country.IL, new(Country.IL, @"^[0-9]{7}$") },

            // IM: Isle Of Man, https://en.wikipedia.org/wiki/IM_postcode_area
            { Country.IM, new(Country.IM, @"^(IM)?[0-9]{2,3}[A-Z]{2}$", "^(IM)?(..?)(...)$", "IM$2 $3") },

            // IN: India, http://en.wikipedia.org/wiki/Postal_Index_Number
            { Country.IN, new(Country.IN, @"^[1-9][0-9]{5}$") },

            // IO: British Indian Ocean Territory, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.IO, new(Country.IO, @"^BBND1ZZ$", "^.+$", "BBND 1ZZ", isSingle:true) },

            // IQ: Iraq, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Iraq
            { Country.IQ, new(Country.IQ, @"^[13456][0-9]{4}$") },

            // IR: Iran, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.IR, new(Country.IR, @"^[0-9]{10}$", "^(.{5})(.{5})$", "$1-$2") },

            // IS: Iceland, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.IS, new(Country.IS, @"^[0-9]{3}$") },

            // IT: Italy, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Italy
            { Country.IT, new(Country.IT, @"^([0-9]{3})(?<!000)[0-9]{2}$") },

            // JE: Jersey, https://en.wikipedia.org/wiki/JE_postcode_area
            { Country.JE, new(Country.JE, @"^(JE)?[0-9]{2,3}[A-Z]{2}$", "^(JE)?(...?)(...)$", "JE$2 $3") },

            // JO: Jordan, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.JO, new(Country.JO, @"^[0-9]{5}$") },

            // JP: Japan, http://en.wikipedia.org/wiki/Postal_codes_in_Japan
            { Country.JP, new(Country.JP, @"^[0-9]{7}$", "^(....)(...)$", "$1-$2") },

            // KG: Kyrgyzstan, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.KG, new(Country.KG, @"^[0-9]{6}$") },

            // KH: Cambodia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.KH, new(Country.KH, @"^[0-9]{5}$") },

            // KR: Korea, http://en.wikipedia.org/wiki/List_of_postal_codes_in_South_Korea
            { Country.KR, new(Country.KR, @"^[1-7][0-9]{5}$", "^(...)(...)$", "$1-$2") },

            // KY: Cayman Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.KY, new(Country.KY, @"^(KY)?[0-9]{5}$", "^(KY)?(.)(....)$", "KY$2-$3") },

            // KZ: Kazakhstan, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.KZ, new(Country.KZ, @"^[0-9]{6}$") },

            // LA: Lao People'S Democratic Republic, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.LA, new(Country.LA, @"^[0-9]{5}$") },

            // LB: Lebanon, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.LB, new(Country.LB, @"^[0-9]{8}$", "^(....)(....)$", "$1 $2") },

            // LI: Liechtenstein, http://en.wikipedia.org/wiki/Postal_codes_in_Switzerland_and_Liechtenstein
            { Country.LI, new(Country.LI, @"^94(8[5-9]|9[0-8])$") },

            // LK: Sri Lanka, http://en.wikipedia.org/wiki/Postal_codes_in_Sri_Lanka
            { Country.LK, new(Country.LK, @"^[0-9]{5}$") },

            // LR: Liberia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.LR, new(Country.LR, @"^[0-9]{4}$") },

            // LS: Lesotho, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.LS, new(Country.LS, @"^[0-9]{3}$") },

            // LT: Lithuania, http://en.wikipedia.org/wiki/Postal_codes_in_Lithuania
            { Country.LT, new(Country.LT, @"^(LT)?[0-9]{5}$", "^(LT)?(.{5})$", "LT-$2") },

            // LU: Luxembourg, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.LU, new(Country.LU, @"^[0-9]{4}$") },

            // LV: Latvia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.LV, new(Country.LV, @"^(LV)?[0-9]{4}$", "^(LV)?(....)$", "LV-$2") },

            // LY: Libya, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.LY, new(Country.LY, @"^[0-9]{5}$") },

            // MA: Morocco, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Morocco
            { Country.MA, new(Country.MA, @"^[1-9][0-9]{4}$", "^(..)(...)$", "$1 $2") },

            // MC: Monaco, http://en.wikipedia.org/wiki/Postal_codes_in_France
            { Country.MC, new(Country.MC, @"^(MC)?980[0-9]{2}$") },

            // MD: Moldova, http://en.wikipedia.org/wiki/Postal_codes_in_Moldova
            { Country.MD, new(Country.MD, @"^(MD)?[0-9]{4}$", "^(MD)?(....)$", "MD-$2") },

            // ME: Montenegro, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Montenegro
            { Country.ME, new(Country.ME, @"^8[145][0-9]{3}$") },

            // MF: Saint Martin, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.MF, new(Country.MF, @"^978[0-9]{2}$") },

            // MG: Madagascar, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.MG, new(Country.MG, @"^[0-9]{3}$") },

            // MH: Marshall Islands, http://en.wikipedia.org/wiki/ZIP_Code
            { Country.MH, new(Country.MH, @"^969[67][0-9]([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

            // MK: Macedonia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.MK, new(Country.MK, @"^[0-9]{4}$") },

            // MM: Myanmar, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.MM, new(Country.MM, @"^[0-9]{5}$") },

            // MN: Mongolia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.MN, new(Country.MN, @"^[0-9]{5}$") },

            // MP: Northern Mariana Islands, http://en.wikipedia.org/wiki/ZIP_Code
            { Country.MP, new(Country.MP, @"^9695[012]([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

            // MQ: Martinique, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.MQ, new(Country.MQ, @"^972[0-9]{2}$") },

            // MT: Malta, http://en.wikipedia.org/wiki/Postal_codes_in_Malta
            { Country.MT, new(Country.MT, @"^[A-Z]{3}[0-9]{4}$", "^(...)(....)$", "$1 $2") },

            // MX: Mexico, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Mexico
            { Country.MX, new(Country.MX, @"^[0-9]{5}$") },

            // MY: Malaysia, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Malaysia
            { Country.MY, new(Country.MY, @"^([0-9]{2})(?<!00)[0-9]{3}$") },

            // MZ: Mozambique, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.MZ, new(Country.MZ, @"^[0-9]{4}$") },

            // NA: Namibia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.NA, new(Country.NA, @"^9[0-2][0-9]{3}$") },

            // NC: new Caledonia, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.NC, new(Country.NC, @"^988[0-9]{2}$") },

            // NE: Niger, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.NE, new(Country.NE, @"^[0-9]{4}$") },

            // NF: Norfolk Island, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.NF, new(Country.NF, @"^[0-9]{4}$") },

            // NG: Nigeria, http://en.wikipedia.org/wiki/Postal_codes_in_Nigeria
            { Country.NG, new(Country.NG, @"^[0-9]{6}$") },

            // NI: Nicaragua, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.NI, new(Country.NI, @"^[0-9]{5}$") },

            // NL: Netherlands, http://en.wikipedia.org/wiki/List_of_postal_codes_in_the_Netherlands. No SA, SD, or SS.
            { Country.NL, new(Country.NL, @"^[1-9][0-9]{3}([A-Z]{2})(?<!SS|SA|SD)$", "^(....)(..)$", "$1 $2") },

            // NO: Norway, http://en.wikipedia.org/wiki/Postal_codes_in_Norway
            { Country.NO, new(Country.NO, @"^[0-9]{4}$") },

            // NP: Nepal, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.NP, new(Country.NP, @"^[0-9]{5}$") },

            // NZ: new Zealand, http://en.wikipedia.org/wiki/Postcodes_in_new_Zealand
            { Country.NZ, new(Country.NZ, @"^[0-9]{4}$") },

            // OM: Oman, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.OM, new(Country.OM, @"^[0-9]{3}$") },

            // PA: Panama, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.PA, new(Country.PA, @"^[0-9]{6}$") },

            // PE: Peru, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Peru
            { Country.PE, new(Country.PE, @"^[0-9]{5}$") },

            // PF: French Polynesia, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.PF, new(Country.PF, @"^987[0-9]{2}$") },

            // PG: Papua new Guinea, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.PG, new(Country.PG, @"^[0-9]{3}$") },

            // PH: Philippines, http://en.wikipedia.org/wiki/List_of_ZIP_Codes_in_the_Philippines
            { Country.PH, new(Country.PH, @"^([0-9]{2})(?<!00)[0-9]{2}$") },

            // PK: Pakistan, http://en.wikipedia.org/wiki/List_of_postal_codes_of_Pakistan
            { Country.PK, new(Country.PK, @"^[1-9][0-9]{4}$") },

            // PL: Poland, // Poland. http://en.wikipedia.org/wiki/List_of_postal_codes_in_Poland
            { Country.PL, new(Country.PL, @"^[0-9]{5}$", "^(..)(...)$", "$1-$2") },

            // PM: Saint Pierre And Miquelon, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.PM, new(Country.PM, @"^97500$") },

            // PN: Pitcairn, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.PN, new(Country.PN, @"^PCRN1ZZ$", "^.+$", "PCRN 1ZZ", isSingle:true) },

            // PR: Puerto Rico, http://en.wikipedia.org/wiki/Postal_codes_in_Puerto_Rico
            { Country.PR, new(Country.PR, @"^[0-9]{5}$") },

            // PS: Palestinian Territory, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.PS, new(Country.PS, @"^[0-9]{5}$") },

            // PT: Portugal, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Portugal
            { Country.PT, new(Country.PT, @"^[1-9][0-9]{6}$", "^(....)(...)$", "$1 $2") },

            // PW: Palau, http://en.wikipedia.org/wiki/ZIP_Code
            { Country.PW, new(Country.PW, @"^96940([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

            // PY: Paraguay, http://en.wikipedia.org/wiki/Postal_codes_in_Norway
            { Country.PY, new(Country.PY, @"^[0-9]{4}$") },

            // RE: Réunion, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.RE, new(Country.RE, @"^974[0-9]{2}$") },

            // RO: Romania, http://en.wikipedia.org/wiki/Postal_codes_in_Romania
            { Country.RO, new(Country.RO, @"^([0-9]{2})(?<!00)[0-9]{4}$") },

            // RS: Serbia, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Serbia
            { Country.RS, new(Country.RS, @"^[123][0-9]{4}$") },

            // RU: Russian Federation, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Russia
            { Country.RU, new(Country.RU, @"^[1-6][0-9]{5}$") },

            // SA: Saudi Arabia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.SA, new(Country.SA, @"^[0-9]{5}([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

            // SD: Sudan, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.SD, new(Country.SD, @"^[0-9]{5}$") },

            // SE: Sweden, http://en.wikipedia.org/wiki/Postal_codes_in_Sweden
            { Country.SE, new(Country.SE, @"^[1-9][0-9]{4}$", "^(...)(..)$", "$1 $2") },

            // SG: Singapore, http://en.wikipedia.org/wiki/Postal_codes_in_Singapore
            { Country.SG, new(Country.SG, @"^[0-9]{5}$") },

            // SH: Saint Helena, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.SH, new(Country.SH, @"^STHL1ZZ$", "^.+$", "STHL 1ZZ", isSingle:true) },

            // SI: Slovenia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.SI, new(Country.SI, @"^(SI)?[0-9]{4}$", "^(SI)?(....)$", "SI-$2") },

            // SK: Slovakia, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Slovakia
            { Country.SK, new(Country.SK, @"^[0-9]{5}$", "^(...)(..)$", "$1 $2") },

            // SM: San Marino, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Italy
            { Country.SM, new(Country.SM, @"^4789[0-9]$") },

            // SN: Senegal, http://en.wikipedia.org/wiki/List_of_postal_codes. The letters CP are frequently used before the postal code. This is not a country code, but an abbreviation for "code postal" or postal code.
            { Country.SN, new(Country.SN, @"^(CP)?[0-9]{5}$", "^(..)?(.{5})$", "CP$2") },

            // SV: El Salvador, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.SV, new(Country.SV, @"^01101$", null, "01101", isSingle:true) },

            // SZ: Swaziland, http://en.wikipedia.org/wiki/List_of_postal_codes. The letter identifies one of the country's four districts.
            { Country.SZ, new(Country.SZ, @"^[HLMS][0-9]{3}$") },

            // TC: Turks And Caicos Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.TC, new(Country.TC, @"^TKCA1ZZ$", "^.+$", "TKCA 1ZZ", isSingle:true) },

            // TD: Chad, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.TD, new(Country.TD, @"^[0-9]{5}$") },

            // TH: Thailand, http://en.wikipedia.org/wiki/Postal_codes_in_Thailand
            { Country.TH, new(Country.TH, @"^[1-9][0-9]{4}$") },

            // TJ: Tajikistan, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.TJ, new(Country.TJ, @"^[0-9]{6}$") },

            // TM: Turkmenistan, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.TM, new(Country.TM, @"^[0-9]{6}$") },

            // TN: Tunisia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.TN, new(Country.TN, @"^[0-9]{4}$") },

            // TR: Turkey, http://en.wikipedia.org/wiki/Postal_codes_in_Turkey
            { Country.TR, new(Country.TR, @"^([0-9]{2})(?<!00)[0-9]{3}$") },

            // TT: Trinidad And Tobago, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.TT, new(Country.TT, @"^[0-9]{6}$") },

            // TW: Taiwan, http://en.wikipedia.org/wiki/List_of_postal_codes_in_the_Republic_of_China
            { Country.TW, new(Country.TW, @"^[1-9][0-9]{4}$") },

            // UA: Ukraine, http://en.wikipedia.org/wiki/Postal_codes_in_Ukraine
            { Country.UA, new(Country.UA, @"^([0-9]{2})(?<!00)[0-9]{3}$") },

            // US: United States, http://en.wikipedia.org/wiki/ZIP_Code
            { Country.US, new(Country.US, @"^[0-9]{5}([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

            // UY: Uruguay, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.UY, new(Country.UY, @"^[0-9]{5}$") },

            // VA: Holy See, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Italy
            { Country.VA, new(Country.VA, @"^00120$", null, "00120", isSingle:true) },

            // VC: Saint Vincent And The Grenadines, http://www.svgpost.gov.vc/index.php?option=com_content&view=article&id=3&Itemid=7
            { Country.VC, new(Country.VC, @"^(VC)?[0-9]{4}$", "^(VC)?(....)$", "VC$2") },

            // VE: Venezuela, http://en.wikipedia.org/wiki/Caracas
            { Country.VE, new(Country.VE, @"^[0-9]{4}[A-Z]?$", "^(....)(.)$", "$1-$2") },

            // VG: Virgin Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.VG, new(Country.VG, @"^(VG)?11[0-6][0-9]$", "^(VG)?(....)$", "VG$2") },

            // VI: Virgin Islands, http://en.wikipedia.org/wiki/ZIP_Code
            { Country.VI, new(Country.VI, @"^008[1-5][0-9]([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

            // VN: Viet Nam, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.VN, new(Country.VN, @"^[0-9]{6}$") },

            // WF: Wallis And Futuna, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.WF, new(Country.WF, @"^986[0-9]{2}$") },

            // XK: Kosovo, https://en.wikipedia.org/wiki/Postal_codes_in_Kosovo
            { Country.XK, new(Country.XK, @"^[1-7][0-9]{4}$") },

            // YT: Mayotte, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
            { Country.YT, new(Country.YT, @"^976[0-9]{2}$") },

            // ZA: South Africa, http://en.wikipedia.org/wiki/Postal_codes_in_South_Africa
            { Country.ZA, new(Country.ZA, @"^([0-9]{4})(?<!0000)$") },

            // ZM: Zambia, http://en.wikipedia.org/wiki/List_of_postal_codes
            { Country.ZM, new(Country.ZM, @"^[0-9]{5}$") },
        };
    }
}
