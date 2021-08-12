using NUnit.Framework;
using Qowaiv;
using Qowaiv.UnitTests;
using System.Collections.Generic;

namespace Email_address_format_specs
{
    public class Local_part
    {
        public static IEnumerable<char> WithoutLimitations
         => "abcdefghijklmnopqrstuvwxyz"
         + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
         + "!#$%&'*+-/=?^_`{}|~";

        [Test]
        public void not_empty()
            => Assert.That(EmailAddress.IsValid($"@qowaiv.org"), Is.False);

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(17)]
        [TestCase(64)]
        public void length_between_1_and_64(int length)
            => Assert.That(EmailAddress.IsValid($"{new string('a', length)}@qowaiv.org"), Is.True);
        
        [TestCase(65)]
        [TestCase(66)]
        [TestCase(99)]
        public void length_not_above_64(int length)
            => Assert.That(EmailAddress.IsValid($"{new string('a', length)}@qowaiv.org"), Is.False);


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
    }
 
    public class address_sign
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

        [TestCase("Joe Smith <info@qowaiv.org>")]
        [TestCase(@"Test |<gaaf <info@qowaiv.org>")]
        public void Brackets(string brackets)
            => Assert.That(EmailAddress.Parse(brackets), Is.EqualTo(Svo.EmailAddress));

        [TestCase("info@qowaiv.org (Joe Smith)")]
        public void Comments_afterwards(string comments)
            => Assert.That(EmailAddress.Parse(comments), Is.EqualTo(Svo.EmailAddress));
    }

    public class Comments
    {
        [TestCase("info@qowaiv.org(afterwards)")]
        [TestCase("(before)info@qowaiv.org")]
        [TestCase("in(local part)fo@qowaiv.org")]
        [TestCase("info@qow(domain part)aiv.org")]
        [TestCase("in(with @)fo@qowaiv.org")]
        [TestCase("info@qow(with @)aiv.org")]
        public void are_ignored(string email)
            => Assert.That(EmailAddress.Parse(email), Is.EqualTo(Svo.EmailAddress));
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
