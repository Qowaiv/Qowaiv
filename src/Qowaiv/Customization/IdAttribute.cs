namespace Qowaiv.Customization;

/// <summary>
/// Used by Qowaiv.CodeGeneration.SingleValueObjects to generateod the
/// blumbping code for the custom identifier.
/// </summary>
/// <typeparam name="TBehavior">
/// Singleton that handles the behavior of the custom identifier.
/// </typeparam>
/// <typeparam name="TValue">
/// Type of the underlying value:
/// - <see cref="int" />
/// - <see cref="long" />
/// - <see cref="Guid" />
/// - <see cref="Uuid" />
/// - <see cref="string" />
/// or other.
/// </typeparam>
[Conditional("CODE_GENERATOR_ATTRIBUTES")]
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
[ExcludeFromCodeCoverage]
public sealed class IdAttribute<TBehavior, TValue> : Attribute
    where TBehavior : IdBehavior<TValue>, new()
    where TValue : IEquatable<TValue>
{
    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"[Id<{typeof(TBehavior)}, {typeof(TValue)}>]";
}
