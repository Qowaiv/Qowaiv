using System.Linq;

namespace Text.Chars_specs;

public class Is
{
    [TestCase('0')]
    [TestCase('1')]
    [TestCase('8')]
    [TestCase('9')]
    public void _0_9(char c) => c.Is_0_9.Should().BeTrue();

    [TestCase('a')]
    [TestCase('b')]
    [TestCase('y')]
    [TestCase('z')]
    public void _a_z(char c) => c.Is_a_z.Should().BeTrue();

    [TestCase('A')]
    [TestCase('B')]
    [TestCase('Y')]
    [TestCase('Z')]
    public void _A_Z(char c) => c.Is_A_Z.Should().BeTrue();

    [TestCase('_')]
    [TestCase('-')]
    [TestCase('.')]
    [TestCase(' ')]
    [TestCase('\t')]
    [TestCase('\u00A0')]
    [TestCase('\u00B7')]
    [TestCase('\u2013')]
    [TestCase('\u2022')]
    public void Markup(char c) => c.Is_Markup.Should().BeTrue();

    [TestCase('0')]
    [TestCase('A')]
    public void IBAN(char c) => c.Is_IBAN.Should().BeTrue();

    [TestCase('\u007F')]
    [TestCase('A')]
    public void ASCII(char c) => c.Is_ASCII.Should().BeTrue();
}

public class Is_Not
{
    [TestCase((char)('0' - 1))]
    [TestCase((char)('9' + 1))]
    [TestCase('A')]
    [TestCase('a')]
    public void _0_9(char c) => c.Is_0_9.Should().BeFalse();

    [TestCase((char)('a' - 1))]
    [TestCase((char)('z' + 1))]
    [TestCase('A')]
    [TestCase('9')]
    [TestCase('!')]
    public void _a_z(char c) => c.Is_a_z.Should().BeFalse();

    [TestCase((char)('A' - 1))]
    [TestCase((char)('Z' + 1))]
    [TestCase('a')]
    [TestCase('9')]
    [TestCase('!')]
    public void _A_Z(char c) => c.Is_A_Z.Should().BeFalse();

    [TestCase('a')]
    [TestCase('0')]
    [TestCase('\u007F')]
    [TestCase('\u2202')]
    public void Markup(char c) => c.Is_Markup.Should().BeFalse();

    [TestCase('a')]
    [TestCase('z')]
    [TestCase('-')]
    public void IBAN(char c) => c.Is_IBAN.Should().BeFalse();

    [TestCase('\u0080')]
    [TestCase('\u0141')]
    public void ASCII(char c) => c.Is_ASCII.Should().BeFalse();
}

public class Non_ASCII
{
    [TestCase('\u0141')] // 'A' when truncated to a byte.
    [TestCase('\u015F')] // '_' when truncated to a byte.
    [TestCase('\u012E')] // '.' when truncated to a byte.
    public void is_not_categorized(char c)
    {
        c.Categories.Should().Be(default);
        c.Is_EMAIL_Punycode.Should().BeFalse();
        c.Is_IBAN.Should().BeFalse();
        c.Is_Markup.Should().BeFalse();
    }
}

public class Is_Email
{
    private static string Matching(Func<char, bool> predicate)
        => new([.. Enumerable.Range(0, 128).Select(i => (char)i).Where(predicate)]);

    [Test]
    public void is_local_contains_81_chars()
        => Matching(c => c.Is_EMAIL_Local)
        .Should().Be("!#$%&'*+-/0123456789=?ABCDEFGHIJKLMNOPQRSTUVWXYZ^_`abcdefghijklmnopqrstuvwxyz{|}~");

    [Test]
    public void is_domain_contains_63_chars()
        => Matching(c => c.Is_EMAIL_Domain)
        .Should().Be("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz");

    [Test]
    public void is_top_domain_contains_52_chars()
        => Matching(c => c.Is_EMAIL_TopDomain)
        .Should().Be("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");

    [Test]
    public void is_punycode_contains_63_chars()
        => Matching(c => c.Is_EMAIL_Punycode)
        .Should().Be("-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
}
