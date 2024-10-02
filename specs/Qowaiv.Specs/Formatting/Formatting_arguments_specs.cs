namespace Formatting_arguments_specs;

public class Is_equal_by_value
{
    [Test]
    public void hash_code_is_value_based_for_none()
    {
        using (Hash.WithoutRandomizer())
        {
            FormattingArguments.None.GetHashCode().Should().Be(0);
        }
    }

    [Test]
    public void hash_code_is_value_based_for_value()
    {
        using (Hash.WithoutRandomizer())
        {
            new FormattingArguments("f", null).GetHashCode().Should().Be(-362413457);
        }
    }
}

public class ToString
{
    [Test]
    public void applies_format_of_arguments_when_applicable()
        => Svo.FormattingArguments.ToString(7).Should().Be("7,000");

    [Test]
    public void ignores_format_of_arguments_when_not_applicable()
        => Svo.FormattingArguments.ToString(typeof(int)).Should().Be("System.Int32");

    [Test]
    public void uses_current_culture_when_not_specified()
    {
        using var _ = TestCultures.en_GB.Scoped();

        var arguments = new FormattingArguments("0.000");
        arguments.ToString(7).Should().Be("7.000");
    }

    [Test]
    public void null_for_null_object()
        => Svo.FormattingArguments.ToString(Nil.Object).Should().BeNull();

    [Test]
    public void null_for_null_IFormattable()
        => Svo.FormattingArguments.ToString(Nil.IFormattable).Should().BeNull();
}

public class IFormattable_ToString_extension
{
    [Test]
    public void applies_format_of_arguments_when_applicable()
        => 123.45.ToString(Svo.FormattingArguments).Should().Be("123,450");

    [Test]
    public void ToString_DecimalWithFormatCollection_FormattedString()
    {
        using (TestCultures.es_EC.Scoped())
        {
            var collection = new FormattingArgumentsCollection
            {
                { typeof(decimal), "0.000" },
            };

            123.45m.ToString(collection).Should().Be("123,450");
        }
    }

    [Test]
    public void ToString_DecimalWithNullCollection_FormattedString()
    {
        using var _ = TestCultures.es_EC.Scoped();

        123.45m.ToString(Nil.FormattingArgumentsCollection).Should().Be("123,45");
    }

    [Test]
    public void null_for_null_IFormattable_with_arguments()
        => Nil.IFormattable.ToString(Svo.FormattingArguments).Should().BeNull();

    [Test]
    public void null_for_null_IFormattable_with_arguments_collection()
        => Nil.IFormattable.ToString([]).Should().BeNull();

    [Test]
    public void null_for_null_IFormattable_with_null_arguments_collection()
        => Nil.IFormattable.ToString(Nil.FormattingArgumentsCollection).Should().BeNull();
}
