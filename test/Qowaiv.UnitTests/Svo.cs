using Qowaiv.TestTools.Globalization;
using System;

namespace Qowaiv.UnitTests
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
    }
}
