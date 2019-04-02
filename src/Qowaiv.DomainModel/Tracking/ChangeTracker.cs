using Qowaiv.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Tracks (potential) changes and fires validations and notification events.</summary>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView))]
    public abstract class ChangeTracker : IEnumerable<ITrackableChange>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<ITrackableChange> _changes = new List<ITrackableChange>();

        /// <summary>If set to true, it buffer changes first before validating.</summary>
        public bool BufferChanges { get; set; }

        /// <summary>Gets the number of changes in the change tracker.</summary>
        public int Count => _changes.Count;

        /// <summary>Adds (and applies) a change to tracker.</summary>
        public void Add(ITrackableChange change)
        {
            _changes.Add(Guard.NotNull(change, nameof(change)));
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
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
