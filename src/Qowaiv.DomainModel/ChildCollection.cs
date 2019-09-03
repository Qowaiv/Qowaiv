using Qowaiv.DomainModel.Diagnostics;
using Qowaiv.DomainModel.Tracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents child collection of (domain-driven design) for an aggregate (both entities and value objects).</summary>
    /// <remarks>
    /// A child collection can not contain null elements.
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
                GuardNull(value, nameof(value));
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
        public void Add(TChild item)
        {
            GuardNull(item, nameof(item));
            tracker.Add(new ItemAdded<TChild>(collection, item));
        }

        /// <summary>Adds a range of items to the child collection.</summary>
        /// <param name="items">
        /// Items to add.
        /// </param>
        public void AddRange(IEnumerable<TChild> items)
        {
            Guard.NotNull(items, nameof(items));

            var index = 0;

            foreach(var item in items)
            {
                GuardNull(item, nameof(items) + "[" + index++.ToString() + "]");
                tracker.Add(new ItemAdded<TChild>(collection, item));
            }
        }

        /// <inheritdoc />
        public void Insert(int index, TChild item)
        {
            GuardNull(item, nameof(item));
            tracker.Add(new ItemInserted<TChild>(collection, index, item));
        }

        /// <inheritdoc />
        public bool Remove(TChild item)
        {
            var index = IndexOf(item);
            if (index == -1)
            {
                return false;
            }
            tracker.Add(new ItemRemovedAt<TChild>(collection, index, item));
            return true;
        }

        /// <inheritdoc />
        public void RemoveAt(int index) => tracker.Add(new ItemRemovedAt<TChild>(collection, index, this[index]));

        /// <summary>Sorts the elements in the child collection.</summary>
        public void Sort() => Sort(Comparer<TChild>.Default);

        /// <summary>Sorts the elements in the child collection.</summary>
        public void Sort(IComparer<TChild> comparer)
        {
            Guard.NotNull(comparer, nameof(comparer));

            tracker.Add(new CollectionSorted<TChild>(collection, comparer));
        }

        /// <inheritdoc />
        public void Clear() => tracker.Add(new ClearedCollection<TChild>(collection));

        #region IEnumerable

        /// <inheritdoc />
        public IEnumerator<TChild> GetEnumerator() => collection.GetEnumerator();

        /// <inheritdoc />
        [ExcludeFromCodeCoverage/* Just to satisfy the none-generic interface. */]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        /// <remarks>We don't want to support null elements in child collections.</remarks>
        private static void GuardNull(TChild item, string paramName)
        {
#pragma warning disable IDE0041 // Use 'is null' check
            // False positive: TChild is not (guaranteed) a reference type, so item is null is rejected by the compiler.
            if (ReferenceEquals(item, null)) { throw new ArgumentNullException(paramName); }
#pragma warning restore IDE0041 // Use 'is null' check
        }
    }
}
