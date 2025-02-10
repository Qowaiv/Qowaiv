namespace Email_address_format_specs;

public class Length
{
    [TestCase("the-total-length@of-an-entire-address.cannot-be-longer-than-two-hundred-and-fifty-four-characters.and-this-address-is-254-characters-exactly.so-it-should-be-valid.and-im-going-to-add-some-more-words-here.to-increase-the-length-blah-blah-blah-blah-bla.org")]
    [TestCase("i234567890_234567890_234567890_234567890_234567890_234567890_234@long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long")]
    [TestCase("Display name is ignored <i234567890(comments are ignored)_234567890_234567890_234567890_234567890_234567890_234@long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long>")]
    public void maximum_is_254(string max)
        => EmailAddress.Parse(max).Length.Should().Be(254);

    [TestCase("i234567890_234567890_234567890_234567890_234567890_234567890_234@long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long1")]
    public void not_255_or_longer(string max)
    {
        max.Length.Should().BeGreaterThanOrEqualTo(255);
        Email.ShouldBeInvalid(max);
    }
}

public class Local_part
{
    public static IEnumerable<char> WithoutLimitations
     => "0123456789"
     + "abcdefghijklmnopqrstuvwxyz"
     + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
     + "!#$%&'*+-/=?^_`{}|~";

    [TestCase("")]
    [TestCase(@"""""")]
    public void not_empty(string empty)
        => Email.ShouldBeInvalid($"{empty}@qowaiv.org");

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(17)]
    [TestCase(64)]
    public void length_between_1_and_64(int length)
        => Email.ShouldBeValid($"{new string('a', length)}@qowaiv.org");

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(17)]
    [TestCase(62)]
    public void length_between_1_and_62_quoted(int length)
    {
        Email.ShouldBeValid($@"""{new string('a', length)}""@qowaiv.org");
        Email.ShouldBeValid($@"Display <""{new string('a', length)}""@qowaiv.org>");
    }

    [TestCase(65)]
    [TestCase(66)]
    [TestCase(99)]
    public void length_not_above_64(int length)
        => Email.ShouldBeInvalid($"{new string('a', length)}@qowaiv.org");

    [TestCase(63)]
    [TestCase(64)]
    [TestCase(99)]
    public void length_above_62_quoted(int length)
    {
        Email.ShouldBeInvalid($@"""{new string('a', length)}""@qowaiv.org");
        Email.ShouldBeInvalid($@"Display <""{new string('a', length)}""@qowaiv.org>");
    }

    [TestCaseSource(nameof(WithoutLimitations))]
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

    [TestCase(@""" ""@qowaiv.org")]
    [TestCase(@"""info@qowaiv.org""@qowaiv.org")]
    [TestCase(@"""i n f o""@qowaiv.org")]
    public void quoted_allows_anything(string quoted)
         => Email.ShouldBeValid(quoted);
}

public class Domain_part
{
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(17)]
    [TestCase(63)]
    public void part_between_1_and_63(int length)
      => Email.ShouldBeValid($"info@{new string('a', length)}.qowaiv.org");

    [TestCase(64)]
    [TestCase(65)]
    [TestCase(99)]
    public void part_not_above_63(int length)
        => Email.ShouldBeInvalid($"info@{new string('a', length)}.qowaiv.org");

    [TestCase(64)]
    [TestCase(65)]
    [TestCase(99)]
    public void last_part_not_above_63(int length)
       => Email.ShouldBeInvalid($"info@{new string('a', length)}");

    public static IEnumerable<char> Forbidden => "!#$%&'*+/=?^`{}|~";

    [TestCaseSource(nameof(Forbidden))]
    public void forbidden_char(char ch)
         => Email.ShouldBeInvalid($"info@qo{ch}waiv.org");

    [Test]
    public void not_empty()
        => Email.ShouldBeInvalid("info@");

    [Test]
    public void not_length_1()
        => Email.ShouldBeInvalid("info@q");

    [TestCase("org")]
    [TestCase("com")]
    [TestCase("museum")]
    [TestCase("topleveldomain")]
    [TestCase("co.jp")]
    public void top_level_domain_can_contain_any_letter(string topLevelDomain)
        => Email.ShouldBeValid($"info@qowaiv.{topLevelDomain}");

    [TestCase("xn--bcher-kva8445foa")]
    [TestCase("xn--eckwd4c7cu47r2wf")]
    [TestCase("xn--3e0b707e")]
    public void can_be_puny_code(string punyCode)
        => Email.ShouldBeValid($"info@qowaiv.{punyCode}");

    [Test]
    public void dots_can_separate_parts()
        => Email.ShouldBeValid("info@one.two.three.4.5.qowaiv.org");

    [TestCase]
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

    [TestCase("info@-qowaiv.org")]
    [TestCase("info@qowaiv.-org")]
    public void can_not_start_with_dash(string mail)
        => Email.ShouldBeInvalid(mail);

    [TestCase("info@qowaiv.org-")]
    [TestCase("info@qowaiv-.org")]
    public void can_not_end_with_dash(string mail)
        => Email.ShouldBeInvalid(mail);

    [Test]
    public void dash_dash_is_an_allowed_sequence()
        => Email.ShouldBeValid("info@qow--aiv.org");
}

public class Address_sign
{
    [TestCase("info_at_qowaiv.org")]
    public void is_required(string email)
        => Email.ShouldBeInvalid(email);

    [Test]
    public void not_multiple_times()
        => Email.ShouldBeInvalid("info@qowaiv@org");
}

public class Display_name
{
    [TestCase(@"""Joe Smith"" info@qowaiv.org")]
    [TestCase(@"""Joe\\tSmith"" info@qowaiv.org")]
    [TestCase(@"""Joe\""Smith"" info@qowaiv.org")]
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

    [TestCase("Joe Smith <info@qowaiv.org>")]
    [TestCase(@"Test |<gaaf <info@qowaiv.org>")]
    public void Brackets(string brackets)
        => EmailAddress.Parse(brackets).Should().Be(Svo.EmailAddress);

    [TestCase("info@qowaiv.org (Joe Smith)")]
    public void Comments_afterwards(string comments)
        => EmailAddress.Parse(comments).Should().Be(Svo.EmailAddress);

    [TestCase("Display Name <info@qowaiv.org> (after name with display)")]
    [TestCase(@"""With extra  display name"" Display Name<info@qowaiv.org>")]
    public void mixing_not_allowed(string mixed)
        => Email.ShouldBeInvalid(mixed);

    [TestCase("Display Name info@qowaiv.org>")]
    [TestCase("Display Name <info@qowaiv.org>>")]
    [TestCase("Display Name >info@qowaiv.org<")]
    [TestCase("Display Name info@qowaiv.org<>")]
    [TestCase("info@qowaiv.org (")]
    [TestCase("info@qowaiv.org )")]
    [TestCase("info@qowaiv.org ))")]
    [TestCase("info@qowaiv.org ())")]
    [TestCase("info@qowaiv.org (()")]
    [TestCase("info@qowaiv.org )(")]
    [TestCase(@"""Display Name info@qowaiv.org")]
    [TestCase(@"Display Name <""Display Name info@qowaiv.org>")]
    [TestCase(@"Display"" info@qowaiv.org")]
    public void mismatches(string mismatch)
        => Email.ShouldBeInvalid(mismatch);
}

public class Comments
{
    [TestCase("info@qowaiv.org(afterwards)")]
    [TestCase("(before)info@qowaiv.org")]
    [TestCase("in(local part)fo@qowaiv.org")]
    [TestCase("info@qow(domain part)aiv.org")]
    [TestCase("in(with @)fo@qowaiv.org")]
    [TestCase("info@qow(with @)aiv.org")]
    [TestCase("in(multiple 1)fo@qow(multiple 2)aiv.or(multiple 3)g")]
    public void are_ignored(string email)
        => EmailAddress.Parse(email).Should().Be(Svo.EmailAddress);

    [TestCase("ipv4.addr@[123.1.7(some comment)2.10]")]
    [TestCase("ipv4.addr@123.1.7(some comment)2.10")]
    public void are_ignored_in_ip_based_domain(string email)
        => EmailAddress.Parse(email).Should().Be(EmailAddress.Parse("ipv4.addr@[123.1.72.10]"));

    [Test]
    public void not_nested()
        => Email.ShouldBeInvalid("in( nested(extra) )fo@qowaiv.org");

    [TestCase("inf(o@qowaiv.org")]
    [TestCase("info)@qowaiv.org")]
    [TestCase("in)wrong order(fo@qowaiv.org")]
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

    [TestCase("mailto")]
    [TestCase("MailTo")]
    [TestCase("MAILTO")]
    public void case_insensitive(string prefix)
        => EmailAddress.Parse($"{prefix}:info@qowaiv.org").Should().Be(Svo.EmailAddress);

    [TestCase("in_domain@mailto:qowaiv.org")]
    [TestCase("mailto:mailto:twice@qowaiv.org")]
    [TestCase("not_at_start_mailto:info@qowaiv.org")]
    [TestCase("at_end_mailto:@qowaiv.org")]
    public void only_once_at_start(string mail)
       => Email.ShouldBeInvalid(mail);

    [Test]
    public void not_with_comment_within()
        => Email.ShouldBeInvalid("mai(comment)lto:info@qowaiv.org");
}

public class Escape_character
{
    [TestCase("\" \"@qowaiv.org")]
    [TestCase("\"quoted\"@qowaiv.org")]
    [TestCase("\"quoted-at-sign@sld.org\"@qowaiv.org")]
    [TestCase("\"very.(),:;<>[]\\\".VERY.\\\"very@\\\\ \\\"very\\\".unusual\"@qowaiv.org")]
    [TestCase("\"()<>[]:,;@\\\\\\\"!#$%&'*+-/=?^_`{}| ~.a\"@qowaiv.org")]
    [TestCase("\"\\e\\s\\c\\a\\p\\e\\d\"@qowaiv.org")]
    public void supported(string str) => Email.ShouldBeValid(str);
}

public class Different_alphabets
{
    [TestCase("Chinese  <伊昭傑@郵件.商務>")]
    [TestCase("Greek    <θσερ@εχαμπλε.ψομ>")]
    [TestCase("Hindi    <राम@मोहन.ईन्फो>")]
    [TestCase("Japanese <あいうえお@あいうえお.com>")]
    [TestCase("Ukranian <юзер@екзампл.ком>")]
    [TestCase("Mixed    <あいうえお@domain.com>")]
    [TestCase("Mixed    <local@あいうえお.com>")]
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

    [TestCase("email@111.222.333")]
    [TestCase("email@111.222.333.256")]
    public void ip_v4_four_groups_within_range(string ip4Based)
        => Email.ShouldBeInvalid(ip4Based);

    [TestCase("email@[123.123.123.123")]
    [TestCase("email@[123.123.123].123")]
    [TestCase("email@123.123.123.123]")]
    [TestCase("email@123.123.[123.123]")]
    public void brackets_must_match(string ip4Based)
        => Email.ShouldBeInvalid(ip4Based);

    [TestCase("user@[IPv6:2001:db8:1ff::a0b:dbd0]")]
    [TestCase("valid.ipv6.addr@[IPv6:0::1]")]
    [TestCase("valid.ipv6.addr@[IPv6:2607:f0d0:1002:51::4]")]
    [TestCase("valid.ipv6.addr@[IPv6:fe80::230:48ff:fe33:bc33]")]
    [TestCase("valid.ipv6.addr@[IPv6:fe80:0000:0000:0000:0202:b3ff:fe1e:8329]")]
    [TestCase("valid.ipv6v4.addr@[IPv6:aaaa:aaaa:aaaa:aaaa:aaaa:aaaa:127.0.0.1]")]
    public void ip_v6_with_prefix(string ipv6) => Email.ShouldBeValid(ipv6);

    [Test]
    public void ip_v6_valid_with_brackets()
        => Email.ShouldBeValid("valid.ipv6.without-brackets@[2607:f0d0:1002:51::4]");

    [Test]
    public void ip_v6_valid_without_brackets()
        => Email.ShouldBeValid("valid.ipv6.without-brackets@2607:f0d0:1002:51::4");

    [TestCase("IP-and-port@127.0.0.1:25")]
    [TestCase("another-invalid-ip@127.0.0.256")]
    [TestCase("invalid-ip@127.0.0.1.26")]
    [TestCase("ipv4.with.ipv6prefix.addr@[IPv6:123.1.72.10]")]
    [TestCase("ab@988.120.150.10")]
    [TestCase("ab@120.256.256.120")]
    [TestCase("ab@120.25.1111.120")]
    [TestCase("ab@[188.120.150.10")]
    [TestCase("ab@188.120.150.10]")]
    [TestCase("ab@[188.120.150.10].com")]
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
