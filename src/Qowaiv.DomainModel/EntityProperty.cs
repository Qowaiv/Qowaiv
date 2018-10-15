using Qowaiv.ComponentModel.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents a editable property of an entity.</summary>
    public class EntityProperty
    {
        /// <summary>Creates a new instance of a property.</summary>
        internal EntityProperty(AnnotatedProperty annotations)
        {
            Annotations = annotations;
            _value = annotations.DefaultValue;
        }

        /// <summary>Gets the annotations of the property.</summary>
        public AnnotatedProperty Annotations { get; }

        /// <summary>Returns true if the property is dirty (has unsaved changes).</summary>
        public bool IsDirty { get; internal set; }

        /// <summary>Gets the value of the property.</summary>
        public object Value
        {
            get => _value;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object _value;

        /// <summary>Gets the type of the property.</summary>
        public Type PropertyType => Annotations.Descriptor.PropertyType;

        /// <summary>Sets the value of the property.</summary>
        internal bool SetValue<TId>(object value, IEntity<TId> entity) where TId : struct
        {
            if (_value == value)
            {
                return false;
            }

            var context = new ValidationContext(entity)
            {
                MemberName = Annotations.Descriptor.Name
            };

            // required is done separately.
            var required = Annotations.RequiredAttribute.GetValidationResult(value, context);
            if (required != ValidationResult.Success)
            {
                throw new ValidationException(required, Annotations.RequiredAttribute, value);
            }

            // other validation attributes.
            foreach (var attr in Annotations.ValidationAttributes)
            {
                var validationResult = attr.GetValidationResult(value, context);
                if (validationResult != ValidationResult.Success)
                {
                    throw new ValidationException(validationResult, attr, value);
                }
            }

            IsDirty = true;
            _value = value;
            return true;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Annotations.DisplayAttribute.Name}, Value: {Value}{(IsDirty ? ", IsDirty" : "")}";
        }
    }
}
