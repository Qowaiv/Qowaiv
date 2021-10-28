using System.Diagnostics.Contracts;

namespace Qowaiv
{
    internal static partial class EmailParser
    {
        [Pure]
        private static bool IsLocal(this char ch)
            => ch.IsDigit()
            || ch.IsLetter()
            || ch.IsUnderscore()
            || ch.IsDot()
            || ch.IsDash()
            || ch.IsLocalASCII()
            || ch.IsNonASCII();

        [Pure]
        private static bool IsDomain(this char ch)
            => ch.IsTopDomain()
            || ch.IsDigit()
            || ch.IsUnderscore();

        [Pure]
        private static bool IsTopDomain(this char ch)
            => ch.IsLetter()
            || ch.IsNonASCII();

        [Pure]
        private static bool IsAt(this char ch) => ch == '@';
        
        [Pure]
        private static bool IsDigit(this char ch) => ch >= '0' && ch <= '9';

        [Pure]
        private static bool IsLetter(this char ch) => (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');

        [Pure]
        private static bool IsUnderscore(this char ch) => ch == '_';

        [Pure]
        private static bool IsDot(this char ch) => ch == '.';

        [Pure]
        private static bool IsDash(this char ch) => ch == '-';

        [Pure]
        private static bool IsNonASCII(this char ch) => ch > 127;

        [Pure]
        private static bool IsLocalASCII(this char ch) => "{}|/%$&#~!?*`'^=+".IndexOf(ch) != NotFound;
        [Pure]
        private static bool IsGt(this char ch) => ch == '>';

        [Pure]
        private static bool IsQuote(this char ch) => ch == '"';

        [Pure]
        private static bool IsEscape(this char ch) => ch == '\\';

        [Pure]
        private static bool IsBracketStart(this char ch) => ch == '[';

        [Pure]
        private static bool IsBracketEnd(this char ch) => ch == ']';
        
        [Pure]
        private static bool IsCommentStart(this char ch) => ch == '(';

        [Pure]
        private static bool IsCommentEnd(this char ch) => ch == ')';
    }
}
