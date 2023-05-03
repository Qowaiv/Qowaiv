﻿namespace Qowaiv.OpenApi;

/// <summary>Describes how a type should be described as OpenAPI Data Type.</summary>
/// <remarks>
/// See: https://swagger.io/docs/specification/data-models/data-types/.
/// </remarks>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class OpenApiDataTypeAttribute : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="OpenApiDataTypeAttribute"/> class.</summary>
    public OpenApiDataTypeAttribute(
        string description,
        string type,
        object example,
        string? format = null,
        bool nullable = false,
#if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.Regex)] string? pattern = null,
#else
        string? pattern = null,
#endif
        string? @enum = null)
    {
        Description = Guard.NotNullOrEmpty(description, nameof(description));
        Type = Guard.NotNullOrEmpty(type, nameof(type));
        Example = Guard.NotNull(example, nameof(example));
        Format = format;
        Nullable = nullable;
        Pattern = pattern;
        Enum = @enum?.Split(',');
    }

    /// <summary>Gets the description of the OpenAPI Data Type.</summary>
    public string Description { get; }

    /// <summary>Gets the type of the OpenAPI Data Type.</summary>
    public string Type { get; }

    /// <summary>Gets the example of the OpenAPI Data Type.</summary>
    public object Example { get; }

    /// <summary>Gets the format of the OpenAPI Data Type.</summary>
    public string? Format { get; }

    /// <summary>Gets if the OpenAPI Data Type is nullable or not.</summary>
    public bool Nullable { get; }

    /// <summary>Gets the Pattern of the OpenAPI Data Type.</summary>
#if NET7_0_OR_GREATER
    [StringSyntax(StringSyntaxAttribute.Regex)]
#endif
    public string? Pattern { get; }

    /// <summary>Gets the Pattern of the OpenAPI Data Type.</summary>
    public string[]? Enum { get; }
}
