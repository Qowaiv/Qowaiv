using System.Collections.Generic;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Implements <see cref="ITrackableChange"/> for inserting an element at a given index of an <see cref="IList{TChild}"/>.</summary>
    public class ItemInserted<TChild> : ITrackableChange
    {
        /// <summary>Creates a new instance of a <see cref="ItemAdded{TChild}"/>.</summary>
        public ItemInserted(IList<TChild> collection, int index, TChild item)
        {
            _collection = Guard.NotNull(collection, nameof(collection));
            _index = index;
            _item = item;
        }

        private readonly IList<TChild> _collection;
        private readonly int _index;
        private readonly TChild _item;

        /// <inheritdoc />
        public void Apply() => _collection.Insert(_index, _item);

        /// <inheritdoc />
        public void Rollback() => _collection.RemoveAt(_index);
    }
}
