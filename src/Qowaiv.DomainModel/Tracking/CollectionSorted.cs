using System.Collections.Generic;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Implements <see cref="ITrackableChange"/> 'sorting the elements of a <see cref="List{TChild}"/>.</summary>
    public class CollectionSorted<TChild> : ITrackableChange
    {
        /// <summary>Creates a new instance of a <see cref="ItemAdded{TChild}"/>.</summary>
        public CollectionSorted(List<TChild> collection, IComparer<TChild> comparer)
        {
            _collection = Guard.NotNull(collection, nameof(collection));
            _comparer = Guard.NotNull(comparer, nameof(comparer));
        }

        private readonly List<TChild> _collection;
        private readonly IComparer<TChild> _comparer;
        private TChild[] _original;


        /// <inheritdoc />
        public void Apply()
        {
            _original = _collection.ToArray();
            _collection.Sort(_comparer);
        }

        /// <inheritdoc />
        public void Rollback()
        {
            _collection.Clear();
            _collection.AddRange(_original);
        }
    }
}
