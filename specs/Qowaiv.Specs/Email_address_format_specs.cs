namespace Email_address_format_specs;

public class Length
{
    [Test]
    [Arguments("the-total-length@of-an-entire-address.cannot-be-longer-than-two-hundred-and-fifty-four-characters.and-this-address-is-254-characters-exactly.so-it-should-be-valid.and-im-going-to-add-some-more-words-here.to-increase-the-length-blah-blah-blah-blah-bla.org")]
    [Arguments("i234567890_234567890_234567890_234567890_234567890_234567890_234@long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long")]
    [Arguments("Display name is ignored <i234567890(comments are ignored)_234567890_234567890_234567890_234567890_234567890_234@long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long>")]
    public void maximum_is_254(string max)
        => EmailAddress.Parse(max).Length.Should().Be(254);

    [Test]
    [Arguments("i234567890_234567890_234567890_234567890_234567890_234567890_234@long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long1")]
    public void not_255_or_longer(string max)
    {
        max.Length.Should().BeGreaterThanOrEqualTo(255);
        Email.ShouldBeInvalid(max);
    }
}

public class Local_part
{
    public static IEnumerable<char> WithoutLimitations
     => ("0123456789"
     + "abcdefghijklmnopqrstuvwxyz"
     + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
     + "!#$%&'*+-/=?^_`{}|~").ToCharArray();

    [Test]
    [Arguments("")]
    [Arguments(@"""""")]
    public void not_empty(string empty)
        => Email.ShouldBeInvalid($"{empty}@qowaiv.org");

    [Test]
    public void can_contain_emoji()
        => Email.ShouldBeValid("❤️@qowaiv.org");

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(17)]
    [Arguments(64)]
    public void length_between_1_and_64(int length)
        => Email.ShouldBeValid($"{new string('a', length)}@qowaiv.org");

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(17)]
    [Arguments(62)]
    public void length_between_1_and_62_quoted(int length)
    {
        Email.ShouldBeValid($@"""{new string('a', length)}""@qowaiv.org");
        Email.ShouldBeValid($@"Display <""{new string('a', length)}""@qowaiv.org>");
    }

    [Test]
    [Arguments(65)]
    [Arguments(66)]
    [Arguments(99)]
    public void length_not_above_64(int length)
        => Email.ShouldBeInvalid($"{new string('a', length)}@qowaiv.org");

    [Test]
    [Arguments(63)]
    [Arguments(64)]
    [Arguments(99)]
    public void length_above_62_quoted(int length)
    {
        Email.ShouldBeInvalid($@"""{new string('a', length)}""@qowaiv.org");
        Email.ShouldBeInvalid($@"Display <""{new string('a', length)}""@qowaiv.org>");
    }

    [Test]
    [MethodDataSource(nameof(WithoutLimitations))]
    public void char_without_limitations(char ch)
        => Email.ShouldBeValid($"{new string(ch, 64)}@qowaiv.org");

    [Test]
    public void dots_can_separate_parts()
        => Email.ShouldBeValid("zero.one.two.three.four.five.6.7.8.9@qowaiv.org");

    [Test]
    public void can_not_start_with_dot()
        => Email.ShouldBeInvalid(".info@qowaiv.org");

    [Test]
    public void can_not_end_with_dot()
        => Email.ShouldBeInvalid("info.@qowaiv.org");

    [Test]
    public void dot_dot_is_forbidden_sequence()
        => Email.ShouldBeInvalid("in..fo@qowaiv.org");

    [Test]
    [Arguments(@""" ""@qowaiv.org")]
    [Arguments(@"""info@qowaiv.org""@qowaiv.org")]
    [Arguments(@"""i n f o""@qowaiv.org")]
    public void quoted_allows_anything(string quoted)
         => Email.ShouldBeValid(quoted);
}

public class Domain_part
{
    [Test]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(17)]
    [Arguments(63)]
    public void part_between_1_and_63(int length)
      => Email.ShouldBeValid($"info@{new string('a', length)}.qowaiv.org");

    [Test]
    [Arguments(64)]
    [Arguments(65)]
    [Arguments(99)]
    public void part_not_above_63(int length)
        => Email.ShouldBeInvalid($"info@{new string('a', length)}.qowaiv.org");

    [Test]
    [Arguments(64)]
    [Arguments(65)]
    [Arguments(99)]
    public void last_part_not_above_63(int length)
       => Email.ShouldBeInvalid($"info@{new string('a', length)}");

    public static IEnumerable<char> Forbidden => "!#$%&'*+/=?^`{}|~".ToCharArray();

    [Test]
    [MethodDataSource(nameof(Forbidden))]
    public void forbidden_char(char ch)
         => Email.ShouldBeInvalid($"info@qo{ch}waiv.org");

    [Test]
    public void not_empty()
        => Email.ShouldBeInvalid("info@");

    [Test]
    public void not_length_1()
        => Email.ShouldBeInvalid("info@q");

    [Test]
    [Arguments("org")]
    [Arguments("com")]
    [Arguments("museum")]
    [Arguments("topleveldomain")]
    [Arguments("co.jp")]
    public void top_level_domain_can_contain_any_letter(string topLevelDomain)
        => Email.ShouldBeValid($"info@qowaiv.{topLevelDomain}");

    [Test]
    [Arguments("xn--bcher-kva8445foa")]
    [Arguments("xn--eckwd4c7cu47r2wf")]
    [Arguments("xn--3e0b707e")]
    public void can_be_punycode(string punyCode)
        => Email.ShouldBeValid($"info@qowaiv.{punyCode}");

    [Test]
    [Arguments("xn-bcher-kva8445foa")]
    [Arguments("xn--e")]
    [Arguments("xn--")]
    public void can_not_be_pseude_punycode(string pseudo)
         => Email.ShouldBeInvalid($"info@qowaiv.{pseudo}");

    [Test]
    public void dots_can_separate_parts()
        => Email.ShouldBeValid("info@one.two.three.4.5.qowaiv.org");

    [Test]
    public void dot_is_optional()
        => Email.ShouldBeValid("info@qowaiv");

    [Test]
    public void can_not_start_with_dot()
        => Email.ShouldBeInvalid("info@.qowaiv.org");

    [Test]
    public void can_not_end_with_dot()
        => Email.ShouldBeInvalid("info@qowaiv.org.");

    [Test]
    public void dot_dot_is_forbidden_sequence()
        => Email.ShouldBeInvalid("info@qowaiv..org");

    [Test]
    public void dashes_can_separate_parts()
       => Email.ShouldBeValid("info@one-two-three-4-5-qowaiv.org");

    [Test]
    [Arguments("info@-qowaiv.org")]
    [Arguments("info@qowaiv.-org")]
    public void can_not_start_with_dash(string mail)
        => Email.ShouldBeInvalid(mail);

    [Test]
    [Arguments("info@qowaiv.org-")]
    [Arguments("info@qowaiv-.org")]
    public void can_not_end_with_dash(string mail)
        => Email.ShouldBeInvalid(mail);

    [Test]
    public void dash_dash_is_an_allowed_sequence()
        => Email.ShouldBeValid("info@qow--aiv.org");
}

public class Address_sign
{
    [Test]
    [Arguments("info_at_qowaiv.org")]
    public void is_required(string email)
        => Email.ShouldBeInvalid(email);

    [Test]
    public void not_multiple_times()
        => Email.ShouldBeInvalid("info@qowaiv@org");
}

public class Display_name
{
    [Test]
    [Arguments(@"""Joe Smith"" info@qowaiv.org")]
    [Arguments(@"""Joe Smith""  info@qowaiv.org")]
    [Arguments("\"Joe Smith\"\tinfo@qowaiv.org")]
    [Arguments(@"""Joe\\tSmith"" info@qowaiv.org")]
    [Arguments(@"""Joe\""Smith"" info@qowaiv.org")]
    public void Quoted(string quoted)
        => EmailAddress.Parse(quoted).Should().Be(Svo.EmailAddress);

    [Test]
    public void quoted_requires_spacing()
        => Email.ShouldBeInvalid(@"""Joe Smith""info@qowaiv.org");

    [Test]
    public void quote_must_be_closed()
        => Email.ShouldBeInvalid(@"""Joe Smith info@qowaiv.org");

    [Test]
    public void not_with_single_quotes()
        => Email.ShouldBeInvalid("'Joe Smith' info@qowaiv.org");

    [Test]
    public void not_with_encoded_brackets()
        => Email.ShouldBeInvalid("Joe Smith &lt;info@qowaiv.org&gt;");

    [Test]
    [Arguments("Joe Smith <info@qowaiv.org>")]
    [Arguments(@"Test |<gaaf <info@qowaiv.org>")]
    public void Brackets(string brackets)
        => EmailAddress.Parse(brackets).Should().Be(Svo.EmailAddress);

    [Test]
    [Arguments("info@qowaiv.org (Joe Smith)")]
    public void Comments_afterwards(string comments)
        => EmailAddress.Parse(comments).Should().Be(Svo.EmailAddress);

    [Test]
    [Arguments("Display Name <info@qowaiv.org> (after name with display)")]
    [Arguments(@"""With extra  display name"" Display Name<info@qowaiv.org>")]
    public void mixing_not_allowed(string mixed)
        => Email.ShouldBeInvalid(mixed);

    [Test]
    [Arguments("Display Name info@qowaiv.org>")]
    [Arguments("Display Name <info@qowaiv.org>>")]
    [Arguments("Display Name >info@qowaiv.org<")]
    [Arguments("Display Name info@qowaiv.org<>")]
    [Arguments("info@qowaiv.org (")]
    [Arguments("info@qowaiv.org )")]
    [Arguments("info@qowaiv.org ))")]
    [Arguments("info@qowaiv.org ())")]
    [Arguments("info@qowaiv.org (()")]
    [Arguments("info@qowaiv.org )(")]
    [Arguments(@"""Display Name info@qowaiv.org")]
    [Arguments(@"Display Name <""Display Name info@qowaiv.org>")]
    [Arguments(@"Display"" info@qowaiv.org")]
    public void mismatches(string mismatch)
        => Email.ShouldBeInvalid(mismatch);
}

public class Comments
{
    [Test]
    [Arguments("info@qowaiv.org(afterwards)")]
    [Arguments("(before)info@qowaiv.org")]
    [Arguments("in(local part)fo@qowaiv.org")]
    [Arguments("info@qow(domain part)aiv.org")]
    [Arguments("in(with @)fo@qowaiv.org")]
    [Arguments("info@qow(with @)aiv.org")]
    [Arguments("info(direct)(attached)@qowaiv.org")]
    [Arguments("in(multiple 1)fo@qow(multiple 2)aiv.or(multiple 3)g")]
    public void are_ignored(string email)
        => EmailAddress.Parse(email).Should().Be(Svo.EmailAddress);

    [Test]
    public void are_ignored_after_quoted_local()
        => EmailAddress.Parse(@"""comments""(are ignored)@qowaiv.org")
        .ToString().Should().Be(@"""comments""@qowaiv.org");

    [Test]
    [Arguments("ipv4.addr@[123.1.7(some comment)2.10]")]
    [Arguments("ipv4.addr@123.1.7(some comment)2.10")]
    public void are_ignored_in_ip_based_domain(string email)
        => EmailAddress.Parse(email).Should().Be(EmailAddress.Parse("ipv4.addr@[123.1.72.10]"));

    [Test]
    public void not_nested()
        => Email.ShouldBeInvalid("in( nested(extra) )fo@qowaiv.org");

    [Test]
    [Arguments("inf(o@qowaiv.org")]
    [Arguments("info)@qowaiv.org")]
    [Arguments("in)wrong order(fo@qowaiv.org")]
    public void not_matching(string notMatching)
       => Email.ShouldBeInvalid(notMatching);
}

public class MailTo_prefix
{
    [Test]
    public void supported()
        => EmailAddress.Parse("mailto:info@qowaiv.org").Should().Be(Svo.EmailAddress);

    [Test]
    public void supported_for_commented()
        => EmailAddress.Parse("mailto:info(with comment)@qowaiv.org").Should().Be(Svo.EmailAddress);

    [Test]
    public void supported_for_with_display_name()
       => EmailAddress.Parse("John Smith <mailto:info@qowaiv.org>").Should().Be(Svo.EmailAddress);

    [Test]
    public void supported_for_quoted()
        => EmailAddress.Parse(@"mailto:""quoted""@qowaiv.org").Should().Be(EmailAddress.Parse(@"""quoted""@qowaiv.org"));

    [Test]
    public void supported_in_quoted()
        => EmailAddress.Parse(@"mailto:""mailto:quoted""@qowaiv.org").Should().Be(EmailAddress.Parse(@"""mailto:quoted""@qowaiv.org"));

    [Test]
    public void ignored_in_comment()
        => EmailAddress.Parse("info(mailto:)@qowaiv.org").Should().Be(Svo.EmailAddress);

    [Test]
    public void ignored_in_display_name()
        => EmailAddress.Parse("mailto:DisplayNames <info@qowaiv.org>").Should().Be(Svo.EmailAddress);

    [Test]
    [Arguments("mailto")]
    [Arguments("MailTo")]
    [Arguments("MAILTO")]
    public void case_insensitive(string prefix)
        => EmailAddress.Parse($"{prefix}:info@qowaiv.org").Should().Be(Svo.EmailAddress);

    [Test]
    [Arguments("in_domain@mailto:qowaiv.org")]
    [Arguments("mailto:mailto:twice@qowaiv.org")]
    [Arguments("not_at_start_mailto:info@qowaiv.org")]
    [Arguments("at_end_mailto:@qowaiv.org")]
    public void only_once_at_start(string mail)
       => Email.ShouldBeInvalid(mail);

    [Test]
    public void not_with_comment_within()
        => Email.ShouldBeInvalid("mai(comment)lto:info@qowaiv.org");
}

public class Escape_character
{
    [Test]
    [Arguments("\" \"@qowaiv.org")]
    [Arguments("\"quoted\"@qowaiv.org")]
    [Arguments("\"quoted-at-sign@sld.org\"@qowaiv.org")]
    [Arguments("\"very.(),:;<>[]\\\".VERY.\\\"very@\\\\ \\\"very\\\".unusual\"@qowaiv.org")]
    [Arguments("\"()<>[]:,;@\\\\\\\"!#$%&'*+-/=?^_`{}| ~.a\"@qowaiv.org")]
    [Arguments("\"\\e\\s\\c\\a\\p\\e\\d\"@qowaiv.org")]
    public void supported(string str) => Email.ShouldBeValid(str);
}

public class Different_alphabets
{
    [Test]
    [Arguments("Chinese  <伊昭傑@郵件.商務>")]
    [Arguments("Greek    <θσερ@εχαμπλε.ψομ>")]
    [Arguments("Hindi    <राम@मोहन.ईन्फो>")]
    [Arguments("Japanese <あいうえお@あいうえお.com>")]
    [Arguments("Ukranian <юзер@екзампл.ком>")]
    [Arguments("Mixed    <あいうえお@domain.com>")]
    [Arguments("Mixed    <local@あいうえお.com>")]
    public void supported(string email) => Email.ShouldBeValid(email);
}

public class IP_based
{
    [Test]
    public void ip_v4_valid_with_brackets()
        => Email.ShouldBeValid("valid.ipv4.addr@[123.1.72.10]");

    [Test]
    public void ip_v4_valid_without_brackets()
        => Email.ShouldBeValid("valid.ipv4.without-brackets@123.1.72.010");

    [Test]
    [Arguments("email@111.222.333")]
    [Arguments("email@111.222.333.256")]
    public void ip_v4_four_groups_within_range(string ip4Based)
        => Email.ShouldBeInvalid(ip4Based);

    [Test]
    [Arguments("email@[123.123.123.123")]
    [Arguments("email@[123.123.123].123")]
    [Arguments("email@123.123.123.123]")]
    [Arguments("email@123.123.[123.123]")]
    public void brackets_must_match(string ip4Based)
        => Email.ShouldBeInvalid(ip4Based);

    [Test]
    [Arguments("user@[IPv6:2001:db8:1ff::a0b:dbd0]")]
    [Arguments("valid.ipv6.addr@[IPv6:0::1]")]
    [Arguments("valid.ipv6.addr@[IPv6:2607:f0d0:1002:51::4]")]
    [Arguments("valid.ipv6.addr@[IPv6:fe80::230:48ff:fe33:bc33]")]
    [Arguments("valid.ipv6.addr@[IPv6:fe80:0000:0000:0000:0202:b3ff:fe1e:8329]")]
    [Arguments("valid.ipv6v4.addr@[IPv6:aaaa:aaaa:aaaa:aaaa:aaaa:aaaa:127.0.0.1]")]
    public void ip_v6_with_prefix(string ipv6) => Email.ShouldBeValid(ipv6);

    [Test]
    public void ip_v6_valid_with_brackets()
        => Email.ShouldBeValid("valid.ipv6.without-brackets@[2607:f0d0:1002:51::4]");

    [Test]
    public void ip_v6_valid_without_brackets()
        => Email.ShouldBeValid("valid.ipv6.without-brackets@2607:f0d0:1002:51::4");

    [Test]
    [Arguments("IP-and-port@127.0.0.1:25")]
    [Arguments("another-invalid-ip@127.0.0.256")]
    [Arguments("invalid-ip@127.0.0.1.26")]
    [Arguments("ipv4.with.ipv6prefix.addr@[IPv6:123.1.72.10]")]
    [Arguments("ab@988.120.150.10")]
    [Arguments("ab@120.256.256.120")]
    [Arguments("ab@120.25.1111.120")]
    [Arguments("ab@[188.120.150.10")]
    [Arguments("ab@188.120.150.10]")]
    [Arguments("ab@[188.120.150.10].com")]
    public void ip_v6_valid_without_brackets(string invalid)
        => Email.ShouldBeInvalid(invalid);
}

internal static class Email
{
    public static void ShouldBeValid(string str)
        => EmailAddress.Parse(str).IsEmptyOrUnknown().Should().BeFalse();

    public static void ShouldBeInvalid(string str)
        => EmailAddress.TryParse(str).Should().BeNull();
}
