using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.DomainModel
{
    /// <summary>Tracks (potential) changes and fires validations and notification events.</summary>
    internal class EntityChangeTracker<TId> : Dictionary<Property, PropertyChange>
        where TId : struct
    {
        private readonly Entity<TId> _entity;
        private readonly IReadOnlyCollection<ValidationAttribute> _typeAttributes;
        private readonly ValidationContext _validationContext;

        /// <summary>Creates a new instance of an <see cref="EntityChangeTracker{TId}"/>.</summary>
        public EntityChangeTracker(Entity<TId> entity)
        {
            _entity = entity;
           _typeAttributes = AnnotatedModelStore.Instance.GetAnnotededModel(entity.GetType()).TypeAttributes;
            _validationContext = new ValidationContext(entity)
            {
                DisplayName = _entity.GetType().Name
            };

        }

        /// <summary>If set to true, it buffer changes first before validating.</summary>
        public bool BufferChanges { get; set; }

        /// <summary>Adds a <see cref="PropertyChange"/> to list of changed properties.</summary>
        public void AddPropertyChange(Property property, object value)
        {
            if (!TryGetValue(property, out PropertyChange change))
            {
                change = new PropertyChange(property);
                this[property] = change;
            }

            property.SetValue(value);

            if (!BufferChanges)
            {
                ProcessChanges();
            }
        }

        /// <summary>Applies all changes at once.</summary>
        public void ProcessChanges()
        {
            ValidateAll();
            InvokePropertiesChanged();
        }

        /// <summary>Validates all changed properties.</summary>
        private void ValidateAll()
        {
            var errors = new List<ValidationException>();

            // We want to be ready for a next usage.
            var changes = Values.ToArray();
            Clear();
            BufferChanges = false;

            try
            {
                ValidateModelBasedValidationAttributes(errors);
                ValidatePropertyChanges(changes, errors);
            }
            // if this fails, we want to rollback too.
            catch (Exception)
            {
                Rollback(changes);
                throw;
            }

            // Rollback.
            if (errors.Any(e => e.ValidationResult is null || e.ValidationResult.GetSeverity() == ValidationSeverity.Error))
            {
                Rollback(changes);
                ValidationMessage.ThrowIfAnyErrors(errors);
            }
        }

        private static void ValidatePropertyChanges(PropertyChange[] changes, List<ValidationException> errors)
        {
            foreach (var change in changes)
            {
                var prop = change.Property;
                var value = prop.Value;
                prop.SetValue(change.Intial);
                errors.AddRange(prop.Validate(value));
            }
        }

        private void ValidateModelBasedValidationAttributes(List<ValidationException> errors)
        {
            foreach (var attr in _typeAttributes)
            {
                var result = attr.GetValidationResult(_entity, _validationContext);
                if (result.GetSeverity() != ValidationSeverity.None)
                {
                    errors.Add(new ValidationException(result, attr, _entity));
                }
            }
        }

        private static void Rollback(PropertyChange[] changes)
        {
            foreach (var change in changes)
            {
                var prop = change.Property;
                prop.SetValue(change.Intial);
            }
        }

        /// <summary>Invoke <see cref="PropertyChangedEventHandler"/> events for
        /// all changed properties.
        /// </summary>
        private void InvokePropertiesChanged()
        {
            foreach (var change in Values)
            {
                var prop = change.Property;
                var value = prop.Value;

                if (change.Intial != value)
                {
                    _entity.OnPropertyChanged(prop);
                }
            }
        }
    }
}
