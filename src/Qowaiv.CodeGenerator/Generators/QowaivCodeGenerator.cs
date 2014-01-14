using ExcelLibrary.SpreadSheet;
using Qowaiv.CodeGenerator.Xml;
using System;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Qowaiv.CodeGenerator.Generators
{
    /// <summary>Generator for resource files.</summary>
    public class QowaivCodeGenerator
    {
        /// <summary>Constructor.</summary>
        public QowaivCodeGenerator() { }

        /// <summary>Generates the resource files.</summary>
        /// <param name="dir">
        /// The directory to write to.
        /// </param>
        public void Generate(DirectoryInfo dir)
        {
            if (!dir.Exists) { dir.Create(); }
            GenerateGender(dir);
            GenerateCountry(dir);
            GenerateIban(dir);
        }

        /// <summary>Generates the gender resource file.</summary>
        protected void GenerateGender(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.Gender.xls"))
            {
                var workbook = Workbook.Load(stream);
                var worksheet =workbook.Worksheets[0];

                var key_index = 1;
                var val_index = 2;
                var cmt_index = 3;

                var resx = new XResourceFile();
                
                var header = worksheet.Cells.GetRow(0);

                int i = 1;

                while(true)
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
                    resx_lng.Save(new FileInfo(Path.Combine(dir.FullName, "GenderLabels." + culture + ".resx")));
                }
            }
        }

        /// <summary>Generates the country info resource file.</summary>
        protected void GenerateCountry(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.Country.xls"))
            {
                using (var writer = new StreamWriter(Path.Combine(dir.FullName, "CountryConstants.cs")))
                {
                    writer.WriteLine("namespace Qowaiv\r\n{\r\n    public partial struct Country\r\n    {");

                    var workbook = Workbook.Load(stream);
                    var worksheet = workbook.Worksheets[0];

                    var all = new List<string>();

                    var key_index = 1;
                    var num_index = 2;
                    var is2_index = 3;
                    var is3_index = 4;
                    //var win_index = 5;
                    var str_index = 6;
                    var end_index = 7;
                    var tel_index = 8;
                    var reg_index = 9;
                    //var nat_index = 10;
                    var def_index = 11;

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
                        DateTime? end = String.IsNullOrEmpty(row.GetCell(end_index).StringValue) ? (DateTime?)null: (DateTime?)row.GetCell(end_index).DateTimeValue;
                        var tel = row.GetCell(tel_index).StringValue.Trim();
                        var display = row.GetCell(def_index).StringValue.Trim();
                        var hasRegInfo = XmlConvert.ToBoolean(row.GetCell(reg_index).StringValue);

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
                        resx.Data.Add(new XResourceFileData(key + "StartDate", start.ToString("yyyy-MM-dd")));

                        if (end.HasValue)
                        {
                            resx.Data.Add(new XResourceFileData(key + "EndDate", end.Value.ToString("yyyy-MM-dd")));
                        }
                        if (!String.IsNullOrEmpty(tel))
                        {
                            resx.Data.Add(new XResourceFileData(key + "CallingCode", tel));
                        }
                        if (hasRegInfo)
                        {
                            resx.Data.Add(new XResourceFileData(key + "RegionInfoExists", true.ToString()));
                        }
                    }

                    resx.Data.Add(new XResourceFileData("All", String.Join(";", all)));

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
                        resx_lng.Save(new FileInfo(Path.Combine(dir.FullName, "CountryLabels." + culture + ".resx")));
                    }
                    writer.WriteLine("    }\r\n}");
                }
            }
        }

        /// <summary>Generates the IBAN pattern file.</summary>
        protected void GenerateIban(DirectoryInfo dir)
        {
            using (var stream = GetType().Assembly
                .GetManifestResourceStream("Qowaiv.CodeGenerator.Resources.IBAN.xls"))
            {
                using (var writer = new StreamWriter(Path.Combine(dir.FullName, "InternationalBankAccountNumberPatterns.cs")))
                {
                    writer.WriteLine("using System.Collections.Generic;");
                    writer.WriteLine("using System.Text.RegularExpressions;");
                    writer.WriteLine();
                    writer.WriteLine("namespace Qowaiv\r\n{\r\n    public partial struct InternationalBankAccountNumber\r\n    {");
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

                    var all = new List<string>();

                    var key_index = 0;
                    var ctr_index = 1;
                    var len_index = 2;
                    var bbn_index = 3;
                    var fld_index = 4;
                    var cmd_index = 5;
                    var chk_index = 6;

                    var header = worksheet.Cells.GetRow(0);

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
                        var comment = String.Join(", ", row.GetCell(cmd_index).StringValue.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));
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
            string pattern = '^'+country;

            var blocks = bban.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!String.IsNullOrEmpty(checksum))
            {
                pattern += checksum.Length == 1 ? '0' + checksum : checksum;
            }
            // if no fixed checksum is specified and the pattern does not start with a numeric char.
            else if (blocks[0].Last() != 'n')
            {
                pattern += @"\d\d";
            }
            

            foreach (var block in blocks)
            {
                var tp = block.Last();
                var len = Int32.Parse(block.Substring(0, block.Length - 1));

                // if the first block and numeric, then add 2 digits for the checksum.
                if (tp == 'n' && pattern.Length == 3)
                {
                    len += 2;
                }

                string p;

                switch (tp)
                {
                    // numeric
                    case 'n': p = @"\d"; break;
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
                    pattern += p;
                }
                else if (len == 2 && tp == 'n')
                {
                    pattern += p + p;
                }
                else
                {
                    pattern += p + '{' + len.ToString() + '}';
                }
            }

            pattern+='$';
            return pattern;
        }
    }
}
