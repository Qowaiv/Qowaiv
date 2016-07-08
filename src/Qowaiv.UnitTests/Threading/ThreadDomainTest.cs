using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.IO;
using Qowaiv.Threading;
using Qowaiv.UnitTests.TestTools;
using Qowaiv.UnitTests.TestTools.Globalization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Qowaiv.UnitTests.Threading
{
	[TestFixture]
	public class ThreadDomainTest
	{
		[TearDown]
		public void TearDown()
		{
			ThreadDomain.Current.Clear();
		}

		[Test]
		public void Register_TypeNull_ThrowsArgumentNullException()
		{
			ExceptionAssert.CatchArgumentNullException(() =>
			{
				ThreadDomain.Register(null, null);
			},
			"type");
		}
		[Test]
		public void Register_ActionNull_ThrowsArgumentNullException()
		{
			ExceptionAssert.CatchArgumentNullException(() =>
			{
				ThreadDomain.Register(typeof(Int32), null);
			},
			"creator");
		}

		[Test]
		public void Get_NullableDecimal_ThrowsNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				ThreadDomain.Current.Get<decimal?>();
			},
			"Type must be a none generic type.");
		}
		
		[Test]
		public void Get_Object_ThrowsNotSupportedException()
		{
			Assert.Catch<NotSupportedException>(() =>
			{
				ThreadDomain.Current.Get<Object>();
			},
			"Converter can not convert from System.String.");
		}

		[Test]
		public void Get_StreamSizeByConfiguration_12kB()
		{
			StreamSize act = ThreadDomain.Current.Get<StreamSize>();
			StreamSize exp = 12000;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Get_CountryByCreator_Portugal()
		{
			using(new CultureInfoScope("pt-PT"))
			{
				var act = ThreadDomain.Current.Get<Country>();
				var exp = Country.PT;

				Assert.AreEqual(exp, act);
			}
		}

		[Test]
		public void Get_IBanByDefault_Empty()
		{
			var act = ThreadDomain.Current.Get<InternationalBankAccountNumber>();
			var exp = InternationalBankAccountNumber.Empty;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Set_Currency_AlbanianLek()
		{
			ThreadDomain.Current.Set(Currency.ALL);

			var act = ThreadDomain.Current.Get<Currency>();
			var exp = Currency.ALL;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Set_MultiThreads_()
		{
			ThreadDomain.Current.Set<Percentage>(0.031418m);

			Task.Factory.StartNew(()=>
			{
				var act = ThreadDomain.Current.Get<Percentage>();
				var exp = Percentage.Zero;

				Assert.AreEqual(exp, act, "StartNew");
			});

			var act0 = ThreadDomain.Current.Get<Percentage>();
			var exp0 = (Percentage)0.031418m;

			Assert.AreEqual(exp0, act0, "Old");

		}

		[Test]
		public void Remove_Null_ThrowsArgumentNullException()
		{
			ExceptionAssert.CatchArgumentNullException(() =>
			{
				ThreadDomain.Current.Remove(null);
			},
			"type");
		}

		[Test]
		public void Remove_Country_BeforSetAndAfter()
		{
			using(new CultureInfoScope("nl-BE"))
			{
				Assert.AreEqual(Country.BE, Thread.CurrentThread.GetValue<Country>(), "Before.");
				Thread.CurrentThread.SetValue<Country>(Country.PT);
				Assert.AreEqual(Country.PT, Thread.CurrentThread.GetValue<Country>(), "Set.");
				Thread.CurrentThread.RemoveValue(typeof(Country));
				Assert.AreEqual(Country.BE, Thread.CurrentThread.GetValue<Country>(), "After.");
			}
		}
	}
	public class NoConverterClass { }
}
