using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools.Validiation;
using Qowaiv.Validation.Abstractions;
using Qowaiv.Validation.Fluent.UnitTests.Models;

namespace Qowaiv.Validation.Fluent.UnitTests.Validators
{
    public class NotUnknownValidatorTest
    {
        [Test]
        public void Validate_Empty_WithError()
        {
            var model = new SimpleModel();

            FluentValidatorAssert.WithErrors<SimpleModelValidator, SimpleModel>(model,
                new ValidationMessage() { Severity = ValidationSeverity.Error, Message = "'Email' must not be empty.", MemberName = "Email" }
            );
        }

        [Test]
        public void Validate_Unknown_WithError()
        {
            using (new CultureInfoScope("nl"))
            {
                var model = new SimpleModel { Email = EmailAddress.Unknown };

                FluentValidatorAssert.WithErrors<SimpleModelValidator, SimpleModel>(model,
                    new ValidationMessage() { Severity = ValidationSeverity.Error, Message = "'Email' mag niet onbekend zijn.", MemberName = "Email" }
                );
            }
        }
    }
}
