namespace Qowaiv.Validation.DataAnnotations;

/// <summary>
/// Describes the type the <see cref="ValidationAttribute"/>
/// is capable of validating.
/// </summary>
[CLSCompliant(false)]
[Conditional("CONTRACTS_FULL")]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class ValidatesAttribute : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="ValidatesAttribute"/> class.</summary>
    public ValidatesAttribute() : this(typeof(object)) { }

    /// <summary>Initializes a new instance of the <see cref="ValidatesAttribute"/> class.</summary>
    public ValidatesAttribute(string typeName) => Type = Type.GetType(typeName) ?? typeof(object);

    /// <summary>Initializes a new instance of the <see cref="ValidatesAttribute"/> class.</summary>
    public ValidatesAttribute(Type type) => Type = type;

    /// <summary>Type that can be validated.</summary>
    public Type Type { get; }

    /// <summary>
    /// If true, the type should be equal to the generic to the generic
    /// argument of the <see cref="ValidationAttribute"/>.
    /// </summary>
    public bool GenericArgument { get; init; }
}
