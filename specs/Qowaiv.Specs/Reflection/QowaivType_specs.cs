using Qowaiv.Reflection;
using System.Numerics;

namespace Reflection.QowaivType_specs;

public class NullOrDefault
{
    [Test]
    public void Is_true_for_null()
        => QowaivType.IsNullOrDefaultValue(null).Should().BeTrue();

    [Test]
    public void Is_true_for_default_primitive()
        => QowaivType.IsNullOrDefaultValue(0).Should().BeTrue();

    [Test]
    public void Is_false_for_object()
        => QowaivType.IsNullOrDefaultValue(new object()).Should().BeFalse();

    [Test]
    public void Is_false_for_non_default_primitive()

        => QowaivType.IsNullOrDefaultValue(17).Should().BeFalse();
}

public class IsNullable
{
    [TestCase(typeof(string))]
    [TestCase(typeof(int))]
    public void False_for_not_nullable(Type type)
        => QowaivType.IsNullable(type).Should().BeFalse();

    [Test]
    public void True_for_nullable()
        => QowaivType.IsNullable(typeof(int?)).Should().BeTrue();
}

public class NotNullableType
{
    [TestCase(typeof(string))]
    [TestCase(typeof(int))]
    public void Returns_type_for_non_nullable(Type type)
#pragma warning disable FAA0004 // Replace NUnit assertion with Fluent Assertions equivalent
        => Assert.That(QowaivType.GetNotNullableType(type), Is.EqualTo(type));
#pragma warning restore FAA0004 // Replace NUnit assertion with Fluent Assertions equivalent

    [Test]
    public void Returns_underlying_type_for_nullable()
        => Assert.That(QowaivType.GetNotNullableType(typeof(int?)), Is.EqualTo(typeof(int)));
}

public class IsNumeric
{
    [TestCase(typeof(byte))]
    [TestCase(typeof(sbyte))]
    [TestCase(typeof(short))]
    [TestCase(typeof(ushort))]
    [TestCase(typeof(int))]
    [TestCase(typeof(uint))]
    [TestCase(typeof(long))]
    [TestCase(typeof(ulong))]
    [TestCase(typeof(float))]
    [TestCase(typeof(double))]
    [TestCase(typeof(decimal))]
    public void Is_true_for_primitive_numerics(Type type)
        => QowaivType.IsNumeric(type).Should().BeTrue();

    [TestCase(typeof(object))]
    [TestCase(typeof(string))]
    [TestCase(typeof(char))]
    [TestCase(typeof(bool))]
    [TestCase(typeof(Guid))]
    [TestCase(typeof(Amount))]
    [TestCase(typeof(BigInteger))]
    public void Is_false_for_all_other_types(Type type)
        => QowaivType.IsNumeric(type).Should().BeFalse();
}
public class IsDate
{
    [TestCase(typeof(DateTime))]
    [TestCase(typeof(DateTimeOffset))]
    [TestCase(typeof(LocalDateTime))]
    [TestCase(typeof(Date))]
    [TestCase(typeof(WeekDate))]
    public void Is_true_for_DateTime_and_Qowaiv_DateTypes(Type type)
        => QowaivType.IsDate(type).Should().BeTrue();

    [TestCase(typeof(object))]
    [TestCase(typeof(string))]
    [TestCase(typeof(char))]
    [TestCase(typeof(bool))]
    [TestCase(typeof(Guid))]
    [TestCase(typeof(Amount))]
    [TestCase(typeof(BigInteger))]
    public void Is_false_for_all_other_types(Type type)
        => QowaivType.IsDate(type).Should().BeFalse();
}
