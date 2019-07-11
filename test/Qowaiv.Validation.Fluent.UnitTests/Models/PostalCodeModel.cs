using FluentValidation;
using Qowaiv.Globalization;

namespace Qowaiv.Validation.Fluent.UnitTests.Models
{
    public class PostalCodeModel
    {
        public PostalCode PostalCode { get; set; }
        public Country Country { get; set; }
    }

    public class PostalCodeModelValidator : AbstractValidator<PostalCodeModel>
    {
        public PostalCodeModelValidator()
        {
            RuleFor(m => m.PostalCode).ValidFor(m => m.Country);
        }
    }
}
