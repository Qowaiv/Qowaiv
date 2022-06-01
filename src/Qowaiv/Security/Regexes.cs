namespace Qowaiv.Security;

internal static class Regexes
{
    /// <summary>
    /// To prevent DoS attacks exploiting regular expressions on user input,
    /// the match time-out is set 1 ms.
    /// </summary>
    public static readonly TimeSpan MaxDuration = TimeSpan.FromMilliseconds(1);
}
