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
        public object Initial { get; private set; }

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
            if (GuardType(value) == _value)
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
        public void Init(object value)
        {
            Initial = GuardType(value);
            _value = value;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Annotations.DisplayAttribute.Name}, Value: {Value}{(IsDirty ? ", IsDirty" : "")}";
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
