namespace Qowaiv.Customization;

/// <summary>
/// Used by Qowaiv.CodeGeneration.SingleValueObjects to generateod the
/// blumbping code for the custom SVO.
/// </summary>
/// <typeparam name="TBehavior">
/// Singleton that handles the behavior of the custom SVO.
/// </typeparam>
[Conditional("CONTRACTS_FULL")]
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
[ExcludeFromCodeCoverage]
public sealed class SvoAttribute<TBehavior> : Attribute
    where TBehavior : SvoBehavior, new()
{
    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"[Svo<{typeof(TBehavior)}>]";
}
