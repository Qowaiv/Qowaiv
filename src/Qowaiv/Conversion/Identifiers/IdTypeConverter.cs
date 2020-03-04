using Qowaiv.Identifiers;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Qowaiv.Conversion.Identifiers
{
    public sealed class IdTypeConverter: TypeConverter
    {
        private readonly Type IdType;
        private readonly FieldInfo m_Value;
        private readonly TypeConverter BaseConverter;

        public IdTypeConverter(Type type)
        {
            Guard.NotNull(type, nameof(type));

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Id<>) && type.GetGenericArguments().Length == 1)
            {
                IdType = type;
                m_Value = IdType.GetField(nameof(m_Value), BindingFlags.Instance | BindingFlags.NonPublic);
                BaseConverter = ((IIdentifierLogic)Activator.CreateInstance(type.GetGenericArguments()[0])).Converter;
            }
            else
            {
                throw new ArgumentException("Incompatible type", nameof(type));
            }
        }

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return BaseConverter.CanConvertFrom(context, sourceType); 
        }

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return BaseConverter.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is null)
            {
                return Activator.CreateInstance(IdType);
            }
            var id = BaseConverter.ConvertFrom(context, culture, value);
            return Activator.CreateInstance(IdType, id);
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var id = m_Value.GetValue(value);
            return base.ConvertTo(context, culture, id, destinationType);
        }
    }
}
