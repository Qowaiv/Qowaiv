using Qowaiv.Validation.Abstractions;
using System.Diagnostics.Contracts;

namespace Specs.Abstractions;

internal static class Arrange
{
    /// <summary>Helper to create arrays.</summary>
    [Pure]
    public static T[] array<T>(params T[] args) => args;

    public static readonly ValidationMessage Error1 = ValidationMessage.Error("Error 1");
    public static readonly ValidationMessage Error2 = ValidationMessage.Error("Error 2");
    public static readonly ValidationMessage Warning1 = ValidationMessage.Warn("Warning 1");
    public static readonly ValidationMessage Warning2 = ValidationMessage.Warn("Warning 2");
    public static readonly ValidationMessage Info1 = ValidationMessage.Info("Info 1");
    public static readonly ValidationMessage Info2 = ValidationMessage.Info("Info 2");

    public static IValidationMessage InvalidSeverity(ValidationSeverity severity) => new MockMessage(severity);

    public static readonly ValidationMessage[] TestMessages =
    [
        Error1, Error2, Warning1, Warning2, Info1, Info2
    ];

    private sealed class MockMessage(ValidationSeverity severity) : IValidationMessage
    {
        public ValidationSeverity Severity => severity;
        public string? PropertyName => throw new NotSupportedException();
        public string? Message => throw new NotSupportedException();
    }
}
