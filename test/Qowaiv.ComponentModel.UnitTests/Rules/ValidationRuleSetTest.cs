using NUnit.Framework;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Rules;
using Qowaiv.ComponentModel.Rules.Globalization;
using Qowaiv.ComponentModel.Tests.TestTools;
using Qowaiv.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.UnitTests.Rules
{
    public class ValidationRuleSetTest
    {
        [Test]
        public void Validate_NoPostalCodeForCountryWithouPostalCode_IsValid()
        {
            var model = new ComplexValidatableModel
            {
                PostalCode = PostalCode.Empty,
                Country = Country.AE,
            };

            DataAnnotationsAssert.IsValid(model);
        }

        [Test]
        public void Validate_NoPostalCode_WithErrors()
        {
            var model = new ComplexValidatableModel
            {
                PostalCode = PostalCode.Empty,
                Country = Country.NL,
            };

            DataAnnotationsAssert.WithErrors(model,
                new ValidationTestMessage(ValidationSeverity.Error, "The PostalCode field is required.", "PostalCode")
            );
        }

        [Test]
        public void Validate_NotValidForCountry_WithErrors()
        {
            var model = new ComplexValidatableModel
            {
                PostalCode = PostalCode.Parse("1234"),
                Country = Country.NL,
            };

            DataAnnotationsAssert.WithErrors(model,
                new ValidationTestMessage(ValidationSeverity.Error, "The postal code 1234 is not valid for Netherlands.", "PostalCode")
            );
        }
    }

    internal class ComplexValidatableModel : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ruleSet.Validate(validationContext);
        }

        private static readonly ValidationRuleSet<ComplexValidatableModel> ruleSet =
            new ValidationRuleSet<ComplexValidatableModel>
            (
                PostalCodeRule.For<ComplexValidatableModel>
                (
                    m => m.PostalCode, m=> m.Country
                ),
                ConditionalRequiredRule.For<ComplexValidatableModel>
                (
                    c => PostalCodeCountryInfo.GetInstance(c.Model.Country).HasPostalCode,
                    m => m.PostalCode
                )
            );

        public PostalCode PostalCode { get; set; }
        public Country Country { get; set; }
    }
}
