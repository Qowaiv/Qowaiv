using System.Reflection;

namespace Qowaiv.TestTools.Globalization;

/// <summary>Contains <see cref="TimeZoneInfo"/>'s for test purposes.</summary>
public static class TestTimeZones
{
    /// <summary>Gets the East Australia Standard Time (+10:00) <see cref="TimeZoneInfo"/>.</summary>
    public static readonly TimeZoneInfo EastAustraliaStandardTime = TimeZoneInfo.FromSerializedString("E. Australia Standard Time;600;(UTC+10:00) Brisbane;E. Australia Standard Time;E. Australia Daylight Time;;");

    /// <summary>Gets the East Alaskan Standard Time (-9:00) <see cref="TimeZoneInfo"/>.</summary>
    public static readonly TimeZoneInfo AlaskanStandardTime = TimeZoneInfo.FromSerializedString("Alaskan Standard Time;-540;(UTC-09:00) Alaska;Alaskan Standard Time;Alaskan Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];");

    /// <summary>Gets the Leiden Central Time (+0:33) <see cref="TimeZoneInfo"/>.</summary>
    public static readonly TimeZoneInfo LeidenTime = New(
        id: "LCT",
        baseUtcOffset: TimeSpan.FromMinutes(33),
        displayName: "(UTC: +0:33) Leiden, Leiderdorp, Oegstgeest",
        standardDisplayName: "Leiden Central Time",
        daylightDisplayName: "Leiden Central Time",
        adjustmentRules: Array.Empty<TimeZoneInfo.AdjustmentRule>(),
        disableDaylightSavingTime: true);

    /// <summary>Creates a custom <see cref="TimeZoneInfo"/>for testing purposes.</summary>
    [Pure]
    public static TimeZoneInfo New(
        string id,
        TimeSpan baseUtcOffset,
        string displayName,
        string standardDisplayName,
        string daylightDisplayName,
        TimeZoneInfo.AdjustmentRule[] adjustmentRules,
        bool disableDaylightSavingTime)
    {
        var ctor = typeof(TimeZoneInfo).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(ct => ct.GetParameters() is { Length: 8 } pars
                && pars[0].ParameterType == typeof(string)
                && pars[1].ParameterType == typeof(TimeSpan)
                && pars[6].ParameterType == typeof(bool));
        try
        {
            return (TimeZoneInfo)ctor.Invoke(new object[] {
                    id,
                    baseUtcOffset,
                    displayName,
                    standardDisplayName,
                    daylightDisplayName,
                    adjustmentRules,
                    disableDaylightSavingTime,
                    false
                });
        }
        catch (TargetInvocationException x) { throw x.InnerException ?? x; }
    }
}
