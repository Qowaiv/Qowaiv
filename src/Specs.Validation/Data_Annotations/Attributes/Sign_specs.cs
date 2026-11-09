using Qowaiv.Validation.DataAnnotations;

namespace Data_annotations.Attributes.Sign_specs;

public class Positive
{
    public class Is_valid_for
    {
        [Test]
        public void Null() => new PositiveAttribute().IsValid(null).Should().BeTrue();

        [Arguments(0.001)]
        [Arguments(1)]
        [Arguments(42)]
        [Arguments(int.MaxValue)]
        [Arguments(0.1f)]
        [Arguments(42.0)]
        [Arguments(double.MaxValue)]
        [Arguments((sbyte)1)]
        [Arguments((short)1)]
        [Arguments((short)32767)]
        [Arguments((long)1)]
        [Arguments(float.PositiveInfinity)]
        [Arguments(double.PositiveInfinity)]
        public void positive_numbers(object value)
            => new PositiveAttribute().IsValid(value).Should().BeTrue();
    }

    public class Is_invalid_for
    {
        [Arguments(0)]
        [Arguments(-1)]
        [Arguments(-42)]
        [Arguments(int.MinValue)]
        [Arguments(0.0f)]
        [Arguments(-0.1f)]
        [Arguments(-42.0)]
        [Arguments((sbyte)-1)]
        [Arguments((short)-1)]
        [Arguments((long)-1)]
        [Arguments(float.NaN)]
        [Arguments(double.NaN)]
        [Arguments(float.NegativeInfinity)]
        [Arguments(double.NegativeInfinity)]
        public void zero_or_negative_numbers(object value)
            => new PositiveAttribute().IsValid(value).Should().BeFalse();

        [Arguments("Hello")]
        [Arguments(true)]
        public void non_numeric_types(object value)
            => value.Invoking(_ => new PositiveAttribute().IsValid(value))
            .Should().Throw<UnsupportedType>();
    }

    public class With_message
    {
        [Arguments("nl", "De waarde van het Quantity veld moet positief zijn.")]
        [Arguments("en", "The value of the Quantity field must be positive.")]
        public void culture_dependent(CultureInfo culture, string message)
        {
            using var _ = culture.Scoped();
            new Model().ValidateWith(new AnnotatedModelValidator<Model>())
                .Should().BeInvalid()
                .WithMessage(ValidationMessage.Error(message, "Quantity"));
        }

        internal class Model
        {
            [Positive]
            public int Quantity { get; set; } = 0;
        }
    }
}

public class NotPositive
{
    public class Is_valid_for
    {
        [Test]
        public void Null() => new NotPositiveAttribute().IsValid(null).Should().BeTrue();

        [Arguments(0)]
        [Arguments(-1)]
        [Arguments(-42)]
        [Arguments(int.MinValue)]
        [Arguments(0.0f)]
        [Arguments(-0.1f)]
        [Arguments(-42.0)]
        [Arguments((sbyte)-1)]
        [Arguments((sbyte)0)]
        [Arguments((short)-1)]
        [Arguments((short)0)]
        [Arguments((long)-1)]
        [Arguments((long)0)]
        [Arguments(float.NegativeInfinity)]
        [Arguments(double.NegativeInfinity)]
        public void zero_or_negative_numbers(object value)
            => new NotPositiveAttribute().IsValid(value).Should().BeTrue();
    }

    public class Is_invalid_for
    {
        [Arguments(0.001)]
        [Arguments(1)]
        [Arguments(42)]
        [Arguments(int.MaxValue)]
        [Arguments(0.1f)]
        [Arguments(42.0)]
        [Arguments(double.MaxValue)]
        [Arguments((sbyte)1)]
        [Arguments((short)1)]
        [Arguments((short)32767)]
        [Arguments((long)1)]
        [Arguments(float.NaN)]
        [Arguments(double.NaN)]
        [Arguments(float.PositiveInfinity)]
        [Arguments(double.PositiveInfinity)]
        public void positive_numbers(object value)
            => new NotPositiveAttribute().IsValid(value).Should().BeFalse();

        [Arguments("Hello")]
        [Arguments(true)]
        public void non_numeric_types(object value)
            => value.Invoking(_ => new NotPositiveAttribute().IsValid(value))
            .Should().Throw<UnsupportedType>();
    }

    public class With_message
    {
        [Arguments("nl", "De waarde van het Adjustment veld mag niet positief zijn.")]
        [Arguments("en", "The value of the Adjustment field must not be positive.")]
        public void culture_dependent(CultureInfo culture, string message)
        {
            using var _ = culture.Scoped();
            new Model().ValidateWith(new AnnotatedModelValidator<Model>())
                .Should().BeInvalid()
                .WithMessage(ValidationMessage.Error(message, "Adjustment"));
        }

        internal class Model
        {
            [NotPositive]
            public int Adjustment { get; set; } = 1;
        }
    }
}

public class Negative
{
    public class Is_valid_for
    {
        [Test]
        public void Null() => new NegativeAttribute().IsValid(null).Should().BeTrue();

        [Arguments(-0.001)]
        [Arguments(-1)]
        [Arguments(-42)]
        [Arguments(int.MinValue)]
        [Arguments(-0.1f)]
        [Arguments(-42.0)]
        [Arguments(-double.MaxValue)]
        [Arguments((sbyte)-1)]
        [Arguments((sbyte)-128)]
        [Arguments((short)-1)]
        [Arguments((short)-32768)]
        [Arguments((long)-1)]
        [Arguments(float.NegativeInfinity)]
        [Arguments(double.NegativeInfinity)]
        public void negative_numbers(object value)
            => new NegativeAttribute().IsValid(value).Should().BeTrue();
    }

    public class Is_invalid_for
    {
        [Arguments(0)]
        [Arguments(0.001)]
        [Arguments(1)]
        [Arguments(42)]
        [Arguments(int.MaxValue)]
        [Arguments(0.0f)]
        [Arguments(0.1f)]
        [Arguments(42.0)]
        [Arguments(double.MaxValue)]
        [Arguments((sbyte)0)]
        [Arguments((sbyte)1)]
        [Arguments((short)0)]
        [Arguments((short)1)]
        [Arguments((long)0)]
        [Arguments((long)1)]
        [Arguments(float.NaN)]
        [Arguments(double.NaN)]
        [Arguments(float.PositiveInfinity)]
        [Arguments(double.PositiveInfinity)]
        public void zero_or_positive_numbers(object value)
            => new NegativeAttribute().IsValid(value).Should().BeFalse();

        [Arguments("Hello")]
        [Arguments(true)]
        public void non_numeric_types(object value)
            => value.Invoking(_ => new NegativeAttribute().IsValid(value))
            .Should().Throw<UnsupportedType>();
    }

    public class With_message
    {
        [Arguments("nl", "De waarde van het Temperature veld moet negatief zijn.")]
        [Arguments("en", "The value of the Temperature field must be negative.")]
        public void culture_dependent(CultureInfo culture, string message)
        {
            using var _ = culture.Scoped();
            new Model().ValidateWith(new AnnotatedModelValidator<Model>())
                .Should().BeInvalid()
                .WithMessage(ValidationMessage.Error(message, "Temperature"));
        }

        internal class Model
        {
            [Negative]
            public int Temperature { get; set; } = 0;
        }
    }
}

public class NotNegative
{
    public class Is_valid_for
    {
        [Test]
        public void Null() => new NotNegativeAttribute().IsValid(null).Should().BeTrue();

        [Arguments(0)]
        [Arguments(0.001)]
        [Arguments(1)]
        [Arguments(42)]
        [Arguments(int.MaxValue)]
        [Arguments(0.0f)]
        [Arguments(0.1f)]
        [Arguments(42.0)]
        [Arguments(double.MaxValue)]
        [Arguments((sbyte)0)]
        [Arguments((sbyte)1)]
        [Arguments((short)0)]
        [Arguments((short)1)]
        [Arguments((short)32767)]
        [Arguments((long)0)]
        [Arguments((long)1)]
        [Arguments(float.PositiveInfinity)]
        [Arguments(double.PositiveInfinity)]

        public void zero_or_positive_numbers(object value)
            => new NotNegativeAttribute().IsValid(value).Should().BeTrue();
    }

    public class Is_invalid_for
    {
        [Arguments(-0.001)]
        [Arguments(-1)]
        [Arguments(-42)]
        [Arguments(int.MinValue)]
        [Arguments(-0.1f)]
        [Arguments(-42.0)]
        [Arguments(-double.MaxValue)]
        [Arguments((sbyte)-1)]
        [Arguments((sbyte)-128)]
        [Arguments((short)-1)]
        [Arguments((short)-32768)]
        [Arguments((long)-1)]
        [Arguments(float.NaN)]
        [Arguments(double.NaN)]
        [Arguments(float.NegativeInfinity)]
        [Arguments(double.NegativeInfinity)]
        public void negative_numbers(object value)
            => new NotNegativeAttribute().IsValid(value).Should().BeFalse();

        [Arguments("Hello")]
        [Arguments(true)]
        public void non_numeric_types(object value)
            => value.Invoking(_ => new NotNegativeAttribute().IsValid(value))
            .Should().Throw<UnsupportedType>();
    }

    public class With_message
    {
        [Arguments("nl", "De waarde van het Count veld mag niet negatief zijn.")]
        [Arguments("en", "The value of the Count field must not be negative.")]
        public void culture_dependent(CultureInfo culture, string message)
        {
            using var _ = culture.Scoped();
            new Model().ValidateWith(new AnnotatedModelValidator<Model>())
                .Should().BeInvalid()
                .WithMessage(ValidationMessage.Error(message, "Count"));
        }

        internal class Model
        {
            [NotNegative]
            public int Count { get; set; } = -1;
        }
    }
}
