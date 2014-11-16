using NUnit.Framework;
using Qowaiv.Formatting;
using Qowaiv.UnitTests.TestTools;
using Qowaiv.UnitTests.TestTools.Formatting;
using Qowaiv.UnitTests.TestTools.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Qowaiv.UnitTests.Formatting
{
	[TestFixture]
	public class FormattingArgumentsCollectionTest
	{
		[Test]
		public void Ctor_Null_ThrowsFormatException()
		{
			ExceptionAssert.ExpectArgumentNullException(() =>
			{
				new FormattingArgumentsCollection(null);
			},
			"formatProvider");
		}
		[Test]
		public void Ctor_WithParent_ThrowsFormatException()
		{
			var parent = new FormattingArgumentsCollection(new CultureInfo("nl"));
			parent.Add(typeof(Date), "yyyy-MM");

			var act = new FormattingArgumentsCollection(new CultureInfo("nl-BE"), parent);
			Assert.AreEqual(1, act.Count, "act.Count");

			parent.Add(typeof(Gender), "f");
			Assert.AreEqual(2, parent.Count, "parent.Count");
			Assert.AreEqual(1, act.Count, "act.Count");
		}

		[Test]
		public void Format_NullFormat_ThrowArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException(() =>
			{
				var collection = new FormattingArgumentsCollection();
				collection.Format(null, null);
			}
			, "format");
		}
		[Test]
		public void Format_NullArugment_ThrowArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException(() =>
			{
				var collection = new FormattingArgumentsCollection();
				collection.Format("Value: '{0}'", null);
			}
			, "args");
		}

		[Test]
		public void Format_InvalidFormat_ThrowsFormatException()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
				collection.Format("}");
			},
			"Input string was not in a correct format.");
		}
		[Test]
		public void Format_UnparsebleIndex_ThrowsFormatException()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
				collection.Format("{a}");
			},
			"Input string was not in a correct format.");
		}
		[Test]
		public void Format_IndexOutOfRange_ThrowsFormatException()
		{
			ExceptionAssert.ExpectException<FormatException>(() =>
			{
				var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
				collection.Format("{0}{1}", 1);
			},
			"Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
		}

		[Test]
		public void Format_ArrayWithNullItem_String()
		{
			var collection = new FormattingArgumentsCollection();

			var act = collection.Format("Value: '{0}'", new object[] { null });
			var exp = "Value: ''";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Format_AlignLeft_String()
		{
			var collection = new FormattingArgumentsCollection();

			var act = collection.Format("{0,-4}", "a");
			var exp = "a   ";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Format_AlignRight_String()
		{
			var collection = new FormattingArgumentsCollection();

			var act = collection.Format("{0,3}", "a");
			var exp = "  a";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Format_EscapeLeft_String()
		{
			var collection = new FormattingArgumentsCollection();

			var act = collection.Format("{{");
			var exp = "{";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Format_EscapeRight_String()
		{
			var collection = new FormattingArgumentsCollection();

			var act = collection.Format("}}");
			var exp = "}";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Format_ComplexPattern_AreEqual()
		{
			using (CultureInfoScope.NewInvariant())
			{
				var collection = new FormattingArgumentsCollection(new CultureInfo("nl-BE"));
				collection.Add(typeof(Date), "yyyy-MM-dd HH:mm");
				collection.Add(typeof(Decimal), "0.000");

				var act = collection.Format("{0:000.00} - {1} * {1:dd-MM-yyyy} - {2} - {3} - {4}", 3, new Date(2014, 10, 8), 666, 0.8m, 0.9);
				var exp = "003,00 - 2014-10-08 00:00 * 08-10-2014 - 666 - 0,800 - 0,9";

				Assert.AreEqual(exp, act);
			}
		}
		[Test]
		public void Format_ComplexPatternWithUnitTestFormatProvider_AreEqual()
		{
			using (CultureInfoScope.NewInvariant())
			{
				var collection = new FormattingArgumentsCollection(new UnitTestFormatProvider());
				collection.Add(typeof(Date), "yyyy-MM-dd HH:mm");
				collection.Add(typeof(Decimal), "0.000");

				var act = collection.Format("{0:yyyy-MM-dd} * {0}", new Date(2014, 10, 8));
				var exp = "Unit Test Formatter, value: '10/08/2014', format: 'yyyy-MM-dd' * Unit Test Formatter, value: '10/08/2014', format: ''";

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void ToString_IFormattableNull_IsNull()
		{
			var collection = new FormattingArgumentsCollection();
			String act = collection.ToString((IFormattable)null);
			String exp = null;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_ObjectNull_IsNull()
		{
			var collection = new FormattingArgumentsCollection();
			String act = collection.ToString((Object)null);
			String exp = null;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_TypeInt32_SystemInt32()
		{
			var collection = new FormattingArgumentsCollection();
			String act = collection.ToString((Object)typeof(Int32));
			String exp = "System.Int32";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void ToString_7_007()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Add(typeof(Int32), "000");
			String act = collection.ToString((Object)7);
			String exp = "007";

			Assert.AreEqual(exp, act);
		}

		#region Collection manipulation

		[Test]
		public void Add_NullType_ThrowsArgumentNullException()
		{
			ExceptionAssert.ExpectArgumentNullException(() =>
			{
				var collection = new FormattingArgumentsCollection();
				collection.Add(null, "");
			},
			"type");
		}
		[Test]
		public void Add_NotIFormattebleType_ThrowsArgumentException()
		{
			ExceptionAssert.ExpectArgumentException(() =>
			{
				var collection = new FormattingArgumentsCollection();
				collection.Add(typeof(Type), "");
			},
			"type",
			"The argument must implement System.IFormattable.");
		}
		[Test]
		public void Add_DuplicateKey_ThrowsArgumentException()
		{
			ExceptionAssert.ExpectArgumentException(() =>
			{
				var collection = new FormattingArgumentsCollection();
				collection.Add(typeof(Int32), "New");
				collection.Add(typeof(Int32), "Update");
			},
			null,
			"An item with the same key has already been added.");
		}

		[Test]
		public void Add_Int32Format_Contains1Item()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Add(typeof(Int32), "Int32Format");

			Assert.AreEqual(1, collection.Count, "Count");
		}
		[Test]
		public void Add_Int32CultureInfo_Contains1Item()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Add(typeof(Int32), new CultureInfo("nl-NL"));

			Assert.AreEqual(1, collection.Count, "Count");
		}
		[Test]
		public void Add_Int32FormatAndFormatProvider_Contains1Item()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Add(typeof(Int32), "Int32Format", new CultureInfo("nl-NL"));

			Assert.AreEqual(1, collection.Count, "Count");
		}

		[Test]
		public void Set_Int32Format_Contains1Item()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Set(typeof(Int32), "Int32Format");

			Assert.AreEqual(1, collection.Count, "Count");
		}
		[Test]
		public void Set_Int32CultureInfo_Contains1Item()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Set(typeof(Int32), new CultureInfo("nl-NL"));

			Assert.AreEqual(1, collection.Count, "Count");
		}
		[Test]
		public void Set_Int32FormatAndFormatProviderContains1Item()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Set(typeof(Int32), "Int32Format", new CultureInfo("nl-NL"));

			Assert.AreEqual(1, collection.Count, "Count");
		}
		[Test]
		public void Set_SameTypeTwice_Contains1Item()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Set(typeof(Int32), "New");
			collection.Set(typeof(Int32), "Update");

			Assert.AreEqual(1, collection.Count, "Count");
		}

		[Test]
		public void Remove_Int32_Unsuccessful()
		{
			var collection = new FormattingArgumentsCollection();
			var act = collection.Remove(typeof(Int32));
			var exp = false;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Remove_AddedInt32_Successful()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Add(typeof(Int32), "Int32Format");
			var act = collection.Remove(typeof(Int32));
			var exp = true;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Types_CollectionWithTwoItems_Int32AndDate()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Add(typeof(Int32), "Int32Format");
			collection.Add(typeof(Date), "Date");

			var act = collection.Types;
			var exp = new Type[] { typeof(Int32), typeof(Date) };

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetEnumerator_IEnumerableKeyValuePair_IsNotNull()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Add(typeof(Int32), "Int32Format");
			collection.Add(typeof(Date), "Date");

			var ienumerable = collection as IEnumerable<KeyValuePair<Type, FormattingArguments>>;

			var act = ienumerable.GetEnumerator();
			Assert.IsNotNull(act);
		}
		[Test]
		public void GetEnumerator_IEnumerable_IsNotNull()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Add(typeof(Int32), "Int32Format");
			collection.Add(typeof(Date), "Date");

			var ienumerable = collection as IEnumerable;

			var act = ienumerable.GetEnumerator();
			Assert.IsNotNull(act);
		}

		[Test]
		public void Clear_CollectionWithTwoItems_0Items()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Add(typeof(Int32), "Int32Format");
			collection.Add(typeof(Date), "Date");
			collection.Clear();

			var act = collection.Count;
			var exp = 0;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Contains_FilledCollectionInt32_IsTrue()
		{
			var collection = new FormattingArgumentsCollection();
			collection.Add(typeof(Int32), "00");
			var act = collection.Contains(typeof(Int32));
			var exp = true;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Contains_EmptyCollectionInt32_IsFalse()
		{
			var collection = new FormattingArgumentsCollection();
			var act = collection.Contains(typeof(Int32));
			var exp = false;
			Assert.AreEqual(exp, act);
		}

		#endregion

		[Test]
		public void DebuggerDisplay_FormattingArgumentsCollection_HasAttribute()
		{
			DebuggerDisplayAssert.HasAttribute(typeof(FormattingArgumentsCollection));
		}
		[Test]
		public void DebuggerDisplay_FormattingArgumentsCollection_String()
		{
			DebuggerDisplayAssert.HasResult("FormattingArgumentsCollection: 'en-GB', Items: 0", new FormattingArgumentsCollection(new CultureInfo("en-GB")));
		}
	}
}
