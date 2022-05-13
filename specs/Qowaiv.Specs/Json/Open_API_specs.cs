namespace Json.Open_API_specs;

public class SVOs : SvoTypeTest
{
    [TestCaseSource(nameof(JsonSerializable))]
    public void Has_OpenApiDataType_attribute(Type svo)
        => svo.Should().BeDecoratedWith<OpenApiDataTypeAttribute>();
}

