using System.Reflection;

namespace Qowaiv.Internals.Diagnostics;

internal static class DebugDisplay
{
    public const string Empty = "{empty}";
    public const string Unknown = "{unknown}";

    [Pure]
    public static string DebuggerDisplay<TSvo>(this TSvo svo, string format) where TSvo : struct, IFormattable
    {
        if (svo.Equals(default(TSvo)) && HasEmptyValue<TSvo>()) return Empty;
        else if (svo.Equals(Qowaiv.Unknown.Value(typeof(TSvo)))) return Unknown;
        else return string.Format(CultureInfo.InvariantCulture, format, svo);
    }

    [Pure]
    private static bool HasEmptyValue<TSvo>()
        => typeof(TSvo).GetCustomAttribute<SingleValueObjectAttribute>()?
        .StaticOptions.HasFlag(SingleValueStaticOptions.HasEmptyValue) ?? false;
}
