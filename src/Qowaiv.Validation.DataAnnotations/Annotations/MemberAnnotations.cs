namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Represents annotations of a member.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
internal sealed class MemberAnnotations : Annotations
{
    /// <summary>Initializes a new instance of the <see cref="MemberAnnotations"/> class.</summary>
    internal MemberAnnotations(
        AnnotationChecks checks,
        TypeAnnotations? type,
        string name,
        DisplayAttribute? display,
        ValidationAttribute[] attributes,
        Func<object, object?> getValue) : base(checks)
    {
        Type = type;
        Name = name;
        Display = display;
        Attributes = attributes;
        GetValue = getValue;

        foreach (var attr in attributes)
        {
            if (attr.RequiresValidationContext)
            {
                RequiresContext = true;
                break;
            }
        }
    }

    /// <summary>Gets the annotations of the declared member type (if any).</summary>
    public readonly TypeAnnotations? Type;

    /// <inheritdoc cref="AnnotationChecks.IsSafe" />
    public bool IsSafe => Checks.HasFlag(AnnotationChecks.IsSafe);

    /// <inheritdoc cref="AnnotationChecks.FixedType" />
    public bool FixedType => Checks.HasFlag(AnnotationChecks.FixedType);

    /// <summary>Gets the name of the member.</summary>
    public readonly string Name;

    /// <summary>Gets the (optional) display attribute.</summary>
    public readonly DisplayAttribute? Display;

    /// <summary>Gets the <see cref="ValidationAttribute"/>s defined on the member.</summary>
    public ValidationAttribute[] Attributes { get; }

    /// <summary>Gets a value indicating whether any attribute needs a <see cref="ValidationContext"/>.</summary>
    public bool RequiresContext { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Func<object, object?> GetValue;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"{Name}, Checks = {Checks}, Attributes = {string.Join(", ", Attributes.Select(Shorten))}";

    [Pure]
    private static string Shorten(Attribute attr) => attr.GetType().Name.Replace("Attribute", string.Empty);
}
