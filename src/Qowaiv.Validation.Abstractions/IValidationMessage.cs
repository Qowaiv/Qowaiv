namespace Qowaiv.Validation.Abstractions
{
    /// <summary>Represents a validation messsage.</summary>
    public interface IValidationMessage
    {
        /// <summary>Gets the severity of the message (None, Info, Warning, Error).</summary>
        ValidationSeverity Severity { get; }

        /// <summary>The name of the property.</summary>
        string PropertyName { get; }

        /// <summary>Gets the (<see cref="string"/>) message.</summary>
        string Message { get; }
    }
}
