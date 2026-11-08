using Qowaiv.Json.Newtonsoft;

namespace Qowaiv.Json.UnitTests;

[InheritsTests]
public class NewtonsoftCanConvertTest : JsonCanConvertTestBase
{
    protected override bool CanConvert(Type? type)
    {
        var converter = new QowaivJsonConverter();
        return type is { } && converter.CanConvert(type);
    }
}
