#if NET8_0_OR_GREATER

using Qowaiv.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using static Extensions.Type_specs.CSharpString;

namespace Json.ModifyTypeInfo_specs;

public class IgnoreEmptySvos
{
    internal static readonly JsonSerializerOptions Options = new()
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        {
            Modifiers = { ModifyTypeInfo.IgnoreEmptySvos },
        },
    };

    [Test]
    public void ignores_empty_svos()
    {
        var json = JsonSerializer.Serialize(new Container<InternationalBankAccountNumber>(default), Options);
        json.Should().Be("{}");
    }

    [Test]
    public void serializes_generic_structs()
    {
        var json = JsonSerializer.Serialize(new Container<Generic<int>>(new(42)), Options);
        json.Should().Be("""{"Val":{"Val":42}}""");
    }

    [Test]
    public void serializes_non_empty_svos()
    {
        var json = JsonSerializer.Serialize(new Container<InternationalBankAccountNumber>(Svo.Iban), Options);
        json.Should().Be("""{"Val":"NL20INGB0001234567"}""");
    }

    private sealed record Container<T>(T Val);

    private readonly record struct Generic<T>(T Val) where T : struct;
}

#endif
