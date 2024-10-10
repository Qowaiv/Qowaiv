using Qowaiv.Threading;
using System.Threading;

namespace Threading.Thread_domain_specs;

public class Gets
{
    [Test]
    public void Country_based_on_current_culture()
    {
        using (TestCultures.pt_PT.Scoped())
        {
            ThreadDomain.Current.Get<Country>().Should().Be(Country.PT);
        }
    }

    [Test]
    public void default_value_for_type_without_registered_factory()
        => ThreadDomain.Current.Get<InternationalBankAccountNumber>()
            .Should().Be(default);

    [Test]
    public void value_that_has_been_set_for_the_current_thread()
    {
        ThreadDomain.Current.Set(Currency.ALL);

        ThreadDomain.Current.Get<Currency>()
            .Should().Be(Currency.ALL);
    }

    [Test]
    public void value_only_for_current_thread()
    {
        ThreadDomain.Current.Set(3.1418.Percent());

        Task.Factory.StartNew
        (
            () => ThreadDomain.Current.Get<Percentage>().Should().Be(0.Percent(), because: "Retrieved in another thread.")
        );

        ThreadDomain.Current.Get<Percentage>().Should().Be(3.1418.Percent(), because: "This thread has a set value.");
    }

    [Test]
    public void Gets_updated_value_when_overwritten()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            Thread.CurrentThread.GetValue<Country>().Should().Be(Country.BE, because: "Default for current thread.");
            Thread.CurrentThread.SetValue(Country.PT);
            Thread.CurrentThread.GetValue<Country>().Should().Be(Country.PT, because: "Default has been overwritten.");
        }
    }

    [Test]
    public void Gets_default_value_when_reset()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            Thread.CurrentThread.SetValue(Country.PT);
            Thread.CurrentThread.GetValue<Country>().Should().Be(Country.PT, because: "Default has been overwritten.");
            Thread.CurrentThread.RemoveValue(typeof(Country));
            Thread.CurrentThread.GetValue<Country>().Should().Be(Country.BE, because: "Default for current thread.");
        }
    }

    [TearDown]
    public void TearDown() => ThreadDomain.Current.Clear();
}

public class Does_not_support
{
    [Test]
    public void Getting_a_nulable_value()
        => ThreadDomain.Current.Invoking(c => c.Get<decimal?>())
           .Should().Throw<NotSupportedException>()
           .WithMessage("Type must be a none generic type.");

    [Test]
    public void Getting_an_type_without_a_string_converter()
        => ThreadDomain.Current.Invoking(c => c.Get<object>())
           .Should().Throw<NotSupportedException>()
           .WithMessage("Converter can not convert from System.String.");

    [TearDown]
    public void TearDown() => ThreadDomain.Current.Clear();
}
