using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Qowaiv.DomainModel.Events
{
    /// <summary>A basic implementation of an <see cref="IEvent"/>.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public abstract class EventBase : IEvent
    {
        /// <inheritdoc />
        public virtual Guid Id { get; set; }

        /// <inheritdoc />
        public virtual int Version { get; set; }

        /// <summary>Represents the event as a DEBUG <see cref="string"/>.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual string DebuggerDisplay
        {
            get
            {
                var props = GetType()
                 .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                 .Where(p => p.Name != nameof(Id) && p.Name != nameof(Version))
                 .ToArray();

                // Get rid of Event suffix.
                var eventName = GetType().Name;
                if (eventName.EndsWith("Event")) { eventName = eventName.Substring(eventName.Length - 5); }

                var sb = new StringBuilder()
                    .AppendFormat("v{0} ", Version)
                    .AppendFormat(eventName)
                    .AppendFormat(", Props[{0}] {{ ", props.Length);

                for(var i = 0; i < props.Length; i++)
                {
                    var prop = props[i];
                    if(i != 0)
                    {
                        sb.Append(", ");
                    }
                    sb.AppendFormat("{0}: {1}", prop.Name, prop.GetValue(this));

                }

                sb.Append(" }, Id: ").Append(Id.ToString().ToUpperInvariant());
                return sb.ToString();
            }
        }
    }
}
