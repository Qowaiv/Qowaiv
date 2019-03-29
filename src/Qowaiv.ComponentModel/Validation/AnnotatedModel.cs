using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Qowaiv.ComponentModel.Validation
{
    /// <summary>Represents an annotated model.</summary>
    /// <remarks>
    /// An annotated model should contains at least one <see cref="ValidationAttribute"/>
    /// or implement <see cref="IValidatableObject"/>.
    /// </remarks>
    public class AnnotatedModel
    {
        internal static readonly AnnotatedModel None = new AnnotatedModel(typeof(object), false, new ValidationAttribute[0], new AnnotatedProperty[0]);

        /// <summary>Gets the singleton instance of the <see cref="AnnotatedModelStore"/>.</summary>
        internal static readonly AnnotatedModelStore Store = new AnnotatedModelStore();

        /// <summary>Creates a new instance of an <see cref="AnnotatedModel"/>.</summary>
        private AnnotatedModel(
            Type type,
            bool isIValidatableObject,
            ValidationAttribute[] typeAttributes,
            AnnotatedProperty[] annotatedProperties)
        {
            ModelType = type;
            IsIValidatableObject = isIValidatableObject;
            TypeAttributes = typeAttributes;
            Properties = annotatedProperties;
        }

        /// <summary>Gets the <see cref="Type"/> of the model.</summary>
        public Type ModelType { get; }

        /// <summary>Gets the <see cref="ValidationAttribute"/>s of the model.</summary>
        public IReadOnlyCollection<ValidationAttribute> TypeAttributes { get; }

        /// <summary>Gets the annotated properties of the model.</summary>
        public IReadOnlyCollection<AnnotatedProperty> Properties { get; }

        /// <summary>Returns true if the model implements <see cref="IValidatableObject"/>,
        /// otherwise false.
        /// </summary>
        public bool IsIValidatableObject { get; }

        /// <summary>Gets an <see cref="AnnotatedModel"/>.</summary>
        /// <param name="type">
        /// Type to create the annotated model for.
        /// </param>
        /// <remarks>
        /// This one uses caching.
        /// </remarks>
        public static AnnotatedModel Get(Type type) => Store.GetAnnotededModel(type);

        /// <summary>Creates an <see cref="AnnotatedModel"/>.</summary>
        internal static AnnotatedModel Create(Type type)
        {
            Guard.NotNull(type, nameof(type));

            var isIValidatable = type.GetInterfaces().Contains(typeof(IValidatableObject));
            var validations = TypeDescriptor.GetAttributes(type).Cast<Attribute>().OfType<ValidationAttribute>().ToArray();
            var properties = AnnotatedProperty.CreateAll(type).ToArray();

            return isIValidatable || validations.Any() || properties.Any()
                ? new AnnotatedModel(type, isIValidatable, validations, properties)
                : None;
        }
    }
}
