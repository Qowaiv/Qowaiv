﻿using System.Collections.Generic;
using System.Linq;

namespace Qowaiv.DomainModel.Tracking
{
    /// <summary>Implements <see cref="ITrackableChange"/> for removing all elements from a <see cref="ICollection{TChild}"/>.</summary>
    public class ClearedCollection<TChild> : ITrackableChange
    {
        /// <summary>Creates a new instance of a <see cref="ClearedCollection{TChild}"/>.</summary>
        public ClearedCollection(ICollection<TChild> collection)
        {
            _collection = Guard.NotNull(collection, nameof(collection));
            _items = _collection.ToArray();
        }

        private readonly ICollection<TChild> _collection;
        private readonly TChild[] _items;

        /// <inheritdoc />
        public void Apply() => _collection.Clear();

        /// <inheritdoc />
        public void Rollback()
        {
            foreach(var item in _items)
            {
                _collection.Add(item);
            }
        }
    }
}
