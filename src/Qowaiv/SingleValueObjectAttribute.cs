namespace Qowaiv;

/// <summary>Describes a single value object as intended at the Domain Driven Design context.</summary>
/// <param name="staticOptions">
/// The available static options of the single value object.
/// </param>
/// <param name="underlyingType">
/// The underlying type of the single value object.
/// </param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
public sealed class SingleValueObjectAttribute(SingleValueStaticOptions staticOptions, Type underlyingType) : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="SingleValueObjectAttribute" /> class.</summary>
    /// <remarks>
    /// The parameterless constructor is marked private so it can not be used or tested.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    private SingleValueObjectAttribute() : this(default, typeof(object)) { }

    /// <summary>The available static options of the single value object.</summary>
    public SingleValueStaticOptions StaticOptions { get; } = staticOptions;

    /// <summary>The underlying type of the single value object.</summary>
    public Type UnderlyingType { get; } = underlyingType;

    /// <summary>Gets and set the database type.</summary>
    /// <remarks>
    /// Use this if the database type is different from the underlying type.
    /// </remarks>
    public Type? DatabaseType { get; init; }
}
