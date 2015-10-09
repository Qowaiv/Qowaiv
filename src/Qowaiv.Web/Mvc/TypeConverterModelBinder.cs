using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Qowaiv.Web.Mvc
{
	/// <summary>A model binder for types with a custom type converter.</summary>
	/// <remarks>
	/// This binder supports models that have there own TypeConverter.
	/// 
	/// The message of the exception thrown by there type converter in case of
	/// failure is the error message added to the model state.
	/// 
	/// This binder is needed because the default model binder don't call
	/// a type converter in case the input is String.Empty.
	/// </remarks>
	public class TypeConverterModelBinder : DefaultModelBinder
	{
		/// <summary>A Dictionary that keeps track of the registered type converters.</summary>
		private static ConcurrentDictionary<Type, TypeConverter> TypeConverters = new ConcurrentDictionary<Type, TypeConverter>();

		/// <summary>Static constructor.</summary>
		/// <remarks>
		/// Add all types of Qowaiv that are supported by the model binder.
		/// </remarks>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
			Justification = "To complex for straight forward assignment.")]
		static TypeConverterModelBinder()
		{
			var qowaivAssembly = typeof(Qowaiv.SingleValueObjectAttribute).Assembly;
			var qowaivWebAssembly = typeof(Qowaiv.Web.InternetMediaType).Assembly;

			// Add types.
			AddAssembly(qowaivAssembly);
			AddAssembly(qowaivWebAssembly);
		}

		/// <summary>Adds the types of the assembly that are marked with teh SingleValueObjectAttribute.</summary>
		/// <param name="assembly">
		/// Assembly to add.
		/// </param>
		public static void AddAssembly(Assembly assembly)
		{
			Guard.NotNull(assembly, "assembly");
			var tps = assembly.GetTypes()
				.Where(tp => tp.GetCustomAttributes(typeof(SingleValueObjectAttribute), false).Any())
				.ToArray();
			AddTypes(tps);
		}

		/// <summary>Adds the specified types.</summary>
		/// <param name="tps">
		/// Types to add.
		/// </param>
		/// <remarks>
		/// Only adds the types that are supported by the model binder.
		/// </remarks>
		public static void AddTypes(params Type[] tps)
		{
			Guard.NotNull(tps, "tps");
			foreach (var tp in tps)
			{
				AddType(tp);
			}
		}

		/// <summary>Adds the specified type.</summary>
		/// <param name="tp">
		/// Type to add.
		/// </param>
		/// <remarks>
		/// Only adds the types that are supported by the model binder.
		/// </remarks>
		public static void AddType(Type tp)
		{
			// Don't add types twice.
			if (!TypeConverters.ContainsKey(tp))
			{
				var converter = TypeDescriptor.GetConverter(tp);
				// Not the default type converter or the enumerator converter.
				if (converter.GetType() != typeof(TypeConverter) &&
					converter.GetType() != typeof(EnumConverter) &&
					converter.CanConvertFrom(typeof(String)))
				{
					TypeConverters[tp] = converter;
				}
			}
		}

		/// <summary>Removes the specified type.</summary>
		/// <param name="tp">
		/// Type to remove.
		/// </param>
		public static void RemoveType(Type tp)
		{
			TypeConverter converter;
			TypeConverters.TryRemove(tp, out converter);
		}

		/// <summary>Gets the types that where added to the model binder.</summary>
		public static IEnumerable<Type> Types { get { return TypeConverters.Keys; } }

		/// <summary>Registers the model binder to the specified model binder dictionary.</summary>
		/// <param name="binders">The model binder dictionary to add to.</param>
		/// <remarks>
		/// Typical usage:
		/// <example>
		/// protected override void Application_Start()
		/// {
		///     base.Application_Start();
		///     TypeConverterModelBinder.RegisterForAll(ModelBinders.Binders);
		/// }
		/// </example>
		/// </remarks>
		public static void RegisterForAll(ModelBinderDictionary binders)
		{
			Guard.NotNull(binders, "binders");
			foreach (var tp in TypeConverters.Keys)
			{
				binders.Add(tp, TypeConverterModelBinder.Instance);
			}
		}

		/// <summary>The singleton instance of the model binder.</summary>
		public static TypeConverterModelBinder Instance { get { return s_Instance; } }
		private static TypeConverterModelBinder s_Instance = new TypeConverterModelBinder();

		/// <summary>Constructor.</summary>
		/// <remarks>
		/// Force singleton usage.
		/// </remarks>
		protected TypeConverterModelBinder() { }

		/// <summary>Binds the model by using the type converter of the type to bind.</summary>
		/// <param name="controllerContext">Controller context.</param>
		/// <param name="bindingContext">The binding context.</param>
		/// <returns>The bound model.</returns>
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			Guard.NotNull(bindingContext, "bindingContext");

			// Get the value result.
			var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (valueResult == null) { return null; }

			string value = null;
			var valueAsArray = valueResult.RawValue as Array;
			if (valueAsArray != null && valueAsArray.Length > 0)
			{
				// provided value is an array; take first element (if available)
				value = valueAsArray.GetValue(0) as string;
			}
			else
			{
				value = valueResult.RawValue as string;
			}

			// only dedicated parsing available for string values
			if (value != null)
			{
				// Set model value, so that it will not disappear from the model state when
				// conversion fails.
				bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

				try
				{
					var converter = TypeConverters[bindingContext.ModelType];
					// return the conversion result.
					return converter.ConvertFrom(value);
				}
				catch (Exception x)
				{
					// add the error.
					bindingContext.ModelState.AddModelError(bindingContext.ModelName, x.Message);
					return null;
				}
			}
			return base.BindModel(controllerContext, bindingContext);
		}
	}
}
