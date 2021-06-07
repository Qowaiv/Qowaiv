namespace Qowaiv
{
    internal static partial class EmailParser
    {
        private static bool IsLocal(this char ch)
            => ch.IsDigit()
            || ch.IsLetter()
            || ch.IsUnderscore()
            || ch.IsDot()
            || ch.IsDash()
            || ch.IsLocalASCII()
            || ch.IsNonASCII();

        private static bool IsDomain(this char ch)
            => ch.IsTopDomain()
            || ch.IsDigit()
            || ch.IsUnderscore();

        private static bool IsTopDomain(this char ch)
            => ch.IsLetter()
            || ch.IsNonASCII();

        private static bool IsAt(this char ch) => ch == '@';
        private static bool IsDigit(this char ch) => ch >= '0' && ch <= '9';
        private static bool IsLetter(this char ch) => (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        private static bool IsUnderscore(this char ch) => ch == '_';
        private static bool IsDot(this char ch) => ch == '.';
        private static bool IsDash(this char ch) => ch == '-';
        private static bool IsNonASCII(this char ch) => ch > 127;
        private static bool IsLocalASCII(this char ch) => "{}|/%$&#~!?*`'^=+".IndexOf(ch) != NotFound;
        private static bool IsGt(this char ch) => ch == '>';
        private static bool IsQuote(this char ch) => ch == '"';
        private static bool IsEscape(this char ch) => ch == '\\';
        private static bool IsBracketStart(this char ch) => ch == '[';
        private static bool IsBracketEnd(this char ch) => ch == ']';
        private static bool IsCommentStart(this char ch) => ch == '(';
        private static bool IsCommentEnd(this char ch) => ch == ')';
    }
}
