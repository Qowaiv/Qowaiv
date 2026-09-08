namespace Financial.Money_specs;

public class Has_constant
{
    [Test]
    public void Zero_equals_default() => Money.Zero.Should().Be(default);
}

public class Has_properties
{
    [Test]
    public void Currency_is_set() => (42 + Currency.EUR).Currency.Should().Be(Currency.EUR);

    [Test]
    public void Amount_is_set() => (42 + Currency.EUR).Amount.Should().Be((Amount)42);
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Money.CompareTo(Nil.Object).Should().Be(1);

    [Test]
    public void to_Money_as_object()
    {
        object obj = Svo.Money;
        Svo.Money.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_Money_only()
        => new object().Invoking(Svo.Money.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            Money.Zero,
            Money.Zero,
            124.40 + Currency.EUR,
            124.41 + Currency.EUR,
            124.42 + Currency.EUR,
            124.39 + Currency.GBP,
        };

        var list = new List<Money> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();

        list.Should().BeEquivalentTo(sorted);
    }

    [Test]
    public void by_operators_for_different_values()
    {
        Money smaller = 17 + Currency.DKK;
        Money bigger = 19 + Currency.DKK;

        (smaller < bigger).Should().BeTrue();
        (smaller <= bigger).Should().BeTrue();
        (smaller > bigger).Should().BeFalse();
        (smaller >= bigger).Should().BeFalse();
    }

    [Test]
    public void by_operators_for_equal_values()
    {
        Money left = 17 + Currency.DKK;
        Money right = 17 + Currency.DKK;

        (left < right).Should().BeFalse();
        (left <= right).Should().BeTrue();
        (left > right).Should().BeFalse();
        (left >= right).Should().BeTrue();
    }
}

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.Money.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.Money.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.Money.Equals(42 + Currency.EUR).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.Money.Equals(42.17 + Currency.EUR).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (Svo.Money == 42.17 + Currency.EUR).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (Svo.Money == 42 + Currency.USD).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (Svo.Money != 42.17 + Currency.EUR).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (Svo.Money != 42.17 + Currency.USD).Should().BeTrue();

    [TestCase("USD 0.00", 931756561)]
    [TestCase("EUR 42.00", -943833957)]
    public void hash_code_is_value_based(Money svo, int hash)
    {
        using (Hash.WithoutRandomizer())
        {
            svo.GetHashCode().Should().Be(hash);
        }
    }
}

public class Can_be_parsed
{
    [TestCase("€ 42.17")]
    [TestCase("€42.17")]
    [TestCase("€+42.17")]
    [TestCase("EUR42.17")]
    [TestCase("EUR+42.17")]
    [TestCase("EUR 42.17")]
    public void with_currency_before(string before)
        => Money.Parse(before, CultureInfo.InvariantCulture).Should().Be(Svo.Money);

    [TestCase("42.17 €")]
    [TestCase("42.17€")]
    [TestCase("42.17 EUR")]
    [TestCase("42.17EUR")]
    public void with_currency_after(string after)
        => Money.Parse(after, CultureInfo.InvariantCulture).Should().Be(Svo.Money);

    [TestCase("en-GB", "€42.17")]
    [TestCase("nl-NL", "€42,17")]
    public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
    {
        using (culture.Scoped())
        {
            Money.Parse(input).Should().Be(Svo.Money);
        }
    }

    [Test]
    public void with_currency_symbol_before_on_french_culture()
    {
        using (TestCultures.fr_FR.Scoped())
        {
            Money.Parse("€ 12").Should().Be(12 + Currency.EUR);
        }
    }

    [Test]
    public void with_currency_symbol_after_negative_amount_on_french_culture()
    {
        using (TestCultures.fr_FR.Scoped())
        {
            Money.Parse("-12,765 €").Should().Be(-12.765 + Currency.EUR);
        }
    }

    [Test]
    public void with_out_currency()
        => Money.Parse("42.17", CultureInfo.InvariantCulture).Should().Be(42.17 + Currency.Empty);

    [Test]
    public void from_valid_input_only_otherwise_throws_on_Parse()
    {
        using (TestCultures.en_GB.Scoped())
        {
            "invalid input".Invoking(Money.Parse)
                .Should().Throw<FormatException>()
                .WithMessage("Not a valid amount");
        }
    }

    [Test]
    public void from_valid_input_only_otherwise_return_false_on_TryParse()
        => Money.TryParse("invalid input", out _).Should().BeFalse();

    [Test]
    public void from_invalid_as_null_with_TryParse()
        => Money.TryParse("invalid input").Should().BeNull();

    [Test]
    public void with_TryParse_returns_SVO()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Money.TryParse("€42.17").Should().Be(Svo.Money);
        }
    }
}

public class Has_custom_formatting
{
    [Test]
    public void _default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Money.ToString().Should().Be("€42.17");
        }
    }

    [Test]
    public void with_null_format_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Money.ToString(default(string)).Should().Be(Svo.Money.ToString());
        }
    }

    [Test]
    public void with_string_empty_pattern_equal_to_default()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Svo.Money.ToString(string.Empty).Should().Be(Svo.Money.ToString());
        }
    }

    [Test]
    public void default_value_is_represented_as_zero()
    {
        using (CultureInfoScope.NewInvariant())
        {
            Money.Zero.ToString().Should().Be("0");
        }
    }

    [Test]
    public void with_empty_format_provider()
    {
        using (TestCultures.es_EC.Scoped())
        {
            Svo.Money.ToString(FormatProvider.Empty).Should().Be("€42,17");
        }
    }

    [Test]
    public void custom_format_provider_is_applied()
    {
        var formatted = Svo.Money.ToString("0.0", FormatProvider.CustomFormatter);
        formatted.Should().Be("Unit Test Formatter, value: '42.2', format: '0.0'");
    }

    [Test]
    public void with_string_empty_pattern_uses_nl_BE_decimal_separator()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            Svo.Money.ToString("0.00").Should().Be("42,17");
        }
    }

    [TestCase("nl-BE", null, "ALL 1600,1", "Lekë 1.600,10")]
    [TestCase("en-GB", null, "EUR 1600.1", "€1,600.10")]
    [TestCase("en-GB", null, "AED 1600.1", "AED1,600.10")]
    [TestCase("nl-BE", "0000", "800", "0800")]
    [TestCase("en-GB", "0000", "800", "0800")]
    [TestCase("es-EC", "00000.0", "1700", "01700,0")]
    public void culture_dependent(CultureInfo culture, string format, string input, string formatted)
    {
        using (culture.Scoped())
        {
            Money.Parse(input).ToString(format).Should().Be(formatted);
        }
    }

    [Test]
    public void with_amount_after_currency_when_positive_pattern_is_set()
    {
        using (TestCultures.nl_BE.Scoped())
        {
            CultureInfo.CurrentCulture.NumberFormat.CurrencyPositivePattern = 3;
            Money.Parse("ALL 1600,1").ToString().Should().Be("1.600,10 Lekë");
        }
    }

    [Test]
    public void with_current_thread_culture_as_default()
    {
        using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
        {
            Svo.Money.ToString(provider: null).Should().Be("€ 42,17");
        }
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
       => typeof(Money).Should().HaveTypeConverterDefined();

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("€42.17").To<Money>().Should().Be(Svo.Money);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.Money).Should().Be("€42.17");
        }
    }
}

public class Supports_JSON_serialization
{
#if NET8_0_OR_GREATER
    [TestCase("EUR 42.17", "EUR 42.17")]
    [TestCase("EUR42.17", "EUR 42.17")]
    [TestCase("€42.17", "EUR 42.17")]
    [TestCase(100L, "100.00")]
    [TestCase(42.17, "42.17")]
    public void System_Text_JSON_deserialization(object json, Money svo)
        => JsonTester.Read_System_Text_JSON<Money>(json).Should().Be(svo);

    [TestCase("EUR42.17", "EUR42.17")]
    public void System_Text_JSON_serialization(Money svo, object json)
        => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
    [TestCase("EUR 42.17", "EUR 42.17")]
    [TestCase("EUR42.17", "EUR 42.17")]
    [TestCase("€42.17", "EUR 42.17")]
    [TestCase(100L, "100.00")]
    [TestCase(42.17, "42.17")]
    public void convention_based_deserialization(object json, Money svo)
        => JsonTester.Read<Money>(json).Should().Be(svo);

    [TestCase("EUR42.17", "EUR42.17")]
    public void convention_based_serialization(Money svo, object json)
        => JsonTester.Write(svo).Should().Be(json);

    [TestCase("Invalid input", typeof(FormatException))]
    [TestCase("2017-06-11", typeof(FormatException))]
    [TestCase(true, typeof(InvalidOperationException))]
    public void throws_for_invalid_json(object json, Type exceptionType)
        => json
            .Invoking(JsonTester.Read<Money>)
            .Should().Throw<Exception>()
            .And.Should().BeOfType(exceptionType);
}
public class Has_operators
{
    [Test]
    public void to_divide_money_by_money_with_same_currency_as_decimal()
    {
        var ratio = Svo.Money / (16 + Currency.EUR);
        ratio.Should().Be(2.635625m);
    }

    [Test]
    public void to_divide_money_by_money_with_different_currencies_is_not_supported()
    {
        Func<decimal> division = () => Svo.Money / (16 + Currency.USD);
        division.Should().Throw<CurrencyMismatchException>();
    }
}

public class Can_be_operated_on
{
    [TestCase(-1, "EUR -1000")]
    [TestCase(0, "EUR 0")]
    [TestCase(+1, "EUR 100")]
    public void sign(int expected, Money value)
        => value.Sign().Should().Be(expected);

    [TestCase(23.1234, -23.1234)]
    [TestCase(23.1234, +23.1234)]
    public void abs(decimal expected, decimal value)
    {
        var money = value + Currency.USD;
        money.Abs().Should().Be(expected + Currency.USD);
    }

    [Test]
    public void plus()
        => (+Svo.Money).Should().Be(Svo.Money);

    [Test]
    public void negate()
    {
        (-Svo.Money).Should().Be(-42.17 + Currency.EUR);
    }

    [Test]
    public void increment()
    {
        var act = Svo.Money;
        act++;
        act.Should().Be(43.17 + Currency.EUR);
    }

    [Test]
    public void decrement()
    {
        var act = Svo.Money;
        act--;
        act.Should().Be(41.17 + Currency.EUR);
    }

    [Test]
    public void add_same_currency()
    {
        var l = 16 + Currency.EUR;
        var r = 26 + Currency.EUR;
        (l + r).Should().Be(42 + Currency.EUR);
    }

    [Test]
    public void add_percentage()
    {
        var money = 16 + Currency.EUR;
        (money + 25.Percent()).Should().Be(20 + Currency.EUR);
    }

    [Test]
    public void subtract_same_currency()
    {
        var l = 69 + Currency.EUR;
        var r = 27 + Currency.EUR;
        (l - r).Should().Be(42 + Currency.EUR);
    }

    [Test]
    public void subtract_percentage()
    {
        var money = 16 + Currency.EUR;
        (money - 25.Percent()).Should().Be(12 + Currency.EUR);
    }

    [Test]
    public void multiply_percentage()
    {
        var money = 100.40m + Currency.USD;
        (money * 50.Percent()).Should().Be(50.20m + Currency.USD);
    }

    [Test]
    public void multiply_float()
    {
        var money = 100.40m + Currency.USD;
        (money * 0.5F).Should().Be(50.20m + Currency.USD);
    }

    [Test]
    public void multiply_double()
    {
        var money = 100.40m + Currency.USD;
        (money * 0.5).Should().Be(50.20m + Currency.USD);
    }

    [Test]
    public void multiply_decimal()
    {
        var money = 100.40m + Currency.USD;
        (money * 0.5m).Should().Be(50.20m + Currency.USD);
    }

    [Test]
    public void multiply_short()
    {
        var money = 100.40m + Currency.USD;
        (money * (short)2).Should().Be(200.8m + Currency.USD);
    }

    [Test]
    public void multiply_int()
    {
        var money = 100.40m + Currency.USD;
        (money * 2).Should().Be(200.8m + Currency.USD);
    }

    [Test]
    public void multiply_long()
    {
        var money = 100.40m + Currency.USD;
        (money * 2L).Should().Be(200.8m + Currency.USD);
    }

    [Test]
    public void multiply_ushort()
    {
        var money = 100.40m + Currency.USD;
        (money * (ushort)2).Should().Be(200.8m + Currency.USD);
    }

    [Test]
    public void multiply_uint()
    {
        var money = 100.40m + Currency.USD;
        (money * 2u).Should().Be(200.8m + Currency.USD);
    }

    [Test]
    public void multiply_ulong()
    {
        var money = 100.40m + Currency.USD;
        (money * 2ul).Should().Be(200.8m + Currency.USD);
    }

    [Test]
    public void divide_percentage()
    {
        var money = 100.40m + Currency.USD;
        (money / 50.Percent()).Should().Be(200.8m + Currency.USD);
    }

    [Test]
    public void divide_float()
    {
        var money = 100.40m + Currency.USD;
        (money / 0.5F).Should().Be(200.8m + Currency.USD);
    }

    [Test]
    public void divide_double()
    {
        var money = 100.40m + Currency.USD;
        (money / 0.5).Should().Be(200.8m + Currency.USD);
    }

    [Test]
    public void divide_decimal()
    {
        var money = 100.40m + Currency.USD;
        (money / 0.5m).Should().Be(200.8m + Currency.USD);
    }

    [Test]
    public void divide_short()
    {
        var money = 100.40m + Currency.USD;
        (money / (short)2).Should().Be(50.20m + Currency.USD);
    }

    [Test]
    public void divide_int()
    {
        var money = 100.40m + Currency.USD;
        (money / 2).Should().Be(50.20m + Currency.USD);
    }

    [Test]
    public void divide_long()
    {
        var money = 100.40m + Currency.USD;
        (money / 2L).Should().Be(50.20m + Currency.USD);
    }

    [Test]
    public void divide_ushort()
    {
        var money = 100.40m + Currency.USD;
        (money / (ushort)2).Should().Be(50.20m + Currency.USD);
    }

    [Test]
    public void divide_uint()
    {
        var money = 100.40m + Currency.USD;
        (money / 2u).Should().Be(50.20m + Currency.USD);
    }

    [Test]
    public void divide_ulong()
    {
        var money = 100.40m + Currency.USD;
        (money / 2ul).Should().Be(50.20m + Currency.USD);
    }
}

public class Can_be_rounded
{
    [Test]
    public void to_2_digits_by_default()
        => (123.4567m + Currency.EUR).Round().Should().Be(123.46m + Currency.EUR);

    [Test]
    public void to_1_digit()
        => (123.4567m + Currency.EUR).Round(1).Should().Be(123.5m + Currency.EUR);

    [Test]
    public void to_multiple_of_0d25()
        => (123.6567m + Currency.EUR).RoundToMultiple(0.25m).Should().Be(123.75m + Currency.EUR);
}

public class Money_zero
{
    [Test]
    public void can_be_added_to_other()
    {
        var sum = Money.Zero + Money.Zero;
        sum.Should().Be(Money.Zero);
    }

    [Test]
    public void can_be_added_to_zero_currency()
    {
        var first = Money.Zero + (0 + Currency.EUR);
        first.Should().Be(0 + Currency.EUR);

        var second = (0 + Currency.USD) + Money.Zero;
        second.Should().Be(0 + Currency.USD);
    }

    [Test]
    public void can_be_added_to_money_currency()
    {
        var first = Money.Zero + (12 + Currency.EUR);
        first.Should().Be(12 + Currency.EUR);

        var second = (12 + Currency.USD) + Money.Zero;
        second.Should().Be(12 + Currency.USD);
    }
}

public class Throws_when
{
    [Test]
    public void adding_multiple_zero_currencies()
    {
        var euros = 0 + Currency.EUR;
        var dollars = 0 + Currency.USD;
        var operation = () => euros + dollars;

        operation.Should().Throw<CurrencyMismatchException>()
            .WithMessage("The addition operation could not be applied. There is a mismatch between EUR and USD.");
    }

    [Test]
    public void adding_multiple_currencies()
    {
        var euros = 16 + Currency.EUR;
        var dollars = 666 + Currency.USD;
        var operation = () => euros + dollars;

        operation.Should().Throw<CurrencyMismatchException>()
            .WithMessage("The addition operation could not be applied. There is a mismatch between EUR and USD.");
    }

    [Test]
    public void subtracting_multiple_currencies()
    {
        var euros = 16 + Currency.EUR;
        var dollars = 666 + Currency.USD;
        var operation = () => euros - dollars;

        operation.Should().Throw<CurrencyMismatchException>()
            .WithMessage("The subtraction operation could not be applied. There is a mismatch between EUR and USD.");
    }
}

public class Can_be_deconstructed
{
    [Test]
    public void in_amount_and_currency_part()
    {
        var (amount, currency) = Svo.Money;
        amount.Should().Be(Svo.Amount);
        currency.Should().Be(Svo.Currency);
    }
}

public class Supports_XML_serialization
{
    [Test]
    public void using_XmlSerializer_to_serialize()
    {
        var xml = Serialize.Xml(Svo.Money);
        xml.Should().Be("EUR42.17");
    }

    [Test]
    public void using_XmlSerializer_to_deserialize()
    {
        var svo = Deserialize.Xml<Money>("EUR42.17");
        svo.Should().Be(Svo.Money);
    }

    [Test]
    public void using_DataContractSerializer()
    {
        var round_tripped = SerializeDeserialize.DataContract(Svo.Money);
        Svo.Money.Should().Be(round_tripped);
    }

    [Test]
    public void as_part_of_a_structure()
    {
        var structure = XmlStructure.New(Svo.Money);
        var round_tripped = SerializeDeserialize.Xml(structure);
        structure.Should().Be(round_tripped);
    }

    [Test]
    public void has_no_custom_XML_schema()
    {
        IXmlSerializable obj = Svo.Money;
        obj.GetSchema().Should().BeNull();
    }
}
