using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) entity.</summary>
    /// <typeparam name="TEntity">
    /// The type of the actual entity.
    /// </typeparam>
    /// <typeparam name="TId">
    /// The type of the identifier.
    /// </typeparam>
    public interface IEntity<TEntity, TId> : IEquatable<TEntity>
        where TEntity : class, IEntity<TEntity, TId>
        where TId : struct
    {
        /// <summary>The identifier of the entity.</summary>
        [Key]
        TId Id { get; }
    }
}
