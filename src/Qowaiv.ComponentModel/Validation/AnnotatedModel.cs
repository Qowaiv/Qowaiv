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

        /// <summary>Returns true if the model is validatable, otherwise false.</summary>
        public bool IsValidatable
        {
            get => IsIValidatableObject || TypeAttributes.Any() || Properties.Any();
        }

        /// <summary>Gets an <see cref="AnnotatedModel"/>.</summary>
        /// <param name="type">
        /// Type to create the annotated model for.
        /// </param>
        /// <remarks>
        /// This one uses caching.
        /// </remarks>
        public static AnnotatedModel Get(Type type) => Store.GetAnnotededModel(type);

        /// <summary>Creates an <see cref="AnnotatedModel"/>.</summary>
        internal static AnnotatedModel Create(Type type, AnnotatedModelStore store, TypePath path)
        {
            Guard.NotNull(type, nameof(type));

            return new AnnotatedModel(
                type,
                type.GetInterfaces().Contains(typeof(IValidatableObject)),
                TypeDescriptor
                    .GetAttributes(type)
                    .Cast<Attribute>()
                    .OfType<ValidationAttribute>().ToArray(),
                    AnnotatedProperty.CreateAll(type, store, path).ToArray()
                );
        }

        /// <summary>Creates an <see cref="AnnotatedModel"/> for a not annotated model.</summary>
        /// <param name="type">
        /// Type to create the not annotated model for.
        /// </param>
        /// <remarks>
        /// Useful in caching scenarios.
        /// </remarks>
        internal static AnnotatedModel None(Type type)
        {
            return new AnnotatedModel(type, false, new ValidationAttribute[0], new AnnotatedProperty[0]);
        }
    }
}
