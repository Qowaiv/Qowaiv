#if NET8_0_OR_GREATER

using Qowaiv.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

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
        var json = JsonSerializer.Serialize(new WithNonContinuousSvo { Iban = default }, Options);
        json.Should().Be("{}");
    }

    [Test]
    public void serializes_non_empty_svos()
    {
        var json = JsonSerializer.Serialize(new WithNonContinuousSvo { Iban = Svo.Iban }, Options);
        json.Should().Be(@"{""Iban"":""NL20INGB0001234567""}");
    }

    private sealed class WithNonContinuousSvo { public InternationalBankAccountNumber Iban { get; init; } }
}


#endif
