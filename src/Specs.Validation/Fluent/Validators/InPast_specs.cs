using FluentValidation;
using Qowaiv.Validation.Fluent;
using Specs.Fluent.Models;

namespace Fluent_validation.In_past_specs;

public class Valid_for_in_past
{
    [Test]
    public void Date()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateModel { Prop = new Date(2017, 06, 10) }
                .ValidateWith(new DateInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateOnlyModel { Prop = new DateOnly(2017, 06, 10) }
                .ValidateWith(new DateOnlyInPastValidator())
                .Should().BeValid();
        }
    }


    [Test]
    public void DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateTimeModel { Prop = new DateTime(2017, 06, 10, 00, 00, 00, DateTimeKind.Utc) }
                .ValidateWith(new DateTimeInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_Date()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateModel { Prop = new Date(2017, 06, 10) }
                .ValidateWith(new NullableDateInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateOnlyModel { Prop = new DateOnly(2017, 06, 10) }
                .ValidateWith(new NullableDateOnlyInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateTimeModel { Prop = new DateTime(2017, 06, 10, 00, 00, 00, DateTimeKind.Utc) }
                .ValidateWith(new NullableDateTimeInPastValidator())
                .Should().BeValid();
        }
    }
}

public class Valid_for_not_set
{
    [Test]
    public void Date()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateModel { Prop = null }
                .ValidateWith(new NullableDateInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateOnlyModel { Prop = null }
                .ValidateWith(new NullableDateOnlyInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateTimeModel { Prop = null }
                .ValidateWith(new NullableDateTimeInPastValidator())
                .Should().BeValid();
        }
    }
}

public class Invalid_for_not_in_past
{
    [Test]
    [Arguments("'Prop' moet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the past.", "en-GB")]
    public void Date(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateModel { Prop = new Date(2017, 06, 12) }
                    .ValidateWith(new DateInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' moet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the past.", "en-GB")]
    public void DateOnly(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateOnlyModel { Prop = new DateOnly(2017, 06, 12) }
                    .ValidateWith(new DateOnlyInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' moet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the past.", "en-GB")]
    public void DateTime(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateTimeModel { Prop = new DateTime(2017, 06, 12, 00, 00, 00, DateTimeKind.Utc) }
                    .ValidateWith(new DateTimeInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' moet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the past.", "en-GB")]
    public void Nullable_Date(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateModel { Prop = new Date(2017, 06, 12) }
                    .ValidateWith(new NullableDateInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' moet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the past.", "en-GB")]
    public void Nullable_DateOnly(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateOnlyModel { Prop = new DateOnly(2017, 06, 12) }
                    .ValidateWith(new NullableDateOnlyInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' moet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should be in the past.", "en-GB")]
    public void Nullable_DateTime(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateTimeModel { Prop = new DateTime(2017, 06, 12, 00, 00, 00, DateTimeKind.Utc) }
                    .ValidateWith(new NullableDateTimeInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }
}

public class DateInPastValidator : ModelValidator<DateModel>
{
    public DateInPastValidator() => RuleFor(m => m.Prop).InPast();
}

public class DateOnlyInPastValidator : ModelValidator<DateOnlyModel>
{
    public DateOnlyInPastValidator() => RuleFor(m => m.Prop).InPast();
}

public class DateTimeInPastValidator : ModelValidator<DateTimeModel>
{
    public DateTimeInPastValidator() => RuleFor(m => m.Prop).InPast();
}

public class NullableDateInPastValidator : ModelValidator<NullableDateModel>
{
    public NullableDateInPastValidator() => RuleFor(m => m.Prop).InPast();
}

public class NullableDateOnlyInPastValidator : ModelValidator<NullableDateOnlyModel>
{
    public NullableDateOnlyInPastValidator() => RuleFor(m => m.Prop).InPast();
}

public class NullableDateTimeInPastValidator : ModelValidator<NullableDateTimeModel>
{
    public NullableDateTimeInPastValidator() => RuleFor(m => m.Prop).InPast();
}
