using Qowaiv.Json;
using System;
using System.Linq;

namespace Qowaiv.Reflection
{
    /// <summary>Helper class for some operations on <see cref="Type"/>.</summary>
    public static class QowaivType
    {
        /// <summary>Returns true if the type is a Single Value Object, otherwise false.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static bool IsSingleValueObject(Type objectType)
        {
            Guard.NotNull(objectType, "objectType");
            return GetSingleValueObjectAttribute(objectType) != null;
        }

        /// <summary>Gets the <see cref="SingleValueObjectAttribute"/> of the type, if any.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static SingleValueObjectAttribute GetSingleValueObjectAttribute(Type objectType)
        {
            Guard.NotNull(objectType, "objectType");
            return (SingleValueObjectAttribute)objectType.GetCustomAttributes(typeof(SingleValueObjectAttribute), false).FirstOrDefault();
        }

        /// <summary>Returns true if the object is null-able, otherwise false.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static bool IsNullable(Type objectType)
        {
            Guard.NotNull(objectType, "objectType");
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>Returns true if the object type is an <see cref="IJsonSerializable"/>.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static bool IsIJsonSerializable(Type objectType)
        {
            Guard.NotNull(objectType, "objectType");
            return objectType.GetInterfaces().Any(iface => iface == typeof(IJsonSerializable));
        }

        /// <summary>Returns true if the object type is a null-able <see cref="IJsonSerializable"/>.</summary>
        /// <param name="objectType">
        /// The type to test for.
        /// </param>
        public static bool IsNullableIJsonSerializable(Type objectType)
        {
            Guard.NotNull(objectType, "objectType");
            return IsNullable(objectType) && IsIJsonSerializable(objectType.GetGenericArguments()[0]);
        }
    }
}
