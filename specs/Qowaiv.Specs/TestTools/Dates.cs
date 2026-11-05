using AwesomeAssertions.Extensions;

namespace Qowaiv.TestTools;

internal static class Dates
{
#if NET48
    public static readonly DateTime UnixEpoch = 1.January(1970).AsUtc();
#else
    public static readonly DateTime UnixEpoch = DateTime.UnixEpoch;
#endif

}
