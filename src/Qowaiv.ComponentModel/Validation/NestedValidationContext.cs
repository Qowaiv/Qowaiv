using Qowaiv.ComponentModel.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel.Validation
{
    /// <summary>Represents a nested wrapper for a (sealed) <see cref="ValidationContext"/>.</summary>
    internal class NestedValidationContext : IServiceProvider
    {
        /// <summary>Constructor.</summary>
        private NestedValidationContext(string path, object instance, IServiceProvider serviceProvider, IDictionary<object, object> items)
        {
            Path = path;
            Instance = instance;
            ServiceProvider = serviceProvider;
            Items = items;
            Messages = new List<ValidationResult>();
            Annotations = AnnotatedModel.Get(instance.GetType());
        }

        /// <summary>Gets the (nested) path.</summary>
        public string Path { get; }
        
        /// <summary>Gets the instance/model.</summary>
        public object Instance { get; }

        /// <summary>Gets the (root) service provider.</summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>Gets the dictionary of key/value pairs that is associated with this context.</summary>
        public IDictionary<object, object> Items { get; }

        /// <summary>Gets the member name.</summary>
        /// <remarks>
        /// Only relevant for testing a property.
        /// </remarks>
        public string MemberName { get; private set; }

        /// <summary>Gets the annotated model for this context.</summary>
        public AnnotatedModel Annotations { get; }

        /// <summary>Gets the enumerable of collected messages.</summary>
        public IEnumerable<ValidationResult> Messages { get; private set; }

        /// <summary>Adds a set of messages.</summary>
        public void AddMessages(IEnumerable<ValidationResult> messages)
        {
            foreach(var message in messages)
            {
                AddMessage(message);
            }
        }

        /// <summary>Adds a message.</summary>
        /// <returns>
        /// True if the message had a severity.
        /// </returns>
        /// <remarks>
        /// Null and <see cref="ValidationMessage.None"/> Messages are not added.
        /// </remarks>
        public bool AddMessage(ValidationResult message)
        {
            var serverity = message.GetSeverity();
            if(serverity == ValidationSeverity.None)
            {
                return false;
            }

            var nested = message;

            // Update if the path could/should be updated.
            if(!string.IsNullOrEmpty(Path) && message.MemberNames.Any())
            {
                var memberNames = message.MemberNames.Select(name => Path + name).ToArray();
                nested = ValidationMessage.For(serverity, message.ErrorMessage, memberNames);
            }
            ((List<ValidationResult>)Messages).Add(nested);
            return true;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType) => ServiceProvider?.GetService(serviceType);

        /// <summary>Creates context for the property.</summary>
        public NestedValidationContext ForProperty(AnnotatedProperty property)
        {
            return new NestedValidationContext(Path, Instance, ServiceProvider, Items)
            {
                MemberName = property.Descriptor.Name,
                Messages = Messages,
            };
        }

        /// <summary>Creates a nested context for the property context.</summary>
        /// <param name="value">
        /// The value of the property.
        /// </param>
        /// <param name="index">
        /// The optional index in case of an enumeration.
        /// </param>
        public NestedValidationContext Nested(object value, int? index = null)
        {
            var path = string.IsNullOrEmpty(Path)
                ? MemberName
                : Path + '.' + MemberName;

            if(index.HasValue)
            {
                path += '[' + index.Value.ToString() + ']';
            }

            return new NestedValidationContext(path + '.', value, ServiceProvider, Items)
            {
                Messages = Messages,
            };
        }

        /// <summary>Implicitly casts to the (sealed base) <see cref="ValidationContext"/>.</summary>
        public static implicit operator ValidationContext(NestedValidationContext context)
        {
            return context is null
                ? null
                : new ValidationContext(context.Instance, context.ServiceProvider, context.Items)
                {
                    MemberName = context.MemberName,
                };
        }

        /// <summary>Creates a root context.</summary>
        public static NestedValidationContext CreateRoot(object instance, IServiceProvider serviceProvider, IDictionary<object, object> items)
        {
            Guard.NotNull(instance, nameof(instance));
            return new NestedValidationContext(string.Empty, instance, serviceProvider, items);
        }
    }
}
