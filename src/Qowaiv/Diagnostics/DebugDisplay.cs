namespace Qowaiv.Diagnostics;

/// <summary>Helper to unify th debugger display of SVO's.</summary>
internal static class DebugDisplay
{
    public const string Empty = "{empty}";
    public const string Unknown = "{unknown}";

    [Pure]
    public static string DebuggerDisplay<TSvo>(this TSvo svo, string format) where TSvo : struct, IFormattable
    => svo switch
    {
        _ when svo.Equals(default(TSvo)) && HasEmptyValue<TSvo>() => Empty,
        _ when svo.Equals(Qowaiv.Unknown.Value(typeof(TSvo))) => Unknown,
        _ => string.Format(CultureInfo.InvariantCulture, format, svo),
    };

    [Pure]
    private static bool HasEmptyValue<TSvo>() => typeof(TSvo)
        .GetInterfaces()
        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEmpty<>));
}
