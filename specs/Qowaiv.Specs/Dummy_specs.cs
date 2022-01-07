namespace Qowaiv.Specs
{
    public class Dummy_specs
    {
        [Test]
        public void X_is_42() => new Dummy().X().Should().Be(42);
    }
}
