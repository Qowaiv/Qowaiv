using System.ComponentModel;

namespace Qowaiv.DomainModel.ChangeManagement
{
    /// <summary>Contains information about a property that was (potentially) changed.</summary>
    internal class PropertyChange
    {
        /// <summary>Creates a new instance of a <see cref="PropertyChange"/>.</summary>
        public PropertyChange(string propertyName, object intial)
        {
            PropertyName = propertyName;
            Intial = intial;
        }

        /// <summary>The linked entity property.</summary>
        public string PropertyName { get; }

        /// <summary>The initial value of the property.</summary>
        /// <remarks>
        /// Required to determine if an <see cref="PropertyChangedEventHandler"/>
        /// event should be raised, and to apply the validation.
        /// </remarks>
        public object Intial { get; }
    }
}
