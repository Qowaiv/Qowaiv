using Qowaiv.Customization;
using Qowaiv.Identifiers;
using System.Reflection;

namespace Qowaiv.OpenApi;

/// <summary>Represents an Open API data type.</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
public sealed record OpenApiDataType
{
    /// <summary>Creates a new instance of the <see cref="OpenApiDataType"/> class.</summary>
#pragma warning disable S107 // Methods should not have too many parameters
    // To overcome the lack of the init key word in netstandard2.0.
    public OpenApiDataType(Type dataType,
       string description,
       string type,
       object? example,
       string? format = null,
       bool nullable = false,
#if NET7_0_OR_GREATER
        [StringSyntax(StringSyntaxAttribute.Regex)] string? pattern = null,
#else
        string? pattern = null,
#endif
       IReadOnlyCollection<string>? @enum = null)
#pragma warning restore S107 // Methods should not have too many parameters
    {
        DataType = Guard.NotNull(dataType, nameof(dataType));
        Description = Guard.NotNullOrEmpty(description, nameof(description));
        Type = Guard.NotNullOrEmpty(type, nameof(type));
        Example = example;
        Format = format;
        Nullable = nullable;
        Pattern = pattern;
        Enum = @enum;
    }

    /// <summary>Gets the bound .NET type of the OpenAPI Data Type.</summary>
    /// <remarks>
    /// Only set when received via one of the <c>From()</c> factory methods.
    /// </remarks>
    public Type DataType { get; }

    /// <summary>Gets the description of the OpenAPI Data Type.</summary>
    public string Description { get; }

    /// <summary>Gets the type of the OpenAPI Data Type.</summary>
    public string Type { get; }

    /// <summary>Gets the example of the OpenAPI Data Type.</summary>
    public object? Example { get; }

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
    public IReadOnlyCollection<string>? Enum { get; }

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

    /// <summary>Returns true if the pattern matches the input, or the is no pattern restriction.</summary>
    [Pure]
    public bool Matches(string? str)
        => Pattern is null || Regex.IsMatch(str!, Pattern, RegexOptions.None, RegOptions.Timeout);

    /// <summary>
    /// Creates an <see cref="OpenApiDataType"/> based on a type, null if not
    /// decorated with a <see cref="OpenApiDataTypeAttribute"/>.
    /// </summary>
    /// <param name="type">
    /// The type to create an <see cref="OpenApiDataType" /> for.
    /// </param>
    [Pure]
    public static OpenApiDataType? FromType(Type type)
        => Guard.NotNull(type, nameof(type)).GetCustomAttributes<OpenApiDataTypeAttribute>().FirstOrDefault() is { } attr
        ? new(
            dataType: AsDataType(type),
            description: attr.Description,
            type: attr.Type,
            example: attr.Example,
            format: attr.Format,
            nullable: attr.Nullable,
            pattern: attr.Pattern,
            @enum: attr.Enum)
        : null;

    [Pure]
    private static Type AsDataType(Type type)
        => IdDataType(type)
        ?? SvoDataType(type)
        ?? type;

    [Pure]
    private static Type? IdDataType(Type type)
         => !type.IsAbstract
        && type.GetInterfaces().Contains(typeof(IIdentifierBehavior))
        && HasPublicParameterlessCtor(type)
        ? typeof(Id<>).MakeGenericType(type)
        : null;

    [Pure]
    private static Type? SvoDataType(Type type)
       => !type.IsAbstract
      && type.IsSubclassOf(typeof(SvoBehavior))
      && HasPublicParameterlessCtor(type)
      ? typeof(Svo<>).MakeGenericType(type)
      : null;

    [Pure]
    private static bool HasPublicParameterlessCtor(Type type) 
        => type.GetConstructors().Any(ctor => !ctor.GetParameters().Any());
}
