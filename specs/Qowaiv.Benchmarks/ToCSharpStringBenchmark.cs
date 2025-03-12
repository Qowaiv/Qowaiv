namespace Benchmarks;

[MemoryDiagnoser(true)]
public class ToCSharpStringBenchmark
{
    private static readonly Type Type = typeof(Dictionary<int, List<LinkedList<string>>>);

    [Params(1, 10, 100/*, 1000, 10_000*/)]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public string[] No_cache()
    {
        var result = new string[Count];
        for (var i = 0; i < Count; i++)
        {
            result[i] = Type.OldToCSharpString(true);
        }
        return result;
    }

    [Benchmark]
    public string[] With_cache()
    {
        var result = new string[Count];
        for (var i = 0; i < Count; i++)
        {
            result[i] = Type.ToCSharpString(true);
        }
        return result;
    }
}

file static class QowaivTypeExtensions
{
    [Pure]
    public static string OldToCSharpString(this Type type, bool withNamespace)
    {
        return new StringBuilder().AppendType(TypeInfo.New(type), withNamespace).ToString();
    }

    [FluentSyntax]
    private static StringBuilder AppendType(this StringBuilder sb, TypeInfo type, bool withNamespace)
    {
        if (primitives.TryGetValue(type.Type, out var primitive))
        {
            return sb.Append(primitive);
        }
        else if (type.NotNullable is { } notNullable)
        {
            return sb.AppendType(notNullable, withNamespace).Append('?');
        }
        else if (type.IsArray)
        {
            var array = new StringBuilder();
            do
            {
                array.Append('[').Append(',', type.ArrayRank - 1).Append(']');
                type = type.ElementType!;
            }
            while (type.IsArray);

            return sb.AppendType(type, withNamespace).Append(array);
        }
        else if (type.IsGenericTypeDefinition)
        {
            return sb
                .AppendPrefix(type, withNamespace)
                .Append(type.Name)
                .Append('<')
                .Append(new string(',', type.GetGenericArguments().Length - 1))
                .Append('>');
        }
        else if (type.GetGenericArguments() is { Length: > 0 } genericArguments)
        {
            sb.AppendPrefix(type, withNamespace)
               .Append(type.Name)
               .Append('<')
               .AppendType(genericArguments[0], withNamespace);

            for (var i = 1; i < genericArguments.Length; i++)
            {
                sb.Append(", ").AppendType(genericArguments[i], withNamespace);
            }
            return sb.Append('>');
        }
        else return sb.AppendPrefix(type, withNamespace).Append(type.Name);
    }

    /// <summary>Appends namespace and/or declaring type.</summary>
    [FluentSyntax]
    private static StringBuilder AppendPrefix(this StringBuilder sb, TypeInfo type, bool withNamespace)
    {
        if (type.IsNestedType)
        {
            sb.AppendType(type.DeclaringType!, withNamespace).Append('.');
        }
        else if (withNamespace && !string.IsNullOrEmpty(type.Namespace))
        {
            sb.Append(type.Namespace).Append('.');
        }
        return sb;
    }

    private static readonly Dictionary<Type, string> primitives = new()
    {
        [typeof(void)] = "void",
        [typeof(object)] = "object",
        [typeof(string)] = "string",
        [typeof(char)] = "char",
        [typeof(bool)] = "bool",
        [typeof(byte)] = "byte",
        [typeof(sbyte)] = "sbyte",
        [typeof(short)] = "short",
        [typeof(ushort)] = "ushort",
        [typeof(int)] = "int",
        [typeof(uint)] = "uint",
        [typeof(long)] = "long",
        [typeof(ulong)] = "ulong",
        [typeof(float)] = "float",
        [typeof(double)] = "double",
        [typeof(decimal)] = "decimal",
    };

    [Pure]
    private static TypeInfo? Info([NotNullIfNotNull(nameof(type))] this Type? type) => type is { } ? TypeInfo.New(type) : null;

    [Pure]
    private static bool IsGenericTypeParameter(this Type type)
        => type.IsGenericParameter && type.DeclaringMethod is null;

    private sealed class TypeInfo
    {
        private TypeInfo(Type type, IEnumerable<Type> genericArguments)
        {
            Type = type;
            GenericTypeArguments = [.. genericArguments
                .Select(t => t.Info())
                .Take(type.GetGenericArguments().Length)
                .OfType<TypeInfo>()];

            DeclaringType = IsNestedType && type.DeclaringType is { } declaringType
                ? new(declaringType, genericArguments)
                : null;
        }

        public Type Type { get; }

        public int ArrayRank => IsArray ? Type.GetArrayRank() : -1;

        public bool IsArray => Type.IsArray;

        public bool IsGenericTypeDefinition
            => GetGenericArguments() is { Length: > 0 } args
            && args.All(a => a.Type.IsGenericTypeParameter());

        /// <summary>A Nested type but not a generic parameter.</summary>
        public bool IsNestedType => Type.IsNested && !Type.IsGenericParameter;

        /// <remarks>
        /// Replaces generic definitions with actual choices of the nested type.
        /// </remarks>
        public TypeInfo? DeclaringType { get; }

        public TypeInfo? ElementType => Type.GetElementType().Info();

        [Pure]
        public TypeInfo[] GetGenericArguments()
            => DeclaringType is { } declaring && Type.IsNested
            ? GenericTypeArguments[declaring.GenericTypeArguments.Length..]
            : GenericTypeArguments;

        public TypeInfo[] GenericTypeArguments { get; }

        public TypeInfo? NotNullable => Nullable.GetUnderlyingType(Type).Info();

        public string Name
        {
            get
            {
                var split = Type.Name.IndexOf('`');
                return split > 0
                    ? Type.Name[..split]
                    : Type.Name;
            }
        }

        public string Namespace => Type.Namespace ?? string.Empty;

        [Pure]
        public override string ToString() => Type.ToCSharpString();

        [Pure]
        public static TypeInfo New(Type type)
            => new(type, type.GetGenericArguments());
    }
}
