﻿using Qowaiv.OpenApi;
using OpenApiDataTypeAttribute = Qowaiv.OpenApi.OpenApiDataTypeAttribute;

namespace Open_API_specs;

public class OpenApiDataType_attributes : SvoTypeTest
{
    public static IEnumerable<Type> Decoratable
        => JsonSerializable.Where(tp => !tp.IsGenericType);

    [TestCaseSource(nameof(Decoratable))]
    public void Decorates(Type svo)
        => svo.Should().BeDecoratedWith<OpenApiDataTypeAttribute>();
}

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
            format: "uuid-base64"));

    [Test]
    public void IIDentifierBehavior_is_resolved_as_Id_type()
        => OpenApiDataType.FromType(typeof(DecoratedId))
        .Should().Be(new OpenApiDataType(
            dataType: typeof(Id<DecoratedId>),
            description: "Custom description",
            type: "string",
            example: "custom example",
            format: "custom-uuid"));


    [OpenApiDataType(description: "Custom description", example: "custom example", type: "string", format: "custom-uuid")]
    internal sealed class DecoratedId : UuidBehavior { }
}

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
                    nullabe = info.Nullable,
                    @enum = info.Enum?.ToArray(),
                });

        Console.WriteLine(JsonConvert.SerializeObject(all, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
        }));

        all.Should().NotBeEmpty();
    }

    private sealed record OpenApiDataTypeInfo
    {
        public string description { get; init; }
        public object example { get; init; }
        public string type { get; init; }
        public string format { get; init; }
        public string pattern { get; init; }
        public bool nullabe { get; init; }
        public string[] @enum { get; init; }
    }
}

