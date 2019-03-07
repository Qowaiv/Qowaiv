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
    internal class EntityChangeTracker<TId> : Dictionary<string, object>
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

        /// <summary>Adds the changed  property with its initial value to the changed properties.</summary>
        public void AddPropertyChange(string propertyName, object value)
        {
            if (!ContainsKey(propertyName))
            {
                var initial = _properties[propertyName];
                this[propertyName] = initial;
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
            lock (locker)
            {
                try
                {
                    var result = ValidateAll();
                    if (result.IsValid)
                    {
                        InvokePropertiesChanged();
                    }
                    else
                    {
                        Rollback();
                        ValidationMessage.ThrowIfAnyErrors(result.Messages);
                    }
                }
                finally
                {
                    Clear();
                }
            }
        }

        /// <summary>Validates all changed properties.</summary>
        private Result ValidateAll()
        {
            BufferChanges = false;

            try
            {
                return _validator.Validate(_entity);
            }
            // if this fails, we want to rollback too.
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        /// <summary>Rolls back all changed properties.</summary>
        private void Rollback()
        {
            foreach (var change in this)
            {
                _properties[change.Key] = change.Value;
            }
        }

        /// <summary>Invoke <see cref="PropertyChangedEventHandler"/> events for
        /// all changed properties.
        /// </summary>
        private void InvokePropertiesChanged()
        {
            foreach (var change in this)
            {
                var value = _properties[change.Key];

                if (change.Value != value)
                {
                    _entity.OnPropertyChanged(change.Key);
                }
            }
        }

        private readonly object locker = new object();
    }
}
