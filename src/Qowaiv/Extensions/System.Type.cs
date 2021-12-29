using Qowaiv;
using System.Text;

namespace System;

/// <summary>Extensions on <see cref="Type"/>.</summary>
public static class QowaivTypeExtensions
{

    /// <summary>Gets a C# formatted <see cref="string"/> representing the <see cref="Type"/>.</summary>
    /// <param name="type">
    /// The type to format as C# string.
    /// </param>
    [Pure]
    public static string ToCSharpString(this Type type) => type.ToCSharpString(false);

    /// <summary>Gets a C# formatted <see cref="string"/> representing the <see cref="Type"/>.</summary>
    /// <param name="type">
    /// The type to format as C# string.
    /// </param>
    /// <param name="withNamespace">
    /// Should the namespace be displayed or not.
    /// </param>
    [Pure]
    public static string ToCSharpString(this Type type, bool withNamespace)
    {
        Guard.NotNull(type, nameof(type));
        return new StringBuilder().AppendType(type, withNamespace).ToString();
    }

    [FluentSyntax]
    private static StringBuilder AppendType(this StringBuilder sb, Type type, bool withNamespace)
    {
        if (primitives.TryGetValue(type, out var primitive))
        {
            return sb.Append(primitive);
        }
        else if (Nullable.GetUnderlyingType(type) is Type underlyging)
        {
            return sb.AppendType(underlyging, withNamespace).Append('?');
        }
        else if (type.IsArray)
        {
            var array = new StringBuilder();
            do
            {
                array.Append('[').Append(',', type.GetArrayRank() - 1).Append(']');
                type = Not.Null(type.GetElementType());
            }
            while (type.IsArray);

            return sb.AppendType(type, withNamespace)
                .Append(array);
        }
        else if (type.IsGenericTypeDefinition)
        {
            return sb
                .AppendNamespace(type, withNamespace)
                .Append(type.ToNonGeneric())
                .Append('<')
                .Append(new string(',', type.GetGenericArguments().Length - 1))
                .Append('>');
        }
        else if (type.IsGenericType)
        {
            var arguments = type.GetGenericArguments();

            sb.AppendNamespace(type, withNamespace)
               .Append(type.ToNonGeneric())
               .Append('<')
               .AppendType(arguments[0], withNamespace);

            for (var i = 1; i < arguments.Length; i++)
            {
                sb.Append(", ").AppendType(arguments[i], withNamespace);
            }
            return sb.Append('>');
        }
        else return sb.AppendNamespace(type, withNamespace).Append(type.Name);
    }

    [FluentSyntax]
    private static StringBuilder AppendNamespace(this StringBuilder sb, Type type, bool withNamespace)
    {
        if (type.IsNested)
        {
            sb.AppendType(Not.Null(type.DeclaringType), withNamespace).Append('.');
        }
        else if (withNamespace && !string.IsNullOrEmpty(type.Namespace))
        {
            sb.Append(type.Namespace).Append('.');
        }
        return sb;
    }

    [Pure]
    private static string ToNonGeneric(this Type type) => type.Name.Substring(0, type.Name.IndexOf('`'));

    private static readonly Dictionary<Type, string> primitives = new()
    {
        { typeof(void), "void" },
        { typeof(object), "object" },
        { typeof(string), "string" },
        { typeof(char), "char" },
        { typeof(bool), "bool" },
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
    };
}
