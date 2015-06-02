using NUnit.Framework;
using Qowaiv.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests.TestTools
{
	public static class SvoAssert
	{
		public static void UnderlyingTypeMatches(Type svo, SingleValueObjectAttribute attr)
		{
			var m_Value = svo.GetField("m_Value", BindingFlags.Instance | BindingFlags.NonPublic);
			Assert.IsNotNull(m_Value, "{0} should contain a m_Value field.", svo);
			Assert.AreEqual(attr.UnderlyingType, m_Value.FieldType, "{0}.m_Field", svo);
		}

		public static void ParseMatches(Type svo, SingleValueObjectAttribute attr)
		{
			var staticMethods = svo.GetMethods(BindingFlags.Public | BindingFlags.Static);

			var parse = staticMethods.SingleOrDefault(method =>
					method.Name == "Parse" &&
					method.GetParameters().Length == 1 &&
					method.GetParameters()[0].ParameterType == typeof(String) &&
					method.ReturnType == svo);

			var parseCulture = staticMethods.SingleOrDefault(method =>
				 method.Name == "Parse" &&
				  method.GetParameters().Length == 2 &&
				  method.GetParameters()[0].ParameterType == typeof(String) &&
				  method.GetParameters()[1].ParameterType == typeof(IFormatProvider) &&
				  method.ReturnType == svo);

			if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.Parse))
			{
				Assert.IsNotNull(parse, "{0} should contain a static Parse method.", svo);
				if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.CultureDependent))
				{
					Assert.IsNotNull(parseCulture, "{0} should contain a static Parse method with an IFormatProvider parameter.", svo);
				}
				else
				{
					Assert.IsNull(parseCulture, "{0} should not contain a static Parse method with an IFormatProvider parameter.", svo);
				}
			}
			else
			{
				Assert.IsNull(parse, "{0} should not contain a static Parse method.", svo);
				Assert.IsNull(parseCulture, "{0} should not contain a static Parse method an IFormatProvider parameter.", svo);
			}
		}

		public static void TryParseMatches(Type svo, SingleValueObjectAttribute attr)
		{
			var staticMethods = svo.GetMethods(BindingFlags.Public | BindingFlags.Static);

			// The out parameter.
			var byrefSvo = svo.MakeByRefType();

			var tryParse = staticMethods.SingleOrDefault(method =>
				  method.Name == "TryParse" &&
				  method.GetParameters().Length == 2 &&
				  method.GetParameters()[0].ParameterType == typeof(String) &&
				  method.GetParameters()[1].ParameterType == byrefSvo &&
				  method.GetParameters()[1].IsOut &&
				  method.ReturnType == typeof(Boolean));

			var tryParseCulture = staticMethods.SingleOrDefault(method =>
				 method.Name == "TryParse" &&
				  method.GetParameters().Length == 3 &&
				  method.GetParameters()[0].ParameterType == typeof(String) &&
				  method.GetParameters()[1].ParameterType == typeof(IFormatProvider) &&
				  method.GetParameters()[2].ParameterType == byrefSvo &&
				  method.GetParameters()[2].IsOut &&
				  method.ReturnType == typeof(Boolean));

			if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.TryParse))
			{
				Assert.IsNotNull(tryParse, "{0} should contain a static TryParse method.", svo);
				if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.CultureDependent))
				{
					Assert.IsNotNull(tryParseCulture, "{0} should contain a static TryParse method with an IFormatProvider parameter.", svo);
				}
				else
				{
					Assert.IsNull(tryParseCulture, "{0} should not contain a static TryParse method with an IFormatProvider parameter.", svo);
				}
			}
			else
			{
				Assert.IsNull(tryParse, "{0} should not contain a static TryParse method.", svo);
				Assert.IsNull(tryParseCulture, "{0} should not contain a static TryParse method with an IFormatProvider parameter.", svo);
			}
		}

		public static void IsValidMatches(Type svo, SingleValueObjectAttribute attr)
		{
			var staticMethods = svo.GetMethods(BindingFlags.Public | BindingFlags.Static);
			
			var isValid = staticMethods.SingleOrDefault(method =>
					method.Name == "IsValid" &&
					method.GetParameters().Length == 1 &&
					method.GetParameters()[0].ParameterType == typeof(String) &&
					method.ReturnType == typeof(Boolean));
			
			var isValidCulture = staticMethods.SingleOrDefault(method =>
				   method.Name == "IsValid" &&
				   method.GetParameters().Length == 2 &&
				   method.GetParameters()[0].ParameterType == typeof(String) &&
				   method.GetParameters()[1].ParameterType == typeof(IFormatProvider) &&
				   method.ReturnType == typeof(Boolean));
			
			if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.IsValid))
			{
				Assert.IsNotNull(isValid, "{0} should contain a static IsValid method.", svo);
				if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.CultureDependent))
				{
					Assert.IsNotNull(isValidCulture, "{0} should contain a static IsValid method with an IFormatProvider parameter.", svo);
				}
				else
				{
					Assert.IsNull(isValidCulture, "{0} should not contain a static IsValid method with an IFormatProvider parameter.", svo);
				}
			}
			else
			{
				Assert.IsNull(isValid, "{0} should not contain a static IsValid method.", svo);
				Assert.IsNull(isValidCulture, "{0} should not contain a static IsValid method with an IFormatProvider parameter.", svo);
			}
		}

		public static void EmptyAndUnknownMatches(Type svo, SingleValueObjectAttribute attr)
		{
			var emptyValue = svo.GetFields(BindingFlags.Static | BindingFlags.Public).SingleOrDefault(field =>
					field.Name == "Empty" &&
					field.IsInitOnly &&
					field.FieldType == svo);

			var unknownValue = svo.GetFields(BindingFlags.Static | BindingFlags.Public).SingleOrDefault(field =>
					field.Name == "Unknown" &&
					field.IsInitOnly &&
					field.FieldType == svo);

			var minValue = svo.GetFields(BindingFlags.Static | BindingFlags.Public).SingleOrDefault(field =>
					field.Name == "MinValue" &&
					field.IsInitOnly &&
					field.FieldType == svo);

			var maxValue = svo.GetFields(BindingFlags.Static | BindingFlags.Public).SingleOrDefault(field =>
					field.Name == "MaxValue" &&
					field.IsInitOnly &&
					field.FieldType == svo);

			var isEmptyMethod = svo.GetMethods(BindingFlags.Instance | BindingFlags.Public).SingleOrDefault(method =>
					method.Name == "IsEmpty" &&
					method.GetParameters().Length == 0 &&
					method.ReturnType == typeof(Boolean));

			var isUnknownMethod = svo.GetMethods(BindingFlags.Instance | BindingFlags.Public).SingleOrDefault(method =>
				   method.Name == "IsUnknown" &&
				   method.GetParameters().Length == 0 &&
				   method.ReturnType == typeof(Boolean));

			var isEmptyOrUnknownMethod = svo.GetMethods(BindingFlags.Instance | BindingFlags.Public).SingleOrDefault(method =>
				   method.Name == "IsEmptyOrUnknown" &&
				   method.GetParameters().Length == 0 &&
				   method.ReturnType == typeof(Boolean));



			if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasEmptyValue))
			{
				Assert.IsNotNull(emptyValue, "{0} should contain a static read-only Empty field.", svo);
				Assert.IsNotNull(isEmptyMethod, "{0} should contain a IsEmpty method.", svo);
			}
			else
			{
				Assert.IsNull(emptyValue, "{0} should not contain a static read-only Empty field.", svo);
				Assert.IsNull(isEmptyMethod, "{0} should not contain a IsEmpty method.", svo);
			}

			if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasUnknownValue))
			{
				Assert.IsNotNull(unknownValue, "{0} should contain a static read-only Unknown field.", svo);
				Assert.IsNotNull(isUnknownMethod, "{0} should contain a IsUnknown method.", svo);
			}
			else
			{
				Assert.IsNull(unknownValue, "{0} should not contain a static read-only Unknown field.", svo);
				Assert.IsNull(isUnknownMethod, "{0} should not contain a IsUnknown method.", svo);
			}

			if (attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasEmptyValue) && attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasUnknownValue))
			{
				Assert.IsNotNull(isEmptyOrUnknownMethod, "{0} should contain a IsEmptyOrUnknown method.", svo);
			}
			else
			{
				Assert.IsNull(isEmptyOrUnknownMethod, "{0} should not contain a IsEmptyOrUnknown method.", svo);
			}

			if (attr.StaticOptions == SingleValueStaticOptions.Continuous)
			{
				Assert.IsNotNull(minValue, "{0} should contain a static read-only Unknown MinValue.", svo);
				Assert.IsNotNull(maxValue, "{0} should contain a static read-only Unknown MaxValue.", svo);
			}
		}

		public static void ImplementsInterfaces(Type svo)
		{
			var interfaces = svo.GetInterfaces().ToList();

			var iComparable = typeof(IComparable<>).MakeGenericType(svo);
			
			Assert.IsTrue(interfaces.Contains(typeof(ISerializable)), "{0} should contain ISerializable.", svo);
			Assert.IsTrue(interfaces.Contains(typeof(IXmlSerializable)), "{0} should contain IXmlSerializable.", svo);
			Assert.IsTrue(interfaces.Contains(typeof(IFormattable)), "{0} should contain IFormattable.", svo);
			Assert.IsTrue(interfaces.Contains(typeof(IJsonSerializable)), "{0} should contain IJsonSerializable.", svo);
			Assert.IsTrue(interfaces.Contains(typeof(IComparable)), "{0} should contain IComparable.", svo);
			Assert.IsTrue(interfaces.Contains(iComparable), "{0} should contain IComparable<{0}>.", svo);
		}

		public static void AttributesMatches(Type svo)
		{
			var attributes = svo.GetCustomAttributes(false);

			Assert.IsTrue(attributes.Any(att => att.GetType() == typeof(DebuggerDisplayAttribute)), "{0} should have DebuggerDisplayAttribute.", svo);
			Assert.IsTrue(attributes.Any(att => att.GetType() == typeof(SerializableAttribute)), "{0} should have SerializableAttribute.", svo);
			Assert.IsTrue(attributes.Any(att => att.GetType() == typeof(TypeConverterAttribute)), "{0} should have TypeConverterAttribute.", svo);
		}
	}
}
