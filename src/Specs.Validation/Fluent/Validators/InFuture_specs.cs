using FluentValidation;
using Qowaiv.Validation.Fluent;
using Specs.Fluent.Models;

namespace Fluent_validation.InFuture_specs;

public class Valid_for_in_future
{
    [Test]
    public void Date()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateModel { Prop = new Date(2017, 06, 12) }
                .ValidateWith(new DateInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateOnlyModel { Prop = new DateOnly(2017, 06, 12) }
                .ValidateWith(new DateOnlyInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateTimeModel { Prop = new DateTime(2017, 06, 12, 00, 00, 00, DateTimeKind.Utc) }
                .ValidateWith(new DateTimeInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_Date()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateModel { Prop = new Date(2017, 06, 12) }
                .ValidateWith(new NullableDateInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateOnlyModel { Prop = new DateOnly(2017, 06, 12) }
                .ValidateWith(new NullableDateOnlyInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateTimeModel { Prop = new DateTime(2017, 06, 12, 00, 00, 00, DateTimeKind.Utc) }
                .ValidateWith(new NullableDateTimeInFutureValidator())
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
                .ValidateWith(new NullableDateInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateOnlyModel { Prop = null }
                .ValidateWith(new NullableDateOnlyInFutureValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateTimeModel { Prop = null }
                .ValidateWith(new NullableDateTimeInFutureValidator())
                .Should().BeValid();
        }
    }
}

public class Invalid_for_not_in_future
{
    [Test]
    [Arguments("'Prop' moet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the future.", "en-GB")]
    public void Date(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateModel { Prop = new Date(2017, 06, 11) }
                    .ValidateWith(new DateInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' moet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the future.", "en-GB")]
    public void DateOnly(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateOnlyModel { Prop = new DateOnly(2017, 06, 11) }
                    .ValidateWith(new DateOnlyInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' moet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the future.", "en-GB")]
    public void DateTime(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateTimeModel { Prop = new DateTime(2017, 06, 11, 00, 00, 00, DateTimeKind.Utc) }
                    .ValidateWith(new DateTimeInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' moet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the future.", "en-GB")]
    public void Nullable_Date(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateModel { Prop = new Date(2017, 06, 11) }
                    .ValidateWith(new NullableDateInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' moet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the future.", "en-GB")]
    public void Nullable_DateOnly(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateOnlyModel { Prop = new DateOnly(2017, 06, 11) }
                    .ValidateWith(new NullableDateOnlyInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' moet in de toekomst liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the future.", "en-GB")]
    public void Nullable_DateTime(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateTimeModel { Prop = new DateTime(2017, 06, 11, 00, 00, 00, DateTimeKind.Utc) }
                    .ValidateWith(new NullableDateTimeInFutureValidator()).Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }
}

public class DateInFutureValidator : ModelValidator<DateModel>
{
    public DateInFutureValidator() => RuleFor(m => m.Prop).InFuture();
}

public class DateOnlyInFutureValidator : ModelValidator<DateOnlyModel>
{
    public DateOnlyInFutureValidator() => RuleFor(m => m.Prop).InFuture();
}

public class DateTimeInFutureValidator : ModelValidator<DateTimeModel>
{
    public DateTimeInFutureValidator() => RuleFor(m => m.Prop).InFuture();
}

public class NullableDateOnlyInFutureValidator : ModelValidator<NullableDateOnlyModel>
{
    public NullableDateOnlyInFutureValidator() => RuleFor(m => m.Prop).InFuture();
}

public class NullableDateInFutureValidator : ModelValidator<NullableDateModel>
{
    public NullableDateInFutureValidator() => RuleFor(m => m.Prop).InFuture();
}

public class NullableDateTimeInFutureValidator : ModelValidator<NullableDateTimeModel>
{
    public NullableDateTimeInFutureValidator() => RuleFor(m => m.Prop).InFuture();
}
