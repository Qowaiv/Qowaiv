using System;

namespace Qowaiv.DomainModel.Events
{
    /// <summary>The minimum contract of an event.</summary>
    public interface IEvent
    {
        /// <summary>The linked identifier of the aggregate root.</summary>
        Guid Id { get; set; }
        
        /// <summary>The version of the event.</summary>
        int Version { get; set; }
    }
}
