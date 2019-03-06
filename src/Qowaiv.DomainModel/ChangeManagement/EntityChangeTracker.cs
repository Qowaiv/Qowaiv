using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Qowaiv.DomainModel.ChangeManagement
{
    /// <summary>Tracks (potential) changes and fires validations and notification events.</summary>
    internal class EntityChangeTracker<TId> : Dictionary<string, PropertyChange>
        where TId : struct
    {
        private readonly Entity<TId> _entity;
        private readonly PropertyCollection _properties;
        private readonly AnnotatedModelValidator _validator;

        /// <summary>Creates a new instance of an <see cref="EntityChangeTracker{TId}"/>.</summary>
        public EntityChangeTracker(Entity<TId> entity, PropertyCollection properties, AnnotatedModelValidator validator)
        {
            _entity = entity;
            _properties = properties;
            _validator = validator ?? new AnnotatedModelValidator();
        }

        /// <summary>If set to true, it buffer changes first before validating.</summary>
        public bool BufferChanges { get; set; }

        /// <summary>Adds a <see cref="PropertyChange"/> to list of changed properties.</summary>
        public void AddPropertyChange(string propertyName, object value)
        {
            if (!TryGetValue(propertyName, out PropertyChange change))
            {
                change = new PropertyChange(propertyName, _properties[propertyName]);
                this[propertyName] = change;
            }

            _properties[propertyName] = value;

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
            Result validationResult = null;

            // We want to be ready for a next usage.
            var changes = Values.ToArray();
            Clear();
            BufferChanges = false;

            try
            {
                validationResult = _validator.Validate(_entity);
            }
            // if this fails, we want to rollback too.
            catch (Exception)
            {
                Rollback(changes);
                throw;
            }

            // Rollback.
            if (!validationResult.IsValid)
            {
                Rollback(changes);
                ValidationMessage.ThrowIfAnyErrors(validationResult.Messages);
            }
        }

        private void Rollback(PropertyChange[] changes)
        {
            foreach (var change in changes)
            {
                _properties[change.PropertyName] = change.Intial;
            }
        }

        /// <summary>Invoke <see cref="PropertyChangedEventHandler"/> events for
        /// all changed properties.
        /// </summary>
        private void InvokePropertiesChanged()
        {
            foreach (var change in Values)
            {
                var value = _properties[change.PropertyName];

                if (change.Intial != value)
                {
                    _entity.OnPropertyChanged(change.PropertyName);
                }
            }
        }
    }
}
