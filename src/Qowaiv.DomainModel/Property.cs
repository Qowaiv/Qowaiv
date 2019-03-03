using Qowaiv.ComponentModel.Validation;
using Qowaiv.DomainModel.ChangeManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents a editable property of an entity.</summary>
    public class Property
    {
        /// <summary>Creates a new instance of a property.</summary>
        internal Property(AnnotatedProperty annotations, object entity)
        {
            Annotations = annotations;
            _value = annotations.DefaultValue;
            _context = new ValidationContext(entity)
            {
                MemberName = Annotations.Descriptor.Name
            };
        }

        /// <summary>Gets the annotations of the property.</summary>
        public AnnotatedProperty Annotations { get; }

        /// <summary>Gets the value of the property.</summary>
        /// <remarks>
        /// Set is internally used to help the <see cref="EntityChangeTracker{TId}"/> to do its job.
        /// </remarks>
        public virtual object Value 
        {
            get => _value;
            internal set => _value = GuardType(value);
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object _value;
     
        /// <summary>Gets the type of the property.</summary>
        public Type PropertyType => Annotations.Descriptor.PropertyType;

        /// <summary>Validates the property based on the value it tries to set.</summary>
        internal IEnumerable<ValidationException> Validate(object value)
        {
            // required is done separately.
            var required = Annotations.RequiredAttribute.GetValidationResult(value, _context);
            if (required != ValidationResult.Success)
            {
                yield return new ValidationException(required, Annotations.RequiredAttribute, value);
            }
            else
            {
                // other validation attributes.
                foreach (var attr in Annotations.ValidationAttributes)
                {
                    var validationResult = attr.GetValidationResult(value, _context);
                    if (validationResult != ValidationResult.Success)
                    {
                        yield return new ValidationException(validationResult, attr, value);
                    }
                }
            }
        }

        internal List<CalculatedProperty> TriggersProperties { get; } = new List<CalculatedProperty>();

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0}, Value: {1}", Annotations.DisplayAttribute.Name, Value);
        }

        /// <summary>Guards values to be of the right type.</summary>
        private object GuardType(object value)
        {
            if (!(value is null) && !PropertyType.IsInstanceOfType(value))
            {
                throw new ArgumentException(string.Format(QowaivMessages.ArgumentException_Must, PropertyType), nameof(value));
            }
            return value;
        }

        internal readonly ValidationContext _context;
    }
}
