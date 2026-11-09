namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Helper for centralizing <see cref="AttributeTargets" /> combinations.</summary>
internal static class AttributeTarget
{
    /// <summary>
    /// <see cref="AttributeTargets.Property" /> | <see cref="AttributeTargets.Field" /> | <see cref="AttributeTargets.Parameter" />.</summary>
    public const AttributeTargets Member = AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter;
}
