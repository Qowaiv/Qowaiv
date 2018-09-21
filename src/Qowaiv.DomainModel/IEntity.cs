using System;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) entity.</summary>
    /// <typeparam name="TId">
    /// The type of the identifier.
    /// </typeparam>
    public interface IEntity<TId> : IEquatable<IEntity<TId>>
        where TId : struct
    {
        /// <summary>The identifier of the entity.</summary>
        TId Id { get; }

        /// <summary>Indicates that the entity not yet has a set identifier.</summary>
        bool IsTransient();
    }
}
