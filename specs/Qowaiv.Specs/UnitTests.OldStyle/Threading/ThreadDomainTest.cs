using NUnit.Framework;
using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.TestTools;
using Qowaiv.TestTools.Globalization;
using Qowaiv.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Qowaiv.UnitTests.Threading
{
    public class ThreadDomainTest
    {
        [TearDown]
        public void TearDown()
        {
            ThreadDomain.Current.Clear();
        }

        [Test]
        public void Get_NullableDecimal_ThrowsNotSupportedException()
        {
            var x = Assert.Catch<NotSupportedException>(() => ThreadDomain.Current.Get<decimal?>());
            Assert.AreEqual("Type must be a none generic type.", x.Message);
        }

        [Test]
        public void Get_Object_ThrowsNotSupportedException()
        {
            var x = Assert.Catch<NotSupportedException>(() => ThreadDomain.Current.Get<object>());
            Assert.AreEqual("Converter can not convert from System.String.", x.Message);
        }

        [Test]
        public void Get_CountryByCreator_Portugal()
        {
            using (new CultureInfoScope("pt-PT"))
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

            Task.Factory.StartNew(() =>
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
        public void Remove_Country_BeforSetAndAfter()
        {
            using (TestCultures.Nl_BE.Scoped())
            {
                Assert.AreEqual(Country.BE, Thread.CurrentThread.GetValue<Country>(), "Before.");
                Thread.CurrentThread.SetValue(Country.PT);
                Assert.AreEqual(Country.PT, Thread.CurrentThread.GetValue<Country>(), "Set.");
                Thread.CurrentThread.RemoveValue(typeof(Country));
                Assert.AreEqual(Country.BE, Thread.CurrentThread.GetValue<Country>(), "After.");
            }
        }
    }
    public class NoConverterClass { }
}
