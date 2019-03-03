using NUnit.Framework;
using Qowaiv.ComponentModel.Messages;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.TestTools.ComponentModel
{
    public static class ValidationAssert
    {

        public static void WithError(string error, ValidationAttribute attribute, object value, ValidationContext context)
        {
            Assert.IsNotNull(attribute, nameof(attribute));
            Assert.IsNotNull(context, nameof(context));

            var result = attribute.GetValidationResult(value, context);

            Assert.AreEqual(
                string.Format("{0}: {1}", ValidationSeverity.Error, error),
                string.Format("{0}: {1}", result.GetSeverity(), result.ErrorMessage));
        }

        public static void IsSuccessful(ValidationAttribute attribute, object value, ValidationContext context)
        {
            Assert.IsNotNull(attribute, nameof(attribute));
            Assert.IsNotNull(context, nameof(context));

            var result = attribute.GetValidationResult(value, context);

            Assert.AreEqual(ValidationMessage.None, result);
        }
    }
}
