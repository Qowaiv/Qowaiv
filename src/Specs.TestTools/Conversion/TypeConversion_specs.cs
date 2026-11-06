using Qowaiv.TestTools.Conversion;

namespace TestTools.Conversion.TypeConversion_specs;

public class With_domain_logics
{
    public class IsIdentity
    {
        [Test]
        public void true_for_same_types()
            => new TypeConversion(typeof(int), typeof(int)).IsIdentity.Should().BeTrue();

        [Test]
        public void false_for_different_types()
            => new TypeConversion(typeof(int), typeof(long)).IsIdentity.Should().BeFalse();
    }

    public class IsUpcast
    {
        [Test]
        public void true_for_same_types()
            => new TypeConversion(typeof(int), typeof(int)).IsUpcast.Should().BeTrue();

        [Test]
        public void true_when_source_assignble_from_target()
            => new TypeConversion(typeof(string), typeof(object)).IsUpcast.Should().BeTrue();

        [Test]
        public void false_when_source_not_assignble_from_target()
            => new TypeConversion(typeof(object), typeof(TypeConversion)).IsUpcast.Should().BeFalse();
    }

    [Test]
    public void Can_be_deconstructed()
    {
        var (source, target) = new TypeConversion(typeof(int), typeof(string));
        source.Should().Be(typeof(int));
        target.Should().Be(typeof(string));
    }
}
