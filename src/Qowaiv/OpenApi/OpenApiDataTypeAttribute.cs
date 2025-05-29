namespace Qowaiv.OpenApi;

/// <summary>Describes how a type should be described as OpenAPI Data Type.</summary>
/// <remarks>
/// See: https://swagger.io/docs/specification/data-models/data-types/.
/// </remarks>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class OpenApiDataTypeAttribute(
    string description,
    string type,
    object example,
    string? format = null,
    bool nullable = false,
    [StringSyntax(StringSyntaxAttribute.Regex)] string? pattern = null,
    string? @enum = null) : Attribute
{
    /// <summary>Gets the description of the OpenAPI Data Type.</summary>
    public string Description { get; } = Guard.NotNullOrEmpty(description);

    /// <summary>Gets the type of the OpenAPI Data Type.</summary>
    public string Type { get; } = Guard.NotNullOrEmpty(type);

    /// <summary>Gets the example of the OpenAPI Data Type.</summary>
    public object Example { get; } = Guard.NotNull(example);

    /// <summary>Gets the format of the OpenAPI Data Type.</summary>
    public string? Format { get; } = format;

    /// <summary>Gets if the OpenAPI Data Type is nullable or not.</summary>
    public bool Nullable { get; } = nullable;

    /// <summary>Gets the Pattern of the OpenAPI Data Type.</summary>
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public string? Pattern { get; } = pattern;

    /// <summary>Gets the Pattern of the OpenAPI Data Type.</summary>
    public string[]? Enum { get; } = @enum?.Split(',');
}
