using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Validation;
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
        public object Value => _value;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object _value;
     
        /// <summary>Gets the type of the property.</summary>
        public Type PropertyType => Annotations.Descriptor.PropertyType;

        /// <summary>Sets the value of the property.</summary>
        /// <exception cref="ValidationException">
        /// If an single error occurs.
        /// </exception>
        /// <exception cref="AggregateException">
        /// If multiple errors occur.
        /// </exception>
        /// <returns>
        /// True if the value have been changed, false if the value equals the original value.
        /// </returns>
        /// <remarks>
        /// Only calls validate if the value has changed, or the property is required.
        /// </remarks>
        public bool Set(object value)
        {
            var hasChanged = GuardType(value) != _value;
            if (hasChanged || Annotations.IsRequired)
            {
                ValidationMessage.ThrowIfAnyErrors(Validate(value));
                _value = value;
            }
            return hasChanged;
        }

        /// <summary>Sets the value of the property without validating it.</summary>
        /// <remarks>
        /// Used to help the <see cref="PropertyChangeTracker{TId}"/> to do its job.
        /// </remarks>
        internal void SetOnly(object value)=> _value = value;

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

        private readonly ValidationContext _context;
    }
}
