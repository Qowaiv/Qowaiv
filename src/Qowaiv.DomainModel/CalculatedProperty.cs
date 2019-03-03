using Qowaiv.ComponentModel.Validation;
using System;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents a calculated property of an entity.</summary>
    public class CalculatedProperty : Property
    {
        internal CalculatedProperty(AnnotatedProperty annotations, object entity)
            : base(annotations, entity) { }

        /// <inheritdoc />
        public override object Value
        {
            get => Annotations.Descriptor.GetValue(_context.ObjectInstance);
            internal set => throw new NotSupportedException("Setting the value is not supported for calculated properties");
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString() + " (calculated)";
        }
    }
}
