using AwesomeAssertions;
using Qowaiv;
using Qowaiv.CodeGeneration;
using Qowaiv.IO;
using Qowaiv.SvoGenerator;
using System.IO;
using System.Text;

namespace Qowaiv_SVO_definitions;

[Explicit]
public class Generation_of
{
    private static readonly string Root = @"c:\code\Qowaiv";

    [Test]
    [Arguments("Date", typeof(DateTime), "date", "Qowaiv", SvoFeatures.Continuous ^ SvoFeatures.Field)]
    [Arguments("DateSpan", typeof(ulong), "date span", "Qowaiv", SvoFeatures.Continuous ^ SvoFeatures.EqualsSvo)]
    [Arguments("EmailAddress", typeof(string), "email address", "Qowaiv", SvoFeatures.Default)]
    [Arguments("HouseNumber", typeof(int), "house number", "Qowaiv", SvoFeatures.Default | SvoFeatures.ComparisonOperators)]
    [Arguments("LocalDateTime", typeof(DateTime), "local date time", "Qowaiv", SvoFeatures.Continuous ^ SvoFeatures.Field)]
    [Arguments("Month", typeof(byte), "month", "Qowaiv", SvoFeatures.Default)]
    [Arguments("MonthSpan", typeof(int), "month span", "Qowaiv", SvoFeatures.Continuous)]
    [Arguments("Percentage", typeof(decimal), "percentage", "Qowaiv", SvoFeatures.Continuous)]
    [Arguments("PostalCode", typeof(string), "postal code", "Qowaiv", SvoFeatures.Default)]
    [Arguments("Sex", typeof(byte), "sex", "Qowaiv", SvoFeatures.Default)]
    [Arguments("Uuid", typeof(Guid), "UUID", "Qowaiv", SvoFeatures.Default ^ SvoFeatures.IsUnknown)]
    [Arguments("WeekDate", typeof(Date), "week date", "Qowaiv", SvoFeatures.Continuous ^ SvoFeatures.Field ^ SvoFeatures.ISerializable)]
    [Arguments("Year", typeof(short), "year", "Qowaiv", SvoFeatures.Default)]
    [Arguments("YearMonth", typeof(int), "year-month", "Qowaiv", SvoFeatures.Continuous)]
    [Arguments("YesNo", typeof(byte), "yes-no", "Qowaiv", SvoFeatures.Default)]

    [Arguments("Amount", typeof(decimal), "amount", "Qowaiv.Financial", SvoFeatures.Continuous, "QowaivMessages.FormatExceptionFinancialAmount")]
    [Arguments("BusinessIdentifierCode", typeof(string), "BIC", "Qowaiv.Financial", SvoFeatures.Default)]
    [Arguments("Currency", typeof(string), "currency", "Qowaiv.Financial", SvoFeatures.Default)]
    [Arguments("InternationalBankAccountNumber", typeof(string), "IBAN", "Qowaiv.Financial", SvoFeatures.Default)]
    [Arguments("Money", typeof(decimal), "money", "Qowaiv.Financial", (SvoFeatures.Continuous)
        ^ SvoFeatures.ISerializable
        ^ SvoFeatures.EqualsSvo
        ^ SvoFeatures.Field
        ^ SvoFeatures.GetHashCode)]

    [Arguments("CasRegistryNumber", typeof(long), "CAS Registry Number", "Qowaiv.Chemistry", SvoFeatures.Default)]

    [Arguments("Country", typeof(string), "country", "Qowaiv.Globalization", SvoFeatures.Default)]

    [Arguments("StreamSize", typeof(long), "stream size", "Qowaiv.IO", SvoFeatures.Continuous ^ SvoFeatures.Field ^ SvoFeatures.IFormattable)]

    [Arguments("EnergyLabel", typeof(int), "EU energy label", "Qowaiv.Sustainability", SvoFeatures.Default)]

    [Arguments("Elo", typeof(double), "elo", "Qowaiv.Statistics", SvoFeatures.Continuous)]

    [Arguments("Fraction", typeof(long), "fraction", "Qowaiv.Mathematics", (SvoFeatures.Continuous)
        ^ SvoFeatures.ISerializable
        ^ SvoFeatures.EqualsSvo
        ^ SvoFeatures.Field)]

    [Arguments("InternetMediaType", typeof(string), "Internet media type", "Qowaiv.Web", SvoFeatures.All)]
    public void Qowaiv(
        string name,
        Type underlying,
        string fullName,
        string ns,
        SvoFeatures features,
        string? formatExceptionMessage = null)
    {
        var sub = ns.Replace("Qowaiv", "").Replace(".", @"\");
        var path = $@"{Root}\src\Qowaiv\Generated\{sub}\{name}.generated.cs".Replace(@"\\", @"\");
        var init = $@"{Root}\src\Qowaiv\{sub}\{name}.cs".Replace(@"\\", @"\");
        var json = $@"{Root}\src\Qowaiv\Json\{sub}\{name}JsonConverter.cs".Replace(@"\\", @"\");
        var spec = $@"{Root}\specs\Qowaiv.Specs\{sub}\{name}_specs.cs".Replace(@"\\", @"\");
        Generate(name, underlying, fullName, ns, features, path, init, json, spec, formatExceptionMessage)
            .Should().BeTrue();
    }

    [Test]
    [Arguments("Timestamp", typeof(ulong), "timestamp", "Qowaiv.Sql", SvoFeatures.Continuous)]
    public void Qowaiv_DataClient(string name, Type underlying, string fulleName, string ns, SvoFeatures features, string? formatExceptionMessage = null)
    {
        var path = $@"{Root}\src\Qowaiv.Data.SqlClient\Generated\{name}.generated.cs";
        var init = $@"{Root}\src\Qowaiv.Data.SqlClient\Sql\{name}.cs";
        var json = $@"{Root}\src\Qowaiv.Data.SqlClient\Json\{name}JsonConverter.cs";
        var spec = $@"{Root}\src\Specs.Data.SqlClient\{name}_specs.cs".Replace(@"\\", @"\");
        Generate(name, underlying, fulleName, ns, features, path, init, json, spec, formatExceptionMessage)
            .Should().BeTrue();
    }

#pragma warning disable S107 // Methods should not have too many parameters
    private static bool Generate(
        string name,
        Type underlying,
        string fulleName,
        string ns,
        SvoFeatures features,
        string path,
        string init,
        string json,
        string spec,
        string? formatExceptionMessage)
    {
        var arguments = new SvoArguments
        {
            Name = name,
            Underlying = underlying,
            FullName = fulleName,
            Namespace = ns,
            Features = features,
            FormatExceptionMessage = formatExceptionMessage ?? $"QowaivMessages.FormatException{name}"
        };

        Generate(new FileInfo(path), arguments);
        GenerateInitial(new FileInfo(init), arguments);
        GenerateJsonConverter(new FileInfo(json), arguments);
        GenerateSpecs(new FileInfo(spec), arguments);

        return true;

        static void Generate(FileInfo location, SvoArguments arguments)
            => Write(location, arguments, SvoTemplate.GeneratedCode, @override: true);

        static void GenerateInitial(FileInfo location, SvoArguments arguments)
            => Write(location, arguments, SvoTemplate.Initial);

        static void GenerateSpecs(FileInfo location, SvoArguments arguments)
            => Write(location, arguments, SvoTemplate.Specs);

        static void GenerateJsonConverter(FileInfo location, SvoArguments arguments)
            => Write(location, arguments, SvoTemplate.JsonConverter);

        static void Write(
            FileInfo location,
            SvoArguments arguments,
            SvoTemplate template,
            bool @override = false)
        {
            if (@override || !location.Exists || location.Length < StreamSize.FromKilobytes(0.25))
            {
                if (!location.Directory!.Exists)
                {
                    location.Directory.Create();
                }
                var writer = new CSharpWriter(new StreamWriter(location.FullName, false, UTF8_no_BOM));
                writer.Write(template.Transform(arguments));
                writer.Flush();
            }
        }
    }

    private static readonly UTF8Encoding UTF8_no_BOM = new(false);
}
