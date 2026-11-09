using Qowaiv.Validation.Abstractions;
using System.Net.NetworkInformation;

namespace Abstractions.Validator_specs;

public class Empty
{
    [Test]
    public void Gets_an_validator_that_does_not_validate()
        => Validator.Empty<string>().Validate("")
        .Should().BeValid().WithMessages();
}

public class Composite
{
    [Test]
    public void Combines_validators()
        => Validator.Composite(new Not42Validator(), new NotEvenValidator())
        .Validate(17).Should().BeInvalid()
        .WithMessages(
            ValidationMessage.Error("Not 42"),
            ValidationMessage.Error("Not even"));


    private sealed class Not42Validator : IValidator<int>
    {
        public Result<int> Validate(int model) => Result.WithMessages<int>(ValidationMessage.Error("Not 42"));
    }

    private sealed class NotEvenValidator : IValidator<int>
    {
        public Result<int> Validate(int model) => Result.WithMessages<int>(ValidationMessage.Error("Not even"));
    }
}

