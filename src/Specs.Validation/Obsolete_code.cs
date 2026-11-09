namespace Obsolete_code;

[Obsolete("Will be dropped when the next major version is released.")]
public class Will_be_dropped
{
    [Test]
    public async Task Result_AsTask()
        => (await Qowaiv.Validation.Abstractions.Result.For(42).AsTask())
        .Should()
        .BeValid()
        .WithMessages()
        .And.Value.Should().Be(42);

    [Test]
    public void Value_on_Should_be_valid()
        => Qowaiv.Validation.Abstractions.Result.For(42).Should().BeValid().Value.Should().Be(42);
}

[Obsolete("Will become private when the next major version is released.")]
public class Will_become_private;
