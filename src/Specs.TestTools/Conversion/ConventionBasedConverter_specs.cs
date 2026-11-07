using Qowaiv.Financial;
using Qowaiv.Humanizer;
using Qowaiv.TestTools.Conversion;

namespace TestTools.Conversion.ConventionBasedConverter_specs;

public class Converts
{
    [Test]
    public void _null_as_null()
        => new ConventionBasedConverter(typeof(string), typeof(string)).Convert(null).Should().BeNull();

    [Test]
    public void TypeConverterBased_when_possible()
        => new ConventionBasedConverter(typeof(string), typeof(int)).Convert("42").Should().Be(42);

    [Test]
    public void Cast_when_possible()
        => new ConventionBasedConverter(typeof(int), typeof(Castable)).Convert(42).Should().Be(new Castable(42));

    [Test]
    public void System_Convert_ChangeType_as_last_restort()
        => new ConventionBasedConverter(typeof(int), typeof(bool)).Convert(1).Should().Be(true);

    [Test]
    public void string_to_amount()
        => new ConventionBasedConverter(typeof(string), typeof(Amount)).Convert("12.34").Should().Be(12.34m.Amount());

    internal readonly record struct Castable(int Value)
    {
        public static implicit operator Castable(int v) => new(v);
    }
}
