using Qowaiv.Validation.Abstractions;
using Qowaiv.Validation.Fluent;
using System;
using System.Diagnostics;

namespace Qowaiv.TestTools.Validiation
{
    /// <summary>Assertion helper class on data annotations.</summary>
    public static class FluentValidatorAssert
    {
        /// <summary>Asserts that the model is valid, throws if not.</summary>
        [DebuggerStepThrough]
        public static void IsValid<TValidator, TModel>(TModel model) where TValidator : FluentValidation.IValidator<TModel> => IsValid(model, Activator.CreateInstance<TValidator>());

        /// <summary>Asserts that the model is valid, throws if not.</summary>
        [DebuggerStepThrough]
        public static void IsValid<TModel>(TModel model, FluentValidation.IValidator<TModel> validator) => WithErrors(model, validator, new IValidationMessage[0]);

        /// <summary>Asserts the model to be invalid with specific messages. Throws if not.</summary>
        [DebuggerStepThrough]
        public static void WithErrors<TValidator, TModel>(TModel model, params IValidationMessage[] expected) where TValidator : FluentValidation.IValidator<TModel> => WithErrors(model, Activator.CreateInstance<TValidator>(), expected);

        /// <summary>Asserts the model to be invalid with specific messages. Throws if not.</summary>
        [DebuggerStepThrough]
        public static void WithErrors<TModel>(TModel model, FluentValidation.IValidator<TModel> validator, params IValidationMessage[] expected)
        {
            if (validator is null) { throw new ArgumentNullException(nameof(validator)); }

            var wrapper = new FluentValidator<TModel>(validator);
            var result = wrapper.Validate(model);
            ValidationMessageAssert.WithErrors(result, expected);
        }
    }
}
