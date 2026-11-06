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
        [StringSyntax(StringSyntaxAttribute.Regex)] string? pattern = null,
        IReadOnlyCollection<string>? @enum = null)
    {
        DataType = Guard.NotNull(dataType);
        Description = Guard.NotNullOrEmpty(description);
        Type = Guard.NotNullOrEmpty(type);
        Example = example;
        Format = format;
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

    /// <summary>Gets the Pattern of the OpenAPI Data Type.</summary>
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public string? Pattern { get; }

    /// <summary>Gets the allowed values of the OpenAPI Data Type.</summary>
    public IReadOnlyCollection<string>? Enum { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
    {
        get
        {
            var sb = new StringBuilder();
            sb.Append($@"{{ type: {Type}, desc: {Description}, example: {Example}");

            if (Format is { Length: > 0 })
                sb.Append($@", format: {Format}");

            if (Pattern is { Length: > 0 })
                sb.Append($@", pattern: {Pattern}");

            sb.Append(" }");

            return sb.ToString();
        }
    }

    /// <summary>Returns true if the pattern matches the input, or the is no pattern restriction.</summary>
    [Pure]
    public bool Matches(string? str)
        => Pattern is null
        || (str is { Length: > 0 } && Regex.IsMatch(str, '^' + Pattern + '$', RegexOptions.None, RegOptions.Timeout));

    /// <inheritdoc cref="IEquatable{T}" />
    [Pure]
    public bool Equals([NotNullWhen(true)]OpenApiDataType? other)
        => other is not null
        && DataType == other.DataType
        && Description == other.Description
        && Type == other.Type
        && Equals(Example, other.Example)
        && Format == other.Format
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
        .And(Pattern)
        .And(Enum ?? []);

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
            pattern: attr.Pattern,
            @enum: attr.Enum ?? DefinedValues(type))
        : null;

    // This is a convenience helper for specifying Open API specs. Failing to resolve the
    // defined values should not crash the generation of those specs, hence no exception is thrown.
    [Pure]
    private static IReadOnlyList<string>? DefinedValues(Type type)
#if NETSTANDARD2_0
        => null;
#else
    {
        if (type.GetInterfaces().Contains(typeof(IEnumerated)))
        {
            var values = new List<string>();
            try
            {
                foreach (var value in DefinedValues())
                    Add(value, values);
            }
            catch { return null; }

            if (Unknown.Value(type) is { } unknown)
                Add(unknown, values);

            return values;
        }
        return null;

        void Add(object value, List<string> values) => values.Add(value is IFormattable formattable ? formattable.ToString(null, CultureInfo.InvariantCulture) : value.ToString()!);

        IEnumerable DefinedValues()
            => (IEnumerable)type
            .GetProperty(nameof(IEnumerated<>.DefinedValues), Flags)!
            .GetValue(null)!;
    }

#pragma warning disable S3011 // We also want to catch explitly defined.
    private const BindingFlags Flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
#endif
}
