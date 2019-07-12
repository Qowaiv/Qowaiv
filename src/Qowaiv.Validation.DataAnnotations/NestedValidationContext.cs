using Qowaiv.Validation.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.Validation.DataAnnotations
{
    /// <summary>Represents a nested wrapper for a (sealed) <see cref="ValidationContext"/>.</summary>
    internal class NestedValidationContext : IServiceProvider
    {
        /// <summary>Constructor.</summary>
        private NestedValidationContext(string path, object instance, IServiceProvider serviceProvider, IDictionary<object, object> items, ISet<object> done)
        {
            Path = path;
            Instance = instance;
            ServiceProvider = serviceProvider;
            Items = items;
            Annotations = AnnotatedModel.Get(instance.GetType());
            Done = done;
        }

        /// <summary>Keeps track of objects that already have been validated.</summary>
        public ISet<object> Done { get; }

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
        public IReadOnlyCollection<IValidationMessage> Messages { get; private set; } = new List<IValidationMessage>();

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
        public bool AddMessage(ValidationResult validationResult)
        {
            var message = ValidationMessage.For(validationResult);

            if (message.Severity <= ValidationSeverity.None)
            {
                return false;
            }

            // Update if the path could/should be updated.
            if (!string.IsNullOrEmpty(Path) && validationResult.MemberNames.Any())
            {
                message = new ValidationMessage(
                    message.Severity,
                    message.Message,
                    message.MemberNames.Select(name => Path + name).ToArray());
            }
            ((List<IValidationMessage>)Messages).Add(message);
            return true;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType) => ServiceProvider?.GetService(serviceType);

        /// <summary>Creates context for the property.</summary>
        public NestedValidationContext ForProperty(AnnotatedProperty property)
        {
            return new NestedValidationContext(Path, Instance, ServiceProvider, Items, Done)
            {
                MemberName = property.Name,
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

            return new NestedValidationContext(path + '.', value, ServiceProvider, Items, Done)
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
            return new NestedValidationContext(string.Empty, instance, serviceProvider, items, new HashSet<object>(ReferenceComparer.Instance));
        }
    }
}
