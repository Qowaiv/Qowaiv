using Qowaiv.Validation.DataAnnotations.Reflection;
using System.Reflection;

namespace System;

internal static class TypeExtensions
{
    extension(Type type)
    {
        [Pure]
        public IEnumerable<Member> GetPublicInstanceMembers() =>
        [
            .. type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(Member.New),
            .. type.GetFields(BindingFlags.Instance | BindingFlags.Public).Select(Member.New),
        ];

        [Pure]
        public Type? GetEnumerableType() => type
            .GetInterfaces()
            .FirstOrDefault(iface =>
                iface.IsGenericType &&
                iface.GetGenericTypeDefinition() == typeof(IEnumerable<>))?
            .GetGenericArguments()[0];

        /// <summary>True if the type implements <see cref="IValidatableObject"/>.</summary>
        [Pure]
        public bool ImplementsIValidatableObject() => typeof(IValidatableObject).IsAssignableFrom(type);

        /// <summary>Returns true for all value types except <see cref="Nullable{T}" />.</summary>
        [Pure]
        public bool IsNonNullableValueType()
            => type.IsValueType
            && (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Nullable<>));

        /// <summary>Gets the decorated <see cref="ValidationAttribute"/>'s for the property.</summary>
        [Pure]
        public IEnumerable<ValidationAttribute> ValidationAttributes()
            => type.GetCustomAttributes<ValidationAttribute>(inherit: true);
    }
}
