using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools.Validiation;
using Qowaiv.Validation.Abstractions;
using Qowaiv.Validation.Fluent.UnitTests.Models;

namespace Qowaiv.Validation.Fluent.UnitTests.Validators
{
    public class PostalCodeValidatorTest
    {
        [Test]
        public void Validate_Defaults_IsValid()
        {
            var model = new PostalCodeModel();
            FluentValidatorAssert.IsValid<PostalCodeModelValidator, PostalCodeModel>(model);
        }

        [Test]
        public void Validate_EmptyPostalCodeWithSetCountry_IsValid()
        {
            var model = new PostalCodeModel { Country = Country.NL, PostalCode = PostalCode.Empty };
            FluentValidatorAssert.IsValid<PostalCodeModelValidator, PostalCodeModel>(model);
        }

        [Test]
        public void Validate_DutchPostalCode_IsValid()
        {
            var model = new PostalCodeModel { Country = Country.NL, PostalCode = PostalCode.Parse("2624DP") };
            FluentValidatorAssert.IsValid<PostalCodeModelValidator, PostalCodeModel>(model);
        }

        [Test]
        public void Validate_DutchPostalCode_Invalid()
        {
            using (new CultureInfoScope("nl"))
            {
                var model = new PostalCodeModel { Country = Country.NL, PostalCode = PostalCode.Parse("12345") };
                FluentValidatorAssert.WithErrors<PostalCodeModelValidator, PostalCodeModel>(model,
                    new ValidationMessage() { Severity = ValidationSeverity.Error, Message = "'Postal Code' 12345 is niet geldig voor Nederland.", MemberName = "PostalCode" });
            }
        }
    }
}
