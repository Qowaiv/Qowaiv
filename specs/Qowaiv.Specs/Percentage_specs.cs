namespace Percentage_specs;

#if NET8_0_OR_GREATER
public class Decimal_scale
{
    [Test]
    public void _0_for_Percentage_Hudrded()
    {
        Scale(Percentage.Hundred).Should().Be(0);
    }

    [Test]
    public void _0_for_100_Percent()
    {
        Scale(100.Percent()).Should().Be(0);
        Scale(100d.Percent()).Should().Be(0);
        Scale(100m.Percent()).Should().Be(0);
        Scale(100.0m.Percent()).Should().Be(0);
        Scale(100.00m.Percent()).Should().Be(0);
    }

    [Test]
    public void not_bigger_than_28()
    {
        var p = (100m / 3).Percent();
        Scale(p).Should().Be(28);
    }

    [TestCase("3.14%")]
    [TestCase("3.140%")]
    [TestCase("3.1400%")]
    [TestCase("3.14000%")]
    public void _minimum_for_parsed(string str)
        => Scale(Percentage.Parse(str, TestCultures.en)).Should().Be(4);

    private static int Scale(Percentage p) => ((decimal)p).Scale;
}
#endif

public class Is_valid_for
{
	[TestCase("1751‱", "en")]
	[TestCase("175.1‰", "en")]
	[TestCase("17.51", "en")]
	[TestCase("17.51%", "en")]
	[TestCase("17,51%", "nl")]
	public void strings_representing_SVO(string input, CultureInfo culture)
		=> Percentage.TryParse(input, culture).Should().NotBeNull();

	[TestCase("175.1<>", "en")]
	[TestCase("17,51#", "nl")]
	public void custom_culture_with_different_symbols(string input, CultureInfo culture)
	{
		using (culture.WithPercentageSymbols("#", "<>").Scoped())
		{
			Percentage.TryParse(input).Should().NotBeNull();
		}
	}
}

public class Is_not_valid_for
{
	[TestCase("‱1‱")]
	[TestCase("‱1‰")]
	[TestCase("‱1%")]
	public void two_symbols(string str)
		=> Percentage.TryParse(str).Should().BeNull();

	[TestCase("1‱1")]
	[TestCase("1‰1")]
	[TestCase("1%1")]
	public void symbol_in_the_middle(string str)
		=> Percentage.TryParse(str).Should().BeNull();
}

public class Has_constant
{
	[Test]
	public void Zero_represent_default_value()
	{
		Percentage.Zero.Should().Be(default);
	}

	[Test]
	public void One_represent_1_percent()
		=> Percentage.One.ToString("0%", CultureInfo.InvariantCulture).Should().Be("1%");

	[Test]
	public void Hundred_represent_100_percent()
		=> Percentage.Hundred.ToString("0%", CultureInfo.InvariantCulture).Should().Be("100%");

    [Test]
    public void Min_Value()
        => ((decimal)Percentage.MinValue).Should().Be(decimal.MinValue / 10_000);

    [Test]
    public void Max_Value()
        => ((decimal)Percentage.MaxValue).Should().Be(decimal.MaxValue / 10_000);
}

public class Is_equal_by_value
{
	[Test]
	public void not_equal_to_null()
	{
		Svo.Percentage.Equals(null).Should().BeFalse();
	}

	[Test]
	public void not_equal_to_other_type()
	{
		Svo.Percentage.Equals(new object()).Should().BeFalse();
	}

	[Test]
	public void not_equal_to_different_value()
	{
		Svo.Percentage.Equals(84.17.Percent()).Should().BeFalse();
	}

	[Test]
	public void equal_to_same_value()
	{
		Svo.Percentage.Equals(17.51.Percent()).Should().BeTrue();
	}

	[Test]
	public void equal_operator_returns_true_for_same_values()
	{
		(Svo.Percentage == 17.51.Percent()).Should().BeTrue();
	}

	[Test]
	public void equal_operator_returns_false_for_different_values()
	{
		(Svo.Percentage == 6.66.Percent()).Should().BeFalse();
	}

	[Test]
	public void not_equal_operator_returns_false_for_same_values()
	{
		(Svo.Percentage != 17.51.Percent()).Should().BeFalse();
	}

	[Test]
	public void not_equal_operator_returns_true_for_different_values()
	{
		(Svo.Percentage != 6.66.Percent()).Should().BeTrue();
	}

	[TestCase("0%", 0)]
	[TestCase("17.51%", 665367300)]
	public void hash_code_is_value_based(Percentage svo, int hash)
	{
		using (Hash.WithoutRandomizer())
		{
			svo.GetHashCode().Should().Be(hash);
		}
	}
}

public class Can_be_parsed
{
	[TestCase("en", "175.1‰")]
	[TestCase("en", "175.1‰")]
	[TestCase("en", "1751‱")]
	[TestCase("nl", "17,51%")]
	[TestCase("fr-FR", "%17,51")]
	public void from_string_with_different_formatting_and_cultures(CultureInfo culture, string input)
	{
		using (culture.Scoped())
		{
			var parsed = Percentage.Parse(input);
			parsed.Should().Be(Svo.Percentage);
		}
	}

	[TestCase("175.1<>", "en")]
	[TestCase("17,51#", "nl")]
	public void with_custom_culture_with_different_symbols(string input, CultureInfo culture)
	{
		var parsed = Percentage.Parse(input, culture.WithPercentageSymbols("#", "<>"));
		parsed.Should().Be(Svo.Percentage);
	}

	[Test]
	public void from_valid_input_only_otherwise_throws_on_Parse()
	{
		using (TestCultures.en_GB.Scoped())
		{
			"invalid input".Invoking(Percentage.Parse)
				.Should().Throw<FormatException>()
				.WithMessage("Not a valid percentage");
		}
	}

	[Test]
	public void from_valid_input_only_otherwise_return_false_on_TryParse()
	{
		Percentage.TryParse("invalid input", out _).Should().BeFalse();
	}

	[Test]
	public void from_invalid_as_null_with_TryParse()
		=> Percentage.TryParse("invalid input").Should().BeNull();

	[Test]
	public void with_TryParse_returns_SVO()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Percentage.TryParse("17.51%").Should().Be(Svo.Percentage);
		}
	}

    [Test]
    public void for_the_max_value()
       => Percentage.Parse(Percentage.MaxValue.ToJson(), CultureInfo.InvariantCulture)
       .Should().Be(Percentage.MaxValue);
}

public class Can_not_be_parsed
{
	[TestCase(NumberStyles.HexNumber)]
	[TestCase(NumberStyles.AllowExponent)]
	public void using_a_number_style_other_then_Number(NumberStyles style)
		=> style.Invoking(s => Percentage.TryParse("4.5%", s, CultureInfo.InvariantCulture, out _))
			.Should().Throw<ArgumentOutOfRangeException>()
			.WithMessage("The number style '*' is not supported.*");
}

public class Can_be_created
{
    [TestCase("-7922816251426433759354395.0336")]
    [TestCase("+7922816251426433759354395.0336")]
    public void when_within_the_boundries(decimal d)
        => d.Invoking(Percentage.Create).Should()
        .Throw<ArgumentOutOfRangeException>()
        .WithMessage("Value is either too large or too small for a percentage.");
}

public class Can_be_created_with_percentage_extension
{
	[Test]
	public void from_int()
	{
		var p = 3.Percent();
		p.ToString(CultureInfo.InvariantCulture).Should().Be("3%");
	}

	[Test]
	public void from_double()
	{
		var p = 3.14.Percent();
		p.ToString(CultureInfo.InvariantCulture).Should().Be("3.14%");
	}

	[Test]
	public void from_decimal()
	{
		var p = 3.14m.Percent();
		p.ToString(CultureInfo.InvariantCulture).Should().Be("3.14%");
	}
}

public class Has_custom_formatting
{
	[Test]
	public void _default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.Percentage.ToString().Should().Be("17.51%");
		}
	}

	[Test]
	public void with_null_pattern_equal_to_default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.Percentage.ToString(default(string)).Should().Be(Svo.Percentage.ToString());
		}
	}

	[Test]
	public void with_string_empty_pattern_equal_to_default()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.Percentage.ToString(string.Empty).Should().Be(Svo.Percentage.ToString());
		}
	}

	[Test]
	public void custom_format_provider_is_applied()
	{
		var formatted = Svo.Percentage.ToString("0.000%", FormatProvider.CustomFormatter);
		formatted.Should().Be("Unit Test Formatter, value: '17.510%', format: '0.000%'");
	}

	[TestCase("en-GB", null, "17.51%", "17.51%")]
	[TestCase("nl-BE", "0.000%", "17.51%", "17,510%")]
	[TestCase("en", "%0.###", "17.51%", "%17.51")]
	[TestCase("en", "‰0.###", "17.51%", "‰175.1")]
	[TestCase("en", "‱0.###", "17.51%", "‱1751")]
	[TestCase("en", "0.###%", "17.51%", "17.51%")]
	[TestCase("en", "0.###‰", "17.51%", "175.1‰")]
	[TestCase("en", "0.###‱", "17.51%", "1751‱")]
	[TestCase("en", "0.###", "17.51%", "17.51")]
	public void culture_dependent(CultureInfo culture, string format, Percentage svo, string expected)
	{
		using (culture.Scoped())
		{
			svo.ToString(format).Should().Be(expected);
		}
	}

	[Test]
	public void with_current_thread_culture_as_default()
	{
		using (new CultureInfoScope(culture: TestCultures.nl_NL, cultureUI: TestCultures.en_GB))
		{
			Svo.Percentage.ToString(provider: null).Should().Be("17,51%");
		}
	}

	[TestCase("fr-FR", "%")]
	[TestCase("fa-IR", "٪")]
	public void with_percent_sign_before_for(string culture, string sign)
		=> Svo.Percentage.ToString(TestCultures.Select(culture)).Should().StartWith(sign);

	[Test]
	public void using_per_mille_sign()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.Percentage.ToString("PM").Should().Be("175.1‰");
		}
	}

	[Test]
	public void using_per_then_thousand_sign()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Svo.Percentage.ToString("PT").Should().Be("1751‱");
		}
	}

	[TestCase("0.##%", "792281625142643375935439503.35%")]
	[TestCase("0.#‰", "7922816251426433759354395033.5‰")]
	[TestCase("0‱", "79228162514264337593543950335‱")]
	public void for_max_value(string format, string formatted)
		=> Percentage.MaxValue.ToString(format, CultureInfo.InvariantCulture).Should().Be(formatted);

	[TestCase("0.##%", "-792281625142643375935439503.35%")]
	[TestCase("0.#‰", "-7922816251426433759354395033.5‰")]
	[TestCase("0‱", "-79228162514264337593543950335‱")]
	public void for_min_value(string format, string formatted)
		=> Percentage.MinValue.ToString(format, CultureInfo.InvariantCulture).Should().Be(formatted);
}

public class Formatting_is_invalid
{
	[Test]
	public void when_multiple_symbols_are_specified()
		=> "0%%".Invoking(Svo.Percentage.ToString).Should().Throw<FormatException>();
}

public class Is_comparable
{
	[Test]
	public void to_null_is_1() => Svo.Percentage.CompareTo(Nil.Object).Should().Be(1);

	[Test]
	public void to_Percentage_as_object()
	{
		object obj = Svo.Percentage;
		Svo.Percentage.CompareTo(obj).Should().Be(0);
	}

	[Test]
	public void to_Percentage_only()
		=> new object().Invoking(Svo.Percentage.CompareTo).Should().Throw<ArgumentException>();

	[Test]
	public void can_be_sorted_using_compare()
	{
		var sorted = new[]
		{
				Percentage.Zero,
				Percentage.One,
				17.51.Percent(),
				33.33.Percent(),
				84.17.Percent(),
				Percentage.Hundred,
			};
		var list = new List<Percentage> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
		list.Sort();
		list.Should().BeEquivalentTo(sorted);
	}

	[Test]
	public void _operators_for_different_values()
	{
		Percentage smaller = 17.51.Percent();
		Percentage bigger = 84.17.Percent();
		(smaller < bigger).Should().BeTrue();
		(smaller <= bigger).Should().BeTrue();
		(smaller > bigger).Should().BeFalse();
		(smaller >= bigger).Should().BeFalse();
	}

	[Test]
	public void _operators_for_equal_values()
	{
		Percentage left = 17.51.Percent();
		Percentage right = 17.51.Percent();
		(left < right).Should().BeFalse();
		(left <= right).Should().BeTrue();
		(left > right).Should().BeFalse();
		(left >= right).Should().BeTrue();
	}
}

public class Casts
{
	[Test]
	public void explicitly_from_decimal()
	{
		var casted = (Percentage)0.1751m;
		casted.Should().Be(Svo.Percentage);
	}

	[Test]
	public void explicitly_to_decimal()
	{
		var casted = (decimal)Svo.Percentage;
		casted.Should().Be(0.1751m);
	}

	[Test]
	public void explicitly_from_double()
	{
		var casted = (Percentage)0.1751;
		casted.Should().Be(Svo.Percentage);
	}

	[Test]
	public void explicitly_to_double()
	{
		var casted = (double)Svo.Percentage;
		casted.Should().Be(0.1751);
	}
}

public class Can_be_multiplied_by
{
	[Test]
	public void _percentage()
	{
		var multiplied = 17.Percent() * 42.Percent();
		multiplied.Should().Be(7.14.Percent());
	}

	[Test]
	public void _decimal()
	{
		var multiplied = 17.Percent() * 0.42m;
		multiplied.Should().Be(7.14.Percent());
	}

	[Test]
	public void _double()
	{
		var multiplied = 17.Percent() * 0.42;
		multiplied.Should().Be(7.14.Percent());
	}

	[Test]
	public void _float()
	{
		var multiplied = 17.Percent() * 0.42F;
		multiplied.Should().Be(7.14.Percent());
	}

	[Test]
	public void _int()
	{
		var multiplied = 17.Percent() * 2;
		multiplied.Should().Be(34.Percent());
	}

	[Test]
	public void _uint()
	{
		var multiplied = 17.Percent() * 2U;
		multiplied.Should().Be(34.Percent());
	}

	[Test]
	public void _long()
	{
		var multiplied = 17.Percent() * 2L;
		multiplied.Should().Be(34.Percent());
	}

	[Test]
	public void _ulong()
	{
		var multiplied = 17.Percent() * 2UL;
		multiplied.Should().Be(34.Percent());
	}

	[Test]
	public void _short()
	{
		var multiplied = 17.Percent() * ((short)2);
		multiplied.Should().Be(34.Percent());
	}

	[Test]
	public void _ushort()
	{
		var multiplied = 17.Percent() * ((ushort)2);
		multiplied.Should().Be(34.Percent());
	}
}

public class Can_be_divided_by
{
	[Test]
	public void _percentage()
	{
		var multiplied = 17.Percent() / 50.Percent();
		multiplied.Should().Be(34.Percent());
	}

	[Test]
	public void _decimal()
	{
		var multiplied = 17.Percent() / 0.5m;
		multiplied.Should().Be(34.Percent());
	}

	[Test]
	public void _double()
	{
		var multiplied = 17.Percent() / 0.5;
		multiplied.Should().Be(34.Percent());
	}

	[Test]
	public void _float()
	{
		var multiplied = 17.Percent() / 0.5F;
		multiplied.Should().Be(34.Percent());
	}

	[Test]
	public void _int()
	{
		var multiplied = 17.Percent() / 2;
		multiplied.Should().Be(8.5.Percent());
	}

	[Test]
	public void _uint()
	{
		var multiplied = 17.Percent() / 2U;
		multiplied.Should().Be(8.5.Percent());
	}

	[Test]
	public void _long()
	{
		var multiplied = 17.Percent() / 2L;
		multiplied.Should().Be(8.5.Percent());
	}

	[Test]
	public void _ulong()
	{
		var multiplied = 17.Percent() / 2UL;
		multiplied.Should().Be(8.5.Percent());
	}

	[Test]
	public void _short()
	{
		var multiplied = 17.Percent() / ((short)2);
		multiplied.Should().Be(8.5.Percent());
	}

	[Test]
	public void _ushort()
	{
		var multiplied = 17.Percent() / ((ushort)2);
		multiplied.Should().Be(8.5.Percent());
	}
}

public class Can_be_added_to
{
	[Test]
	public void _percentage()
	{
		var addition = 13.Percent() + 34.Percent();
		addition.Should().Be(47.Percent());
	}

	[Test]
	public void _amount()
	{
		var addition = (Amount)44 + 50.Percent();
		addition.Should().Be((Amount)66);
	}

	[Test]
	public void _money()
	{
		var addition = (44.6 + Currency.EUR) + 50.Percent();
		addition.Should().Be(66.9 + Currency.EUR);
	}

	[Test]
	public void _decimal()
	{
		var addition = 34.586m + 75.Percent();
		addition.Should().Be(60.5255m);
	}

	[Test]
	public void _double()
	{
		var addition = 34.586 + 75.Percent();
		addition.Should().BeApproximately(60.5255, 0.00001);
	}

	[Test]
	public void _float()
	{
		var addition = 34.586f + 75.Percent();
		addition.Should().BeApproximately(60.5255f, 0.00001f);
	}

	[Test]
	public void _int()
	{
		var addition = 400 + 17.Percent();
		addition.Should().Be(468);
	}

	[Test]
	public void _uint()
	{
		var addition = 400U + 17.Percent();
		addition.Should().Be(468U);
	}

	[Test]
	public void _long()
	{
		var addition = 400L + 17.Percent();
		addition.Should().Be(468L);
	}

	[Test]
	public void _ulong()
	{
		var addition = 400UL + 17.Percent();
		addition.Should().Be(468UL);
	}

	[Test]
	public void _short()
	{
		var addition = ((short)400) + 17.Percent();
		addition.Should().Be((short)468);
	}

	[Test]
	public void _ushort()
	{
		var addition = ((ushort)400) + 17.Percent();
		addition.Should().Be((ushort)468);
	}
}

public class Can_be_subtracted_from
{
	[Test]
	public void _percentage()
	{
		var addition = 13.Percent() - 34.Percent();
		addition.Should().Be(-21.Percent());
	}

	[Test]
	public void _amount()
	{
		var addition = (Amount)44.6 - 50.Percent();
		addition.Should().Be((Amount)22.3);
	}

	[Test]
	public void _money()
	{
		var addition = (44.6 + Currency.EUR) - 50.Percent();
		addition.Should().Be(22.3 + Currency.EUR);
	}

	[Test]
	public void _decimal()
	{
		var addition = 34.586m - 75.Percent();
		addition.Should().Be(8.6465m);
	}

	[Test]
	public void _double()
	{
		var addition = 34.586 - 75.Percent();
		addition.Should().BeApproximately(8.6465, 0.00001);
	}

	[Test]
	public void _float()
	{
		var addition = 34.586f - 75.Percent();
		addition.Should().BeApproximately(8.6465f, 0.00001f);
	}

	[Test]
	public void _int()
	{
		var addition = 400 - 17.Percent();
		addition.Should().Be(332);
	}

	[Test]
	public void _uint()
	{
		var addition = 400U - 17.Percent();
		addition.Should().Be(332U);
	}

	[Test]
	public void _long()
	{
		var addition = 400L - 17.Percent();
		addition.Should().Be(332L);
	}

	[Test]
	public void _ulong()
	{
		var addition = 400UL - 17.Percent();
		addition.Should().Be(332UL);
	}

	[Test]
	public void _short()
	{
		var addition = ((short)400) - 17.Percent();
		addition.Should().Be((short)332);
	}

	[Test]
	public void _ushort()
	{
		var addition = ((ushort)400) - 17.Percent();
		addition.Should().Be((ushort)332);
	}
}

public class Can_get_a_percentage_of
{
	[Test]
	public void _percentage()
	{
		var addition = 13.Percent() * 34.Percent();
		addition.Should().Be(4.42.Percent());
	}

	[Test]
	public void _amount()
	{
		var addition = (Amount)44.6 * 80.Percent();
		addition.Should().Be((Amount)35.68);
	}

	[Test]
	public void _money()
	{
		var addition = (44.6 + Currency.EUR) * 80.Percent();
		addition.Should().Be(35.68 + Currency.EUR);
	}

	[Test]
	public void _decimal()
	{
		var addition = 34.586m * 75.Percent();
		addition.Should().Be(25.9395m);
	}

	[Test]
	public void _double()
	{
		var addition = 34.586 * 75.Percent();
		addition.Should().BeApproximately(25.9395, 0.00001);
	}

	[Test]
	public void _float()
	{
		var addition = 34.586f * 75.Percent();
		addition.Should().BeApproximately(25.9395f, 0.00001f);
	}

	[Test]
	public void _int()
	{
		var addition = 400 * 17.Percent();
		addition.Should().Be(68);
	}

	[Test]
	public void _uint()
	{
		var addition = 400U * 17.Percent();
		addition.Should().Be(68U);
	}

	[Test]
	public void _long()
	{
		var addition = 400L * 17.Percent();
		addition.Should().Be(68L);
	}

	[Test]
	public void _ulong()
	{
		var addition = 400UL * 17.Percent();
		addition.Should().Be(68UL);
	}

	[Test]
	public void _short()
	{
		var addition = ((short)400) * 17.Percent();
		addition.Should().Be((short)68);
	}

	[Test]
	public void _ushort()
	{
		var addition = ((ushort)400) * 17.Percent();
		addition.Should().Be((ushort)68);
	}
}

public class Can_get_100_percent_based_on_percentage
{
	[Test]
	public void _percentage()
	{
		var addition = 13.Percent() / 25.Percent();
		addition.Should().Be(52.Percent());
	}

	[Test]
	public void _amount()
	{
		var addition = (Amount)44.6 / 80.Percent();
		addition.Should().Be((Amount)55.75);
	}

	[Test]
	public void _money()
	{
		var addition = (44.6 + Currency.EUR) / 80.Percent();
		addition.Should().Be(55.75 + Currency.EUR);
	}

	[Test]
	public void _decimal()
	{
		var addition = 34.586m / 75.Percent();
		addition.Should().BeApproximately(46.11467m, 0.00001m);
	}

	[Test]
	public void _double()
	{
		var addition = 34.586 / 75.Percent();
		addition.Should().BeApproximately(46.11467, 0.00001);
	}

	[Test]
	public void _float()
	{
		var addition = 34.586f / 75.Percent();
		addition.Should().BeApproximately(46.11467f, 0.00001f);
	}

	[Test]
	public void _int()
	{
		var addition = 400 / 17.Percent();
		addition.Should().Be(2352);
	}

	[Test]
	public void _uint()
	{
		var addition = 400U / 17.Percent();
		addition.Should().Be(2352U);
	}

	[Test]
	public void _long()
	{
		var addition = 400L / 17.Percent();
		addition.Should().Be(2352L);
	}

	[Test]
	public void _ulong()
	{
		var addition = 400UL / 17.Percent();
		addition.Should().Be(2352UL);
	}

	[Test]
	public void _short()
	{
		var addition = ((short)400) / 17.Percent();
		addition.Should().Be((short)2352);
	}

	[Test]
	public void _ushort()
	{
		var addition = ((ushort)400) / 17.Percent();
		addition.Should().Be((ushort)2352);
	}
}

public class Can_be_rounded
{
	[Test]
	public void zero_decimals()
	{
		var actual = Svo.Percentage.Round();
		actual.Should().Be(18.Percent());
	}

	[Test]
	public void one_decimal()
	{
		var actual = Svo.Percentage.Round(1);
		actual.Should().Be(17.5.Percent());
	}

	[Test]
	public void away_from_zero()
	{
		var actual = 16.5.Percent().Round(0, DecimalRounding.AwayFromZero);
		actual.Should().Be(17.Percent());
	}

	[Test]
	public void to_even()
	{
		var actual = 16.5.Percent().Round(0, DecimalRounding.ToEven);
		actual.Should().Be(16.Percent());
	}

	[Test]
	public void to_multiple()
	{
		var actual = 16.4.Percent().RoundToMultiple(3.Percent());
		actual.Should().Be(15.Percent());
	}

	[TestCase(27)]
	[TestCase(28)]
	public void up_to_26_digits(int decimals)
		=> decimals.Invoking(Svo.Percentage.Round)
			.Should().Throw<ArgumentOutOfRangeException>()
			.WithMessage("Percentages can only round to between -26 and 26 digits of precision.*");

	[Test]
	public void up_to_minus_26_digits()
		=> (-27).Invoking(Svo.Percentage.Round)
			.Should().Throw<ArgumentOutOfRangeException>();

	[Test, Obsolete("Only exists for guidance towards decimal rounding methods.")]
	public void using_system_midpoint_rounding()
	{
		var rounded = Svo.Percentage.Round(0, MidpointRounding.AwayFromZero);
		rounded.Should().Be(18.Percent());
	}
}

public class Can_be_increased
{
	[Test]
	public void with_1_percent()
	{
		var increased = Svo.Percentage;
		increased++;
		increased.Should().Be(18.51.Percent());
	}
}

public class Can_be_decreased
{
	[Test]
	public void with_1_percent()
	{
		var decreased = Svo.Percentage;
		decreased--;
		decreased.Should().Be(16.51.Percent());
	}
}

public class Can_be_negated
{
	[TestCase("17.51%", "-17.51%")]
	[TestCase("-17.51%", "17.51%")]
	public void negate(Percentage negated, Percentage input)
		=> (-input).Should().Be(negated);
}

public class Can_be_plussed
{
	[TestCase("-17.51%", "-17.51%")]
	[TestCase("17.51%", "17.51%")]
	public void plus(Percentage plussed, Percentage input)
		=> (+input).Should().Be(plussed);
}


public class Can_get
{
	[TestCase(-1, "-3%")]
	[TestCase(0, "0%")]
	[TestCase(+1, "10%")]
	public void Sign(int expected, Percentage percentage)
	{
		var actual = percentage.Sign();
		actual.Should().Be(expected);
	}

	[TestCase("3%", "-3%")]
	[TestCase("0%", "0%")]
	[TestCase("10%", "10%")]
	public void absolute_value(Percentage expected, Percentage percentage)
	{
		var actual = percentage.Abs();
		actual.Should().Be(expected);
	}
}

public class Can_get_maximum_of
{
	[TestCase("12%", "12%", "5%")]
	[TestCase("15%", "5%", "15%")]
	[TestCase("12%", "12%", "12%")]
	public void two_values(Percentage max, Percentage p0, Percentage p1)
		=> Percentage.Max(p0, p1).Should().Be(max);

	[Test]
	public void multiple_values()
	{
		var max = Percentage.Max(15.Percent(), 66.Percent(), -117.Percent());
		max.Should().Be(66.Percent());
	}
}

public class Can_get_minimum_of
{
	[TestCase("5%", "12%", "5%")]
	[TestCase("5%", "5%", "15%")]
	[TestCase("5%", "5%", "5%")]
	public void two_values(Percentage min, Percentage p0, Percentage p1)
		=> Percentage.Min(p0, p1).Should().Be(min);

	[Test]
	public void multiple_values()
	{
		var min = Percentage.Min(15.Percent(), 66.Percent(), -117.Percent());
		min.Should().Be(-117.Percent());
	}
}

public class Supports_type_conversion
{
	[Test]
	public void via_TypeConverter_registered_with_attribute()
		=> typeof(Percentage).Should().HaveTypeConverterDefined();

	[Test]
	public void from_null_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.FromNull<string>().To<Percentage>().Should().Be(Percentage.Zero);
		}
	}

	[Test]
	public void from_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.From("17.51%").To<Percentage>().Should().Be(Svo.Percentage);
		}
	}

	[Test]
	public void to_string()
	{
		using (TestCultures.en_GB.Scoped())
		{
			Converting.ToString().From(Svo.Percentage).Should().Be("17.51%");
		}
	}

	[Test]
	public void from_int()
		=> Converting.From(-17).To<Percentage>().Should().Be(-1700.Percent());

	[Test]
	public void to_int()
		=> Converting.To<int>().From(1700.Percent()).Should().Be(17);

	[Test]
	public void from_decimal()
		=> Converting.From(0.1751m).To<Percentage>().Should().Be(Svo.Percentage);

	[Test]
	public void to_decimal()
		=> Converting.To<decimal>().From(Svo.Percentage).Should().Be(0.1751m);

	[Test]
	public void from_double()
		=> Converting.From(0.1751).To<Percentage>().Should().Be(Svo.Percentage);

	[Test]
	public void to_double()
		=> Converting.To<double>().From(Svo.Percentage).Should().Be(0.1751);
}

public class Supports_JSON_serialization
{
#if NET6_0_OR_GREATER
	[TestCase("17.51", "17.51%")]
	[TestCase("175.1‰", "17.51%")]
	[TestCase(0.1751, "17.51%")]
	[TestCase(1L, "100%")]
	public void System_Text_JSON_deserialization(object json, Percentage svo)
		=> JsonTester.Read_System_Text_JSON<Percentage>(json).Should().Be(svo);

	[TestCase("17.51%", "17.51%")]
	public void System_Text_JSON_serialization(Percentage svo, object json)
		=> JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
	[TestCase("17.51", "17.51%")]
	[TestCase("175.1‰", "17.51%")]
	[TestCase(0.1751, "17.51%")]
	public void convention_based_deserialization(object json, Percentage svo)
		=> JsonTester.Read<Percentage>(json).Should().Be(svo);

	[TestCase("17.51%", "17.51%")]
	public void convention_based_serialization(Percentage svo, object json)
		=> JsonTester.Write(svo).Should().Be(json);

	[TestCase("Invalid input", typeof(FormatException))]
	public void throws_for_invalid_json(object json, Type exceptionType)
		=> json
			.Invoking(JsonTester.Read<Percentage>)
			.Should().Throw<Exception>()
			.And.Should().BeOfType(exceptionType);
}

public class Supports_XML_serialization
{
	[Test]
	public void using_XmlSerializer_to_serialize()
	{
		var xml = Serialize.Xml(Svo.Percentage);
		xml.Should().Be("17.51%");
	}

	[Test]
	public void using_XmlSerializer_to_deserialize()
	{
		var svo = Deserialize.Xml<Percentage>("17.51%");
		svo.Should().Be(Svo.Percentage);
	}

	[Test]
	public void using_DataContractSerializer()
	{
		var round_tripped = SerializeDeserialize.DataContract(Svo.Percentage);
		round_tripped.Should().Be(Svo.Percentage);
	}

	[Test]
	public void as_part_of_a_structure()
	{
		var structure = XmlStructure.New(Svo.Percentage);
		var round_tripped = SerializeDeserialize.Xml(structure);
		round_tripped.Should().Be(structure);
	}

	[Test]
	public void has_no_custom_XML_schema()
	{
		IXmlSerializable obj = Svo.Percentage;
		obj.GetSchema().Should().BeNull();
	}
}

public class Is_Open_API_data_type
{
	[Test]
	public void with_info()
		 => OpenApiDataType.FromType(typeof(Percentage))
		 .Should().Be(new OpenApiDataType(
			 dataType: typeof(Percentage),
			 description: "Ratio expressed as a fraction of 100 denoted using the percent sign '%'.",
			 type: "string",
			 example: "13.76%",
			 format: "percentage",
			 pattern: @"-?[0-9]+(\.[0-9]+)?%"));

	[TestCase("17.51%")]
	[TestCase("-4.1%")]
	[TestCase("-0.1%")]
	[TestCase("31%")]
	public void pattern_matches(string input)
		=> OpenApiDataType.FromType(typeof(Percentage))!.Matches(input).Should().BeTrue();
}

#if NET8_0_OR_GREATER
#else
public class Supports_binary_serialization
{
	[Test]
	[Obsolete("Usage of the binary formatter is considered harmful.")]
	public void using_BinaryFormatter()
	{
		var round_tripped = SerializeDeserialize.Binary(Svo.Percentage);
		round_tripped.Should().Be(Svo.Percentage);
	}

	[Test]
	public void storing_decimal_in_SerializationInfo()
	{
		var info = Serialize.GetInfo(Svo.Percentage);
		info.GetDecimal("Value").Should().Be(0.1751m);
	}
}
#endif

public class Debugger
{
	[TestCase("17.51%", "17.51%")]
	public void has_custom_display(object display, Percentage svo)
		=> svo.Should().HaveDebuggerDisplay(display);
}
