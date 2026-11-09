namespace Qowaiv.Validation.DataAnnotations;

[DebuggerDisplay("Checks = {Checks}, Members = {Members.Length}")]
internal sealed class TypeAnnotations(AnnotationChecks checks, MemberAnnotations[] members) : Annotations(checks)
{
    /// <summary>A sealed enumerable type, which lacks members and does not implement <see cref="IValidatableObject"/>.</summary>
    /// <remarks>
    /// Typically used for <see cref="Array"/>.
    /// </remarks>
    public static readonly TypeAnnotations SealedCollection = new(AnnotationChecks.Enumerable, []);

    public readonly MemberAnnotations[] Members = members;

    /// <summary>
    /// Gets a value indicating whether a <see cref="ValidationContext"/> is needed
    /// for this type: when it implements <see cref="IValidatableObject"/>, or when
    /// any of its members has an attribute that requires the context.
    /// </summary>
    [Pure]
    public bool NeedsContext { get; }
        = (checks & AnnotationChecks.Validatable) != AnnotationChecks.None
        || members.Any(m => m.RequiresContext);

    /// <summary>Gets the <see cref="TypeAnnotations"/> of the <see cref="Type"/>.</summary>
    /// <param name="type">
    /// The type to get the annotations from.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// When the type is null.
    /// </exception>
    /// <returns>
    /// The annotations of the type or null if the type lacks annotations.
    /// </returns>
    [Pure]
    internal static TypeAnnotations? Get(Type type)
        => Store.Get(Guard.NotNull(type));

    private static readonly AnnotationStore Store = new();
}
