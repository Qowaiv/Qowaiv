using Qowaiv.ComponentModel.Validation;
using System;
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

        /// <summary>Returns true if the property is dirty (has unsaved changes).</summary>
        public bool IsDirty => !Equals(Value, Initial);

        /// <summary>Gets the value of the property.</summary>
        public object Value
        {
            get => _value;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object _value;

        /// <summary>Gets the initial value.</summary>
        public object Initial { get; protected set; }

        /// <summary>Gets the type of the property.</summary>
        public Type PropertyType => Annotations.Descriptor.PropertyType;

        /// <summary>Sets the value of the property.</summary>
        /// <exception cref="ValidationException">
        /// When the value is not allowed according to the validation attributes.
        /// </exception>
        /// <returns>
        /// True if the value have been changed, false if the value equals the original value.
        /// </returns>
        public bool Set(object value)
        {
            if (_value == value)
            {
                return false;
            }

            // required is done separately.
            var required = Annotations.RequiredAttribute.GetValidationResult(value, _context);
            if (required != ValidationResult.Success)
            {
                throw new ValidationException(required, Annotations.RequiredAttribute, value);
            }

            // other validation attributes.
            foreach (var attr in Annotations.ValidationAttributes)
            {
                var validationResult = attr.GetValidationResult(value, _context);
                if (validationResult != ValidationResult.Success)
                {
                    throw new ValidationException(validationResult, attr, value);
                }
            }
            _value = value;
            return true;
        }

        /// <summary>Loads the value of the property.</summary>
        /// <remarks>
        /// This implies that property is not (longer) dirty after loading,
        /// and will not trigger any validation.
        /// </remarks>
        public virtual void Init(object value)
        {
            Initial = value;
            _value = value;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Annotations.DisplayAttribute.Name}, Value: {Value}{(IsDirty ? ", IsDirty" : "")}";
        }

        private readonly ValidationContext _context;
    }
}
