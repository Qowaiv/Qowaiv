using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Attributes.Size_specs;

public class At_least
{
    public class Supports
    {
        [Test]
        public void Stream() => new Size.AtLeastAttribute(2).IsValid(System.IO.Stream.Null).Should().BeTrue();

        [Test]
        public void ICollection_Byte() => new Size.AtLeastAttribute(2).IsValid(Array.Empty<byte>()).Should().BeTrue();

        [Test]
        public void IReadOnlyCollection_Byte() => new Size.AtLeastAttribute(2).IsValid(new ReadOnlyByteArray()).Should().BeTrue();

        [Test]
        public void BinaryData() => new Size.AtLeastAttribute(2).IsValid(new BinaryData([1, 2])).Should().BeTrue();
    }

    public class Does_not_support
    {
        [Test]
        public void type_without_count_property()
            => new Size.AtLeastAttribute(4).Invoking(a => a.IsValid(new object()))
            .Should().Throw<UnsupportedType>().WithMessage("Size.AtLeastAttribute does not support the type object.");
    }

    public class Ignores
    {
        [Test]
        public void _null() => new Size.AtLeastAttribute(0).IsValid(null).Should().BeTrue();

        [Test]
        public void length_zero() => new Size.AtLeastAttribute(4).IsValid(System.IO.Stream.Null).Should().BeTrue();
    }

    [Test]
    [MatrixDataSource]
    public void validates([MatrixRange<int>(4, 10)] int value) => new Size.AtLeastAttribute(4).IsValid(new byte[value]).Should().BeTrue();

    [Test]
    [MatrixDataSource]
    public void invalidates([MatrixRange<int>(1, 10)] int value) => new Size.AtLeastAttribute(11).IsValid(new byte[value]).Should().BeFalse();

    [Test]
    [Arguments("nl", "De grootte van het veld AtLeastProp moet minstens 4 byte zijn.")]
    [Arguments("en", "The size of the AtLeastProp field should be at least 4 byte.")]
    public void with_message(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();
        new Model() {AtLeastProp = new([1, 2]) }.ValidateWith(new AnnotatedModelValidator<Model>())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "AtLeastProp"));
    }
}

public class At_most
{
    public class Supports
    {
        [Test]
        public void Stream() => new Size.AtMostAttribute(2).IsValid(System.IO.Stream.Null).Should().BeTrue();

        [Test]
        public void ICollection_Byte() => new Size.AtMostAttribute(2).IsValid(Array.Empty<byte>()).Should().BeTrue();

        [Test]
        public void IReadOnlyCollection_Byte() => new Size.AtMostAttribute(2).IsValid(new ReadOnlyByteArray()).Should().BeTrue();

        [Test]
        public void BinaryData() => new Size.AtMostAttribute(2).IsValid(new BinaryData([1, 2])).Should().BeTrue();
    }

    public class Does_not_support
    {
        [Test]
        public void type_without_count_property()
            => new Size.AtMostAttribute(4).Invoking(a => a.IsValid(new object()))
            .Should().Throw<UnsupportedType>().WithMessage("Size.AtMostAttribute does not support the type object.");
    }

    public class Ignores
    {
        [Test]
        public void _null() => new Size.AtMostAttribute(0).IsValid(null).Should().BeTrue();
    }

    [Test]
    [MatrixDataSource]
    public void validates([MatrixRange<int>(1, 4)] int value) => new Size.AtMostAttribute(4).IsValid(new byte[value]).Should().BeTrue();

    [Test]
    [MatrixDataSource]
    public void invalidates([MatrixRange<int>(12, 20)] int value) => new Size.AtMostAttribute(11).IsValid(new byte[value]).Should().BeFalse();

    [Test]
    [Arguments("nl", "De grootte van het veld AtMostProp mag niet meer dan 4 byte zijn.")]
    [Arguments("en", "The size of the AtMostProp field should be at most 4 byte.")]
    public void with_message(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();
        new Model() { AtMostProp = new([1, 2, 3, 4, 5]) }.ValidateWith(new AnnotatedModelValidator<Model>())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "AtMostProp"));
    }
}

public class In_range
{
    public class Supports
    {
        [Test]
        public void Stream() => new Size.InRangeAttribute(2, 4).IsValid(System.IO.Stream.Null).Should().BeTrue();

        [Test]
        public void ICollection_Byte() => new Size.InRangeAttribute(2, 4).IsValid(Array.Empty<byte>()).Should().BeTrue();

        [Test]
        public void IReadOnlyCollection_Byte() => new Size.InRangeAttribute(2, 4).IsValid(new ReadOnlyByteArray()).Should().BeTrue();

        [Test]
        public void BinaryData() => new Size.InRangeAttribute(2, 4).IsValid(new BinaryData([1, 2])).Should().BeTrue();
    }

    public class Does_not_support
    {
        [Test]
        public void type_without_count_property()
            => new Size.InRangeAttribute(1, 4).Invoking(a => a.IsValid(new object()))
            .Should().Throw<UnsupportedType>().WithMessage("Size.InRangeAttribute does not support the type object.");
    }


    public class Ignores
    {
        [Test]
        public void _null() => new Size.InRangeAttribute(2, 4).IsValid(null).Should().BeTrue();

        [Test]
        public void length_zero() => new Size.InRangeAttribute(2, 4).IsValid(System.IO.Stream.Null).Should().BeTrue();
    }


    [Test]
    [MatrixDataSource]
    public void validates([MatrixRange<int>(4, 10)] int value) => new Size.InRangeAttribute(4, 10).IsValid(new byte[value]).Should().BeTrue();

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(5)]
    [Arguments(6)]
    [Arguments(7)]
    public void invalidates(int value) => new Size.InRangeAttribute(3, 4).IsValid(new byte[value]).Should().BeFalse();

    [Test]
    [Arguments("nl", "De grootte van het veld InRangeProp moet tussen de 3 byte en 4 byte zijn.")]
    [Arguments("en", "The size of the InRangeProp field should be between 3 byte and 4 byte.")]
    public void with_message(CultureInfo culture, string message)
    {
        using var _ = culture.Scoped();
        new Model() { InRangeProp = new([1, 2]) }.ValidateWith(new AnnotatedModelValidator<Model>())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error(message, "InRangeProp"));
    }
}

file sealed class Model
{
    [Size.InRange(3, 4)]
    public BinaryData InRangeProp { get; init; } = new([1, 2, 3, 4]);

    [Size.AtLeast(4)]
    public BinaryData AtLeastProp { get; init; } = new([1, 2, 3, 4]);

    [Size.AtMost(4)]
    public BinaryData AtMostProp { get; init; } = new([1, 2, 3, 4]);
}

file sealed class ReadOnlyByteArray : IReadOnlyCollection<byte>
{
    public int Count => 0;

    public IEnumerator<byte> GetEnumerator(){ yield break; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
