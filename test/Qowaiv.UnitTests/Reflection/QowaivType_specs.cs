using NUnit.Framework;
using Qowaiv;
using Qowaiv.Financial;
using Qowaiv.Reflection;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Reflection.QowaivType_specs
{
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
    public class CSharpString
    {
        [TestCase(typeof(void), "void")]
        [TestCase(typeof(byte), "byte")]
        [TestCase(typeof(float), "float")]
        [TestCase(typeof(double), "double")]
        [TestCase(typeof(object), "object")]
        public void Uses_primitive_names(Type primitive, string csharpString)
            => Assert.That(QowaivType.ToCSharpString(primitive), Is.EqualTo(csharpString));

        [Test]
        public void Uses_question_mark_for_nullables()
            => Assert.That(QowaivType.ToCSharpString(typeof(int?)), Is.EqualTo("int?"));

        [TestCase(typeof(int[]), "int[]")]
        [TestCase(typeof(int[,]), "int[,]")]
        [TestCase(typeof(int[,,]), "int[,,]")]
        public void Supports_arrays(Type array, string csharpString)
            => Assert.That(QowaivType.ToCSharpString(array), Is.EqualTo(csharpString));

        [TestCase(typeof(int[][]), "int[][]")]
        [TestCase(typeof(int[][,]), "int[][,]")]
        [TestCase(typeof(int[][][]), "int[][][]")]
        public void Supports_jagged_arrays(Type jaggedArray, string csharpString)
            => Assert.That(QowaivType.ToCSharpString(jaggedArray), Is.EqualTo(csharpString));

        [TestCase(typeof(Nullable<>), "Nullable<>")]
        [TestCase(typeof(Dictionary<,>), "Dictionary<,>")]
        public void Supports_generic_type_definitions(Type definition, string csharpString)
            => Assert.That(QowaivType.ToCSharpString(definition), Is.EqualTo(csharpString));

        [TestCase(typeof(IList<Guid>), "IList<Guid>")]
        [TestCase(typeof(Dictionary<int, string>), "Dictionary<int, string>")]
        [TestCase(typeof(Dictionary<object, List<int?>>[]), "Dictionary<object, List<int?>>[]")]
        public void Supports_generic_type(Type definition, string csharpString)
            => Assert.That(QowaivType.ToCSharpString(definition), Is.EqualTo(csharpString));

        [TestCase(typeof(NestedType), "CSharpString.NestedType")]
        public void Supports_nested_types(Type nestedType, string csharpString)
           => Assert.That(QowaivType.ToCSharpString(nestedType), Is.EqualTo(csharpString));

        [Test]
        public void Supports_generic_arguments()
        {
            var generic = typeof(GenericOf).GetMethod(nameof(GenericOf.Default)).ReturnType;
           Assert.That(QowaivType.ToCSharpString(generic), Is.EqualTo("CSharpString.GenericOf.TModel"));
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
            => Assert.That(QowaivType.ToCSharpString(type, withNamespace: true), Is.EqualTo(csharpString));

        internal class NestedType { }

        internal class GenericOf
        {
            public TModel Default<TModel>() => default;
        }
    }
}
