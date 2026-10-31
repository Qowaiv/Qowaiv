#if NETSTANDARD2_0

namespace System;

internal static class ReadOnlySpanExtensions
{
    extension(ReadOnlySpan<char> span)
    {
        [Pure]
        public bool Contains(char value)
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
}
#endif
