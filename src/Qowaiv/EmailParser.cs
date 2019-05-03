using System;
using System.Net;
using System.Linq;


namespace Qowaiv
{
    /// <summary>A dedicated parser to parse email addresses.</summary>
    /// <remarks>
    /// A regular expression to validate email addresses is potentially slow,
    /// and definably hard to read/maintain.
    /// </remarks>
    internal static class EmailParser
    {
        private const int NotFound = -1;
        private const char At = '@';
        private const char Dot = '.';
        private const char Lt = '<';
        private const char Gt = '>';
        private const char Dash = '-';

        /// <summary>Parses an email address string.</summary>
        /// <returns>
        /// null if the string was not a valid email address.
        /// a stripped lowercased email address if valid.
        /// </returns>
        internal static string Parse(string str)
        {
            var start = 0;

            // Something of the form header <local@domain>
            var withHeader = str[str.Length - 1] == Gt;

            // Search for the < to start with.
            if (withHeader)
            {
                // No < to start parsing for.
                var lt = str.IndexOf(Lt);
                if (lt == NotFound)
                {
                    return null;
                }
                start = lt + 1;
            }

            // Skip the > if with header.
            var end = str.Length - (withHeader ? 1 : 0);

            // buffers.
            var local = new char[EmailAddress.MaxLength];
            var domain = new char[EmailAddress.MaxLength];

            var withAt = false;
            var index_l = 0;
            var index_d = 0;
            var prev = default(char);
            var hasBrackets = false;
            var dot = NotFound;

            for (var pos = start; pos < end; pos++)
            {
                var ch = str[pos];

                if (TooLong(index_l, index_d))
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
                    // No @ yet, and a not empty local part.
                    if (!withAt && index_l != 0)
                    {
                        withAt = true;
                        local[index_l++] = At;
                    }
                    else
                    {
                        return null;
                    }
                }
                // Local part.
                else if (withAt)
                {
                    // Don't start with a dot.
                    if (!IsValidLocal(ch) || ch == Dot && index_l == 0)
                    {
                        return null;
                    }
                    local[index_l++] = char.ToLowerInvariant(ch);
                }
                // Domain part.
                else
                {
                    // Potentially an email address of the type local@[ip-address].
                    if (index_d == 0 && ch == '[' && str[end - 1] == ']')
                    {
                        hasBrackets = true;
                        end--;
                        continue;
                    }

                    // Don't start with a dash or a dot.
                    if (!IsValid(ch) || (index_d == 0 && (ch == Dash || ch == Dot)))
                    {
                        return null;
                    }

                    if (ch == Dot)
                    {
                        // No -.
                        if(prev == Dash)
                        {
                            return null;
                        }
                        dot = index_d;
                    }
                    // No .-
                    else if(ch == Dash && prev == Dot)
                    {
                        return null;
                    }
                    domain[index_d++] = char.ToLowerInvariant(ch);
                }
                prev = ch;
            }

            if (!withAt)
            {
                return null;
            }

            var localPart = new string(local, 0, index_l);
            var domainPart = new string(domain, 0, index_d);

            // a valid extension is only applicable without brackets.
            // in both cases a valid IP-address might save the day.
            if ((!hasBrackets && HasValidExtension(domainPart, dot)) || IsValidIpAddress(domainPart))
            {
                return localPart + domainPart;
            }
            return null;
        }

        /// <summary>Valid email address characters for the local part also include: {}|/%$&#~!?*`'^=+.</summary>
        private static bool IsValidLocal(char ch)
        {
            return IsValid(ch)
                || "{}|/%$&#~!?*`'^=+".IndexOf(ch) != NotFound;
        }
        /// <summary>Valid email address characters are letters, digits and ., _ and -.</summary>
        private static bool IsValid(char ch)
        {
            return "._-".IndexOf(ch) != NotFound
                || char.IsLetterOrDigit(ch);
        }

        private static bool TooLong(int local, int domain)
        {
            return local + (domain < 2 ? 2 : domain) >= EmailAddress.MaxLength;
        }

        private static bool HasValidExtension(string domain, int dot)
        {
            var extension = domain.Substring(dot + 1);

            return extension.Length > 1 
                && extension.All(ch => ch >= 'a' && ch <= 'z');
        }
        private static bool IsValidIpAddress(string domain) => IPAddress.TryParse(domain, out IPAddress ip);
    }
}
