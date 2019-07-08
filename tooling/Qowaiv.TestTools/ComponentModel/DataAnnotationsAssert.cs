using Qowaiv.ComponentModel.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Qowaiv.TestTools.ComponentModel
{
    /// <summary>Assertion helper class on data annotations.</summary>
    public static class DataAnnotationsAssert
    {
        /// <summary>Asserts that the model is valid, throws if not.</summary>
        [DebuggerStepThrough]
        public static void IsValid<T>(T model) => IsValid<T>(model, null);

        /// <summary>Asserts that the model is valid, throws if not.</summary>
        [DebuggerStepThrough]
        public static void IsValid<T>(T model, AnnotatedModelValidator validator) => WithErrors(model, new ValidationResult[0]);

        /// <summary>Asserts the model to be invalid with specific messages. Throws if not.</summary>
        [DebuggerStepThrough]
        public static void WithErrors<T>(T model, params ValidationResult[] expected) => WithErrors(model, null, expected);

        /// <summary>Asserts the model to be invalid with specific messages. Throws if not.</summary>
        [DebuggerStepThrough]
        public static void WithErrors<T>(T model, AnnotatedModelValidator validator, params ValidationResult[] expected)
        {
            var result = (validator ?? new AnnotatedModelValidator()).Validate(model);
            ValidationResultAssert.WithErrors(result, expected);
        }
    }
}
