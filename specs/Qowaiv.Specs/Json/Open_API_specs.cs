namespace Json.Open_API_specs;

public class SVOs : SvoTypeTest
{
    [TestCaseSource(nameof(JsonSerializable))]
    public void Has_OpenApiDataType_attribute(Type svo)
        => svo.Should().BeDecoratedWith<OpenApiDataTypeAttribute>();

    [Test]
    public void For_README_md()
    {
        var attributes = OpenApiDataTypeAttribute.From(typeof(Date).Assembly)
           .OrderBy(attr => attr.DataType.Namespace)
           .ThenBy(attr => attr.DataType.Name);

        var all = new Dictionary<string, OpenApiDataType>();

        foreach (var attribute in attributes)
        {
            var name = $"{attribute.DataType.Namespace}.{attribute.DataType.Name}".Replace("Qowaiv.", "", StringComparison.InvariantCulture);

            all[name] = new OpenApiDataType
            {
                description = attribute.Description,
                example = attribute.Example,
                type = attribute.Type,
                format = attribute.Format,
                pattern = attribute.Pattern,
                nullabe = attribute.Nullable,
                @enum = attribute.Enum,
            };
        }

        Console.WriteLine(JsonConvert.SerializeObject(all, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
        }));

        Assert.IsTrue(true, "Test should pass.");
    }

    private class OpenApiDataType
    {
        public string description { get; set; }
        public object example { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public string pattern { get; set; }
        public bool nullabe { get; set; }
        public string[] @enum { get; set; }
    }
}

