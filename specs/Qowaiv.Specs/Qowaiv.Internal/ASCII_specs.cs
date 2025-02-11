namespace Text.ASCII_specs;

public class Email
{
    [Test]
    public void is_local_contains_82_chars()
    {
        var local = new string([.. Enumerable.Range(0, 127)
            .Select(i => (char)i)
            .Where(ASCII.EmailAddress.IsLocal)]);

        local.Should().Be("!#$%&'*+-./0123456789=?ABCDEFGHIJKLMNOPQRSTUVWXYZ^_`abcdefghijklmnopqrstuvwxyz{|}~");
    }

    [Test]
    public void is_domain_contains_63_chars()
    {
        var domain = new string([.. Enumerable.Range(0, 127)
            .Select(i => (char)i)
            .Where(ASCII.EmailAddress.IsDomain)]);

        domain.Should().Be("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz");
    }

    [Test]
    public void is_top_domain_contains_52_chars()
    {
        var topDomain = new string([.. Enumerable.Range(0, 127)
            .Select(i => (char)i)
            .Where(ASCII.EmailAddress.IsTopDomain)]);

        topDomain.Should().Be("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
    }

    [Test]
    public void is_punycode_contains_63_chars()
    {
        var punycode = new string([.. Enumerable.Range(0, 127)
            .Select(i => (char)i)
            .Where(ASCII.EmailAddress.IsPunycode)]);

        punycode.Should().Be("-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
    }
}
