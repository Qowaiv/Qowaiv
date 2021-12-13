namespace Obsolete_code;

[Obsolete("Will be dropped when the next major version is released.")]
public class Will_be_dropped
{
    [Test]
    public void Date_Today() => Date.Today.Should().Be(Clock.Today());

    [Test]
    public void Date_Yesterday() => Date.Yesterday.Should().Be(Clock.Yesterday());

    [Test]
    public void Date_Tomorrow() => Date.Tomorrow.Should().Be(Clock.Tomorrow());
}

[Obsolete("Will become private when the next major version is released.")]
public class Will_become_private { }
