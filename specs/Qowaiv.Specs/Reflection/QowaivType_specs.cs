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
        => type.IsNullable.Should().BeFalse();

    [Test]
    public void True_for_nullable()
        => typeof(int?).IsNullable.Should().BeTrue();
}

public class NotNullableType
{
    [TestCase(typeof(string))]
    [TestCase(typeof(int))]
    public void Returns_type_for_non_nullable(Type type)
        => QowaivType.GetNotNullableType(type).Should().Be(type);

    [Test]
    public void Returns_underlying_type_for_nullable()
        => QowaivType.GetNotNullableType(typeof(int?)).Should().Be<int>();
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
        => type.IsNumeric.Should().BeTrue();

    [TestCase(typeof(object))]
    [TestCase(typeof(string))]
    [TestCase(typeof(char))]
    [TestCase(typeof(bool))]
    [TestCase(typeof(Guid))]
    [TestCase(typeof(Amount))]
    [TestCase(typeof(BigInteger))]
    public void Is_false_for_all_other_types(Type type)
        => type.IsNumeric.Should().BeFalse();
}
public class IsDate
{
    [TestCase(typeof(DateTime))]
#if NET10_0_OR_GREATER
    [TestCase(typeof(DateOnly))]
#endif
    [TestCase(typeof(DateTimeOffset))]
    [TestCase(typeof(LocalDateTime))]
    [TestCase(typeof(Date))]
    [TestCase(typeof(WeekDate))]
    [TestCase(typeof(YearMonth))]
    public void Is_true_for_DateTime_DateOnly_and_Qowaiv_DateTypes(Type type)
        => type.IsDate.Should().BeTrue();

    [TestCase(typeof(object))]
    [TestCase(typeof(string))]
    [TestCase(typeof(char))]
    [TestCase(typeof(bool))]
    [TestCase(typeof(Guid))]
    [TestCase(typeof(Amount))]
    [TestCase(typeof(BigInteger))]
    public void Is_false_for_all_other_types(Type type)
        => type.IsDate.Should().BeFalse();
}

public class CSharpString
{
    [TestCase(typeof(void), "void")]
    [TestCase(typeof(byte), "byte")]
    [TestCase(typeof(float), "float")]
    [TestCase(typeof(double), "double")]
    [TestCase(typeof(object), "object")]
    public void Uses_primitive_names(Type primitive, string csharpString)
        => primitive.ToCSharpString().Should().Be(csharpString);

    [Test]
    public void Uses_question_mark_for_nullables()
        => (typeof(int?)).ToCSharpString().Should().Be("int?");

    [TestCase(typeof(int[]), "int[]")]
    [TestCase(typeof(int[,]), "int[,]")]
    [TestCase(typeof(int[,,]), "int[,,]")]
    public void Supports_arrays(Type array, string csharpString)
        => array.ToCSharpString().Should().Be(csharpString);

    [TestCase(typeof(int[][]), "int[][]")]
    [TestCase(typeof(int[][,]), "int[][,]")]
    [TestCase(typeof(int[][][]), "int[][][]")]
    public void Supports_jagged_arrays(Type jaggedArray, string csharpString)
        => jaggedArray.ToCSharpString().Should().Be(csharpString);

    [TestCase(typeof(Nullable<>), "Nullable<>")]
    [TestCase(typeof(Dictionary<,>), "Dictionary<,>")]
    [TestCase(typeof(GenericBase<>.NestedType), "GenericBase<>.NestedType")]
    [TestCase(typeof(Nested0<>.Nested1<>.Nested2<>), "Nested0<>.Nested1<>.Nested2<>")]
    public void Supports_generic_type_definitions(Type definition, string csharpString)
        => definition.ToCSharpString().Should().Be(csharpString);

    [TestCase(typeof(IList<Guid>), "IList<Guid>")]
    [TestCase(typeof(Dictionary<int, string>), "Dictionary<int, string>")]
    [TestCase(typeof(Dictionary<object, List<int?>>[]), "Dictionary<object, List<int?>>[]")]
    public void Supports_generic_type(Type definition, string csharpString)
        => definition.ToCSharpString().Should().Be(csharpString);

    [TestCase(typeof(NestedType), "CSharpString.NestedType")]
    [TestCase(typeof(GenericBase<int>.NestedType), "GenericBase<int>.NestedType")]
    [TestCase(typeof(GenericBase<int>.GenericNested<long>), "GenericBase<int>.GenericNested<long>")]
    [TestCase(typeof(Nested0<int>.Nested1<long>.Nested2<bool>), "Nested0<int>.Nested1<long>.Nested2<bool>")]
    [TestCase(typeof(Nested0<int>.Nested1<long>.Nested2<bool>.Nested3<string, char>), "Nested0<int>.Nested1<long>.Nested2<bool>.Nested3<string, char>")]
    [TestCase(typeof(NonGenericBase.GenericNested<int>), "NonGenericBase.GenericNested<int>")]
    [TestCase(typeof(GenericBase<NonGenericBase.GenericNested<int>>.NestedType), "GenericBase<NonGenericBase.GenericNested<int>>.NestedType")]
    public void Supports_nested_types(Type nestedType, string csharpString)
        => nestedType.ToCSharpString().Should().Be(csharpString);

    [Test]
    public void Supports_generic_arguments()
    {
        var generic = typeof(GenericOf).GetMethod(nameof(GenericOf.Default))!.ReturnType;
        generic.ToCSharpString().Should().Be("TModel");
    }

    [TestCase(typeof(string), "string")]
    [TestCase(typeof(byte[]), "byte[]")]
    [TestCase(typeof(Guid), "System.Guid")]
    [TestCase(typeof(Dictionary<string, Action>), "System.Collections.Generic.Dictionary<string, System.Action>")]
    [TestCase(typeof(long?), "long?")]
    [TestCase(typeof(Dictionary<,>), "System.Collections.Generic.Dictionary<,>")]
    [TestCase(typeof(Nullable<>), "System.Nullable<>")]
    [TestCase(typeof(Dictionary<object, List<int?>>[]), "System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<int?>>[]")]
    [TestCase(typeof(NestedType), "Reflection.QowaivType_specs.CSharpString.NestedType")]
    public void With_namespaces_if_specified(Type type, string csharpString)
        => type.ToCSharpString(withNamespace: true).Should().Be(csharpString);

    [EmptyTestClass]
    internal class NestedType { }

    internal static class GenericOf
    {
        public static TModel? Default<TModel>() => default;
    }
}

internal class GenericBase<T>
{
    public T? Model { get; set; }

    [EmptyTestClass]
    internal class NestedType { }

    internal class GenericNested<T2>
    {
        public T2? Model { get; set; }
    }
}

internal class Nested0<T0>
{
    public T0? Model { get; set; }

    internal class Nested1<T1>
    {
        public T1? Model { get; set; }

        internal class Nested2<T2>
        {
            public T2? Model { get; set; }

            internal class Nested3<T3, T4>
            {
                public T3? First { get; set; }
                public T4? Second { get; set; }
            }
        }
    }
}

internal class NonGenericBase
{
    internal class GenericNested<T>
    {
        public T? Model { get; set; }
    }
}
