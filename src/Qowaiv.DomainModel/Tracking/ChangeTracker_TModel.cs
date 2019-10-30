﻿using Qowaiv.DomainModel.Diagnostics;
using Qowaiv.Validation.Abstractions;
using System;
using System.Diagnostics;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Tracks (potential) changes and fires validations and notification events.</summary>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView))]
    public class ChangeTracker<TModel> : ChangeTracker where TModel : class
    {
        private TModel _model;
        private IValidator<TModel> _validator;

        /// <summary>Creates a new instance of an <see cref="ChangeTracker"/>.</summary>
        public void Init(TModel model, IValidator<TModel> validator)
        {
            if (!(_model is null)) { throw new InvalidOperationException(QowaivDomainModelMessages.InvalidOperationException_ChangeTrackerNotInitialized); }
            _model = Guard.NotNull(model, nameof(model));
            _validator = Guard.NotNull(validator, nameof(validator));
        }

        /// <inheritdoc />
        protected sealed override void OnAddComplete()
        {
            if (Mode == ChangeTrackerMode.None)
            {
                Process().ThrowIfInvalid();
            }
        }

        /// <summary>Applies all changes at once.</summary>
        public Result<TModel> Process()
        {
            if (_model is null)
            {
                throw new InvalidOperationException(QowaivDomainModelMessages.InvalidOperationException_ChangeTrackerNotInitialized); 
            }
            lock (locker)
            {
                try
                {
                    var result = Validate();
                    if (!result.IsValid)
                    {
                        Rollback();
                    }
                    return result;
                }
                finally
                {
                    Clear();
                }
            }
        }

        /// <summary>Validates all changed properties.</summary>
        private Result<TModel> Validate()
        {
            Mode = ChangeTrackerMode.None;

            try
            {
                return _validator.Validate(_model);
            }
            // if this fails, we want to rollback too.
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        private readonly object locker = new object();
    }
}
