namespace Financial.Amount_specs;

public class Has_constant
{
    [Test]
    public void MinValue_equal_to_decimal_MinValue() => Amount.MinValue.Should().Be(decimal.MinValue.Amount());

    [Test]
    public void MaxValue_equal_to_decimal_MaxValue() => Amount.MaxValue.Should().Be(decimal.MaxValue.Amount());
}

public class Min_and_max
{
    [TestCase(17, 17)]
    [TestCase(17, 18)]
    [TestCase(18, 17)]
    public void Min_of_two_returns_minimum(Amount a1, Amount a2)
        => Amount.Min(a1, a2).Should().Be(17.Amount());

    [Test]
    public void Min_of_collection_returns_minimum()
        => Amount.Min(700.Amount(), 17.Amount(), 48.Amount()).Should().Be(17.Amount());

    [TestCase(17, 17)]
    [TestCase(17, 16)]
    [TestCase(16, 17)]
    public void Max_of_two_returns_minimum(Amount a1, Amount a2)
        => Amount.Max(a1, a2).Should().Be(17.Amount());

    [Test]
    public void Max_of_collection_returns_maximum()
        => Amount.Max(7.Amount(), 17.Amount(), -48.Amount()).Should().Be(17.Amount());
}

public class Can_not_be_parsed
{
    [TestCase(NumberStyles.Number)]
    [TestCase(NumberStyles.Integer)]
    public void strings_with_currency_if_style_does_not_allow_currency_sign(NumberStyles style)
        => Amount.TryParse("42 EUR", style, CultureInfo.InvariantCulture, out _)
            .Should().BeFalse();

    [TestCase(NumberStyles.HexNumber)]
    [TestCase(NumberStyles.AllowExponent)]
    public void using_a_number_style_other_then_Curency(NumberStyles style)
         => style.Invoking(s => Amount.TryParse("4.50", s, CultureInfo.InvariantCulture, out _))
             .Should().Throw<ArgumentOutOfRangeException>()
             .WithMessage("The number style '*' is not supported.*");
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.Amount.CompareTo(Nil.Object).Should().Be(1);
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
       => typeof(Amount).Should().HaveTypeConverterDefined();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<Amount>().Should().Be(Amount.Zero);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("42.17").To<Amount>().Should().Be(Svo.Amount);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.To<string>().From(Svo.Amount).Should().Be("42.17");
        }
    }

    [Test]
    public void from_decimal()
        => Converting.From(42.17m).To<Amount>().Should().Be(Svo.Amount);

    [Test]
    public void from_double()
        => Converting.From(42.17).To<Amount>().Should().Be(Svo.Amount);

    [Test]
    public void to_decimal()
        => Converting.To<decimal>().From(Svo.Amount).Should().Be(42.17m);

    [Test]
    public void to_double()
        => Converting.To<double>().From(Svo.Amount).Should().Be(42.17);


    public class Supports_JSON_serialization
    {
#if NET6_0_OR_GREATER
        [TestCase("1234.56", 1234.56)]
        [TestCase(1234.56, 1234.56)]
        [TestCase(1234L, 1234.00)]
        public void System_Text_JSON_deserialization(object json, Amount svo)
            => JsonTester.Read_System_Text_JSON<Amount>(json).Should().Be(svo);

        [Test]
        public void System_Text_JSON_deserialization_min_value()
        {
            var amount = System.Text.Json.JsonSerializer.Deserialize<Amount>("-7.922816251426434E+28");
            amount.Should().Be(Amount.MinValue);
        }

        [TestCase("7.922816251426434E+28")]
        [TestCase("79228162514264337593543950335")]
        public void System_Text_JSON_deserialization_max_value(string json)
        {
            var amount = System.Text.Json.JsonSerializer.Deserialize<Amount>(json);
            amount.Should().Be(Amount.MaxValue);
        }

        [TestCase(1234.56, 1234.56)]
        public void System_Text_JSON_serialization(Amount svo, object json)
            => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);

        [Test]
        public void System_Text_JSON_serialization_Max_value()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(Amount.MaxValue);
            json.Should().Be("79228162514264337593543950335");
        }
#endif
        [TestCase("1234.56", 1234.56)]
        [TestCase(1234.56, 1234.56)]
        [TestCase(1234L, 1234.00)]
        public void convention_based_deserialization(object json, Amount svo)
          => JsonTester.Read<Amount>(json).Should().Be(svo);

        [TestCase(1234.56, 1234.56)]
        public void convention_based_serialization(Amount svo, object json)
            => JsonTester.Write(svo).Should().Be(json);

        [Test]
        public void convention_based_serialization_max_value()
            => JsonTester.Write(Amount.MaxValue).Should().Be(79228162514264337593543950335M);

        [TestCase("Invalid input", typeof(FormatException))]
        [TestCase("2017-06-11", typeof(FormatException))]
        public void throws_for_invalid_json(object json, Type exceptionType)
            => json
                .Invoking(JsonTester.Read<Amount>)
                .Should().Throw<Exception>()
                .And.Should().BeOfType(exceptionType);
    }

    public class Casts_explicit
    {
        public class From
        {
            [Test]
            public void Int32() => ((Amount)42).Should().Be(42.Amount());

            [Test]
            public void Int64() => ((Amount)42L).Should().Be(42.Amount());

            [Test]
            public void Decimal() => ((Amount)42.0m).Should().Be(42.Amount());

            [Test]
            public void Double() => ((Amount)42.0).Should().Be(42.Amount());
        }

        public class To
        {
            [Test]
            public void Int32() => ((int)42.Amount()).Should().Be(42);

            [Test]
            public void Int64() => ((long)42.Amount()).Should().Be(42L);

            [Test]
            public void Decimal() => ((decimal)42.Amount()).Should().Be(42m);

            [Test]
            public void Double() => ((double)42.Amount()).Should().Be(42.0);
        }
    }
    public class Has_operators
    {
        [Test]
        public void to_divide_amount_by_amount_as_decimal()
        {
            var ratio = Svo.Amount / 2.Amount();
            ratio.Should().Be(21.085m);
        }
    }
}
