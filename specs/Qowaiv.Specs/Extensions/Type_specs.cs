namespace Extensions.Type_specs;

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
    public void Supports_generic_type_definitions(Type definition, string csharpString)
        => definition.ToCSharpString().Should().Be(csharpString);

    [TestCase(typeof(IList<Guid>), "IList<Guid>")]
    [TestCase(typeof(Dictionary<int, string>), "Dictionary<int, string>")]
    [TestCase(typeof(Dictionary<object, List<int?>>[]), "Dictionary<object, List<int?>>[]")]
    public void Supports_generic_type(Type definition, string csharpString)
        => definition.ToCSharpString().Should().Be(csharpString);

    [TestCase(typeof(NestedType), "CSharpString.NestedType")]
    public void Supports_nested_types(Type nestedType, string csharpString)
       => nestedType.ToCSharpString().Should().Be(csharpString);

    [Test]
    public void Supports_generic_arguments()
    {
        var generic = typeof(GenericOf).GetMethod(nameof(GenericOf.Default)).ReturnType;
        generic.ToCSharpString().Should().Be("CSharpString.GenericOf.TModel");
    }

    [TestCase(typeof(string), "string")]
    [TestCase(typeof(byte[]), "byte[]")]
    [TestCase(typeof(Guid), "System.Guid")]
    [TestCase(typeof(Dictionary<string, Action>), "System.Collections.Generic.Dictionary<string, System.Action>")]
    [TestCase(typeof(long?), "long?")]
    [TestCase(typeof(Dictionary<,>), "System.Collections.Generic.Dictionary<,>")]
    [TestCase(typeof(Nullable<>), "System.Nullable<>")]
    [TestCase(typeof(Dictionary<object, List<int?>>[]), "System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<int?>>[]")]
    [TestCase(typeof(NestedType), "Extensions.Type_specs.CSharpString.NestedType")]
    public void With_namespaces_if_specified(Type type, string csharpString)
        => type.ToCSharpString(withNamespace: true).Should().Be(csharpString);

    internal class NestedType { }

    internal static class GenericOf
    {
        public static TModel? Default<TModel>() => default;
    }
}
