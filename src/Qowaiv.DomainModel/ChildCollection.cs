using Qowaiv.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents a read-only collection of (domain-driven design) child objects (both entities and value objects).</summary>
    /// <remarks>
    /// The contract is read-only, but the aggregate root can manipulate its
    /// children, so also child collections:
    /// <see cref="AggregateRoot{TAggrgate}.Add{TChild}(ChildCollection{TChild}, TChild)"/>
    /// <see cref="AggregateRoot{TAggrgate}.AddRange{TChild}(ChildCollection{TChild}, IEnumerable{TChild})"/>
    /// <see cref="AggregateRoot{TAggrgate}.Remove{TChild}(ChildCollection{TChild}, TChild)"/>
    /// <see cref="AggregateRoot{TAggrgate}.Clear{TChild}(ChildCollection{TChild})"/>
    /// </remarks>
    /// <typeparam name="TChild">
    /// The type of the elements in the child collection.
    /// </typeparam>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView))]
    public class ChildCollection<TChild> : IReadOnlyList<TChild>
    {
        /// <summary>The underlying collection.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<TChild> collection = new List<TChild>();

        /// <summary>Gets or the element at the specified index.</summary>
        public TChild this[int index] => collection[index];

        /// <summary>Gets the number of elements in the collection.</summary>
        public int Count => collection.Count;

        /// <summary>Adds an element the collection.</summary>
        /// <remarks>
        /// Can be accessed by <see cref="AggregateRoot{TAggrgate}.Add{TChild}(ChildCollection{TChild}, TChild)"/>.
        /// </remarks>
        internal void Add(TChild item) => collection.Add(item);

        /// <summary>Removes an element from the collection.</summary>
        /// <remarks>
        /// Can be accessed by <see cref="AggregateRoot{TAggrgate}.Remove{TChild}(ChildCollection{TChild}, TChild)"/>.
        /// </remarks>
        internal void Remove(TChild item) => collection.Remove(item);

        /// <summary>Removes all elements from the collection.</summary>
        /// <remarks>
        /// Can be accessed by <see cref="AggregateRoot{TAggrgate}.Clear{TChild}(ChildCollection{TChild})"/>.
        /// </remarks>
        internal void Clear() => collection.Clear();

        #region IEnumerable

        /// <inheritdoc />
        public IEnumerator<TChild> GetEnumerator() => collection.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
