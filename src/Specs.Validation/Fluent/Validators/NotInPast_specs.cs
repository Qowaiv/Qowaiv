using FluentValidation;
using Qowaiv.Validation.Fluent;
using Specs.Fluent.Models;

namespace Validators.NotInPast_specs;

public class Valid_for_not_in_past
{
    [Test]
    public void Date()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateModel { Prop = new Date(2017, 06, 11) }
                .ValidateWith(new DateNotInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new DateOnlyModel { Prop = new DateOnly(2017, 06, 11) }
                .ValidateWith(new DateOnlyNotInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new DateTime(2017, 06, 11, 00, 00, 00, DateTimeKind.Utc)))
        {
            new DateTimeModel { Prop = new DateTime(2017, 06, 12, 00, 00, 00, DateTimeKind.Utc) }
                .ValidateWith(new DateTimeNotInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_Date()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateModel { Prop = new Date(2017, 06, 11) }
                .ValidateWith(new NullableDateNotInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateOnlyModel { Prop = new DateOnly(2017, 06, 11) }
                .ValidateWith(new NullableDateOnlyNotInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void Nullable_DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new DateTime(2017, 06, 11, 00, 00, 00, DateTimeKind.Utc)))
        {
            new NullableDateTimeModel { Prop = new DateTime(2017, 06, 12, 00, 00, 00, DateTimeKind.Utc) }
                .ValidateWith(new NullableDateTimeNotInPastValidator())
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
                .ValidateWith(new NullableDateNotInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateOnly()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateOnlyModel { Prop = null }
                .ValidateWith(new NullableDateOnlyNotInPastValidator())
                .Should().BeValid();
        }
    }

    [Test]
    public void DateTime()
    {
        using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
        {
            new NullableDateTimeModel { Prop = null }
                .ValidateWith(new NullableDateTimeNotInPastValidator())
                .Should().BeValid();
        }
    }
}
public class Invalid_for_not_past
{
    [Test]
    [Arguments("'Prop' mag niet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the past.", "en-GB")]
    public void Date(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateModel { Prop = new Date(2017, 06, 10) }
                    .ValidateWith(new DateNotInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' mag niet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the past.", "en-GB")]
    public void DateOnly(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateOnlyModel { Prop = new DateOnly(2017, 06, 10) }
                    .ValidateWith(new DateOnlyNotInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' mag niet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the past.", "en-GB")]
    public void DateTime(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new DateTimeModel { Prop = new DateTime(2017, 06, 10, 00, 00, 00, DateTimeKind.Utc) }
                    .ValidateWith(new DateTimeNotInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' mag niet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the past.", "en-GB")]
    public void Nullable_Date(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateModel { Prop = new Date(2017, 06, 10) }
                    .ValidateWith(new NullableDateNotInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Test]
    [Arguments("'Prop' mag niet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the past.", "en-GB")]
    public void Nullable_DateOnly(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateOnlyModel { Prop = new DateOnly(2017, 06, 10) }
                    .ValidateWith(new NullableDateOnlyNotInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }

    [Arguments("'Prop' mag niet in het verleden liggen.", "nl-NL")]
    [Arguments("'Prop' should not be in the past.", "en-GB")]
    public void Nullable_DateTime(string message, CultureInfo culture)
    {
        using (culture.Scoped())
        {
            using (Clock.SetTimeForCurrentContext(() => new Date(2017, 06, 11)))
            {
                new NullableDateTimeModel { Prop = new DateTime(2017, 06, 10, 00, 00, 00, DateTimeKind.Utc) }
                    .ValidateWith(new NullableDateTimeNotInPastValidator())
                    .Should().BeInvalid()
                    .WithMessage(ValidationMessage.Error(message, "Prop"));
            }
        }
    }
}

public class DateNotInPastValidator : ModelValidator<DateModel>
{
    public DateNotInPastValidator() => RuleFor(m => m.Prop).NotInPast();
}

public class DateOnlyNotInPastValidator : ModelValidator<DateOnlyModel>
{
    public DateOnlyNotInPastValidator() => RuleFor(m => m.Prop).NotInPast();
}

public class DateTimeNotInPastValidator : ModelValidator<DateTimeModel>
{
    public DateTimeNotInPastValidator() => RuleFor(m => m.Prop).NotInPast();
}

public class NullableDateNotInPastValidator : ModelValidator<NullableDateModel>
{
    public NullableDateNotInPastValidator() => RuleFor(m => m.Prop).NotInPast();
}

public class NullableDateOnlyNotInPastValidator : ModelValidator<NullableDateOnlyModel>
{
    public NullableDateOnlyNotInPastValidator() => RuleFor(m => m.Prop).NotInPast();
}

public class NullableDateTimeNotInPastValidator : ModelValidator<NullableDateTimeModel>
{
    public NullableDateTimeNotInPastValidator() => RuleFor(m => m.Prop).NotInPast();
}
