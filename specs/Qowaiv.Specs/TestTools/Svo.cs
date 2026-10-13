using Qowaiv.Chemistry;
using Qowaiv.Sustainability;
using System.Net.Http;

namespace Qowaiv.TestTools;

public static class Svo
{
    /// <summary>¤42.17</summary>
    public static Amount Amount => 42.17m.Amount();

    /// <summary>AEGONL2UXXX</summary>
    public static BusinessIdentifierCode BusinessIdentifierCode => BusinessIdentifierCode.Parse("AEGONL2UXXX");

    /// <summary>10028-14-5 (Nobelium)</summary>
    public static CasRegistryNumber CasRegistryNumber => 10028_14_5.CasNr();

    /// <summary>Holy See (VA)</summary>
    public static Country Country => Country.VA;

    /// <summary>Qowaiv==</summary>
    public static CryptographicSeed CryptographicSeed => CryptographicSeed.Parse("Qowaig==");

    /// <summary>EUR</summary>
    public static Currency Currency => Currency.EUR;

    /// <summary>2017-06-11</summary>
    public static Date Date => new(2017, 06, 11);

#if NET10_0_OR_GREATER
    /// <summary>2017-06-11</summary>
    public static DateOnly DateOnly => new(2017, 06, 11);
#endif
    /// <summary>10Y+3M-5D</summary>
    public static DateSpan DateSpan => 10.Years() + 3.Months() - 5.Days();

    /// <summary>2017-06-11 06:15:00U</summary>
    public static DateTime DateTime => new(2017, 06, 11, 06, 15, 00, 000, DateTimeKind.Local);

    /// <summary>2017-06-11 06:15:00 +0:00</summary>
    public static DateTimeOffset DateTimeOffset => new(2017, 06, 11, 06, 15, 00, TimeSpan.Zero);

    /// <summary>info@qowaiv.org</summary>
    public static EmailAddress EmailAddress => EmailAddress.Parse("info@qowaiv.org");

    /// <summary>1732.4</summary>
    public static Elo Elo => 1732.4;

    /// <summary>A++</summary>
    public static EnergyLabel EnergyLabel => EnergyLabel.A(2);

    /// <summary>0.000 (fr-BE)</summary>
    public static FormattingArguments FormattingArguments => new("0.000", TestCultures.fr_BE);

    /// <summary>-69/17</summary>
    public static Fraction Fraction => -69.DividedBy(17);

    /// <summary>8a1a8c42-d2ff-e254-e26e-b6abcbf19420</summary>
    public static Guid Guid => Guid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

    public static Hash Hash => Hash.Code("QOWAIV");

    /// <summary>123.456.789</summary>
    public static HouseNumber HouseNumber => 123_456_789L;

    /// <summary>POST.</summary>
    public static HttpMethod HttpMethod => HttpMethod.Post;

    /// <summary>NL20 INGB 0001 2345 67</summary>
    public static InternationalBankAccountNumber Iban => InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");

    /// <summary>application/x-chess-pgn</summary>
    public static InternetMediaType InternetMediaType => InternetMediaType.Parse("application/x-chess-pgn");

    /// <summary>2017-06-11 06:15:00</summary>
    public static LocalDateTime LocalDateTime => new(2017, 06, 11, 06, 15, 00);

    public static Money Money => 42.17m + Currency.EUR;

    public static Month Month => Month.February;

    /// <summary>5Y+9M</summary>
    public static MonthSpan MonthSpan => 5.Years() + 9.Months();

    /// <summary>17.51%</summary>
    public static Percentage Percentage => 17.51m.Percent();

    /// <summary>H0H0H0 (Canada)</summary>
    public static PostalCode PostalCode => PostalCode.Parse("H0H0H0");

    public static Secret Secret => Secret.Parse("Ken sent me!");
    public static Sex Sex => Sex.Female;
    public static StreamSize StreamSize => 123456789.Bytes();
    public static TimeZoneInfo TimeZone => TestTimeZones.EastAustraliaStandardTime;
    public static Timestamp Timestamp => 1234567890L;
    public static Uuid Uuid => Uuid.Parse("Qowaiv_SVOLibrary_GUIA");

    /// <summary>2017-W23-7</summary>
    public static WeekDate WeekDate => new(2017, 23, 7);
    public static Year Year => 1979.CE();

    /// <summary>2017-06</summary>
    public static YearMonth YearMonth => new(year: 2017, month: 06);

    /// <summary>17 years</summary>
    public static YearSpan YearSpan => 17.Years();

    /// <summary>Yes</summary>
    public static YesNo YesNo => YesNo.Yes;

    public static IEnumerable<object> All() => typeof(Svo)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(field => field.GetValue(null)!);

#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members    
    // They have equivilent meaning, other naming would not make tings better.
    public static class Generated
    {
        /// <summary>QOWAIV</summary>
        public static Specs_Generated.CustomSvo CustomSvo => Specs_Generated.CustomSvo.Parse("QOWAIV");

        /// <summary>8a1a8c42-d2ff-e254-e26e-b6abcbf19420</summary>
        public static Specs_Generated.GuidBasedId CustomGuid => Specs_Generated.GuidBasedId.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

        /// <summary>Qowaiv_SVOLibrary_GUIA</summary>
        public static Specs_Generated.UuidBasedId CustomUuid => Specs_Generated.UuidBasedId.Parse("Qowaiv_SVOLibrary_GUIA");

        /// <summary>PREFIX17</summary>
        public static Specs_Generated.Int32BasedId Int32Id => Specs_Generated.Int32BasedId.Create(17);

        /// <summary>PREFIX987654321</summary>
        public static Specs_Generated.Int64BasedId Int64Id => Specs_Generated.Int64BasedId.Create(987654321L);

        /// <summary>Qowaiv-ID</summary>
        public static Specs_Generated.StringBasedId StringId => Specs_Generated.StringBasedId.Parse("Qowaiv-ID");
    }
#pragma warning restore S3218 // Inner class members should not shadow outer class "static" or type members
}
