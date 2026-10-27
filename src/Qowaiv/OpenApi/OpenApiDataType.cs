#pragma warning disable S107 // Methods should not have too many parameters
// To overcome the lack of the init key word in netstandard2.0.

using System.Reflection;

namespace Qowaiv.OpenApi;

/// <summary>Represents an Open API data type.</summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed record OpenApiDataType
{
    /// <summary>Initializes a new instance of the <see cref="OpenApiDataType" /> class.</summary>
    public OpenApiDataType(
        Type dataType,
        string description,
        string type,
        object? example,
        string? format = null,
        bool nullable = false,
        [StringSyntax(StringSyntaxAttribute.Regex)] string? pattern = null,
        IReadOnlyCollection<string>? @enum = null)
#pragma warning restore S107 // Methods should not have too many parameters
    {
        DataType = Guard.NotNull(dataType);
        Description = Guard.NotNullOrEmpty(description);
        Type = Guard.NotNullOrEmpty(type);
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
    [StringSyntax(StringSyntaxAttribute.Regex)]
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
        => Pattern is null
        || (str is { Length: > 0 } && Regex.IsMatch(str, '^' + Pattern + '$', RegexOptions.None, RegOptions.Timeout));

    /// <summary>
    /// Creates an <see cref="OpenApiDataType" /> based on a type, null if not
    /// decorated with a <see cref="OpenApiDataTypeAttribute" />.
    /// </summary>
    /// <param name="type">
    /// The type to create an <see cref="OpenApiDataType" /> for.
    /// </param>
    [Pure]
    public static OpenApiDataType? FromType(Type type)
        => Guard.NotNull(type).GetCustomAttributes<OpenApiDataTypeAttribute>().FirstOrDefault() is { } attr
        ? new(
            dataType: type,
            description: attr.Description,
            type: attr.Type,
            example: attr.Example,
            format: attr.Format,
            nullable: attr.Nullable,
            pattern: attr.Pattern,
            @enum: attr.Enum ?? DefinedValues(type))
        : null;

    [Pure]
    private static IReadOnlyCollection<string>? DefinedValues(Type type)
#if NETSTANDARD2_0
        => null;
#else
    {
        if (Interface() is { } contract && type.GetInterfaces().Contains(contract))
        {
            var values = new List<string>();
            try
            {
                foreach (var member in DefinedValues())
                {
                    values.Add(member is IFormattable formattable ? formattable.ToString(null, CultureInfo.InvariantCulture) : member.ToString());
                }
            }
            catch { return null; }
            return values;
        }
        return null;

        Type? Interface()
        {
            try { return typeof(IEnumerated<>).MakeGenericType(type); }
            catch { return null; }
        }

        IEnumerable DefinedValues()
            => (IEnumerable)type
            .GetProperty(nameof(IEnumerated<>.DefinedValues), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)!
            .GetValue(null)!;
    }
#endif

    /// <inheritdoc cref="IEquatable{T}" />
    [Pure]
    public bool Equals(OpenApiDataType? other)
        => other is not null
        && DataType == other.DataType
        && Description == other.Description
        && Type == other.Type
        && Example == other.Example
        && Format == other.Format
        && Nullable == other.Nullable
        && Pattern == other.Pattern
        && Enumerable.SequenceEqual(Enum ?? [], other.Enum ?? []);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Hash
        .Code(DataType)
        .And(Description)
        .And(Type)
        .And(Example)
        .And(Format)
        .And(Nullable)
        .And(Pattern)
        .And(Enum);
}
