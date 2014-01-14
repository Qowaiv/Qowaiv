using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qowaiv.CodeGenerator.CodeDom.CSharp
{
    /// <summary>Extensions on System.Type.</summary>
    public static class TypeCSharpExtensions
    {
        private readonly static Dictionary<Type, string> CSharpAliasTypes = new Dictionary<Type, string>()
        {
            { typeof(object), "object" },
            { typeof(char), "char" },
            { typeof(string), "string" },
            { typeof(bool), "bool" },
            
            { typeof(byte), "byte" },
            { typeof(short), "short" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            
            { typeof(sbyte), "sbyte" },
            { typeof(ushort), "ushort" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            
            { typeof(decimal), "decimal" },

            { typeof(float), "float" },
            { typeof(double), "double" },
        };

        /// <summary>Gets the C# name for the type.</summary>
        /// <param name="tp">
        /// The type.
        /// </param>
        /// <param name="options">
        /// The naming options.
        /// </param>
        public static string GetCSharpName(this Type tp, CSharpNameOptions options = CSharpNameOptions.AliasForPrimitives)
        {
            var sb = new StringBuilder();
            sb.AppendCSharpName(tp, options);
            return sb.ToString();
        }

        private static StringBuilder AppendCSharpName(this StringBuilder sb, Type tp, CSharpNameOptions options)
        {
            if (tp.IsArray)
            {
                return sb
                    .AppendCSharpName(tp.GetElementType(), options)
                    .Append('[')
                    .Append(new string(',', tp.GetArrayRank() - 1))
                    .Append(']');
            }

            if (tp.IsGenericType)
            {
                if (tp.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return sb
                        .AppendCSharpName(tp.GetGenericArguments().First(), options)
                        .Append('?');
                }

                var genericArguments = tp.GetGenericArguments();

                var genericTypeDefenition = options.HasFlag(CSharpNameOptions.UseFullName) ?
                    tp.GetGenericTypeDefinition().FullName :
                    tp.GetGenericTypeDefinition().Name;

                sb.Append(genericTypeDefenition, 0, genericTypeDefenition.IndexOf('`'))
                    .Append('<')
                    .AppendCSharpName(genericArguments.First(), options);

                for (int i = 1; i < genericArguments.Length; i++)
                {
                    sb.Append(", ").AppendCSharpName(genericArguments[i], options);
                }
                return sb.Append('>');
            }
            if (options.HasFlag(CSharpNameOptions.AliasForPrimitives) && CSharpAliasTypes.ContainsKey(tp))
            {
                return
                    sb.Append(CSharpAliasTypes[tp]);
            }
            return
                sb.Append(options.HasFlag(CSharpNameOptions.UseFullName) ? tp.FullName : tp.Name);
        }
    }
}
