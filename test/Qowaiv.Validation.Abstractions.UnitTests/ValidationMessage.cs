using System;

namespace Qowaiv.Validation.Abstractions.UnitTests
{
    [Serializable]
    internal class ValidationMessage : IValidationMessage, IEquatable<ValidationMessage>
    {
        public ValidationSeverity Severity { get; set; }

        public string PropertyName  {get;set;}

        public string Message { get; set; }

        public override int GetHashCode() => ToString().GetHashCode();
        public override bool Equals(object obj) => obj is ValidationMessage other && Equals(other);
        public bool Equals(ValidationMessage other) => ToString() == other?.ToString();

        public override string ToString() => $"{Severity} Message: {Message}, Member: {PropertyName}";

        public static ValidationMessage None => new ValidationMessage();
        public static ValidationMessage Info(string message, string member = null) => new ValidationMessage { Severity = ValidationSeverity.Info, Message = message, PropertyName = member };
        public static ValidationMessage Warn(string message, string member = null) => new ValidationMessage { Severity = ValidationSeverity.Warning, Message = message, PropertyName = member };
        public static ValidationMessage Error(string message, string member = null) => new ValidationMessage { Severity = ValidationSeverity.Error, Message = message, PropertyName = member };

    }
}
