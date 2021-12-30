namespace Qowaiv.TestTools;

public static class Svo
{
    public static readonly Amount Amount = (Amount)42.17m;
    public static readonly BusinessIdentifierCode BusinessIdentifierCode = BusinessIdentifierCode.Parse("AEGONL2UXXX");
    public static readonly Country Country = Country.VA;
    public static readonly CryptographicSeed CryptographicSeed = CryptographicSeed.Parse("Qowaiv==");

    /// <summary>EUR</summary>
    public static readonly Currency Currency = Currency.EUR;

    /// <summary>2017-06-11</summary>
    public static readonly Date Date = new(2017, 06, 11);
    
    /// <summary>10Y+3M-5D</summary>
    public static readonly DateSpan DateSpan = new(10, 3, -5);

    /// <summary>2017-06-11 06:15:00U</summary>
    public static readonly DateTime DateTime = new(2017, 06, 11, 06, 15, 00);
    /// <summary>2017-06-11 06:15:00 +0:00</summary>
    public static readonly DateTimeOffset DateTimeOffset = new(2017, 06, 11, 06, 15, 00, TimeSpan.Zero);


    /// <summary>info@qowaiv.org</summary>
    public static readonly EmailAddress EmailAddress = EmailAddress.Parse("info@qowaiv.org");
    public static readonly Elo Elo = 1732.4;
    
    /// <summary>-69/17</summary>
    public static readonly Fraction Fraction = -69.DividedBy(17);

    [Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
    public static readonly Gender Gender = Gender.Female;
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
    public static readonly PostalCode PostalCode = PostalCode.Parse("H0H0H0");
    public static readonly Secret Secret = Secret.Parse("Ken sent me!");
    public static readonly Sex Sex = Sex.Female;
    public static readonly StreamSize StreamSize = 123456789.Bytes();
    public static readonly TimeZoneInfo TimeZone = TestTimeZones.EastAustraliaStandardTime;
    public static readonly Timestamp Timestamp = 1234567890L;
    public static readonly Uuid Uuid = Uuid.Parse("Qowaiv_SVOLibrary_GUIA");
    public static readonly WeekDate WeekDate = new(2017, 23, 7);
    public static readonly Year Year = 1979.CE();
    public static readonly YesNo YesNo = YesNo.Yes;

    /// <summary>PREFIX17</summary>
    public static readonly Int32Id Int32Id = Int32Id.Create(17);

    /// <summary>PREFIX987654321</summary>
    public static readonly Int64Id Int64Id = Int64Id.Create(987654321L);
    public static readonly StringId StringId = StringId.Parse("Qowaiv-ID");
    
    /// <summary>8A1A8C42-D2FF-E254-E26E-B6ABCBF19420</summary>
    public static readonly CustomGuid CustomGuid = CustomGuid.Parse("8A1A8C42-D2FF-E254-E26E-B6ABCBF19420");
    public static readonly CustomUuid CustomUuid = CustomUuid.Parse("Qowaiv_SVOLibrary_GUIA");


    public static IEnumerable<object> All() => typeof(Svo)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(field => field.GetValue(null));
}

public sealed class ForInt32 : Int32IdBehavior
{
    public override string ToString(object obj, string format, IFormatProvider formatProvider)
       => string.Format(formatProvider, $"PREFIX{{0:{format}}}", obj);

    public override bool TryParse(string str, out object id)
        => (str ?? "").StartsWith("PREFIX")
        ? base.TryParse(str?[6..], out id)
        : base.TryParse(str, out id);
}

public sealed class ForInt64 : Int64IdBehavior
{
    public override string ToString(object obj, string format, IFormatProvider formatProvider)
       => string.Format(formatProvider, $"PREFIX{{0:{format}}}", obj);

    public override bool TryCreate(object obj, out object id)
        => base.TryCreate(obj, out id)
        && id is long number
        && IsValid(number);

    public override bool TryParse(string str, out object id)
        => (str ?? "").StartsWith("PREFIX")
        ? base.TryParse(str?[6..], out id)
        : base.TryParse(str, out id) && IsValid(id);

    private static bool IsValid(long number) => number % 2 == 1;
}

public class ForString : StringIdBehavior { }
public class ForGuid : GuidBehavior { }
public class ForUuid : UuidBehavior { }
