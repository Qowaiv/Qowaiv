using Qowaiv.Validation.Fluent;
using Specs.Fluent.Models;

namespace Fluent_validation.Unknown_specs;

public class Set_value
{
    [Test]
    public void Is_valid()
        => new UnknownModel { Email = EmailAddress.Parse("info@qowaiv.corg") }
        .ValidateWith(new UnknownModelValidator())
        .Should().BeValid();
}

public class Unknown
{
    [Test]
    [Arguments("'Email' must not be unknown.", "en-GB")]
    [Arguments("'Email' mag niet onbekend zijn.", "nl-BE")]
    public void Not_valid(string message, CultureInfo culture)
    {
        using (new CultureInfoScope(culture))
        {
            new UnknownModel { Email = EmailAddress.Unknown }
                .ValidateWith(new UnknownModelValidator())
                .Should().BeInvalid()
                .WithMessage(ValidationMessage.Error(message, "Email"));
        }
    }
}

public class Empty
{
    [Test]
    public void Is_valid()
        => new UnknownModel { Email = EmailAddress.Empty }
        .ValidateWith(new UnknownModelValidator())
        .Should().BeValid();
}
