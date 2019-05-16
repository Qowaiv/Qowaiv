using Qowaiv.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace Qowaiv.ComponentModel.Validation
{
    /// <summary>Represents a property that contains at least one <see cref="ValidationAttribute"/>.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class AnnotatedProperty
    {
        /// <summary>The underlying descriptor.</summary>
        private readonly PropertyDescriptor descriptor;

        /// <summary>Creates a new instance of an <see cref="AnnotatedProperty"/>.</summary>
        private AnnotatedProperty(PropertyDescriptor desc)
        {
            descriptor = desc;
            DisplayAttribute = desc.GetDisplayAttribute() ?? new DisplayAttribute { Name = desc.Name };
            RequiredAttribute = desc.GetRequiredAttribute() ?? NotRequiredAttributeAttribute.Optional;
            ValidationAttributes = desc.GetValidationAttributes().Except(new[] { RequiredAttribute }).ToArray();
            TypeConverter = desc.GetTypeConverter();
            DefaultValue = desc.GetDefaultValue();
            IsEnumerable = PropertyType != typeof(string)
                && PropertyType != typeof(byte[])
                && !(GetEnumerableType(PropertyType) is null);
            IsNestedModel = desc.Attributes[typeof(NestedModelAttribute)] != null;
        }

        /// <summary>Gets the type of the property.</summary>
        public Type PropertyType => descriptor.PropertyType;

        /// <summary>Gets the name of the property.</summary>
        public string Name => descriptor.Name;

        /// <summary>True if the property is read-only, otherwise false.</summary>
        public bool IsReadOnly => descriptor.IsReadOnly;

        /// <summary>True if the property is an <see cref="IEnumerable{T}"/> type, otherwise false.</summary>
        public bool IsEnumerable { get; }

        /// <summary>Gets the default value for the property.</summary>
        public object DefaultValue { get; }

        /// <summary>True if the model is decorated with the <see cref="NestedModelAttribute"/>, otherwise false.</summary>
        public bool IsNestedModel { get; }

        /// <summary>Gets the type converter associated with the property.</summary>
        /// <remarks>
        /// If not decorated, get the default type converter of the property type.
        /// </remarks>
        public TypeConverter TypeConverter { get; }

        /// <summary>Gets the display attribute.</summary>
        /// <remarks>
        /// Returns a display attribute with the name equal to the property name if not decorated.
        /// </remarks>
        public DisplayAttribute DisplayAttribute { get; }

        /// <summary>Gets the required attribute.</summary>
        /// <remarks>
        /// <see cref="NotRequiredAttributeAttribute"/> if the property is not decorated.
        /// </remarks>
        public RequiredAttribute RequiredAttribute { get; }

        /// <summary>Gets the <see cref="ValidationAttribute"/>s the property
        /// is decorated with.
        /// </summary>
        public IReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        /// <summary>Gets the value of the property for the specified model.</summary>
        public object GetValue(object model) => descriptor.GetValue(model);

        #region Debugger experience
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string DebuggerDisplay
        {
            get => $"{descriptor.PropertyType} {descriptor.Name}, Attributes: {string.Join(", ", GetAll().Select(a => Shorten(a)))}";
        }
        private IEnumerable<ValidationAttribute> GetAll()
        {
            yield return RequiredAttribute;
            foreach (var attr in ValidationAttributes)
            {
                yield return attr;
            }
        }
        private static string Shorten(Attribute attr) => attr.GetType().Name.Replace("Attribute", "");

        #endregion

        /// <summary>Creates a <see cref="AnnotatedProperty"/> for all annotated properties.</summary>
        internal static IEnumerable<AnnotatedProperty> CreateAll(Type type)
        {
            return TypeDescriptor
                .GetProperties(type)
                .Cast<PropertyDescriptor>()
                .Select(desc => new AnnotatedProperty(desc));
        }

        private static Type GetEnumerableType(Type type)
        {
            var enumType = type
                .GetInterfaces()
                .FirstOrDefault(iface =>
                    iface.IsGenericType &&
                    iface.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return enumType?.GetGenericArguments()[0];
        }
    }
}
