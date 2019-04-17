using Qowaiv.Diagnostics;
using Qowaiv.DomainModel.Tracking;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents child collection of (domain-driven design) for an aggregate (both entities and value objects).</summary>
    /// <remarks>
    /// 
    /// </remarks>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView))]
    public class ChildCollection<TChild> : IList<TChild>
    {
        /// <summary>The underlying collection.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<TChild> collection = new List<TChild>();

        private readonly ChangeTracker tracker;

        /// <summary>Creates a new instance of <see cref="ChildCollection{TChild}"/>.</summary>
        /// <param name="tracker"></param>
        public ChildCollection(ChangeTracker tracker) => this.tracker = Guard.NotNull(tracker, nameof(tracker));

        /// <summary>Gets or the element at the specified index.</summary>
        public TChild this[int index]
        {
            get => collection[index];
            set
            {
                var org = collection[index];
                tracker.Add(new IndexUpdated<TChild>(collection, index, org, value));
                collection[index] = value;
            }
        }

        /// <summary>Gets the number of elements in the collection.</summary>
        public int Count => collection.Count;

        /// <summary>The child collection is not read-only.</summary>
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public int IndexOf(TChild item) => collection.IndexOf(item);
        
        /// <inheritdoc />
        public bool Contains(TChild item) => collection.Contains(item);
        
        /// <inheritdoc />
        public void CopyTo(TChild[] array, int arrayIndex) => collection.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public void Add(TChild item) => tracker.Add(new ItemAdded<TChild>(collection, item));

        /// <inheritdoc />
        public void Insert(int index, TChild item) => tracker.Add(new ItemInserted<TChild>(collection, index, item));

        /// <inheritdoc />
        public bool Remove(TChild item)
        {
            var count = Count;
            tracker.Add(new ItemRemoved<TChild>(collection, item));
            return count != Count;
        }

        /// <inheritdoc />
        public void RemoveAt(int index) => tracker.Add(new ItemRemovedAt<TChild>(collection, index, this[index]));

        /// <inheritdoc />
        public void Clear() => tracker.Add(new ClearedCollection<TChild>(collection));

        #region IEnumerable

        /// <inheritdoc />
        public IEnumerator<TChild> GetEnumerator() => collection.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
