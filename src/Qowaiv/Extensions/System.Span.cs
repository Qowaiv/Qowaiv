#if NET6_0_OR_GREATER
namespace System;

/// <summary>Extensions on <see cref="Span{T}"/>.</summary>
internal static class QowaivSpanExtensions
{
    /// <summary>Tries to write a nullable <see cref="string"/> to a <see cref="Span{T}"/>&lt;<see cref="char"/>&gt;.</summary>
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

    /// <summary>Tries to write a <see cref="char"/> to a <see cref="Span{T}"/>&lt;<see cref="char"/>&gt;.</summary>
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
}
#endif
