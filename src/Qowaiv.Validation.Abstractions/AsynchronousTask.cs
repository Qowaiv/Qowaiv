namespace Qowaiv.Validation.Abstractions;

/// <summary>Extensions on <see cref="Task"/>.</summary>
public static class AsynchronousTask
{
    extension<TModel>(Task<Result<TModel>> promise)
    {
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
        /// A result with the transformed value.
        /// </returns>
        /// <exception cref="NoValue">
        /// When the action returns <see langword="null"/> for a valid result.
        /// </exception>
        [Impure]
        public Task<Result<TOut>> ActAsync<TOut>(
            Func<TModel, TOut> action,
            bool continueOnCapturedContext = false)
        {
            Guard.NotNull(action);

            async Task<Result<TOut>> Execute()
                => await promise.ConfigureAwait(continueOnCapturedContext) is { } result
                ? result.Act(action)
                : Result.WithMessages<TOut>();

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
        /// <typeparam name="TOut">
        /// The type of the new result value.
        /// </typeparam>
        /// <returns>
        /// A result with the merged messages.
        /// </returns>
        [Impure]
        public Task<Result<TOut>> ActAsync<TOut>(
            Func<TModel, Result<TOut>> action,
            bool continueOnCapturedContext = false)
        {
            Guard.NotNull(action);

            async Task<Result<TOut>> Execute()
                => await promise.ConfigureAwait(continueOnCapturedContext) is { } result
                ? result.Act(action)
                : Result.WithMessages<TOut>();

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
            Guard.NotNull(action);

            async Task<Result<TOut>> Execute()
                => await promise.ConfigureAwait(continueOnCapturedContext) is { } result
                ? await result.ActAsync(action, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext)
                : Result.Null<TOut>();

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
            Func<TModel, Result> action,
            bool continueOnCapturedContext = false)
        {
            Guard.NotNull(action);

            async Task<Result<TModel>> Execute()
                => await promise.ConfigureAwait(continueOnCapturedContext) is { } result
                ? result.Act(action)
                : Result.Null<TModel>();

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
            Guard.NotNull(action);

            async Task<Result<TModel>> Execute()
                => await promise.ConfigureAwait(continueOnCapturedContext) is { } result
                ? await result.ActAsync(action, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext)
                : Result.Null<TModel>();

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
            Func<TModel, Result<TOut>> action,
            Action<TModel, TOut> update,
            bool continueOnCapturedContext = false)
        {
            Guard.NotNull(action);
            Guard.NotNull(update);

            async Task<Result<TModel>> Execute()
                => await promise.ConfigureAwait(continueOnCapturedContext) is { } result
                 ? result.Act(action, update)
                 : Result.WithMessages<TModel>();

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
            Guard.NotNull(action);
            Guard.NotNull(update);

            async Task<Result<TModel>> Execute()
                => await promise.ConfigureAwait(continueOnCapturedContext) is { } result
                 ? await result.ActAsync(action, update, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext)
                : Result.WithMessages<TModel>();

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
            Func<TModel, Result<TOut>> action,
            Func<TModel, TOut, TModel> update,
            bool continueOnCapturedContext = false)
        {
            Guard.NotNull(action);
            Guard.NotNull(update);

            async Task<Result<TModel>> Execute()
                => await promise.ConfigureAwait(continueOnCapturedContext) is { } result
                ? result.Act(action, update)
                : Result.WithMessages<TModel>();

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
            Func<TModel, TOut, TModel> update,
            bool continueOnCapturedContext = false)
        {
            Guard.NotNull(action);
            Guard.NotNull(update);

            async Task<Result<TModel>> Execute()
                => await promise.ConfigureAwait(continueOnCapturedContext) is { } result
                ? await result.ActAsync(action, update, continueOnCapturedContext).ConfigureAwait(continueOnCapturedContext)
                : Result.WithMessages<TModel>();

            return Execute();
        }
    }
}
