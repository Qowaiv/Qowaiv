﻿using Qowaiv.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace Qowaiv.UnitTests.Threading;

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
        x.Message.Should().Be("Type must be a none generic type.");
    }

    [Test]
    public void Get_Object_ThrowsNotSupportedException()
    {
        var x = Assert.Catch<NotSupportedException>(() => ThreadDomain.Current.Get<object>());
        x.Message.Should().Be("Converter can not convert from System.String.");
    }

    [Test]
    public void Get_CountryByCreator_Portugal()
    {
        using (new CultureInfoScope("pt-PT"))
        {
            var act = ThreadDomain.Current.Get<Country>();
            var exp = Country.PT;

            act.Should().Be(exp);
        }
    }

    [Test]
    public void Get_IBanByDefault_Empty()
    {
        var act = ThreadDomain.Current.Get<InternationalBankAccountNumber>();
        var exp = InternationalBankAccountNumber.Empty;

        act.Should().Be(exp);
    }

    [Test]
    public void Set_Currency_AlbanianLek()
    {
        ThreadDomain.Current.Set(Currency.ALL);

        var act = ThreadDomain.Current.Get<Currency>();
        var exp = Currency.ALL;

        act.Should().Be(exp);
    }

    [Test]
    public void Set_MultiThreads_()
    {
        ThreadDomain.Current.Set<Percentage>(3.1418m.Percent());

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
    public void Remove_Country_before_set_and_after()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            Assert.AreEqual(Country.BE, Thread.CurrentThread.GetValue<Country>(), "Before.");
            Thread.CurrentThread.SetValue(Country.PT);
            Assert.AreEqual(Country.PT, Thread.CurrentThread.GetValue<Country>(), "Set.");
            Thread.CurrentThread.RemoveValue(typeof(Country));
            Assert.AreEqual(Country.BE, Thread.CurrentThread.GetValue<Country>(), "After.");
        }
    }
}

[EmptyTestClass]
public class NoConverterClass { }
