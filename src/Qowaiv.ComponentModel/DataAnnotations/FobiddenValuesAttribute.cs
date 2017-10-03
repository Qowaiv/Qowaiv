using System;

namespace Qowaiv.ComponentModel.DataAnnotations
{
	/// <summary>Validates if the decorated item has a value that is specified in the forbidden values.</summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class ForbiddenValuesAttribute : SetOfValuesAttribute
	{
		/// <summary>Creates a new instance of an <see cref="ForbiddenValuesAttribute"/>.</summary>
		public ForbiddenValuesAttribute(string value1, string value2)
			: base(new[] { value1, value2 }) { }

		/// <summary>Creates a new instance of an <see cref="ForbiddenValuesAttribute"/>.</summary>
		/// <param name="values">
		/// String representations of the forbidden values.
		/// </param>
		public ForbiddenValuesAttribute(params string[] values)
			: base(values) { }
		
		/// <summary>Gets the forbidden values.</summary>
		public string[] Forbidden { get; }

		/// <summary>Return false the value of <see cref="SetOfValuesAttribute.IsValid(object)"/>
		/// equals one of the values of the <see cref="ForbiddenValuesAttribute"/>.
		/// </summary>
		protected override bool OnEqual => false;
	}
}
