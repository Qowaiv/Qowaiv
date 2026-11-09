using FluentValidation;
using Qowaiv.Validation.Fluent;
using Specs.Fluent.Models;

namespace Fluent_validation.NotInFuture_specs;

public class Valid_for_not_in_future
{
    [Test]
    public void Date()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateModel { Prop = new Date(2017, 06, 10) }
                .ValidateWith(new DateNotInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateOnlyModel { Prop = new DateOnly(2017, 06, 10) }
                .ValidateWith(new DateOnlyNotInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateTimeModel { Prop = new DateTime(2017, 06, 10, 00, 00, 00, DateTimeKind.Utc) }
                .ValidateWith(new DateTimeNotInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_Date()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateModel { Prop = new Date(2017, 06, 10) }
                .ValidateWith(new NullableDateNotInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateOnlyModel { Prop = new Date(2017, 06, 10) }
                .ValidateWith(new NullableDateOnlyNotInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateTimeModel { Prop = new Date(2017, 06, 10) }
                .ValidateWith(new NullableDateTimeNotInFutureValidator())
                .Should().BeValid();
        }
    }
}

public class Not_invalid_for_not_set
{ 
    [Test]
    public void Date()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateModel { Prop = null }
                .ValidateWith(new NullableDateNotInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateOnlyModel { Prop = null }
                .ValidateWith(new NullableDateOnlyNotInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateTimeModel { Prop = null }
                .ValidateWith(new NullableDateTimeNotInFutureValidator())
                .Should().BeValid();
        }
    }
}

public class Invalid_for_in_future
{
    [Test]
    [Arguments("'Prop' mag niet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the future.", "en-GB")]
    public void Date(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateModel { Prop = new Date(2017, 06, 12) }
                    .ValidateWith(new DateNotInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' mag niet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the future.", "en-GB")]
    public void DateOnly(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateOnlyModel { Prop = new Date(2017, 06, 12) }
                    .ValidateWith(new DateOnlyNotInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' mag niet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the future.", "en-GB")]
    public void DateTime(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateTimeModel { Prop = new Date(2017, 06, 12) }
                    .ValidateWith(new DateTimeNotInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' mag niet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the future.", "en-GB")]
    public void Nullable_Date(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                 new NullableDateModel { Prop = new Date(2017, 06, 12) }
                    .ValidateWith(new NullableDateNotInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' mag niet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the future.", "en-GB")]
    public void Nullable_DateOnly(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateOnlyModel { Prop = new Date(2017, 06, 12) }
                    .ValidateWith(new NullableDateOnlyNotInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Arguments("'Prop' mag niet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the future.", "en-GB")]
    public void Nullable_DateTime(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateTimeModel { Prop = new Date(2017, 06, 12) }
                    .ValidateWith(new NullableDateTimeNotInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }
}

public class DateNotInFutureValidator : ModelValidator<DateModel>
{
    public DateNotInFutureValidator() => RuleFor(m => m.Prop).NotInFuture();
}

public class DateOnlyNotInFutureValidator : ModelValidator<DateOnlyModel>
{
    public DateOnlyNotInFutureValidator() => RuleFor(m => m.Prop).NotInFuture();
}

public class DateTimeNotInFutureValidator : ModelValidator<DateTimeModel>
{
    public DateTimeNotInFutureValidator() => RuleFor(m => m.Prop).NotInFuture();
}

public class NullableDateNotInFutureValidator : ModelValidator<NullableDateModel>
{
    public NullableDateNotInFutureValidator() => RuleFor(m => m.Prop).NotInFuture();
}

public class NullableDateOnlyNotInFutureValidator : ModelValidator<NullableDateOnlyModel>
{
    public NullableDateOnlyNotInFutureValidator() => RuleFor(m => m.Prop).NotInFuture();
}

public class NullableDateTimeNotInFutureValidator : ModelValidator<NullableDateTimeModel>
{
    public NullableDateTimeNotInFutureValidator() => RuleFor(m => m.Prop).NotInFuture();
}
