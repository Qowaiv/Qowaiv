namespace Qowaiv.UnitTests.Financial;

/// <summary>Tests the currency SVO.</summary>
public class CurrencyTest
{
    /// <summary>The test instance for most tests.</summary>
    public static readonly Currency TestStruct = Currency.EUR;

    #region currency const tests

    /// <summary>Currency.Empty should be equal to the default of currency.</summary>
    [Test]
    public void Empty_None_EqualsDefault() => Currency.Empty.Should().Be(default);

    #endregion

    #region Current

    [Test]
    public void Current_CurrentCultureNlNL_Germany()
    {
        using (TestCultures.nl_NL.Scoped())
        {
            var act = Currency.Current;
            var exp = Currency.EUR;

            act.Should().Be(exp);
        }
    }

    [Test]
    public void Current_CurrentCultureEsEC_Ecuador()
    {
        using (TestCultures.es_EC.Scoped())
        {
            var act = Currency.Current;
            var exp = Currency.USD;

            act.Should().Be(exp);
        }
    }

    [Test]
    public void Current_CurrentCultureEn_Empty()
    {
        using (TestCultures.en.Scoped())
        {
            var act = Currency.Current;
            var exp = Currency.Empty;

            act.Should().Be(exp);
        }
    }

    #endregion

    #region currency IsEmpty tests

    /// <summary>Currency.IsEmpty() should be true for the default of currency.</summary>
    [Test]
    public void IsEmpty_Default_IsTrue() => default(Currency).IsEmpty().Should().BeTrue();
    /// <summary>Currency.IsEmpty() should be false for Currency.Unknown.</summary>
    [Test]
    public void IsEmpty_Unknown_IsFalse() => Currency.Unknown.IsEmpty().Should().BeFalse();
    /// <summary>Currency.IsEmpty() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmpty_TestStruct_IsFalse() => TestStruct.IsEmpty().Should().BeFalse();

    /// <summary>Currency.IsUnknown() should be false for the default of currency.</summary>
    [Test]
    public void IsUnknown_Default_IsFalse() => default(Currency).IsUnknown().Should().BeFalse();
    /// <summary>Currency.IsUnknown() should be true for Currency.Unknown.</summary>
    [Test]
    public void IsUnknown_Unknown_IsTrue() => Currency.Unknown.IsUnknown().Should().BeTrue();
    /// <summary>Currency.IsUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsUnknown_TestStruct_IsFalse() => TestStruct.IsUnknown().Should().BeFalse();

    /// <summary>Currency.IsEmptyOrUnknown() should be true for the default of currency.</summary>
    [Test]
    public void IsEmptyOrUnknown_Default_IsFalse() => default(Currency).IsEmptyOrUnknown().Should().BeTrue();
    /// <summary>Currency.IsEmptyOrUnknown() should be true for Currency.Unknown.</summary>
    [Test]
    public void IsEmptyOrUnknown_Unknown_IsTrue() => Currency.Unknown.IsEmptyOrUnknown().Should().BeTrue();
    /// <summary>Currency.IsEmptyOrUnknown() should be false for the TestStruct.</summary>
    [Test]
    public void IsEmptyOrUnknown_TestStruct_IsFalse() => TestStruct.IsEmptyOrUnknown().Should().BeFalse();

    #endregion

    #region IFormatProvider

    [Test]
    public void FormatAmount_BYR_NAf12Dot34()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Amount number = (Amount)1200.34m;
            var act = number.ToString("C", Currency.BYR);
            var exp = "BYR1,200";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void FormatAmount_ANG_NAf12Dot34()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Amount number = (Amount)12.34m;
            var act = number.ToString("C", Currency.ANG);
            var exp = "NAf.12.34";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void FormatDecimal_ANG_ANG12Dot34()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var number = 12.34m;
            var act = number.ToString("C", Currency.ANG);
            var exp = "NAf.12.34";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void FormatDecimal_TND_TND12Dot340()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var number = 12.34m;
            var act = number.ToString("C", Currency.TND);
            var exp = "TND12.340";
            act.Should().Be(exp);
        }
    }

    [Test]
    public void FormatDecimal_EUR_E12Dot34()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var number = 12.34m;
            var act = number.ToString("C", Currency.EUR);
            var exp = "€12.34";

            act.Should().Be(exp);
        }
    }

    [Test]
    public void FormatDouble_EUR_E12Dot34()
    {
        using (TestCultures.en_GB.Scoped())
        {
            var number = 12.34;
            var act = number.ToString("C", Currency.EUR);
            var exp = "€12.34";

            act.Should().Be(exp);
        }
    }

    #endregion

    #region Properties

    [Test]
    public void Symbol_AZN_Special()
    {
        var act = Currency.AZN.Symbol;
        var exp = "₼";
        act.Should().Be(exp);
    }

    #endregion
}
