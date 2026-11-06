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
    [Test]
    [Arguments(typeof(string))]
    [Arguments(typeof(int))]
    public void False_for_not_nullable(Type type)
        => type.IsNullable.Should().BeFalse();

    [Test]
    public void True_for_nullable()
        => typeof(int?).IsNullable.Should().BeTrue();
}

public class NotNullableType
{
    [Test]
    [Arguments(typeof(string))]
    [Arguments(typeof(int))]
    public void Returns_type_for_non_nullable(Type type)
        => QowaivType.GetNotNullableType(type).Should().Be(type);

    [Test]
    public void Returns_underlying_type_for_nullable()
        => QowaivType.GetNotNullableType(typeof(int?)).Should().Be<int>();
}

public class IsNumeric
{
    [Test]
    [Arguments(typeof(byte))]
    [Arguments(typeof(sbyte))]
    [Arguments(typeof(short))]
    [Arguments(typeof(ushort))]
    [Arguments(typeof(int))]
    [Arguments(typeof(uint))]
    [Arguments(typeof(long))]
    [Arguments(typeof(ulong))]
    [Arguments(typeof(float))]
    [Arguments(typeof(double))]
    [Arguments(typeof(decimal))]
    public void Is_true_for_primitive_numerics(Type type)
        => type.IsNumeric.Should().BeTrue();

    [Test]
    [Arguments(typeof(object))]
    [Arguments(typeof(string))]
    [Arguments(typeof(char))]
    [Arguments(typeof(bool))]
    [Arguments(typeof(Guid))]
    [Arguments(typeof(Amount))]
    [Arguments(typeof(BigInteger))]
    public void Is_false_for_all_other_types(Type type)
        => type.IsNumeric.Should().BeFalse();
}
public class IsDate
{
    [Test]
    [Arguments(typeof(DateTime))]
#if NET10_0_OR_GREATER
    [Arguments(typeof(DateOnly))]
#endif
    [Arguments(typeof(DateTimeOffset))]
    [Arguments(typeof(LocalDateTime))]
    [Arguments(typeof(Date))]
    [Arguments(typeof(WeekDate))]
    [Arguments(typeof(YearMonth))]
    public void Is_true_for_DateTime_DateOnly_and_Qowaiv_DateTypes(Type type)
        => type.IsDate.Should().BeTrue();

    [Test]
    [Arguments(typeof(object))]
    [Arguments(typeof(string))]
    [Arguments(typeof(char))]
    [Arguments(typeof(bool))]
    [Arguments(typeof(Guid))]
    [Arguments(typeof(Amount))]
    [Arguments(typeof(BigInteger))]
    public void Is_false_for_all_other_types(Type type)
        => type.IsDate.Should().BeFalse();
}

public class CSharpString
{
    [Test]
    [Arguments(typeof(void), "void")]
    [Arguments(typeof(byte), "byte")]
    [Arguments(typeof(float), "float")]
    [Arguments(typeof(double), "double")]
    [Arguments(typeof(object), "object")]
    public void Uses_primitive_names(Type primitive, string csharpString)
        => primitive.ToCSharpString().Should().Be(csharpString);

    [Test]
    public void Uses_question_mark_for_nullables()
        => (typeof(int?)).ToCSharpString().Should().Be("int?");

    [Test]
    [Arguments(typeof(int[]), "int[]")]
    [Arguments(typeof(int[,]), "int[,]")]
    [Arguments(typeof(int[,,]), "int[,,]")]
    public void Supports_arrays(Type array, string csharpString)
        => array.ToCSharpString().Should().Be(csharpString);

    [Test]
    [Arguments(typeof(int[][]), "int[][]")]
    [Arguments(typeof(int[][,]), "int[][,]")]
    [Arguments(typeof(int[][][]), "int[][][]")]
    public void Supports_jagged_arrays(Type jaggedArray, string csharpString)
        => jaggedArray.ToCSharpString().Should().Be(csharpString);

    [Test]
    [MethodDataSource(nameof(GenericTypeDefinitions))]
    public void Supports_generic_type_definitions(Type definition, string csharpString)
        => definition.ToCSharpString().Should().Be(csharpString);

    // The TUnit source generator can not handle typeof(Nested0<>.Nested1<>.Nested2<>) as attribute argument.
    public static IEnumerable<(Type, string)> GenericTypeDefinitions()
    {
        yield return (typeof(Nullable<>), "Nullable<>");
        yield return (typeof(Dictionary<,>), "Dictionary<,>");
        yield return (typeof(GenericBase<>.NestedType), "GenericBase<>.NestedType");
        yield return (typeof(Nested0<>.Nested1<>.Nested2<>), "Nested0<>.Nested1<>.Nested2<>");
    }

    [Test]
    [Arguments(typeof(IList<Guid>), "IList<Guid>")]
    [Arguments(typeof(Dictionary<int, string>), "Dictionary<int, string>")]
    [Arguments(typeof(Dictionary<object, List<int?>>[]), "Dictionary<object, List<int?>>[]")]
    public void Supports_generic_type(Type definition, string csharpString)
        => definition.ToCSharpString().Should().Be(csharpString);

    [Test]
    [Arguments(typeof(NestedType), "CSharpString.NestedType")]
    [Arguments(typeof(GenericBase<int>.NestedType), "GenericBase<int>.NestedType")]
    [Arguments(typeof(GenericBase<int>.GenericNested<long>), "GenericBase<int>.GenericNested<long>")]
    [Arguments(typeof(Nested0<int>.Nested1<long>.Nested2<bool>), "Nested0<int>.Nested1<long>.Nested2<bool>")]
    [Arguments(typeof(Nested0<int>.Nested1<long>.Nested2<bool>.Nested3<string, char>), "Nested0<int>.Nested1<long>.Nested2<bool>.Nested3<string, char>")]
    [Arguments(typeof(NonGenericBase.GenericNested<int>), "NonGenericBase.GenericNested<int>")]
    [Arguments(typeof(GenericBase<NonGenericBase.GenericNested<int>>.NestedType), "GenericBase<NonGenericBase.GenericNested<int>>.NestedType")]
    public void Supports_nested_types(Type nestedType, string csharpString)
        => nestedType.ToCSharpString().Should().Be(csharpString);

    [Test]
    public void Supports_generic_arguments()
    {
        var generic = typeof(GenericOf).GetMethod(nameof(GenericOf.Default))!.ReturnType;
        generic.ToCSharpString().Should().Be("TModel");
    }

    [Test]
    [Arguments(typeof(string), "string")]
    [Arguments(typeof(byte[]), "byte[]")]
    [Arguments(typeof(Guid), "System.Guid")]
    [Arguments(typeof(Dictionary<string, Action>), "System.Collections.Generic.Dictionary<string, System.Action>")]
    [Arguments(typeof(long?), "long?")]
    [Arguments(typeof(Dictionary<,>), "System.Collections.Generic.Dictionary<,>")]
    [Arguments(typeof(Nullable<>), "System.Nullable<>")]
    [Arguments(typeof(Dictionary<object, List<int?>>[]), "System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<int?>>[]")]
    [Arguments(typeof(NestedType), "Reflection.QowaivType_specs.CSharpString.NestedType")]
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
