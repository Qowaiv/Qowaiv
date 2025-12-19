namespace Qowaiv.Globalization;

public partial class PostalCodeCountryInfo
{
    /// <summary>Gets the country based settings.</summary>
    private static readonly Dictionary<Country, PostalCodeCountryInfo> Instances = new()
    {
        // AD: Andorra, http://en.wikipedia.org/wiki/Postal_codes_in_Andorra
        { Country.AD, New(Country.AD, @"^(AD)?[1-7][0-9]{2}$", "^(AD)?(...)$", "AD-$2") },

        // AF: Afghanistan, http://en.wikipedia.org/wiki/Postal_codes_in_Afghanistan. The range is 10-43.
        { Country.AF, New(Country.AF, @"^(0[1-9]|[1-3][0-9]|4[0-3])([0-9]{2})(?<!00)$") },

        // AI: Anguilla, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.AI, New(Country.AI, @"^(AI)?2640$", "^.+$", "AI-2640", isSingle: true) },

        // AL: Albania, http://en.wikipedia.org/wiki/Postal_codes_in_Albania
        { Country.AL, New(Country.AL, @"^[1-9][0-9]{3}$") },

        // AM: Armenia, http://en.wikipedia.org/wiki/Postal_codes_in_Armenia. Range 0-4.
        { Country.AM, New(Country.AM, @"^[0-4][0-9]{3}$") },

        // AR: Argentina, http://en.wikipedia.org/wiki/Argentine_postal_code
        { Country.AR, New(Country.AR, @"^[A-Z][1-9][0-9]{3}[A-Z]{3}$", "^(.)(....)(...)$", "$1 $2 $3") },

        // AS: American Samoa, http://en.wikipedia.org/wiki/ZIP_Code
        { Country.AS, New(Country.AS, @"^9[0-9]{4}([0-9]{4})?$", "^(.{5})(....)?$", "$1 $2") },

        // AT: Austria, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Austria
        { Country.AT, New(Country.AT, @"^[1-9][0-9]{3}$") },

        // AU: Australia, http://en.wikipedia.org/wiki/Postcodes_in_Australia
        { Country.AU, New(Country.AU, @"^(0[89]|[1-9][0-9])[0-9]{2}$") },

        // AX: Åland Islands, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Finland
        { Country.AX, New(Country.AX, @"^22[0-9]{3}$", "^(..)(...)$", "$1-$2") },

        // AZ: Azerbaijan, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.AZ, New(Country.AZ, @"^(AZ)?[0-9]{4}$", "^(AZ)?(....)$", "AZ-$2") },

        // BA: Bosnia And Herzegovina, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.BA, New(Country.BA, @"^[0-9]{5}$") },

        // BB: Barbados, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.BB, New(Country.BB, @"^(BB)?[0-9]{5}$", "^(BB)?(.{5})$", "BB-$2") },

        // BD: Bangladesh, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.BD, New(Country.BD, @"^[0-9]{4}$") },

        // BE: Belgium, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Belgium
        { Country.BE, New(Country.BE, @"^[1-9][0-9]{3}$") },

        // BG: Bulgaria, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Bulgaria
        { Country.BG, New(Country.BG, @"^[1-9][0-9]{3}$") },

        // BH: Bahrain, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.BH, New(Country.BH, @"^(1[0-2]|[1-9])[0-9]{2}$") },

        // BL: Saint Barthélemy, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.BL, New(Country.BL, @"^977[0-9]{2}$") },

        // BM: Bermuda, http://en.wikipedia.org/wiki/Postal_codes_in_Bermuda
        { Country.BM, New(Country.BM, @"^[A-Z]{2}([A-Z0-9]{2})?$", "^(..)(..)$", "$1 $2") },

        // BN: Brunei Darussalam, http://en.wikipedia.org/wiki/Postal_codes_in_Brunei
        { Country.BN, New(Country.BN, @"^[A-Z]{2}[0-9]{4}$") },

        // BO: Bolivia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.BO, New(Country.BO, @"^[0-9]{4}$") },

        // BR: Brazil, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Brazil
        { Country.BR, New(Country.BR, @"^([0-9]{2})(?<!00)[0-9]{6}$", "^(.{5})(...)$", "$1-$2") },

        // BT: Bhutan, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.BT, New(Country.BT, @"^[0-9]{3}$") },

        // BY: Belarus, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.BY, New(Country.BY, @"^[0-9]{6}$") },

        // CA: Canada, http://en.wikipedia.org/wiki/Canadian_postal_code. No D, F, I, O, Q, and U (and W and Z to start with).
        { Country.CA, New(Country.CA, @"^[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$", "^(...)(...)$", "$1 $2") },

        // CC: Cocos, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.CC, New(Country.CC, @"^[0-9]{4}$") },

        // CH: Switzerland, http://en.wikipedia.org/wiki/Postal_codes_in_Switzerland_and_Liechtenstein
        { Country.CH, New(Country.CH, @"^[1-9][0-9]{3}$") },

        // CL: Chile, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.CL, New(Country.CL, @"^[0-9]{7}$", "^(...)(....)$", "$1-$2") },

        // CN: China, http://en.wikipedia.org/wiki/Postal_code_of_China
        { Country.CN, New(Country.CN, @"^([0-9]{2})(?<!00)[0-9]{4}$") },

        // CO: Colombia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.CO, New(Country.CO, @"^[0-9]{6}$") },

        // CR: Costa Rica, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.CR, New(Country.CR, @"^[0-9]{5}$") },

        // CU: Cuba, http://en.wikipedia.org/wiki/List_of_postal_codes. The letters CP are frequently used before the postal code. This is not a country code, but an abbreviation for "codigo postal" or postal code.
        { Country.CU, New(Country.CU, @"^(CP)?[0-9]{5}$", "^(..)?(.{5})$", "CP$2") },

        // CV: Cape Verde, http://en.wikipedia.org/wiki/Cape_Verde
        { Country.CV, New(Country.CV, @"^[0-9]{4}$") },

        // CX: Christmas Island, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.CX, New(Country.CX, @"^[0-9]{4}$") },

        // CY: Cyprus, http://en.wikipedia.org/wiki/Postal_codes_in_Cyprus
        { Country.CY, New(Country.CY, @"^[1-9][0-9]{3}$") },

        // CZ: Czech Republic, http://en.wikipedia.org/wiki/List_of_postal_codes_in_the_Czech_Republic
        { Country.CZ, New(Country.CZ, @"^[1-7][0-9]{4}$", "^(...)(..)$", "$1 $2") },

        // DE: Germany, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Germany
        { Country.DE, New(Country.DE, @"^([0-9]{2})(?<!00)[0-9]{3}$") },

        // DK: Denmark, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Denmark
        { Country.DK, New(Country.DK, @"^(DK)?[1-9][0-9]{3}$", "^(DK)?(....)$", "DK-$2") },

        // DZ: Algeria, http://en.wikipedia.org/wiki/List_of_postal_codes_of_Algerian_cities
        { Country.DZ, New(Country.DZ, @"^[0-9]{5}$") },

        // EC: Ecuador, http://en.wikipedia.org/wiki/Postal_codes_in_Ecuador
        { Country.EC, New(Country.EC, @"^[0-9]{6}$") },

        // EE: Estonia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.EE, New(Country.EE, @"^[0-9]{5}$") },

        // EG: Egypt, http://en.wikipedia.org/wiki/Postal_codes_in_Egypt
        { Country.EG, New(Country.EG, @"^[1-9][0-9]{4}$") },

        // ES: Spain, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Spain. Start with 01-52.
        { Country.ES, New(Country.ES, @"^((0[1-9])|([1-4][0-9])|(5[012]))[0-9]{3}$") },

        // ET: Ethiopia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.ET, New(Country.ET, @"^[0-9]{4}$") },

        // FI: Finland, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Finland
        { Country.FI, New(Country.FI, @"^[0-9]{5}$", "^(..)(...)$", "$1-$2") },

        // FK: Falkland Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.FK, New(Country.FK, @"^FIQQ1ZZ$", "^.+$", "FIQQ 1ZZ", isSingle: true) },

        // FM: Micronesia, http://en.wikipedia.org/wiki/ZIP_Code
        { Country.FM, New(Country.FM, @"^9694[1234]([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

        // FO: Faroe Islands, http://en.wikipedia.org/wiki/List_of_postal_codes_in_the_Faroe_Islands
        { Country.FO, New(Country.FO, @"^(FO)?[1-9][0-9]{2}$", "^(FO)?(...)$", "FO-$2") },

        // FR: France, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.FR, New(Country.FR, @"^([0-9]{2})(?<!00)[0-9]{3}$") },

        // GA: Gabon, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.GA, New(Country.GA, @"^[0-9]{4}$", "^(..)(..)$", "$1 $2") },

        // GB: United Kingdom, http://en.wikipedia.org/wiki/UK_postcodes
        { Country.GB, New(Country.GB, @"^[A-Z]?[A-Z][0-9][A-Z0-9]?[0-9][A-Z]{2}$", "^(.+)(...)$", "$1 $2") },

        // GE: Georgia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.GE, New(Country.GE, @"^[0-9]{4}$") },

        // GF: French Guiana, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.GF, New(Country.GF, @"^973[0-9]{2}$") },

        // GG: Guernsey, https://en.wikipedia.org/wiki/GY_postcode_area
        { Country.GG, New(Country.GG, @"^(GY)?[0-9]{2,3}[A-Z]{2}$", "^(GY)?(...?)(...)$", "GY$2 $3") },

        // GI: Gibraltar, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.GI, New(Country.GI, @"^GX111AA$", "^.+$", "GX11 1AA", isSingle: true) },

        // GL: Greenland, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Greenland
        { Country.GL, New(Country.GL, @"^(GL)?39[0-9]{2}$", "^(GL)?(....)$", "GL-$2") },

        // GP: Guadeloupe, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.GP, New(Country.GP, @"^971[0-9]{2}$") },

        // GR: Greece, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Greece
        { Country.GR, New(Country.GR, @"^[1-9][0-9]{4}$", "^(...)(..)$", "$1 $2") },

        // GS: South Georgia And The South Sandwich Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.GS, New(Country.GS, @"^SIQQ1ZZ$", "^.+$", "SIQQ 1ZZ", isSingle: true) },

        // GT: Guatemala, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.GT, New(Country.GT, @"^[0-9]{5}$") },

        // GU: Guam, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.GU, New(Country.GU, @"^969([12][0-9]|3[0-3])([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

        // GW: Guinea-Bissau, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.GW, New(Country.GW, @"^[0-9]{4}$") },

        // HM: Heard Island And Mcdonald Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.HM, New(Country.HM, @"^[0-9]{4}$") },

        // HN: Honduras, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.HN, New(Country.HN, @"^[0-9]{5}$") },

        // HR: Croatia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.HR, New(Country.HR, @"^[0-9]{5}$") },

        // HT: Haiti, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.HT, New(Country.HT, @"^[0-9]{4}$") },

        // HU: Hungary, http://en.wikipedia.org/wiki/Postal_codes_in_Hungary
        { Country.HU, New(Country.HU, @"^[1-9][0-9]{3}$") },

        // ID: Indonesia, http://en.wikipedia.org/wiki/Postal_codes_in_Indonesia
        { Country.ID, New(Country.ID, @"^[1-9][0-9]{4}$") },

        // IE: Ireland, https://en.wikipedia.org/wiki/Postal_addresses_in_the_Republic_of_Ireland
        { Country.IE, New(Country.IE, "^[A-Z][0-9][0-9|A-Z]{5E}$", "^(...)(....)$", "$1 $2") },

        // IL: Israel, http://en.wikipedia.org/wiki/Postal_codes_in_Israel
        { Country.IL, New(Country.IL, @"^[0-9]{7}$") },

        // IM: Isle Of Man, https://en.wikipedia.org/wiki/IM_postcode_area
        { Country.IM, New(Country.IM, @"^(IM)?[0-9]{2,3}[A-Z]{2}$", "^(IM)?(..?)(...)$", "IM$2 $3") },

        // IN: India, http://en.wikipedia.org/wiki/Postal_Index_Number
        { Country.IN, New(Country.IN, @"^[1-9][0-9]{5}$") },

        // IO: British Indian Ocean Territory, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.IO, New(Country.IO, @"^BBND1ZZ$", "^.+$", "BBND 1ZZ", isSingle: true) },

        // IQ: Iraq, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Iraq
        { Country.IQ, New(Country.IQ, @"^[13456][0-9]{4}$") },

        // IR: Iran, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.IR, New(Country.IR, @"^[0-9]{10}$", "^(.{5})(.{5})$", "$1-$2") },

        // IS: Iceland, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.IS, New(Country.IS, @"^[0-9]{3}$") },

        // IT: Italy, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Italy
        { Country.IT, New(Country.IT, @"^([0-9]{3})(?<!000)[0-9]{2}$") },

        // JE: Jersey, https://en.wikipedia.org/wiki/JE_postcode_area
        { Country.JE, New(Country.JE, @"^(JE)?[0-9]{2,3}[A-Z]{2}$", "^(JE)?(...?)(...)$", "JE$2 $3") },

        // JO: Jordan, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.JO, New(Country.JO, @"^[0-9]{5}$") },

        // JP: Japan, http://en.wikipedia.org/wiki/Postal_codes_in_Japan
        { Country.JP, New(Country.JP, @"^[0-9]{7}$", "^(....)(...)$", "$1-$2") },

        // KG: Kyrgyzstan, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.KG, New(Country.KG, @"^[0-9]{6}$") },

        // KH: Cambodia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.KH, New(Country.KH, @"^[0-9]{5}$") },

        // KR: Korea, http://en.wikipedia.org/wiki/List_of_postal_codes_in_South_Korea
        { Country.KR, New(Country.KR, @"^[1-7][0-9]{5}$", "^(...)(...)$", "$1-$2") },

        // KY: Cayman Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.KY, New(Country.KY, @"^(KY)?[0-9]{5}$", "^(KY)?(.)(....)$", "KY$2-$3") },

        // KZ: Kazakhstan, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.KZ, New(Country.KZ, @"^[0-9]{6}$") },

        // LA: Lao People'S Democratic Republic, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.LA, New(Country.LA, @"^[0-9]{5}$") },

        // LB: Lebanon, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.LB, New(Country.LB, @"^[0-9]{8}$", "^(....)(....)$", "$1 $2") },

        // LI: Liechtenstein, http://en.wikipedia.org/wiki/Postal_codes_in_Switzerland_and_Liechtenstein
        { Country.LI, New(Country.LI, @"^94(8[5-9]|9[0-8])$") },

        // LK: Sri Lanka, http://en.wikipedia.org/wiki/Postal_codes_in_Sri_Lanka
        { Country.LK, New(Country.LK, @"^[0-9]{5}$") },

        // LR: Liberia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.LR, New(Country.LR, @"^[0-9]{4}$") },

        // LS: Lesotho, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.LS, New(Country.LS, @"^[0-9]{3}$") },

        // LT: Lithuania, http://en.wikipedia.org/wiki/Postal_codes_in_Lithuania
        { Country.LT, New(Country.LT, @"^(LT)?[0-9]{5}$", "^(LT)?(.{5})$", "LT-$2") },

        // LU: Luxembourg, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.LU, New(Country.LU, @"^[0-9]{4}$") },

        // LV: Latvia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.LV, New(Country.LV, @"^(LV)?[0-9]{4}$", "^(LV)?(....)$", "LV-$2") },

        // LY: Libya, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.LY, New(Country.LY, @"^[0-9]{5}$") },

        // MA: Morocco, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Morocco
        { Country.MA, New(Country.MA, @"^[1-9][0-9]{4}$", "^(..)(...)$", "$1 $2") },

        // MC: Monaco, http://en.wikipedia.org/wiki/Postal_codes_in_France
        { Country.MC, New(Country.MC, @"^(MC)?980[0-9]{2}$") },

        // MD: Moldova, http://en.wikipedia.org/wiki/Postal_codes_in_Moldova
        { Country.MD, New(Country.MD, @"^(MD)?[0-9]{4}$", "^(MD)?(....)$", "MD-$2") },

        // ME: Montenegro, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Montenegro
        { Country.ME, New(Country.ME, @"^8[145][0-9]{3}$") },

        // MF: Saint Martin, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.MF, New(Country.MF, @"^978[0-9]{2}$") },

        // MG: Madagascar, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.MG, New(Country.MG, @"^[0-9]{3}$") },

        // MH: Marshall Islands, http://en.wikipedia.org/wiki/ZIP_Code
        { Country.MH, New(Country.MH, @"^969[67][0-9]([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

        // MK: Macedonia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.MK, New(Country.MK, @"^[0-9]{4}$") },

        // MM: Myanmar, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.MM, New(Country.MM, @"^[0-9]{5}$") },

        // MN: Mongolia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.MN, New(Country.MN, @"^[0-9]{5}$") },

        // MP: Northern Mariana Islands, http://en.wikipedia.org/wiki/ZIP_Code
        { Country.MP, New(Country.MP, @"^9695[012]([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

        // MQ: Martinique, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.MQ, New(Country.MQ, @"^972[0-9]{2}$") },

        // MT: Malta, http://en.wikipedia.org/wiki/Postal_codes_in_Malta
        { Country.MT, New(Country.MT, @"^[A-Z]{3}[0-9]{4}$", "^(...)(....)$", "$1 $2") },

        // MX: Mexico, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Mexico
        { Country.MX, New(Country.MX, @"^[0-9]{5}$") },

        // MY: Malaysia, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Malaysia
        { Country.MY, New(Country.MY, @"^([0-9]{2})(?<!00)[0-9]{3}$") },

        // MZ: Mozambique, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.MZ, New(Country.MZ, @"^[0-9]{4}$") },

        // NA: Namibia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.NA, New(Country.NA, @"^9[0-2][0-9]{3}$") },

        // NC: New Caledonia, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.NC, New(Country.NC, @"^988[0-9]{2}$") },

        // NE: Niger, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.NE, New(Country.NE, @"^[0-9]{4}$") },

        // NF: Norfolk Island, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.NF, New(Country.NF, @"^[0-9]{4}$") },

        // NG: Nigeria, http://en.wikipedia.org/wiki/Postal_codes_in_Nigeria
        { Country.NG, New(Country.NG, @"^[0-9]{6}$") },

        // NI: Nicaragua, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.NI, New(Country.NI, @"^[0-9]{5}$") },

        // NL: Netherlands, http://en.wikipedia.org/wiki/List_of_postal_codes_in_the_Netherlands. No SA, SD, or SS.
        { Country.NL, New(Country.NL, @"^[1-9][0-9]{3}([A-Z]{2})(?<!SS|SA|SD)$", "^(....)(..)$", "$1 $2") },

        // NO: Norway, http://en.wikipedia.org/wiki/Postal_codes_in_Norway
        { Country.NO, New(Country.NO, @"^[0-9]{4}$") },

        // NP: Nepal, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.NP, New(Country.NP, @"^[0-9]{5}$") },

        // NZ: New Zealand, http://en.wikipedia.org/wiki/Postcodes_in_New_Zealand
        { Country.NZ, New(Country.NZ, @"^[0-9]{4}$") },

        // OM: Oman, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.OM, New(Country.OM, @"^[0-9]{3}$") },

        // PA: Panama, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.PA, New(Country.PA, @"^[0-9]{6}$") },

        // PE: Peru, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Peru
        { Country.PE, New(Country.PE, @"^[0-9]{5}$") },

        // PF: French Polynesia, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.PF, New(Country.PF, @"^987[0-9]{2}$") },

        // PG: Papua New Guinea, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.PG, New(Country.PG, @"^[0-9]{3}$") },

        // PH: Philippines, http://en.wikipedia.org/wiki/List_of_ZIP_Codes_in_the_Philippines
        { Country.PH, New(Country.PH, @"^([0-9]{2})(?<!00)[0-9]{2}$") },

        // PK: Pakistan, http://en.wikipedia.org/wiki/List_of_postal_codes_of_Pakistan
        { Country.PK, New(Country.PK, @"^[1-9][0-9]{4}$") },

        // PL: Poland, // Poland. http://en.wikipedia.org/wiki/List_of_postal_codes_in_Poland
        { Country.PL, New(Country.PL, @"^[0-9]{5}$", "^(..)(...)$", "$1-$2") },

        // PM: Saint Pierre And Miquelon, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.PM, New(Country.PM, @"^97500$") },

        // PN: Pitcairn, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.PN, New(Country.PN, @"^PCRN1ZZ$", "^.+$", "PCRN 1ZZ", isSingle: true) },

        // PR: Puerto Rico, http://en.wikipedia.org/wiki/Postal_codes_in_Puerto_Rico
        { Country.PR, New(Country.PR, @"^[0-9]{5}$") },

        // PS: Palestinian Territory, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.PS, New(Country.PS, @"^[0-9]{5}$") },

        // PT: Portugal, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Portugal
        { Country.PT, New(Country.PT, @"^[1-9][0-9]{6}$", "^(....)(...)$", "$1 $2") },

        // PW: Palau, http://en.wikipedia.org/wiki/ZIP_Code
        { Country.PW, New(Country.PW, @"^96940([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

        // PY: Paraguay, http://en.wikipedia.org/wiki/Postal_codes_in_Norway
        { Country.PY, New(Country.PY, @"^[0-9]{4}$") },

        // RE: Réunion, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.RE, New(Country.RE, @"^974[0-9]{2}$") },

        // RO: Romania, http://en.wikipedia.org/wiki/Postal_codes_in_Romania
        { Country.RO, New(Country.RO, @"^([0-9]{2})(?<!00)[0-9]{4}$") },

        // RS: Serbia, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Serbia
        { Country.RS, New(Country.RS, @"^[123][0-9]{4}$") },

        // RU: Russian Federation, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Russia
        { Country.RU, New(Country.RU, @"^[1-6][0-9]{5}$") },

        // SA: Saudi Arabia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.SA, New(Country.SA, @"^[0-9]{5}([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

        // SD: Sudan, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.SD, New(Country.SD, @"^[0-9]{5}$") },

        // SE: Sweden, http://en.wikipedia.org/wiki/Postal_codes_in_Sweden
        { Country.SE, New(Country.SE, @"^[1-9][0-9]{4}$", "^(...)(..)$", "$1 $2") },

        // SG: Singapore, http://en.wikipedia.org/wiki/Postal_codes_in_Singapore
        { Country.SG, New(Country.SG, @"^[0-9]{6}$") },

        // SH: Saint Helena, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.SH, New(Country.SH, @"^STHL1ZZ$", "^.+$", "STHL 1ZZ", isSingle: true) },

        // SI: Slovenia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.SI, New(Country.SI, @"^(SI)?[0-9]{4}$", "^(SI)?(....)$", "SI-$2") },

        // SK: Slovakia, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Slovakia
        { Country.SK, New(Country.SK, @"^[0-9]{5}$", "^(...)(..)$", "$1 $2") },

        // SM: San Marino, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Italy
        { Country.SM, New(Country.SM, @"^4789[0-9]$") },

        // SN: Senegal, http://en.wikipedia.org/wiki/List_of_postal_codes. The letters CP are frequently used before the postal code. This is not a country code, but an abbreviation for "code postal" or postal code.
        { Country.SN, New(Country.SN, @"^(CP)?[0-9]{5}$", "^(..)?(.{5})$", "CP$2") },

        // SV: El Salvador, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.SV, New(Country.SV, @"^01101$", replace: "01101", isSingle: true) },

        // SZ: Swaziland, http://en.wikipedia.org/wiki/List_of_postal_codes. The letter identifies one of the country's four districts.
        { Country.SZ, New(Country.SZ, @"^[HLMS][0-9]{3}$") },

        // TC: Turks And Caicos Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.TC, New(Country.TC, @"^TKCA1ZZ$", "^.+$", "TKCA 1ZZ", isSingle: true) },

        // TD: Chad, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.TD, New(Country.TD, @"^[0-9]{5}$") },

        // TH: Thailand, http://en.wikipedia.org/wiki/Postal_codes_in_Thailand
        { Country.TH, New(Country.TH, @"^[1-9][0-9]{4}$") },

        // TJ: Tajikistan, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.TJ, New(Country.TJ, @"^[0-9]{6}$") },

        // TM: Turkmenistan, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.TM, New(Country.TM, @"^[0-9]{6}$") },

        // TN: Tunisia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.TN, New(Country.TN, @"^[0-9]{4}$") },

        // TR: Turkey, http://en.wikipedia.org/wiki/Postal_codes_in_Turkey
        { Country.TR, New(Country.TR, @"^([0-9]{2})(?<!00)[0-9]{3}$") },

        // TT: Trinidad And Tobago, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.TT, New(Country.TT, @"^[0-9]{6}$") },

        // TW: Taiwan, http://en.wikipedia.org/wiki/List_of_postal_codes_in_the_Republic_of_China
        { Country.TW, New(Country.TW, @"^[1-9][0-9]{4}$") },

        // UA: Ukraine, http://en.wikipedia.org/wiki/Postal_codes_in_Ukraine
        { Country.UA, New(Country.UA, @"^([0-9]{2})(?<!00)[0-9]{3}$") },

        // US: United States, http://en.wikipedia.org/wiki/ZIP_Code
        { Country.US, New(Country.US, @"^[0-9]{5}([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

        // UY: Uruguay, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.UY, New(Country.UY, @"^[0-9]{5}$") },

        // VA: Holy See, http://en.wikipedia.org/wiki/List_of_postal_codes_in_Italy
        { Country.VA, New(Country.VA, @"^00120$", replace: "00120", isSingle: true) },

        // VC: Saint Vincent And The Grenadines, http://www.svgpost.gov.vc/index.php?option=com_content&view=article&id=3&Itemid=7
        { Country.VC, New(Country.VC, @"^(VC)?[0-9]{4}$", "^(VC)?(....)$", "VC$2") },

        // VE: Venezuela, http://en.wikipedia.org/wiki/Caracas
        { Country.VE, New(Country.VE, @"^[0-9]{4}[A-Z]?$", "^(....)(.)$", "$1-$2") },

        // VG: Virgin Islands, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.VG, New(Country.VG, @"^(VG)?11[0-6][0-9]$", "^(VG)?(....)$", "VG$2") },

        // VI: Virgin Islands, http://en.wikipedia.org/wiki/ZIP_Code
        { Country.VI, New(Country.VI, @"^008[1-5][0-9]([0-9]{4})?$", "^(.{5})(....)$", "$1-$2") },

        // VN: Viet Nam, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.VN, New(Country.VN, @"^[0-9]{6}$") },

        // WF: Wallis And Futuna, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.WF, New(Country.WF, @"^986[0-9]{2}$") },

        // XK: Kosovo, https://en.wikipedia.org/wiki/Postal_codes_in_Kosovo
        { Country.XK, New(Country.XK, @"^[1-7][0-9]{4}$") },

        // YT: Mayotte, http://en.wikipedia.org/wiki/List_of_postal_codes_in_France
        { Country.YT, New(Country.YT, @"^976[0-9]{2}$") },

        // ZA: South Africa, http://en.wikipedia.org/wiki/Postal_codes_in_South_Africa
        { Country.ZA, New(Country.ZA, @"^([0-9]{4})(?<!0000)$") },

        // ZM: Zambia, http://en.wikipedia.org/wiki/List_of_postal_codes
        { Country.ZM, New(Country.ZM, @"^[0-9]{5}$") },
    };
}
