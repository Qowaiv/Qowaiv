using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Qowaiv.Reflection
{
    /// <summary>Helper class for some operations on <see cref="Type"/>.</summary>
    public static class QowaivType
    {
        /// <summary>Returns true if the value is null or equal to the default value.</summary>
        public static bool IsNullOrDefaultValue(object value)
            => value is null || value.Equals(Activator.CreateInstance(value.GetType()));

        /// <summary>Returns true if the object is null-able, otherwise false.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static bool IsNullable(Type objectType)
            => Nullable.GetUnderlyingType(objectType) != null;

        /// <summary>Returns true if the object type is a number.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static bool IsNumeric(Type objectType)
        {
            var code = Type.GetTypeCode(GetNotNullableType(objectType));
            return code >= TypeCode.SByte && code <= TypeCode.Decimal;
        }

        /// <summary>Returns true if the object type is a date (of any kind).</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        /// <remarks>
        /// Tests on the types:
        /// * <see cref="DateTime"/>
        /// * <see cref="DateTimeOffset"/>
        /// * <see cref="LocalDateTime"/>
        /// * <see cref="Date"/>
        /// * <see cref="WeekDate"/>
        /// </remarks>
        public static bool IsDate(Type objectType)
        {
            var type = GetNotNullableType(objectType);
            return type == typeof(DateTime)
                || type == typeof(DateTimeOffset) 
                || type == typeof(LocalDateTime)
                || type == typeof(Date)
                || type == typeof(WeekDate);
        }

        /// <summary>Gets the not null-able type if it is a null-able, otherwise the provided type.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static Type GetNotNullableType(Type objectType)
            => Nullable.GetUnderlyingType(objectType) ?? objectType;

        /// <summary>Gets a C# formatted <see cref="string"/> representing the <see cref="Type"/>.</summary>
        /// <param name="type">
        /// The type to format as C# string.
        /// </param>
        public static string ToCSharpString(this Type type) => type.ToCSharpString(false);

        /// <summary>Gets a C# formatted <see cref="string"/> representing the <see cref="Type"/>.</summary>
        /// <param name="type">
        /// The type to format as C# string.
        /// </param>
        /// <param name="withNamespace">
        /// Should the namespace be displayed or not.
        /// </param>
        public static string ToCSharpString(this Type type, bool withNamespace)
        {
            Guard.NotNull(type, nameof(type));
            return new StringBuilder().AppendType(type, withNamespace).ToString();
        }

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
                    type = type.GetElementType();
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
            else
            {
                return sb.AppendNamespace(type, withNamespace).Append(type.Name);
            }
        }

        private static StringBuilder AppendNamespace(this StringBuilder sb, Type type, bool withNamespace)
        {
            if (type.IsNested)
            {
                sb.AppendType(type.DeclaringType, withNamespace).Append('.');
            }
            else if (withNamespace && !string.IsNullOrEmpty(type.Namespace))
            {
                sb.Append(type.Namespace).Append('.');
            }
            return sb;
        }

        private static string ToNonGeneric(this Type type) => type.Name.Substring(0, type.Name.IndexOf('`'));

        private static readonly Dictionary<Type, string> primitives = new Dictionary<Type, string>
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


        /// <summary>Returns true if the type is a Single Value Object, otherwise false.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        [Obsolete("Will be dropped when the next major version is released.")]
        public static bool IsSingleValueObject(Type objectType)
            => GetSingleValueObjectAttribute(objectType) != null;

        /// <summary>Gets the <see cref="SingleValueObjectAttribute"/> of the type, if any.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        [Obsolete("Use Type.GetCustomAttribute<SingleValueObjectAttribute>(). Will be dropped when the next major version is released.")]
        public static SingleValueObjectAttribute GetSingleValueObjectAttribute(Type objectType)
            => Guard.NotNull(objectType, nameof(objectType))
            .GetCustomAttribute<SingleValueObjectAttribute>();
    }
}
