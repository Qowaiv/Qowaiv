using System.Collections.Generic;

namespace Qowaiv.DomainModel.Persistence
{
    /// <summary>Represents the delta of a model.</summary>
    public class Delta
    {
        /// <summary>The changed properties with their new values.</summary>
        public Dictionary<string, object> New { get; } = new Dictionary<string, object>();
        
        /// <summary>The changed properties with their old values.</summary>
        public Dictionary<string, object> Old { get; } = new Dictionary<string, object>();
    }
}
