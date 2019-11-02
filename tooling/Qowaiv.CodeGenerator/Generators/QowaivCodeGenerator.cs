using ExcelLibrary.SpreadSheet;
using Qowaiv.CodeGenerator.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Qowaiv.CodeGenerator.Generators
{
    /// <summary>Generator for resource files.</summary>
    public class QowaivCodeGenerator
    {
        private const string Resx = ".resx";
        private const string DateFromat = "yyyy-MM-dd";

        /// <summary>Generates the resource files.</summary>
        /// <param name="dir">
        /// The directory to write to.
        /// </param>
        public void Generate(DirectoryInfo dir)
        {
            Guard.NotNull(dir, nameof(dir));

            if (!dir.Exists) { dir.Create(); }

            var fin = new DirectoryInfo(Path.Combine(dir.FullName, "Financial"));
            if (!fin.Exists) { fin.Create(); }

            var glo = new DirectoryInfo(Path.Combine(dir.FullName, "Globalization"));
            if (!glo.Exists) { glo.Create(); }

            GenerateGender(dir);
            GenerateCountry(glo);
            GenerateCurrency(fin);
            GenerateCountryToCurrency(glo);
            GenerateIban(fin);
            GeneratePostalCode(glo);
            GenerateUnknown(dir);
            GenerateInternetMediaType(dir);
        }

        /// <summary>Generates the gender resource file.</summary>
        private void GenerateGender(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.Gender.xls"))
            {
                var workbook = Workbook.Load(stream);
                var worksheet = workbook.Worksheets[0];

                const int key_index = 1;
                const int val_index = 2;
                const int cmt_index = 3;

                var resx = new XResourceFile();

                var header = worksheet.Cells.GetRow(0);

                int i = 1;

                while (true)
                {
                    var row = worksheet.Cells.GetRow(i++);

                    if (row.LastColIndex == int.MinValue) { break; }

                    resx.Data.Add(new XResourceFileData(
                        row.GetCell(key_index).StringValue,
                        row.GetCell(val_index).StringValue,
                        row.GetCell(cmt_index).StringValue));
                }

                resx.Save(new FileInfo(Path.Combine(dir.FullName, "GenderLabels.resx")));

                for (int lng_index = cmt_index + 1; lng_index <= header.LastColIndex; lng_index++)
                {
                    var resx_lng = new XResourceFile();

                    var culture = header.GetCell(lng_index).StringValue;

                    i = 1;

                    while (true)
                    {
                        var row = worksheet.Cells.GetRow(i++);
                        if (row.LastColIndex == int.MinValue) { break; }

                        var key = row.GetCell(key_index).StringValue;
                        var val = row.GetCell(lng_index).StringValue;
                        var cmd = row.GetCell(cmt_index).StringValue;
                        var def = resx[key].Value;

                        if (!string.IsNullOrEmpty(val) && def != val)
                        {
                            resx_lng.Data.Add(new XResourceFileData(key, val, cmd));
                        }
                    }
                    resx_lng.Save(new FileInfo(Path.Combine(dir.FullName, "GenderLabels." + culture + Resx)));
                }
            }
        }

        /// <summary>Generates the country info resource file.</summary>
        private void GenerateCountry(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.Country.xls"))
            {
                using (var writer = new StreamWriter(Path.Combine(dir.FullName, "CountryConstants.cs")))
                {
                    writer.WriteLine("namespace Qowaiv.Globalization\r\n{\r\n    public partial struct Country\r\n    {");

                    var workbook = Workbook.Load(stream);
                    var worksheet = workbook.Worksheets[0];

                    var all = new List<string>();

                    const int key_index = 1;
                    const int num_index = 2;
                    const int is2_index = 3;
                    const int is3_index = 4;
                    // const int win_index = 5
                    const int str_index = 6;
                    const int end_index = 7;
                    const int tel_index = 8;
                    // const int  nat_index = 9
                    const int def_index = 10;

                    var resx = new XResourceFile();

                    var header = worksheet.Cells.GetRow(0);

                    int i = 1;

                    while (true)
                    {
                        var row = worksheet.Cells.GetRow(i++);

                        if (row.LastColIndex == int.MinValue) { break; }

                        var key = row.GetCell(key_index).StringValue.Trim();

                        var num = row.GetCell(num_index).StringValue.Trim();
                        var iso2 = row.GetCell(is2_index).StringValue.Trim();
                        var iso3 = row.GetCell(is3_index).StringValue.Trim();
                        var start = row.GetCell(str_index).DateTimeValue;
                        DateTime? end = string.IsNullOrEmpty(row.GetCell(end_index).StringValue) ? (DateTime?)null : row.GetCell(end_index).DateTimeValue;
                        var tel = row.GetCell(tel_index).StringValue.Trim();
                        var display = row.GetCell(def_index).StringValue.Trim();

                        if (key != "ZZ")
                        {
                            all.Add(key);

                            writer.WriteLine("        /// <summary>Describes the country {0} ({1}).</summary>", display, key);
                            if (end.HasValue)
                            {
                                writer.WriteLine("        /// <remarks>End date is {0:yyyy-MM-dd}.</remarks>", end.Value);
                            }
                            writer.Write("        public static readonly Country {0} = new Country()", key);
                            writer.Write(" { m_Value = \"");
                            writer.Write(key);
                            writer.WriteLine("\" };");
                            writer.WriteLine();
                        }
                        key += '_';

                        resx.Data.Add(new XResourceFileData(key + "DisplayName", display));
                        resx.Data.Add(new XResourceFileData(key + "ISO", num));
                        resx.Data.Add(new XResourceFileData(key + "ISO2", iso2));
                        resx.Data.Add(new XResourceFileData(key + "ISO3", iso3));
                        resx.Data.Add(new XResourceFileData(key + "StartDate", start.ToString(DateFromat)));

                        if (end.HasValue)
                        {
                            resx.Data.Add(new XResourceFileData(key + "EndDate", end.Value.ToString(DateFromat)));
                        }
                        if (!string.IsNullOrEmpty(tel))
                        {
                            resx.Data.Add(new XResourceFileData(key + "CallingCode", tel));
                        }
                    }

                    resx.Data.Add(new XResourceFileData("All", string.Join(";", all)));

                    resx.Save(new FileInfo(Path.Combine(dir.FullName, "CountryLabels.resx")));

                    for (int lng_index = def_index + 1; lng_index <= header.LastColIndex; lng_index++)
                    {
                        var resx_lng = new XResourceFile();

                        var culture = header.GetCell(lng_index).StringValue;

                        i = 1;

                        while (true)
                        {
                            var row = worksheet.Cells.GetRow(i++);
                            if (row.LastColIndex == int.MinValue) { break; }

                            var key = row.GetCell(key_index).StringValue + "_DisplayName";
                            var val = row.GetCell(lng_index).StringValue.Trim();
                            var cmd = string.Format("{0} ({1})", row.GetCell(def_index).StringValue, row.GetCell(is2_index).StringValue);
                            var def = resx[key].Value;

                            if (!string.IsNullOrEmpty(val) && def != val)
                            {
                                resx_lng.Data.Add(new XResourceFileData(key, val, cmd));
                            }
                        }
                        resx_lng.Save(new FileInfo(Path.Combine(dir.FullName, "CountryLabels." + culture + Resx)));
                    }
                    writer.WriteLine("    }\r\n}");
                }
            }
        }

        /// <summary>Generates the country info resource file.</summary>
        private void GenerateCountryToCurrency(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.CountryToCurrency.xls"))
            {
                using (var writer = new StreamWriter(Path.Combine(dir.FullName, "CountryToCurrencyMappings.cs")))
                {
                    writer.WriteLine("using Qowaiv.Globalization;");
                    writer.WriteLine("using System.Collections.Generic;");
                    writer.WriteLine("using System.Collections.ObjectModel;");
                    writer.WriteLine();

                    writer.WriteLine("namespace Qowaiv.Financial\r\n{\r\n\tinternal partial struct CountryToCurrency\r\n    {");

                    writer.WriteLine("        public static readonly ReadOnlyCollection<CountryToCurrency> All = new ReadOnlyCollection<CountryToCurrency>(new List<CountryToCurrency>()\r\n        {");

                    var workbook = Workbook.Load(stream);
                    var worksheet = workbook.Worksheets[0];

                    const int country_index = 1;
                    const int currency_index = 2;
                    const int date_index = 3;

                    int i = 1;

                    while (true)
                    {
                        var row = worksheet.Cells.GetRow(i++);

                        if (row.LastColIndex == int.MinValue) { break; }

                        var country = row.GetCell(country_index).StringValue.Trim();
                        var cur = row.GetCell(currency_index).StringValue.Trim();
                        var start = (Date)row.GetCell(date_index).DateTimeValue;

                        if (start == Date.MinValue)
                        {
                            writer.WriteLine($"{new string(' ', 12)}new CountryToCurrency(Country.{country}, Currency.{cur}),");
                        }
                        else
                        {
                            writer.WriteLine($"{new string(' ', 12)}new CountryToCurrency(Country.{country}, Currency.{cur}, new Date({start:yyyy, MM, dd})),");
                        }

                    }
                    writer.WriteLine("        });\r\n    }\r\n}");
                }
            }
        }

        /// <summary>Generates the country info resource file.</summary>
        private void GenerateCurrency(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.Currency.xls"))
            {
                using (var writer = new StreamWriter(Path.Combine(dir.FullName, "CurrencyConstants.cs")))
                {
                    writer.WriteLine("namespace Qowaiv.Financial\r\n{\r\n    public partial struct Currency\r\n    {");

                    var workbook = Workbook.Load(stream);
                    var worksheet = workbook.Worksheets[0];

                    var all = new List<string>();

                    const int key_index = 1;
                    const int iso_index = 2;
                    const int num_index = 3;
                    const int dig_index = 4;
                    const int sym_index = 5;
                    const int str_index = 6;
                    const int end_index = 7;
                    const int def_index = 8;

                    var resx = new XResourceFile();

                    var header = worksheet.Cells.GetRow(0);

                    int i = 1;

                    while (true)
                    {
                        var row = worksheet.Cells.GetRow(i++);

                        if (row.LastColIndex == int.MinValue) { break; }

                        var key = row.GetCell(key_index).StringValue.Trim();

                        var iso = row.GetCell(iso_index).StringValue.Trim();
                        int num = string.IsNullOrEmpty(row.GetCell(num_index).StringValue) ? 0 : (int)(double)row.GetCell(num_index).Value;
                        int dig = string.IsNullOrEmpty(row.GetCell(dig_index).StringValue) ? 0 : (int)(double)row.GetCell(dig_index).Value;
                        var start = row.GetCell(str_index).DateTimeValue;
                        DateTime? end = string.IsNullOrEmpty(row.GetCell(end_index).StringValue) ? (DateTime?)null : (DateTime?)row.GetCell(end_index).DateTimeValue;
                        var sym = row.GetCell(sym_index).StringValue.Trim();
                        var display = row.GetCell(def_index).StringValue.Trim();

                        if (key != "ZZZ")
                        {
                            all.Add(key);

                            writer.WriteLine("        /// <summary>Describes the currency {0} ({1}).</summary>", display, key);
                            if (end.HasValue)
                            {
                                writer.WriteLine("        /// <remarks>End date is {0:yyyy-MM-dd}.</remarks>", end.Value);
                            }
                            writer.Write("        public static readonly Currency {0} = new Currency()", key);
                            writer.Write(" { m_Value = \"");
                            writer.Write(key);
                            writer.WriteLine("\" };");
                            writer.WriteLine();
                        }
                        key += '_';

                        resx.Data.Add(new XResourceFileData(key + "DisplayName", display));
                        resx.Data.Add(new XResourceFileData(key + "Num", num.ToString("000")));
                        resx.Data.Add(new XResourceFileData(key + "ISO", iso));
                        resx.Data.Add(new XResourceFileData(key + "Digits", dig.ToString()));

                        resx.Data.Add(new XResourceFileData(key + "StartDate", start.ToString(DateFromat)));

                        if (!string.IsNullOrEmpty(sym))
                        {
                            resx.Data.Add(new XResourceFileData(key + "Symbol", sym));
                        }

                        if (end.HasValue)
                        {
                            resx.Data.Add(new XResourceFileData(key + "EndDate", end.Value.ToString(DateFromat)));
                        }
                    }

                    resx.Data.Add(new XResourceFileData("All", string.Join(";", all)));

                    resx.Save(new FileInfo(Path.Combine(dir.FullName, "CurrencyLabels.resx")));

                    for (int lng_index = def_index + 1; lng_index <= header.LastColIndex; lng_index++)
                    {
                        var resx_lng = new XResourceFile();

                        var culture = header.GetCell(lng_index).StringValue;

                        i = 1;

                        while (true)
                        {
                            var row = worksheet.Cells.GetRow(i++);
                            if (row.LastColIndex == int.MinValue) { break; }

                            var key = row.GetCell(key_index).StringValue + "_DisplayName";
                            var val = row.GetCell(lng_index).StringValue.Trim();
                            var cmd = string.Format("{0} ({1})", row.GetCell(def_index).StringValue, row.GetCell(iso_index).StringValue);
                            var def = resx[key].Value;

                            if (!string.IsNullOrEmpty(val) && def != val)
                            {
                                resx_lng.Data.Add(new XResourceFileData(key, val, cmd));
                            }
                        }
                        resx_lng.Save(new FileInfo(Path.Combine(dir.FullName, "CurrencyLabels." + culture + Resx)));
                    }
                    writer.WriteLine("    }\r\n}");
                }
            }
        }

        /// <summary>Generates the IBAN pattern file.</summary>
        private void GenerateIban(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.IBAN.xls"))
            {
                using (var writer = new StreamWriter(Path.Combine(dir.FullName, "InternationalBankAccountNumberPatterns.cs")))
                {
                    writer.WriteLine("using Qowaiv.Financial;");
                    writer.WriteLine("using System.Collections.Generic;");
                    writer.WriteLine("using System.Text.RegularExpressions;");
                    writer.WriteLine();
                    writer.WriteLine("namespace Qowaiv.Financial\r\n{\r\n    public partial struct InternationalBankAccountNumber\r\n    {");
                    writer.WriteLine("        /// <summary>Gets the localized patterns.</summary>");
                    writer.WriteLine("        /// <remarks>");
                    writer.WriteLine("        /// See http://en.wikipedia.org/wiki/International_Bank_Account_Number.");
                    writer.WriteLine("        /// </remarks>");
                    writer.WriteLine("        private static readonly Dictionary<Country, Regex> LocalizedPatterns = new Dictionary<Country, Regex>()");
                    writer.WriteLine("        {");

                    //    // United Arab Emirates, Length: 23, BBAN: 3n,16n, Format: AEkk BBBC CCCC CCCC CCCC CCC (B = National bank code, C = Account Number, Effective 15 October 2011)
                    //    { Country.AE, @"^AE\d\d[0-9]{3}[0-9]{16}$" },

                    var workbook = Workbook.Load(stream);
                    var worksheet = workbook.Worksheets[0];

                    const int key_index = 0;
                    const int ctr_index = 1;
                    const int len_index = 2;
                    const int bbn_index = 3;
                    const int fld_index = 4;
                    const int cmd_index = 5;
                    const int chk_index = 6;
                    int i = 1;

                    while (true)
                    {
                        var row = worksheet.Cells.GetRow(i++);

                        if (row.LastColIndex == int.MinValue) { break; }

                        var key = row.GetCell(key_index).StringValue.Trim();
                        var country = row.GetCell(ctr_index).StringValue.Trim();
                        var len = (Int32)(Double)row.GetCell(len_index).Value;
                        var bban = row.GetCell(bbn_index).StringValue.Trim();
                        var fields = row.GetCell(fld_index).StringValue.Trim();
                        var comment = string.Join(", ", row.GetCell(cmd_index).StringValue.Split(new [] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));
                        var checksum = row.GetCell(chk_index).StringValue.Trim();

                        if (key != fields.Substring(0, 2)) { throw new NotSupportedException("Invalid key specified."); }

                        var pattern = GetIbanPattern(key, bban, checksum);

                        writer.WriteLine(
"            // {0}, Length: {1}, BBAN: {2}, Fields: {3} ({4})",
                            country,
                            len,
                            bban,
                            fields,
                            comment);
                        writer.WriteLine(
"            { Country." + key + ", new Regex(@\"" + pattern + "\", RegexOptions.Compiled) },");

                        writer.WriteLine();
                    }
                    writer.WriteLine("        };");
                    writer.WriteLine("    }");
                    writer.WriteLine("}");
                }
            }
        }
        private static string GetIbanPattern(string country, string bban, string checksum)
        {
            var pattern = new StringBuilder().Append('^').Append(country);

            var blocks = bban.Split(new [] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!string.IsNullOrEmpty(checksum))
            {
                pattern.Append(checksum.Length == 1 ? '0' + checksum : checksum);
            }
            // if no fixed checksum is specified and the pattern does not start with a numeric char.
            else if (blocks[0].Last() != 'n')
            {
                pattern.Append(@"[0-9]{2}");
            }


            foreach (var block in blocks)
            {
                var tp = block.Last();
                var len = int.Parse(block.Substring(0, block.Length - 1));

                // if the first block and numeric, then add 2 digits for the checksum.
                if (tp == 'n' && pattern.Length == 3)
                {
                    len += 2;
                }

                string p;

                switch (tp)
                {
                    // numeric
                    case 'n': p = @"[0-9]"; break;
                    // alphanumeric
                    case 'a': p = "[A-Z]"; break;
                    // both numeric and alphanumeric
                    case 'c': p = "[0-9A-Z]"; break;
                    default:
                        throw new NotSupportedException("Type '" + tp + "' is not supported.");
                }
                // add length
                if (len == 1)
                {
                    pattern.Append(p);
                }
                else if (len == 2 && tp == 'n')
                {
                    pattern.Append(p).Append(p);
                }
                else
                {
                    pattern.Append(p).Append('{').Append(len).Append('}');
                }
            }

            pattern.Append('$');
            return pattern.ToString();
        }

        /// <summary>Generates the PostalCode Settings file.</summary>
        private void GeneratePostalCode(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.PostalCode.xls"))
            {
                var file = new FileInfo(Path.Combine(dir.FullName, "Globalization", "PostalCodeCountryInfoInstances.cs"));
                if (!file.Directory.Exists) { file.Directory.Create(); }

                using (var settingswriter = new StreamWriter(file.FullName))
                {
                    settingswriter.WriteLine("using System.Collections.Generic;");
                    settingswriter.WriteLine();
                    settingswriter.WriteLine("namespace Qowaiv.Globalization\r\n{\r\n    public partial class PostalCodeCountryInfo\r\n    {");
                    settingswriter.WriteLine("        /// <summary>Gets the country based settings.</summary>");
                    settingswriter.WriteLine("        private static readonly Dictionary<Country, PostalCodeCountryInfo> Instances = new Dictionary<Country, PostalCodeCountryInfo>()");
                    settingswriter.WriteLine("        {");

                    using (var testswriter = new StreamWriter(Path.Combine(dir.FullName, "PostalCodeCountryInfoTest.cs")))
                    {
                        testswriter.WriteLine("using NUnit.Framework;");
                        testswriter.WriteLine();
                        testswriter.WriteLine("namespace Qowaiv.UnitTests\r\n{\r\n    public partial class PostalCodeTest\r\n    {");

                        var workbook = Workbook.Load(stream);
                        var worksheet = workbook.Worksheets[0];

                        const int key_index = 1;
                        const int name_index = 2;
                        const int exists_index = 3;
                        const int single_index = 4;
                        const int prefix_index = 5;
                        const int pattern_index = 6;
                        const int search_index = 7;
                        const int replace_index = 8;
                        const int lenghts_index = 9;
                        const int alpha_index = 10;
                        const int comment_index = 11;
                        int i = 1;

                        while (true)
                        {
                            var row = worksheet.Cells.GetRow(i++);

                            if (row.LastColIndex == int.MinValue) { break; }

                            var key = row.GetCell(key_index).StringValue.Trim();
                            var name = row.GetCell(name_index).StringValue.Trim();

                            var exists = (bool)row.GetCell(exists_index).Value;
                            var single = (bool)row.GetCell(single_index).Value;
                            var prefix = (bool)row.GetCell(prefix_index).Value;

                            var pattern = row.GetCell(pattern_index).StringValue.Trim();
                            var search = row.GetCell(search_index).StringValue.Trim();
                            var replace = row.GetCell(replace_index).StringValue.Trim();

                            var lenghts = string.Join(", ", row.GetCell(lenghts_index).StringValue.Trim().Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                            var alpha = (bool)row.GetCell(alpha_index).Value;

                            var comment = row.GetCell(comment_index).StringValue.Trim();
                            if (exists)
                            {
                                if (!string.IsNullOrEmpty(search) && prefix && !single)
                                {
                                    search = "(" + key + ")?" + search;
                                }

                                var settings = string.Format("@\"{0}\"", GetPostalCodePattern(pattern, key, prefix));
                                if (!string.IsNullOrEmpty(search))
                                {
                                    settings += string.Format(", \"^{0}$\", \"{1}\"", search, replace);
                                }
                                else if (!string.IsNullOrEmpty(replace))
                                {
                                    settings += string.Format(", null, \"{0}\"", replace);
                                }
                                if (single)
                                {
                                    settings += ", isSingle:true";
                                }

                                // Comment
                                settingswriter.WriteLine("            // {0}: {1}, {2}", key, name, comment);
                                // Dictionary entry
                                settingswriter.WriteLine("            { Country." + key + ", New(Country." + key + "," + settings + ") },");
                                settingswriter.WriteLine();


                                // Comment
                                testswriter.WriteLine("        /// <summary>Tests patterns that should not be valid for {0} ({1}).</summary>", name, key);
                                // Method declaration
                                testswriter.WriteLine("        [Test]\r\n        public void IsNotValid_{0}_All()", key);
                                testswriter.WriteLine("        {");
                                // Method Logic
                                testswriter.WriteLine("            IsNotValid(Country.{0}, {1}, {2}, {3});", key, alpha.ToString().ToLowerInvariant(), prefix.ToString().ToLowerInvariant(), lenghts);
                                testswriter.WriteLine("        }");
                                testswriter.WriteLine();
                            }
                        }
                        testswriter.WriteLine("    }");
                        testswriter.WriteLine("}");
                    }
                    settingswriter.WriteLine("        };");
                    settingswriter.WriteLine("    }");
                    settingswriter.WriteLine("}");
                }
            }
        }
        private static string GetPostalCodePattern(string pattern, string country, bool prefix)
        {
            if (prefix)
            {
                pattern = '(' + country + ")?" + pattern;
            }

            if (pattern[0] != '^') { pattern = '^' + pattern; }
            if (pattern.Last() != '$') { pattern += '$'; }

            return pattern;
        }

        /// <summary>Generates the gender resource file.</summary>
        private void GenerateUnknown(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.Unknown.xls"))
            {
                var workbook = Workbook.Load(stream);
                var worksheet = workbook.Worksheets[0];

                const int key_index = 0;
                const int val_index = 1;
                const int cmt_index = 2;

                var resx = new XResourceFile();

                var header = worksheet.Cells.GetRow(0);

                int i = 1;

                while (true)
                {
                    var row = worksheet.Cells.GetRow(i++);

                    if (row.LastColIndex == int.MinValue) { break; }

                    resx.Data.Add(new XResourceFileData(
                        row.GetCell(key_index).StringValue,
                        row.GetCell(val_index).StringValue,
                        row.GetCell(cmt_index).StringValue));
                }

                resx.Save(new FileInfo(Path.Combine(dir.FullName, "UnknownLabels.resx")));

                for (int lng_index = cmt_index + 1; lng_index <= header.LastColIndex; lng_index++)
                {
                    var resx_lng = new XResourceFile();

                    var culture = header.GetCell(lng_index).StringValue;

                    i = 1;

                    while (true)
                    {
                        var row = worksheet.Cells.GetRow(i++);
                        if (row.LastColIndex == int.MinValue) { break; }

                        var key = row.GetCell(key_index).StringValue;
                        var val = row.GetCell(lng_index).StringValue;
                        var cmd = row.GetCell(cmt_index).StringValue;
                        var def = resx[key].Value;

                        if (!string.IsNullOrEmpty(val) && def != val)
                        {
                            resx_lng.Data.Add(new XResourceFileData(key, val, cmd));
                        }
                    }
                    resx_lng.Save(new FileInfo(Path.Combine(dir.FullName, "UnknownLabels." + culture + Resx)));
                }
            }
        }

        private void GenerateInternetMediaType(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.Web.InternetMediaType.xls"))
            {
                var workbook = Workbook.Load(stream);
                var worksheet = workbook.Worksheets[0];

                var resx = new XResourceFile();

                const int key_index = 0;
                const int val_index = 1;
                const int cmt_index = 2;
                int i = 1;

                while (true)
                {
                    var row = worksheet.Cells.GetRow(i++);

                    if (row.LastColIndex == int.MinValue) { break; }

                    resx.Data.Add(new XResourceFileData(
                        row.GetCell(key_index).StringValue,
                        row.GetCell(val_index).StringValue,
                        row.GetCell(cmt_index).StringValue));
                }

                resx.Save(new FileInfo(Path.Combine(dir.FullName, "InternetMediaType.FromFile.resx")));
            }
        }
    }
}
