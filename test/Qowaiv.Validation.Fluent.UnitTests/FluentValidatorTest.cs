﻿using NUnit.Framework;
using Qowaiv.Globalization;
using Qowaiv.TestTools.Validiation;
using Qowaiv.Validation.Abstractions;
using Qowaiv.Validation.Fluent.UnitTests.Models;

namespace Qowaiv.Validation.Fluent.UnitTests
{
    public class FluentValidatorTest
    {
        [Test]
        public void Validate_Empty_WithError()
        {
            var validator = new FluentValidator<SimpleModel>(new SimpleModelValidator());

            var messages = validator.Validate(new SimpleModel { });

            ValidationMessageAssert.WithErrors(messages, new ValidationMessage() { Severity = ValidationSeverity.Error, Message = "'Email' must not be empty.", MemberName = "Email" });
        }

        [Test]
        public void Validate_Unknown_WithError()
        {
            using (new CultureInfoScope("nl"))
            {
                var validator = new FluentValidator<SimpleModel>(new SimpleModelValidator());
                var messages = validator.Validate(new SimpleModel { Email = EmailAddress.Unknown });

                ValidationMessageAssert.WithErrors(messages, new ValidationMessage() { Severity = ValidationSeverity.Error, Message = "'Email' mag niet onbekend zijn.", MemberName = "Email" });
            }
        }
    }
}
