using NUnit.Framework;
using Qowaiv.TestTools.Validiation;
using Qowaiv.Validation.Abstractions;
using Qowaiv.Validation.Fluent.UnitTests.Models;

namespace Qowaiv.Validation.Fluent.UnitTests.Validators
{
    public class NoIPBasedEmailAddressTest
    {
        [Test]
        public void NoIPBasedEmailAddress_ValidEmail_IsValid()
        {
            var model = new NoIPBasedEmailAddressModel { Email = EmailAddress.Parse("test@qowaiv.org") };
            FluentValidatorAssert.IsValid<NoIPBasedEmailAddressModelValidator, NoIPBasedEmailAddressModel>(model);
        }

        [Test]
        public void NoIPBasedEmailAddress_IPBased_WithError()
        {
            var model = new NoIPBasedEmailAddressModel { Email = EmailAddress.Parse("qowaiv@172.16.254.1") };
            FluentValidatorAssert.WithErrors<NoIPBasedEmailAddressModelValidator, NoIPBasedEmailAddressModel>(model,
                ValidationMessage.Error("'Email' has a IP address based domain.", "Email")
            );
        }
    }
}
