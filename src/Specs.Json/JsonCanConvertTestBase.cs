using Qowaiv.Json.UnitTests.Models;

namespace Qowaiv.Json.UnitTests;

/// <remarks>
/// This abstract base class should help to guarantee that different
/// implementations have the same behaviour.
/// </remarks>
public abstract class JsonCanConvertTestBase
{
    [Test]
    public void CanConvert_Null_False() => CanConvert(null).Should().BeFalse();

    [Test]
    public void CanConvert_Object_False() => CanConvert(typeof(object)).Should().BeFalse();

    [Test]
    public void CanConvert_Int32_False() => CanConvert(typeof(int)).Should().BeFalse();

    [Test]
    public void CanConvert_SvoWithFromJsonBoolOnly_False() => CanConvert(typeof(SvoWithFromJsonBoolOnly)).Should().BeFalse();
    
    [Test]
    public void CanConvert_SvoWithFromJsonStringOnly_True() => CanConvert(typeof(SvoWithFromJsonStringOnly)).Should().BeTrue();

    [Test]
    public void CanConvert_SvoWithFromJson_True() => CanConvert(typeof(SvoWithFromJson)).Should().BeTrue();

    [Test]
    public void CanConvert_NullableSvoWithFromJson_True() => CanConvert(typeof(SvoWithFromJson?)).Should().BeTrue();

    [Test]
    public void CanConvert_SvoWithFromJsonClass_True() => CanConvert(typeof(SvoWithFromJsonClass)).Should().BeTrue();

    protected abstract bool CanConvert(Type? type);
}
