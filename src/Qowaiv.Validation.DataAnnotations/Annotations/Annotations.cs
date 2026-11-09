namespace Qowaiv.Validation.DataAnnotations;

internal abstract class Annotations(AnnotationChecks checks)
{
    public AnnotationChecks Checks { get; } = checks;

    public bool CheckEnumerable => Checks.HasFlag(AnnotationChecks.Enumerable);

    public bool CheckInheritance => Checks.HasFlag(AnnotationChecks.Inheritance);

    public bool CheckValidatable => Checks.HasFlag(AnnotationChecks.Validatable);

    public bool CheckRecursive => (Checks & AnnotationChecks.Recursive) != AnnotationChecks.None;
}
