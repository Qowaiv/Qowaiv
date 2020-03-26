using Qowaiv.Text;
using System.Net;
using System.Net.Sockets;

namespace Qowaiv
{
    /// <summary>A dedicated parser to parse email addresses.</summary>
    /// <remarks>
    /// A regular expression to validate email addresses is potentially slow,
    /// and definably hard to read/maintain.
    /// </remarks>
    internal static class EmailParser
    {
        /// <summary>The maximum length of the local part is 64.</summary>
        private const int LocalMaxLength = 64;

        /// <summary>The maximum length of a (individual) domain part is 63.</summary>
        private const int DomainPartMaxLength = 63;

        private const int NotFound = -1;
        private const char At = '@';
        private const char Dot = '.';
        private const char Dash = '-';
        private const char Colon = ':';
        private const char Quote = '"';

        private const char BracketOpen = '[';
        private const char BracketClose = ']';

        private const string IPv6Prefix = "IPv6:";

        /// <summary>Parses an email address string.</summary>
        /// <returns>
        /// null if the string was not a valid email address.
        /// a stripped lowercased email address if valid.
        /// </returns>
        internal static string Parse(string str)
        {
            return new State(str)
                .RemoveQuotedDislayName()
                .RemoveDisplayName()
                .RemoveComment()
                .DiscoverLocal()
                .DiscoverDomain()
                .Parsed();
        }

        /// <summary>Removes the quoted email address display name from the string.</summary>
        /// <remarks>
        /// example: "John Smith" john.smith@example.org
        /// </remarks>
        private static State RemoveQuotedDislayName(this State s)
        {
            if (s.Done || s.Buffer.First() != Quote)
            {
                return s;
            }
            // try to ad the quoted block to local.
            return s.QuotedBlockToLocal(allowDisplayName: true);
        }

        private static State LocalToResult(this State s)
        {
            s.Result.Add(s.Local).Add(At);
            return s;
        }

        /// <summary>Removes the email address display name from the string.</summary>
        /// <remarks>
        /// To indicate the message recipient, an email address also may have an
        /// associated display name for the recipient, which is followed by the
        /// address specification surrounded by angled brackets, for example:
        /// John Smith &lt;john.smith@example.org&gt;.
        /// </remarks>
        private static State HandleQuoteBlock(this State s)
        {
            if (s.Done || s.Buffer.First() != Quote)
            {
                return s;
            }

            s.Local.Add(Quote);

            var escape = false;
            for (var pos = 1; pos < s.Buffer.Length - 1; pos++)
            {
                var ch = s.Buffer[pos];

                // The escape character is found.
                if (ch == '\\')
                {
                    // toggle state.
                    escape = !escape;
                }
                else if (ch == Quote && !escape)
                {
                    pos++;
                    var next = s.Buffer[pos];

                    // It is a prefix.
                    if (char.IsWhiteSpace(next))
                    {
                        // There is a second quote block, w
                        if (s.DisplayNameRemoved)
                        {
                            return s.Invalid();
                        }
                        s.DisplayNameRemoved = true;
                        s.Local.Clear();
                        s.Buffer.RemoveRange(0, pos + 1).Trim();

                        // Removes the quoted block, and search for a quoted domain.
                        return s.HandleQuoteBlock();
                    }

                    // We found a quoted 
                    if (next == At)
                    {
                        s.Result.Add(s.Local).Add(Quote).Add(At);
                        s.Buffer.RemoveRange(0, pos + 1);
                        return s;
                    }
                    return s.Invalid();
                }
                else
                {
                    escape = false;
                }
                s.Local.Add(ch);
            }

            // The quote open was never closed.
            return s.Invalid();
        }

        /// <summary>Removes the email address display name from the string.</summary>
        /// <remarks>
        /// To indicate the message recipient, an email address also may have an
        /// associated display name for the recipient, which is followed by the
        /// address specification surrounded by angled brackets, for example:
        /// John Smith &lt;john.smith@example.org&gt;.
        /// </remarks>
        private static State RemoveDisplayName(this State s)
        {
            if (s.Done || s.Buffer.Last() != '>' || s.DisplayNameRemoved)
            {
                return s;
            }

            var lt = s.Buffer.LastIndexOf('<');

            if (lt == CharBuffer.NotFound)
            {
                return s.Invalid();
            }
            s.DisplayNameRemoved = true;
            s.Buffer
                .RemoveFromEnd(1)
                .RemoveRange(0, lt + 1);

            return s.HandleQuoteBlock();
        }

        /// <summary>Removes email address comments from the string.</summary>
        /// <remarks>
        /// Comments are allowed in the domain as well as in the local-part:
        /// john.smith@(comment)example.com and 
        /// john.smith@example.com(comment) are equivalent to 
        /// john.smith@example.com.
        /// </remarks>
        private static State RemoveComment(this State s)
        {
            var level = 0;
            var length = 0;

            for (var pos = s.Buffer.Length - 1; pos > -1; pos--)
            {
                var ch = s.Buffer[pos];
                if (ch == ')')
                {
                    if (level == 0)
                    {
                        level++;
                    }
                    // not nested.
                    else
                    {
                        return s.Invalid();
                    }
                }
                else if (ch == '(')
                {
                    if (level == 1)
                    {
                        level--;
                        s.Buffer.RemoveRange(pos, length + 2);
                        length = 0;
                    }
                    else
                    {
                        return s.Invalid();
                    }
                }
                else if (level == 1)
                {
                    length++;
                }
            }
            if (level != 0)
            {
                return s.Invalid();
            }
            s.Buffer.Trim();
            return s;
        }

        private static State DiscoverLocal(this State s, bool mailto = false)
        {
            if (s.Done || s.Result.NotEmpty())
            {
                return s;
            }

            var pos = 0;
            var end = s.Buffer.Length - 1;
            var prev = default(char);

            do
            {
                var ch = s.Buffer[pos++];

                if(ch == Quote)
                {
                    return s.Local.Empty()
                        ? s.QuotedBlockToLocal()
                        : s.Invalid();
                }

                if (ch == At)
                {
                    s.Buffer.RemoveRange(0, pos);
                    // We should have a local and not ".@".
                    return s.Local.NotEmpty() && prev != Dot
                        ? s.LocalToResult()
                        : s.Invalid();
                }

                // If no MailTo: detected yet, we should remove it.
                if (!mailto && ch == Colon && s.Local.Equals(nameof(mailto), true))
                {
                    s.Buffer.RemoveRange(0, pos);
                    s.Local.Clear();
                    return s.DiscoverLocal(mailto: true);
                }

                // Don't start with a dot.
                if (!ch.IsValidLocal() || ch == Dot && s.Local.Empty())
                {
                    return s.Invalid();
                }
                s.Local.Add(ch);
                prev = ch;
            }
            while (pos < end && !s.Done);

            // If we end up here not @ was discovered.
            return s.Invalid();
        }

        private static State DiscoverDomain(this State s)
        {
            if (s.Done)
            {
                return s;
            }

            var pos = 0;
            var end = s.Buffer.Length;
            var prev = default(char);
            var isIPAddress = false;
            var dot = NotFound;

            do
            {
                var ch = s.Buffer[pos++];

                // Potentially an email address of the type local@[ip-address].
                if (ch == BracketOpen)
                {
                    if (s.Domain.Empty() && s.Buffer.Last() == BracketClose)
                    {
                        isIPAddress = true;
                        end--;
                        continue;
                    }
                    return s.Invalid();
                }

                // Don't start with a dash or a dot.
                if (s.Domain.Empty() && (ch == Dash || ch == Dot))
                {
                    return s.Invalid();
                }

                if (ch == Dot)
                {
                    // No -.
                    if (prev == Dash)
                    {
                        return s.Invalid();
                    }
                    dot = s.Domain.Length;
                }
                // No .-
                else if (ch == Dash && prev == Dot)
                {
                    return s.Invalid();
                }
                if (ch == Colon)
                {
                    isIPAddress = true;
                }
                else if (!ch.IsValidDomain())
                {
                    return s.Invalid();
                }
                s.Domain.AddLower(ch);
                prev = s.Domain.Last();
            }
            while (pos < end && !s.Done);

            // a valid extension is only applicable without brackets.
            if (!isIPAddress && s.Domain.IsValidDomain(dot))
            {
                s.Result.Add(s.Domain);
                s.Domain.Clear();
                return s;
            }
            return s.ValidateIPBasedDomain();
        }

        private static State ValidateIPBasedDomain(this State s)
        {
            if (s.Done)
            {
                return s;
            }
            // strips the prefix if so.
            var isIPv6 = s.Domain.IsIPv6();

            // Validate The IP address.
            if (!IPAddress.TryParse(s.Domain, out IPAddress ip))
            {
                return s.Invalid();
            }
            var isIPv4 = ip.AddressFamily == AddressFamily.InterNetwork;
            isIPv6 |= ip.AddressFamily == AddressFamily.InterNetworkV6;

            // Only IPv4 and IPv6.
            if (!isIPv4 && !isIPv6)
            {
                return s.Invalid();
            }
            // IPv6 prefix with an IPv4 address.
            if (isIPv6 && ip.AddressFamily != AddressFamily.InterNetworkV6)
            {
                return s.Invalid();
            }
            // As IPAddress parse is too forgiving.
            if (isIPv4 && s.Domain.Count(Dot) != 3)
            {
                return s.Invalid();
            }

            if (isIPv6)
            {
                s.Result.Add('[').Add(IPv6Prefix).Add(ip.ToString()).Add(']');
            }
            else
            {
                s.Result.Add('[').Add(ip.ToString()).Add(']');
            }
            s.Domain.Clear();
            return s;
        }

        /// <summary>Removes the email address display name from the string.</summary>
        /// <remarks>
        /// To indicate the message recipient, an email address also may have an
        /// associated display name for the recipient, which is followed by the
        /// address specification surrounded by angled brackets, for example:
        /// John Smith &lt;john.smith@example.org&gt;.
        /// </remarks>
        private static State QuotedBlockToLocal(this State s, bool allowDisplayName = false)
        {
            s.Local.Add(Quote);
            
            var pos = 1;
            var escape = false;

            while(pos < s.Buffer.Length - 1)
            {
                var ch = s.Buffer[pos++];
          
                // The escape character is found.
                if (ch == '\\')
                {
                    // toggle state.
                    escape = !escape;
                }
                else if (ch == Quote && !escape)
                {
                    s.Local.Add(Quote);
                    var next = s.Buffer[pos++];

                    s.Buffer.RemoveRange(0, pos);

                    // Quoted display name.
                    if (allowDisplayName && char.IsWhiteSpace(next))
                    {
                        s.Local.Clear();
                        s.Buffer.TrimLeft();
                        s.DisplayNameRemoved = true;
                        return s;
                    }
                    // Quoted/literal local part.
                    return next == At && pos < LocalMaxLength
                         ? s.LocalToResult()
                         : s.Invalid();
                }
                else
                {
                    escape = false;
                }
                if (pos < LocalMaxLength)
                {
                    s.Local.Add(ch);
                }
            }

            // The quote open was never closed.
            return s.Invalid();
        }

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

        private static bool IsDigit(this char ch)=> ch >= '0' && ch <= '9';
        private static bool IsLetter(this char ch) => (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        private static bool IsExtra(this char ch) => "._-".IndexOf(ch) != NotFound;
        private static bool IsNonASCII(this char ch) => ch > 127;
        private static bool IsValidDomain(this CharBuffer buffer, int dot)
        {
            var start = dot + 1;
            if (buffer.Length - start < 2)
            {
                return false;
            }

            // If there is no dot, no extra requirements.
            if (dot == NotFound)
            {
                return true;
            }

            for (var i = start; i < buffer.Length; i++)
            {
                if (!buffer[i].IsValidTopDomain())
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsIPv6(this CharBuffer buffer)
        {
            if (buffer.StartsWith(IPv6Prefix.ToLowerInvariant()))
            {
                buffer.RemoveRange(0, IPv6Prefix.Length);
                return true;
            }
            return false;
        }

        /// <summary>Internal state.</summary>
        private ref struct State
        {
            public State(string str)
            {
                Buffer = new CharBuffer(str).Trim();
                Local = new CharBuffer(LocalMaxLength);
                Domain = new CharBuffer(EmailAddress.MaxLength);
                Result = new CharBuffer(EmailAddress.MaxLength);

                DisplayNameRemoved = false;
            }

            public readonly CharBuffer Buffer;
            public readonly CharBuffer Local;
            public readonly CharBuffer Domain;
            public readonly CharBuffer Result;

            public bool DisplayNameRemoved;

            public bool Done => Buffer.Empty() || TooLong();

            private bool TooLong()
            {
                // if the local part is more then 64 characters.
                if (Local.Length > LocalMaxLength)
                {
                    return true;
                }
                // The result will be too long. 
                if (Result.Length + Domain.Length > EmailAddress.MaxLength)
                {
                    return true;
                }

                return Domain.Length > DomainPartMaxLength
                    && Domain.Length - (Domain.LastIndexOf(Dot) + 1) > DomainPartMaxLength;
            }

            public override string ToString() => $"Buffer: {Buffer}, Result:{Result}";

            public State Invalid()
            {
                Buffer.Clear();
                return this;
            }

            public string Parsed()
            {
                return Done
                    ? null
                    : Result.ToString();
            }
        }
    }
}
