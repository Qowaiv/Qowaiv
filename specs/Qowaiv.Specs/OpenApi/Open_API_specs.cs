namespace Open_API_specs;

[Obsolete("Will be dropped in Qowaiv 8.0.")]
public class Open_API_data_type
{
    [Test]
    public void Resolved_by_base_if_not_decorated()
        => OpenApiDataType.FromType(typeof(ForUuid))
        .Should().Be(new OpenApiDataType(
            dataType: typeof(CustomUuid),
            description: "UUID based identifier",
            type: "string",
            example: "lmZO_haEOTCwGsCcbIZFFg",
            format: "uuid-base64",
            nullable: true));

    [Test]
    public void Identifier_behavior_is_resolved_as_Id_type()
        => OpenApiDataType.FromType(typeof(DecoratedId))
        .Should().Be(new OpenApiDataType(
            dataType: typeof(Id<DecoratedId>),
            description: "Custom description",
            type: "string",
            example: "custom example",
            format: "custom-uuid"));

    [Test]
    public void has_custom_debug_display()
            => new OpenApiDataType(
            dataType: typeof(Date),
            description: "Custom description",
            type: "string",
            example: "custom example",
            format: "custom-uuid",
            pattern: "[0-9]{4}-[0-9]{2}-[0-9]{2}")
        .Should().HaveDebuggerDisplay("{ type: string, desc: Custom description, example: custom example, format: custom-uuid, pattern: [0-9]{4}-[0-9]{2}-[0-9]{2} }");

    [OpenApiDataType(description: "Custom description", example: "custom example", type: "string", format: "custom-uuid")]
    internal sealed class DecoratedId : UuidBehavior { }
}

#if NET6_0_OR_GREATER
[Explicit]
public class README_md
{
    [Test]
    public void Describes()
    {
        var all = OpenApiDataTypes.FromAssemblies(typeof(Date).Assembly)
            .OrderBy(attr => attr.DataType.Namespace)
            .ThenBy(attr => attr.DataType.Name)
            .ToDictionary(
                info => $"{info.DataType.Namespace}.{info.DataType.Name}".Replace("Qowaiv.", "", StringComparison.InvariantCulture),
                info => new OpenApiDataTypeInfo
                {
                    description = info.Description,
                    example = info.Example,
                    type = info.Type,
                    format = info.Format,
                    pattern = info.Pattern,
                    nullable = info.Nullable,
                    @enum = info.Enum?.ToArray(),
                });

        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(all, Options));
        all.Should().NotBeEmpty();
    }

    private sealed record OpenApiDataTypeInfo
    {
        public string? description { get; init; }
        public object? example { get; init; }
        public string? type { get; init; }
        public string? format { get; init; }
        public string? pattern { get; init; }
        public bool nullable { get; init; }
        public string[]? @enum { get; init; } = [];
    }

    private static readonly System.Text.Json.JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };
}
#endif
