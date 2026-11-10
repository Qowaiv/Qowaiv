using FluentValidation;
using Qowaiv.Validation.Fluent;

namespace Fluent_validation.Postal_code_valid_for_specs;

public class Valid_for
{
    [Test]
    public void postal_code_for_country()
        => new PostalCodeModel
        {
            PostalCode = PostalCode.Parse("2628ZD"),
            Country = Country.NL,
        }
        .ValidateWith(new PostalCodeModelValidator())
        .Should().BeValid();
}

public class Not_invalid_for
{
    [Test]
    public void Empty_postal_code_for_empty_country()
        => new PostalCodeModel
        {
            PostalCode = PostalCode.Empty,
            Country = Country.Empty,
        }
        .ValidateWith(new PostalCodeModelValidator())
        .Should().BeValid();

    [Test]
    public void Empty_postal_code_for_country()
         => new PostalCodeModel
         {
             PostalCode = PostalCode.Empty,
             Country = Country.NL,
         }
         .ValidateWith(new PostalCodeModelValidator())
         .Should().BeValid();

    [Test]
    public void Postal_code_for_empty_country()
         => new PostalCodeModel
         {
             PostalCode = PostalCode.Parse("12345"),
             Country = Country.Empty,
         }
         .ValidateWith(new PostalCodeModelValidator())
        .Should().BeValid();
}
public class Invalid_for
{
    [Test]
    [Arguments("'Postal Code' 12345 is not valid for Netherlands.", "en-GB")]
    [Arguments("'Postal Code' 12345 is niet geldig voor Nederland.", "nl-BE")]
    public void postal_code_not_comform_country(string message, CultureInfo culture)
    {
        using (new CultureInfoScope(culture))
        {
            new PostalCodeModel
            {
                Country = Country.NL,
                PostalCode = PostalCode.Parse("12345")
            }
            .ValidateWith(new PostalCodeModelValidator())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "PostalCode"));
        }
    }
}

internal class PostalCodeModel
{
    public PostalCode PostalCode { get; set; }
    public Country Country { get; set; }
}

internal class PostalCodeModelValidator : ModelValidator<PostalCodeModel>
{
    public PostalCodeModelValidator()
        => RuleFor(m => m.PostalCode).ValidFor(m => m.Country);
}
