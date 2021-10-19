using Qowaiv.Identifiers;
using Qowaiv.TestTools.Globalization;
using System;
using CustomGuid = Qowaiv.Identifiers.Id<Qowaiv.Specs.ForGuid>;
using CustomUuid = Qowaiv.Identifiers.Id<Qowaiv.Specs.ForUuid>;
using Int32Id = Qowaiv.Identifiers.Id<Qowaiv.Specs.ForInt32>;
using Int64Id = Qowaiv.Identifiers.Id<Qowaiv.Specs.ForInt64>;
using StringId = Qowaiv.Identifiers.Id<Qowaiv.Specs.ForString>;

namespace Qowaiv.Specs
{
    public static class Svo
    {
        public static readonly DateTime DateTime = new(2017, 06, 11, 06, 15, 00);
        public static readonly EmailAddress EmailAddress = EmailAddress.Parse("info@qowaiv.org");
        public static readonly Gender Gender = Gender.Female;
        public static readonly Month Month = Month.February;
        public static readonly Percentage Percentage = 17.51.Percent();
        public static readonly PostalCode PostalCode = PostalCode.Parse("H0H0H0");
        public static readonly TimeZoneInfo TimeZone = TestTimeZones.EastAustraliaStandardTime;

        public static readonly Guid Guid = Guid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
        public static readonly Uuid Uuid = Uuid.Parse("Qowaiv_SVOLibrary_GUIA");

        public static readonly Year Year = 1979;
        public static readonly YesNo YesNo = YesNo.Yes;

        public static readonly Int32Id Int32Id = Int32Id.Create(17);
        public static readonly Int64Id Int64Id = Int64Id.Create(9876543210L);
        public static readonly StringId StringId = StringId.Parse("Qowaiv-ID");
        public static readonly CustomGuid CustomGuid = CustomGuid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
        public static readonly CustomUuid CustomUuid = CustomUuid.Parse("Qowaiv_SVOLibrary_GUIA");
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
            => $"PREFIX{obj:format}";

        public override bool TryParse(string str, out object id)
            => (str ?? "").StartsWith("PREFIX")
            ? base.TryParse(str?[6..], out id)
            : base.TryParse(str, out id);
    }

    public class ForString : StringIdBehavior { }
    public class ForGuid : GuidBehavior { }
    public class ForUuid : UuidBehavior { }
}
