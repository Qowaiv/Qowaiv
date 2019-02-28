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
        private AnnotatedProperty(PropertyDescriptor descriptor, RequiredAttribute requiredAttribute, ValidationAttribute[] validationAttributes)
        {
            Descriptor = descriptor;
            DisplayAttribute = descriptor.GetDisplayAttribute() ?? new DisplayAttribute { Name = descriptor.Name };
            RequiredAttribute = requiredAttribute ?? Optional;
            ValidationAttributes = validationAttributes;
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

        /// <summary>Gets the <see cref="ValidationAttribute"/>s the property
        /// is decorated with.
        /// </summary>
        public IReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        /// <summary>Gets the default value for the property.</summary>
        public object DefaultValue { get; }

        /// <summary>Creates a <see cref="ValidationContext"/> for the property only.</summary>
        public ValidationContext CreateValidationContext(object model, ValidationContext validationContext)
        {
            var propertyContext = new ValidationContext(model, validationContext, validationContext.Items)
            {
                MemberName = Descriptor.Name
            };
            return propertyContext;
        }

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
        /// <param name="type">
        /// The type to get the properties for.
        /// </param>
        internal static IEnumerable<AnnotatedProperty> CreateAll(Type type)
        {
            var descriptors = TypeDescriptor.GetProperties(type);
            return descriptors.Cast<PropertyDescriptor>()
                .Select(descriptor => TryCreate(descriptor));
        }

        /// <summary>Tries to create a <see cref="AnnotatedProperty"/>.</summary>
        /// <param name="descriptor">
        /// The <see cref="PropertyDescriptor"/>/.
        /// </param>
        /// <returns>
        /// A <see cref="AnnotatedProperty"/> if the property is annotated,
        /// otherwise <code>null</code>.
        /// </returns>
        private static AnnotatedProperty TryCreate(PropertyDescriptor descriptor)
        {
            var validationAttributes = new List<ValidationAttribute>();
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
            return new AnnotatedProperty(descriptor, required, validationAttributes.ToArray());
        }
    }
}
