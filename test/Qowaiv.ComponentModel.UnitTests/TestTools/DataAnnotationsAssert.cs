using NUnit.Framework;
using Qowaiv.ComponentModel.Validation;

namespace Qowaiv.ComponentModel.Tests.TestTools
{
    public static class DataAnnotationsAssert
    {
        public static void IsValid<T>(T model) => IsValid<T>(model, null);

        public static void IsValid<T>(T model, AnnotatedModelValidator validator) => WithErrors<T>(model, new ValidationTestMessage[0]);

        public static void WithErrors<T>(T model, params ValidationTestMessage[] expected) => WithErrors(model, null, expected);

        public static void WithErrors<T>(T model, AnnotatedModelValidator validator, params ValidationTestMessage[] expected)
        {
            var result = (validator ?? new AnnotatedModelValidator()).Validate(model);
            var actual = result.Errors.ForAssertion();
            Assert.AreEqual(expected, actual);
        }
    }
}
