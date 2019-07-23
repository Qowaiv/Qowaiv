using Qowaiv.DomainModel.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Tracks (potential) changes and fires validations and notification events.</summary>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView))]
    public abstract class ChangeTracker : IEnumerable<ITrackableChange>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<ITrackableChange> _changes = new List<ITrackableChange>();

        /// <summary>Gets the mode of the change tracker.</summary>
        public ChangeTrackerMode Mode { get; internal protected set; }

        /// <summary>Sets the mode to buffering.</summary>
        public void BufferChanges() => Mode = ChangeTrackerMode.Buffering;

        /// <summary>Sets the mode to initialization.</summary>
        public void Intialize() => Mode = ChangeTrackerMode.Initialization;

        /// <summary>Gets the number of changes in the change tracker.</summary>
        public int Count => _changes.Count;

        /// <summary>Adds (and applies) a change to tracker.</summary>
        public void Add(ITrackableChange change)
        {
            Guard.NotNull(change, nameof(change));

            if (Mode != ChangeTrackerMode.Initialization)
            {
                _changes.Add(change);
            }
            change.Apply();
            OnAddComplete();
        }

        /// <summary>Action that is applied when the change has been added.</summary>
        protected abstract void OnAddComplete();

        /// <summary>Rolls back all changed properties.</summary>
        protected void Rollback()
        {
            foreach (var change in this)
            {
                change.Rollback();
            }
            _changes.Clear();
        }

        /// <summary>Removes all changes from the change tracker.</summary>
        protected void Clear() => _changes.Clear();

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
        [ExcludeFromCodeCoverage/* Just to satisfy the none-generic interface. */]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
