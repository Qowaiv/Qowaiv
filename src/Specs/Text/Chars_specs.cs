using System.Linq;

namespace Text.Chars_specs;

public class Is
{
    [Test]
    [Arguments('0')]
    [Arguments('1')]
    [Arguments('8')]
    [Arguments('9')]
    public void _0_9(char c) => c.Is_0_9.Should().BeTrue();

    [Test]
    [Arguments('a')]
    [Arguments('b')]
    [Arguments('y')]
    [Arguments('z')]
    public void _a_z(char c) => c.Is_a_z.Should().BeTrue();

    [Test]
    [Arguments('A')]
    [Arguments('B')]
    [Arguments('Y')]
    [Arguments('Z')]
    public void _A_Z(char c) => c.Is_A_Z.Should().BeTrue();

    [Test]
    [Arguments('_')]
    [Arguments('-')]
    [Arguments('.')]
    [Arguments(' ')]
    [Arguments('\t')]
    [Arguments('\u00A0')]
    [Arguments('\u00B7')]
    [Arguments('\u2013')]
    [Arguments('\u2022')]
    public void Markup(char c) => c.Is_Markup.Should().BeTrue();

    [Test]
    [Arguments('0')]
    [Arguments('A')]
    public void IBAN(char c) => c.Is_IBAN.Should().BeTrue();

    [Test]
    [Arguments('\u007F')]
    [Arguments('A')]
    public void ASCII(char c) => c.Is_ASCII.Should().BeTrue();
}

public class Is_Not
{
    [Test]
    [Arguments((char)('0' - 1))]
    [Arguments((char)('9' + 1))]
    [Arguments('A')]
    [Arguments('a')]
    public void _0_9(char c) => c.Is_0_9.Should().BeFalse();

    [Test]
    [Arguments((char)('a' - 1))]
    [Arguments((char)('z' + 1))]
    [Arguments('A')]
    [Arguments('9')]
    [Arguments('!')]
    public void _a_z(char c) => c.Is_a_z.Should().BeFalse();

    [Test]
    [Arguments((char)('A' - 1))]
    [Arguments((char)('Z' + 1))]
    [Arguments('a')]
    [Arguments('9')]
    [Arguments('!')]
    public void _A_Z(char c) => c.Is_A_Z.Should().BeFalse();

    [Test]
    [Arguments('a')]
    [Arguments('0')]
    [Arguments('\u007F')]
    [Arguments('\u2202')]
    public void Markup(char c) => c.Is_Markup.Should().BeFalse();

    [Test]
    [Arguments('a')]
    [Arguments('z')]
    [Arguments('-')]
    public void IBAN(char c) => c.Is_IBAN.Should().BeFalse();

    [Test]
    [Arguments('\u0080')]
    [Arguments('\u0141')]
    public void ASCII(char c) => c.Is_ASCII.Should().BeFalse();
}

public class Non_ASCII
{
    [Test]
    [Arguments('\u0141')] // 'A' when truncated to a byte.
    [Arguments('\u015F')] // '_' when truncated to a byte.
    [Arguments('\u012E')] // '.' when truncated to a byte.
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
