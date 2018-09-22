using System;
using System.Collections.Generic;

namespace Qowaiv.DomainModel
{
    /// <summary>Compares entities.</summary>
    public class EntityEqualityComparer<TId> : IEqualityComparer<IEntity<TId>>
        where TId : struct
    {
        /// <summary>Returns true if both entities have the same type,
        /// are not transient, and have the same id, otherwise false.
        /// </summary>
        public bool Equals(IEntity<TId> x, IEntity<TId> y)
        {
            if (x is null || y is null)
            {
                return ReferenceEquals(x, y);
            }

            return
                x.GetType().Equals(y.GetType()) &&
                x.Id.Equals(y.Id) &&
                !x.IsTransient &&
                !y.IsTransient;
        }

        /// <summary>Gets a hash code for the entity.</summary>
        /// <exception cref="obj">
        /// If the entity is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// When an entity is transient, as the id (which is the source of the hash)
        /// is likely to change.
        /// </exception>
        public int GetHashCode(IEntity<TId> obj)
        {
            Guard.NotNull(obj, nameof(obj));

            return obj.IsTransient
                ? throw new NotSupportedException(QowaivDomainModelMessages.NotSupported_GetHashCodeOnIsTransient)
                : obj.Id.GetHashCode() ^ obj.GetType().GetHashCode();
        }
    }
}
