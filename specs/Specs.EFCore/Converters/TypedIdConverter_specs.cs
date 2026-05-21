using Qowaiv.EntityFrameworkCore.Converters;
using Specs_Generated;

namespace Specs.Converters.TypedIdConverter_specs;

public class Creates
{
    [Test]
    public void From_underlying_GUID()
    {
        var converter = TypedIdConverter.Create<GuidBasedId>();
        converter.Should().BeEquivalentTo(new
        {
            ModelClrType = typeof(GuidBasedId),
            ProviderClrType = typeof(Guid),
        });
    }

    [Test]
    public void From_underlying_long()
    {
        var converter = TypedIdConverter.Create<Int64BasedId>();
        converter.Should().BeEquivalentTo(new
        {
            ModelClrType = typeof(Int64BasedId),
            ProviderClrType = typeof(long),
        });
    }

    [Test]
    public void From_underlying_int()
    {
        var converter = TypedIdConverter.Create<Int32BasedId>();
        converter.Should().BeEquivalentTo(new
        {
            ModelClrType = typeof(Int32BasedId),
            ProviderClrType = typeof(int),
        });
    }

    [Test]
    public void From_underlying_string()
    {
        var converter = TypedIdConverter.Create<StringBasedId>();
        converter.Should().BeEquivalentTo(new
        {
            ModelClrType = typeof(GuidBasedId),
            ProviderClrType = typeof(string),
        });
    }

    [Test]
    public void From_not_supported_underlying_type()
    {
        var converter = TypedIdConverter.Create<UuidBasedId>();
        converter.Should().BeEquivalentTo(new
        {
            ModelClrType = typeof(UuidBasedId),
            ProviderClrType = typeof(string),
        });
    }
}

public class Does_not_support
{
    [Test]
    public void Ids_Lacking_TypeConverter() => typeof(object)
        .Invoking(TypedIdConverter.Create)
        .Should().Throw<NotSupportedException>()
        .WithMessage("Type 'object' lacks a custom type converter.");
}
