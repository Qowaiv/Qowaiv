namespace SVO_contract_specs;

public class Implements : SingleValueObjectSpecs
{
    [TestCaseSource(nameof(AllSvos))]
    public void IEquatable(Type type) => type.Should().Implement(typeof(IEquatable<>).MakeGenericType(type));

    [TestCaseSource(nameof(AllSvos))]
    public void IComparable(Type type) => type.Should().Implement<IComparable>();

    [TestCaseSource(nameof(AllSvos))]
    public void IComparable_TSelf(Type type) => type.Should().Implement(typeof(IComparable<>).MakeGenericType(type));

    [TestCaseSource(nameof(AllSvos))]
    public void IFormattable(Type type) => type.Should().Implement<IFormattable>();

    [TestCaseSource(nameof(AllSvos))]
    public void ISerializable(Type type) => type.Should().Implement<ISerializable>();

    [TestCaseSource(nameof(AllSvos))]
    public void IXmlSerializable(Type type) => type.Should().Implement<IXmlSerializable>();

#if NET7_0_OR_GREATER
    [TestCaseSource(nameof(AllSvosExceptGeneric))]
    public void IEqualityOperators(Type type) => type.Should().Implement(typeof(System.Numerics.IEqualityOperators<,,>).MakeGenericType(type, type, typeof(bool)));

    [TestCaseSource(nameof(AllSvosExceptGeneric))]
    public void IParsable(Type type) => type.Should().Implement(typeof(IParsable<>).MakeGenericType(type));
#endif
}

public class Is_decorated_with : SingleValueObjectSpecs
{
    [TestCaseSource(nameof(AllSvosExceptGeneric))]
    public void Type_converter(Type svo)
        => svo.Should().HaveTypeConverterDefined();

    [TestCaseSource(nameof(AllSvos))]
    public void Open_API_data_type_Legacy(Type svo)
        => svo.Should().BeDecoratedWith<Qowaiv.Json.OpenApiDataTypeAttribute>();
    
    [TestCaseSource(nameof(AllSvosExceptGeneric))]
    public void Open_API_data_type(Type svo)
        => svo.Should().BeDecoratedWith<Qowaiv.OpenApi.OpenApiDataTypeAttribute>();

#if NET5_0_OR_GREATER
    [TestCaseSource(nameof(AllSvos))]
    public void System_Text_Json_JsonConverter(Type type)
    {
        type.Should().BeDecoratedWith<System.Text.Json.Serialization.JsonConverterAttribute>();
        var converterType = type.GetCustomAttribute<System.Text.Json.Serialization.JsonConverterAttribute>()?.ConverterType;

        typeof(SvoJsonConverter<>).MakeGenericType(type).Should().NotBeAssignableTo(converterType);
    }
#endif
}
