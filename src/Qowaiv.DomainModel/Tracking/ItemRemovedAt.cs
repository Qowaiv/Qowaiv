using System.Collections.Generic;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Implements <see cref="ITrackableChange"/> for adding an element to a <see cref="IList{T}{TChild}"/>.</summary>
    public class ItemRemovedAt<TChild> : ITrackableChange
    {
        /// <summary>Creates a new instance of a <see cref="ItemAdded{TChild}"/>.</summary>
        public ItemRemovedAt(IList<TChild> collection, int index, TChild item)
        {
            _collection = Guard.NotNull(collection, nameof(collection));
            _index = index;
            _item = item;
        }

        private readonly IList<TChild> _collection;
        private readonly int _index;
        private readonly TChild _item;

        /// <inheritdoc />
        public void Apply() => _collection.RemoveAt(_index);

        /// <inheritdoc />
        public void Rollback() => _collection.Insert(_index, _item);
    }
}
