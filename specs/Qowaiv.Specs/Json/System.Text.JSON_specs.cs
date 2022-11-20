#if NET5_0_OR_GREATER
namespace Json.System_Text_JSON_specs;

public class All_SVO_s : SvoTypeTest
{
	[TestCaseSource(nameof(AllSvos))]
	public void have_JSON_Type_converter_attribute(Type type)
	{
		type.Should().BeDecoratedWith<System.Text.Json.Serialization.JsonConverterAttribute>();
		var converterType = type.GetCustomAttribute<System.Text.Json.Serialization.JsonConverterAttribute>().ConverterType;
		typeof(SvoJsonConverter<>).MakeGenericType(type).Should().NotBeAssignableTo(converterType);
	}
}
#endif
