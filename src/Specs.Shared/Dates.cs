namespace Qowaiv.TestTools;

public static class Dates
{
#if NET10_0_OR_GREATER
    public static readonly DateTime UnixEpoch = DateTime.UnixEpoch;
#else
    public static readonly DateTime UnixEpoch = 1.January(1970).AsUtc();
#endif

}
