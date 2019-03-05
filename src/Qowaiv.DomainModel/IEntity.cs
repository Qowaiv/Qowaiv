using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.DomainModel
{
    /// <summary>Represents an (domain-driven design) entity.</summary>
    /// <typeparam name="TId">
    /// The type of the identifier.
    /// </typeparam>
    public interface IEntity<TId> : IEquatable<IEntity<TId>>, INotifyPropertyChanged, IValidatableObject
        where TId : struct
    {
        /// <summary>The identifier of the entity.</summary>
        [Key]
        TId Id { get; }
    }
}
