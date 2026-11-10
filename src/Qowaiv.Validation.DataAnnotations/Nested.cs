namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Represents a wrapper for a (sealed) <see cref="ValidationContext"/>.</summary>
internal readonly ref struct Nested(object instance, TypeAnnotations annotations, MemberPath path)
{
    public readonly object Instance = instance;
    public readonly TypeAnnotations Annotations = annotations;
    public readonly MemberPath Path = path;

    public void Deconstruct(out object instance, out TypeAnnotations annotations, out MemberPath path)
    {
        instance = Instance;
        annotations = Annotations;
        path = Path;
    }
}
