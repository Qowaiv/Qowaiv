using System;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.ComponentModel.DataAnnotations
{
	/// <summary>Decorates the item as being optional.</summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class OptionalAttribute : RequiredAttribute
	{ 
		/// <summary>Returns true as an <see cref="OptionalAttribute"/> is always valid.</summary>
		public override bool IsValid(object value) => true;
	}
}
