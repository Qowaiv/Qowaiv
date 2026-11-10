using Qowaiv.Validation.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Specs.TestTools;

internal static class AnnotationsValidations
{
    extension<T>(T obj) where T : notnull
    {
        /// <summary>Helps to test types that have custom should you want to skip.</summary>
        [Pure]
        public object AsObject() => obj;
    }

    extension<TModel>(TModel model) where TModel : class
    {
        [Pure]
        public Qowaiv.Validation.Abstractions.Result<TModel> ValidateAnnotations()
            => AnnotatedModelValidator.Validate(model);
    }
}
