namespace Qowaiv.Validation.DataAnnotations;

/// <summary>Validates if the decorated enum has a value that is a defined.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
[Validates(typeof(Enum))]
public sealed class DefinedEnumValuesOnlyAttribute()
    : ValidationAttribute(() => QowaivValidationMessages.AllowedValuesAttribute_ValidationError)
{
    /// <summary>If true, only explicitly defined values of flag enums are allowed; combinations of defined values that are not defined themselves are rejected.</summary>
    /// <remarks>
    /// When enabled, the logic falls back on <see cref="Enum.IsDefined(Type, object)"/>, as
    /// that is stricter than, our implementation.
    /// </remarks>
    public bool OnlyAllowDefinedFlagsCombinations { get; set; }

    /// <summary>Returns true if the value is defined for the enum, otherwise false.</summary>
    /// <exception cref="ArgumentException">
    /// If the type of the value is not an enum.
    /// </exception>
    [Pure]
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        _ when value.GetType() is { IsEnum: true } enumType => Contains(value, enumType),
        _ => throw UnsupportedType.ForAttribute<DefinedEnumValuesOnlyAttribute>(value.GetType()),
    };

    [Pure]
    private bool Contains(object value, Type enumType)
    {
        var strict = OnlyAllowDefinedFlagsCombinations;
        var lookup = strict ? Strict : Loose;
        return lookup.GetOrAdd(enumType, static (type, flag) => DefinedEnums.New(type, flag), strict).Contains(value);
    }

    /// <summary>Contains enums with flags where only defined flags are allowed.</summary>
    private static readonly ConcurrentDictionary<Type, DefinedEnums> Strict = [];

    /// <summary>Contains all other enums.</summary>
    private static readonly ConcurrentDictionary<Type, DefinedEnums> Loose = [];
}
