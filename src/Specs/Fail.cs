namespace Specs;

public class Fail
{
    [Test]
    public void Always() => Math.Sqrt(4).Should().Be(34);
}
