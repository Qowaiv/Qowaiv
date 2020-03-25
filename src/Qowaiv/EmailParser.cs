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
        private const int LocalPartMaxLength = 64;

        /// <summary>The maximum length of a (individual) domain part is 63.</summary>
        private const int DomainPartMaxLength = 63;

        private const int NotFound = -1;
        private const char At = '@';
        private const char Dot = '.';
        private const char Dash = '-';
        private const char Colon = ':';
        private const char Quote = '"';

        private const string IPv6Prefix = "IPv6:";

        /// <summary>Parses an email address string.</summary>
        /// <returns>
        /// null if the string was not a valid email address.
        /// a stripped lowercased email address if valid.
        /// </returns>
        internal static string Parse(string s)
        {
            var str = new CharBuffer(s)
               .Trim()
               .RemoveQuotedPrefix()
               .RemoveDisplayName()
               .RemoveComment()
            ;

            if (str.Empty())
            {
                return null;
            }

            // buffers.
            var local = new CharBuffer(EmailAddress.MaxLength);
            var domain = new CharBuffer(EmailAddress.MaxLength);

            var mailto = false;
            var noAt = true;
            var prev = default(char);
            var hasBrackets = false;
            var dot = NotFound;

            var end = str.Length;

            for (var pos = 0; pos < end; pos++)
            {
                var ch = str[pos];

                if (TooLong(local, domain))
                {
                    return null;
                }

                // Never a double dot.
                if (ch == Dot && prev == Dot)
                {
                    return null;
                }

                // @
                if (ch == At)
                {
                    // No @ yet, and a not empty local part, and not predicated by a dot.
                    if (noAt && local.NotEmpty() && prev != Dot)
                    {
                        noAt = false;
                        local.Add(At);
                    }
                    else
                    {
                        return null;
                    }
                }
                // Local part.
                else if (noAt)
                {
                    // If no MailTo: detected yet, we should remove it.
                    if (!mailto && ch == Colon && local.Equals(nameof(mailto), true))
                    {
                        local.Clear();
                        mailto = true;
                        continue;
                    }

                    // Don't start with a dot.
                    if (!IsValidLocal(ch) || ch == Dot && local.Empty())
                    {
                        return null;
                    }
                    local.Add(ch);
                }
                // Domain part.
                else
                {
                    if (domain.Empty() && ch == '[')
                    {
                        // Potentially an email address of the type local@[ip-address].
                        if (str[end - 1] == ']')
                        {
                            hasBrackets = true;
                            end--;
                            continue;
                        }
                        return null;
                    }

                    // Don't start with a dash or a dot.
                    if (!IsValidDomain(ch) || (domain.Empty() && (ch == Dash || ch == Dot)))
                    {
                        return null;
                    }

                    if (ch == Dot)
                    {
                        // No -.
                        if (prev == Dash)
                        {
                            return null;
                        }
                        dot = domain.Length;
                    }
                    // No .-
                    else if (ch == Dash && prev == Dot)
                    {
                        return null;
                    }
                    domain.AddLower(ch);
                }
                prev = ch;
            }

            if (noAt)
            {
                return null;
            }

            // a valid extension is only applicable without brackets.
            if (!hasBrackets && domain.IsValidDomain(dot))
            {
                return local.Add(domain);
            }

            return ValidateIPDomain(local, domain);
        }

        private static string ValidateIPDomain(CharBuffer local, CharBuffer domain)
        {
            // strips the prefix if so.
            var isIPv6 = domain.IsIPv6();

            // Validate The IP address.
            if (IPAddress.TryParse(domain, out IPAddress ip))
            {
                var isIPv4 = ip.AddressFamily == AddressFamily.InterNetwork;
                isIPv6 |= ip.AddressFamily == AddressFamily.InterNetworkV6;

                // Only IPv4 and IPv6.
                if (!isIPv4 && !isIPv6)
                {
                    return null;
                }
                // IPv6 prefix with an IPv4 address.
                if (isIPv6 && ip.AddressFamily != AddressFamily.InterNetworkV6)
                {
                    return null;
                }
                // As IPAddress parse is too forgiving.
                if (isIPv4 && domain.Count(Dot) != 3)
                {
                    return null;
                }

                return isIPv6
                    ? local.Add('[').Add(IPv6Prefix).Add(ip.ToString()).Add(']')
                    : local.Add('[').Add(ip.ToString()).Add(']');
            }
            return null;
        }

        /// <summary>Valid email address characters for the local part also include: {}|/%$&amp;#~!?*`'^=+.</summary>
        private static bool IsValidLocal(char ch)
        {
            return IsValid(ch)
                || "{}|/%$&#~!?*`'^=+".IndexOf(ch) != NotFound;
        }

        private static bool IsValidDomain(char ch) => IsValid(ch) || ch == Colon;

        /// <summary>Valid email address characters are letters, digits and ., _ and -.</summary>
        private static bool IsValid(char ch)
        {
            return "._-".IndexOf(ch) != NotFound
                || char.IsLetterOrDigit(ch);
        }

        private static bool TooLong(CharBuffer local, CharBuffer domain)
        {
            if (local.Length > LocalPartMaxLength || local.Length + (domain.Length < 2 ? 2 : domain.Length) >= EmailAddress.MaxLength)
            {
                return true;
            }
            if (domain.Length > DomainPartMaxLength)
            {
                var lastDot = domain.LastIndexOf(Dot);
                return lastDot != NotFound && domain.Length - lastDot > DomainPartMaxLength;
            }
            return false;
        }

        public static bool IsValidDomain(this CharBuffer buffer, int dot)
        {
            if (buffer.IndexOf(Colon) != NotFound)
            {
                return false;
            }

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
                var ch = buffer[i];
                if (ch < 'a' || ch > 'z')
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsIPv6(this CharBuffer buffer)
        {
            if (buffer.StartsWith(IPv6Prefix))
            {
                buffer.RemoveRange(0, IPv6Prefix.Length);
                return true;
            }
            return false;
        }

        /// <summary>Removes the email address display name from the string.</summary>
        /// <remarks>
        /// To indicate the message recipient, an email address also may have an
        /// associated display name for the recipient, which is followed by the
        /// address specification surrounded by angled brackets, for example:
        /// John Smith &lt;john.smith@example.org&gt;.
        /// </remarks>
        private static CharBuffer RemoveQuotedPrefix(this CharBuffer buffer)
        {
            if (buffer.Empty())
            {
                return buffer;
            }
            if (buffer.First() == Quote)
            {
                var escape = false;

                var pos = 1;
                var end = buffer.Length - 1;

                while (pos < end)
                {
                    var ch = buffer[pos++];

                    // The escape character is found.
                    if (ch == '\\')
                    {
                        // toggle state.
                        escape = !escape;
                    }
                    // The (potential) and character.
                    else if (ch == Quote)
                    {
                        // if not escaped.
                        if (!escape)
                        {
                            // if followed by a whitespace.
                            if (char.IsWhiteSpace(buffer[pos++]))
                            {
                                return buffer.RemoveRange(0, pos).Trim();
                            }
                            return buffer.Clear();
                        }
                        escape = false;
                    }
                    else
                    {
                        escape = false;
                    }
                }
                return buffer.Clear();
            }
            return buffer;
        }

        /// <summary>Removes the email address display name from the string.</summary>
        /// <remarks>
        /// To indicate the message recipient, an email address also may have an
        /// associated display name for the recipient, which is followed by the
        /// address specification surrounded by angled brackets, for example:
        /// John Smith &lt;john.smith@example.org&gt;.
        /// </remarks>
        private static CharBuffer RemoveDisplayName(this CharBuffer buffer)
        {
            if (buffer.Empty())
            {
                return buffer;
            }
            if (buffer.Last() == '>')
            {
                var lt = buffer.LastIndexOf('<');

                if (lt == CharBuffer.NotFound)
                {
                    return buffer.Clear();
                }
                return buffer
                    .RemoveFromEnd(1)
                    .RemoveRange(0, lt + 1);
            }
            return buffer;
        }

        /// <summary>Removes email address comments from the string.</summary>
        /// <remarks>
        /// Comments are allowed in the domain as well as in the local-part:
        /// john.smith@(comment)example.com and 
        /// john.smith@example.com(comment) are equivalent to 
        /// john.smith@example.com.
        /// </remarks>
        private static CharBuffer RemoveComment(this CharBuffer buffer)
        {
            var level = 0;
            var length = 0;

            for (var pos = buffer.Length - 1; pos > -1; pos--)
            {
                var ch = buffer[pos];
                if (ch == ')')
                {
                    if (level == 0)
                    {
                        level++;
                    }
                    // not nested.
                    else { buffer.Clear(); }
                }
                else if (ch == '(')
                {
                    if (level == 1)
                    {
                        level--;
                        buffer.RemoveRange(pos, length + 2);
                        length = 0;
                    }
                    else { buffer.Clear(); }
                }
                else if (level == 1)
                {
                    length++;
                }
            }
            if (level != 0)
            {
                return buffer.Clear();
            }
            return buffer.Trim();
        }
    }
}
