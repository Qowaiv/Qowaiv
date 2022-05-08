namespace Qowaiv.OpenApi;

/// <summary>Describes how a type should be described as OpenAPI Data Type.</summary>
/// <remarks>
/// See: https://swagger.io/docs/specification/data-models/data-types/
/// </remarks>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
[DebuggerDisplay("{DebuggerDisplay}")]
public sealed class OpenApiDataTypeAttribute : Attribute
{
    /// <summary>Creates a new instance of a <see cref="OpenApiDataTypeAttribute"/>.</summary>
    public OpenApiDataTypeAttribute(
        string description,
        string type,
        object example,
        string? format = null,
        bool nullable = false,
        string? pattern = null,
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
    public string? Pattern { get; }

    /// <summary>Gets the Pattern of the OpenAPI Data Type.</summary>
    public string[]? Enum { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
    {
        get
        {
            var sb = new StringBuilder();
            sb.Append($@"{{ type: {Type}, desc: {Description}, example: {Example}");

            if (!string.IsNullOrEmpty(Format))
            {
                sb.Append($@", format: {Format}");
            }
            if (!string.IsNullOrEmpty(Pattern))
            {
                sb.Append($@", pattern: {Pattern}");
            }
            if (Nullable)
            {
                sb.Append($", nullable: true");
            }
            sb.Append(" }");

            return sb.ToString();
        }
    }
}
