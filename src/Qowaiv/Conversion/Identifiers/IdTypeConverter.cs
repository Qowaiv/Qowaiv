﻿using Qowaiv.Identifiers;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Qowaiv.Conversion.Identifiers
{
    /// <summary>Provides a conversion for strongly typed identifiers.</summary>
    public sealed class IdTypeConverter : TypeConverter
    {
        /// <summary>Accessor to the underlying value.</summary>
        private readonly FieldInfo m_Value;

        /// <summary>Accessor to the private constructor.</summary>
        private readonly ConstructorInfo Ctor;

        /// <summary>The base type according to the <see cref="IIdentifierBehavior"/>.</summary>
        private readonly Type BaseType;

        /// <summary>The <see cref="TypeConverter"/> of the underlying value.</summary>
        private readonly TypeConverter BaseConverter;

        /// <summary>Creates a new instance of the <see cref="IdTypeConverter"/> class.</summary>
        /// <param name="type">
        /// The type to convert for.
        /// </param>
        public IdTypeConverter(Type type)
        {
            Guard.NotNull(type, nameof(type));

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Id<>) && type.GetGenericArguments().Length == 1)
            {
                m_Value = type.GetField(nameof(m_Value), BindingFlags.Instance | BindingFlags.NonPublic);
                var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                Ctor = ctors.FirstOrDefault(ctor => ctor.GetParameters().Length == 1);
                var behavior = ((IIdentifierBehavior)Activator.CreateInstance(type.GetGenericArguments()[0]));
                BaseType = behavior.BaseType;
                BaseConverter = behavior.Converter;
            }
            else
            {
                throw new ArgumentException("Incompatible type", nameof(type));
            }
        }

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == BaseType || BaseConverter.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == BaseType || BaseConverter.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (string.Empty.Equals(value))
            {
                value = null;
            }
            if (value is null || value.GetType() == BaseType)
            {
                return Ctor.Invoke(new[] { value });
            }
            var id = BaseConverter.ConvertFrom(context, culture, value);
            return Ctor.Invoke(new[] { id });
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var id = m_Value.GetValue(value);
            return destinationType == BaseType
                ? id
                : BaseConverter.ConvertTo(context, culture, id, destinationType);
        }
    }
}
