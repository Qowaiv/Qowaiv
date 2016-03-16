using System;
using System.Diagnostics.CodeAnalysis;

namespace Qowaiv
{
	/// <summary>Describes a single value object as intended at the Domain Driven Design context.</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public sealed class SingleValueObjectAttribute : Attribute
	{
		/// <summary>Initializes a new instance of a single value object attribute.</summary>
		/// <remarks>
		/// The parameterless constructor is marked private so it can not be used or tested.
		/// </remarks>
		[ExcludeFromCodeCoverage]
		private SingleValueObjectAttribute() { }

		/// <summary>Initializes a new instance of a single value object attribute.</summary>
		/// <param name="staticOptions">
		/// The available static options of the single value object.
		/// </param>
		/// <param name="underlyingType">
		/// The underlying type of the single value object.
		/// </param>
		public SingleValueObjectAttribute(SingleValueStaticOptions staticOptions, Type underlyingType)
		{
			this.StaticOptions = staticOptions;
			this.UnderlyingType = underlyingType;
		}

		/// <summary>The available static options of the single value object.</summary>
		public SingleValueStaticOptions StaticOptions { get; private set; }

		/// <summary>The underlying type of the single value object.</summary>
		public Type UnderlyingType { get; private set; }

		/// <summary>Gets and set the database type.</summary>
		/// <remarks>
		/// Use this if the database type is different from the underlying type.
		/// </remarks>
		public Type DatabaseType { get; set; }
	}
}
