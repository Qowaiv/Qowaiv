namespace Qowaiv.Diagnostics;

/// <summary>Helper to unify the debugger display of SVOs.</summary>
internal static class DebugDisplay
{
    public const string Empty = "{empty}";
    public const string Unknown = "{unknown}";

    [Pure]
    public static string DebuggerDisplay<TSvo>(this TSvo svo, string format) where TSvo : struct, IFormattable => svo switch
    {
        IEmpty empty when !empty.HasValue => Empty,
        IUnknown unk when !unk.IsKnown => Unknown,
        _ => string.Format(CultureInfo.InvariantCulture, format, svo),
    };
}
