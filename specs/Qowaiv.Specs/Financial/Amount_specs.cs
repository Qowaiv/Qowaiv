namespace Financial.Amount_specs;

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
        using (TestCultures.En_GB.Scoped())
        {
            Converting.FromNull<string>().To<Amount>().Should().Be(Amount.Zero);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.En_GB.Scoped())
        {
            Converting.From("42.17").To<Amount>().Should().Be(Svo.Amount);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.En_GB.Scoped())
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

        [TestCase(1234.56, 1234.56)]
        public void System_Text_JSON_serialization(Amount svo, object json)
            => JsonTester.Write_System_Text_JSON(svo).Should().Be(json);
#endif
        [TestCase("1234.56", 1234.56)]
        [TestCase(1234.56, 1234.56)]
        [TestCase(1234L, 1234.00)]
        public void convention_based_deserialization(object json, Amount svo)
          => JsonTester.Read<Amount>(json).Should().Be(svo);

        [TestCase(1234.56, 1234.56)]
        public void convention_based_serialization(Amount svo, object json)
            => JsonTester.Write(svo).Should().Be(json);

        [TestCase("Invalid input", typeof(FormatException))]
        [TestCase("2017-06-11", typeof(FormatException))]
        public void throws_for_invalid_json(object json, Type exceptionType)
        {
            var exception = Assert.Catch(() => JsonTester.Read<Amount>(json));
            Assert.IsInstanceOf(exceptionType, exception);
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
