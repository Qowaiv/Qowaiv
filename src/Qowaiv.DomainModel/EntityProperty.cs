using Qowaiv.ComponentModel.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Qowaiv.DomainModel
{
    public class EntityProperty
    {
        internal EntityProperty(AnnotatedProperty annotations)
        {
            Annotations = annotations;
            _value = annotations.DefaultValue;
        }

        public AnnotatedProperty Annotations { get; }

        public bool IsDirty { get; internal set; }

        public object Value
        {
            get => _value;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object _value;

        internal bool SetValue<TId>(object value, IEntity<TId> entity) where TId : struct
        {
            if (_value == value)
            {
                return false;
            }

            var context = new ValidationContext(entity)
            {
                DisplayName = Annotations.DisplayAttribute.Name
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
            return true;
        }
    }
}
