namespace Unknown_specs;

public class Is_unknown
{
    [Test]
    public void question_mark_with_culture()
    {
        Unknown.IsUnknown("?", new CultureInfo("nl-NL")).Should().BeTrue();
    }

    [Test]
    public void question_mark_without_culture()
    {
        Unknown.IsUnknown("?", null).Should().BeTrue();
    }

    [Test]
    public void language_specific_unknown_for_specific_culture()
    {
        Unknown.IsUnknown("Não SABe", new CultureInfo("pt-PT")).Should().BeTrue();
    }
}

public class Is_not_unknown
{
    [Test]
    public void Null()
    {
        Unknown.IsUnknown(null).Should().BeFalse();
    }

    [Test]
    public void string_empty()
    {
        Unknown.IsUnknown(string.Empty).Should().BeFalse();
    }

    [Test]
    public void string_not_representing_unknown_for_specified_culture()
    {
        Unknown.IsUnknown("onbekend", CultureInfo.InvariantCulture).Should().BeFalse();
    }
}

public class Can_be_resolved_for
{
    [Test]
    public void value_type_with_unknown_value()
    {
        Unknown.Value(typeof(PostalCode)).Should().Be(PostalCode.Unknown);
    }
    [Test]
    public void reference_type_with_unknown_value()
    {
        Unknown.Value(typeof(ClassWithUnknownValue)).Should().Be(ClassWithUnknownValue.Unknown);
    }

    private class ClassWithUnknownValue
    {
        public static ClassWithUnknownValue Unknown { get; } = new ClassWithUnknownValue();
    }
}
public class Can_not_be_resolved_for
{
    [Test]
    public void null_type()
        => Unknown.Value(null).Should().BeNull();

    [Test]
    public void value_type_without_unknown_value()
        => Unknown.Value(typeof(Uuid)).Should().BeNull();

    [Test]
    public void value_type_with_unknown_value_of_wrong_type()
        => Unknown.Value(typeof(ClassWithUnknownValueOfWrongType)).Should().BeNull();

    [Test]
    public void reference_type_without_unknown_value()
        => Unknown.Value(typeof(object)).Should().BeNull();

    private class ClassWithUnknownValueOfWrongType
    {
        private ClassWithUnknownValueOfWrongType() { }
        public static object Unknown { get; } = new object();
    }
}
