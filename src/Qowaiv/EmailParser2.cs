﻿using Qowaiv.Text;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Qowaiv
{
    /// <summary>
    /// 
    /// # Grammar
    /// display    => (.+ &lt; [address] &gt;) | [address] (.+) | [address]
    /// email      => [mailto] [local] [domain]
    /// mailto     => (mailto:)?
    /// local      => [quoted] | [localpart]
    /// quoted     => ".+"
    /// 
    /// localpart  => [l]{1,64}@ &amp;&amp; not ..
    /// l          => ._- [a-z][0-9] [{}|/%$&amp;#~!?*`'^=+] [non-ASCII]
    /// 
    /// domain     => [domainpart] | [ip]
    /// domainpart => [top]+(\.[d]+)* &amp;&amp; not .. | .- | -.
    /// top        => [a-z] [non-ASCII]
    /// d          => _- [a-z][0-9] [non-ASCII]
    /// ip         => [ip4] | [ip6]
    /// ip4        => IpAdress.TryParse() &amp;&amp; 3 dots &amp;&amp; Ip4mask
    /// ip6        => (IPv6:)? IpAdress.TryParse() &amp;&amp; Ip6mask
    /// </summary>
    internal static class EmailParser2
    {
        /// <summary>The maximum length of the local part is 64.</summary>
        private const int LocalMaxLength = 64;

        /// <summary>The maximum length of a (individual) domain part is 63.</summary>
        private const int DomainPartMaxLength = 63;

        private const int NotFound = -1;

        public static string Parse(string str)
            => new State(str)
                .DisplayName()
                .MailTo()
                .Local()
                .Domain()
                .Parsed();

        private static State DisplayName(this State state)
        {
            if (state.Input.IsEmpty()) { return state.Invalid(); }
            else if (state.Input.Last().IsGt())
            {
                var lt = state.Input.LastIndexOf('<');
                if (lt == NotFound)
                {
                    return state.Invalid();
                }
                else
                {
                    state.Input.RemoveFromEnd(1).RemoveRange(0, lt + 1);
                    return state;
                }
            }
            else if (state.Input.Last().IsCommentEnd())
            {
                state.Prev();
                while (state.Input.NotEmpty())
                {
                    var ch = state.Prev();
                    if (ch.IsCommentEnd()) { return state.Invalid(); }
                    else if (ch.IsCommentStart())
                    {
                        state.Input.TrimRight();
                        return state;
                    }
                }
                return state.Invalid();

            }
            else { return state; }
        }

        private static State MailTo(this State state)
        {
            if (state.Input.StartsWith("MAILTO:", ignoreCase: true))
            {
                state.Input.RemoveFromStart(7);
            }
            return state;
        }

        private static State Local(this State state)
            => state.Input.NotEmpty() && state.Input.First().IsQuote()
            ? state.LocalQuoted()
            : state.LocalPart();

        private static State LocalQuoted(this State state)
        {
            if (state.Quoted().Buffer.NotEmpty() && state.Input.NotEmpty())
            {
                var ch = state.Next();
                if (ch.IsAt())
                {
                    state.Result.Add(state.Input).Add(ch);
                    return state;
                }
            }
            return state.Invalid();
        }

        private static State LocalPart(this State state)
        {
            while (state.Input.NotEmpty() && state.Buffer.Length <= LocalMaxLength)
            {
                var ch = state.NextNoComment();

                if (ch.IsDot() && (state.Buffer.IsEmpty() || state.Buffer.Last().IsDot()))
                {
                    return state.Invalid();
                }
                else if (ch.IsAt())
                {
                    if (state.Buffer.IsEmpty() || state.Buffer.Last().IsDot())
                    {
                        return state.Invalid();
                    }
                    else
                    {
                        state.Result.Add(state.Buffer).Add(ch);
                        return state;
                    }
                }
                else if (ch.IsLocal())
                {
                    state.Buffer.Add(ch);
                }
                else { return state.Invalid(); }
            }
            return state.Invalid();
        }

        private static State Domain(this State state)
        {
            var dot = NotFound;
            state.Buffer.Clear();

            while (state.Input.NotEmpty()
                && state.Buffer.Length + state.Result.Length < EmailAddress.MaxLength)
            {
                var ch = state.NextNoComment();

                if (ch.IsDot())
                {
                    if (state.Buffer.IsEmpty()
                        || state.Input.IsEmpty()
                        || state.Buffer.Last().IsDot()
                        || state.Buffer.Last().IsDash()
                        || state.Buffer.Length - dot > DomainPartMaxLength)
                    {
                        return state.Invalid();
                    }
                    else
                    {
                        dot = state.Buffer.Length;
                        state.Buffer.Add(ch);
                    }
                }
                else if (ch.IsDash())
                {
                    if (state.Buffer.IsEmpty()
                        || state.Input.IsEmpty()
                        || state.Buffer.Last().IsDot())
                    {
                        return state.Invalid();
                    }
                    else { state.Buffer.Add(ch); }
                }
                else if (ch.IsDomain())
                {
                    state.Buffer.AddLower(ch);
                }
                else
                {
                    state.Buffer.AddLower(ch);
                    return state.IP();
                }
            }

            if (state.Input.IsEmpty()
                && state.Buffer.Length > 1
                && (dot == NotFound
                || state.Buffer.Skip(dot + 1).All(IsTopDomain)))
            {
                state.Result.Add(state.Buffer);
                return state;
            }
            else { return state.IP(); }
        }

        private static State IP(this State state)
        {
            if (state.Buffer.First().IsBracketStart())
            {
                if (state.Input.Last().IsBracketEnd())
                {
                    state.Buffer.RemoveFromStart(1);
                    state.Input.RemoveFromEnd(1);
                }
                else { return state.Invalid(); }
            }
            var isIp6 = state.Input.StartsWith("IPv6:", ignoreCase: true);
            if (isIp6) { state.Input.RemoveFromStart(6); }

            state.Buffer.Add(state.Input);

            if (IPAddress.TryParse(state.Buffer, out var ip) && ip.IsValid(state.Buffer, isIp6))
            {
                state.Result
                    .Add('[')
                    .Add(ip.AddressFamily == AddressFamily.InterNetworkV6 ? "IPv6:" : string.Empty)
                    .Add(ip.ToString())
                    .Add(']');
                return state;
            }
            else { return state.Invalid(); }
        }

        private static State Quoted(this State state)
        {
            if (state.Input.IsEmpty() || !state.Input.First().IsQuote())
            {
                return state;
            }

            var escaped = false;
            while (state.Input.NotEmpty() && state.Buffer.Length < LocalMaxLength)
            {
                var ch = state.Next();
                state.Buffer.Add(ch);

                if (!escaped && ch.IsQuote() && state.Buffer.Length > 1)
                {
                    return state;
                }
                else if (ch.IsEscape())
                {
                    escaped = !escaped;
                }
                else { escaped = false; }
            }
            return state.Invalid();
        }

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

        private static bool IsValid(this IPAddress a, CharBuffer buffer, bool isIp6)
            => a.AddressFamily == AddressFamily.InterNetworkV6
            || (
                a.AddressFamily == AddressFamily.InterNetwork
                && !isIp6
                && buffer.Count('.') == 3);

        /// <summary>Internal state.</summary>
        private ref struct State
        {
            public State(string str)
            {
                Input = str.Buffer().Trim();
                Buffer = CharBuffer.Empty(EmailAddress.MaxLength);
                Result = CharBuffer.Empty(EmailAddress.MaxLength);
            }

            public readonly CharBuffer Input;
            public readonly CharBuffer Buffer;
            public readonly CharBuffer Result;
            public override string ToString() => $"Buffer: {Input}, Result:{Result}";

            /// <summary>Gets the first <see cref="char"/> of the buffer, and removes it.</summary>
            public char Next()
            {
                var ch = Input.First();
                Input.RemoveFromStart(1);
                return ch;
            }

            public char Prev()
            {
                var ch = Input.Last();
                Input.RemoveFromEnd(1);
                return ch;
            }

            public char NextNoComment()
            {
                var ch = Next();
                if (!ch.IsCommentStart()) { return ch; }

                while (Input.NotEmpty() && !ch.IsCommentEnd())
                {
                    ch = Next();
                    if (ch.IsCommentStart()) { return default; }
                }
                return NextNoComment();
            }

            public State Invalid()
            {
                Input.Clear();
                Buffer.Clear();
                Result.Clear();
                return this;
            }

            public string Parsed() => Result.IsEmpty() ? null : Result.ToString();
        }
    }
}
