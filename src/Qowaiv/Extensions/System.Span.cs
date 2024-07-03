namespace System;

internal static class QowaivSpanExtensions
{
    [Pure]
    public static bool TryWrite(this Span<char> span, string? value, out int charsWritten)
    {
        if (value is { Length: > 0 })
        {
            if (value.TryCopyTo(span))
            {
                charsWritten = value.Length;
                return true;
            }
            else
            {
                charsWritten = 0;
                return false;
            }
        }
        else
        {
            charsWritten = 0;
            return true;
        }
    }

    [Pure]
    public static bool TryWrite(this Span<char> span, char ch, out int charsWritten)
    {
        if (span.Length != 0)
        {
            span.Fill(ch);
            charsWritten = 1;
            return true;
        }
        else
        {
            charsWritten = 0;
            return false;
        }
    }

#if NETSTANDARD2_0
    [Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [Pure]
    private static bool TryCopyTo(this string str, Span<char> destination)
    {
        var span = new Span<char>(str.ToCharArray());
        return span.TryCopyTo(destination);
    }
#endif
}
