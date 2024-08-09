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
        => QowaivType.GetNotNullableType(type).Should().Be(type);

    [Test]
    public void Returns_underlying_type_for_nullable()
        => QowaivType.GetNotNullableType(typeof(int?)).Should().Be(typeof(int));
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
#if NET6_0_OR_GREATER
    [TestCase(typeof(DateOnly))]
#endif
    [TestCase(typeof(DateTimeOffset))]
    [TestCase(typeof(LocalDateTime))]
    [TestCase(typeof(Date))]
    [TestCase(typeof(WeekDate))]
    [TestCase(typeof(YearMonth))]
    public void Is_true_for_DateTime_DateOnly_and_Qowaiv_DateTypes(Type type)
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
