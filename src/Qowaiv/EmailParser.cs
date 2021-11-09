using System.Net;
using System.Net.Sockets;

namespace Qowaiv;

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
/// ip6        => (IPv6:)? IpAdress.TryParse() &amp;&amp; Ip6mask
/// </summary>
internal static partial class EmailParser
{
    private const int LocalMaxLength = 64;
    private const int DomainPartMaxLength = 63;
    private const int NotFound = -1;

    [Pure]
    public static string Parse(string str)
        => new State(str).Root().Parsed();

    [FluentSyntax]
    private static State Root(this State state)
    {
        state.Quoted();

        if (state.Buffer.NotEmpty() && state.Input.NotEmpty())
        {
            var ch = state.Next();
            if (ch.IsAt())
            {
                if (state.Buffer.Length < 3 || state.Buffer.Length > LocalMaxLength)
                {
                    return state.Invalid();
                }
                else
                {
                    state.Result.Add(state.Buffer).Add(ch);
                    return state.Domain();
                }
            }
            else if (char.IsWhiteSpace(ch))
            {
                state.Buffer.Clear();
                state.Input.TrimLeft();
                return state.Email();
            }
            else return state.Invalid();
        }
        else return state.DisplayName().Email();
    }

    [FluentSyntax]
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

    [FluentSyntax]
    private static State Email(this State state)
        => state
        .MailTo()
        .Local()
        .Domain();

    [FluentSyntax]
    private static State MailTo(this State state)
    {
        if (state.Input.StartsWith("MAILTO:", ignoreCase: true))
        {
            state.Input.RemoveFromStart(7);
        }
        return state;
    }

    [FluentSyntax]
    private static State Local(this State state)
        => state.Input.NotEmpty() && state.Input.First().IsQuote()
        ? state.LocalQuoted()
        : state.LocalPart();

    [FluentSyntax]
    private static State LocalQuoted(this State state)
    {
        if (state.Quoted().Buffer.NotEmpty() && state.Input.NotEmpty())
        {
            var ch = state.Next();
            if (ch.IsAt() && state.Buffer.Length <= LocalMaxLength)
            {
                state.Result.Add(state.Buffer).Add(ch);
                return state;
            }
        }
        return state.Invalid();
    }

    [FluentSyntax]
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
            else return state.Invalid();
        }
        return state.Invalid();
    }

    [FluentSyntax]
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
                    || state.Buffer.Length - dot - 1 > DomainPartMaxLength)
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
            || state.ValidTopDomain(dot)))
        {
            state.Result.Add(state.Buffer);
            return state;
        }
        else return state.IP();
    }

    [FluentSyntax]
    private static State IP(this State state)
    {
        if (state.Buffer.First().IsBracketStart())
        {
            if (state.Input.Last().IsBracketEnd())
            {
                state.Buffer.RemoveFromStart(1);
                state.Input.RemoveFromEnd(1);
            }
            else return state.Invalid();
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
        else return state.Invalid();
    }

    [FluentSyntax]
    private static State Quoted(this State state)
    {
        if (!state.Input.First().IsQuote()) { return state; }

        var escaped = false;
        while (state.Input.NotEmpty())
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

    [Pure]
    private static bool ValidTopDomain(this State state, int dot)
    {
        var topDomain = state.Buffer.Substring(dot + 1);
        return topDomain.IsPunycode() || topDomain.All(IsTopDomain);
    }

    [Pure]
    private static bool IsValid(this IPAddress a, CharBuffer buffer, bool isIp6)
        => a.AddressFamily == AddressFamily.InterNetworkV6
        || (
            a.AddressFamily == AddressFamily.InterNetwork
            && !isIp6
            && buffer.Count('.') == 3);

    [Pure]
    private static bool IsPunycode(this string str)
        => str.StartsWith("xn--", StringComparison.Ordinal)
        && str.Length > 5
        && str.All(ch => IsTopDomain(ch) || ch.IsDash() || ch.IsDigit());
}
