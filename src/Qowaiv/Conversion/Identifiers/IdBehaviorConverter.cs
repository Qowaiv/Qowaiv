using Qowaiv.Identifiers;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Qowaiv.Conversion.Identifiers
{
    /// <summary>
    /// Converter that uses <see cref="IIdentifierBehavior.TryCreate(object, out object)"/>
    /// to convert the underlying value from any object from a supported type.
    /// </summary>
    public sealed class IdBehaviorConverter : TypeConverter
    {
        private readonly IIdentifierBehavior Behavior;
        private readonly Type[] SupportedTypes;

        /// <summary>Creates a new instance of the <see cref="IdBehaviorConverter"/> class.</summary>
        public IdBehaviorConverter(IIdentifierBehavior behavior, params Type[] supportedTypes)
        {
            Behavior = Guard.NotNull(behavior, nameof(behavior));
            SupportedTypes= Guard.HasAny(supportedTypes,nameof(supportedTypes));
        }

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => SupportedTypes.Contains(sourceType) || base.CanConvertFrom(context, sourceType);

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            => Behavior.TryCreate(value, out var result)
            ? result : base.ConvertFrom(context, culture, value);
    }
}
