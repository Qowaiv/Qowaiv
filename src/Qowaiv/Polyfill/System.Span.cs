#if NET10_0_OR_GREATER
namespace System;

internal static class SpanExtensions
{
    extension(Span<char> span)
    {
        [Pure]
        public bool TryWrite(string? value, out int charsWritten)
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
        public bool TryWrite(char ch, out int charsWritten)
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
    }
}
#endif
