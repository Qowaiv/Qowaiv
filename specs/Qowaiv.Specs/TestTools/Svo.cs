using Qowaiv.Chemistry;
using Qowaiv.Sustainability;

namespace Qowaiv.TestTools;

public static class Svo
{
    /// <summary>¤42.17</summary>
    public static readonly Amount Amount = 42.17.Amount();

    /// <summary>AEGONL2UXXX</summary>
    public static readonly BusinessIdentifierCode BusinessIdentifierCode = BusinessIdentifierCode.Parse("AEGONL2UXXX");

    /// <summary>10028-14-5 (Nobelium)</summary>
    public static readonly CasRegistryNumber CasRegistryNumber = 10028_14_5.CasNr();

    /// <summary>Holy See (VA)</summary>
    public static readonly Country Country = Country.VA;

    /// <summary>Qowaiv==</summary>
    public static readonly CryptographicSeed CryptographicSeed = CryptographicSeed.Parse("Qowaiv==");

    /// <summary>EUR</summary>
    public static readonly Currency Currency = Currency.EUR;

    /// <summary>2017-06-11</summary>
    public static readonly Date Date = new(2017, 06, 11);

#if NET6_0_OR_GREATER
    /// <summary>2017-06-11</summary>
    public static readonly DateOnly DateOnly = new(2017, 06, 11);
#endif
    /// <summary>10Y+3M-5D</summary>
    public static readonly DateSpan DateSpan = new(10, 3, -5);

    /// <summary>2017-06-11 06:15:00U</summary>
    public static readonly DateTime DateTime = new(2017, 06, 11, 06, 15, 00, 000, DateTimeKind.Local);

    /// <summary>2017-06-11 06:15:00 +0:00</summary>
    public static readonly DateTimeOffset DateTimeOffset = new(2017, 06, 11, 06, 15, 00, TimeSpan.Zero);

    /// <summary>info@qowaiv.org</summary>
    public static readonly EmailAddress EmailAddress = EmailAddress.Parse("info@qowaiv.org");

    /// <summary>1732.4</summary>
    public static readonly Elo Elo = 1732.4;

    /// <summary>A++</summary>
    public static readonly EnergyLabel EnergyLabel = EnergyLabel.A(2);

    /// <summary>0.000 (fr-BE)</summary>
    public static readonly FormattingArguments FormattingArguments = new("0.000", TestCultures.fr_BE);

    /// <summary>-69/17</summary>
    public static readonly Fraction Fraction = -69.DividedBy(17);

    /// <summary>8a1a8c42-d2ff-e254-e26e-b6abcbf19420</summary>
    public static readonly Guid Guid = Guid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

    public static readonly Hash Hash = Hash.Code("QOWAIV");

    /// <summary>123.456.789</summary>
    public static readonly HouseNumber HouseNumber = 123456789L;

    /// <summary>NL20 INGB 0001 2345 67</summary>
    public static readonly InternationalBankAccountNumber Iban = InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");

    /// <summary>application/x-chess-pgn</summary>
    public static readonly InternetMediaType InternetMediaType = InternetMediaType.Parse("application/x-chess-pgn");

    /// <summary>2017-06-11 06:15:00</summary>
    public static readonly LocalDateTime LocalDateTime = new(2017, 06, 11, 06, 15, 00);
    public static readonly Money Money = 42.17 + Currency.EUR;
    public static readonly Month Month = Month.February;

    /// <summary>5Y+9M</summary>
    public static readonly MonthSpan MonthSpan = MonthSpan.FromMonths(69);

    public static readonly Percentage Percentage = 17.51.Percent();

    /// <summary>H0H0H0 (Canada)</summary>
    public static readonly PostalCode PostalCode = PostalCode.Parse("H0H0H0");

    public static readonly Secret Secret = Secret.Parse("Ken sent me!");
    public static readonly Sex Sex = Sex.Female;
    public static readonly StreamSize StreamSize = 123456789.Bytes();
    public static readonly TimeZoneInfo TimeZone = TestTimeZones.EastAustraliaStandardTime;
    public static readonly Timestamp Timestamp = 1234567890L;
    public static readonly Uuid Uuid = Uuid.Parse("Qowaiv_SVOLibrary_GUIA");

    /// <summary>2017-W23-7</summary>
    public static readonly WeekDate WeekDate = new(2017, 23, 7);
    public static readonly Year Year = 1979.CE();

    /// <summary>2017-06</summary>
    public static readonly YearMonth YearMonth = new(year: 2017, month: 06);

    public static readonly YesNo YesNo = YesNo.Yes;

    /// <summary>PREFIX17</summary>
    [Obsolete("Will be dropped in Qowaiv 8.0")]
    public static readonly Int32Id Int32Id = Int32Id.Create(17);

    /// <summary>PREFIX987654321</summary>
    [Obsolete("Will be dropped in Qowaiv 8.0")]
    public static readonly Int64Id Int64Id = Int64Id.Create(987654321L);

    [Obsolete("Will be dropped in Qowaiv 8.0")]
    public static readonly StringId StringId = StringId.Parse("Qowaiv-ID");

    /// <summary>8A1A8C42-D2FF-E254-E26E-B6ABCBF19420</summary>
    [Obsolete("Will be dropped in Qowaiv 8.0")]
    public static readonly CustomGuid CustomGuid = CustomGuid.Parse("8A1A8C42-D2FF-E254-E26E-B6ABCBF19420");

    [Obsolete("Will be dropped in Qowaiv 8.0")]
    public static readonly CustomUuid CustomUuid = CustomUuid.Parse("Qowaiv_SVOLibrary_GUIA");

    public static IEnumerable<object> All() => typeof(Svo)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(field => field.GetValue(null)!);

#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members    
    // They have equivilent meaning, other naming would not make tings better.
    public static class Generated
    {
        /// <summary>QOWAIV</summary>
        public static readonly Specs_Generated.CustomSvo CustomSvo = Specs_Generated.CustomSvo.Parse("QOWAIV");

        /// <summary>8a1a8c42-d2ff-e254-e26e-b6abcbf19420</summary>
        public static readonly Specs_Generated.GuidBasedId CustomGuid = Specs_Generated.GuidBasedId.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

        /// <summary>Qowaiv_SVOLibrary_GUIA</summary>
        public static readonly Specs_Generated.UuidBasedId CustomUuid = Specs_Generated.UuidBasedId.Parse("Qowaiv_SVOLibrary_GUIA");

        /// <summary>PREFIX17</summary>
        public static readonly Specs_Generated.Int32BasedId Int32Id = Specs_Generated.Int32BasedId.Create(17);

        /// <summary>PREFIX987654321</summary>
        public static readonly Specs_Generated.Int64BasedId Int64Id = Specs_Generated.Int64BasedId.Create(987654321L);

        /// <summary>Qowaiv-ID</summary>
        public static readonly Specs_Generated.StringBasedId StringId = Specs_Generated.StringBasedId.Parse("Qowaiv-ID");
    }
#pragma warning restore S3218 // Inner class members should not shadow outer class "static" or type members
}

[Obsolete("Will be dropped in Qowaiv 8.0")]
public sealed class ForInt32 : Int32IdBehavior
{
    public override string ToString(object? obj, string? format, IFormatProvider? formatProvider)
       => string.Format(formatProvider, $"PREFIX{{0:{format}}}", obj);

    public override bool TryParse(string? str, out object? id)
        => str is { Length: > 6 } && str[..6] == "PREFIX"
        ? base.TryParse(str[6..], out id)
        : base.TryParse(str, out id);
}

[Obsolete("Will be dropped in Qowaiv 8.0")]
public sealed class ForInt64 : Int64IdBehavior
{
    public override string ToString(object? obj, string? format, IFormatProvider? formatProvider)
       => string.Format(formatProvider, $"PREFIX{{0:{format}}}", obj);

    public override bool TryCreate(object? obj, out object? id)
        => base.TryCreate(obj, out id)
        && id is long number
        && IsValid(number);

    public override bool TryParse(string? str, out object? id)
        => str is { Length: > 6 } && str[..6] == "PREFIX"
        ? base.TryParse(str[6..], out id)
        : base.TryParse(str, out id) && IsValid(id!);

    private static bool IsValid(long number) => (number & 1) == 1;
}

[Obsolete("Will be dropped in Qowaiv 8.0")]
public class ForString : StringIdBehavior { }

[Obsolete("Will be dropped in Qowaiv 8.0")]
public class ForGuid : GuidBehavior { }

[Obsolete("Will be dropped in Qowaiv 8.0")]
public class ForUuid : UuidBehavior { }
