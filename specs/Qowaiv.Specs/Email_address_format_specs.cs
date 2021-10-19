using NUnit.Framework;
using Qowaiv;
using Qowaiv.Specs;
using System.Collections.Generic;

namespace Email_address_format_specs
{
    public class Length
    {
        [TestCase("the-total-length@of-an-entire-address.cannot-be-longer-than-two-hundred-and-fifty-four-characters.and-this-address-is-254-characters-exactly.so-it-should-be-valid.and-im-going-to-add-some-more-words-here.to-increase-the-length-blah-blah-blah-blah-bla.org")]
        [TestCase("i234567890_234567890_234567890_234567890_234567890_234567890_234@long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long")]
        [TestCase("Display name is ignored <i234567890(comments are ignored)_234567890_234567890_234567890_234567890_234567890_234@long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long>")]
        public void maximum_is_254(string max)
        {
            Assert.That(EmailAddress.IsValid(max), Is.True);
            Assert.That(EmailAddress.Parse(max).Length, Is.EqualTo(254));
        }

        [TestCase("i234567890_234567890_234567890_234567890_234567890_234567890_234@long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long1")]
        public void not_more_then_255(string max)
        {
            Assert.That(max?.Length, Is.AtLeast(255));
            Assert.That(EmailAddress.IsValid(max), Is.False);
        }
    }

    public class Local_part
    {
        public static IEnumerable<char> WithoutLimitations
         => "abcdefghijklmnopqrstuvwxyz"
         + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
         + "!#$%&'*+-/=?^_`{}|~";

        [TestCase("")]
        [TestCase(@"""""")]
        public void not_empty(string empty)
            => Assert.That(EmailAddress.IsValid($"{empty}@qowaiv.org"), Is.False);

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(17)]
        [TestCase(64)]
        public void length_between_1_and_64(int length)
            => Assert.That(EmailAddress.IsValid($"{new string('a', length)}@qowaiv.org"), Is.True);

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(17)]
        [TestCase(62)]
        public void length_between_1_and_62_quoted(int length)
        {
            Assert.That(EmailAddress.IsValid($@"""{new string('a', length)}""@qowaiv.org"), Is.True);
            Assert.That(EmailAddress.IsValid($@"Display <""{new string('a', length)}""@qowaiv.org>"), Is.True);
        }

        [TestCase(65)]
        [TestCase(66)]
        [TestCase(99)]
        public void length_not_above_64(int length)
            => Assert.That(EmailAddress.IsValid($"{new string('a', length)}@qowaiv.org"), Is.False);

        [TestCase(63)]
        [TestCase(64)]
        [TestCase(99)]
        public void length_not_above_62_quoted(int length)
        {
            Assert.That(EmailAddress.IsValid($@"""{new string('a', length)}""@qowaiv.org"), Is.False);
            Assert.That(EmailAddress.IsValid($@"Display <""{new string('a', length)}""@qowaiv.org>"), Is.False);
        }


        [TestCaseSource(nameof(WithoutLimitations))]
        public void char_without_limitiations(char ch)
            => Assert.That(EmailAddress.IsValid($"{new string(ch, 64)}@qowaiv.org"), Is.True);

        [Test]
        public void dots_can_seperate_parts()
            => Assert.That(EmailAddress.IsValid("zero.one.two.three.four.five.6.7.8.9@qowaiv.org"), Is.True);

        [Test]
        public void can_not_start_with_dot()
            => Assert.That(EmailAddress.IsValid(".info@qowaiv.org"), Is.False);

        [Test]
        public void can_not_end_with_dot()
            => Assert.That(EmailAddress.IsValid("info.@qowaiv.org"), Is.False);

        [Test]
        public void dot_dot_is_forbidden_sequence()
            => Assert.That(EmailAddress.IsValid("in..fo@qowaiv.org"), Is.False);

        [TestCase(@""" ""@qowaiv.org")]
        [TestCase(@"""info@qowaiv.org""@qowaiv.org")]
        [TestCase(@"""i n f o""@qowaiv.org")]
        public void quoted_allows_anything(string quoted)
             => Assert.That(EmailAddress.IsValid(quoted), Is.True);
    }
 
    public class Domain_part
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(17)]
        [TestCase(63)]
        public void part_between_1_and_63(int length)
          => Assert.That(EmailAddress.IsValid($"info@{new string('a', length)}.qowaiv.org"), Is.True);

        [TestCase(64)]
        [TestCase(65)]
        [TestCase(99)]
        public void part_not_above_63(int length)
            => Assert.That(EmailAddress.IsValid($"info@{new string('a', length)}.qowaiv.org"), Is.False);

        public static IEnumerable<char> Forbidden => "!#$%&'*+/=?^`{}|~";

        [TestCaseSource(nameof(Forbidden))]
        public void forbidden_char(char ch)
             => Assert.That(EmailAddress.IsValid($"info@qo{ch}waiv.org"), Is.False);

        [Test]
        public void not_empty()
            => Assert.That(EmailAddress.IsValid("info@"), Is.False);

        [Test]
        public void not_length_1()
            => Assert.That(EmailAddress.IsValid("info@q"), Is.False);

        [TestCase("org")]
        [TestCase("com")]
        [TestCase("museum")]
        [TestCase("topleveldomain")]
        [TestCase("co.jp")]
        public void top_level_domain_can_contain_any_letter(string topLevelDomain)
            => Assert.That(EmailAddress.IsValid($"info@qowaiv.{topLevelDomain}"), Is.True);

        [TestCase("xn--bcher-kva8445foa")]
        [TestCase("xn--eckwd4c7cu47r2wf")]
        [TestCase("xn--3e0b707e")]
        public void can_be_punycode(string punycode)
            => Assert.That(EmailAddress.IsValid($"info@qowaiv.{punycode}"), Is.True);

        [Test]
        public void dots_can_seperate_parts()
            => Assert.That(EmailAddress.IsValid("info@one.two.three.4.5.qowaiv.org"), Is.True);

        [TestCase]
        public void dot_is_optional()
            => Assert.That(EmailAddress.IsValid("info@qowaiv"), Is.True);

        [Test]
        public void can_not_start_with_dot()
            => Assert.That(EmailAddress.IsValid("info@.qowaiv.org"), Is.False);

        [Test]
        public void can_not_end_with_dot()
            => Assert.That(EmailAddress.IsValid("info@qowaiv.org."), Is.False);

        [Test]
        public void dot_dot_is_forbidden_sequence()
            => Assert.That(EmailAddress.IsValid("info@qowaiv..org"), Is.False);

        [Test]
        public void dashes_can_seperate_parts()
           => Assert.That(EmailAddress.IsValid("info@one-two-three-4-5-qowaiv.org"), Is.True);

        [TestCase("info@-qowaiv.org")]
        [TestCase("info@qowaiv.-org")]
        public void can_not_start_with_dash(string mail)
           => Assert.That(EmailAddress.IsValid(mail), Is.False);

        [TestCase("info@qowaiv.org-")]
        [TestCase("info@qowaiv-.org")]
        public void can_not_end_with_dash(string mail)
            => Assert.That(EmailAddress.IsValid(mail), Is.False);

        [Test]
        public void dash_dash_is_an_allowed_sequence()
            => Assert.That(EmailAddress.IsValid("info@qow--aiv.org"), Is.True);
    }

    public class Address_sign
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("info_at_qowaiv.org")]
        public void is_required(string email)
            => Assert.That(EmailAddress.IsValid(email), Is.False);

        [Test]
        public void not_multiple_times()
            => Assert.That(EmailAddress.IsValid("info@qowaiv@org"), Is.False);
    }

    public class Display_name
    {
        [TestCase(@"""Joe Smith"" info@qowaiv.org")]
        [TestCase(@"""Joe\\tSmith"" info@qowaiv.org")]
        [TestCase(@"""Joe\""Smith"" info@qowaiv.org")]
        public void Quoted(string quoted)
            => Assert.That(EmailAddress.Parse(quoted), Is.EqualTo(Svo.EmailAddress));

        [Test]
        public void quoted_requires_spacing()
            => Assert.That(EmailAddress.IsValid(@"""Joe Smith""info@qowaiv.org"), Is.False);

        [Test]
        public void quote_must_be_closed()
            => Assert.That(EmailAddress.IsValid(@"""Joe Smith info@qowaiv.org"), Is.False);

        [Test]
        public void not_with_single_quotes()
            => Assert.That(EmailAddress.IsValid("'Joe Smith' info@qowaiv.org"), Is.False);

        [Test]
        public void not_with_encoded_brackets()
            => Assert.That(EmailAddress.IsValid("Joe Smith &lt;info@qowaiv.org&gt;"), Is.False);

        [TestCase("Joe Smith <info@qowaiv.org>")]
        [TestCase(@"Test |<gaaf <info@qowaiv.org>")]
        public void Brackets(string brackets)
            => Assert.That(EmailAddress.Parse(brackets), Is.EqualTo(Svo.EmailAddress));

        [TestCase("info@qowaiv.org (Joe Smith)")]
        public void Comments_afterwards(string comments)
            => Assert.That(EmailAddress.Parse(comments), Is.EqualTo(Svo.EmailAddress));

        [TestCase("Display Name <info@qowaiv.org> (after name with display)")]
        [TestCase(@"""With extra  display name"" Display Name<info@qowaiv.org>")]
        public void mixing_not_allowed(string mixed)
            => Assert.That(EmailAddress.IsValid(mixed), Is.False);

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
            => Assert.That(EmailAddress.IsValid(mismatch), Is.False);
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
            => Assert.That(EmailAddress.Parse(email), Is.EqualTo(Svo.EmailAddress));
        
        [Test]
        public void not_nested()
            => Assert.That(EmailAddress.IsValid("in( nested(extra) )fo@qowaiv.org"), Is.False);

        [TestCase("inf(o@qowaiv.org")]
        [TestCase("info)@qowaiv.org")]
        [TestCase("in)wrong order(fo@qowaiv.org")]
        public void not_matching(string notMatching)
           => Assert.That(EmailAddress.IsValid("notMatching"), Is.False);
    }

    public class MailTo_prefix
    {
        [Test]
        public void supported()
            => Assert.That(EmailAddress.Parse("mailto:info@qowaiv.org"), Is.EqualTo(Svo.EmailAddress));

        [Test]
        public void supported_for_commented()
            => Assert.That(EmailAddress.Parse("mailto:info(with comment)@qowaiv.org"), Is.EqualTo(Svo.EmailAddress));

        [Test]
        public void supported_for_with_display_name()
           => Assert.That(EmailAddress.Parse("John Smith <mailto:info@qowaiv.org>"), Is.EqualTo(Svo.EmailAddress));

        [Test]
        public void supported_for_quoted()
            => Assert.That(EmailAddress.Parse(@"mailto:""quoted""@qowaiv.org"), Is.EqualTo(EmailAddress.Parse(@"""quoted""@qowaiv.org")));

        [Test]
        public void supported_in_quoted()
            => Assert.That(EmailAddress.Parse(@"mailto:""mailto:quoted""@qowaiv.org"), Is.EqualTo(EmailAddress.Parse(@"""mailto:quoted""@qowaiv.org")));

        [Test]
        public void ignored_in_comment()
            => Assert.That(EmailAddress.Parse("info(mailto:)@qowaiv.org"), Is.EqualTo(Svo.EmailAddress));

        [Test]
        public void ignored_in_display_name()
            => Assert.That(EmailAddress.Parse("mailto:DisplayNames <info@qowaiv.org>"), Is.EqualTo(Svo.EmailAddress));

        [TestCase("mailto")]
        [TestCase("MailTo")]
        [TestCase("MAILTO")]
        public void case_insensitive(string prefix)
            => Assert.That(EmailAddress.Parse($"{prefix}:info@qowaiv.org"), Is.EqualTo(Svo.EmailAddress));

        [TestCase("in_domain@mailto:qowaiv.org")]
        [TestCase("mailto:mailto:twice@qowaiv.org")]
        [TestCase("not_at_start_mailto:info@qowaiv.org")]
        [TestCase("at_end_mailto:@qowaiv.org")]
        public void only_once_at_start(string mail)
           => Assert.That(EmailAddress.IsValid(mail), Is.False);

        [Test]    
        public void not_with_comment_within()
            => Assert.That(EmailAddress.IsValid("mai(comment)lto:info@qowaiv.org"), Is.False);

    }

    public class Escape_character
    {
        [TestCase("\" \"@qowaiv.org")]
        [TestCase("\"quoted\"@qowaiv.org")]
        [TestCase("\"quoted-at-sign@sld.org\"@qowaiv.org")]
        [TestCase("\"very.(),:;<>[]\\\".VERY.\\\"very@\\\\ \\\"very\\\".unusual\"@qowaiv.org")]
        [TestCase("\"()<>[]:,;@\\\\\\\"!#$%&'*+-/=?^_`{}| ~.a\"@qowaiv.org")]
        [TestCase("\"\\e\\s\\c\\a\\p\\e\\d\"@qowaiv.org")]
        public void supported(string str)
            => Assert.That(EmailAddress.IsValid(str), Is.True);
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
        public void supported(string email)
            => Assert.That(EmailAddress.IsValid(email), Is.True);
    }

    public class IP_based
    {
        [Test]
        public void ip_v4_valid_with_brackets()
            => Assert.That(EmailAddress.IsValid("valid.ipv4.addr@[123.1.72.10]"), Is.True);

        [Test]
        public void ip_v4_valid_without_brackets()
            => Assert.That(EmailAddress.IsValid("valid.ipv4.without-brackets@123.1.72.010"), Is.True);


        [TestCase("email@111.222.333")]
        [TestCase("email@111.222.333.256")]
        public void ip_v4_four_groups_within_range(string ip4Based)
            => Assert.That(EmailAddress.IsValid(ip4Based), Is.False);

        [TestCase("email@[123.123.123.123")]
        [TestCase("email@[123.123.123].123")]
        [TestCase("email@123.123.123.123]")]
        [TestCase("email@123.123.[123.123]")]
        public void brackets_must_match(string ip4Based)
            => Assert.That(EmailAddress.IsValid(ip4Based), Is.False);

        [TestCase("user@[IPv6:2001:db8:1ff::a0b:dbd0]")]
        [TestCase("valid.ipv6.addr@[IPv6:0::1]")]
        [TestCase("valid.ipv6.addr@[IPv6:2607:f0d0:1002:51::4]")]
        [TestCase("valid.ipv6.addr@[IPv6:fe80::230:48ff:fe33:bc33]")]
        [TestCase("valid.ipv6.addr@[IPv6:fe80:0000:0000:0000:0202:b3ff:fe1e:8329]")]
        [TestCase("valid.ipv6v4.addr@[IPv6:aaaa:aaaa:aaaa:aaaa:aaaa:aaaa:127.0.0.1]")]
        public void ip_v6_with_prefix(string ipv6)
            => Assert.That(EmailAddress.IsValid(ipv6), Is.True);

        [Test]
        public void ip_v6_valid_with_brackets()
            => Assert.That(EmailAddress.IsValid("valid.ipv6.without-brackets@[2607:f0d0:1002:51::4]"), Is.True);

        [Test]
        public void ip_v6_valid_without_brackets()
            => Assert.That(EmailAddress.IsValid("valid.ipv6.without-brackets@2607:f0d0:1002:51::4"), Is.True);

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
        public void ip_v6_valid_wixthout_brackets(string invalid)
            => Assert.That(EmailAddress.IsValid(invalid), Is.False);
    }
}
