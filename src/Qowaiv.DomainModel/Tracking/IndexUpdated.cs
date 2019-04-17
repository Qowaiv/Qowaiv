using System.Collections.Generic;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Implements <see cref="ITrackableChange"/> for adding an element to a <see cref="IList{T}{TChild}"/>.</summary>
    public class IndexUpdated<TChild> : ITrackableChange
    {
        /// <summary>Creates a new instance of a <see cref="ItemAdded{TChild}"/>.</summary>
        public IndexUpdated(IList<TChild> collection, int index, TChild org, TChild updated)
        {
            _collection = Guard.NotNull(collection, nameof(collection));
            _index = index;
            _org = org;
            _updated = updated;
        }

        private readonly IList<TChild> _collection;
        private readonly int _index;
        private readonly TChild _org;
        private readonly TChild _updated;

        /// <inheritdoc />
        public void Apply() => _collection[_index] = _updated;

        /// <inheritdoc />
        public void Rollback() => _collection[_index] = _org;
    }
}
