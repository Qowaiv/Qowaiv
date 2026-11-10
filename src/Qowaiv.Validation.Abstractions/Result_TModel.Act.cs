namespace Qowaiv.Validation.Abstractions;

public sealed partial class Result<TModel> : Result
{
    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <typeparam name="TOut">
    /// The type of the new result value.
    /// </typeparam>
    /// <returns>
    /// A result with the transformed value.
    /// </returns>
    /// <exception cref="NoValue">
    /// When the action returns <see langword="null"/> for a valid result.
    /// </exception>
    [Impure]
    public Result<TOut> Act<TOut>(Func<TModel, TOut> action)
    {
        Guard.NotNull(action);

        return IsValid
            ? NotNull(action(Value), Messages)
            : WithMessages<TOut>(Messages);
    }

    /// <inheritdoc cref="Act{TOut}(Func{TModel, TOut})"/>
    [Impure]
    public Result<TOut> Act<TOut>(Func<TModel, Result<TOut>> action)
    {
        Guard.NotNull(action);

        if (IsValid)
        {
            var outcome = action(Value);
            var value = outcome.IsValid ? outcome.Value : default;
            return new(value, Messages.Append(outcome.Messages));
        }
        else return WithMessages<TOut>(Messages);
    }

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <returns>
    /// A result with the merged messages.
    /// </returns>
    [Impure]
    public Result<TModel> Act(Func<TModel, Result> action)
    {
        Guard.NotNull(action);

        if (IsValid)
        {
            var outcome = action(Value);
            return new(_value, Messages.Append(outcome.Messages));
        }
        else return WithMessages<TModel>(Messages);
    }

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <param name="update">
    /// The update to apply on a successfully invoked action.
    /// </param>
    /// <typeparam name="TOut">
    /// The type of the new result value.
    /// </typeparam>
    /// <returns>
    /// The updated model with the merged messages.
    /// </returns>
    [Impure]
    public Result<TModel> Act<TOut>(Func<TModel, Result<TOut>> action, Action<TModel, TOut> update)
    {
        Guard.NotNull(update);

        return Act(action, (model, result) =>
        {
            if (model is { }) { update.Invoke(model, result); }
            return model;
        });
    }

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <param name="update">
    /// The update to apply on a successfully invoked action.
    /// </param>
    /// <typeparam name="TOut">
    /// The type of the new result value.
    /// </typeparam>
    /// <returns>
    /// The updated model with the merged messages.
    /// </returns>
    /// <exception cref="NoValue">
    /// When the update returns <see langword="null"/> for a valid result.
    /// </exception>
    [Impure]
    public Result<TModel> Act<TOut>(Func<TModel, Result<TOut>> action, Func<TModel, TOut, TModel> update)
    {
        Guard.NotNull(update);

        var resolved = Act(action);
        return resolved.IsValid
            ? NotNull(update(_value!, resolved.Value), resolved.Messages)
            : new(_value, resolved.Messages);
    }

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <param name="continueOnCapturedContext">
    /// true to attempt to marshal the continuation back to the original context captured;
    /// otherwise, false. See <see cref="Task.ConfigureAwait(bool)"/>.
    /// </param>
    /// <typeparam name="TOut">
    /// The type of the new result value.
    /// </typeparam>
    /// <returns>
    /// A result with the merged messages.
    /// </returns>
    [Impure]
    public Task<Result<TOut>> ActAsync<TOut>(
        Func<TModel, Task<Result<TOut>>> action,
        bool continueOnCapturedContext = false)
    {
        _ = Guard.NotNull(action);

        async Task<Result<TOut>> Execute()
        {
            if (IsValid)
            {
                var outcome = await action(Value).ConfigureAwait(continueOnCapturedContext);
                var value = outcome.IsValid ? outcome.Value : default;
                return new(value, Messages.Append(outcome.Messages));
            }
            else return WithMessages<TOut>(Messages);
        }
        return Execute();
    }

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <param name="continueOnCapturedContext">
    /// true to attempt to marshal the continuation back to the original context captured;
    /// otherwise, false. See <see cref="Task.ConfigureAwait(bool)"/>.
    /// </param>
    /// <returns>
    /// A result with the merged messages.
    /// </returns>
    [Impure]
    public Task<Result<TModel>> ActAsync(
        Func<TModel, AsyncResult> action,
        bool continueOnCapturedContext = false)
    {
        _ = Guard.NotNull(action);

        async Task<Result<TModel>> Execute()
        {
            if (IsValid)
            {
                var outcome = await action(Value).ConfigureAwait(continueOnCapturedContext);
                return new(_value, Messages.Append(outcome.Messages));
            }
            else return WithMessages<TModel>(Messages);
        }
        return Execute();
    }

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <param name="update">
    /// The update to apply on a successfully invoked action.
    /// </param>
    /// <param name="continueOnCapturedContext">
    /// true to attempt to marshal the continuation back to the original context captured;
    /// otherwise, false. See <see cref="Task.ConfigureAwait(bool)"/>.
    /// </param>
    /// <typeparam name="TOut">
    /// The type of the new result value.
    /// </typeparam>
    /// <returns>
    /// The updated model with the merged messages.
    /// </returns>
    [Impure]
    public Task<Result<TModel>> ActAsync<TOut>(
        Func<TModel, Task<Result<TOut>>> action,
        Action<TModel, TOut> update,
        bool continueOnCapturedContext = false)
    {
        Guard.NotNull(update);

        return ActAsync(
            action,
            (model, result) =>
            {
                if (model is { }) { update.Invoke(model, result); }
                return model;
            },
            continueOnCapturedContext);
    }

    /// <summary>Invokes the action when <see cref="Result{TModel}"/> is valid.</summary>
    /// <param name="action">
    /// The action to invoke.
    /// </param>
    /// <param name="update">
    /// The update to apply on a successfully invoked action.
    /// </param>
    /// <param name="continueOnCapturedContext">
    /// true to attempt to marshal the continuation back to the original context captured;
    /// otherwise, false. See <see cref="Task.ConfigureAwait(bool)"/>.
    /// </param>
    /// <typeparam name="TOut">
    /// The type of the new result value.
    /// </typeparam>
    /// <returns>
    /// The updated model with the merged messages.
    /// </returns>
    /// <exception cref="NoValue">
    /// When the update returns <see langword="null"/> for a valid result.
    /// </exception>
    [Impure]
    public Task<Result<TModel>> ActAsync<TOut>(
        Func<TModel, Task<Result<TOut>>> action,
        Func<TModel, TOut, TModel> update,
        bool continueOnCapturedContext = false)
    {
        Guard.NotNull(update);

        async Task<Result<TModel>> Execute()
        {
            var resolved = await ActAsync(action, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext);
            return resolved.IsValid
                ? NotNull(update(_value!, resolved.Value), resolved.Messages)
                : new(_value, resolved.Messages);
        }
        return Execute();
    }

    /// <summary>Throws <see cref="NoValue"/> exception when valid with null value.</summary>
    [Pure]
    internal Result<TModel> NotNull()
       => IsValid && Value is null ? throw NoValue.For<TModel>() : this;

    [Pure]
    private static Result<TOut> NotNull<TOut>(TOut? value, ValidationMessages messages)
    {
        var result = new Result<TOut>(value, messages);
        return result.IsValid && value is null ? throw NoValue.For<TOut>() : result;
    }
}
