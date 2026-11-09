using Qowaiv.Financial;
using Qowaiv.TestTools.Globalization;
using Qowaiv.Validation.DataAnnotations;
using System.ComponentModel;
using System.Net.Http;

namespace Data_Annotations.Attributes.SetOf_values_specs;

public class Supports
{
    [Test]
    public void primitive_params_of_same_type_as_TValue()
        => new AllowedAttribute<int>(17, 42).Values
        .Should().BeEquivalentTo([17, 42]);

    [Test]
    public void primitive_params_that_can_be_converted()
    {
        using (TestCultures.en_US.Scoped())
        {
            new AllowedAttribute<Amount>(42.12, "17.30").Values
                .Should().BeEquivalentTo([Amount.Create(42.12), 17.30m.Amount()]);
        }
    }

    [Test]
    public void HttpMethod_as_TValue()
        => new AllowedAttribute<HttpMethod>("GET", "POST").Values
        .Should().BeEquivalentTo([HttpMethod.Get, HttpMethod.Post]);

    [Test]
    public void custom_type_converter()
        => new AllowedAttribute<int>('A', 'B'){ TypeConverter = typeof(AsciiConverter) }.Values
        .Should().BeEquivalentTo([0 + 'A', 0 + 'B']);

    private sealed class AsciiConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(char);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
            => value is char ch ? (int)ch : null;
    }
}
