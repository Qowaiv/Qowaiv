using System;

namespace Qowaiv.ComponentModel.DataAnnotations
{
	/// <summary>Validates if the decorated item has a value that is specified in the allowed values.</summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class AllowedValuesAttribute : SetOfValuesAttribute
	{
		/// <summary>Creates a new instance of an <see cref="AllowedValuesAttribute"/>.</summary>
		public AllowedValuesAttribute(string value1, string value2)
			: base(new[] { value1, value2 }) { }

		/// <summary>Creates a new instance of an <see cref="AllowedValuesAttribute"/>.</summary>
		/// <param name="values">
		/// String representations of the allowed values.
		/// </param>
		public AllowedValuesAttribute(params string[] values)
			: base(values) { }

		/// <summary>Return true the value of <see cref="SetOfValuesAttribute.IsValid(object)"/>
		/// equals one of the values of the <see cref="AllowedValuesAttribute"/>.
		/// </summary>
		protected override bool OnEqual => true;
	}
}
