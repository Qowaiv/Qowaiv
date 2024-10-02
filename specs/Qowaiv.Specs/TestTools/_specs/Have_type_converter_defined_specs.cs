namespace TestTools.Should.Have_type_converter_defined_specs;

public class Discovers
{
    [Test]
    public void Defined_converter()
        => typeof(WithConverter).Should().HaveTypeConverterDefined();
}

public class Fails_on
{
    [Test]
    public void not_defined_converter()
       => typeof(object)
       .Invoking(x => x.Should().HaveTypeConverterDefined())
       .Should().Throw<AssertionException>()
       .WithMessage("There is no type converter defined for 'System.Object'.");
}

[TypeConverter(typeof(WithConverterTypeConverter))]
internal readonly struct WithConverter : IFormattable
{
    public string ToString(string? format, IFormatProvider? formatProvider) => 17.ToString(format, formatProvider);
}

internal class WithConverterTypeConverter : Qowaiv.Conversion.SvoTypeConverter<WithConverter>
{
    protected override WithConverter FromString(string? str, CultureInfo? culture) => new();
}

