namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Non-generic access to <see cref="DefinedEnums{TEnum}"/>, for when the enum type is only known at runtime.</summary>
internal abstract class DefinedEnums
{
    [Pure]
    public static DefinedEnums New(Type enumType, bool onlyAllowDefinedFlags)
        => (DefinedEnums)typeof(DefinedEnums<>)
        .MakeGenericType(enumType)
        .GetMethod(nameof(DefinedEnums<>.New))!
        .Invoke(null, [onlyAllowDefinedFlags])!;

    /// <summary>Returns true if the value is defined.</summary>
    /// <remarks>The value is expected to be of the enum type.</remarks>
    [Pure]
    public abstract bool Contains(object value);
}
