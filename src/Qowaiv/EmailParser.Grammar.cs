namespace Qowaiv;

internal static partial class EmailParser
{
    [Pure]
    public static string? Parse2(string str)
    {
        var input = new CharSpan(str).TrimLeft().TrimRight();
        var output = Chars.Init(str.Length + 10);
        var context = Grammar.root.Match(new Context(input, output));

        return context?.Out is { } email && email.Length <= EmailAddress.MaxLength
            ? email.ToString()
            : null;
    }

    /// <summary>Parses an email address with the following grammar:
    /// at         => @
    /// root       => [quoted] ( [at] [domain] | [display] [email] )
    /// display    => (.+ &lt; [email] &gt;) | [email] (.+) | [email]
    /// email      => [mailto] [local] [domain]
    /// mailto     => (mailto:)?
    /// local      => ([quoted] | [localpart]) [at]
    /// quoted     => ".+"
    ///
    /// localpart  => [l]{1,64} &amp;&amp; not ..
    /// l          => ._- [a-z][0-9] [{}|/%$&amp;#~!?*`'^=+] [non-ASCII]
    ///
    /// domain     => [domainpart] | [ip]
    /// domainpart => [top]+(\.[d]+)* &amp;&amp; not .. | .- | -.
    /// top        => [a-z] [non-ASCII]
    /// d          => _- [a-z][0-9] [non-ASCII]
    /// ip         => [ip4] | [ip6]
    /// ip4        => IpAdress.TryParse() &amp;&amp; 3 dots &amp;&amp; Ip4mask
    /// ip6        => (IPv6:)? IpAdress.TryParse() &amp;&amp; Ip6mask.
    /// </summary>
    private abstract class Grammar
    {
        private static readonly Grammar quote /*........*/ = new Ch('"', true);
        private static readonly Grammar display_name /*.*/ = new DisplayName_();
        private static readonly Grammar mailto /*.......*/ = new MailTo_();
        private static readonly Grammar localpart /*....*/ = new LocalPart_();
        private static readonly Grammar domain /*.......*/ = new Domain_() | new IP_();
        private static readonly Grammar quoted /*.......*/ = quote + new Quoted_() + quote + new Ch('@', true);
        private static readonly Grammar local /*........*/ = localpart | quoted;
        private static readonly Grammar email /*........*/ = mailto + local + domain;

        public static readonly Grammar root =
            email
            | (display_name + '<' + email + '>');

        public static Grammar operator |(Grammar l, Grammar r) => new Or(l, r);

        public static Grammar operator +(Grammar l, Grammar r) => new Concat(l, r);

        public static implicit operator Grammar(char ch) => new Ch(ch);

        [Pure]
        public abstract Context? Match(Context c);

        [Pure]
        public override string ToString() => GetType().Name;
    }

    private sealed class Ch(char ch, bool write = false) : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
            => c.In.NotEmpty && c.In.First == ch
            ? new(c.In.Next(), Chars(c))
            : null;

        [Pure]
        private Chars Chars(Context c) => write ? c.Out + ch : c.Out;

        [Pure]
        public override string ToString() => $"'{ch}'";
    }

    private sealed class Concat(Grammar l, Grammar r) : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
            => l.Match(c) is { } first && r.Match(first) is { } second
            ? second
            : null;

        [Pure]
        public override string ToString() => $"( {l} + {r} )";
    }

    private sealed class Or(Grammar l, Grammar r) : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
            => l.Match(c) ?? r.Match(c);

        [Pure]
        public override string ToString() => $"( {l} | {r} )";
    }

    private sealed class LocalPart_ : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
        {
            Context? next = c;

            while (next is { In.NotEmpty: true } curr)
            {
                var ch = curr.In.First;

                if (ch == '@')
                {
                    return curr.Out.Last != '.' && curr.Out.Length.IsInRange(1, LocalMaxLength)
                        ? curr.Next() + '@'
                        : null;
                }
                else if (ch.IsLocal())
                {
                    if (ch == '.' && (curr.Out.Length == 0 || curr.Out.Last == '.'))
                    {
                        return null;
                    }
                    curr += ch;
                    next = curr.Next();
                }
                else if (ch == '(')
                {
                    next = curr.NextNoComment();
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }

    private sealed class DisplayName_ : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
        {
            if (c.In.LastIndexOf('<') is { } lt)
            {
                return new(c.In.Next(lt), c.Out);
            }
            else
            {
                return null;
            }
        }
    }

    private sealed class Quoted_ : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
        {
            var next = c;
            var escaped = false;

            while (next is { In.NotEmpty: true })
            {
                var ch = next.In.First;

                if (!escaped && ch.IsQuote())
                {
                    return next.Out.Length.IsInRange(2, LocalMaxLength - 1)
                        ? next
                        : null;
                }
                else if (ch == '\\')
                {
                    escaped = !escaped;
                }
                else
                {
                    escaped = false;
                }
                next += ch;
                next = next.Next();
            }
            return null;
        }
    }

    private sealed class Domain_ : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
        {
            Context? next = c;

            while (next is { In.NotEmpty: true } curr)
            {
                var ch = curr.In.First;

                if (ch == '.')
                {
                    if (curr.Out.Last == '@' || curr.Out.Last == '.' || curr.Out.Last == '-')
                    {
                        return null;
                    }
                    curr += ch;
                    next = curr.Next();
                }
                else if (ch == '-')
                {
                    if (curr.Out.Last == '@' || curr.Out.Last == '.')
                    {
                        return null;
                    }
                    curr += ch;
                    next = curr.Next();
                }
                else if (ch.IsDomain())
                {
                    curr += char.ToLowerInvariant(ch);
                    next = curr.Next();
                }
                else if (ch == '(')
                {
                    next = curr.NextNoComment();
                }
                else if (ch == '>')
                {
                    return next;
                }
                else
                {
                    return null;
                }
            }
            return next is { } final
                && final.Out.Last != '.'
                && final.Out.Last != '-'
                ? final
                : null;
        }
    }

    private sealed class IP_ : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
        {
            Context? next = c;

            while (next is { In.NotEmpty: true } curr)
            {
                var ch = curr.In.First;

                if (ch == '@')
                {
                    return curr.Out.Length.IsInRange(1, LocalMaxLength) && !curr.Out.Last.IsDot()
                        ? curr
                        : null;
                }
                else if (ch.IsLocal())
                {
                    if (ch.IsDot() && (curr.Out.Length == 0 || curr.Out.Last.IsDot()))
                    {
                        return null;
                    }
                    curr += curr.In.First;
                    next = curr.Next();
                }
                else if (ch == '(')
                {
                    next = curr.NextNoComment();
                }
                else if (ch == '>')
                {
                    return next;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }

    private sealed class MailTo_ : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
            => c.In.Length >= 7 && c.In[6] == ':' && c.In.StartsWithCaseInsensitive("MAILTO")
            ? new(c.In.Next(7), c.Out)
            : c;
    }
}
