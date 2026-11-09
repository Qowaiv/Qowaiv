using Qowaiv.Validation.Fluent;
using Specs.Fluent.Models;

namespace Fluent_validation.Number_specs;

public class IsPositive
{
    [Test]
    public void Valid_for_null() => new ZeroModel
    {
        NullableNumber = null,
        Number = 42,
    }
    .ValidateWith(new IsPositiveValidator())
    .Should().BeValid().WithoutMessages();

    [Arguments(double.Epsilon)]
    [Arguments(double.PositiveInfinity)]
    [Arguments(1.0)]
    [Arguments(42.0)]
    public void Valid_for_positive(double value) => new ZeroModel
    {
        NullableNumber = value,
        Number = value,
    }
    .ValidateWith(new IsPositiveValidator())
    .Should().BeValid().WithoutMessages();

    [Arguments(double.NaN)]
    [Arguments(0)]
    [Arguments(-1.0)]
    [Arguments(-42.0)]
    [Arguments(double.NegativeInfinity)]
    public void Invalid_for(double value)
    {
        using var _ = CultureInfo.InvariantCulture.Scoped();

        new ZeroModel
        {
            NullableNumber = value,
            Number = value,
        }
        .ValidateWith(new IsPositiveValidator())
        .Should().BeInvalid().WithMessages(
            ValidationMessage.Error("'Nullable Number' must be positive.", "NullableNumber"),
            ValidationMessage.Error("'Number' must be positive.", "Number"));
    }

    [Arguments("'Number' must be positive.", "en-GB")]
    [Arguments("'Number' moet positief zijn.", "nl-NL")]
    public void Invalid_with_message(string message, CultureInfo culture) {
        using var _ = culture.Scoped();

        new ZeroModel
        {
            NullableNumber = 42,
            Number = -42,
        }
        .ValidateWith(new IsPositiveValidator())
        .Should().BeInvalid().WithMessage(
            ValidationMessage.Error(message, "Number")); 
    }
}

public class IsNegative
{
    [Test]
    public void Valid_for_null() => new ZeroModel
    {
        NullableNumber = null,
        Number = -42,
    }
    .ValidateWith(new IsNegativeValidator())
    .Should().BeValid().WithoutMessages();

    [Test]
    [Arguments(-double.Epsilon)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(-1.0)]
    [Arguments(-42.0)]
    public void Valid_for_negative(double value) => new ZeroModel
    {
        NullableNumber = value,
        Number = value,
    }
    .ValidateWith(new IsNegativeValidator())
    .Should().BeValid().WithoutMessages();

    [Test]
    [Arguments(double.NaN)]
    [Arguments(0)]
    [Arguments(1.0)]
    [Arguments(42.0)]
    [Arguments(double.PositiveInfinity)]
    public void Invalid_for(double value)
    {
        using var _ = CultureInfo.InvariantCulture.Scoped();

        new ZeroModel
        {
            NullableNumber = value,
            Number = value,
        }
        .ValidateWith(new IsNegativeValidator())
        .Should().BeInvalid().WithMessages(
            ValidationMessage.Error("'Nullable Number' must be negative.", "NullableNumber"),
            ValidationMessage.Error("'Number' must be negative.", "Number"));
    }

    [Test]
    [Arguments("'Number' must be negative.", "en-GB")]
    [Arguments("'Number' moet negatief zijn.", "nl-NL")]
    public void Invalid_with_message(string message, CultureInfo culture)
    {
        using var _ = culture.Scoped();

        new ZeroModel
        {
            Number = 42,
            NullableNumber = -42,
        }
        .ValidateWith(new IsNegativeValidator())
        .Should().BeInvalid().WithMessage(
            ValidationMessage.Error(message, "Number"));
    }
}

public class IsNotPositive
{
    [Test]
    public void Valid_for_null() => new ZeroModel
    {
        NullableNumber = null,
        Number = -42,
    }
    .ValidateWith(new IsNotPositiveValidator())
    .Should().BeValid().WithoutMessages();

    [Test]
    [Arguments(0)]
    [Arguments(-double.Epsilon)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(-1.0)]
    [Arguments(-42.0)]
    public void Valid_for_not_positive(double value) => new ZeroModel
    {
        NullableNumber = value,
        Number = value,
    }
    .ValidateWith(new IsNotPositiveValidator())
    .Should().BeValid().WithoutMessages();

    [Test]
    [Arguments(double.NaN)]
    [Arguments(double.Epsilon)]
    [Arguments(+1.0)]
    [Arguments(+42.0)]
    [Arguments(double.PositiveInfinity)]
    public void Invalid_for(double value)
    {
        using var _ = CultureInfo.InvariantCulture.Scoped();

        new ZeroModel
        {
            NullableNumber = value,
            Number = value,
        }
        .ValidateWith(new IsNotPositiveValidator())
        .Should().BeInvalid().WithMessages(
            ValidationMessage.Error("'Nullable Number' must not be positive.", "NullableNumber"),
            ValidationMessage.Error("'Number' must not be positive.", "Number"));
    }

    [Test]
    [Arguments("'Number' must not be positive.", "en-GB")]
    [Arguments("'Number' mag niet positief zijn.", "nl-NL")]
    public void Invalid_with_message(string message, CultureInfo culture)
    {
        using var _ = culture.Scoped();

        new ZeroModel
        {
            NullableNumber = -42,
            Number = +42,
        }
        .ValidateWith(new IsNotPositiveValidator())
        .Should().BeInvalid().WithMessage(
            ValidationMessage.Error(message, "Number"));
    }
}

public class IsNotNegative
{
    [Test]
    public void Valid_for_null() => new ZeroModel
    {
        NullableNumber = null,
        Number = +42,
    }
    .ValidateWith(new IsNotNegativeValidator())
    .Should().BeValid().WithoutMessages();

    [Test]
    [Arguments(0)]
    [Arguments(double.Epsilon)]
    [Arguments(double.PositiveInfinity)]
    [Arguments(+1.0)]
    [Arguments(+42.0)]
    public void Valid_for_negative(double value) => new ZeroModel
    {
        NullableNumber = value,
        Number = value,
    }
    .ValidateWith(new IsNotNegativeValidator())
    .Should().BeValid().WithoutMessages();

    [Test]
    [Arguments(double.NaN)]
    [Arguments(-1.0)]
    [Arguments(-42.0)]
    [Arguments(double.NegativeInfinity)]
    public void Invalid_for(double value)
    {
        using var _ = CultureInfo.InvariantCulture.Scoped();

        new ZeroModel
        {
            NullableNumber = value,
            Number = value,
        }
        .ValidateWith(new IsNotNegativeValidator())
        .Should().BeInvalid().WithMessages(
            ValidationMessage.Error("'Nullable Number' must not be negative.", "NullableNumber"),
            ValidationMessage.Error("'Number' must not be negative.", "Number"));
    }

    [Test]
    [Arguments("'Number' must not be negative.", "en-GB")]
    [Arguments("'Number' mag niet negatief zijn.", "nl-NL")]
    public void Invalid_with_message(string message, CultureInfo culture)
    {
        using var _ = culture.Scoped();

        new ZeroModel
        {
            Number = -42,
            NullableNumber = +42,
        }
        .ValidateWith(new IsNotNegativeValidator())
        .Should().BeInvalid().WithMessage(
            ValidationMessage.Error(message, "Number"));
    }
}
