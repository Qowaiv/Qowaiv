using System.ComponentModel;

namespace Qowaiv.DomainModel
{
    /// <summary>Contains information about a property that was (potentially) changed.</summary>
    internal class PropertyChange
    {
        /// <summary>Creates a new instance of a <see cref="PropertyChange"/>.</summary>
        public PropertyChange(Property property)
        {
            Property = property;
            Intial = property.Value;
        }

        /// <summary>The linked entity property.</summary>
        public Property Property { get; }

        /// <summary>The initial value of the property.</summary>
        /// <remarks>
        /// Required to determine if an <see cref="PropertyChangedEventHandler"/>
        /// event should be raised, and to apply the validation.
        /// </remarks>
        public object Intial { get; }
    }
}
