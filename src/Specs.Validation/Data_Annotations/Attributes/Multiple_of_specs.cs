using Qowaiv.Financial;
using Qowaiv.Validation.DataAnnotations;

namespace Data_Annotations.Attributes.Multiple_of_specs;

public class Valid_for
{
    [Test]
    public void Null() => new MultipleOfAttribute(0.1).IsValid(null).Should().BeTrue();

    [Test]
    [Arguments(45, 5.000)]
    [Arguments(45, 1.000)]
    [Arguments(45, 0.100)]
    [Arguments(45, 0.010)]
    [Arguments(45, 0.001)]
    public void shorts(short value, double factor)
      => new MultipleOfAttribute(factor).IsValid(value).Should().BeTrue();

    [Test]
    [Arguments(45, 5.000)]
    [Arguments(45, 1.000)]
    [Arguments(45, 0.100)]
    [Arguments(45, 0.010)]
    [Arguments(45, 0.001)]
    public void ints(int value, double factor)
       => new MultipleOfAttribute(factor).IsValid(value).Should().BeTrue();

    [Test]
    [Arguments(45, 5.000)]
    [Arguments(45, 1.000)]
    [Arguments(45, 0.100)]
    [Arguments(45, 0.010)]
    [Arguments(45, 0.001)]
    public void longs(long value, double factor)
        => new MultipleOfAttribute(factor).IsValid(value).Should().BeTrue();

    [Test]
    [Arguments("45.000", 5.000)]
    [Arguments("45.000", 1.000)]
    [Arguments("45.100", 0.100)]
    [Arguments("45.170", 0.010)]
    [Arguments("45.178", 0.001)]
    public void decimals(decimal value, double factor)
        => new MultipleOfAttribute(factor).IsValid(value).Should().BeTrue();

    [Test]
    [Arguments("45.000", 5.000)]
    [Arguments("45.000", 1.000)]
    [Arguments("45.100", 0.100)]
    [Arguments("45.170", 0.010)]
    [Arguments("45.178", 0.001)]
    public void amounts(Amount value, double factor)
        => new MultipleOfAttribute(factor).IsValid(value).Should().BeTrue();

    [Test]
    [Arguments("EUR 45.000", 5.000)]
    [Arguments("EUR 45.000", 1.000)]
    [Arguments("EUR 45.100", 0.100)]
    [Arguments("EUR 45.170", 0.010)]
    [Arguments("EUR 45.178", 0.001)]
    public void money(Money value, double factor)
        => new MultipleOfAttribute(factor).IsValid(value).Should().BeTrue();

    [Test]
    [Arguments("45.000%", 5.000)]
    [Arguments("45.000%", 1.000)]
    [Arguments("45.100%", 0.100)]
    [Arguments("45.170%", 0.010)]
    [Arguments("45.178%", 0.001)]
    public void percentages(Percentage value, double factor)
       => new MultipleOfAttribute(factor).IsValid(value).Should().BeTrue();
}

public class Not_valid_for{
    [Test]
    [Arguments(45, 06)]
    [Arguments(45, 10)]
    public void integers(long value, double factor)
        => new MultipleOfAttribute(factor).IsValid(value).Should().BeFalse();

    [Test]
    [Arguments("45.000", 6.00)]
    [Arguments("45.500", 1.00)]
    [Arguments("45.120", 0.10)]
    [Arguments("45.178", 0.01)]
    public void decimals(decimal value, double factor)
        => new MultipleOfAttribute(factor).IsValid(value).Should().BeFalse();

    [Test]
    [Arguments("45.000", 6.00)]
    [Arguments("45.500", 1.00)]
    [Arguments("45.120", 0.10)]
    [Arguments("45.178", 0.01)]
    public void amounts(Amount value, double factor)
        => new MultipleOfAttribute(factor).IsValid(value).Should().BeFalse();

    [Test]
    [Arguments("EUR 45.000", 6.00)]
    [Arguments("EUR 45.500", 1.00)]
    [Arguments("EUR 45.120", 0.10)]
    [Arguments("EUR 45.178", 0.01)]
    public void money(Money value, double factor)
        => new MultipleOfAttribute(factor).IsValid(value).Should().BeFalse();

    [Test]
    [Arguments("45.000%", 6.00)]
    [Arguments("45.500%", 1.00)]
    [Arguments("45.120%", 0.10)]
    [Arguments("45.178%", 0.01)]
    public void percentages(Percentage value, double factor)
       => new MultipleOfAttribute(factor).IsValid(value).Should().BeFalse();

    [Test]
    [Arguments(45.0f)]
    [Arguments(float.NaN)]
    [Arguments(float.PositiveInfinity)]
    [Arguments(45.0)]
    [Arguments(double.NaN)]
    [Arguments(double.PositiveInfinity)]
    public void floating_point_types(object value)
        => value.Invoking(v => new MultipleOfAttribute(10).IsValid(v))
        .Should().Throw<UnsupportedType>();
}

public class Does_not_support
{
    [Test]
    public void doubles()
        => 12d.Invoking(v => new MultipleOfAttribute(0.1).IsValid(v))
        .Should().Throw<UnsupportedType>();

    [Test]
    public void floats()
        => 12f.Invoking(v => new MultipleOfAttribute(0.1).IsValid(v))
        .Should().Throw<UnsupportedType>();

    [Test]
    public void non_numeric_types()
        => (new object()).Invoking(v => new MultipleOfAttribute(0.1).IsValid(v))
        .Should().Throw<UnsupportedType>();

    [Test]
    [MethodDataSource(nameof(NotSupportedIConvertables))]
    public void not_supported_IConvertables(object value)
        => value.Invoking(v => new MultipleOfAttribute(10).IsValid(v))
        .Should().Throw<UnsupportedType>();

    public static IEnumerable<object?> NotSupportedIConvertables()
    {
        yield return true;
        yield return "Hello, World!";
        yield return 'C';
        yield return new DateTime(2017, 06, 11, 06, 15, 00, DateTimeKind.Local);
        yield return DBNull.Value;
    }
}

public class Initialization
{
    [Test]
    [MatrixDataSource]
    public void has_sufficient_precision([MatrixRange<int>(1, 15)] int decimals)
        => new MultipleOfAttribute(Math.Pow(10, -decimals)).Factor.Should().Be(Pow(-decimals));

    private static decimal Pow(int decimals) => new(1, 0, 0, false, scale: (byte)-decimals);
}

public class With_message
{
    [Test]
    [Arguments("nl", "De waarde van veld Total is geen veelvoud van 0,001.")]
    [Arguments("en", "The value of the Total field is not a multiple of 0.001.")]
    public void culture_dependent(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();
        new Model().ValidateWith(new AnnotatedModelValidator<Model>())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "Total"));
    }

    internal class Model
    {
        [MultipleOf(0.001)]
        public Amount Total { get; set; } = 0.12345m.Amount();

        [MultipleOf("0.001")]
        public Amount Sub { get; set; } = 0.123m.Amount();
    }
}
