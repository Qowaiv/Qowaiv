namespace Qowaiv.Text;

/// <summary>Centralized place to define <see cref="Regex"/> options.</summary>
internal static class RegOptions
{
    /// <summary>
    /// To prevent DoS attacks exploiting regular expressions on user input,
    /// the match time-out is set 1 ms.
    /// </summary>
#if DEBUG
    public static readonly TimeSpan Timeout = TimeSpan.FromMilliseconds(10);
#else
    public static readonly TimeSpan Timeout = TimeSpan.FromMilliseconds(1);
#endif

    public static readonly TimeSpan Replacement = TimeSpan.FromMilliseconds(50);

#if NET7_0_OR_GREATER
    public const RegexOptions Default = RegexOptions.Compiled | RegexOptions.NonBacktracking;
#else
    public const RegexOptions Default = RegexOptions.Compiled;
#endif
    public const RegexOptions IgnoreCase = Default | RegexOptions.IgnoreCase;
    public const RegexOptions RightToLeft = RegexOptions.Compiled | RegexOptions.RightToLeft;
    public const RegexOptions WithBackTracking = RegexOptions.Compiled | RegexOptions.IgnoreCase;
}
