using System;

namespace Qowaiv.TestTools.Globalization
{
    /// <summary>Contains <see cref="TimeZoneInfo"/>'s for test purposes.</summary>
    public static class TestTimeZones
    {
        /// <summary>Gets the East Australia Standard Time (+10:00) <see cref="TimeZoneInfo"/>.</summary>
        public static readonly TimeZoneInfo EastAustraliaStandardTime = TimeZoneInfo.FromSerializedString("E. Australia Standard Time;600;(UTC+10:00) Brisbane;E. Australia Standard Time;E. Australia Daylight Time;;");

        /// <summary>Gets the East Alaskan Standard Time (-9:00) <see cref="TimeZoneInfo"/>.</summary>
        public static readonly TimeZoneInfo AlaskanStandardTime = TimeZoneInfo.FromSerializedString("Alaskan Standard Time;-540;(UTC-09:00) Alaska;Alaskan Standard Time;Alaskan Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];");
    }
}
