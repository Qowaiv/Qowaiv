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
    internal static partial class EmailParser
    {
        /// <summary>The maximum length of the local part is 64.</summary>
        private const int LocalMaxLength = 64;

        /// <summary>The maximum length of a (individual) domain part is 63.</summary>
        private const int DomainPartMaxLength = 63;

        private const int NotFound = -1;
        private const char At = '@';
        private const char Dot = '.';
        private const char Dash = '-';
        private const char Underscore = '_';
        private const char Colon = ':';
        private const char Quote = '"';

        private const char BracketOpen = '[';
        private const char BracketClose = ']';

        private const string IPv6Prefix = "IPv6:";

        /// <summary>Parses an email address string.</summary>
        /// <returns>
        /// null if the string was not a valid email address.
        /// a stripped and normalized email address if valid.
        /// </returns>
        internal static string Parse(string str)
        {
            return new State(str)
                .CheckForQuotedBlock()
                .RemoveDisplayName()
                .RemoveComment()
                .DiscoverLocal()
                .DiscoverDomain()
                .Parsed();
        }

        /// <summary>Removes the email address display name from the string.</summary>
        /// <remarks>
        /// To indicate the message recipient, an email address also may have an
        /// associated display name for the recipient, which is followed by the
        /// address specification surrounded by angled brackets, for example:
        /// John Smith &lt;john.smith@example.org&gt;.
        /// </remarks>
        private static State CheckForQuotedBlock(this State s)
        {
            if (s.Done || s.Input.First() != Quote) { return s; }

            s.Buffer.Add(Quote);

            var pos = 1;
            var escape = false;

            while (pos < s.Input.Length - 1)
            {
                var ch = s.Input[pos++];

                // The escape character is found.
                if (ch == '\\')
                {
                    // toggle state.
                    escape = !escape;
                }
                else if (ch == Quote && !escape)
                {
                    s.Buffer.Add(Quote);
                    var next = s.Input[pos++];

                    s.Input.RemoveRange(0, pos);

                    // Quoted/literal local part.
                    if (next == At)
                    {
                        return pos < LocalMaxLength
                            ? s.LocalToResult()
                            : s.Invalid();
                    }
                    // Quoted display name.
                    if (char.IsWhiteSpace(next) && !s.DisplayNameRemoved)
                    {
                        s.DisplayNameRemoved = true;

                        s.Buffer.Clear();
                        s.Input.TrimLeft();
                        s.DisplayNameRemoved = true;

                        // if is followed by literal block, it has to be done before the remove comments.
                        return s.CheckForQuotedBlock();
                    }

                    // invalid.
                    return s.Invalid();
                }
                else
                {
                    escape = false;
                }
                if (pos < LocalMaxLength)
                {
                    s.Buffer.Add(ch);
                }
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
            if (s.Done || s.Input.Last() != '>' || s.DisplayNameRemoved) { return s; }

            var lt = s.Input.LastIndexOf('<');

            if (lt == NotFound) { return s.Invalid(); }

            s.DisplayNameRemoved = true;
            s.Input
                .RemoveFromEnd(1)
                .RemoveRange(0, lt + 1);

            return s;
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

            for (var pos = s.Input.Length - 1; pos > -1; pos--)
            {
                var ch = s.Input[pos];
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
                        s.Input.RemoveRange(pos, length + 2);
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
            s.Input.Trim();
            return s;
        }

        private static State DiscoverLocal(this State s, bool mailto = false)
        {
            if (s.Done || s.Result.NotEmpty()) { return s; }

            var pos = 0;
            var end = s.Input.Length - 2; // At least an @ and something should be there afterwards.
            var prev = default(char);

            while (pos < end && !s.Done)
            {
                var ch = s.Input[pos++];

                if (ch == At)
                {
                    s.Input.RemoveRange(0, pos);
                    // We should have a local and not ".@".
                    return s.Buffer.NotEmpty() && prev != Dot ? s.LocalToResult() : s.Invalid();
                }

                if (ch == Quote && s.Buffer.IsEmpty())
                {
                    s.DisplayNameRemoved = true;
                    return s.CheckForQuotedBlock();
                }

                // If no MailTo: detected yet, we should remove it.
                if (!mailto && ch == Colon && s.Buffer.Equals(nameof(mailto), true))
                {
                    s.Input.RemoveRange(0, pos);
                    s.Buffer.Clear();
                    return s.DiscoverLocal(mailto: true);
                }

                // Obviously, invalid characters are not allowed.
                if (!ch.IsValidLocal()) { return s.Invalid(); }

                // Don't start with a dot. and not ..
                if (ch == Dot && (prev == Dot || s.Buffer.IsEmpty())) { return s.Invalid(); }

                s.Buffer.Add(ch);
                prev = ch;
            }

            // If we end up here not @ was discovered.
            return s.Invalid();
        }

        private static State DiscoverDomain(this State s)
        {
            if (s.Done) { return s; }

            var pos = 0;
            var dot = NotFound;
            var end = s.Input.Length;
            char prev = default;
            var isIP = s.Input.First() == BracketOpen;

            while (pos < end && !s.Done)
            {
                var ch = s.Input[pos++];

                isIP |= ch == Colon;

                // Obviously, invalid characters are not allowed.
                if (!isIP && !ch.IsValidDomain()) { return s.Invalid(); }

                // Don't start with a dash or a dot.
                if (s.Buffer.IsEmpty() && (ch == Dash || ch == Dot)) { return s.Invalid(); }

                if (ch == Dot)
                {
                    // No dash or dot before the dot.
                    if (prev == Dash || prev == Dot) { return s.Invalid(); }
                    dot = pos;
                }

                // No dot before the dash.
                if (ch == Dash && prev == Dot) { return s.Invalid(); }

                s.Buffer.AddLower(ch);
                prev = ch;
            }

            if (isIP)
            {
                return s.DisoverIPAddress();
            }

            // Not allowed as last characters.
            if (prev == Dot || prev == Dash || s.Done) { return s.Invalid(); }

            if (s.Buffer.IsValidDomain(dot))
            {
                s.Result.Add(s.Buffer);
                s.Buffer.Clear();
                return s;
            }
            return s.DisoverIPAddress();
        }

        /// <summary>Tries to discover an IP address based domain.</summary>
        private static State DisoverIPAddress(this State s)
        {
            if (s.Done) { return s; }

            if (s.Buffer.First() == BracketOpen)
            {
                if (s.Buffer.Last() != BracketClose)
                {
                    return s.Invalid();
                }
                s.Buffer.RemoveFromEnd(1).RemoveRange(0, 1);
            }

            // strips the prefix if so.
            var isIPv6 = s.Buffer.IsIPv6();

            // Validate The IP address.
            if (!IPAddress.TryParse(s.Buffer, out IPAddress ip)) { return s.Invalid(); }

            var isIPv4 = ip.AddressFamily == AddressFamily.InterNetwork;
            isIPv6 |= ip.AddressFamily == AddressFamily.InterNetworkV6;

            // Only IPv4 and IPv6.
            if (!isIPv4 && !isIPv6) { return s.Invalid(); }

            // IPv6 prefix with an IPv4 address.
            if (isIPv6 && ip.AddressFamily != AddressFamily.InterNetworkV6) { return s.Invalid(); }

            // As IPAddress parse is too forgiving.
            if (isIPv4 && s.Buffer.Count(Dot) != 3) { return s.Invalid(); }

            s.Result
                .Add(BracketOpen)
                .Add(isIPv6 ? IPv6Prefix : string.Empty)
                .Add(ip.ToString())
                .Add(BracketClose);

            s.Buffer.Clear();

            return s;
        }

        private static State LocalToResult(this State s)
        {
            s.Result.Add(s.Buffer).Add(At);
            s.Buffer.Clear();
            return s;
        }

        private static bool IsValidDomain(this CharBuffer buffer, int dot)
        {
            var start = dot;
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
    }
}
