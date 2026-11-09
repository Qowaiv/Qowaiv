using Qowaiv.Validation.Fluent;
using Specs.Fluent.Models;

namespace Fluent_validation.Required_specs;

public class Known
{
    [Test]
    public void Is_valid()
        => new RequiredModel { Email = EmailAddress.Parse("test@qowaiv.org"), Country = Country.NL }
        .ValidateWith(new RequiredModelValidator())
        .Should().BeValid();
}
public class Uknown
{
    [Test]
    [Arguments("'Email' is required.", "en-GB")]
    [Arguments("'Email' is verplicht.", "nl-BE")]
    public void Not_valid_by_default(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            new RequiredModel { Email = EmailAddress.Unknown, Country = Country.NL }
                .ValidateWith(new RequiredModelValidator())
                .Should().BeInvalid()
                .WithMessage(ValidationMessage.Error(message, "Email"));
        }
    }

    [Test]
    public void Is_valid_when_explictly_allowed()
        => new RequiredModel { Email = EmailAddress.Parse("test@qowaiv.org"), Country = Country.Unknown }
        .ValidateWith(new RequiredModelValidator())
        .Should().BeValid();
}
public class Empty
{
    [Test]
    [Arguments("'Email' is required.", "en-GB")]
    [Arguments("'Email' is verplicht.", "nl-BE")]
    public void Not_valid(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            new RequiredModel { Email = EmailAddress.Empty, Country = Country.NL }
            .ValidateWith(new RequiredModelValidator())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "Email"));
        }
    }
}
