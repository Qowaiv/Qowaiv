using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.Validation;
using Qowaiv.Diagnostics;
using System;
using System.Diagnostics;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Tracks (potential) changes and fires validations and notification events.</summary>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView))]
    public class ChangeTracker<TModel> : ChangeTracker where TModel : class
    {
        private TModel _model;
        private AnnotatedModelValidator _validator;

        /// <summary>Creates a new instance of an <see cref="ChangeTracker"/>.</summary>
        public void Init(TModel model, AnnotatedModelValidator validator)
        {
            if (!(_model is null)) { throw new InvalidOperationException(QowaivDomainModelMessages.InvalidOperationException_ChangeTrackerNotInitialized); }
            _model = Guard.NotNull(model, nameof(model));
            _validator = validator ?? new AnnotatedModelValidator();
        }

        /// <inheritdoc />
        protected sealed override void OnAddComplete()
        {
            if (!BufferChanges)
            {
                var result = Process();
                if(!result.IsValid)
                {
                    throw InvalidModelException.For<TModel>(result.Errors);
                }
            }
        }
        
        /// <summary>Applies all changes at once.</summary>
        public Result<TModel> Process()
        {
            if (_model is null) { throw new InvalidOperationException(QowaivDomainModelMessages.InvalidOperationException_ChangeTrackerNotInitialized); }

            lock (locker)
            {
                try
                {
                    var result = ValidateAll();
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
        private Result<TModel> ValidateAll()
        {
            BufferChanges = false;

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
