using Qowaiv.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace Qowaiv.ComponentModel.Validation
{
    /// <summary>Represents a property that contains at least one <see cref="ValidationAttribute"/>.</summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class AnnotatedProperty
    {
        private static readonly RequiredAttribute Optional = new NotRequiredAttributeAttribute();

        /// <summary>Creates a new instance of an <see cref="AnnotatedProperty"/>.</summary>
        private AnnotatedProperty(PropertyDescriptor descriptor, bool? typeIsAnnotatedModel, RequiredAttribute requiredAttribute, ValidationAttribute[] validationAttributes)
        {
            Descriptor = descriptor;
            DisplayAttribute = descriptor.GetDisplayAttribute() ?? new DisplayAttribute { Name = descriptor.Name };
            RequiredAttribute = requiredAttribute ?? Optional;
            ValidationAttributes = validationAttributes;
            m_TypeIsAnnotatedModel = typeIsAnnotatedModel;
            DefaultValue = descriptor.GetDefaultValue();
        }

        /// <summary>Gets the <see cref="PropertyDescriptor"/>.</summary>
        public PropertyDescriptor Descriptor { get; }

        /// <summary>Gets the <see cref="DisplayAttribute"/>.</summary>
        public DisplayAttribute DisplayAttribute { get; }

        /// <summary>Gets the <see cref="System.ComponentModel.DataAnnotations.RequiredAttribute"/>
        /// if the property was decorated with one, otherwise a <see cref="NotRequiredAttributeAttribute"/>.
        /// </summary>
        public RequiredAttribute RequiredAttribute { get; }

        /// <summary>True if the <see cref="RequiredAttribute"/> is not a <see cref="NotRequiredAttributeAttribute"/>.</summary>
        public bool IsRequired => !(RequiredAttribute is NotRequiredAttributeAttribute);


        /// <summary>Returns true, if the type itself is <see cref="AnnotatedModel"/>.</summary>
        public bool TypeIsAnnotatedModel
        {
            get
            {
                if(!m_TypeIsAnnotatedModel.HasValue)
                {
                    m_TypeIsAnnotatedModel = AnnotatedModel.Store.IsAnnotededModel(Descriptor.PropertyType, new TypePath());
                }
                return m_TypeIsAnnotatedModel.GetValueOrDefault();
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? m_TypeIsAnnotatedModel;

        /// <summary>Gets the <see cref="ValidationAttribute"/>s the property
        /// is decorated with.
        /// </summary>
        public IReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        /// <summary>Gets the default value for the property.</summary>
        public object DefaultValue { get; }

        /// <summary>Gets the value of the property for the specified model.</summary>
        public object GetValue(object model) => Descriptor.GetValue(model);

        #region Debugger experience
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string DebuggerDisplay
        {
            get => $"{Descriptor.PropertyType} {Descriptor.Name}, Attributes: {string.Join(", ", GetAll().Select(a => Shorten(a)))}";
        }
        private IEnumerable<ValidationAttribute> GetAll()
        {
            yield return RequiredAttribute;
            foreach (var attr in ValidationAttributes)
            {
                yield return attr;
            }
        }
        private static string Shorten(Attribute attr) => attr.GetType().Name.Replace("Attribute", "");

        #endregion

        /// <summary>Creates a <see cref="AnnotatedProperty"/> for all annotated properties.</summary>
        internal static IEnumerable<AnnotatedProperty> CreateAll(
            Type type,
            AnnotatedModelStore store,
            TypePath path)
        {
            var descriptors = TypeDescriptor.GetProperties(type);
            return descriptors
                .Cast<PropertyDescriptor>()
                .Select(descriptor => TryCreate(descriptor, store, path))
                .Where(property => !(property is null));
        }

        /// <summary>Tries to create a <see cref="AnnotatedProperty"/>.</summary>
        /// <returns>
        /// A <see cref="AnnotatedProperty"/> if the property is annotated,
        /// otherwise <code>null</code>.
        /// </returns>
        private static AnnotatedProperty TryCreate(
            PropertyDescriptor descriptor,
            AnnotatedModelStore store,
            TypePath path)
        {
            var type = descriptor.PropertyType;
            var validationAttributes = new List<ValidationAttribute>();
            var requiredAttribute = CollectAttributes(descriptor, validationAttributes);

            bool? typeIsAnnotated = default(bool?);

            if (requiredAttribute is null && !validationAttributes.Any())
            {
                typeIsAnnotated = store.IsAnnotededModel(type, path);

                if (!typeIsAnnotated.Value)
                {
                    return null;
                }
            }
            return new AnnotatedProperty(descriptor, typeIsAnnotated, requiredAttribute, validationAttributes.ToArray());
        }

        /// <summary>Fills the collection with all validation attributes,
        /// except the first <see cref="System.ComponentModel.DataAnnotations.RequiredAttribute"/>,
        /// as that one is returned.
        /// </summary>
        private static RequiredAttribute CollectAttributes(PropertyDescriptor descriptor, List<ValidationAttribute> validationAttributes)
        {
            var attributes = descriptor.Attributes
                .Cast<Attribute>()
                .OfType<ValidationAttribute>();

            RequiredAttribute required = null;

            foreach (var attribute in attributes)
            {
                // find a required attribute.
                if (required == null && attribute is RequiredAttribute req)
                {
                    required = req;
                }
                else
                {
                    validationAttributes.Add(attribute);
                }
            }

            return required is NotRequiredAttributeAttribute ? null : required;
        }
    }
}
