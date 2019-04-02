namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Implements <see cref="ITrackableChange"/> for adding an item to a <see cref="ValueObjectCollection{TValueObject}"/>.</summary>
    public class ItemAdded<TValueObject> : ITrackableChange
    {
        /// <summary>Creates a new instance of a <see cref="ItemAdded{TValueObject}"/>.</summary>
        public ItemAdded(ValueObjectCollection<TValueObject> collection, TValueObject item)
        {
            _collection = Guard.NotNull(collection, nameof(collection));
            _item = item;
        }

        private readonly ValueObjectCollection<TValueObject> _collection;
        private readonly TValueObject _item;

        /// <inheritdoc />
        public void Apply() => _collection.Add(_item);

        /// <inheritdoc />
        public void Rollback() => _collection.Remove(_item);
    }
}
