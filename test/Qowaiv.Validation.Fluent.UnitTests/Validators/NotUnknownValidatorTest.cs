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
            var model = new NotUnknownModel();

            FluentValidatorAssert.WithErrors<NotUnknownModelValidator, NotUnknownModel>(model,
                ValidationMessage.Error("'Email' must not be empty.", "Email")
            );
        }

        [Test]
        public void Validate_Unknown_WithError()
        {
            using (new CultureInfoScope("nl"))
            {
                var model = new NotUnknownModel { Email = EmailAddress.Unknown };

                FluentValidatorAssert.WithErrors<NotUnknownModelValidator, NotUnknownModel>(model,
                    ValidationMessage.Error("'Email' mag niet onbekend zijn.", "Email")
                );
            }
        }
    }
}
