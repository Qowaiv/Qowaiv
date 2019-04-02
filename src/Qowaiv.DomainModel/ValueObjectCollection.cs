using Qowaiv.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents a read-only collection of (domain-driven design) value objects.</summary>
    /// <typeparam name="TValueObject">
    /// The type of the value object.
    /// </typeparam>
    [DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView))]
    public class ValueObjectCollection<TValueObject> : IReadOnlyList<TValueObject>
    {
        /// <summary>The underlying collection.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<TValueObject> collection = new List<TValueObject>();

        /// <summary>Gets or the element at the specified index.</summary>
        public TValueObject this[int index] => collection[index];

        /// <summary>Gets the number of value objects in the collection.</summary>
        public int Count => collection.Count;

        /// <summary>Adds a value object to the collection.</summary>
        /// <remarks>
        /// Can be accessed by <see cref="AggregateRoot{TAggrgate}.Add{TValueObject}(ValueObjectCollection{TValueObject}, TValueObject)"/>.
        /// </remarks>
        internal void Add(TValueObject item) => collection.Add(item);

        /// <summary>Removes a value object from the collection.</summary>
        /// <remarks>
        /// Can be accessed by <see cref="AggregateRoot{TAggrgate}.Remove{TValueObject}(ValueObjectCollection{TValueObject}, TValueObject)"/>.
        /// </remarks>
        internal void Remove(TValueObject item) => collection.Remove(item);

        #region IEnumerable

        /// <inheritdoc />
        public IEnumerator<TValueObject> GetEnumerator() => collection.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
