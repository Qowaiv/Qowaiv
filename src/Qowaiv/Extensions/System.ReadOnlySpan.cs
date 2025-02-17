#if NETSTANDARD2_0

namespace System;

internal static class QowaivReadOnlySpanExtensions
{
    [Pure]
    public static bool Contains(this ReadOnlySpan<char> span, char value)
    {
        for (var i = 0; i < span.Length; i++)
        {
            if (span[i] == value)
            {
                return true;
            }
        }
        return false;
    }
}
#endif
