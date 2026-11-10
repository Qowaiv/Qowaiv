namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Decorates a property or field as optional.</summary>
/// <remarks>
/// Null object pattern implementation for a <see cref="RequiredAttribute"/>.
/// See: https://en.wikipedia.org/wiki/Null_object_pattern.
/// </remarks>
[AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
[Validates(typeof(object))]
public sealed class OptionalAttribute : RequiredAttribute
{
    /// <summary>Returns true as an <see cref="OptionalAttribute"/> is always valid.</summary>
    [Pure]
    public override bool IsValid(object? value) => true;
}
