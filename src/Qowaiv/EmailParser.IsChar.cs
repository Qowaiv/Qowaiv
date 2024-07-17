using System.Runtime.CompilerServices;

namespace Qowaiv;

internal static partial class EmailParser
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsLocal(this char ch) => !ASCII.IsAscii(ch) || ASCII.EmailAddress.IsLocal(ch);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsDomain(this char ch) => !ASCII.IsAscii(ch) || ASCII.EmailAddress.IsDomain(ch);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsTopDomain(this char ch) => !ASCII.IsAscii(ch) || ASCII.EmailAddress.IsTopDomain(ch);

    [Pure]
    private static bool IsPunycode(this char ch) => !ASCII.IsAscii(ch) || ASCII.EmailAddress.IsPunycode(ch);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsAt(this char ch) => ch == '@';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsDot(this char ch) => ch == '.';

    [Pure]
    private static bool IsDash(this char ch) => ch == '-';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsGt(this char ch) => ch == '>';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsQuote(this char ch) => ch == '"';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsEscape(this char ch) => ch == '\\';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsBracketStart(this char ch) => ch == '[';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsBracketEnd(this char ch) => ch == ']';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsCommentStart(this char ch) => ch == '(';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsCommentEnd(this char ch) => ch == ')';
}
