using Qowaiv.Diagnostics.Contracts;

namespace Obsolete_code;

[Obsolete("Will be dropped when the next major version is released.")]
public class Will_be_dropped
{
}

[Obsolete("Will become private when the next major version is released.")]
public class Will_become_private { }

public class Will_seal
{
    [Test]
    public void _0_types()
    {
        var decorated = typeof(Qowaiv.Date).Assembly.GetTypes().Concat(
        typeof(Qowaiv.Data.SvoParameter).Assembly.GetTypes())
        .Where(tp => tp.GetCustomAttributes<WillBeSealedAttribute>().Any())
        .OrderBy(tp => tp.FullName);

        decorated.Should().BeEmpty();
    }
}
