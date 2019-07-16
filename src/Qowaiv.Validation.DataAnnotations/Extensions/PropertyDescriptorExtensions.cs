using Qowaiv;
using Qowaiv.Validation.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace System.ComponentModel
{
    /// <summary>Extensions on <see cref="PropertyDescriptor"/>.</summary>
    public static class PropertyDescriptorExtensions
    {
        /// <summary>Gets the decorated <see cref="RequiredAttribute"/> for the property.</summary>
        public static RequiredAttribute GetRequiredAttribute(this PropertyDescriptor descriptor)
        {
            Guard.NotNull(descriptor, nameof(descriptor));

            return descriptor.Attributes
                .Cast<Attribute>()
                .OfType<RequiredAttribute>()
                .FirstOrDefault(attr => !(attr is NotRequiredAttributeAttribute));
        }

        /// <summary>Gets the decorated <see cref="RequiredAttribute"/> for the property.</summary>
        public static IEnumerable<ValidationAttribute> GetValidationAttributes(this PropertyDescriptor descriptor)
        {
            Guard.NotNull(descriptor, nameof(descriptor));

            return descriptor.Attributes
                .Cast<Attribute>()
                .OfType<ValidationAttribute>();
        }

        /// <summary>Gets the decorated <see cref="DisplayAttribute"/> for the property.</summary>
        public static DisplayAttribute GetDisplayAttribute(this PropertyDescriptor descriptor)
        {
            Guard.NotNull(descriptor, nameof(descriptor));

            return (DisplayAttribute)descriptor.Attributes[typeof(DisplayAttribute)];
        }

        /// <summary>Gets the type converter associated with the property.</summary>
        public static TypeConverter GetTypeConverter(this PropertyDescriptor descriptor)
        {
            var converterType = descriptor.Attributes
                .Cast<Attribute>()
                .OfType<TypeConverterAttribute>()
                .Select(att => Type.GetType(att.ConverterTypeName))
                .FirstOrDefault();

            return converterType is null
                ? TypeDescriptor.GetConverter(descriptor.PropertyType)
                : (TypeConverter)Activator.CreateInstance(converterType);
        }
    }
}
