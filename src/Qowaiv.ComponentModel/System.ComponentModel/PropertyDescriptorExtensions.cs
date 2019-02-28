﻿using Qowaiv;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace System.ComponentModel
{
    /// <summary>Extensions on <see cref="PropertyDescriptor"/>.</summary>
    public static class PropertyDescriptorExtensions
    {
        /// <summary>Gets the default value for the property (type).</summary>
        /// <remarks>
        /// Uses both the <see cref="DefaultValueAttribute"/> as the
        /// <see cref="TypeConverterAttribute"/> if available/applicable.
        /// </remarks>
        public static object GetDefaultValue(this PropertyDescriptor descriptor)
        {
            Guard.NotNull(descriptor, nameof(descriptor));

            var defaultValueAttribute = descriptor.Attributes
                .Cast<Attribute>()
                .OfType<DefaultValueAttribute>()
                .FirstOrDefault();

            if (defaultValueAttribute is null)
            {
                return descriptor.PropertyType.IsValueType
                    ? Activator.CreateInstance(descriptor.PropertyType)
                    : null;
            }

            return GetConverter(descriptor)
                .ConvertFrom(defaultValueAttribute.Value);
        }

        /// <summary>Gets the decorated <see cref="DisplayAttribute"/> for the property.</summary>
        public static DisplayAttribute GetDisplayAttribute(this PropertyDescriptor descriptor)
        {
            Guard.NotNull(descriptor, nameof(descriptor));

            return (DisplayAttribute)descriptor.Attributes[typeof(DisplayAttribute)];
        }

        private static TypeConverter GetConverter(PropertyDescriptor descriptor)
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
