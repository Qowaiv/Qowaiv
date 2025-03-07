using System.Reflection;

namespace Qowaiv.TestTools.Globalization;

/// <summary>Contains <see cref="TimeZoneInfo" />'s for test purposes.</summary>
public static class TestTimeZones
{
    /// <summary>Gets the East Australia Standard Time (+10:00) <see cref="TimeZoneInfo" />.</summary>
    public static readonly TimeZoneInfo EastAustraliaStandardTime = TimeZoneInfo.FromSerializedString("E. Australia Standard Time;600;(UTC+10:00) Brisbane;E. Australia Standard Time;E. Australia Daylight Time;;");

    /// <summary>Gets the East Alaskan Standard Time (-9:00) <see cref="TimeZoneInfo" />.</summary>
    public static readonly TimeZoneInfo AlaskanStandardTime = TimeZoneInfo.FromSerializedString("Alaskan Standard Time;-540;(UTC-09:00) Alaska;Alaskan Standard Time;Alaskan Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];");

    /// <summary>Gets the Leiden Central Time (+0:33) <see cref="TimeZoneInfo" />.</summary>
    public static readonly TimeZoneInfo LeidenTime = New(
        id: "LCT",
        baseUtcOffset: TimeSpan.FromMinutes(33),
        displayName: "(UTC: +0:33) Leiden, Leiderdorp, Oegstgeest",
        standardDisplayName: "Leiden Central Time",
        daylightDisplayName: "Leiden Central Time",
        adjustmentRules: [],
        disableDaylightSavingTime: true);

    /// <summary>Creates a custom <see cref="TimeZoneInfo" />for testing purposes.</summary>
    /// <remarks>
    /// Prior to .NET 6, the non-public constructor had 7 arguments, afterwards 8.
    /// </remarks>
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
        var ctor = typeof(TimeZoneInfo).GetConstructors(NonPublicInstance)
            .Find(ct => ct.GetParameters() is { Length: >= 7 } pars
                && pars[0].ParameterType == typeof(string)
                && pars[1].ParameterType == typeof(TimeSpan)
                && pars[6].ParameterType == typeof(bool))
            ?? throw new InvalidOperationException("No constructor could be found to create a custom time zone.");
        try
        {
            var args = new List<object>
            {
                id,
                baseUtcOffset,
                displayName,
                standardDisplayName,
                daylightDisplayName,
                adjustmentRules,
                disableDaylightSavingTime,
            };
            if (ctor.GetParameters().Length == 8) { args.Add(true); }

            return (TimeZoneInfo)ctor.Invoke([.. args]);
        }
        catch (TargetInvocationException x) { throw x.InnerException ?? x; }
    }

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    // We need access and have exception handling in place once this fails.
    private const BindingFlags NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;
}
