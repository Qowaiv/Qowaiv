# International Bank Account Number
An overview of all IBAN's supported. IBAN's for countries not included are
considered valid if their length is between 12 and 36, and if they satisfy
[ISO 7064 MOD-97-10](https://en.wikipedia.org/wiki/ISO/IEC_7064).

# Basic Bank Account Number
Abbreviated as BBAN, is a way of describing the IBAN (for a specific country).
* a: alphabetic
* c: alphanumeric 
* n: numeric

In this table, for countries with fixed characters (such as Costa Rica) those
are displayed as `[?]`. This is strictly not BBAN, but helps when generating
validator code.

## IBAN specifications
| Country                   | Chars | BBAn*           | check | IBAN Fields                               | Off. | Example                                   |
|:--------------------------|:-----:|-----------------|:-----:|:------------------------------------------|:----:|:------------------------------------------|
| Andorra                   |    24 | 8n,12c          |       | ADkk bbbb ssss cccc cccc cccc             | yes  | AD12 0001 2030 2003 5910 0100             |
| United Arab Emirates      |    23 | 3n,16n          |       | AEkk bbbc cccc cccc cccc ccc              | yes  | AE95 0210 0000 0069 3123 456              |
| Albania                   |    28 | 8n,16c          |       | ALkk bbbs sssx cccc cccc cccc cccc        | yes  | AL47 2121 1009 0000 0002 3569 8741        |
| Austria                   |    20 | 16n             |       | ATkk bbbb bccc cccc cccc                  | yes  | AT61 1904 3002 3457 3201                  |
| Azerbaijan                |    28 | 4a,20c          |       | AZkk bbbb cccc cccc cccc cccc cccc        | yes  | AZ21 NABZ 0000 0000 1370 1000 1944        |
| Bosnia and Herzegovina    |    20 | 16n             |    39 | BAkk bbbs sscc cccc ccxx                  | yes  | BA39 1290 0794 0102 8494                  |
| Belgium                   |    16 | 12n             |       | BEkk bbbc cccc ccxx                       | yes  | BE43 0689 9999 9501                       |
| Bulgaria                  |    22 | 4a,6n,8c        |       | BGkk bbbb ssss ttcc cccc cc               | yes  | BG80 BNBG 9661 1020 3456 78               |
| Bahrain                   |    22 | 4a,14c          |       | BHkk bbbb cccc cccc cccc cc               | yes  | BH29 BMAG 1299 1234 56BH 00               |
| Brazil                    |    29 | 23n,1a,1c       |       | BRkk bbbb bbbb ssss sccc cccc ccct n      | yes  | BR97 0036 0305 0000 1000 9795 493P 1      |
| Belarus                   |    28 | 4c,4n,16c       |       | BYkk bbbb aaaa cccc cccc cccc cccc        | yes  | BY13 NBRB 3600 9000 0000 2Z00 AB00        |
| Switzerland               |    21 | 5n,12c          |       | CHkk bbbb bccc cccc cccc c                | yes  | CH36 0838 7000 0010 8017 3                |
| Costa Rica                |    22 | [0],17n         |       | CRkk 0bbb cccc cccc cccc cc               | yes  | CR05 0152 0200 1026 2840 66               |
| Cyprus                    |    28 | 8n,16c          |       | CYkk bbbs ssss cccc cccc cccc cccc        | yes  | CY17 0020 0128 0000 0012 0052 7600        |
| Czech Republic            |    24 | 20n             |       | CZkk bbbb pppp ppcc cccc cccc             | yes  | CZ65 0800 0000 1920 0014 5399             |
| Germany                   |    22 | 18n             |       | DEkk bbbb bbbb cccc cccc cc               | yes  | DE68 2105 0170 0012 3456 78               |
| Denmark                   |    18 | 14n             |       | DKkk bbbb cccc cccc cx                    | yes  | DK50 0040 0440 1162 43                    |
| Dominican Republic        |    28 | 4c,20n          |       | DOkk bbbb cccc cccc cccc cccc cccc        | yes  | DO22 ACAU 0000 0000 0001 2345 6789        |
| Estonia                   |    20 | 16n             |       | EEkk bbss cccc cccc cccx                  | yes  | EE38 2200 2210 2014 5685                  |
| Egypt                     |    29 | 25n             |       | EGkk bbbb ssss cccc cccc cccc cccc c      | yes  | EG38 0019 0005 0000 0000 2631 8000 2      |
| Spain                     |    24 | 20n             |       | ESkk bbbb ssss xxcc cccc cccc             | yes  | ES91 2100 0418 4502 0005 1332             |
| Finland                   |    18 | 14n             |       | FIkk bbbb bbcc cccc cx                    | yes  | FI21 1234 5600 0007 85                    |
| Faroe Islands             |    18 | 14n             |       | FOkk bbbb cccc cccc cx                    | yes  | FO20 0040 0440 1162 43                    |
| France                    |    27 | 10n,11c,2n      |       | FRkk bbbb bsss sscc cccc cccc cxx         | yes  | FR14 2004 1010 0505 0001 3M02 606         |
| United Kingdom            |    22 | 4a,14n          |       | GBkk bbbb ssss sscc cccc cc               | yes  | GB46 BARC 2078 9863 2748 45               |
| Georgia                   |    22 | 2a,16n          |       | GEkk bbcc cccc cccc cccc cc               | yes  | GE29 NB00 0000 0101 9049 17               |
| Gibraltar                 |    23 | 4a,15c          |       | GIkk bbbb cccc cccc cccc ccc              | yes  | GI75 NWBK 0000 0000 7099 453              |
| Greenland                 |    18 | 14n             |       | GLkk bbbb cccc cccc cx                    | yes  | GL20 0040 0440 1162 43                    |
| Greece                    |    27 | 7n,16c          |       | GRkk bbbs sssc cccc cccc cccc ccc         | yes  | GR16 0110 1250 0000 0001 2300 695         |
| Guatemala                 |    28 | 4c,20c          |       | GTkk bbbb mmtt cccc cccc cccc cccc        | yes  | GT82 TRAJ 0102 0000 0012 1002 9690        |
| Croatia                   |    21 | 17n             |       | HRkk bbbb bbbc cccc cccc c                | yes  | HR12 1001 0051 8630 0016 0                |
| Hungary                   |    28 | 24n             |       | HUkk bbbs sssx cccc cccc cccc cccx        | yes  | HU42 1177 3016 1111 1018 0000 0000        |
| Ireland                   |    22 | 4a,14n          |       | IEkk qqqq bbbb bbcc cccc cc               | yes  | IE29 AIBK 9311 5212 3456 78               |
| Israel                    |    23 | 19n             |       | ILkk bbbs sscc cccc cccc ccc              | yes  | IL62 0108 0000 0009 9999 999              |
| Iraq                      |    23 | 4a,15n          |       | IQkk bbbb sssc cccc cccc ccc              | yes  | IQ98 NBIQ 8501 2345 6789 012              |
| Iceland                   |    26 | 22n             |       | ISkk bbss ttcc cccc iiii iiii ii          | yes  | IS14 0159 2600 7654 5510 7303 39          |
| Italy                     |    27 | 1a,10n,12c      |       | ITkk xbbb bbss sssc cccc cccc ccc         | yes  | IT60 X054 2811 1010 0000 0123 456         |
| Jordan                    |    30 | 4a,4n,18c       |       | JOkk bbbb ssss cccc cccc cccc cccc cc     | yes  | JO94 CBJO 0010 0000 0000 0131 0003 02     |
| Kuwait                    |    30 | 4a,22c          |       | KWkk bbbb cccc cccc cccc cccc cccc cc     | yes  | KW81 CBKU 0000 0000 0000 1234 5601 01     |
| Kazakhstan                |    20 | 3n,13c          |       | KZkk bbbc cccc cccc cccc                  | yes  | KZ75 125K ZT20 6910 0100                  |
| Lebanon                   |    28 | 4n,20c          |       | LBkk bbbb cccc cccc cccc cccc cccc        | yes  | LB30 0999 0000 0001 0019 2557 9115        |
| Saint Lucia               |    32 | 4a,24c          |       | LCkk bbbb cccc cccc cccc cccc cccc cccc   | yes  | LC55 HEMM 0001 0001 0012 0012 0002 3015   |
| Liechtenstein             |    21 | 5n,12c          |       | LIkk bbbb bccc cccc cccc c                | yes  | LI21 0881 0000 2324 013A A                |
| Lithuania                 |    20 | 16n             |       | LTkk bbbb bccc cccc cccc                  | yes  | LT12 1000 0111 0100 1000                  |
| Luxembourg                |    20 | 3n,13c          |       | LUkk bbbc cccc cccc cccc                  | yes  | LU28 0019 4006 4475 0000                  |
| Latvia                    |    21 | 4a,13c          |       | LVkk bbbb cccc cccc cccc c                | yes  | LV80 BANK 0000 4351 9500 1                |
| Libya                     |    25 | 21n             |       | LYkk bbbs sscc cccc cccc cccc c           | yes  | LY83 0020 4800 0020 1001 2036 1           |
| Monaco                    |    27 | 10n,11c,2n      |       | MCkk bbbb bsss sscc cccc cccc cxx         | yes  | MC11 1273 9000 7000 1111 1000 H79         |
| Moldova                   |    24 | 2c,18c          |       | MDkk bbcc cccc cccc cccc cccc             | yes  | MD24 AG00 0225 1000 1310 4168             |
| Montenegro                |    22 | 18n             |    25 | MEkk bbbc cccc cccc cccc xx               | yes  | ME25 5050 0001 2345 6789 51               |
| North Macedonia           |    19 | 3n,10c,2n       |    07 | MKkk bbbc cccc cccc cxx                   | yes  | MK07 2501 2000 0058 984                   |
| Mauritania                |    27 | 23n             |    13 | MRkk bbbb bsss sscc cccc cccc cxx         | yes  | MR13 0002 0001 0100 0012 3456 753         |
| Malta                     |    31 | 4a,5n,18c       |       | MTkk bbbb ssss sccc cccc cccc cccc ccc    | yes  | MT84 MALT 0110 0001 2345 MTLC AST0 01S    |
| Mauritius                 |    30 | 4a,16n,[000],3a |       | MUkk bbbb bbss cccc cccc cccc 000m mm     | yes  | MU17 BOMM 0101 1010 3030 0200 000M UR     |
| Netherlands               |    18 | 4a,10n          |       | NLkk bbbb cccc cccc cc                    | yes  | NL20 INGB 0001 2345 67                    |
| Norway                    |    15 | 11n             |       | NOkk bbbb cccc ccx                        | yes  | NO93 8601 1117 947                        |
| Pakistan                  |    24 | 4a,16c          |       | PKkk bbbb cccc cccc cccc cccc             | yes  | PK36 SCBL 0000 0011 2345 6702             |
| Poland                    |    28 | 24n             |       | PLkk bbbs sssx cccc cccc cccc cccc        | yes  | PL61 1090 1014 0000 0712 1981 2874        |
| Palestinian territories   |    29 | 4a,21c          |       | PSkk bbbb cccc cccc cccc cccc cccc c      | yes  | PS92 PALS 0000 0000 0400 1234 5670 2      |
| Portugal                  |    25 | 21n             |    50 | PTkk bbbb ssss cccc cccc cccx x           | yes  | PT50 0002 0123 1234 5678 9015 4           |
| Qatar                     |    29 | 4a,21c          |       | QAkk bbbb cccc cccc cccc cccc cccc c      | yes  | QA58 DOHB 0000 1234 5678 90AB CDEF G      |
| Romania                   |    24 | 4a,16c          |       | ROkk bbbb cccc cccc cccc cccc             | yes  | RO49 AAAA 1B31 0075 9384 0000             |
| Serbia                    |    22 | 18n             |    35 | RSkk bbbc cccc cccc cccc xx               | yes  | RS35 2600 0560 1001 6113 79               |
| Russia                    |    33 | 14n,15c         |       | RUkk bbbb bbbb bsss sscc cccc cccc cccc c | yes  | RU02 0445 2560 0407 0281 0412 3456 7890 1 |
| Saudi Arabia              |    24 | 2n,18c          |       | SAkk bbcc cccc cccc cccc cccc             | yes  | SA84 4000 0108 0540 1173 0013             |
| Seychelles                |    31 | 4a,20n,3a       |       | SCkk bbbb bb ss cccc cccc cccc cccc mmm   | yes  | SC18 SSCB 1101 0000 0000 0000 1497 USD    |
| Sudan                     |    18 | 14n             |       | SDkk bbcc cccc cccc cc                    | yes  | SD21 2901 0501 2340 01                    |
| Sweden                    |    24 | 20n             |       | SEkk bbbc cccc cccc cccc cccx             | yes  | SE35 5000 0000 0549 1000 0003             |
| Slovenia                  |    19 | 15n             |    56 | SIkk bbss sccc cccc cxx                   | yes  | SI56 1910 0000 0123 438                   |
| Slovakia                  |    24 | 20n             |       | SKkk bbbb pppp ppcc cccc cccc             | yes  | SK31 1200 0000 1987 4263 7541             |
| San Marino                |    27 | 1a,10n,12c      |       | SMkk xbbb bbss sssc cccc cccc ccc         | yes  | SM86 U032 2509 8000 0000 0270 100         |
| São Tomé and Príncipe     |    25 | 21n             |       | STkk bbbb ssss cccc cccc cccc c           | yes  | ST23 0001 0001 0051 8453 1014 6           |
| El Salvador               |    28 | 4a,20n          |       | SVkk bbbb cccc cccc cccc cccc cccc        | yes  | SV62 CENR 0000 0000 0000 0070 0025        |
| East Timor                |    23 | 19n             |    38 | TLkk bbbc cccc cccc cccc cxx              | yes  | TL38 0010 0123 4567 8910 106              |
| Tunisia                   |    24 | 20n             |    59 | TNkk bbss sccc cccc cccc ccxx             | yes  | TN59 1000 6035 1835 9847 8831             |
| Turkey                    |    26 | 5n,[0],16c      |       | TRkk bbbb b0cc cccc cccc cccc cc          | yes  | TR33 0006 1005 1978 6457 8413 26          |
| Ukraine                   |    29 | 6n,19c          |       | UAkk bbbb bbcc cccc cccc cccc cccc c      | yes  | UA21 3996 2200 0002 6007 2335 6600 1      |
| Vatican City              |    22 | 3n,15n          |       | VAkk bbbc cccc cccc cccc cc               | yes  | VA59 0011 2300 0012 3456 78               |
| Virgin Islands, British   |    24 | 4a,16n          |       | VGkk bbbb cccc cccc cccc cccc             | yes  | VG96 VPVG 0000 0123 4567 8901             |
| Kosovo                    |    20 | 4n,10n,2n       |       | XKkk bbbb cccc cccc cccc                  | yes  | XK05 1212 0123 4567 8906                  |
| Angola                    |    25 | 21n             |       | AOkk nnnn nnnn nnnn nnnn nnnn n           | no   | AO06 0044 0000 6729 5030 1010 2           |
| Burkina Faso              |    28 | 2c,22n          |       | BFkk ccnn nnnn nnnn nnnn nnnn nnnn        | no   | BF42 BF08 4010 1300 4635 7400 0390        |
| Burundi                   |    27 | 5n,5n,11n,2n    |       | BIkk nnnn nnnn nnnn nnnn nnnn nnn         | no   | BI13 2000 1100 0100 0012 3456 789         |
| Benin                     |    28 | 2c,22n          |       | BJkk ccnn nnnn nnnn nnnn nnnn nnnn        | no   | BJ66 BJ06 1010 0100 1443 9000 0769        |
| Central African Republic  |    27 | 23n             |       | CFkk nnnn nnnn nnnn nnnn nnnn nnn         | no   | CF42 2000 1000 0101 2006 9700 160         |
| Congo, Republic of the    |    27 | 23n             |       | CGkk nnnn nnnn nnnn nnnn nnnn nnn         | no   | CG39 3001 1000 1010 1345 1300 019         |
| Côte d'Ivoire             |    28 | 1a,23n          |       | CIkk annn nnnn nnnn nnnn nnnn nnnn        | no   | CI02 N259 9162 9182 8879 7488 1965        |
| Cameroon                  |    27 | 23n             |       | CMkk nnnn nnnn nnnn nnnn nnnn nnn         | no   | CM21 1000 2000 3002 7797 6315 008         |
| Cabo Verde                |    25 | 21n             |       | CVkk nnnn nnnn nnnn nnnn nnnn n           | no   | CV64 0005 0000 0020 1082 1514 4           |
| Djibouti                  |    27 | 23n             |       | DJkk nnnn nnnn nnnn nnnn nnnn nnn         | no   | DJ21 1000 2010 0104 0994 3020 008         |
| Algeria                   |    26 | 22n             |       | DZkk nnnn nnnn nnnn nnnn nnnn nn          | no   | DZ58 0002 1000 0111 3000 0005 70          |
| Gabon                     |    27 | 23n             |       | GAkk nnnn nnnn nnnn nnnn nnnn nnn         | no   | GA21 4002 1010 0320 0189 0020 126         |
| Equatorial Guinea         |    27 | 23n             |       | GQkk nnnn nnnn nnnn nnnn nnnn nnn         | no   | GQ70 5000 2001 0037 1522 8190 196         |
| Guinea-Bissau             |    25 | 2c,19n          |       | GWkk ccnn nnnn nnnn nnnn nnnn n           | no   | GW04 GW14 3001 0181 8006 3760 1           |
| Honduras                  |    28 | 4a,20n          |       | HNkk aaaa nnnn nnnn nnnn nnnn nnnn        | no   | HN54 PISA 0000 0000 0000 0012 3124        |
| Iran                      |    26 | [0],2n,[0],18n  |       | IRkk 0nn0 nnnn nnnn nnnn nnnn nn          | no   | IR58 0540 1051 8002 1273 1130 07          |
| Comoros                   |    27 | 23n             |       | KMkk nnnn nnnn nnnn nnnn nnnn nnn         | no   | KM46 0000 5000 0100 1090 4400 137         |
| Morocco                   |    28 | 24n             |       | MAkk nnnn nnnn nnnn nnnn nnnn nnnn        | no   | MA64 0115 1900 0001 2050 0053 4921        |
| Madagascar                |    27 | 23n             |       | MGkk nnnn nnnn nnnn nnnn nnnn nnn         | no   | MG46 0000 5030 0712 8942 1016 045         |
| Mali                      |    28 | 2c,22n          |       | MLkk annn nnnn nnnn nnnn nnnn nnnn        | no   | ML13 ML01 6012 0102 6001 0066 8497        |
| Mozambique                |    25 | 21n             |       | MZkk nnnn nnnn nnnn nnnn nnnn n           | no   | MZ97 1234 1234 1234 1234 1234 1           |
| Niger                     |    28 | 2a,22n          |       | NEkk aann nnnn nnnn nnnn nnnn nnnn        | no   | NE58 NE03 8010 0100 1303 0500 0268        |
| Nicaragua                 |    32 | 4a,24n          |       | NIkk aaaa nnnn nnnn nnnn nnnn nnnn nnnn   | no   | NI92 BAMC 0000 0000 0000 0000 03123 123   |
| Senegal                   |    28 | 2a,22n          |       | SNkk aann nnnn nnnn nnnn nnnn nnnn        | no   | SN05 TI80 0835 4151 5881 3959 8706        |
| Chad                      |    27 | 23n             |       | TDkk nnnn nnnn nnnn nnnn nnnn nnn         | no   | TD89 6000 2000 0102 7109 1600 153         |
| Togo                      |    28 | 2a,22n          |       | TGkk aann nnnn nnnn nnnn nnnn nnnn        | no   | TG53 TG00 9060 4310 3465 0040 0070        |

Source: [Wikipedia](https://en.wikipedia.org/wiki/International_Bank_Account_Number)
