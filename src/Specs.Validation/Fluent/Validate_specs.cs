using Qowaiv.Validation.Fluent;
using Specs.Fluent.Models;

namespace Fluent_validation.Validate_specs;

public class Validation_without_warnings
{
    [Test]
    public void Is_valid()
        => new WarningModel()
        .ValidateWith(new WarningModelValidator())
        .Should().BeValid()
        .WithMessages(
            ValidationMessage.Warn("Test warning.", "Message"),
            ValidationMessage.Info("Nice that you validated this.", "Message"));
}
