using Qowaiv.Financial;
using Qowaiv.Formatting;
using Qowaiv.Globalization;
using Qowaiv.Hashing;
using Qowaiv.Identifiers;
using Qowaiv.IO;
using Qowaiv.Mathematics;
using Qowaiv.Security;
using Qowaiv.Security.Cryptography;
using Qowaiv.Sql;
using Qowaiv.Statistics;
using Qowaiv.TestTools.Globalization;
using Qowaiv.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CustomGuid = Qowaiv.Identifiers.Id<Qowaiv.Specs.ForGuid>;
using CustomUuid = Qowaiv.Identifiers.Id<Qowaiv.Specs.ForUuid>;
using Int32Id = Qowaiv.Identifiers.Id<Qowaiv.Specs.ForInt32>;
using Int64Id = Qowaiv.Identifiers.Id<Qowaiv.Specs.ForInt64>;
using StringId = Qowaiv.Identifiers.Id<Qowaiv.Specs.ForString>;

namespace Qowaiv.Specs
{
    public static class Svo
    {
        public static readonly Amount Amount = (Amount)42.17m;
        public static readonly BusinessIdentifierCode BusinessIdentifierCode = BusinessIdentifierCode.Parse("AEGONL2UXXX");
        public static readonly Country Country = Country.VA;
        public static readonly CryptographicSeed CryptographicSeed = CryptographicSeed.Parse("Qowaiv==");
        public static readonly Currency Currency = Currency.EUR;
        public static readonly Date Date = new(2017, 06, 11);
        public static readonly DateSpan DateSpan = new(10, 3, -5);
        public static readonly DateTime DateTime = new(2017, 06, 11, 06, 15, 00);
        public static readonly EmailAddress EmailAddress = EmailAddress.Parse("info@qowaiv.org");
        public static readonly Elo Elo = 1732.4;
        public static readonly Fraction Fraction = -69.DividedBy(17);
        [Obsolete("Will be dropped in version 7. Use Qowaiv.Sex instead.")]
        public static readonly Gender Gender = Gender.Female;
        public static readonly Guid Guid = Guid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
        public static readonly Hash Hash = Hash.Code("QOWAIV");
        public static readonly HouseNumber HouseNumber = 123456789L;
        public static readonly InternationalBankAccountNumber Iban = InternationalBankAccountNumber.Parse("NL20 INGB 0001 2345 67");
        public static readonly InternetMediaType InternetMediaType = InternetMediaType.Parse("application/x-chess-pgn");
        public static readonly LocalDateTime LocalDateTime = new(2017, 06, 11, 06, 15, 00);
        public static readonly Money Money = 42.17 + Currency.EUR;
        public static readonly Month Month = Month.February;
        public static readonly MonthSpan MonthSpan = MonthSpan.FromMonths(69);
        public static readonly Percentage Percentage = 17.51.Percent();
        public static readonly PostalCode PostalCode = PostalCode.Parse("H0H0H0");
        public static readonly Secret Secret = Secret.Parse("Ken sent me!");
        public static readonly Sex Sex = Sex.Female;
        public static readonly StreamSize StreamSize = 123456789;
        public static readonly TimeZoneInfo TimeZone = TestTimeZones.LeidenTime;
        public static readonly Timestamp Timestamp = 1234567890L;
        public static readonly Uuid Uuid = Uuid.Parse("Qowaiv_SVOLibrary_GUIA");
        public static readonly WeekDate WeekDate = new(2017, 23, 7);
        public static readonly Year Year = 1979;
        public static readonly YesNo YesNo = YesNo.Yes;

        public static readonly Int32Id Int32Id = Int32Id.Create(17);
        public static readonly Int64Id Int64Id = Int64Id.Create(987654321L);
        public static readonly StringId StringId = StringId.Parse("Qowaiv-ID");
        public static readonly CustomGuid CustomGuid = CustomGuid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
        public static readonly CustomUuid CustomUuid = CustomUuid.Parse("Qowaiv_SVOLibrary_GUIA");


        public static IEnumerable<object> All() => typeof(Svo)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(field => field.GetValue(null));

        public static IEnumerable<Type> AllTypes()
             => typeof(SingleValueObjectAttribute).Assembly
            .GetTypes()
            .Where(tp => tp.IsValueType && tp.IsPublic && !tp.IsEnum && !tp.IsAbstract)
            .Except(new[]
            {
                typeof(FormattingArguments),
                typeof(Secret),
                typeof(Hash),
            });
    }

    public sealed class ForInt32 : Int32IdBehavior
    {
        public override string ToString(object obj, string format, IFormatProvider formatProvider)
            => $"PREFIX{obj:format}";

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
}
