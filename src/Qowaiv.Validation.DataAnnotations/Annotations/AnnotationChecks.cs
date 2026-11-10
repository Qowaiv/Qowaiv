namespace Qowaiv.Validation.DataAnnotations;

[Flags]
internal enum AnnotationChecks
{
    None = 0,

    /// <summary>Type contains members.</summary>
    Members /*.....*/ = 0b000001,

    /// <summary>Type is enumerable (and its items have annotations).</summary>
    Enumerable /*..*/ = 0b000010,

    /// <summary>Type implements <see cref="IValidatableObject"/>.</summary>
    Validatable /*.*/ = 0b000100,

    /// <summary>Type is not sealed.</summary>
    Inheritance /*.*/ = 0b001000,

    /// <summary>Indicates that a member value can safely be accessed without the change of crashing.</summary>
    IsSafe /*......*/ = 0b010000,

    /// <summary>Type is sealed concreated class or struct.</summary>
    FixedType /*...*/ = 0b100000,

    /// <summary>When a type is not sealed, it can result in anything.</summary>
    NotSealed = Inheritance | Members | Enumerable | Validatable,

    /// <summary>Recursion should be applied if any of the flags is checked.</summary>
    Recursive = Members | Enumerable | Validatable,
}

internal static class AnnotationCheck
{
    [Pure]
    public static AnnotationChecks New(Type type) => type switch
    {
        _ when !type.IsSealed => AnnotationChecks.NotSealed,
        _ when type.IsArray => New(type.GetElementType()!).And(AnnotationChecks.Enumerable),
        _ when Nullable.GetUnderlyingType(type) is { } underlying => New(underlying),
        _ when type.GetEnumerableType() is { } enumerable => New(enumerable).And(AnnotationChecks.Enumerable),
        _ when type.ImplementsIValidatableObject() => AnnotationChecks.Validatable,
        _ => AnnotationChecks.None,
    };

    extension(AnnotationChecks checks)
    {
        [Pure]
        private AnnotationChecks And(AnnotationChecks and)
            => checks is AnnotationChecks.None
            ? checks
            : checks | and;
    }
}
