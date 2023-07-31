using Qowaiv.Reflection;
using System.Numerics;

namespace Reflection.QowaivType_specs;

public class NullOrDefault
{
    [Test]
    public void Is_true_for_null()
        => Assert.That(QowaivType.IsNullOrDefaultValue(null), Is.True);

    [Test]
    public void Is_true_for_default_primitive()
        => Assert.That(QowaivType.IsNullOrDefaultValue(0), Is.True);

    [Test]
    public void Is_false_for_object()
        => Assert.That(QowaivType.IsNullOrDefaultValue(new object()), Is.False);

    [Test]
    public void Is_false_for_non_default_primitive()
        => Assert.That(QowaivType.IsNullOrDefaultValue(17), Is.False);
}

public class IsNullable
{
    [TestCase(typeof(string))]
    [TestCase(typeof(int))]
    public void False_for_not_nullable(Type type)
        => Assert.That(QowaivType.IsNullable(type), Is.False);

    [Test]
    public void True_for_nullable()
        => Assert.That(QowaivType.IsNullable(typeof(int?)), Is.True);
}

public class NotNullableType
{
    [TestCase(typeof(string))]
    [TestCase(typeof(int))]
    public void Returns_type_for_non_nullable(Type type)
        => Assert.That(QowaivType.GetNotNullableType(type), Is.EqualTo(type));

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
        => Assert.That(QowaivType.IsNumeric(type), Is.True);

    [TestCase(typeof(object))]
    [TestCase(typeof(string))]
    [TestCase(typeof(char))]
    [TestCase(typeof(bool))]
    [TestCase(typeof(Guid))]
    [TestCase(typeof(Amount))]
    [TestCase(typeof(BigInteger))]
    public void Is_false_for_all_other_types(Type type)
        => Assert.That(QowaivType.IsNumeric(type), Is.False);
}
public class IsDate
{
    [TestCase(typeof(DateTime))]
    [TestCase(typeof(DateTimeOffset))]
    [TestCase(typeof(LocalDateTime))]
    [TestCase(typeof(Date))]
    [TestCase(typeof(WeekDate))]
    public void Is_true_for_DateTime_and_Qowaiv_DateTypes(Type type)
        => Assert.That(QowaivType.IsDate(type), Is.True);

    [TestCase(typeof(object))]
    [TestCase(typeof(string))]
    [TestCase(typeof(char))]
    [TestCase(typeof(bool))]
    [TestCase(typeof(Guid))]
    [TestCase(typeof(Amount))]
    [TestCase(typeof(BigInteger))]
    public void Is_false_for_all_other_types(Type type)
        => Assert.That(QowaivType.IsDate(type), Is.False);
}
