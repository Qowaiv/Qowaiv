using NUnit.Framework;

namespace Qowaiv.UnitTests
{
    public partial class EmailValdationTest
    {
        [TestCase("w@com")]
        [TestCase("w.b.f@test.com")]
        [TestCase("w.b.f@test.museum")]
        [TestCase("a.a@test.com")]
        [TestCase("ab@288.120.150.10.com")]
        [TestCase("k.haak@12move.nl")]
        [TestCase("K.HAAK@12MOVE.NL")]
        [TestCase("email@domain.topleveldomain")]
        [TestCase("email@domain.co.jp")]
        [TestCase("FIRSTNAME-LASTNAME@d--n.com")]
        [TestCase("i234567890_234567890_234567890_234567890_234567890_234567890_234@long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long.long")]
        
        
        // https://github.com/jstedfast/EmailValidation
        [TestCase("customer/department=shipping@example.com")]
        [TestCase("!def!xyz%abc@example.com")]

        // examples from wikipedia
        [TestCase("niceandsimple@example.com")]
        [TestCase("very.common@example.com")]
        [TestCase("a.little.lengthy.but.fine@dept.example.com")]
        [TestCase("disposable.style.email.with+symbol@example.com")]
        [TestCase("postbox@com")]
        [TestCase("admin@mailserver1")]

        //// examples from https://github.com/Sembiance/email-validator
        [TestCase("country-code-tld@sld.rw")]
        [TestCase("country-code-tld@sld.uk")]
        [TestCase("letters-in-sld@123.com")]
        [TestCase("local@dash-in-sld.com")]
        [TestCase("local@sld.newTLD")]
        [TestCase("local@sub.domains.com")]
        [TestCase("one-character-third-level@a.example.com")]
        [TestCase("one-letter-sld@x.org")]
        [TestCase("punycode-numbers-in-tld@sld.xn--3e0b707e")]
        [TestCase("single-character-in-sld@x.org")]
        [TestCase("the-character-limit@for-each-part.of-the-domain.is-sixty-three-characters.this-is-exactly-sixty-three-characters-so-it-is-valid-blah-blah.com")]
        [TestCase("the-total-length@of-an-entire-address.cannot-be-longer-than-two-hundred-and-fifty-four-characters.and-this-address-is-254-characters-exactly.so-it-should-be-valid.and-im-going-to-add-some-more-words-here.to-increase-the-length-blah-blah-blah-blah-bla.org")]
        [TestCase("uncommon-tld@sld.mobi")]
        [TestCase("uncommon-tld@sld.museum")]
        [TestCase("uncommon-tld@sld.travel")]
        public void Valid(string email) => Assert.IsTrue(EmailAddress.IsValid(email), email);
    }
}
