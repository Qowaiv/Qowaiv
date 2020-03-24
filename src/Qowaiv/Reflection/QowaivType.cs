using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qowaiv.Reflection
{
    /// <summary>Helper class for some operations on <see cref="Type"/>.</summary>
    public static class QowaivType
    {
        /// <summary>Returns true if the value is null or equal to the default value.</summary>
        public static bool IsNullOrDefaultValue(object value)
        {
            return value is null || value.Equals(Activator.CreateInstance(value.GetType()));
        }

        /// <summary>Returns true if the type is a Single Value Object, otherwise false.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static bool IsSingleValueObject(Type objectType)
        {
            Guard.NotNull(objectType, nameof(objectType));
            return GetSingleValueObjectAttribute(objectType) != null;
        }

        /// <summary>Gets the <see cref="SingleValueObjectAttribute"/> of the type, if any.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static SingleValueObjectAttribute GetSingleValueObjectAttribute(Type objectType)
        {
            Guard.NotNull(objectType, nameof(objectType));
            return (SingleValueObjectAttribute)objectType.GetCustomAttributes(typeof(SingleValueObjectAttribute), false).FirstOrDefault();
        }

        /// <summary>Returns true if the object is null-able, otherwise false.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static bool IsNullable(Type objectType)
        {
            Guard.NotNull(objectType, nameof(objectType));
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

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

            return
                type == typeof(DateTime) ||
                type == typeof(DateTimeOffset) ||
                type == typeof(LocalDateTime) ||
                type == typeof(Date) ||
                type == typeof(WeekDate);
        }

        /// <summary>Gets the not null-able type if it is a null-able, otherwise the provided type.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static Type GetNotNullableType(Type objectType)
        {
            Guard.NotNull(objectType, nameof(objectType));
            if (IsNullable(objectType))
            {
                return objectType.GetGenericArguments()[0];
            }
            return objectType;
        }
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
            if (type.IsArray)
            {
                return sb.AppendType(type.GetElementType(), withNamespace)
                    .Append('[')
                    .Append(',', type.GetArrayRank() - 1)
                    .Append(']');

            }

            if (primitives.TryGetValue(type, out var primitive))
            {
                return sb.Append(primitive);
            }

            if (type.IsGenericTypeDefinition)
            {
                return sb
                    .AppendNamespace(type, withNamespace)
                    .Append(type.ToNonGeneric())
                    .Append('<')
                    .Append(new string(',', type.GetGenericArguments().Length - 1))
                    .Append('>');
            }

            if (type.IsGenericType)
            {
                var arguments = type.GetGenericArguments();

                // special case for nullables.
                if (arguments.Length == 1 && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return sb.AppendType(arguments[0], withNamespace).Append('?');
                }

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

            return sb.AppendNamespace(type, withNamespace).Append(type.Name);
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
            { typeof(object), "object" },
            { typeof(string), "string" },
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
}
