namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Implements <see cref="ITrackableChange"/> for removing an element from a <see cref="ChildCollection{TChild}"/>.</summary>
    public class ItemRemoved<TChild> : ITrackableChange
    {
        /// <summary>Creates a new instance of a <see cref="ItemRemoved{TChild}"/>.</summary>
        public ItemRemoved(ChildCollection<TChild> collection, TChild item)
        {
            _collection = Guard.NotNull(collection, nameof(collection));
            _item = item;
        }

        private readonly ChildCollection<TChild> _collection;
        private readonly TChild _item;

        /// <inheritdoc />
        public void Apply() => _collection.Remove(_item);

        /// <inheritdoc />
        public void Rollback() => _collection.Add(_item);
    }
}
