using System.Runtime.CompilerServices;

namespace Qowaiv;

internal static partial class Email
{
    private static class Is
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Local(char ch) => !ASCII.IsAscii(ch) || ASCII.EmailAddress.IsLocal(ch);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Domain(char ch) => !ASCII.IsAscii(ch) || ASCII.EmailAddress.IsDomain(ch);

        [Pure]
        public static bool FinalPart(ReadOnlySpan<char> part)
            => TopDomain(part) || Punycode(part);

        [Pure]
        private static bool TopDomain(ReadOnlySpan<char> part)
        {
            for (var i = 0; i < part.Length; i++)
            {
                var ch = part[i];
                if (ASCII.IsAscii(ch) && !ASCII.EmailAddress.IsTopDomain(ch)) return false;
            }
            return true;
        }

        [Pure]
        private static bool Punycode(ReadOnlySpan<char> part)
        {
            if (!part.StartsWith("xn--".AsSpan()) || part.Length <= 5) return false;

            for (var i = 4; i < part.Length; i++)
            {
                var ch = part[i];
                if (!ASCII.IsAscii(ch) || !ASCII.EmailAddress.IsPunycode(ch)) return false;
            }
            return true;
        }
    }
}
