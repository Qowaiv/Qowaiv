namespace Qowaiv.Validation.Microsoft.Extensions;

internal static class QowaivValidateContext
{
    extension(ValidateContext context)
    {
        [Pure]
        public ValidationMessages ValidationMessages()
            => context.ValidationErrors?.Values is { } values
            ? Abstractions.ValidationMessages.New(values
                .SelectMany(errors => errors)
                .Select(error => ValidationMessage.Error(error.ErrorMessage, error.Path)))
            : Abstractions.ValidationMessages.None;
    }
}
