#pragma warning disable QW0010 // Use System.DateOnly instead of Qowaiv.Date: We support netstandard 2.0.

namespace FluentValidation;

/// <summary>Fluent validation for <see cref="Clock"/>.</summary>
public static class ClockValidation
{
    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, DateTime> ruleBuilder)
    {
        /// <summary>Requires a date time to be in the future.</summary>
        /// <param name="utcNow">
        /// An optional function providing now, or if not specified, <see cref="Clock.UtcNow()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateTime> InFuture(Func<DateTime>? utcNow = null)
        {
            Guard.NotNull(ruleBuilder);

            utcNow ??= Clock.UtcNow;

            return ruleBuilder
                .Must(date => date > utcNow())
                .WithMessage(_ => QowaivValidationFluentMessages.InFuture);
        }

        /// <summary>Requires a date time not to be in the future.</summary>
        /// <param name="utcNow">
        /// An optional function providing now, or if not specified, <see cref="Clock.UtcNow()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateTime> NotInFuture(Func<DateTime>? utcNow = null)
        {
            Guard.NotNull(ruleBuilder);

            utcNow ??= Clock.UtcNow;

            return ruleBuilder
                .Must(date => date <= utcNow())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInFuture);
        }

        /// <summary>Requires a date time to be in the past.</summary>
        /// <param name="utcNow">
        /// An optional function providing now, or if not specified, <see cref="Clock.UtcNow()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateTime> InPast(Func<DateTime>? utcNow = null)
        {
            Guard.NotNull(ruleBuilder);

            utcNow ??= Clock.UtcNow;

            return ruleBuilder
                .Must(date => date < utcNow())
                .WithMessage(_ => QowaivValidationFluentMessages.InPast);
        }

        /// <summary>Requires a date time not to be in the past.</summary>
        /// <param name="utcNow">
        /// An optional function providing now, or if not specified, <see cref="Clock.UtcNow()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateTime> NotInPast(Func<DateTime>? utcNow = null)
        {
            Guard.NotNull(ruleBuilder);

            utcNow ??= Clock.UtcNow;

            return ruleBuilder
                .Must(date => date >= utcNow())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInPast);
        }
    }

    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, DateTime?> ruleBuilder)
    {
        /// <summary>Requires a date time to be in the future (if set).</summary>
        /// <param name="utcNow">
        /// An optional function providing now, or if not specified, <see cref="Clock.UtcNow()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateTime?> InFuture(Func<DateTime>? utcNow = null)
        {
            Guard.NotNull(ruleBuilder);

            utcNow ??= Clock.UtcNow;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value > utcNow())
                .WithMessage(_ => QowaivValidationFluentMessages.InFuture);
        }

        /// <summary>Requires a date time not to be in the future (if set).</summary>
        /// <param name="utcNow">
        /// An optional function providing now, or if not specified, <see cref="Clock.UtcNow()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateTime?> NotInFuture(Func<DateTime>? utcNow = null)
        {
            Guard.NotNull(ruleBuilder);

            utcNow ??= Clock.UtcNow;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value <= utcNow())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInFuture);
        }

        /// <summary>Requires a date time to be in the past (if set).</summary>
        /// <param name="utcNow">
        /// An optional function providing now, or if not specified, <see cref="Clock.UtcNow()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateTime?> InPast(Func<DateTime>? utcNow = null)
        {
            Guard.NotNull(ruleBuilder);

            utcNow ??= Clock.UtcNow;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value < utcNow())
                .WithMessage(_ => QowaivValidationFluentMessages.InPast);
        }

        /// <summary>Requires a date time not to be in the past (if set).</summary>
        /// <param name="utcNow">
        /// An optional function providing now, or if not specified, <see cref="Clock.UtcNow()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateTime?> NotInPast(Func<DateTime>? utcNow = null)
        {
            Guard.NotNull(ruleBuilder);

            utcNow ??= Clock.UtcNow;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value >= utcNow())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInPast);
        }
    }

    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, Date> ruleBuilder)
    {
        /// <summary>Requires a date to be in the future.</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, Date> InFuture(Func<Date>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= Clock.Today;

            return ruleBuilder
                .Must(date => date > today())
                .WithMessage(_ => QowaivValidationFluentMessages.InFuture);
        }

        /// <summary>Requires a date not to be in the future.</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, Date> NotInFuture(Func<Date>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= Clock.Today;

            return ruleBuilder
                .Must(date => date <= today())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInFuture);
        }

        /// <summary>Requires a date to be in the past.</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, Date> InPast(Func<Date>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= Clock.Today;

            return ruleBuilder
                .Must(date => date < today())
                .WithMessage(_ => QowaivValidationFluentMessages.InPast);
        }

        /// <summary>Requires a date not to be in the past.</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, Date> NotInPast(Func<Date>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= Clock.Today;

            return ruleBuilder
                .Must(date => date >= today())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInPast);
        }
    }

    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, Date?> ruleBuilder)
    {
        /// <summary>Requires a date to be in the future (if set).</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, Date?> InFuture(Func<Date>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= Clock.Today;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value > today())
                .WithMessage(_ => QowaivValidationFluentMessages.InFuture);
        }

        /// <summary>Requires a date not to be in the future (if set).</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, Date?> NotInFuture(Func<Date>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= Clock.Today;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value <= today())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInFuture);
        }

        /// <summary>Requires a date to be in the past (if set).</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, Date?> InPast(Func<Date>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= Clock.Today;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value < today())
                .WithMessage(_ => QowaivValidationFluentMessages.InPast);
        }

        /// <summary>Requires a date not to be in the past (if set).</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, Date?> NotInPast(Func<Date>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= Clock.Today;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value >= today())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInPast);
        }
    }

    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, DateOnly> ruleBuilder)
    {
        /// <summary>Requires a date to be in the future.</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateOnly> InFuture(Func<DateOnly>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= DateOnlyToday;

            return ruleBuilder
                .Must(date => date > today())
                .WithMessage(_ => QowaivValidationFluentMessages.InFuture);
        }

        /// <summary>Requires a date not to be in the future.</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateOnly> NotInFuture(Func<DateOnly>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= DateOnlyToday;

            return ruleBuilder
                .Must(date => date <= today())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInFuture);
        }

        /// <summary>Requires a date to be in the past.</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateOnly> InPast(Func<DateOnly>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= DateOnlyToday;

            return ruleBuilder
                .Must(date => date < today())
                .WithMessage(_ => QowaivValidationFluentMessages.InPast);
        }

        /// <summary>Requires a date not to be in the past.</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateOnly> NotInPast(Func<DateOnly>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= DateOnlyToday;

            return ruleBuilder
                .Must(date => date >= today())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInPast);
        }
    }

    /// <typeparam name="TModel">
    /// Type of the model being validated.
    /// </typeparam>
    extension<TModel>(IRuleBuilder<TModel, DateOnly?> ruleBuilder)
    {
        /// <summary>Requires a date to be in the future (if set).</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateOnly?> InFuture(Func<DateOnly>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= DateOnlyToday;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value > today())
                .WithMessage(_ => QowaivValidationFluentMessages.InFuture);
        }

        /// <summary>Requires a date not to be in the future (if set).</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateOnly?> NotInFuture(Func<DateOnly>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= DateOnlyToday;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value <= today())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInFuture);
        }

        /// <summary>Requires a date to be in the past (if set).</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateOnly?> InPast(Func<DateOnly>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= DateOnlyToday;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value < today())
                .WithMessage(_ => QowaivValidationFluentMessages.InPast);
        }

        /// <summary>Requires a date not to be in the past (if set).</summary>
        /// <param name="today">
        /// An optional function providing today, or if not specified, <see cref="Clock.Today()"/>.
        /// </param>
        [FluentSyntax]
        public IRuleBuilderOptions<TModel, DateOnly?> NotInPast(Func<DateOnly>? today = null)
        {
            Guard.NotNull(ruleBuilder);

            today ??= DateOnlyToday;

            return ruleBuilder
                .Must(date => !date.HasValue || date.Value >= today())
                .WithMessage(_ => QowaivValidationFluentMessages.NotInPast);
        }
    }

    [Pure]
    private static DateOnly DateOnlyToday() => Clock.Today();
}
