using Qowaiv.ComponentModel.Messages;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.DomainModel
{
    /// <summary>Tracks (potential) property changes that are done in a batch.</summary>
    internal class PropertyChangeTracker<TId> : Dictionary<Property, PropertyChange>
        where TId : struct
    {
        private readonly Entity<TId> _entity;
        private readonly PropertyChangedEventHandler _propertyChanged;

        public PropertyChangeTracker(Entity<TId> entity)
        {
            _entity = entity;
        }

        public PropertyChangeTracker(Entity<TId> entity, PropertyChangedEventHandler propertyChanged)
            : this(entity)
        {
            _propertyChanged = propertyChanged;
        }

        /// <summary>Adds a <see cref="PropertyChange"/> to list of changed properties.</summary>
        public void AddChange(Property property, object value)
        {
            if (!TryGetValue(property, out PropertyChange change))
            {
                change = new PropertyChange(property);
                this[property] = change;
            }
        }

        /// <summary>Applies all changes at once.</summary>
        public void ApplyChanges()
        {
            // Unlink from parent.
            _entity._tracker = null;
            ValidateAll();
            InvokePropertiesChanged();
        }


        /// <summary>Validates all changed properties.</summary>
        private void ValidateAll()
        {
            var errors = new List<ValidationException>();
            foreach (var change in Values)
            {
                var prop = change.Property;
                var value = prop.Value;
                prop.SetOnly(change.Intial);
                errors.AddRange(prop.Validate(value));
            }

            // Rollback.
            if(errors.Any(e => e.ValidationResult is null || e.ValidationResult.GetSeverity() == ValidationSeverity.Error))
            {
                foreach (var change in Values)
                {
                    var prop = change.Property;
                    prop.SetOnly(change.Intial);
                }
                ValidationMessage.ThrowIfAnyErrors(errors);
            }
        }

        /// <summary>Invoke <see cref="PropertyChangedEventHandler"/> events for
        /// all changed properties.
        /// </summary>
        private void InvokePropertiesChanged()
        {
            if (_propertyChanged is null)
            {
                return;
            }

            foreach (var change in Values)
            {
                var prop = change.Property;
                var value = prop.Value;

                if (change.Intial != value)
                {
                    _propertyChanged.Invoke(_entity, new PropertyChangedEventArgs(prop.PropertyType.Name));
                }
            }
        }

    }
}
