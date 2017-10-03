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
		private static readonly RequiredAttribute Optional = new OptionalAttribute();

		/// <summary>Creates a new instance of an <see cref="AnnotatedProperty"/>.</summary>
		private AnnotatedProperty(PropertyDescriptor descriptor, RequiredAttribute requiredAttribute, ValidationAttribute[] validationAttributes)
		{
			Descriptor = descriptor;
			RequiredAttribute = requiredAttribute ?? Optional;
			ValidationAttributes = validationAttributes;
		}

		/// <summary>Gets the <see cref="PropertyDescriptor"/>.</summary>
		public PropertyDescriptor Descriptor { get; }

		/// <summary>Gets the <see cref="System.ComponentModel.DataAnnotations.RequiredAttribute"/>
		/// if the property was decorated with one, otherwise a <see cref="OptionalAttribute"/>.
		/// </summary>
		public RequiredAttribute RequiredAttribute { get; }

		/// <summary>Gets the <see cref="ValidationAttribute"/>s the property
		/// is decorated with.
		/// </summary>
		public IReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

		/// <summary>Creates a <see cref="ValidationContext"/> for the property only.</summary>
		public ValidationContext CreateValidationContext(object model, ValidationContext validationContext)
		{
			var propertyContext = new ValidationContext(model, validationContext, validationContext.Items);
			propertyContext.MemberName = Descriptor.Name;
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
				.Select(descriptor => TryCreate(descriptor))
				.Where(descriptor => descriptor != null);
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
				if(required == null && attribute is RequiredAttribute req)
				{
					required = req;
				}
				else
				{
					validationAttributes.Add(attribute);
				}
			}
			if ((required != null && !(required is OptionalAttribute)) || validationAttributes.Count != 0)
			{
				return new AnnotatedProperty(descriptor, required, validationAttributes.ToArray());
			}
			else
			{
				return null;
			}
		}
	}
}
