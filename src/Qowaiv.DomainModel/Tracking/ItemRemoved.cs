namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Implements <see cref="ITrackableChange"/> for removing an item from a <see cref="ValueObjectCollection{TValueObject}"/>.</summary>
    public class ItemRemoved<TValueObject> : ITrackableChange
    {
        /// <summary>Creates a new instance of a <see cref="ItemRemoved{TValueObject}"/>.</summary>
        public ItemRemoved(ValueObjectCollection<TValueObject> collection, TValueObject item)
        {
            _collection = Guard.NotNull(collection, nameof(collection));
            _item = item;
        }

        private readonly ValueObjectCollection<TValueObject> _collection;
        private readonly TValueObject _item;

        /// <inheritdoc />
        public void Apply() => _collection.Remove(_item);

        /// <inheritdoc />
        public void Rollback() => _collection.Add(_item);
    }
}
