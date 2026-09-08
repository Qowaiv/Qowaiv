namespace Formatting_arguments_specs;

public class Is_equal_by_value
{
    [Test]
    public void not_equal_to_null()
        => Svo.FormattingArguments.Equals(null).Should().BeFalse();

    [Test]
    public void not_equal_to_other_type()
        => Svo.FormattingArguments.Equals(new object()).Should().BeFalse();

    [Test]
    public void not_equal_to_different_value()
        => Svo.FormattingArguments.Equals(FormattingArguments.None).Should().BeFalse();

    [Test]
    public void equal_to_same_value()
        => Svo.FormattingArguments.Equals(new FormattingArguments("0.000", TestCultures.fr_BE)).Should().BeTrue();

    [Test]
    public void equal_operator_returns_true_for_same_values()
        => (new FormattingArguments("0.000", TestCultures.fr_BE) == Svo.FormattingArguments).Should().BeTrue();

    [Test]
    public void equal_operator_returns_false_for_different_values()
        => (new FormattingArguments("0.000", TestCultures.fr_BE) == FormattingArguments.None).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_false_for_same_values()
        => (new FormattingArguments("0.000", TestCultures.fr_BE) != Svo.FormattingArguments).Should().BeFalse();

    [Test]
    public void not_equal_operator_returns_true_for_different_values()
        => (new FormattingArguments("0.000", TestCultures.fr_BE) != FormattingArguments.None).Should().BeTrue();

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

public class Can_be_deconstructed
{
    [Test]
    public void in_format_and_formatProvider_part()
    {
        var (format, formatProvider) = Svo.FormattingArguments;
        format.Should().Be("0.000");
        formatProvider.Should().Be(TestCultures.fr_BE);
    }
}

public class Has_constant
{
    [Test]
    public void None_equals_default()
        => FormattingArguments.None.Should().Be(default(FormattingArguments));
}

public class Has_properties
{
    [Test]
    public void format_of_default_is_null()
        => FormattingArguments.None.Format.Should().BeNull();

    [Test]
    public void format_of_TestStruct_is_format_string()
        => Svo.FormattingArguments.Format.Should().Be("0.000");
}

public class Supports_XML_serialization
{
    [Test]
    public void as_part_of_a_structure()
    {
        var structure = new XmlStructure<FormattingArguments>()
        {
            Id = 17,
            Svo = FormattingArguments.None,
            Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
        };
        var round_tripped = SerializeDeserialize.Xml(structure);
        round_tripped.Should().Be(structure);
    }
}
