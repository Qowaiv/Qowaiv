namespace TestTools.Format_provider_helper_specs;

public class GetFormat
{
    [Test]
    public void Empty_returns_null()
        => FormatProvider.Empty.GetFormat(typeof(decimal)).Should().BeNull();
    [Test]
    public void CustomFormatter_returns_null_on_null_type()
        => FormatProvider.CustomFormatter.GetFormat(null).Should().BeNull();

    [Test]
    public void CustomFormatter_returns_itself_on_ICustomFormatter()
       => FormatProvider.CustomFormatter.GetFormat(typeof(ICustomFormatter))
        .Should().Be(FormatProvider.CustomFormatter);
}
