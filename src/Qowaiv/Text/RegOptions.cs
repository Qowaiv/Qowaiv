namespace Qowaiv.Text;

/// <summary>Centralized place to define <see cref="Regex"/> options.</summary>
internal static class RegOptions
{
#if DEBUG
    public const int TimeoutMilliseconds = 50;
#else
    public const int MatchTimeoutMilliseconds = 100;
#endif

    /// <summary>
    /// To prevent DoS attacks exploiting regular expressions on user input,
    /// the match time-out is set 50 ms.
    /// </summary>
    public static readonly TimeSpan Timeout = TimeSpan.FromMilliseconds(TimeoutMilliseconds);

    public static readonly TimeSpan Replacement = TimeSpan.FromMilliseconds(100);

#if NET7_0_OR_GREATER
    public const RegexOptions Default = RegexOptions.Compiled | RegexOptions.NonBacktracking;
#else
    public const RegexOptions Default = RegexOptions.Compiled | RegexOptions.CultureInvariant;
#endif
    public const RegexOptions IgnoreCase = Default | RegexOptions.IgnoreCase;
    public const RegexOptions RightToLeft = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.RightToLeft;
    public const RegexOptions WithBackTracking = RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;
}
