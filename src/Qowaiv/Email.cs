using System.Net;
using System.Net.Sockets;

namespace Qowaiv;

internal static partial class Email
{
    private const int NoMatch = -1;
    private const int PartLength = 64;
    private const int TotalLength = 254;
    private const char At = '@';
    private const char Dot = '.';
    private const char Dash = '-';
    private const char Quote = '"';

    [Pure]
    public static string? Parse(ReadOnlySpan<char> str)
    {
        // The char is first written than checked for its length, hence the
        // + 1 for the buffer.
        Span<char> buffer = stackalloc char[TotalLength + 1];
        return new Parser(str, buffer, 0).Email().Result;
    }

    private ref struct Parser
    {
        public Parser(ReadOnlySpan<char> input, Span<char> buffer, int length)
        {
            Input = input;
            Buffer = buffer;
            Length = length;
        }

        private readonly ReadOnlySpan<char> Input;
        private readonly Span<char> Buffer;
        private readonly int Length;

        private readonly bool Success => Length != NoMatch;

        private readonly bool Failure => Length == NoMatch || Input.Length < 2;

        public readonly string? Result
            => Success
            ? Buffer.ToString()
            : null;

        [Pure]
        public override string ToString()
           => Success
           ? $"Input = {Input.ToString()}, Result = {Buffer.ToString()}, Length = {Length}"
           : $"Input = {Input.ToString()}, Result = {Buffer.ToString()}, Success = false";

        [Pure]
        public readonly Parser Email()
            => DisplayName()
            .MailTo()
            .Local()
            .Domain();

        [Pure]
        private readonly Parser None() => new(Input, Buffer, NoMatch);

        /// <remarks>
        /// <![CDATA[
        /// Trims:
        /// * "[display]" [email]
        /// * [display] <[email]>
        /// * [email] ([display])
        ///
        /// if defined.
        /// ]]>
        /// </remarks>
        [Pure]
        private readonly Parser DisplayName()
        {
            var quote = QuoteLength();
            if (quote != NoMatch && char.IsWhiteSpace(Input[quote]))
            {
                return new(Input[quote..].TrimStart(), Buffer, Length);
            }
            else if (Input[^1] == '>')
            {
                var lt = Input.LastIndexOf('<');
                return lt != NoMatch
                    && Input.Length - lt > 4
                    ? new(Input[(lt + 1)..^1], Buffer, Length)
                    : None();
            }
            else if (Input[^1] == ')')
            {
                var op = Input.LastIndexOf('(');

                return op == NoMatch
                    || Input[(op + 1)..^1].Contains(')')
                    ? None()
                    : new(Input[..op].TrimEnd(), Buffer, Length);
            }
            else return this;
        }

        /// <remarks>Trims MailTo: prefix if used.</remarks>
        [Pure]
        private readonly Parser MailTo()
            => Input.StartsWith("MAILTO:".AsSpan(), StringComparison.OrdinalIgnoreCase)
            ? new(Input[7..], Buffer, Length)
            : this;

        [Pure]
        private readonly Parser Local()
            => LocalRegular() is { Success: true } local
            ? local
            : LocalQuoted();

        [Pure]
        private readonly Parser LocalRegular()
        {
            if (Failure) return None();

            var (ch, index, length) = Next(-1, 0);
            char prev = default;

            while (index < Input.Length && length <= PartLength + 1)
            {
                if (Is.Local(ch)) { /* Continue. */ }
                else if (ch is Dot)
                {
                    if (prev is Dot or default(char)) return None();
                }
                else if (ch is At)
                {
                    return length is 1 || prev is Dot
                        ? None()
                        : new(Input[++index..], Buffer, length);
                }
                else return None();

                prev = ch;
                (ch, index, length) = Next(index, length);
            }
            return None();
        }

        [Pure]
        private readonly Parser LocalQuoted()
        {
            if (Failure) return None();

            var length = QuoteLength();

            if (length.IsInRange(3, PartLength) &&
                Next(length - 1, length) is { Ch: At } next)
            {
                return new(Input[(next.Index + 1)..], Buffer, next.Length);
            }
            else return None();
        }

        [Pure]
        private readonly Parser Domain()
            => RegularDomain() is { Success: true } domain
            ? domain
            : IpDomain();

        [Pure]
        private readonly Parser RegularDomain()
        {
            if (Failure) return None();

            var (ch, index, length) = Next(-1, Length);
            char prev = default;
            var part = 0;

            while (part < PartLength && index < Input.Length)
            {
                part++;
                if (length > TotalLength) return None();
                else if (Is.Domain(ch)) { /* Continue. */ }
                else if (ch is Dot)
                {
                    if (prev is Dot or Dash or default(char)) return None();
                    part = 0;
                }
                else if (ch is Dash)
                {
                    if (part is 1) return None();
                }
                else return None();

                prev = ch;
                (ch, index, length) = Next(index, length);
            }
            var result = Buffer[..length];
            var last = result[^part..];
            return prev is not Dot and not Dash
                && part < PartLength
                && Is.FinalPart(last)
                ? new(Input, result, length)
                : None();
        }

        [Pure]
        private readonly Parser IpDomain() => IpBrackets().IpV4V6();

        [Pure]
        private readonly Parser IpBrackets()
            => !Failure && Input[0] is '[' && Input[^1] is ']'
            ? new(Input[1..^1], Buffer, Length)
            : this;

        [Pure]
        private readonly Parser IpV4V6()
        {
            if (Failure) return None();

            var isIp6 = Input.StartsWith("IPV6:".AsSpan(), StringComparison.OrdinalIgnoreCase);
            var dots = 0;
            var length = Length;
            Buffer[length++] = '[';

            var (ch, index, len) = Next(isIp6 ? 4 : -1, length);

            // Strip comments
            while (index < Input.Length)
            {
                dots += ch is Dot ? 1 : 0;
                (ch, index, len) = Next(index, len);
                if (len is NoMatch or TotalLength) return None();
            }

            var str = Buffer[length..len].ToString();

            if (IPAddress.TryParse(str, out var ip) && IsValid(ip))
            {
                str = ip.AddressFamily == AddressFamily.InterNetworkV6
                    ? $"IPv6:{ip}"
                    : ip.ToString();

                for (var i = 0; i < str.Length; i++)
                {
                    Buffer[length++] = str[i];
                }
                length = Length + str.Length + 1;
                Buffer[length++] = ']';

                return new(Input, Buffer[..length], length);
            }
            else return None();

            bool IsValid(IPAddress ip)
                => ip.AddressFamily == AddressFamily.InterNetworkV6
                || (ip.AddressFamily == AddressFamily.InterNetwork
                    && !isIp6
                    && dots == 3);
        }

        [Pure]
        private readonly (char Ch, int Index, int Length) Next(int index, int length)
        {
            var comment = false;

            while (++index < Input.Length)
            {
                var ch = Input[index];
                switch (ch)
                {
                    case '(':
                        if (comment) return (default, index, NoMatch);
                        else { comment = true; }
                        break;

                    case ')':
                        if (!comment) return (default, index, NoMatch);
                        else { comment = false; }
                        break;

                    default:
                        if (!comment)
                        {
                            Buffer[length] = ch;
                            return (ch, index, length + 1);
                        }
                        break;
                }
            }
            return (default, index, length);
        }

        [Pure]
        private readonly int QuoteLength()
        {
            var escaped = false;
            var length = 0;

            if (Input[0] is not Quote) return NoMatch;

            Buffer[length++] = Quote;

            // there should at least be one char after.
            for (var i = 1; i < Input.Length - 1; i++)
            {
                var ch = Input[i];

                // Copy the local while it could fit.
                if (length < PartLength) { Buffer[length++] = ch; }

                if (!escaped && ch == Quote) return i + 1;

                escaped = ch is '\\' && !escaped;
            }
            return NoMatch;
        }
    }
}
