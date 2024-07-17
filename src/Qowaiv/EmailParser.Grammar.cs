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
        private static readonly Grammar display_name /*.*/ = new DisplayName_();
        private static readonly Grammar mailto /*.......*/ = new MailTo_();
        private static readonly Grammar localpart /*....*/ = new LocalPart_();
        private static readonly Grammar domain /*.......*/ = new Domain_();
        private static readonly Grammar quoted /*.......*/ = '"' + new Quoted_() + '"';
        private static readonly Grammar local /*........*/ = (localpart | quoted) + '@';
        private static readonly Grammar email /*........*/ = mailto + local + domain;

        public static readonly Grammar root = email | (display_name + email);

        public static Grammar operator |(Grammar l, Grammar r) => new Or(l, r);

        public static Grammar operator +(Grammar l, Grammar r) => new Concat(l, r);

        public static implicit operator Grammar(char ch) => new Char(ch);

        [Pure]
        public abstract Context? Match(Context c);

        [Pure]
        public override string ToString() => GetType().Name;
    }

    private sealed class Char(char ch) : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
            => c.In.First == ch
            ? new(c.In.Next(), c.Out + ch)
            : null;

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
            var input = c.In;
            var output = c.Out;

            while (input.NotEmpty)
            {
                var ch = input.First;

                if (ch == '@')
                {
                    return output.Length.IsInRange(1, LocalMaxLength) && !output.Last.IsDot()
                        ? new(input, output)
                        : null;
                }
                else if (ch.IsLocal())
                {
                    if (ch.IsDot() && (output.Length == 0 || output.Last.IsDot()))
                    {
                        return null;
                    }
                    output += input.First;
                }
                else
                {
                    return null;
                }
                input++;
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
                var next = c.In.Next(lt + 1).Last(out var gt);
                return gt == '>' ? new(next, c.Out) : null;
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
            var input = c.In;
            var output = c.Out;
            var escaped = false;

            while (input.NotEmpty)
            {
                var ch = input.First;

                if (!escaped && ch.IsQuote())
                {
                    return output.Length.IsInRange(2, LocalMaxLength - 1)
                        ? new(input, output)
                        : null;
                }
                else if (ch.IsEscape())
                {
                    escaped = !escaped;
                }
                else
                {
                    escaped = false;
                }
                output += ch;
                input++;
            }
            return null;
        }
    }

    private sealed class MailTo_ : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
            => c.In.StartsWithCaseInsensitive("MAILTO:")
            ? new(c.In.Next(7), c.Out)
            : c;
    }

    private sealed class Domain_ : Grammar
    {
        [Pure]
        public override Context? Match(Context c)
        {
            var input = c.In;
            var output = c.Out;

            while (input.NotEmpty)
            {
                output += input.First;
                input++;
            }
            return new Context(input, output);
        }
    }

    private readonly struct Context(CharSpan input, Chars output)
    {
        public readonly CharSpan In = input;
        public readonly Chars Out = output;

        [Pure]
        public override string ToString() => $"In: {In}, Out: {Out}";
    }
}
