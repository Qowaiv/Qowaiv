using System.Collections.Generic;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Implements <see cref="ITrackableChange"/> for adding an element to an <see cref="ICollection{TChild}"/>.</summary>
    public class ItemAdded<TChild> : ITrackableChange
    {
        /// <summary>Creates a new instance of a <see cref="ItemAdded{TChild}"/>.</summary>
        public ItemAdded(ICollection<TChild> collection, TChild item)
        {
            _collection = Guard.NotNull(collection, nameof(collection));
            _item = item;
        }

        private readonly ICollection<TChild> _collection;
        private readonly TChild _item;

        /// <inheritdoc />
        public void Apply() => _collection.Add(_item);

        /// <inheritdoc />
        public void Rollback() => _collection.Remove(_item);
    }
}
