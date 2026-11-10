namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validates if the decorated enum has a value that is defined.</summary>
/// <typeparam name="TEnum">
/// The type of the enum.
/// </typeparam>
[AttributeUsage(AttributeTarget.Member, AllowMultiple = false)]
[Validates(GenericArgument = true)]
public sealed class DefinedOnlyAttribute<TEnum>()
    : ValidationAttribute(() => QowaivValidationMessages.AllowedValuesAttribute_ValidationError)
    where TEnum : struct, Enum
{
    /// <summary>If false, combinations of defined single values, that are not defined explicitly themselves, are allowed for flag enums.</summary>
    /// <remarks>
    /// When enabled, the logic falls back on <see cref="Enum.IsDefined(Type, object)"/>,
    /// because that is stricter than our implementation.
    /// </remarks>
    public bool OnlyAllowDefinedFlagsCombinations { get; init; }

    /// <summary>Returns true if the value is null or is defined for the enum, otherwise false.</summary>
    /// <exception cref="InvalidCastException">
    /// If the type of the value is not an enum.
    /// </exception>
    [Pure]
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        TEnum val => (OnlyAllowDefinedFlagsCombinations ? Strict : Loose).Value.Contains(val),
        _ => throw UnsupportedType.ForAttribute<DefinedOnlyAttribute<TEnum>>(value.GetType()),
    };

    private static readonly Lazy<DefinedEnums<TEnum>> Strict = new(() => DefinedEnums<TEnum>.New(onlyAllowDefinedFlags: true));

    private static readonly Lazy<DefinedEnums<TEnum>> Loose = new(() => DefinedEnums<TEnum>.New(onlyAllowDefinedFlags: false));
}
