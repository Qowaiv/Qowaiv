namespace Qowaiv
{
    internal static partial class EmailParser
    {
        /// <summary>Valid email address characters for the local part also include: {}|/%$&amp;#~!?*`'^=+.</summary>
        private static bool IsValidLocal(this char ch) => IsValid(ch) || "{}|/%$&#~!?*`'^=+".IndexOf(ch) != NotFound;

        private static bool IsValidDomain(this char ch) => IsValid(ch);

        private static bool IsValidTopDomain(this char ch) => ch.IsLetter() || ch.IsNonASCII();

        /// <summary>Valid email address characters are letters, digits and ., _ and -.</summary>
        private static bool IsValid(char ch)
        {
            return ch.IsLetter()
                || ch.IsDigit()
                || ch.IsExtra()
                || ch.IsNonASCII();
        }

        private static bool IsDigit(this char ch) => ch >= '0' && ch <= '9';
        private static bool IsLetter(this char ch) => (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        private static bool IsExtra(this char ch) => ch == Dot || ch == Dash || ch == Underscore;
        private static bool IsNonASCII(this char ch) => ch > 127;
    }
}
