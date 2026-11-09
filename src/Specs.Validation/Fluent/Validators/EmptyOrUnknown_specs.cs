using Qowaiv.Validation.Fluent;
using Specs.Fluent.Models;

namespace Fluent_validation.EmptyOrUnknown_specs;

public class Set_value
{
    [Test]
    public void Is_valid()
        => new UnknownModel { Country = Country.NL }
        .ValidateWith(new UnknownModelValidator())
        .Should().BeValid();
}

public class Unknown
{
    [Test]
    [Arguments("'Country' must not be empty or unknown.", "en-GB")]
    [Arguments("'Country' mag niet leeg of onbekend zijn.", "nl-BE")]
    public void Not_valid(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            new UnknownModel { Country = Country.Unknown }
                .ValidateWith(new UnknownModelValidator())
                .Should().BeInvalid()
                .WithMessage(ValidationMessage.Error(message, "Country"));
        }
    }

    [Test]
    public void Is_valid_with_warning_severity()
        => new UnknownWithSeverityModel { Email = EmailAddress.Unknown }
            .ValidateWith(new UnknownWithSeverityModelValidator())
            .Should().BeValid()
            .WithMessage(ValidationMessage.Warn("'Email' must not be empty or unknown.", "Email"));
}

public class Empty
{
    [Test]
    [Arguments("'Country' must not be empty or unknown.", "en-GB")]
    [Arguments("'Country' mag niet leeg of onbekend zijn.", "nl-BE")]
    public void Is_not_valid(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            new UnknownModel { Country = Country.Empty }
            .ValidateWith(new UnknownModelValidator())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "Country"));
        }
    }

    [Test]
    public void Is_valid_with_warning_severity()
        => new UnknownWithSeverityModel { Email = EmailAddress.Empty }
        .ValidateWith(new UnknownWithSeverityModelValidator())
        .Should().BeValid()
        .WithMessage(ValidationMessage.Warn("'Email' must not be empty or unknown.", "Email"));
}
