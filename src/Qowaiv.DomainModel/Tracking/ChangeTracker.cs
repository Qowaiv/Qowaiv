using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.Messages;
using Qowaiv.ComponentModel.Validation;
using Qowaiv.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Tracks (potential) changes and fires validations and notification events.</summary>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView))]
    public class ChangeTracker<TModel> : IEnumerable<ITrackableChange> where TModel : class
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<ITrackableChange> _changes = new List<ITrackableChange>();
        private readonly TModel _model;
        private readonly AnnotatedModelValidator _validator;

        /// <summary>Creates a new instance of an <see cref="ChangeTracker"/>.</summary>
        public ChangeTracker(TModel model, AnnotatedModelValidator validator)
        {
            _model = Guard.NotNull(model, nameof(model));
            _validator = validator ?? new AnnotatedModelValidator();
        }

        /// <summary>If set to true, it buffer changes first before validating.</summary>
        public bool BufferChanges { get; set; }

        /// <summary>Gets the number of changes in the change tracker.</summary>
        public int Count => _changes.Count;

        /// <summary>Adds a change to tracker.</summary>
        public void Add(ITrackableChange change)
        {
            Guard.NotNull(change, nameof(change));
            _changes.Add(change);
            change.Apply();

            if (!BufferChanges)
            {
                var result = Process();
                ValidationMessage.ThrowIfAnyErrors(result.Messages);
            }
        }

        /// <summary>Applies all changes at once.</summary>
        public Result<TModel> Process()
        {
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
                    _changes.Clear();
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

        /// <summary>Rolls back all changed properties.</summary>
        private void Rollback()
        {
            foreach (var change in this)
            {
                change.Rollback();
            }
            _changes.Clear();
        }

        private readonly object locker = new object();

        #region IEnumerable

        private IEnumerable<ITrackableChange> LoopReversed()
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                yield return _changes[i];
            }
        }

        /// <inheritdoc />
        public IEnumerator<ITrackableChange> GetEnumerator() => LoopReversed().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
