using Qowaiv.Reflection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Qowaiv.Data
{
	/// <summary>Factory class for creating database parameters.</summary>
	public static class SvoParameter
	{
		/// <summary>Creates a <see cref="SqlParameter"/> based on the single value object.</summary>
		/// <param name="parameterName">
		/// The name of the parameter to map.
		/// </param>
		/// <param name="value">
		/// An <see cref="object"/>that is the value of the <see cref="SqlParameter"/>.
		/// </param>
		/// <returns>
		/// A <see cref="SqlParameter"/> with a converted value if the value is a 
		/// single value object, otherwise with a non-converted value.
		/// </returns>
		public static SqlParameter CreateForSql(string parameterName, object value)
		{
			// If null, return DBNull.
			if (value == null) { return new SqlParameter(parameterName, DBNull.Value); }

			var sourceType = value.GetType();

			lock (locker)
			{
				if (!Attributes.TryGetValue(sourceType, out SingleValueObjectAttribute attr))
				{
					attr = QowaivType.GetSingleValueObjectAttribute(sourceType);
				}
				// No attribute, so not supported.
				if (attr == null)
				{
					Attributes[sourceType] = null;
					return new SqlParameter(parameterName, value);
				}

				if (IsDbNullValue(value, sourceType, attr))
				{
					return new SqlParameter(parameterName, DBNull.Value);
				}

				MethodInfo cast = GetCast(sourceType, attr);
				var casted = cast.Invoke(null, new[] { value });
				return new SqlParameter(parameterName, casted);
			}
		}
		
		/// <summary>Returns true if the value should be represented by a <see cref="DBNull.Value"/>, otherwise false.</summary>
		private static bool IsDbNullValue(object value, Type sourceType, SingleValueObjectAttribute attr)
		{
			if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasEmptyValue))
			{
				var defaultValue = Activator.CreateInstance(sourceType);
				return Equals(value, defaultValue);
			}
			return false;
		}

		/// <summary>Gets the cast needed for casting to the database type.</summary>
		/// <exception cref="InvalidCastException">
		/// If the required cast is not defined.
		/// </exception>
		private static MethodInfo GetCast(Type sourceType, SingleValueObjectAttribute attr)
		{
			MethodInfo cast;
			if (!Casts.TryGetValue(sourceType, out cast))
			{
				var returnType = attr.DatabaseType ?? attr.UnderlyingType;
				var methods = sourceType.GetMethods(BindingFlags.Public | BindingFlags.Static)
					.Where(m =>
						m.IsHideBySig &&
						m.IsSpecialName &&
						m.GetParameters().Length == 1 &&
						m.ReturnType == returnType)
					.ToList();

				if (methods.Any())
				{
					cast = methods[0];
					Casts[sourceType] = cast;
				}
				else
				{
					throw new InvalidCastException(string.Format(QowaivMessages.InvalidCastException_FromTo, sourceType, returnType));
				}
			}
			return cast;
		}

		private static readonly Dictionary<Type, SingleValueObjectAttribute> Attributes = new Dictionary<Type, SingleValueObjectAttribute>();
		private static readonly Dictionary<Type, MethodInfo> Casts = new Dictionary<Type, MethodInfo>();

		/// <summary>The locker for adding a casts and unsupported types.</summary>
		private static readonly object locker = new object();
	}
}
